# 🏥 HMS – Healthcare Management System

## 📘 Overview
The **Healthcare Management System (HMS)** is a desktop application built with **C# (.NET WinForms)** and **SQL Server**.  
It streamlines hospital operations by managing patients, staff, appointments, visits, prescriptions, billing, insurance, and laboratory services.

---

## 🎯 Features
- 👥 **People Management** – Centralized table for patients, doctors, nurses, and staff  
- 🩺 **Visits & Appointments** – Schedule and track patient visits  
- 💊 **Prescriptions & Drugs** – Doctors issue prescriptions; pharmacists dispense them  
- ⚕️ **Vitals Recording** – Nurses capture patient vitals per visit  
- 🧪 **Lab Tests** – Lab technicians manage test orders and results  
- 💰 **Billing & Payments** – Handle invoices, insurance coverage, and payments  
- 🧾 **Insurance Management** – Manage insurance providers and patient policies  
- 🧑‍⚕️ **Staff Scheduling** – HR manages shifts and weekly schedules  
- 🔐 **User Roles** – Role-based access: Admin, Doctor, Nurse, Pharmacist, HR, LabTech, Receptionist

---

## 🧱 Tech Stack
| Component | Technology |
|------------|-------------|
| UI | C# (.NET WinForms) |
| Database | Microsoft SQL Server |
| Data Access | ADO.NET |
| Language | C# |

---

## 🗂️ Database Overview
Key tables include:  
`People`, `Users`, `Roles`,  
`Doctors`, `Nurses`, `Pharmacists`, `LabTechnicians`,  
`Appointments`, `Visits`, `Vitals`,  
`Prescriptions`, `Drugs`, `PrescriptionDrugs`,  
`Payments`, `InsuranceProviders`, `Claims`,  
`Departments`, `StaffSchedule`.

---

## 🚀 Getting Started
1. Clone or download this repository  
2. Open the solution in Visual Studio  
3. Restore the database in SQL Server from the provided .bak file:
  - Open SQL Server Management Studio (SSMS)
  - Right-click Databases → Restore Database...
  - Select Device, browse to the .bak file, and restore
4. Update the connection string in App.config to point to your restored database
5. Run the project and log in using a seeded Admin account
---

## 👩‍💻 Roles & Access
| Role | Access |
|------|----------|
| Admin | Full access |
| Doctor | Visits, Prescriptions |
| Nurse | Vitals |
| Pharmacist | Dispense Prescriptions |
| HR | Staff & Schedule |
| LabTechnician | Lab Tests |
| Receptionist | Appointments |

---

## 🧩 Future Enhancements
- Patient portal login  
- Email/SMS notifications  
- Dashboard analytics  
- Data export to Excel/PDF  

---

## 📄 License
This project is for **educational and demonstration** purposes.  
Feel free to extend or modify for learning.
