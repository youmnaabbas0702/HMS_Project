CREATE PROCEDURE SP_FindUser
    @UserID INT
AS
BEGIN
    SELECT 
        PersonID,
        UserName,
        RoleID,
        IsActive
    FROM Users
    WHERE UserID = @UserID;
END
GO
