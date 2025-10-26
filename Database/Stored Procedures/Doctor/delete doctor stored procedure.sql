CREATE PROCEDURE SP_DeleteDoctor_ByPersonID
	@PersonID INT
AS
BEGIN
	BEGIN TRANSACTION;
	BEGIN TRY
		-- Delete from Doctors first (FK constraint)
		DELETE FROM Doctors WHERE PersonID = @PersonID;

		-- Delete from People
		DELETE FROM People WHERE PersonID = @PersonID;

		COMMIT;
	END TRY
	BEGIN CATCH
		ROLLBACK;
	END CATCH
END;
go

CREATE PROCEDURE SP_DeleteDoctor_ByDoctorID
	@DoctorID INT
AS
BEGIN
	BEGIN TRANSACTION;
	BEGIN TRY
		DECLARE @PersonID INT;

		-- Find linked PersonID
		SELECT @PersonID = PersonID FROM Doctors WHERE DoctorID = @DoctorID;

		-- Delete Doctor
		DELETE FROM Doctors WHERE DoctorID = @DoctorID;

		-- Delete linked Person
		DELETE FROM People WHERE PersonID = @PersonID;

		COMMIT;
	END TRY
	BEGIN CATCH
		ROLLBACK;
	END CATCH
END;
go

CREATE PROCEDURE SP_DeactivateDoctor
	@DoctorID INT
AS
BEGIN
	UPDATE Doctors
	SET IsActive = 0
	WHERE DoctorID = @DoctorID;
END;

