# Employment System API 🚀

## Overview  
The Employment System allows employers to create and manage job vacancies, while applicants can apply for available jobs.  
This project is built using **.NET 8**, **Entity Framework Core**, and **JWT Authentication** for enhanced security.  

---

## 🛠 Prerequisites  
Before running the project, ensure you have the following installed:  

- **.NET 8 SDK** → [Download](https://dotnet.microsoft.com/en-us/download)  
- **SQL Server** or **MySQL** for the database  
- **Postman** for API testing  
- **Git** to clone the project from GitHub  

---

## 🚀 How to Run the Project  

### 1️⃣ Clone the Repository  
```bash
git clone https://github.com/your-repo/EmploymentSystem.git
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
http://localhost:5000
```

---

## 🛠 API Testing with Postman  
Import the **`Postman_Collection.json`** file to test the API.  

### Available Endpoints  
| Method | Endpoint | Description |
|--------|---------|-------------|
| **GET** | `/api/vacancy` | Retrieve all vacancies |
| **GET** | `/api/vacancy/{id}` | Retrieve a specific vacancy |
| **POST** | `/api/vacancy` | Create a new vacancy |
| **PUT** | `/api/vacancy/{id}` | Update a vacancy |
| **DELETE** | `/api/vacancy/{id}` | Delete a vacancy |
| **PUT** | `/api/vacancy/{id}/status` | Activate or deactivate a vacancy |
| **GET** | `/api/vacancy/archived` | Retrieve archived vacancies |

---

## ⚙️ Database Configuration  
Ensure the correct database connection settings in **`appsettings.json`**:  
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=EmploymentDB;User Id=your_user;Password=your_password;"
}
```

---

## 🔐 Security & Authentication  
- **JWT Authentication**: Protects API endpoints with token-based authentication.  
- **Roles**: Supports `Admin`, `Employer`, `Applicant` with different permissions.  

---

## 📂 Included Files  
✔ **Complete source code** in `EmploymentSystem-Solution/`  
✔ **SQL script** in `Database/employment_db.sql`  
✔ **Postman collection** in `Postman_Collection.json`  
✔ **This setup guide** in `README.md`  

🎯 **🚀 Ready to go! Good luck! 💪🔥**
