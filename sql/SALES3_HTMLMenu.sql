SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

ALTER   PROCEDURE SALES3_HTMLMenu
@userid uniqueidentifier = null
AS

set nocount on

declare @nullguid uniqueidentifier
set @nullguid = 0x0

declare @name varchar(64)
declare @login varchar(96),@app varchar(50)

select @name = isnull(p.firstname,'')+' '+isnull(p.lastname, ''), @login=l.login,@app=s.applicationname
from libuser l with(nolock), person p with(nolock),subscription s with(nolock) 
where l.personid = p.personid and l.userid=@userid and s.subscriptionid = l.subscriptionid

/*------------------------------------------------------
Reseller coded tasks
------------------------------------------------------*/
if @app in('microsofteref','microsofteref2','microsofteref3') --do not show for microsoft P.C. 01-11-2008
	begin
		return 0
	end

select 'SalesGroups'=''
-- user's resellercodes only ---
select 'SalesGroup'= sg.resellercode, '@resellercode'=sg.resellercode
from salesgroup sg with(nolock), salesgroupmember sm with(nolock), libuser u with(nolock)
where sg.salesgroupid=sm.salesgroupid and sm.userid=u.userid
and u.userid = @userid
and sg.status>0 and sg.type>0
and sg.resellercode is not null
and sg.flags & 2 = 0	-- not disabled
union
-- all resellercodes for admins --
select distinct 'SalesGroup'= sg.resellercode,  '@resellercode'=sg.resellercode
from salesgroup sg with(nolock), permissions pm with(nolock), libuser u with(nolock)
where pm.userid=u.userid and pm.superuser=1
and u.userid = @userid
and sg.status>0 and sg.type>0
and sg.resellercode is not null
and sg.flags & 2 = 0	-- not disabled

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[SALES3_HTMLMenu]  TO [PolecatCustomers]
GO

