CREATE PROCEDURE SP_UpdateDoctor
    @DoctorID INT,
    @NationalNo NVARCHAR(20),
    @FullName NVARCHAR(200),
    @NationalityID INT,
    @DateOfBirth DATE,
    @Gender BIT,
    @Address NVARCHAR(100),
    @Phone NVARCHAR(20),
    @Email NVARCHAR(100),
    @PersonPicturePath NVARCHAR(255),
    @DepartmentID INT,
    @LicenseNumber NVARCHAR(100),
    @ExperienceYears TINYINT,
    @DateJoined DATE,
    @IsActive BIT
AS
BEGIN

    BEGIN TRANSACTION;

    BEGIN TRY
        DECLARE @PersonID INT;

        -- Find the PersonID associated with this Doctor
        SELECT @PersonID = PersonID
        FROM Doctors
        WHERE DoctorID = @DoctorID;

        IF @PersonID IS NULL
        BEGIN
            -- No matching doctor found
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Update People table
        UPDATE People
        SET
            NationalNo = @NationalNo,
            FullName = @FullName,
            NationalityID = @NationalityID,
            DateOfBirth = @DateOfBirth,
            Gender = @Gender,
            Address = @Address,
            Phone = @Phone,
            Email = @Email,
            PersonPicturePath = @PersonPicturePath
        WHERE PersonID = @PersonID;

        -- Update Doctors table
        UPDATE Doctors
        SET
            DepartmentID = @DepartmentID,
            LicenseNumber = @LicenseNumber,
            ExperienceYears = @ExperienceYears,
            DateJoined = @DateJoined,
            IsActive = @IsActive
        WHERE DoctorID = @DoctorID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW; -- Rethrows the error to the caller
    END CATCH
END
GO

