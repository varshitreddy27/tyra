USE [bookdb]
GO

/****** Object:  StoredProcedure [dbo].[SALES3_HTMLUserInfo]    Script Date: 09/22/2010 10:17:08 ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER OFF
GO



ALTER      PROCEDURE [dbo].[SALES3_HTMLUserInfo]
@userid uniqueidentifier = null,  -- the person making the request
@login varchar(96) = null
AS
set nocount on

/*---------------------------------------
Calculate Security
---------------------------------------*/
declare @warninglevel int, @acklevel int
declare @enduserid uniqueidentifier
select 
    @enduserid = l.userid,	
    @warninglevel=case
    when prefstring is null then 0
    when charindex('warning=',prefstring)>0 then 
      cast(substring(prefstring,charindex('warning=',prefstring)+8,
           charindex('|',prefstring+'|',
           charindex('warning=',prefstring)) - (charindex('warning=',prefstring)+8)) as int)
      else 0 end,
    @acklevel=case
    when prefstring is null then 0
    when charindex('acklevel=',prefstring)>0 then 
      cast(substring(prefstring,charindex('acklevel=',prefstring)+9,
           charindex('|',prefstring+'|',
           charindex('acklevel=',prefstring)) - (charindex('acklevel=',prefstring)+9)) as int)
      else 0 end
    from libuser l with(nolock) 
    where l.login=@login
 

/*---------------------------------------
Display user summary
---------------------------------------*/
select 'User Information'
declare @now datetime, @sqlstr nvarchar(4000),@isskillport bit
set @now=getdate()
set @isskillport = 0

--check to see if skillport app
if exists (select login from libuser l with(nolock) inner join subscription s with(nolock) on l.subscriptionid = s.subscriptionid where l.login = @login and s.applicationname = 'skillport')
	begin
		set @isskillport = 1
	end

set @sqlstr = 'select case when l.subscriptionstatusid = 8 and left(l.login,6) <> ''expire'' then '''' + l.login + '' (soft deleted) '' else case left(l.login,6) when ''expire'' then l.login+ '' (''+left(l.option2,len(l.option2)-1)+'')''+REPLICATE('' *'',l.probationlevel)
				else  l.login+REPLICATE('' *'',l.probationlevel) end end as ''B24 Login'', ''Name'' = isnull(p.firstname+'' ''+p.lastname,''''), ''Application''=s.ApplicationName,'
if @isskillport = 1 --if skillport app, include fixed username
	begin
		set @sqlstr = @sqlstr + '''SkillPort UserID'' =right(l.fixedusername,len(l.fixedusername)-charindex(''/'',l.fixedusername)),'
	end						
set @sqlstr = @sqlstr + '''Collections''=isnull(dbo.B24_CSListHide(l.collectionstr,null,7),''''),''Email''=p.email,''Cost Center'' = isnull(cc.Description,l.costcenter),''Scrambled'' = case when l.probationlevel>0 then ''yes'' else ''no'' end,
			''CFA'' = case when charindex('',CFA,'' ,'',''+isnull(ur.RoleString,'''')+'','')>0 then ''yes'' else ''no'' end, ''Restrictions'' = case when ' + cast(@warninglevel as varchar(4)) + '=1 and ' + cast(@acklevel as varchar(4)) + '=1 then ''Warning 1 (acknowledged)''
			when ' + cast(@warninglevel as varchar(4)) + '=1 then ''Warning 1''	when ' + cast(@warninglevel as varchar(4)) + '=2 and ' + cast(@acklevel as varchar(4))+ '=2 then ''Warning 2 (acknowledged)''  when ' + cast(@warninglevel as varchar(4)) + '=2 then ''Warning 2'' 
			when ' + cast(@warninglevel as varchar(4)) + '>=3 and '+ cast(@acklevel as varchar(4)) + '>=' + cast(@warninglevel as varchar(4)) + 'then ''Access Reinstated'' when ' + cast(@warninglevel as varchar(4))+ '>=3 and ' + cast(@acklevel as varchar(4))+ '<' + cast(@warninglevel as varchar(4)) + 'then ''Access Revoked'' else ''None'' end
			,''Send Email''=case when p.DontSendEmail=1 then ''no'' else ''yes'' end,
			''AutoLogin Cookie''=case when l.UseCookie=1 then ''yes'' else ''no'' end,''New Books Email''=dt.description,''Last Login'' = isnull(cast(l.lastlogin as varchar), ''never''),
			''Comment'' = isnull(p.Notes,''''),''@subscriptionid'' = s.subscriptionid, 
			
			''@enduserid''=l.userid  from libuser l with(nolock) join person p with(nolock)	on p.personid=l.personid  join subscription s with(nolock) on l.subscriptionid=s.subscriptionid
			 left outer join domaintable dt with(nolock) on l.emailstatus=dt.number and dt.tablename=''Libuser'' and dt.columnname=''EmailStatus''  left outer join UserRoles ur with(nolock) on l.userid=ur.userid left outer join CostCenters cc with(nolock) 
			on cc.subscriptionid=l.subscriptionid and cc.costcenter=l.costcenter  join company c with(nolock) on s.companyid=c.companyid
			where l.login=''' + @login + ''''

exec sp_executesql @sqlstr

/*---------------------------------------
Datafeed history (if exists)
---------------------------------------*/
if exists(select * from datafeedcollectionitem df with(nolock) where df.userid=@enduserid)
begin

select 'Datafeed History'

select 
'Date'=left(df.created,11),
'Collection'=c.Description,
'From'=left(df.starts,11),
'Till'=left(df.expires,11),
'Granted'= case when df.status=1 then 'yes' else 'no' end
from datafeedcollectionitem df with(nolock)
join collection c with(nolock) on df.collectionstr=c.name
where df.userid=@enduserid
order by df.created

end

--- exec SALES3_HTMLMenu @userid






GO


