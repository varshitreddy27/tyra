USE [bookdb]
GO

/****** Object:  StoredProcedure [dbo].[SALES3_HTMLSubInfo]    Script Date: 05/13/2011 10:27:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



ALTER PROCEDURE [dbo].[SALES3_HTMLSubInfo]    
@userid uniqueidentifier = null,  -- the person making the request    
@subscriptionid varchar(38) = null  -- subscription id    
--@login varchar(20) = null      -- or login of someone in sub    
AS    
set nocount on    
    
declare @subid uniqueidentifier    
declare @trialrequestid uniqueidentifier    
    
-- resolve subscription id is missing if login was given instead    
/*    
if @subscriptionid is null or len(@subscriptionid)=0    
 select @subid = subscriptionid from libuser with(nolock) where login=@login    
else    
*/    
set @subid = @subscriptionid   
 select @trialrequestid = trialrequestid from trialrequest with(nolock) where subscriptionid=@subid    
    
    
/*--------------------------------    
  identify all the salesperps    
---------------------------------*/    
    
create table #p (userid uniqueidentifier, login varchar(96), name varchar(161))    
create table #s (subid uniqueidentifier)    
    
declare @GeneralAdmin int, @SalesManager int, @IsSupport int    
select @GeneralAdmin=count(*) from permissions with(nolock) where userid=@userid and GeneralAdmin=1    
select @SalesManager=count(*) from permissions with(nolock) where userid=@userid and SalesManager=1    
set @IsSupport = dbo.B24IsInSalesGroup('FREDSUP', @userid)    
    
--- the sales level colleagues ---    
if @GeneralAdmin>0 or @IsSupport>0    
  insert into #p    
  select distinct l.userid, l.login, isnull(p.firstname+' '+p.lastname,'')    
  from libuser l with(nolock), permissions pm with(nolock), person p with(nolock)    
  where l.userid in  (select sgm.userid from salesgroupmember sgm with(nolock)    
    union select userid from libuser with(nolock) where login like '%unassigned%')    
    and p.personid=l.personid    
    and pm.userid=l.userid    
--- for sales manager ---    
else if @SalesManager>0    
  insert into #p    
  select distinct l.userid, l.login, isnull(p.firstname+' '+p.lastname,'')    
  from libuser l with(nolock), permissions pm with(nolock), person p with(nolock)    
  where l.userid in (select sgm.userid from salesgroupmember sgm with(nolock), salesgroupmember sgm2 with(nolock), salesgroup sg with(nolock)    
                     where sg.salesgroupid=sgm.salesgroupid and sg.salesgroupid=sgm2.salesgroupid and sg.type>0    
                     union select userid from libuser with(nolock) where login = 'unassignedsales')    
    and p.personid=l.personid    
    and pm.userid=l.userid and (pm.generaladmin=0 and pm.superuser=0)    
--- for regular sales ---    
else    
  insert into #p    
  select distinct l.userid, l.login, isnull(p.firstname+' '+p.lastname,'')    
  from libuser l with(nolock), permissions pm with(nolock), person p with(nolock)    
  where l.userid in (select sgm.userid from salesgroupmember sgm with(nolock), salesgroupmember sgm2 with(nolock), salesgroup sg with(nolock)    
         where sg.salesgroupid=sgm.salesgroupid and sg.salesgroupid=sgm2.salesgroupid and sgm2.userid = @userid and sg.type>0)    
    and p.personid=l.personid    
    and pm.userid=l.userid and (pm.reseller=1 or (pm.salesmanager=0 and pm.generaladmin=0 and pm.superuser=0))    
