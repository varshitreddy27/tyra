USE [bookdb]
GO
/****** Object:  StoredProcedure [dbo].[B24_GetCollections]    Script Date: 01/06/2011 14:24:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[B24_GetCollections]
@SubscriptionID uniqueidentifier,
@RqstUserID uniqueidentifier = null,
@ApplicationName varchar(32) = null,
@GetCounts int = 0

AS
set nocount on

declare @allguid uniqueidentifier
declare @curs cursor, @cstr varchar(255), @cname varchar(255), @cid uniqueidentifier

set @allguid  = 'FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF'

-- when requesterid is specified and the subid is ALLGUID


if @SubscriptionID is not null and @subscriptionid<>0x0 and @subscriptionid<>@allguid
begin
	declare @c table (collectionid uniqueidentifier, cnt int, hidden tinyint, name varchar(255), typeid int, capacity int, isprivate int, fullaccess int, 
			description varchar(255), created datetime, status int, alias varchar(255), isdefault int, seatcount int, accessexpires datetime,
			sortorder int,NativeLanguage varchar(64), Attributes varchar(500))

	declare @x table (collectionid uniqueidentifier, cnt int)

	if not exists (select * from subscription with(nolock) where subscriptionid=@subscriptionid) -- subid param is actually a userid
		select @subscriptionid=subscriptionid from libuser with(nolock) where userid=@subscriptionid


	if isnull(@GetCounts,0)=1
	begin
		declare @y table (cnt int, collectionstr varchar(4096))
		insert into @y
		select cnt=count(*),collectionstr from libuser l with(nolock) 
			where subscriptionid=@subscriptionid and l.subscriptionstatusid not in (5,6,8)
			group by collectionstr
	
		insert into @x
		select x.collectionid,sum(cnt) from @y y
			join (select cs.collectionid,c.name  from collectionsubscription cs with(nolock) 
			       join collection c with(nolock) on cs.collectionid=c.collectionid
			       where cs.subscriptionid=@subscriptionid and isnull(cs.allowedseatcount,0)<>-1 and c.status=1) AS X on ','+y.collectionstr+',' like '%,'+x.name+',%'
			       
		group by x.collectionid

		insert into @c -- get all the subs collections that have counted limits
			   (collectionid, cnt, hidden, name, typeid, capacity, isprivate, fullaccess, description, created, status, alias, isdefault, seatcount, accessexpires,sortorder,NativeLanguage, Attributes)
		select	c.collectionid,isnull(x.cnt,0),0,
			c.Name, c.CollectionTypeID,
			Capacity=case   when cs.allowedseatcount=-1 then -1
					when (cs.allowedseatcount - isnull(x.cnt,0))<0 then cs.allowedseatcount
					else isnull(x.cnt,0) end,
			c.private, c.FullAccess, c.Description, c.Created, c.Status, c.alias, cs.IsDefault, cs.AllowedSeatCount, cs.AccessExpires,c.sortorder,NativeLanguage,Attributes
		from collectionsubscription cs with(nolock)
			join collection c with(nolock) on c.collectionid=cs.collectionid
			left outer join @x x on x.collectionid=cs.collectionid
		where cs.subscriptionid=@subscriptionid
		and c.status=1
	end
	else
		insert into @c
			 (collectionid, cnt, hidden, name, typeid, capacity, isprivate, fullaccess, description, created, status, alias, isdefault, seatcount, accessexpires,sortorder,NativeLanguage,Attributes)
		select	cs.collectionid,0,0, -- get the counted ones, if any, and assume unlimited
			c.Name, c.CollectionTypeID,
			Capacity=case   when cs.allowedseatcount=-1 then -1 else 0 end,
			c.private, c.FullAccess, c.Description, c.Created, c.Status, c.alias, cs.IsDefault, cs.AllowedSeatCount, cs.AccessExpires,c.sortorder,NativeLanguage,Attributes
		from collectionsubscription cs with(nolock)
			join collection c with(nolock) on c.collectionid=cs.collectionid
			where cs.subscriptionid=@subscriptionid
			and c.status=1

	if exists (select * from @c where name like 'execbp-%')
	begin
		set @curs=cursor forward_only read_only for
			select name, collectionid
			from @c where name like 'execbp-%'
			order by sortorder,name
		open @curs
		fetch next from @curs into @cname, @cid
		while @@fetch_status=0
		begin
			if @cstr is null
				set @cstr=@cname
			else
				set @cstr=@cstr+','+@cname
			update @c set hidden=1 where collectionid=@cid
			fetch next from @curs into @cname, @cid
		end

		close @curs
		deallocate @curs

		insert into @c (collectionid, cnt, hidden, name, typeid, capacity, isprivate, fullaccess, description, created, status, alias, isdefault, seatcount, accessexpires,sortorder)
		values (null,0,0,@cstr,0,0,0,1, 'ExecBlueprints',null,0,null,0,0,null,null)
	end -- execbp hack

	if exists (select * from @c where name IN ('UCT','Special25652') )
	begin
		update @c set hidden=1 where collectionid in (select collectionid from collection where name in ('~UCT','UCT','Special25652','~Special25652') or typeid =7)
	end -- UCT hack

	select	
		CollectionID,
		CollectionName=Name,
		CollectionTypeID=typeid,
		Capacity,
		IsPrivate,
		FullAccess,
		Description,
		Created,
		Status,
		Alias,
		IsDefault,
		SeatCount,
		AccessExpires,
		sortorder,
		Hidden,
		NativeLanguage,
		Attributes
		from @c
		where typeid <> 7
		order by sortorder,Name
end

else
if @rqstuserid is not null and @rqstuserid<>0x0
begin

	declare @nullguid uniqueidentifier, @showall int
	set @nullguid = '{00000000-0000-0000-0000-000000000000}'
	
	set @showall=0
--	if exists(select * from permissions with(nolock) where userid=@rqstuserid and (GeneralAdmin=1 or SuperUser=1))
--		set @showall=1

	if @subscriptionid=@allguid
		set @showall=1

	if @showall=1 -- priv'd, sees all collections
		select	
			CollectionID,
			CollectionName=Name,
			CollectionTypeID,
			Capacity,
			IsPrivate=private,
			FullAccess,
			Description,
			Created,
			Status,
			Alias,
			IsDefault = 0,
			SeatCount = 0,
			AccessExpires = null,
			sortorder,
			Hidden=0,
			NativeLanguage,
			Attributes
			from Collection c with(nolock)
			where name not like '~%' and name not like 'coursebooks-%' and name<>'empty'
			    and private=0 and status=1
			order by c.Name
	else -- explicit collections specified
	begin
		create table #c  
		(CollectionID uniqueidentifier,CollectionName varchar(256), CollectionTypeID int,  
		Capacity int, IsPrivate int, FullAccess int, Description varchar(255),  
		Created datetime, Status int, Alias varchar(32), IsDefault int, SeatCount int,  
		AccessExpires datetime, sortorder int, Hidden int, NativeLanguage varchar(64), Attributes varchar(500))  
  
		insert into #c
			select	distinct
				c.CollectionID,
				CollectionName=c.Name,
				c.CollectionTypeID,
				c.Capacity,
				IsPrivate=c.private,
				c.FullAccess,
				c.Description,
				c.Created,
				c.Status,
				Alias,
				IsDefault = 0,
				SeatCount = 0,
				AccessExpires = null,
				c.sortorder,
				Hidden=0,
				NativeLanguage,
				Attributes
				from Collection c with(nolock), salesgroupcollections s with(nolock)
				where c.collectionid=s.collectionid
		 		  and (s.grouporuserid=@rqstuserid or
					   s.grouporuserid in (select sm.salesgroupid from salesgroupmember sm with(nolock)
								where sm.userid=@rqstuserid))
				  and c.name not like '~%' and c.name not like 'coursebooks-%' and c.name<>'empty'
				  and c.private=0 and c.status=1
				order by c.sortorder,c.Name

		update #c set description = dbo.B24_HTMLDecode(description)

		;with sqlcte( description)
		as (select description  from #c group by description having  count(*)>1 )

		update	#c set description = #c.description + ' (' + collectionname + ')'
		from #c, sqlcte t where #c.description =t.description 

		select	
				CollectionID,
				CollectionName,
				CollectionTypeID,
				Capacity,
				IsPrivate,
				FullAccess,
				Description,
				Created,
				Status,
				Alias,
				IsDefault, 
				SeatCount,
				AccessExpires,
				sortorder,
				Hidden,
				NativeLanguage,
				Attributes
				from #c
				where collectiontypeid <> 7
			drop table #c
	end
end
else
select	
	c.CollectionID,
	CollectionName=c.Name,
	c.CollectionTypeID,
	c.Capacity,
	IsPrivate=c.private,
	c.FullAccess,
	Description=case when @applicationname='ieee-cs' and c.name like 'partneritpro%' then 'Online Bookshelf Platinum' 
			when ca.description is not null then ca.description else c.Description end,
	c.Created,
	c.Status,
	Alias,
	ca.IsDefault,
	SeatCount = 0,
	AccessExpires = null,
	Hidden=0,
	NativeLanguage,
	Attributes
	from Collection c with(nolock), collectionapplication ca with(nolock)
	where c.collectionid=ca.collectionid and ca.applicationname=@applicationname
	    and c.name not like '~%' and c.name not like 'coursebooks-%' and c.name<>'empty'
	    and c.private=0 and c.status=1
	order by c.description,c.sortorder,c.Name
return 0



