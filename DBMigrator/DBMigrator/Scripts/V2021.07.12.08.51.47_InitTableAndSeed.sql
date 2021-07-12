/* ****************************************************************
 * Author: Chris
 * Description: InitTableAndSeed
 * ****************************************************************
 */
IF EXISTS (SELECT * from sysobjects where name='dbo.Things')
	BEGIN
		DROP TABLE [dbo].[Things]
	END
GO

CREATE TABLE [dbo].[Things](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL
CONSTRAINT [PK_Versions] PRIMARY KEY CLUSTERED 
(
[Id] ASC
)) ON [PRIMARY]

/*Seed things*/
SET IDENTITY_INSERT Things ON

INSERT INTO Things (Id, [Name]) VALUES(1,'Thing 1')
INSERT INTO Things (Id, [Name]) VALUES(2,'Thing 2')
INSERT INTO Things (Id, [Name]) VALUES(3,'Thing 3')

SET IDENTITY_INSERT Things OFF