--- add subscriptions from the above ---    
insert into #s    
select subscriptionid from subscription s with(nolock)    
where s.subscriptionid=@subid and s.salespersonid in(select userid from #p)    
    
--- add any reseller branded subs    
insert into #s    
select subscriptionid from subscription s with(nolock)    
where s.subscriptionid=@subid and s.resellercode in    
(select resellercode from salesgroup sg with(nolock), salesgroupmember sgm with(nolock) where sg.salesgroupid = sgm.salesgroupid and sgm.userid = @userid)    
    
-- remove access to those who don't shouldn't be here    
declare @access int    
set @access = 0   -- deny by default    
select @access = count(*) from subscription s with(nolock)    
where s.subscriptionid in(select subid from #s)    
    
drop table #p    
drop table #s    
   
if @access=0    
begin    
 select 'Access Denied'=''    
 return    
end    
  
    
declare @usedseats integer    
select @usedseats=count(*) from libuser l with(nolock)    
where l.subscriptionid=@subid    
and l.subscriptionstatusid not in (8,9,11,12,13,14,15) -- exclude SoftDeleted, PaidDenied and Library types    
    
declare @appname varchar(100)    
declare @allowpdf int    
declare @seats int    
declare @subtype int   
declare @pwdroot varchar(10)  
select @appname=s.applicationname,@allowpdf=allowpdf,@seats=seats,@subtype=type, @pwdroot= PasswordRoot from subscription s with(nolock) where s.subscriptionid=@subid    
    
    
--- chapters to go uplift    
declare @deniedUplift varchar(256)  -- is it available? -- null means yes, otherwise there's a message    
set @deniedUplift = dbo.B24_ChapterDownloadUpliftAllowed(@appname,@allowpdf,@subtype,@seats)    
    
declare @upliftQuota integer -- is it enabled?    
set @upliftQuota = 0    
select @upliftQuota=quota from entitlement with (nolock) where ownerid=@subscriptionid and type=2 and status =1    
    
/*---------------------------------------    
Display a title    
---------------------------------------*/    
select 'Account Information'    
    
if @deniedUplift is not null  or @upliftQuota = 0 -- don't display chapter uplift     
begin    
select  'B24 SubID'= s.passwordroot,    
  'Company/Dept'=left(c.name+' / '+s.companyordepartment,32),    
 'Application'=s.ApplicationName,    
 'Group Code' = isnull(s.groupcode,''),    
 'Type'=left(st.description,10),    
 'Status'=left(ss.description,10),    
-- 'Corporate License' = case when s.corpsubscription=s.subscriptionid then 'yes' else 'no' end,    
 'Starts'=convert(char(15),s.starts,107),    
 'Expires'=convert(char(15),s.expires,107),    
 'Seats' = cast(s.seats as varchar) ,    
 'Registered Users' =  cast(@usedseats as varchar) ,    
--'Salesgroup' = isnull(s.resellercode, ''),   
 'Salesgroup' = isnull(sg.name, ''),    
 'Salesperson'= ps.FirstName+' '+ps.LastName+' ('+ps.email+')',    
 'Contract#' = s.accountnumber,  
 --'@personid'=pc.personid,    
 --'@personid'=pb.personid,    
 'Notes'=s.Notes,    
 '@subscriptionid'=s.subscriptionid    
from subscription s with(nolock)    
    LEFT OUTER Join Libuser us with(nolock) ON s.salespersonid=us.userid    
    LEFT OUTER JOIN Person ps  with(nolock)ON ps.personid=us.personid    
    LEFT OUTER JOIN Person pc  with(nolock)ON s.contactid=pc.personid    
    LEFT OUTER JOIN Person pb  with(nolock)ON s.billtoid=pb.personid    
    join libuser sl with(nolock) on sl.userid=s.salespersonid    
    join company c with(nolock) on s.companyid=c.companyid    
    join domaintable st with(nolock) on s.type=st.number and st.tablename='Subscription' and st.columnname='Type'    
    join person sp with(nolock) on sl.personid=sp.personid    
    join domaintable ss with(nolock) on s.status=ss.number and ss.tablename='Subscription' and ss.columnname='Status'    
    join salesgroup sg on sg.resellercode = s.resellercode  
where s.subscriptionid=@subid    
end    
else     
begin    
select  'B24 SubID'= s.passwordroot,    
 'Company/Dept'=left(c.name+' / '+s.companyordepartment,32),    
 'Application'=s.ApplicationName,    
 'Group Code' = isnull(s.groupcode,''),    
 'Type'=left(st.description,10),    
 'Status'=left(ss.description,10),    
-- 'Corporate License' = case when s.corpsubscription=s.subscriptionid then 'yes' else 'no' end,    
 'Starts'=convert(char(15),s.starts,107),    
 'Expires'=convert(char(15),s.expires,107),    
 'Seats' = cast(s.seats as varchar) ,    
 'Registered Users' =  cast(@usedseats as varchar) ,    
    'Chapters to Go' =    
           case     
              when @upliftQuota > 0 then 'Enabled (' + cast(@upliftQuota as varchar) + ' per 90 days)'    
              when @deniedUplift is not null then 'N/A'    
              else 'Disabled'    
           end,    
 --'Salesgroup' = isnull(s.resellercode, ''),   
 'Salesgroup' = isnull(sg.name, ''),    
 'Salesperson'= ps.FirstName+' '+ps.LastName+' ('+ps.email+')',   
  'Contract#' = s.accountnumber,  
 --'@personid'=pc.personid,     --'@personid'=pb.personid,    
 'Notes'=s.Notes,    
 '@subscriptionid'=s.subscriptionid   
  
from subscription s with(nolock)    
    LEFT OUTER Join Libuser us with(nolock) ON s.salespersonid=us.userid    
    LEFT OUTER JOIN Person ps  with(nolock)ON ps.personid=us.personid    
    LEFT OUTER JOIN Person pc  with(nolock)ON s.contactid=pc.personid    
    LEFT OUTER JOIN Person pb  with(nolock)ON s.billtoid=pb.personid    
    join libuser sl with(nolock) on sl.userid=s.salespersonid    
    join company c with(nolock) on s.companyid=c.companyid    
    join domaintable st with(nolock) on s.type=st.number and st.tablename='Subscription' and st.columnname='Type'    
    join person sp with(nolock) on sl.personid=sp.personid    
    join domaintable ss with(nolock) on s.status=ss.number and ss.tablename='Subscription' and ss.columnname='Status'    
    join salesgroup sg on sg.resellercode = s.resellercode  
where s.subscriptionid=@subid    
end    
    
    
/*---------------------------------------    
Registration Url    
---------------------------------------*/    
if(@appname <> 'library')    
begin    
 if exists (select * from subscription with(nolock) where corpsubscription = @subscriptionid)    
 begin    
   select 'Registration URL for Webmasters'=''    
   if(@appname='AdminBriefing')    
   begin    
  select HiddenColumn='HiddenColumn', URL=isnull(a.baseurl,'http://www.AdminBriefing.com')+'/Register.aspx?site='+s.passwordroot    
  from applications a with(nolock)    
  join subscription s with(nolock) on a.applicationname=s.applicationname    
  where s.subscriptionid=@subscriptionid    
   end    
   else    
   begin    
  select HiddenColumn='HiddenColumn',   URL=isnull(a.baseurl,'http://www.books24x7.com')+'gatekeeper.asp?site='+s.passwordroot    
  from applications a with(nolock)    
  join subscription s with(nolock) on a.applicationname=s.applicationname    
  where s.subscriptionid=@subscriptionid    
   end    
 end    
end    
    
/*---------------------------------------    
Registration Rules    
---------------------------------------*/    
if exists (select * from CorpSubscription with(nolock) where SubscriptionID=@subscriptionid)    
begin    
     select 'Registration Rules'=''    
  --- registration rules ---    
  select    
   'Rule' = case when r.refereripaddr is not null then replace(r.refereripaddr, '%', '*')    
       when r.refererurl is not null then replace(r.refererurl, '%', '*')    
     when r.emailpattern is not null then replace(r.emailpattern,'%', '*')    
     when r.disallowaddr is not null then replace(r.disallowaddr,'%', '*')    
       else '' end,    
   Type = case  when r.refereripaddr is not null then 'IP'    
          when r.refererurl is not null then 'URL'    
     when r.emailpattern is not null then 'Email'    
     when r.disallowaddr is not null then 'IP Disallowed'    
     else '' end    
  from corpsubscription r with(nolock)    
  where r.subscriptionid=@subscriptionid    
  order by Type desc, 'Rule'    
end    
    
/*---------------------------------------    
Library Authentication Rules    
---------------------------------------*/    
if exists (select * from AllowedIPAddr with(nolock) where SubscriptionID=@subscriptionid)    
   and (@GeneralAdmin>0 or @SalesManager>0 or @IsSupport>0)    
   and (@appname='library')    
begin    
  select 'Library Authentication Rules'=''    
  select    
   'Active' = case when isnull(a.Status,0)=0 then '__X__'    
     else '' end,    
   'Deny' = case when isnull(a.DisallowAddr,0)=1 then '__X__'    
     else '' end,    
   'Rule'= case when a.refererurl = 'ezproxy://' then 'EZPROXY Secure Tokens'    
        when a.IPAddrMask is not null then replace(a.IPAddrMask, '%', '*')    
       when a.refererurl is not null then replace(a.refererurl, '%', '*')    
       else '' end,    
   'No Anonymous User Reg Allowed' = case when isnull(a.AuthType,0)=0 then ''    
     else '______________X______________' end    
  from allowedipaddr a with(nolock)    
  where a.subscriptionid=@subscriptionid    
end    
    
/*    
select top 100 s.passwordroot, * from subscription s with(nolock)    
join  CorpSubscription cs with(nolock) on s.subscriptionid=cs.subscriptionid    
order by s.lastupdate desc    
*/    
    
    
/*---------------------------------------    
Report on Single Sign On, if set    
---------------------------------------*/    
    
if exists (select preauthenticated from subscription where subscriptionid = @subscriptionid and preauthenticated =1 )    
begin    
select ' Single Sign On'=''    
SELECT 
     'Preauth Method' = CASE S.PreauthID 
          When NULL Then ''
          ELSE PA.descr 
       END, 
       S.SharedSecret As 'Shared Secret', 
     S.LoginURL As 'Login URL', S.TimeoutURL 'Timeout URL' 
FROM
      Preauthmethod PA Right Join
            Subscription S
      ON S.PreauthId = PA.PreauthId and PreAuthenticated Is Not Null 
WHERE   
      S.subscriptionid =  @subscriptionid  
      end   
    
    
/*---------------------------------------    
Report on General Flags, if set    
---------------------------------------*/    
if exists(select s.generalflags from subscription s with(nolock) where s.subscriptionid=@subscriptionid and s.generalflags>0)    
 or exists(select d.comment from subscriptionflag sf with(nolock)right join domaintable d with(nolock) on sf.flagid = d.number    
    and subscriptionid = @subscriptionid where d.tablename ='subscriptionflag' and d.columnname ='flagid' and sf.status = 1)    
begin    
  
--create table #f (mask integer, description varchar(256), always int)    
--insert #f (mask, description, always) values (1, 'No New Book Notifications', 1)    
--insert #f (mask, description, always) values (2, 'No Welcome Email', 1)    
--insert #f (mask, description, always) values (4, 'No Marketing, Administrative or Newsletter Email', 1)    
--insert #f (mask, description, always) values (8, 'Passwords are Case Sensitive', 1)    
--insert #f (mask, description, always) values (16, 'Register new users with auto-login preference set to false', 1)    
--insert #f (mask, description, always) values (32, 'Tokens ONLY for noAnon Rules (no Registration) [Library]', 1)    
--insert #f (mask, description, always) values (64, 'No Collaboration Features', 0)    
--insert #f (mask, description, always) values (128, 'Registration Waitinglist Applies', 1)    
--insert #f (mask, description, always) values (256, 'Akamai Not Used', 0)    
--insert #f (mask, description, always) values (512, 'Hide emails at registration', 0)    
--insert #f (mask, description, always) values (1024, 'SecurityImmunity (used for internal and QA subs)', 0)    
--insert #f (mask, description, always) values (2048, 'Full Share Link (Mailto Collaboration)', 0)    
--insert #f (mask, description, always) values (4096, 'Use ParentSubs SU Limit(supported)', 0)    
--insert #f (mask, description, always) values (8192, 'Small Share Link (Mailto Collaboration)', 0)    
--insert #f (mask, description, always) values (16384, 'Emails fields use selectable domain dropdown', 0)    
--insert #f (mask, description, always) values (32768, 'Send welcome upon first login', 1)    
--insert #f (mask, description, always) values (65536, 'Collection changes disallowed', 0)    
--insert #f (mask, description, always) values (131072, 'Clear State on logout', 0)    
--insert #f (mask, description, always) values (262144, 'Include active users in report', 1)    
--insert #f (mask, description, always) values (524288, 'Corporate Top Books', 1)    
--insert #f (mask, description, always) values (1048576, 'Global account', 1)    
--insert #f (mask, description, always) values (2097152, 'New Book Notification after first login', 1)    
--insert #f (mask, description, always) values (4194304, 'Costcenter required', 1)    
--insert #f (mask, description, always) values (8388608, 'Disable all Downloads', 1)    
--insert #f (mask, description, always) values (67108864, 'No Direct Login', 1)    
--insert #f (mask, description, always) values (134217728, 'No New Book Notifications by Default (users opt in)', 1)    
create table #f (mask integer, description varchar(256), always int)    
insert #f (mask, description, always) values (1, 'No New Book Notices', 1)    
insert #f (mask, description, always) values (2, 'No Welcome Email', 1)    
insert #f (mask, description, always) values (4, 'No Marketing, System or Newsletter Email', 1)    
insert #f (mask, description, always) values (256, 'Akamai Not Used', 0)    
insert #f (mask, description, always) values (32768, 'Send welcome upon first login', 1)    
insert #f (mask, description, always) values (2097152, 'No NTNs Until After User First Login', 1)    
insert #f (mask, description, always) values (8388608, 'All Downloads Disabled', 1)    
insert #f (mask, description, always) values (67108864, 'No Direct Login Allowed', 1)    
insert #f (mask, description, always) values (134217728, 'Users Opt In for NTNs', 1)    
select 'Special Subscription Settings'=''    
   
select 'Setting'=#f.description, HiddenColumn='HiddenColumn'   from #f    
join subscription s with(nolock) on s.generalflags&#f.mask = #f.mask    
where s.subscriptionid=@subscriptionid    
--s.subscriptionid in ('E2F88BA3-32F8-43B5-8C94-E5167F95E14D','A235244B-9E89-4E21-96A4-97CB525F3D27','F0662602-A5A7-4988-B695-C9074F5154F2')    
and #f.always=1    
union     
    
    
select  'Setting'=d.comment, HiddenColumn='HiddenColumn'     
from subscriptionflag sf with(nolock)    
right join domaintable d with(nolock)    
on sf.flagid = d.number    
and subscriptionid = @subscriptionid    
where d.tablename ='subscriptionflag'    
and d.columnname ='flagid' and sf.status = 1    
and sf.flagid in (30,32,33,34,35,36,39,40,41)  
    
drop table #f    
end    
    
/*---------------------------------------    
Show collection entitlements    
---------------------------------------*/    
    
create table #c    
(collectionid uniqueidentifier,collectionname varchar(256), collectiontypeid int,    
capacity int, isprivate int, fullaccess int, description varchar(255),    
created datetime, status int, alias varchar(32), isdefault int, seatcount int,    
accessexpires datetime, sortorder int, hidden int, nativelanguage varchar(64),attributes varchar(500))    
    
insert into #c    
exec B24_GetCollections @subscriptionid,null,null,1    
    
select 'Collections'=''    
update #c set description = dbo.B24_HTMLDecode(description)  
  
;with sqlcte( description)  
as (select description  from #c group by description having  count(*)>1 )  
  
update #c set description = #c.description + ' (' + collectionname + ')'  
from #c, sqlcte t where #c.description =t.description   
  
if @deniedUplift is not null or @upliftQuota = 0 -- don't display chapter uplift     
begin    
select  --c.name,    
  'Name'=description,    
  'Licenses' = case  when seatcount=-1 and isdefault>0 then 'All users'    
        else cast(seatcount as varchar) end,    
  'Assigned' = case  when seatcount=-1 and isdefault>0 then ''    
        else cast(capacity as varchar) end,    
  'Selectable' = case  when isdefault=-1 or isdefault=0 then 'no'    
        when isdefault=1 then ''    
        when isdefault=2 then 'user'    
        when isdefault=3 then 'admin.'    
        else 'other ('+cast(isdefault as varchar)+')' end,    
  'Expires' = case when accessexpires is not null then left(accessexpires, 11)    
    else '' end    
-- ,cs.csflags    
-- ,c.sortorder    
from #c    
where #c.collectionid is not null    
order by description,sortorder, isdefault    
end    
else   --- include chapters to go -- they're potentially available    
begin    
select  --c.name,    
  'Name'=description,    
  'Licenses' = case  when seatcount=-1 and isdefault>0 then 'All users'    
        else cast(seatcount as varchar) end,    
  'Assigned' = case  when seatcount=-1 and isdefault>0 then ''    
        else cast(capacity as varchar) end,    
  'Selectable' = case  when isdefault=-1 or isdefault=0 then 'no'    
        when isdefault=1 then ''    
        when isdefault=2 then 'user'    
        when isdefault=3 then 'admin.'    
        else 'other ('+cast(isdefault as varchar)+')' end,    
  'Expires' = case when accessexpires is not null then left(accessexpires, 11)    
    else '' end,    
  'Chapters To Go' =     
        case --- find out if indvidual collections have uplift disabled (they don't have a type 3 entry in the entitlement table)    
            when @upliftQuota = 0 then 'N/A'    
            when exists(select * from entitlement with(nolock) where type=3 and resourceid=collectionid and ownerid=@subscriptionid and quota > 0 and status = 1) then 'Enabled'    
            else 'Disabled'    
        end    
-- ,cs.csflags    
-- ,c.sortorder    
from #c    
where #c.collectionid is not null    
order by  description,sortorder, isdefault    
end    
    
drop table #c    
    
--select top 100 * from collection where fullaccess!=1    
    
/*---------------------------------------    
Show current users    
---------------------------------------*/    
--if (@appname!='library' or @IsSupport>0)  and exists(select userid from libuser with(nolock) where subscriptionid=@subid)      
-- "Admin Users" section should be exposed for library subscriptions also.  
-- if (@IsSupport>0)  and exists(select userid from libuser with(nolock) where subscriptionid=@subid)   
if exists(select userid from libuser with(nolock) where subscriptionid=@subid)    
   
begin    
 --if @usedseats>100    
 --begin    
 -- select 'Admin Users (Partial Listing)'=''    
 -- select 'Manage Account to lookup users',    
 -- '@subscriptionid'=s.subscriptionid    
 -- from subscription s with(nolock)    
 -- where s.subscriptionid=@subid    
 --end    
 -- else   
  if @appname = 'skillport'  
  begin  
  select 'Admin Users'=''    
    
 select top 100    
 'Name' = p.FirstName+' '+p.LastName + '<BR/>'+ p.Email + '<BR/>'+ u.login ,    
 '@login' = u.login, --- a link!    
 '@enduserid' = u.userid, --- a link!    
 'Last Login' = case when  CONVERT(CHAR(10),lastlogin,120) = '1900-01-01' then 'never' else isnull(cast(lastlogin as varchar), 'never') end,    
-- 'Sessions'= isnull(cast(u.sessioncount as varchar), 'never'),    
-- 'Sections'= isnull(cast(u.pagecount as varchar), 'none'),    
-- 'Scrambled' = case when u.probationlevel>0 then 'yes' else 'no' end,    
-- Admin = case when u.userid=s.CustomerAdminID then 'X' else '' end,    
-- Admin = case when u.isAdmin=1 then 'X' else '' end,    
 CFA = case when charindex('CFA',ur.rolestring)>0 then 'X' else '' end,    
 CTA = case when charindex('CBTA',ur.rolestring)>0 then 'X' else '' end,    
 'Email Admin' = case when p.EmailAdminFlag = 1 then 'X' else '' end,    
-- 'Cost Center' = u.CostCenter,    
 Reports = case when exists(select rp.runnable from reportuserpermissions rp with(nolock) where rp.userid=u.userid) then 'X' else '' end    
-- '@subscriptionid' = u.subscriptionid -- link    
 from libuser u with(nolock)    
 join person p with(nolock) on p.personid = u.personid    
 join subscription s with(nolock) on u.subscriptionid=s.subscriptionid    
 left join userroles ur with(nolock) on ur.userid=u.userid    
 where u.subscriptionid=@subid    
 and u.subscriptiontypeid<>9 -- filter out anonymous users    
 and u.subscriptionstatusid<>8 -- filter out soft deleted users    
 and u.subscriptiontypeid not in (9,11,12,13,14,15) -- exclude PaidDenied and Library types    
 and (   
   --u.IsAdmin =1 or   
   p.EmailAdminFlag = 1  -- the user should be displayed if the email admin flag is set  
   or charindex('CFA',ur.rolestring)>0  
   or charindex('CBTA',ur.rolestring)>0  
   or exists(select rp.runnable from reportuserpermissions rp with(nolock) where rp.userid=u.userid)   
   )  
 order by    
 p.LastName,    
 --Admin desc,    
 CFA desc,    
 Reports desc,    
 u.sessioncount desc, u.lastlogin desc   
 end   
 else     
   begin  
  select 'Admin Users'=''    
    
 select top 100    
 'Name' = p.FirstName+' '+p.LastName + '<BR/>'+ p.Email + '<BR/>'+ u.login ,    
 '@login' = u.login, --- a link!    
 '@enduserid' = u.userid, --- a link!    
 'Last Login' = case when  CONVERT(CHAR(10),lastlogin,120) = '1900-01-01' then 'never' else isnull(cast(lastlogin as varchar), 'never') end,    
-- 'Sessions'= isnull(cast(u.sessioncount as varchar), 'never'),    
-- 'Sections'= isnull(cast(u.pagecount as varchar), 'none'),    
-- 'Scrambled' = case when u.probationlevel>0 then 'yes' else 'no' end,    
-- Admin = case when u.userid=s.CustomerAdminID then 'X' else '' end,    
 Admin = case when u.isAdmin=1 then 'X' else '' end,    
 CFA = case when charindex('CFA',ur.rolestring)>0 then 'X' else '' end,    
 CTA = case when charindex('CBTA',ur.rolestring)>0 then 'X' else '' end,    
 'Email Admin' = case when p.EmailAdminFlag = 1 then 'X' else '' end,    
-- 'Cost Center' = u.CostCenter,    
 Reports = case when exists(select rp.runnable from reportuserpermissions rp with(nolock) where rp.userid=u.userid) then 'X' else '' end    
-- '@subscriptionid' = u.subscriptionid -- link    
 from libuser u with(nolock)    
 join person p with(nolock) on p.personid = u.personid    
 join subscription s with(nolock) on u.subscriptionid=s.subscriptionid    
 left join userroles ur with(nolock) on ur.userid=u.userid    
 where u.subscriptionid=@subid    
 and u.subscriptiontypeid<>9 -- filter out anonymous users    
 and u.subscriptionstatusid<>8 -- filter out soft deleted users    
 and u.subscriptiontypeid not in (9,11,12,13,14,15) -- exclude PaidDenied and Library types    
 and ( u.IsAdmin =1    
   or p.EmailAdminFlag = 1  -- the user should be displayed if the email admin flag is set  
   or charindex('CFA',ur.rolestring)>0  
   or charindex('CBTA',ur.rolestring)>0  
   or exists(select rp.runnable from reportuserpermissions rp with(nolock) where rp.userid=u.userid)   
   )  
 order by    
 p.LastName,    
 Admin desc,    
 CFA desc,    
 Reports desc,    
 u.sessioncount desc, u.lastlogin desc   
 end   
end   
else if @appname='library'    
 --select 'This is a library subscription.'=''   
 begin   
 select 'Admin Users'=''    
 select 'None.'=''  
 end  
else    
 select 'This subscription has NO Admin users'=''    
/*---------------------------------------    
Show current users    
---------------------------------------*/    
if exists(select userid from libuser with(nolock) where subscriptionid=@subid)    
   
begin    
 if @usedseats>25    
 begin    
  select 'Registered Users'=''  
  select 'To view users in the subscription run the Registered Users report',    
  '@subscriptionid'=s.subscriptionid    
  from subscription s with(nolock)    
  where s.subscriptionid=@subid    
 end    
 else     
   begin  
  select 'Registered Users'=''    
    
 select top 25    
 'Name' = p.FirstName+' '+p.LastName + '<BR/>'+ p.Email + '<BR/>'+ u.login ,    
 '@login' = u.login, --- a link!    
 '@enduserid' = u.userid, --- a link!    
 'Last Login' = case when  CONVERT(CHAR(10),lastlogin,120) = '1900-01-01' then 'never' else isnull(cast(lastlogin as varchar), 'never') end    
-- Reports = case when exists(select rp.runnable from reportuserpermissions rp with(nolock) where rp.userid=u.userid) then 'X' else '' end    
-- '@subscriptionid' = u.subscriptionid -- link    
 from libuser u with(nolock)    
 join person p with(nolock) on p.personid = u.personid    
 join subscription s with(nolock) on u.subscriptionid=s.subscriptionid    
 left join userroles ur with(nolock) on ur.userid=u.userid    
 where u.subscriptionid=@subid    
 and u.subscriptiontypeid<>9 -- filter out anonymous users    
 and u.subscriptionstatusid<>8 -- filter out soft deleted users    
 and u.subscriptiontypeid not in (9,11,12,13,14,15) -- exclude PaidDenied and Library types    
 --and ( 
 --  or exists(select rp.runnable from reportuserpermissions rp with(nolock) where rp.userid=u.userid)   
 --  )  
 order by    
 p.LastName,    
 --Reports desc,    
 u.sessioncount desc, u.lastlogin desc   
 end   
end   
else    
 select 'This subscription has NO registered users'='' 
/*---------------------------------------    
Show pending registrations    
---------------------------------------*/    
    
if @appname!='library' and exists  
--(select email from registrationwaitinglist with(nolock) where subscriptionid=@subid)    
(select email  from  registrationwaitinglist wl with(nolock)    
 left join domaintable dt with(nolock) on dt.tablename='RegistrationWaitingList' and dt.columnname='status' and dt.number=wl.status    
 where (wl.subscriptionid = @subid or wl.subscriptionid=@trialrequestid)    
 and wl.subscriptionid is not null    
 and (wl.status in (0,1,2,3) or wl.status is null)  )  
begin    
    
 select 'Pending Registrations'=''    
    
 select    
 'Name' = isnull(wl.FirstName,'')+' '+isnull(wl.LastName,''),    
 'Email' = isnull(wl.email,''),    
 'Suggested Login' = isnull(wl.login,''),    
 'Send Instructions' = case when wl.sendemail = 1 then 'yes' else 'no' end,    
-- 'Submitted' = cast(wl.created as varchar),    
 'Status' = isnull(dt.Description,'Submitted'),    
 ' ' = wl.errorstring    
 from  registrationwaitinglist wl with(nolock)    
 left join domaintable dt with(nolock) on dt.tablename='RegistrationWaitingList' and dt.columnname='status' and dt.number=wl.status    
 where (wl.subscriptionid = @subid or wl.subscriptionid=@trialrequestid)    
 and wl.subscriptionid is not null    
 and (wl.status in (0,1,2,3) or wl.status is null)    
 order by wl.created    
    
end    
/*---------------------------------------    
Show Persistent Url for library sub  
---------------------------------------*/    
  
if @appname ='library' and exists (select url from dbo.NBNCustomization where NBNCustomizeID = coalesce(@pwdroot,NBNCustomizeID)and Active = 1 )  
begin  
 select 'Library Persistent Url' = ''  
 SELECT  'Persistent Url' = url,HiddenColumn='HiddenColumn'  
 FROM dbo.NBNCustomization with(nolock)  
 WHERE NBNCustomizeID = coalesce(@pwdroot,NBNCustomizeID)  
 AND CustomizeType = coalesce('PasswordRoot',customizetype)  
 and Active=1  
end  
  
/*---------------------------------------    
Original trial request    
---------------------------------------*/    
--may get rid of concept    
/*    
declare @havetr int    
select @havetr = count(*) from trialrequest tr with(nolock) where tr.subscriptionid=@subid    
if (@havetr>0)    
begin    
select 'Users MAY NOT have been registered (check registered list above)'=''    
select    
  'Role' = 'Primary',    
  'Name' = tr.FirstName+' '+tr.LastName,    
  'Email'= tr.Email    
from trialrequest tr with(nolock)    
  where tr.subscriptionid=@subid    
  and tr.userid is null    
union    
select    
  'Role' = 'Secondary',    
  'Name' = tru.FirstName+' '+tru.LastName,    
  'Email'= tru.Email    
from trialrequest tr with(nolock), trialrequestuser tru with(nolock)    
  where tr.subscriptionid=@subid and tru.trialrequestid=tr.trialrequestid    
  and tru.userid is null    
end    
*/    
/*---------------------------------------    
Generate an upgrade form    
to allow active trials and expiring trials    
to be converted    
---------------------------------------*/    
/*    
declare @trialrequestid uniqueidentifier    
select    
  @trialrequestid = tr.trialrequestid    
  from    
    subscription s with(nolock),    
    trialrequest tr with(nolock)    
  where s.subscriptionid=@subid    
    and s.type=0 and s.status in (1,2) -- trials either active or expiring    
    and s.expires>getdate()        -- haven't expired already    
 and tr.subscriptionid = s.subscriptionid    
    and tr.resellercode is not null and s.resellercode is not null -- reseller only    
    
----------------------------------------    
if @trialrequestid is not null    
 exec SALES3_HTMLCorporateTrialUpgradeForm @userid, @trialrequestid    
*/



GO


