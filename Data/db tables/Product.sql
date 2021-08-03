create table Users (
	ID int identity (1,1),
	Name varchar(30) not null,
	Email varchar(30)  unique not null,
	Password varchar(20) not null
)
ALTER TABLE users add constraint pk_users primary key (ID);


create table Desktops (
	ID int identity (1,1),
	UserID int not null,
	Name varchar(20) unique not null,
)

ALTER TABLE Desktops add constraint pk_Desktops primary key (ID);
ALTER TABLE Desktops add constraint fk_Desktops_Users foreign key (UserID) references users (ID) ;


create table Boards (
	ID int identity (1,1),
	DesktopID int not null,
	Name varchar(20) unique not null,
)

ALTER TABLE Boards add constraint pk_Boards primary key (ID);
ALTER TABLE Boards add constraint fk_Boards_Desktops foreign key (DesktopID) references Desktops (ID) ;



create table Lists (
	ID int identity (1,1),
	BoardsID int not null,
	Title varchar(20) not null,
)

ALTER TABLE Lists add constraint pk_Lists primary key (ID);
ALTER TABLE Lists add constraint fk_Lists_Boards foreign key (BoardsID) references Boards (ID) ;

create table Cards (
	ID int identity (1,1),
	ListsID int not null,
	Title varchar(20) not null,
	Descripption varchar(MAX) not null,

)

ALTER TABLE Cards add constraint pk_Cards primary key (ID);
ALTER TABLE Cards add constraint fk_Cards_Lists foreign key (ListsID) references Lists (ID) ;

--drop table Cards
--drop table Lists
--drop table Boards
--drop table Desktops
--drop table users

