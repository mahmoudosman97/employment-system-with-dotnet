# Employment System API 🚀

## Overview  
The Employment System allows employers to create and manage job vacancies, while applicants can apply for available jobs.  
This project is built using **.NET 8**, **Entity Framework Core**, and **JWT Authentication** for enhanced security.  

---

## 🛠 Prerequisites  
Before running the project, ensure you have the following installed:  

- **.NET 8 SDK** → [Download](https://dotnet.microsoft.com/en-us/download)  
- **SQL Server** for the database  
- **Postman** for API testing  
- **Git** to clone the project from GitHub  

---

## 🚀 How to Run the Project  

### 1️⃣ Clone the Repository  
```bash
git clone https://github.com/mahmoudosman97/employment-system-with-dotnet.git
cd EmploymentSystem
```

### 2️⃣ Restore Dependencies  
```bash
dotnet restore
```

### 3️⃣ Database Setup  
Run the following command to apply migrations and create the database:  
```bash
dotnet ef database update
```
OR, if using SQL scripts, execute **`Database/employment_db.sql`** manually.

### 4️⃣ Run the Application  
```bash
dotnet run
```
Once the application starts, the API will be available at:  
```
http://localhost:5212
```

---

## 🛠 API Testing with Postman  
Import the **`Postman_Collection.json`** file to test the API.  

### **Available Endpoints**  

#### **1. Application Endpoints**  
| Method | Endpoint | Description |
|--------|-----------|-------------|
| **POST** | `/api/Application` | Submit a new application |
| **GET** | `/api/Application/search` | Search for applications |
| **GET** | `/api/Application/applicant/{applicantId}` | Get applications by applicant ID |
| **GET** | `/api/Application/vacancy/{vacancyId}` | Get applications by vacancy ID |

---

#### **2. User Endpoints**  
| Method | Endpoint | Description |
|--------|-----------|-------------|
| **POST** | `/api/User/register` | Register a new user (Employer/Applicant) |
| **POST** | `/api/User/login` | User login to obtain JWT token |

---

#### **3. Vacancy Endpoints**  
| Method | Endpoint | Description |
|--------|-----------|-------------|
| **GET** | `/api/Vacancy` | Retrieve all vacancies |
| **POST** | `/api/Vacancy` | Create a new vacancy |
| **GET** | `/api/Vacancy/{id}` | Retrieve a specific vacancy |
| **PUT** | `/api/Vacancy/{id}` | Update a vacancy |
| **DELETE** | `/api/Vacancy/{id}` | Delete a vacancy |
| **PUT** | `/api/Vacancy/{id}/status` | Activate or deactivate a vacancy |

---

## ⚙️ Database Configuration  
Ensure the correct database connection settings in **`appsettings.json`**:  
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=EmploymentDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

---

## 🔐 Security & Authentication  
- **JWT Authentication**: Protects API endpoints with token-based authentication.  
- **Roles**: Supports `Employer`, `Applicant` with different permissions.  

---

## 📂 Included Files  
✔ **Complete source code** in `EmploymentSystem-Solution/`  
✔ **SQL script** in `Database/employment_db.sql`  
✔ **Postman collection** in `Postman_Collection.json`  
✔ **This setup guide** in `README.md`  

🎯 **🚀 Ready to go! Good luck! 💪🔥**  
