create view vw_Doctors as
SELECT Doctors.PersonID, Doctors.DoctorID, People.NationalNo, People.FullName, Countries.CountryName, People.DateOfBirth, case People.Gender when 1 then 'Male' else 'Female' end as Gender, People.Address, People.phone, People.Email, Doctors.LicenseNumber, Doctors.ExperienceYears, Departments.DepartmentName, 
             Doctors.DateJoined, case Doctors.IsActive when 1 then 'Yes' else 'No' end as IsActive
FROM   Doctors INNER JOIN
             Departments ON Doctors.DepartmentID = Departments.DepartmentID INNER JOIN
             People ON Doctors.PersonID = People.PersonID INNER JOIN
             Countries ON People.NationalityID = Countries.CountryID;
go

CREATE Procedure SP_GetAllDoctors
as
begin
   select * from vw_Doctors
end
go

exec SP_GetAllDoctors

ALTER VIEW vw_Doctors
AS
SELECT 
    D.DoctorID,
    D.PersonID,

    -- Person info
    P.NationalNo,
    P.FullName,
    P.DateOfBirth,
    P.Gender,
    P.Address,
    P.Phone,
    P.Email,

    -- Nationality (both ID + Name)
    P.NationalityID,
    C.CountryName,

    -- Doctor info
    D.DepartmentID,
    Dep.DepartmentName,
    D.LicenseNumber,
    D.ExperienceYears,
    D.DateJoined,
    D.IsActive
FROM Doctors D
INNER JOIN People P ON D.PersonID = P.PersonID
INNER JOIN Departments Dep ON D.DepartmentID = Dep.DepartmentID
INNER JOIN Countries C ON P.NationalityID = C.CountryID;
GO

ALTER PROCEDURE SP_GetAllDoctors
AS
BEGIN
    SELECT * FROM vw_Doctors;
END;
GO
