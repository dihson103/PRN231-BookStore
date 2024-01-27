CREATE DATABASE Assignment2_PRN231

CREATE TABLE [Role]
(
	role_id		INT PRIMARY KEY IDENTITY(1, 1),
	role_desc	VARCHAR(100)
)

CREATE TABLE Publisher
(
	pub_id			INT PRIMARY KEY IDENTITY(1, 1),
	publisher_name	NVARCHAR(100),
	city			NVARCHAR(100),
	[state]			NVARCHAR(100),
	country			NVARCHAR(100)
)

CREATE TABLE [User]
(
	[user_id]		INT PRIMARY KEY IDENTITY(1, 1),
	role_id			INT FOREIGN KEY REFERENCES [Role](role_id),
	pub_id			INT FOREIGN KEY REFERENCES Publisher(pub_id),
	[source]		NVARCHAR(MAX),
	email_address	VARCHAR(100),
	[password]		VARCHAR(100),
	first_name		NVARCHAR(100),
	middle_name		NVARCHAR(100),
	last_name		NVARCHAR(100),
	hire_date		DATETIME
)

CREATE TABLE Book
(
	book_id			INT PRIMARY KEY IDENTITY(1, 1),
	pub_id			INT FOREIGN KEY REFERENCES Publisher(pub_id),
	title			NVARCHAR(MAX),
	advance			NVARCHAR(100),
	royalty			FLOAT,
	ytd_sales		FLOAT,
	notes			NVARCHAR(MAX),
	published_date	DATETIME
)

CREATE TABLE Author
(
	author_id		INT PRIMARY KEY IDENTITY(1, 1),
	last_name		NVARCHAR(100),
	first_name		NVARCHAR(100),
	phone			VARCHAR(20),
	[address]		NVARCHAR(100),
	city			NVARCHAR(100),
	[state]			NVARCHAR(100),
	zip				INT,
	email_address	VARCHAR(100)
)

CREATE TABLE BookAuthor
(
	author_id			INT REFERENCES Author(author_id),
	book_id				INT REFERENCES Book(book_id),
	author_order		INT,
	royality_percentage	FLOAT,
	PRIMARY KEY(author_id, book_id)
)