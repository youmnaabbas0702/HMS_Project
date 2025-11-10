CREATE PROCEDURE SP_UpdateUser
    @UserID INT,
    @PersonID INT,
    @UserName NVARCHAR(100),
    @PasswordHash VARCHAR(64),
    @RoleID INT,
    @IsActive BIT,
    @IsUpdated BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE Users
        SET 
            PersonID = @PersonID,
            UserName = @UserName,
            PasswordHash = @PasswordHash,
            RoleID = @RoleID,
            IsActive = @IsActive
        WHERE UserID = @UserID;

        IF @@ROWCOUNT > 0
            SET @IsUpdated = 1; -- Successfully updated
        ELSE
            SET @IsUpdated = 0; -- No record found
    END TRY
    BEGIN CATCH
        SET @IsUpdated = 0; -- Error occurred
    END CATCH
END;
GO
