use master
go

if exists(select * from sys.databases where name = 'BirdBlue')
	drop database BirdBlue
go

create database BirdBlue
go

use BirdBlue
go

create table BirdUser(
	id int primary key identity,
	username varchar(60),
	email varchar(60),
	pass varchar(60)
);
go

create table BirdBlueet(
	id int primary key identity,
	content varchar(140),
	likes int not null,
	postDate datetime,
	userId int references BirdUser(id),
);
go

create table Followers(
	id int primary key identity,
	userIdFollowing int references BirdUser(id),
	userIdFollowed int references BirdUser(id)
);
go

--drop table Followers

create table Likes(
	id int primary key identity,
	userId int references BirdUser(id),
	postId int references BirdBlueet(id)
);
go

--create clustered index indexLikes on Likes (userId, postId)

create procedure addUser 
	@username varchar(60),
	@email varchar(60),
	@pass varchar(60)
as
begin
	insert into BirdUser values 
		(@username, @email, CONVERT(VARCHAR(32), HashBytes('SHA2_256', @pass), 2))
end
go

--drop procedure addUser

create index login_index on BirdUser(username, email, pass) include (id)
go

create procedure signIn
	@login varchar(60),
	@pass varchar(60)
as
begin
	select id from BirdUser where (@login = username or @login = email)and CONVERT(VARCHAR(32), HashBytes('SHA2_256', @pass), 2) = pass 
end
go

create procedure createPost 
	@userId int,
	@content varchar(140)
as
begin
	insert into BirdBlueet values 
		(@content, 0, getdate(), @userId)
end
go

create index username_index on BirdUser(username)
go

create procedure searchUsers
	@username varchar(60)
as
begin
	select top 10 username from BirdUser where username like '%' + @username + '%' order by username
end
go

--drop procedure getTimeline


create procedure followUser
	@userIdFollowing int,
	@userIdFollowed int
as
begin
	insert into Followers values
		(@userIdFollowing, @userIdFollowed)
end
go

create procedure likeButton
	@userId int,
	@postId int
as
begin
	declare
	@likeId int
	select @likeId = ID from Likes where userId = @userId and postId = @postId

	if(@likeId is null)
		begin
			insert into Likes values
				(@userId, @postId)
		end
	else
		begin
			delete from Likes where id = @likeId
		end
end
go

create procedure getLikes
	@postId int
as
begin
	select likes from BirdBlueet where @postId = Id
	-- select count(id) as 'N° Likes' from Likes where @postId = postId
end
go

create index timeline_index on BirdBlueet(postDate, userId) include (content, id)
go

create index follower_index on Followers(userIdFollowing, userIdFollowed)
go

create procedure getTimeline
	@userId int
as
begin
	select top 20 b.id, b.content from BirdBlueet b 
	inner join Followers f on b.userId = f.userIdFollowed
	where f.userIdFollowing = @userId 
	order by b.postDate
end
go

create trigger LikeOnLike
on Likes
after insert
as begin
	declare
	@postId int
    select @postId = postId from INSERTED


	UPDATE BirdBlueet
	SET
	  likes = likes + 1
	WHERE
	  Id = @postId
	
end
go

create trigger dislikeOndislike
on Likes
after delete
as begin
	declare

	@postId int
    select @postId = postId from DELETED

	UPDATE BirdBlueet
	SET
	  likes = likes - 1
	WHERE
	  Id = @postId
	
end
go

exec addUser 'matueus', 'matueus@email.com', 'matueus123'
exec addUser 'rafale', 'rafale@email.com', 'rafale123'
exec addUser 'zeca', 'zeca@email.com', 'zeca123'
exec addUser 'maria', 'maria@email.com', 'maria123'
exec addUser 'mariana', 'mariana@email.com', 'mariana123'
exec addUser 'cripot', 'tigrinho@email.com', 'tigrinho123'
exec addUser 'supercao', 'batman@email.com', 'batman123'

select * from BirdUser

exec signIn 'supercao', 'batman123'

exec createPost 1, 'hoje ta frio'
exec createPost 3, 'hoje ta quente'
exec createPost 4, 'hoje ta muito frio'
exec createPost 5, 'hoje ta muito quente'
exec createPost 2, 'hoje ta um pouquinho frio'
exec createPost 1, 'hoje ta um pouqinho quente'
exec createPost 2, 'hoje ta legal'

select * from BirdBlueet

exec searchUsers 'ma'

exec followUser 1, 5
exec followUser 1, 4
exec followUser 1, 3

select * from Followers

exec likeButton 3, 1
exec likeButton 1, 1
exec likeButton 2, 1

select * from Likes

exec getLikes 1

exec getTimeline 1498
select * from BirdBlueet order by likes desc

exec likeButton 3, 1
select count(*) from BirdUser
select * from BirdUser
select * from BirdUser where username like '%Juan%'

--select * from sys.dm_os_wait_stats order by wait_time_ms desc