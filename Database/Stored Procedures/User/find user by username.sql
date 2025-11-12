CREATE PROCEDURE SP_FindUserByUserName
    @UserName nvarchar(100)
AS
BEGIN
    SELECT 
        PersonID,
        UserID,
        RoleID,
        IsActive
    FROM Users
    WHERE UserName = @UserName;
END
GO
