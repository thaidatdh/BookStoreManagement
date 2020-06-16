/*
VERSION: 1.0.0
DATE: 14/06/2020
*/
/*CREATE DATABASE*/
USE master;
DROP DATABASE IF EXISTS BSDB;
CREATE DATABASE BSDB;
USE BSDB;
/*CREATE TABLES*/
DROP TABLE IF EXISTS users;
CREATE TABLE users (
	user_id INT PRIMARY KEY IDENTITY (1, 1),
	first_name NVARCHAR(30),
	last_name NVARCHAR(30),
	dob NVARCHAR(8),
	address NVARCHAR(MAX),
	phone NVARCHAR(15),
	gender NVARCHAR(15) DEFAULT 'NOT_SPECIFY',
	email NVARCHAR(50),
	note NVARCHAR(MAX),
	photo_link NVARCHAR(MAX),
	user_type NVARCHAR(10) DEFAULT 'CUSTOMER',
	create_date DATETIME DEFAULT CURRENT_TIMESTAMP,
	created_by INT,
	updated_date DATETIME DEFAULT CURRENT_TIMESTAMP,
	updated_by INT
);

DROP TABLE IF EXISTS customer;
CREATE TABLE customer (
	user_id INT PRIMARY KEY,
	credit_card NVARCHAR(MAX),
	momo NVARCHAR(MAX),
	bank_number NVARCHAR(MAX),
	bank_name NVARCHAR(MAX)
);

DROP TABLE IF EXISTS staff;
CREATE TABLE staff (
	staff_id INT PRIMARY KEY IDENTITY(1,1),
	user_id INT NOT NULL,
	username NVARCHAR(50),
	password NVARCHAR(MAX),
	salary BIGINT DEFAULT 0,
	start_date NVARCHAR(8),
	end_date NVARCHAR(8),
	active BIT DEFAULT 1,
	create_date DATETIME DEFAULT CURRENT_TIMESTAMP,
	created_by INT,
	updated_date DATETIME DEFAULT CURRENT_TIMESTAMP,
	updated_by INT
);

SET IDENTITY_INSERT users ON;
INSERT INTO users(user_id, first_name, last_name, user_type) VALUES (1,'','Administrator','ADMIN');
INSERT INTO users(user_id, first_name, last_name, user_type) VALUES (2,'','GUEST','CUSTOMER');
SET IDENTITY_INSERT users OFF;

SET IDENTITY_INSERT staff ON;
INSERT INTO staff(staff_id, user_id, username, password, active) VALUES (1,1,'admin','12',1);
SET IDENTITY_INSERT staff OFF;

INSERT INTO customer(user_id) VALUES (1);

DROP TABLE IF EXISTS book;
CREATE TABLE book (
	book_id INT PRIMARY KEY IDENTITY(1,1),
	name NVARCHAR(MAX),
	barcode NVARCHAR(MAX),
	format NVARCHAR(255),
	size NVARCHAR(255),
	page NVARCHAR(10),
	description NVARCHAR(MAX),
	price BIGINT,
	remaining INT,
	location NVARCHAR(MAX),
	category_id NVARCHAR(MAX),
	author_id NVARCHAR(MAX),
	publisher_id INT,
	published_date NVARCHAR(8),
	provider_id INT,
	photo_link NVARCHAR(MAX),
	create_date DATETIME DEFAULT CURRENT_TIMESTAMP,
	created_by INT,
	updated_date DATETIME DEFAULT CURRENT_TIMESTAMP,
	updated_by INT
);

DROP TABLE IF EXISTS author;
CREATE TABLE author (
	author_id INT PRIMARY KEY IDENTITY(1,1),
	name NVARCHAR(MAX),
	note NVARCHAR(MAX),
	create_date DATETIME DEFAULT CURRENT_TIMESTAMP,
	created_by INT,
	updated_date DATETIME DEFAULT CURRENT_TIMESTAMP,
	updated_by INT
);

DROP TABLE IF EXISTS publisher;
CREATE TABLE publisher (
	author_id INT PRIMARY KEY IDENTITY(1,1),
	name NVARCHAR(MAX),
	address NVARCHAR(50),
	email NVARCHAR(50),
	contact NVARCHAR(15),
	note NVARCHAR(MAX),
	create_date DATETIME DEFAULT CURRENT_TIMESTAMP,
	created_by INT,
	updated_date DATETIME DEFAULT CURRENT_TIMESTAMP,
	updated_by INT
);

DROP TABLE IF EXISTS provider;
CREATE TABLE provider (
	author_id INT PRIMARY KEY IDENTITY(1,1),
	name NVARCHAR(MAX),
	address NVARCHAR(50),
	email NVARCHAR(50),
	contact NVARCHAR(15),
	note NVARCHAR(MAX),
	create_date DATETIME DEFAULT CURRENT_TIMESTAMP,
	created_by INT,
	updated_date DATETIME DEFAULT CURRENT_TIMESTAMP,
	updated_by INT
);

DROP TABLE IF EXISTS definition;
CREATE TABLE definition (
	definition_id INT PRIMARY KEY IDENTITY(1,1),
	definition_type INT,
	value_1 NVARCHAR(MAX),
	value_2 NVARCHAR(MAX),
	create_date DATETIME DEFAULT CURRENT_TIMESTAMP,
	created_by INT,
	updated_date DATETIME DEFAULT CURRENT_TIMESTAMP,
	updated_by INT
);
SET IDENTITY_INSERT definition ON;
INSERT INTO definition(definition_id, definition_type, value_1, value_2) VALUES (1,1,'STORE','BookStore');
INSERT INTO definition(definition_id, definition_type, value_1, value_2) VALUES (2,1,'ADDRESS','');
INSERT INTO definition(definition_id, definition_type, value_1, value_2) VALUES (3,1,'CONTACT','');
INSERT INTO definition(definition_id, definition_type, value_1, value_2) VALUES (4,2,'ADMIN','ALL');
SET IDENTITY_INSERT definition OFF;

DROP TABLE IF EXISTS discount;
CREATE TABLE discount (
	discount_id INT PRIMARY KEY IDENTITY(1,1),
	description NVARCHAR(MAX),
	code NVARCHAR(20),
	percentage REAL,
	amount BIGINT,
	type NVARCHAR(10),
	start_date NVARCHAR(8),
	end_date NVARCHAR(8),
	create_date DATETIME DEFAULT CURRENT_TIMESTAMP,
	created_by INT,
	updated_date DATETIME DEFAULT CURRENT_TIMESTAMP,
	updated_by INT
);

DROP TABLE IF EXISTS transactions;
CREATE TABLE transactions (
	transaction_id INT PRIMARY KEY IDENTITY(1,1),
	customer_id INT,
	provider_id INT,
	staff_id INT,
	amount BIGINT,
	discount BIGINT,
	entry_date NVARCHAR(8),
	type NVARCHAR(8),
	create_date DATETIME DEFAULT CURRENT_TIMESTAMP,
	created_by INT,
	updated_date DATETIME DEFAULT CURRENT_TIMESTAMP,
	updated_by INT
);

DROP TABLE IF EXISTS transaction_detail;
CREATE TABLE transaction_detail (
	transaction_detail_id INT PRIMARY KEY IDENTITY(1,1),
	transaction_id INT,
	book_id INT,
	price BIGINT,
	amount INT,
	discount BIGINT,
	discount_id INT
);