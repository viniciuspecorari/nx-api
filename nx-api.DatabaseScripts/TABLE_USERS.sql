DROP TABLE [dbo].[Users]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users](
	[Id] [uniqueidentifier] NULL,
	[Name] [nvarchar](150) NULL,
	[Email] [nvarchar](100) NULL,
	[Password] [nvarchar](100) NULL,
	[CreatedAt] [datetime] NULL
) ON [PRIMARY]
GO


