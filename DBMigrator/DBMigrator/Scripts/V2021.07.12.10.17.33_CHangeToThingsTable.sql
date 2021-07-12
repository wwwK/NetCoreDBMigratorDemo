/* ****************************************************************
 * Author: Chris
 * Description: CHangeToThingsTable
 * ****************************************************************
 */
ALTER TABLE Things
ADD CreatedAt DateTime DEFAULT GETDATE()
GO

/*Update existing records*/
UPDATE Things SET CreatedAt = GETDATE()

GO
/*Now make the CreatedAt a not null field*/
ALTER TABLE Things
ALTER COLUMN CreatedAt DateTime NOT NULL

GO

/*Seed some more things and we should see the CreatedAt default to SQL Server GETDATE()*/
SET IDENTITY_INSERT Things ON

INSERT INTO Things (Id, [Name]) VALUES(4,'Thing 4')
INSERT INTO Things (Id, [Name]) VALUES(5,'Thing 5')

SET IDENTITY_INSERT Things OFF