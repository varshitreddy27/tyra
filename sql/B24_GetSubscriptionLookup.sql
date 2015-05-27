USE [bookdb]
GO
SET ANSI_NULLS ON 
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER Procedure [dbo].[B24_GetSubscriptionLookup]
@userid	uniqueidentifier,			-- user requesting this information 
						-- (required for wild-card lookup)
@subscriptionid uniqueidentifier,		-- subs id desired (if null, search following fields)
@salespersonfirstname varchar(80),	-- wildcard lookup
@salespersonlastname varchar(80),	-- wildcard lookup
@accountnumber varchar(20),		-- wildcard lookup
@companyname varchar(80),		-- wildcard lookup
@contactfirstname varchar(80),		-- wildcard lookup
@contactlastname varchar(80),		-- wildcard lookup
@passwordroot char(5) = null,
@ExactMatch int = 0,			-- only return rows that match exactly
@ActiveOnly int = 0,			-- only return active subscriptions
@UserIsSalesPersonOnly int = 0

/*
	Whenever subscription is null, any ignored wildcard lookup *must* be null
	
	For wildcard lookups, retrieve subscriptions matching all non-null wildcards
	(as if they are and-ed together).
*/

As
set nocount on

declare @inta int, @HasGenAdminPriv int, @sql nvarchar(4000), @clausecnt int, @salesrestricted int, @HasSalesMgrPriv int, @IsReseller int, @email varchar(96), @AllowGlobalAccount int, @IsSupport int
declare @appname varchar(50)
--get app name
set @appname = app_name()
set @clausecnt=0

select @HasGenAdminPriv=isnull(GeneralAdmin,0), @HasSalesMgrPriv=isnull(SalesManager,0),@IsReseller=isnull(reseller,0)
  from permissions with(nolock) where userid=@userid
set @IsSupport = dbo.B24IsInSalesGroup('FREDSUP', @userid)

-- first check that the user is an MAB admin or salesperson...
select @inta=count(*) 
		from permissions with(nolock)
		where userid=@userid and 
			(SalesMarketing = 1 or GeneralAdmin = 1)
if @inta=0 and @isReseller=0 and @IsSupport=0
begin
	raiserror(50024,15,1)
	return 1
end

if @ExactMatch is null
  set @ExactMatch = 0

if @ActiveOnly is null
  set @ActiveOnly = 0

