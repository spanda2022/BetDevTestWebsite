USE [BetDevTest]
GO

/****** Object:  Table [dbo].[Products]    Script Date: 2022/09/03 15:42:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Products](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[Price] [money] NULL,
	[Quantity] [int] NULL,
	[IsActive] [bit] NULL,
	[Image] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Products] ADD  DEFAULT ((0)) FOR [Quantity]
GO

ALTER TABLE [dbo].[Products] ADD  DEFAULT ((1)) FOR [IsActive]
GO

USE [BetDevTest]
GO

/****** Object:  Table [dbo].[Cart]    Script Date: 2022/09/03 15:42:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Cart](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CardNo] [varchar](50) NULL,
	[UserEmail] [varchar](50) NULL,
	[TotalCartPrice] [money] NULL,
	[IsActive] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Cart] ADD  DEFAULT ((1)) FOR [IsActive]
GO


USE [BetDevTest]
GO

/****** Object:  Table [dbo].[OrderHistory]    Script Date: 2022/09/03 15:45:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OrderHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderNo] [varchar](50) NULL,
	[OrderDate] [datetime] NULL,
	[UserEmail] [varchar](50) NULL,
	[CartId] [int] NOT NULL,
	[TotalPrice] [money] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[OrderHistory]  WITH CHECK ADD  CONSTRAINT [FK_OrderHistory_ToActiveCart] FOREIGN KEY([CartId])
REFERENCES [dbo].[Cart] ([Id])
GO

ALTER TABLE [dbo].[OrderHistory] CHECK CONSTRAINT [FK_OrderHistory_ToActiveCart]
GO


USE [BetDevTest]
GO

/****** Object:  Table [dbo].[CartProducts]    Script Date: 2022/09/03 15:47:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CartProducts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CartId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Qty] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CartProducts]  WITH CHECK ADD  CONSTRAINT [FK_CartProducts_ToActiveCart] FOREIGN KEY([CartId])
REFERENCES [dbo].[Cart] ([Id])
GO

ALTER TABLE [dbo].[CartProducts] CHECK CONSTRAINT [FK_CartProducts_ToActiveCart]
GO

ALTER TABLE [dbo].[CartProducts]  WITH CHECK ADD  CONSTRAINT [FK_CartProducts_ToProducts] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
GO

ALTER TABLE [dbo].[CartProducts] CHECK CONSTRAINT [FK_CartProducts_ToProducts]
GO


