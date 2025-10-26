CREATE PROCEDURE SP_FindPersonByID
    @PersonID INT
AS
BEGIN
    SELECT 
        PersonID,
        NationalNo,
        FullName,
        NationalityID,
        DateOfBirth,
        Gender,
        Address,
        Phone,
        Email,
        PersonPicturePath
    FROM People
    WHERE PersonID = @PersonID;
END
GO

CREATE PROCEDURE SP_FindDoctorByID
    @DoctorID INT
AS
BEGIN
    SELECT 
        DoctorID,
        PersonID,
        DepartmentID,
        LicenseNumber,
        ExperienceYears,
        DateJoined,
        IsActive
    FROM Doctors
    WHERE DoctorID = @DoctorID;
END
GO
