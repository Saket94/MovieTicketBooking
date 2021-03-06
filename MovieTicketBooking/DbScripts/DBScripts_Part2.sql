USE [CinemaTicketBookingDB]
GO
/****** Object:  Table [dbo].[tblBooking]    Script Date: 01/06/2020 10:06:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblBooking](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[seat_Number] [int] NULL,
	[cust_id] [int] NULL,
	[status] [bit] NULL,
	[passCode] [nchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblCustomer]    Script Date: 01/06/2020 10:06:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblCustomer](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSeat]    Script Date: 01/06/2020 10:06:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSeat](
	[Seat_Number] [int] IDENTITY(1,1) NOT NULL,
	[Status] [bit] NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_Seat] PRIMARY KEY CLUSTERED 
(
	[Seat_Number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[tblSeat] ON 

INSERT [dbo].[tblSeat] ([Seat_Number], [Status]) VALUES (1, 1)
INSERT [dbo].[tblSeat] ([Seat_Number], [Status]) VALUES (2, 1)
INSERT [dbo].[tblSeat] ([Seat_Number], [Status]) VALUES (3, 1)
INSERT [dbo].[tblSeat] ([Seat_Number], [Status]) VALUES (4, 1)
INSERT [dbo].[tblSeat] ([Seat_Number], [Status]) VALUES (5, 1)
INSERT [dbo].[tblSeat] ([Seat_Number], [Status]) VALUES (6, 1)
INSERT [dbo].[tblSeat] ([Seat_Number], [Status]) VALUES (7, 1)
INSERT [dbo].[tblSeat] ([Seat_Number], [Status]) VALUES (8, 1)
INSERT [dbo].[tblSeat] ([Seat_Number], [Status]) VALUES (9, 1)
INSERT [dbo].[tblSeat] ([Seat_Number], [Status]) VALUES (10, 1)
INSERT [dbo].[tblSeat] ([Seat_Number], [Status]) VALUES (11, 1)
INSERT [dbo].[tblSeat] ([Seat_Number], [Status]) VALUES (12, 1)
INSERT [dbo].[tblSeat] ([Seat_Number], [Status]) VALUES (13, 1)
INSERT [dbo].[tblSeat] ([Seat_Number], [Status]) VALUES (14, 1)
INSERT [dbo].[tblSeat] ([Seat_Number], [Status]) VALUES (15, 1)
INSERT [dbo].[tblSeat] ([Seat_Number], [Status]) VALUES (16, 1)
INSERT [dbo].[tblSeat] ([Seat_Number], [Status]) VALUES (17, 1)
INSERT [dbo].[tblSeat] ([Seat_Number], [Status]) VALUES (18, 1)
INSERT [dbo].[tblSeat] ([Seat_Number], [Status]) VALUES (19, 1)
INSERT [dbo].[tblSeat] ([Seat_Number], [Status]) VALUES (20, 1)
INSERT [dbo].[tblSeat] ([Seat_Number], [Status]) VALUES (21, 1)
INSERT [dbo].[tblSeat] ([Seat_Number], [Status]) VALUES (22, 1)
INSERT [dbo].[tblSeat] ([Seat_Number], [Status]) VALUES (23, 1)
INSERT [dbo].[tblSeat] ([Seat_Number], [Status]) VALUES (24, 1)
INSERT [dbo].[tblSeat] ([Seat_Number], [Status]) VALUES (25, 1)
INSERT [dbo].[tblSeat] ([Seat_Number], [Status]) VALUES (26, 1)
INSERT [dbo].[tblSeat] ([Seat_Number], [Status]) VALUES (27, 1)
INSERT [dbo].[tblSeat] ([Seat_Number], [Status]) VALUES (28, 1)
INSERT [dbo].[tblSeat] ([Seat_Number], [Status]) VALUES (29, 1)
INSERT [dbo].[tblSeat] ([Seat_Number], [Status]) VALUES (30, 1)
SET IDENTITY_INSERT [dbo].[tblSeat] OFF
ALTER TABLE [dbo].[tblSeat] ADD  DEFAULT ((0)) FOR [Status]
GO
ALTER TABLE [dbo].[tblBooking]  WITH CHECK ADD FOREIGN KEY([cust_id])
REFERENCES [dbo].[tblCustomer] ([id])
GO
ALTER TABLE [dbo].[tblBooking]  WITH CHECK ADD FOREIGN KEY([cust_id])
REFERENCES [dbo].[tblCustomer] ([id])
GO
ALTER TABLE [dbo].[tblBooking]  WITH CHECK ADD FOREIGN KEY([seat_Number])
REFERENCES [dbo].[tblSeat] ([Seat_Number])
GO
ALTER TABLE [dbo].[tblBooking]  WITH CHECK ADD FOREIGN KEY([seat_Number])
REFERENCES [dbo].[tblSeat] ([Seat_Number])
GO
