use HMS_v1

create table People
(
PersonID int primary key identity,
NationalNo nvarchar(100),
FullName nvarchar(200),
DateOfBirth Date,
Gender bit,
Address nvarchar(100),
phone nvarchar(20),
Email nvarchar(100),
PersonPicturePath nvarchar(255)
)

create table Roles
(
RoleID int primary key identity,
RoleName nvarchar(50),
Description nvarchar(100)
)

create table Users
(
UserID int primary key identity,
PersonID int foreign key references People,
UserName nvarchar(100),
PasswordHash varchar(64),
RoleID int foreign key references Roles,
IsActive bit
)

create table Departments
(
DepartmentID int primary key identity,
DepartmentName nvarchar(50),
Description nvarchar(100)
)

create table Doctors
(
DoctorID int primary key identity,
PersonID int foreign key references People,
DepartmentID int foreign key references Departments,
LicenseNumber nvarchar(100) unique null,
ExperienceYears tinyint,
DateJoined Date,
IsActive bit
)

create table InsuranceProviders
(
ProviderID int identity primary key,
ProviderName nvarchar(100) unique,
Phone nvarchar(20),
Email nvarchar(50),
Notes nvarchar(100)
)

create table Patients
(
PatientID int primary key identity,
PersonID int foreign key references People,
EmergencyContact nvarchar(100),
InsuranceProviderID int foreign key references InsuranceProviders,
InsurancePercentage DECIMAL(5,2) CHECK (InsurancePercentage BETWEEN 0 AND 100)
)

create table ChronicConditions
(
ConditionID int primary key identity,
PatientID int foreign key references Patients,
DiseaseName nvarchar(100),
Status tinyint,
Severity tinyint,
Notes nvarchar(255),
LastUpdated Date
)

create table AllergyConditions
(
AllergyConditionID int primary key identity,
PatientID int foreign key references Patients,
Allergen nvarchar(50),
Severity tinyint,
RecordedDate Date
)

create table StaffSchedules
(
ScheduleID int primary key identity,
PersonID int foreign key references People,
ShiftName nvarchar(50),
DayInWeek tinyint,
StartTime time,
EndTime time,
IsActive bit,
CreatedBy int references Users,
CreatedAt Date
)

create Table Nurses
(
NurseID int primary key identity,
PersonID int foreign key references People,
DepartmentID int references Departments,
YearsOfExperience tinyint,
DateJoined Date,
IsActive bit
)

create table Pharmacist
(
PharmacistID int primary key identity,
YearsOfExperience tinyint,
DateJoined Date,
IsActive bit
)

create table Appointments
(
AppointmentID int identity primary key,
PatientID int foreign key references Patients,
DoctorID int foreign key references Doctors,
CreatedDate Date,
AppointmentDate Date,
StartTime time,
EndTime time,
Status tinyint,
LastStatusDate Date,
Fees Decimal (8,2),
Notes nvarchar(100)
)

create table MedicalVisits
(
VisitID int primary key identity,
AppointmentID int references Appointments,
VisitDate Date,
StartTime time,
EndTime time,
Status tinyint
)

CREATE TABLE Vitals (
    VitalID INT PRIMARY KEY IDENTITY,
    VisitID INT FOREIGN KEY REFERENCES MedicalVisits(VisitID),
    NurseID INT FOREIGN KEY REFERENCES Nurses,
    BP_Systolic INT,          -- upper blood pressure
    BP_Diastolic INT,         -- lower blood pressure
    HeartRate INT,            -- beats per minute
    RespiratoryRate INT,      -- breaths per minute
    Temperature DECIMAL(4,1), -- body temp °C
    SpO2 INT,                 -- oxygen saturation %
    Weight DECIMAL(5,2),      -- kg
    Height DECIMAL(5,2),      -- cm
    BMI AS (CASE WHEN Height > 0 THEN (Weight / POWER(Height/100.0, 2)) END) PERSISTED,
    Notes NVARCHAR(500),
    RecordedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE Prescriptions (
    PrescriptionID INT PRIMARY KEY IDENTITY,
    VisitID INT FOREIGN KEY REFERENCES MedicalVisits(VisitID),
    DoctorID INT FOREIGN KEY REFERENCES Users(UserID),
    PharmacistID INT NULL FOREIGN KEY REFERENCES Pharmacist, -- who dispensed
    PrescriptionDate DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(20) DEFAULT 'Pending',  -- Pending / Dispensed / Cancelled
    DispensedDate DATETIME NULL,            -- when it was filled
    Notes NVARCHAR(500)
);

CREATE TABLE Drugs (
    DrugID INT PRIMARY KEY IDENTITY,
    DrugName NVARCHAR(150) NOT NULL,
    GenericName NVARCHAR(150) NULL,
    Category NVARCHAR(100) NULL,        -- e.g. Antibiotic, Analgesic
    Form NVARCHAR(50) NULL,             -- Tablet, Syrup, Injection
    Strength NVARCHAR(50) NULL,         -- e.g. 500mg
    Manufacturer NVARCHAR(100) NULL,
    IsActive BIT DEFAULT 1
);

CREATE TABLE PrescriptionDrugs (
    PrescriptionDrugID INT PRIMARY KEY IDENTITY,
    PrescriptionID INT FOREIGN KEY REFERENCES Prescriptions(PrescriptionID),
    DrugID INT FOREIGN KEY REFERENCES Drugs(DrugID),
    Dosage NVARCHAR(50) NULL,        -- e.g. 1 Tablet
    Frequency NVARCHAR(50) NULL,     -- e.g. Twice a day
    Duration NVARCHAR(50) NULL,      -- e.g. 5 days
    Instructions NVARCHAR(200) NULL  -- e.g. After meals
);

create Table LabTechnecians
(
TechnecianID int primary key identity,
PersonID int references People,
DepartmentID int references Departments,
YearsOfExperience tinyint,
DateJoined Date,
IsActive bit
)

create table TestOrders
(
TestOrderID int primary key identity,
VisitID int references MedicalVisits,
TestType nvarchar(50),
Status tinyint,
PreConditions nvarchar(100),
RequestedDate DateTime,
LastStatusDate DateTime,
DoneByTechnecianID int references LabTechnecians,
Result nvarchar(255)
)

create table Payments
(
PaymentID int primary key identity,
VisitID int references MedicalVisits,
TotalAmount Decimal(8,2),
InsuranceCoveragePercentage Decimal(5,2) CHECK (InsuranceCoveragePercentage BETWEEN 0 AND 100),
NetAmount as (TotalAmount - (TotalAmount * InsuranceCoveragePercentage / 100)) Persisted,
Status tinyint,
PaymentMethod nvarchar(20),
PaymentDate DateTime
)

create table Claims
(
ClaimID int primary key identity,
PaymentID int references Payments,
ClaimAmount Decimal(8,2),
InsuranceProviderID int references InsuranceProviders,
Status tinyint
)

CREATE TABLE Midications (
    DrugID INT PRIMARY KEY IDENTITY,
    MedicineName NVARCHAR(150),
    Composition NVARCHAR(200),
    Uses NVARCHAR(300),
    SideEffects NVARCHAR(300),
    ImageURL NVARCHAR(300),
    Manufacturer NVARCHAR(150),
    ExcellentReviewPercentage DECIMAL(5,2),
    AverageReviewPercentage DECIMAL(5,2),
    PoorReviewPercentage DECIMAL(5,2)
);