set @salespersonfirstname=replace(upper(ltrim(rtrim(@salespersonfirstname))),'''','''''')
set @salespersonlastname=replace(upper(ltrim(rtrim(@salespersonlastname))),'''','''''')
set @accountnumber=replace(upper(ltrim(rtrim(@accountnumber))),'''','''''')
set @companyname=replace(upper(ltrim(rtrim(@companyname))),'''','''''')
set @contactfirstname=replace(upper(ltrim(rtrim(@contactfirstname))),'''','''''')
set @contactlastname=replace(upper(ltrim(rtrim(@contactlastname))),'''','''''')
set @passwordroot=replace(upper(ltrim(rtrim(@passwordroot))),'''','''''')

if exists (select * from salesgroupmember sgm with(nolock) join salesgroup sg with(Nolock) on sgm.salesgroupid=sg.salesgroupid
	where sgm.userid=@userid and isnull(sg.flags,0)&16<>0)
	set @AllowGlobalAccount=1
else
	set @AllowGlobalAccount=0


if @subscriptionID is null and @passwordroot is not null
begin
	select @subscriptionid=subscriptionid from subscription with(nolock) where passwordroot=@passwordroot
	if @subscriptionid is null
	begin
		raiserror(50029,15,1,'A valid SubscriptionID')
		return 1
	end
end

/*================================================================
Create a table to hold all the potential results to be considered
================================================================*/
create table #t(u uniqueidentifier, salesid uniqueidentifier, status int)	
create table #sp (salespersonid uniqueidentifier)

set @clausecnt=0
set @salesrestricted=0

if @UserIsSalesPersonOnly=1
begin
	set @salesrestricted=1
	insert into #sp values (@userid)
end
else
if isnull(@salespersonfirstname,'')<>'' or isnull(@salespersonlastname,'')<>'' -- If a sales person was specified, gather up all matching salespersonids
begin
	set @salesrestricted=1
	set @sql = '
	insert into #sp
	select l.userid
	from person sp with(nolock)
		join libuser l with(nolock) on sp.personid=l.personid
		join permissions pv with(nolock) on pv.userid=l.userid
	where '

	if isnull(@salespersonfirstname,'')<>''
	begin
		set @clausecnt=@clausecnt+1
		if @clausecnt>1
			set @sql=@sql+'and '
		set @sql=@sql+case when @exactmatch=1 then 'sp.firstname='''+@salespersonfirstname+''' '
					else 'sp.firstname like ''%'+@salespersonfirstname+'%'' ' end
	end

	if isnull(@salespersonlastname,'')<>''
	begin
		set @clausecnt=@clausecnt+1
		if @clausecnt>1
			set @sql=@sql+'and '
		set @sql=@sql+case when @exactmatch=1 then 'sp.lastname='''+@salespersonlastname+''' '
					else 'sp.lastname like ''%'+@salespersonlastname+'%'' ' end
	end

--	if app_name()='sql query analyzer' print @sql
	exec sp_executesql @sql
end


if isnull(@salespersonfirstname,'')<>'' or isnull(@salespersonlastname,'')<>'' or @UserIsSalesPersonOnly=1
begin
	set @salesrestricted=1
	set @sql = '
	insert into #t(u,salesid)
	select s.subscriptionid, s.salespersonid
	from #sp sp
	join subscription s with (nolock,index(SalesPersonNDX)) on s.salespersonid=sp.salespersonid and s.salespersonid is not null '

	if @subscriptionid is not null
		set @sql=@sql+'and s.subscriptionid='''+cast(@subscriptionid as char(36))+''' '

	if @activeonly=1
		set @sql=@sql+'and s.expires>getdate() and s.status>0 '

	if isnull(@accountnumber,'')<>''
		set @sql=@sql+case when @exactmatch=1 then 'and s.accountnumber='''+@accountnumber+''' '
					else 'and s.accountnumber like ''%'+@accountnumber+'%'' ' end

	if isnull(@companyname,'')<>''
		set @sql=@sql+'join company c with(nolock) on s.companyid=c.companyid and '
			+case when @exactmatch=1 then 'c.name='''+@companyname+''' '
				else 'c.name like ''%'+@companyname+'%'' ' end	

	if isnull(@contactfirstname,'')<>'' or isnull(@contactlastname,'')<>''
		set @sql=@sql+'join person cp with(nolock) on s.contactid=cp.personid '

	if isnull(@contactfirstname,'')<>''
		set @sql=@sql+case when @exactmatch=1 then 'and cp.firstname='''+@contactfirstname+''' '
					else 'and cp.firstname like ''%'+@contactfirstname+'%'' ' end

	if isnull(@contactlastname,'')<>''
		set @sql=@sql+case when @exactmatch=1 then 'and cp.lastname='''+@contactlastname+''' '
					else 'and cp.lastname like ''%'+@contactlastname+'%'' ' end
end
else
if isnull(@companyname,'')<>''
begin

	set @sql = '
	insert into #t(u,salesid)
	select s.subscriptionid, s.salespersonid
	from company c with(nolock) '

	set @sql=@sql+'join subscription s with(nolock,index(companyndx)) on s.companyid=c.companyid and s.salespersonid is not null '
	
	if @subscriptionid is not null
		set @sql=@sql+'and s.subscriptionid='''+cast(@subscriptionid as char(36))+''' '

	if @activeonly=1
		set @sql=@sql+'and s.expires>getdate() and s.status>0 '

	if isnull(@accountnumber,'')<>''
		set @sql=@sql+case when @exactmatch=1 then 'and s.accountnumber='''+@accountnumber+''' '
					else 'and s.accountnumber like ''%'+@accountnumber+'%'' ' end

	if isnull(@contactfirstname,'')<>'' or isnull(@contactlastname,'')<>''
		set @sql=@sql+'join person cp with(nolock) on s.contactid=cp.personid '


	if isnull(@contactfirstname,'')<>''
		set @sql=@sql+case when @exactmatch=1 then 'and cp.firstname='''+@contactfirstname+''' '
					else 'and cp.firstname like ''%'+@contactfirstname+'%'' ' end

	if isnull(@contactlastname,'')<>''
		set @sql=@sql+case when @exactmatch=1 then 'and cp.lastname='''+@contactlastname+''' '
					else 'and cp.lastname like ''%'+@contactlastname+'%'' ' end

	set @clausecnt=@clausecnt+1

	set @sql=@sql+' where ' + case when @exactmatch=1 then 'c.name='''+@companyname+''' '
			else 'c.name like ''%'+@companyname+'%'' ' end
end
else
if isnull(@contactfirstname,'')<>'' or isnull(@contactlastname,'')<>''
begin
	set @sql = '
	insert into #t(u,salesid)
	select s.subscriptionid, s.salespersonid
	from person cp with(nolock) '

	set @sql=@sql+'join subscription s with(nolock,index(ContactNDX)) on s.contactid=cp.personid and s.salespersonid is not null '
	
	if @subscriptionid is not null
		set @sql=@sql+'and s.subscriptionid='''+cast(@subscriptionid as char(36))+''' '

	if @activeonly=1
		set @sql=@sql+'and s.expires>getdate() and s.status>0 '

	if isnull(@accountnumber,'')<>''
		set @sql=@sql+case when @exactmatch=1 then 'and s.accountnumber='''+@accountnumber+''' '
					else 'and s.accountnumber like ''%'+@accountnumber+'%'' ' end

	set @sql=@sql+'where '
	if isnull(@contactfirstname,'')<>''
	begin
		set @clausecnt=@clausecnt+1
		if @clausecnt>1
			set @sql=@sql+'and '
		set @sql=@sql+case when @exactmatch=1 then 'cp.firstname='''+@contactfirstname+''' '
					else 'cp.firstname like ''%'+@contactfirstname+'%'' ' end
	end

	if isnull(@contactlastname,'')<>''
	begin
		set @clausecnt=@clausecnt+1
		if @clausecnt>1
			set @sql=@sql+'and '
		set @sql=@sql+case when @exactmatch=1 then 'cp.lastname='''+@contactlastname+''' '
					else 'cp.lastname like ''%'+@contactlastname+'%'' ' end
	end
end
else
if (@subscriptionid is not null) or (isnull(@accountnumber,'')<>'')
begin
	set @sql = '
	insert into #t(u,salesid)
	select s.subscriptionid, s.salespersonid
	from subscription s with(nolock) where '

	if @subscriptionid is not null
	begin
		set @clausecnt=@clausecnt+1
		if @clausecnt>1
			set @sql=@sql+'and '
		set @sql=@sql+'s.subscriptionid='''+cast(@subscriptionid as char(36))+''' '
	end

	if @activeonly=1
	begin
		set @clausecnt=@clausecnt+1
		if @clausecnt>1
			set @sql=@sql+'and '
		set @sql=@sql+'s.expires>getdate() and s.status>0 '
	end

	if isnull(@accountnumber,'')<>''
	begin
		set @clausecnt=@clausecnt+1
		if @clausecnt>1
			set @sql=@sql+'and '
		set @sql=@sql+case when @exactmatch=1 then 's.accountnumber='''+@accountnumber+''' '
					else 's.accountnumber like ''%'+@accountnumber+'%'' ' end
	end
end
else
begin
	raiserror(50036, 15, 1, 'Some information')
	return 1
end

--	--Hack to limit who has access to microsoft customers, exclude sales3 (P.C. 7/29/2008)
	if (isnull(@HasGenAdminPriv,0)=0 and charindex('Reseller',@appname)=0) 
	begin
		set @clausecnt=@clausecnt+1
		if @clausecnt>1
			set @sql=@sql+' and '
	 
		set @sql=@sql+ ' (charindex(''microsoft'',s.applicationname)=0 or '''
		--hack to allow •	Gregbe@microsoft.com•	Kevinke@microsoft.com•	Willt@microsoft.com,Jen Gropel  access to MSFT accounts per Gary 06/25/2009.  
		--Need to check with jen when she is back from maturnity on this   

		set @sql = @sql + cast(@userid as varchar(36)) + ''' in (''C3275C80-3BCB-4801-9C06-9583A99F2728'',''D0F1B23D-9C1A-4862-B5F8-B31C8A97086B'',''8B08C669-7FB1-47D4-A7F8-034CAAFA298D'',''8FF5BFA8-34D8-4CF9-AD52-D1A1E9D285F7'',''7C955E10-BDF2-410E-A1A0-2E1B15248975''))'
	
	end

if app_name()='Microsoft SQL Server Management Studio - Query' print @sql
exec sp_executesql @sql


/*============================================================
The temp table now contains all the matches for lookup
We must apply permission filters to hide unwanted data
============================================================*/

select @email=p.email from libuser l with(nolock) join person p with(nolock) on l.personid=p.personid
where l.userid=@userid

/*------------------------------------------
Unless General Admin, restrict to salesgroup
and unassigned sales 
------------------------------------------*/ 
if @HasGenAdminPriv  = 0 and @IsSupport=0  -- allow support personnel (FREDSUP)to see accounts as well
begin
	declare @UnassignedID uniqueidentifier
	if exists (select * from salesgroupmember sm with(nolock), salesgroup s with(nolock) where userid=@userid and sm.salesgroupid=s.salesgroupid and s.name like '%peachpit%')
		select @UnassignedID=userid from libuser with(nolock) where login='vqsunassignedsales'
	else
	if exists (select * from salesgroupmember sm with(nolock), salesgroup s with(nolock) where userid=@userid and sm.salesgroupid=s.salesgroupid and s.name like '%osborne%')
		select @UnassignedID=userid from libuser with(nolock) where login='osborneunassignedsales'
	else
	if exists (select * from salesgroupmember sm with(nolock), salesgroup s with(nolock) where userid=@userid and sm.salesgroupid=s.salesgroupid and s.name like '%cdi%')
		select @UnassignedID=userid from libuser with(nolock) where login='cdilearnunassignedsales'
	else
	if exists (select * from salesgroupmember sm with(nolock), salesgroup s with(nolock) where userid=@userid and sm.salesgroupid=s.salesgroupid and s.name like 'appcon%')
		select @UnassignedID=userid from libuser with(nolock) where login='appconunassignedsales'
	else
		select @UnassignedID=userid from libuser with(nolock) where login='unassignedsales'

	delete #t 
	from #t join subscription s with(nolock) on #t.u=s.subscriptionid
		where  #t.salesid is not null and #t.salesid not in (
			select sg.userid from salesgroupmember sg with(nolock)
			where sg.salesgroupid in (select sgm.salesgroupid from salesgroupmember sgm with(nolock) where sgm.userid=@userid)
			union select @UnassignedID)
		and (@AllowGlobalAccount=0 or (@AllowGlobalAccount=1 and isnull(s.generalflags,0)&1048576=0))
end

/*---------------------------------------------------
Remove hits assigned to @UnassignedID from resellers
unless branded with the reseller's code(s)
---------------------------------------------------*/
if @IsReseller>0 and @IsSupport=0
begin
	declare @IsInSKILACgroup tinyint
	if exists (select * from salesgroupmember sgm with(nolock) join salesgroup sg with(nolock) on sg.salesgroupid=sgm.salesgroupid and sgm.userid=@userid and sg.name='SKILAC' and sg.type>0)
		set @IsInSKILACgroup=1
	else
		set @IsInSKILACgroup=0

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

	if @IsInSKILACgroup=0
		delete #t
		from #t
		inner join subscription s with(nolock)
		on #t.u = s.subscriptionid
		where  #t.salesid<>@userid
		and #t.salesid in
		(select sgm.userid from salesgroupmember sgm with(nolock)
			       join salesgroup sg with(nolock) on sg.salesgroupid=sgm.salesgroupid and sg.type>0
			       join permissions pm with(nolock) on pm.userid=sgm.userid
			       join libuser l with(nolock) on pm.userid=l.userid
			       join person p with(nolock) on p.personid=l.personid
		where (pm.SalesManager>@HasSalesMgrPriv or pm.GeneralAdmin>@HasGenAdminPriv or pm.SuperUser=1 or pm.Reseller=0)
	   		or ((p.email like '%@skillsoft.com' or p.email like '%@books24x7.com')
	     		  and (@email not like '%@skillsoft.com' and @email not like '%@books24x7.com'))    
			or (pm.reseller = 1 and #t.salesid not in (
														select b.userid  
														from salesgroup a with(nolock) 
														inner join salesgroupmember b with(nolock) 
														on a.salesgroupid = b.salesgroupid 
														where a.salesgroupid in(select salesgroupid 
																				from salesgroupmember 
																				where userid = @userid
																				)
														and a.type > 0
														) 
				and isnull(s.generalflags,0)&1048576=0 --global accounts
				)
		)


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
		       and (@email not like '%@skillsoft.com' and @email not like '%@books24x7.com')))
		   and sgm.userid not in (select distinct userid from salesgroupmember with(nolock) join salesgroup sg with(nolock) on sgm.salesgroupid=sg.salesgroupid and sg.type>0
						where userid<>@userid and sgm.salesgroupid in (select sgm.salesgroupid 
								from salesgroupmember sgm with(nolock) join salesgroup sg with(nolock) on sgm.salesgroupid=sg.salesgroupid and sg.type>0
								where sgm.userid=@userid)) )

end

--Do not show trials until they get processed
delete #t
from #t t
inner join trialrequest tr with(nolock)
on t.u = tr.subscriptionid
where tr.UpgradeRqstID =12

select 
	s.SubscriptionID,
--	'Name'=case when ltrim(rtrim(c.Name))='' then '-' else c.Name end,
	'Name'=(select case when ltrim(rtrim(c.Name))='' then '-' else c.Name end from company c with(nolock) where c.companyid=s.companyid),
	'CompanyOrDepartment'=case when ltrim(rtrim(s.CompanyOrDepartment))='' then '-' else s.CompanyOrDepartment end,
	s.Seats,
	s.Created,
	s.Starts,
		s.Expires,
	s.Type,
	s.Status,
	s.ResellerCode,
'Salesgroup'=case when s.ResellerCode is not null then(select sg.name from salesgroup sg where sg.resellercode=s.ResellerCode) else ' ' end,

	s.PasswordRoot,
	s.ApplicationName
	from Subscription s with(nolock) join #t on s.subscriptionid=#t.u 
--	join company c with(nolock) on s.companyid=c.companyid
	order by 'name'

drop table #t

return





GO
GRANT EXECUTE ON [dbo].[B24_GetSubscriptionLookup] TO [PolecatCustomers]
