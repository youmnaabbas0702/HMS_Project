--declare procedure
create Procedure SP_AddNewDoctor
	@NationalNo nvarchar(20),
	@FullName nvarchar(200),
	@NationalityID int,
	@DateOfBirth date,
	@Gender bit,
	@Address nvarchar(100),
	@Phone nvarchar(20),
	@Email nvarchar(100),
	@PersonPicturePath nvarchar(255),
	@DepartmentID tinyint,
	@LicenseNumber nvarchar(100),
	@ExperienceYears tinyint,
	@DateJoined date,
	@IsActive bit,
	@NewPersonID int output,
	@NewDoctorID int output
as 
begin
	BEGIN TRANSACTION;

BEGIN TRY
	insert into People values
	(@NationalNo, @FullName, @NationalityID, @DateOfBirth,@Gender, @Address, @Phone, @Email,@PersonPicturePath);
	set @NewPersonID = SCOPE_IDENTITY();

	insert into Doctors values
	(@NewPersonID, @DepartmentID, @LicenseNumber, @ExperienceYears,@DateJoined, @IsActive);
	set @NewDoctorID = SCOPE_IDENTITY();

	COMMIT;
END TRY
BEGIN CATCH
	ROLLBACK;
END CATCH
end