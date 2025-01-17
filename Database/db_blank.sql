USE [master]
GO
/****** Object:  Database [BSDB]    Script Date: 8/14/2020 7:58:10 PM ******/
CREATE DATABASE [BSDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BSDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\BSDB.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BSDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\BSDB_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BSDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BSDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BSDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BSDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BSDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BSDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [BSDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [BSDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BSDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BSDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BSDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BSDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BSDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BSDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BSDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BSDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [BSDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BSDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BSDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BSDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BSDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BSDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [BSDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BSDB] SET RECOVERY FULL 
GO
ALTER DATABASE [BSDB] SET  MULTI_USER 
GO
ALTER DATABASE [BSDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BSDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BSDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BSDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [BSDB] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'BSDB', N'ON'
GO
ALTER DATABASE [BSDB] SET QUERY_STORE = OFF
GO
USE [BSDB]
GO
/****** Object:  Table [dbo].[author]    Script Date: 8/14/2020 7:58:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[author](
	[author_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](max) NULL,
	[note] [nvarchar](max) NULL,
	[is_deleted] [bit] NULL,
	[create_date] [datetime] NULL,
	[created_by] [int] NULL,
	[updated_date] [datetime] NULL,
	[updated_by] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[author_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[book]    Script Date: 8/14/2020 7:58:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[book](
	[book_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](max) NULL,
	[barcode] [nvarchar](max) NULL,
	[format] [nvarchar](255) NULL,
	[size] [nvarchar](255) NULL,
	[page] [nvarchar](10) NULL,
	[description] [nvarchar](max) NULL,
	[price] [bigint] NULL,
	[remaining] [int] NULL,
	[location] [nvarchar](max) NULL,
	[category_id] [nvarchar](max) NULL,
	[author_id] [nvarchar](max) NULL,
	[publisher_id] [int] NULL,
	[published_date] [nvarchar](8) NULL,
	[provider_id] [int] NULL,
	[photo_link] [nvarchar](max) NULL,
	[is_deleted] [bit] NULL,
	[create_date] [datetime] NULL,
	[created_by] [int] NULL,
	[updated_date] [datetime] NULL,
	[updated_by] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[book_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[customer]    Script Date: 8/14/2020 7:58:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[customer](
	[user_id] [int] NOT NULL,
	[credit_card] [nvarchar](max) NULL,
	[momo] [nvarchar](max) NULL,
	[bank_number] [nvarchar](max) NULL,
	[bank_name] [nvarchar](max) NULL,
	[point] [int] NULL,
	[is_deleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[definition]    Script Date: 8/14/2020 7:58:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[definition](
	[definition_id] [int] IDENTITY(1,1) NOT NULL,
	[definition_type] [int] NULL,
	[value_1] [nvarchar](max) NULL,
	[value_2] [nvarchar](max) NULL,
	[create_date] [datetime] NULL,
	[created_by] [int] NULL,
	[updated_date] [datetime] NULL,
	[updated_by] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[definition_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[discount]    Script Date: 8/14/2020 7:58:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[discount](
	[discount_id] [int] IDENTITY(1,1) NOT NULL,
	[description] [nvarchar](max) NULL,
	[code] [nvarchar](20) NULL,
	[percentage] [real] NULL,
	[amount] [bigint] NULL,
	[type] [nvarchar](10) NULL,
	[start_date] [nvarchar](8) NULL,
	[end_date] [nvarchar](8) NULL,
	[create_date] [datetime] NULL,
	[created_by] [int] NULL,
	[updated_date] [datetime] NULL,
	[updated_by] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[discount_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[provider]    Script Date: 8/14/2020 7:58:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[provider](
	[provider_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](max) NULL,
	[address] [nvarchar](50) NULL,
	[email] [nvarchar](50) NULL,
	[contact] [nvarchar](15) NULL,
	[note] [nvarchar](max) NULL,
	[is_deleted] [bit] NULL,
	[create_date] [datetime] NULL,
	[created_by] [int] NULL,
	[updated_date] [datetime] NULL,
	[updated_by] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[provider_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[publisher]    Script Date: 8/14/2020 7:58:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[publisher](
	[publisher_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](max) NULL,
	[address] [nvarchar](50) NULL,
	[email] [nvarchar](50) NULL,
	[contact] [nvarchar](15) NULL,
	[note] [nvarchar](max) NULL,
	[is_deleted] [bit] NULL,
	[create_date] [datetime] NULL,
	[created_by] [int] NULL,
	[updated_date] [datetime] NULL,
	[updated_by] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[publisher_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[staff]    Script Date: 8/14/2020 7:58:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[staff](
	[staff_id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[username] [nvarchar](50) NULL,
	[password] [nvarchar](max) NULL,
	[salary] [bigint] NULL,
	[start_date] [nvarchar](8) NULL,
	[end_date] [nvarchar](8) NULL,
	[active] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[staff_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[transaction_detail]    Script Date: 8/14/2020 7:58:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[transaction_detail](
	[transaction_detail_id] [int] IDENTITY(1,1) NOT NULL,
	[transaction_id] [int] NULL,
	[book_id] [int] NULL,
	[price] [bigint] NULL,
	[amount] [int] NULL,
	[discount] [bigint] NULL,
	[discount_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[transaction_detail_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[transactions]    Script Date: 8/14/2020 7:58:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[transactions](
	[transaction_id] [int] IDENTITY(1,1) NOT NULL,
	[customer_id] [int] NULL,
	[provider_id] [int] NULL,
	[staff_id] [int] NULL,
	[amount] [bigint] NULL,
	[discount] [bigint] NULL,
	[entry_date] [nvarchar](8) NULL,
	[type] [nvarchar](8) NULL,
	[is_deleted] [bit] NULL,
	[create_date] [datetime] NULL,
	[created_by] [int] NULL,
	[updated_date] [datetime] NULL,
	[updated_by] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[transaction_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[users]    Script Date: 8/14/2020 7:58:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[user_id] [int] IDENTITY(1,1) NOT NULL,
	[first_name] [nvarchar](30) NULL,
	[last_name] [nvarchar](30) NULL,
	[dob] [nvarchar](8) NULL,
	[address] [nvarchar](max) NULL,
	[phone] [nvarchar](15) NULL,
	[gender] [nvarchar](15) NULL,
	[email] [nvarchar](50) NULL,
	[note] [nvarchar](max) NULL,
	[photo_link] [nvarchar](max) NULL,
	[user_type] [nvarchar](10) NULL,
	[create_date] [datetime] NULL,
	[created_by] [int] NULL,
	[updated_date] [datetime] NULL,
	[updated_by] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[author] ADD  DEFAULT ((0)) FOR [is_deleted]
GO
ALTER TABLE [dbo].[author] ADD  DEFAULT (getdate()) FOR [create_date]
GO
ALTER TABLE [dbo].[author] ADD  DEFAULT (getdate()) FOR [updated_date]
GO
ALTER TABLE [dbo].[book] ADD  DEFAULT ((0)) FOR [is_deleted]
GO
ALTER TABLE [dbo].[book] ADD  DEFAULT (getdate()) FOR [create_date]
GO
ALTER TABLE [dbo].[book] ADD  DEFAULT (getdate()) FOR [updated_date]
GO
ALTER TABLE [dbo].[customer] ADD  DEFAULT ((0)) FOR [point]
GO
ALTER TABLE [dbo].[customer] ADD  DEFAULT ((0)) FOR [is_deleted]
GO
ALTER TABLE [dbo].[definition] ADD  DEFAULT (getdate()) FOR [create_date]
GO
ALTER TABLE [dbo].[definition] ADD  DEFAULT (getdate()) FOR [updated_date]
GO
ALTER TABLE [dbo].[discount] ADD  DEFAULT (getdate()) FOR [create_date]
GO
ALTER TABLE [dbo].[discount] ADD  DEFAULT (getdate()) FOR [updated_date]
GO
ALTER TABLE [dbo].[provider] ADD  DEFAULT ((0)) FOR [is_deleted]
GO
ALTER TABLE [dbo].[provider] ADD  DEFAULT (getdate()) FOR [create_date]
GO
ALTER TABLE [dbo].[provider] ADD  DEFAULT (getdate()) FOR [updated_date]
GO
ALTER TABLE [dbo].[publisher] ADD  DEFAULT ((0)) FOR [is_deleted]
GO
ALTER TABLE [dbo].[publisher] ADD  DEFAULT (getdate()) FOR [create_date]
GO
ALTER TABLE [dbo].[publisher] ADD  DEFAULT (getdate()) FOR [updated_date]
GO
ALTER TABLE [dbo].[staff] ADD  DEFAULT ((0)) FOR [salary]
GO
ALTER TABLE [dbo].[staff] ADD  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[transactions] ADD  DEFAULT ((0)) FOR [is_deleted]
GO
ALTER TABLE [dbo].[transactions] ADD  DEFAULT (getdate()) FOR [create_date]
GO
ALTER TABLE [dbo].[transactions] ADD  DEFAULT (getdate()) FOR [updated_date]
GO
ALTER TABLE [dbo].[users] ADD  DEFAULT ('NOT_SPECIFY') FOR [gender]
GO
ALTER TABLE [dbo].[users] ADD  DEFAULT ('CUSTOMER') FOR [user_type]
GO
ALTER TABLE [dbo].[users] ADD  DEFAULT (getdate()) FOR [create_date]
GO
ALTER TABLE [dbo].[users] ADD  DEFAULT (getdate()) FOR [updated_date]
GO
USE [master]
GO
ALTER DATABASE [BSDB] SET  READ_WRITE 
GO
