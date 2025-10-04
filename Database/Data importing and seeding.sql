INSERT INTO Roles (RoleName, Description)
VALUES
('Admin', 'Full access to all modules and settings'),
('Doctor', 'Can manage visits, prescriptions, and patient records'),
('Nurse', 'Can record vitals and preliminary observations'),
('Receptionist', 'Can manage appointments and patient check-ins'),
('Pharmacist', 'Can view and dispense prescriptions'),
('HR', 'Can manage staff records and schedules'),
('LabTechnician', 'Can handle lab tests and results');

INSERT INTO Departments (DepartmentName, Description)
VALUES
('General Medicine', 'General outpatient and internal medicine'),
('Cardiology', 'Heart and vascular diseases'),
('Neurology', 'Brain and nervous system'),
('Orthopedics', 'Bones, muscles, and joint care'),
('Pediatrics', 'Children and infants health'),
('Gynecology', 'Women’s reproductive health'),
('Laboratory', 'Diagnostic and lab testing unit'),
('Pharmacy', 'Medication management and dispensing'),
('Radiology', 'Imaging and scanning department'),
('Administration', 'HR and hospital management');


INSERT INTO InsuranceProviders (ProviderName, Phone, Email, Notes)
VALUES
('AXA Health', '01001234567', 'support@axa.com', 'Covers general and chronic cases'),
('MedCare Insurance', '01122334455', 'info@medcare.com', 'Requires prior approval for surgery'),
('Nile Life', '01298765432', 'claims@nilelife.com', 'Covers diagnostics and lab tests'),
('Global Health Co.', '01099887766', 'contact@globalhealth.com', 'International policy support'),
('SecurePlus', '01144556677', 'help@secureplus.com', 'Basic outpatient and emergency coverage');


SELECT 
    IDENTITY(INT, 1, 1) AS DrugID, 
    *
INTO Drugs
FROM medicine_dataset

ALTER TABLE Drugs
ADD CONSTRAINT PK_Drugs PRIMARY KEY (DrugID);

drop medicine_dataset

select * from Drugs

ALTER TABLE PrescriptionDrugs
ADD CONSTRAINT FK_PrescriptionDrugs_Drugs
FOREIGN KEY (DrugID) REFERENCES Drugs(DrugID);
