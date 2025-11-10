CREATE PROCEDURE SP_AddNewUser
    @PersonID INT,
    @UserName NVARCHAR(100),
    @PasswordHash VARCHAR(64),
    @RoleID INT,
    @IsActive BIT,
    @NewUserID INT OUTPUT
AS
BEGIN
    -- We use SET NOCOUNT ON to prevent extra "rows affected" messages 
    -- from interfering with output parameters or scalar results.
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO Users (PersonID, UserName, PasswordHash, RoleID, IsActive)
        VALUES (@PersonID, @UserName, @PasswordHash, @RoleID, @IsActive);

        -- Return the newly created UserID
        SET @NewUserID = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        -- If an error happens, set output to -1
        SET @NewUserID = -1;
    END CATCH
END;
GO

DECLARE @NewUserID INT;

EXEC SP_AddNewUser
    @PersonID = 3,
    @UserName = N'Walaa123',
    @PasswordHash = 'ABC123XYZ456HASH',
    @RoleID = 2,
    @IsActive = 1,
    @NewUserID = @NewUserID OUTPUT;

SELECT @NewUserID AS NewUserID;

