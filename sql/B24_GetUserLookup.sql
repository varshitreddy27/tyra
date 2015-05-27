SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[B24_GetUserLookup]
@userid uniqueidentifier,
@login varchar(64),
@fname varchar(80),
@lname varchar(80),
@email varchar(96),
@SubscriptionID uniqueidentifier = null,
@PasswordRoot char(5) = null,
@ExactMatch int = 0,
@ActiveOnly int = 0

AS

set nocount on

declare @AppName varchar(50)
set @appname = app_name()

if @ExactMatch is null
  set @ExactMatch = 0

if @ActiveOnly is null
  set @ActiveOnly = 0

set @login=replace(upper(ltrim(rtrim(@login))),'''','''''')
set @fname=replace(upper(ltrim(rtrim(@fname))),'''','''''')
set @lname=replace(upper(ltrim(rtrim(@lname))),'''','''''')
set @email=replace(upper(ltrim(rtrim(@email))),'''','''''')

if @exactmatch=0
begin
	set @login=replace(replace(@login,'_','[_]'),'%','[%]')
	set @email=replace(replace(@email,'_','[_]'),'%','[%]')
end

declare @nullguid uniqueidentifier, @usersubid uniqueidentifier, @sql nvarchar(4000)
set @nullguid = '{00000000-0000-0000-0000-000000000000}'


-- there are many ways (unions, outer joins) to answer this query,
-- but I believe this may be reasonably efficient (allows precompilation)
-- as well as one of the more "transparent" ways to get the job done.

-- first check that the user is an MAB admin or salesperson...
declare @inta int, @HasGenAdminPriv int, @IsReseller int, @clausecnt int, @HasSalesMgrPriv int, @salesemail varchar(96), @AllowGlobalAccount int, @IsSupport int

select @inta=(SalesMarketing +GeneralAdmin), @HasGenAdminPriv=GeneralAdmin,@IsReseller=Reseller, @HasSalesMgrPriv=SalesManager
from permissions with(nolock)
    where   userid=@userid
set @IsSupport = dbo.B24IsInSalesGroup('FREDSUP', @userid)

if @inta=0 and @isReseller=0 and @IsSupport=0
begin
	raiserror(50024,15,1)
	return 1
end

if exists (select * from salesgroupmember sgm with(nolock) join salesgroup sg with(Nolock) on sgm.salesgroupid=sg.salesgroupid
	where sgm.userid=@userid and isnull(sg.flags,0)&16<>0)
	set @AllowGlobalAccount=1
else
	set @AllowGlobalAccount=0


select @usersubid=l.subscriptionid, @salesemail=p.email from libuser l with(nolock) join person p with(nolock) on l.personid=p.personid
where l.userid=@userid

create table #t(u uniqueidentifier, s uniqueidentifier, salesid uniqueidentifier, status int)


if (@subscriptionID is null or @subscriptionID=@nullguid) 
	set @subscriptionid=null

if @subscriptionID is null and @passwordroot is not null
	select @subscriptionid=subscriptionid from subscription with(nolock) where passwordroot=@passwordroot

set @sql = '
insert into #t(u,s)
select top 500 l.userid,l.subscriptionid
from libuser l with (nolock'+
	   case when  isnull(@login,'')<>'' then ',index(LoginMustBeUniqueIX)) '
		
		else ') ' end

if @subscriptionid is not null or @ActiveOnly=1
	set @sql=@sql+'join subscription s with(nolock) on l.subscriptionid=s.subscriptionid '

if isnull(@fname,'')<>'' or isnull(@lname,'')<>'' or isnull(@email,'')<>''
	set @sql=@sql+'join person p with(nolock'+
	   case when isnull(@email,'')<>'' then ',index(emailname)) '
		when isnull(@lname,'')<>'' then ',index(lastname)) '
		else ') ' end + ' on l.personid=p.personid '

set @sql=@sql + 'where l.subscriptionstatusid not in ('+case when @activeonly=1 then '0,5,6,8' else '-1' end+') and '

set @clausecnt=0

if @subscriptionid is not null
begin
	set @clausecnt=@clausecnt+1
	if @clausecnt>1
		set @sql=@sql+'and '
	set @sql=@sql+'l.subscriptionid='''+cast(@subscriptionid as char(36))+''' '
end

if @activeonly=1
begin
	set @clausecnt=@clausecnt+1
	if @clausecnt>1
		set @sql=@sql+'and '
	set @sql=@sql+'s.expires>getdate() and s.status>0 '
end


if isnull(@login,'')<>''
begin
	set @clausecnt=@clausecnt+1
	if @clausecnt>1
		set @sql=@sql+'and '
	set @sql=@sql+case when @exactmatch=1 then 'l.login='''+@login+''' '
				else 'l.login like ''%'+@login+'%'' ' end
end

if isnull(@fname,'')<>''
begin
	set @clausecnt=@clausecnt+1
	if @clausecnt>1
		set @sql=@sql+'and '
	set @sql=@sql+case when @exactmatch=1 then 'p.firstname='''+@fname+''' '
				else 'p.firstname like ''%'+@fname+'%'' ' end
end

if isnull(@lname,'')<>''
begin
	set @clausecnt=@clausecnt+1
	if @clausecnt>1
		set @sql=@sql+'and '
	set @sql=@sql+case when @exactmatch=1 then 'p.lastname='''+@lname+''' '
				else 'p.lastname like ''%'+@lname+'%'' ' end
end

if isnull(@email,'')<>''
begin
	set @clausecnt=@clausecnt+1
	if @clausecnt>1
		set @sql=@sql+'and '
	set @sql=@sql+case when @exactmatch=1 then 'p.email='''+@email+''' '
				else 'p.email like ''%'+@email+'%'' ' end
end
 
----Hack to limit who has access to microsoft customers, exclude sales3 (P.C. 7/29/2008)
if (isnull(@HasGenAdminPriv,0)=0 and charindex('Reseller',@appname)=0) 
begin
	set @clausecnt=@clausecnt+1
	if @clausecnt>1
		set @sql=@sql+'and '
	set @sql=@sql+ ' l.subscriptionid not in (select subscriptionid from subscription with(nolock) where charindex(''microsoft'',applicationname)>0) '
end
 
if @clausecnt=0
begin
	raiserror(50036, 15, 1, 'Some information')
	return 1
end
 
if app_name()='Microsoft SQL Server Management Studio - Query' print @sql
exec sp_executesql @sql

update #t set salesid=s.salespersonid,status=case when s.status=0 or s.expires<getdate() then 0 else 1 end
from subscription s
where #t.s=s.subscriptionid

if @HasGenAdminPriv=0 and @IsSupport=0
begin
	declare @UnassignedID uniqueidentifier
	if exists (select * from salesgroupmember sm with(nolock), salesgroup s with(nolock) where userid=@userid and sm.salesgroupid=s.salesgroupid and s.name like '%osborne%')
		select @UnassignedID=userid from libuser with(nolock) where login='osborneunassignedsales'
	else
	if exists (select * from salesgroupmember sm with(nolock), salesgroup s with(nolock) where userid=@userid and sm.salesgroupid=s.salesgroupid and s.name like '%cdi%')
		select @UnassignedID=userid from libuser with(nolock) where login='cdilearnunassignedsales'
	else
	if exists (select * from salesgroupmember sm with(nolock), salesgroup s with(nolock) where userid=@userid and sm.salesgroupid=s.salesgroupid and s.name like 'appcon%')
		select @UnassignedID=userid from libuser with(nolock) where login='appconunassignedsales'
	else
	if exists (select * from salesgroupmember sm with(nolock), salesgroup s with(nolock) where userid=@userid and sm.salesgroupid=s.salesgroupid and s.name like 'mindsharp%')
		select @UnassignedID=userid from libuser with(nolock) where login='msunassignedsales'
	else
		select @UnassignedID=userid from libuser with(nolock) where login='unassignedsales'

	update #t set salesid=@userid where #t.s=@usersubid	-- pretend the callers sub is owned by him so it wont get deleted from the lookup result.
								-- this allows a salesperson to always be able to find his own subscription.

	delete #t
	from #t join subscription s with(nolock) on #t.s=s.subscriptionid
		where (#t.salesid is null or (#t.salesid is not null and #t.salesid not in (select sg.userid from salesgroupmember sg with (nolock)
								where sg.salesgroupid in (select sgm.salesgroupid from salesgroupmember sgm with (nolock)
												join salesgroup sg with(nolock) on sgm.salesgroupid=sg.salesgroupid and sg.type>0
							 				where  sgm.userid=@userid)
							          	union select @UnassignedID))
			and (@AllowGlobalAccount=0 or (@AllowGlobalAccount=1 and isnull(s.generalflags,0)&1048576=0)))

end

--------------------------------------------------------
--  Remove hits assigned to @UnassignedID from resellers
--  unless branded with the reseller's code(s)
---------------------------------------------------------
  if @IsReseller>0 and @IsSupport=0
  delete #t
    where  #t.salesid=@UnassignedID
    and #t.u not in

    (select subscriptionid from subscription s with(nolock) where s.resellercode in
    (select resellercode from salesgroup sg with(nolock) where sg.type>0 and sg.salesgroupid in
    (select sgm.salesgroupid from salesgroupmember sgm with(nolock) where sgm.userid=@userid)))

/*---------------------------------------------------
If Requestor has Reseller restriction then remove
subscriptions owned by anyone who is not reseller
restricted or owned by anyone with management privs
---------------------------------------------------*/

if @IsReseller>0 and @IsSupport=0
begin
	declare @IsInSKILACgroup tinyint
	if exists (select * from salesgroupmember sgm with(nolock) join salesgroup sg with(nolock) on sg.salesgroupid=sgm.salesgroupid and sgm.userid=@userid and sg.name='SKILAC' and sg.type>0)
		set @IsInSKILACgroup=1
	else
		set @IsInSKILACgroup=0

	if @IsInSKILACgroup=0
		delete #t
		where  #t.salesid<>@userid
		and #t.salesid in
		(select sgm.userid from salesgroupmember sgm with(nolock)
			       join salesgroup sg with(nolock) on sg.salesgroupid=sgm.salesgroupid and sg.type>0
			       join permissions pm with(nolock) on pm.userid=sgm.userid
			       join libuser l with(nolock) on pm.userid=l.userid
			       join person p with(nolock) on p.personid=l.personid
		--	 where (pm.SalesManager=1 or pm.GeneralAdmin=1 or pm.SuperUser=1)or pm.Reseller=0)
		where pm.SalesManager>@HasSalesMgrPriv or pm.GeneralAdmin>@HasGenAdminPriv or pm.SuperUser=1 or pm.Reseller=0
		   or ((p.email like '%@skillsoft.com' or p.email like '%@books24x7.com')
		       and (@salesemail not like '%@skillsoft.com' and @salesemail not like '%@books24x7.com'))  )


	else -- @IsInSKILACgroup=1
		delete #t
		where  #t.salesid<>@userid
		and #t.salesid in
		(select sgm.userid from salesgroupmember sgm with(nolock)
			       join salesgroup sg with(nolock) on sg.salesgroupid=sgm.salesgroupid and sg.type>0
			       join permissions pm with(nolock) on pm.userid=sgm.userid
			       join libuser l with(nolock) on pm.userid=l.userid
			       join person p with(nolock) on p.personid=l.personid
		--	 where (pm.SalesManager=1 or pm.GeneralAdmin=1 or pm.SuperUser=1)or pm.Reseller=0)
		where (pm.SalesManager>@HasSalesMgrPriv or pm.GeneralAdmin>@HasGenAdminPriv or pm.SuperUser=1 or pm.Reseller=0
		   or ((p.email like '%@skillsoft.com' or p.email like '%@books24x7.com')
		       and (@salesemail not like '%@skillsoft.com' and @salesemail not like '%@books24x7.com')))
		   and sgm.userid not in (select distinct userid from salesgroupmember sgm with(nolock) join salesgroup sg with(nolock) on sgm.salesgroupid=sg.salesgroupid and sg.type>0
						where userid<>@userid and sgm.salesgroupid in (select sgm.salesgroupid 
								from salesgroupmember sgm with(nolock) join salesgroup sg with(nolock) on sgm.salesgroupid=sg.salesgroupid and sg.type>0
								where sgm.userid=@userid)) )
end




select top 201
	l.userid,
	l.login,
	'DisplayLogin'=case when charindex(':',l.login)>1 then right(l.login,len(l.login) - charindex(':',l.login)) else l.login end,
	l.FixedUserName,
	l.lastlogin,
	l.subscriptionid,
	l.ProbationLevel,
	p.firstname,
	p.lastname,
	p.email,
	'SubscriptionType'=s.type,
	s.starts,
	s.expires,
	s.PasswordRoot,
    s.ApplicationName,
    'Name'=(select case when ltrim(rtrim(c.Name))='' then '-' else c.Name end from company c with(nolock) where c.companyid=s.companyid),
	'CompanyOrDepartment'=case when ltrim(rtrim(s.CompanyOrDepartment))='' then '-' else s.CompanyOrDepartment end,    
	'CostCenter'=l.CostCenter,
	l.SubscriptionStatusID
	from #t join libuser l with (nolock) on l.userid=#t.u
		LEFT OUTER JOIN subscription s with (nolock) ON l.SubscriptionID = s.SubscriptionID
		LEFT OUTER JOIN person p with (nolock) on l.personid=p.personid
	order by	 l.login


drop table #t

return

/*
 GRANT  EXECUTE  ON [dbo].[B24_GetUserLookup]  TO [PolecatProduction]
*/


