/* ****************************************************************
 * Author: Chris
 * Description: Sproc
 * ****************************************************************
 */
DROP PROCEDURE IF EXISTS dbo.pr_things_select
GO

CREATE PROCEDURE dbo.pr_things_select
@Id INT  = NULL
AS
BEGIN
	SELECT * FROM Things
	WHERE (@Id IS NULL OR Id = @Id)
END
