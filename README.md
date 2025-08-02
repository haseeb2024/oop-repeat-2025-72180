# Car Management Application

A comprehensive ASP.NET Core MVC application for managing car management operations, customer relationships, and mechanic workflows.

## 🚗 Project Overview

This application provides a complete solution for car management with role-based access control, automated cost calculations, and comprehensive data management.

## ✨ Features

### Core Functionality
- **Customer Management** - Complete CRUD operations for customer profiles
- **Car Registration** - Vehicle management with customer associations
- **Service Records** - Detailed service history and work tracking
- **Mechanic Management** - Mechanic profiles and service assignments
- **Cost Calculation** - Automated pricing (€75 per hour, rounded up)
- **Role-Based Access** - Administrator, Customer, and Mechanic roles

### Technical Features
- **ASP.NET Core Identity** - Secure authentication and authorization
- **Entity Framework Core** - Database management with MySQL
- **Unit Testing** - xUnit framework for business logic validation
- **RESTful API** - JSON endpoints for data access
- **Responsive UI** - Modern web interface

## 🛠️ Technical Stack

- **Framework**: ASP.NET Core 8.0 MVC
- **Database**: MySQL with Entity Framework Core
- **Authentication**: ASP.NET Core Identity
- **Testing**: xUnit
- **Package Manager**: NuGet

## 📁 Solution Structure

```
CarServiceManagement/
├── CarManagementApp.Domain/          # Business entities and DTOs
│   ├── Entities/                     # Domain models
│   └── DTOs/                        # Data transfer objects
├── CarManagementApp.API/             # REST API endpoints
│   └── Controllers/                  # API controllers
├── CarManagementAppRun/              # Main MVC application
│   ├── Controllers/                  # MVC controllers
│   ├── Views/                        # Razor views
│   ├── Services/                     # Business logic services
│   ├── Data/                         # Database context and seeding
│   └── wwwroot/                      # Static files
└── CarManagementApp.Tests/           # Unit tests
    └── ServiceCostCalculatorTests.cs # Business logic tests
```

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK
- MySQL Server
- Visual Studio 2022 or VS Code

### Database Setup
1. **Install MySQL Server**
2. **Configure Connection String** in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=CarServiceDB;Uid=root;Pwd=root;"
   }
   ```

### Installation Steps
1. **Clone the repository**
   ```bash
   git clone 
   cd CarServiceManagement
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the solution**
   ```bash
   dotnet build
   ```

4. **Run database migrations**
   ```bash
   cd CarManagementAppRun
   dotnet ef database update
   ```

5. **Start the application**
   ```bash
   dotnet run
   ```



## 👤 Login Credentials

### Administrator Access
- **Email**: admin@carservice.com
- **Password**: Dorset001^
-

### Customer Accounts
- **Customer 1**: customer1@carservice.com / Dorset001^
- **Customer 2**: customer2@carservice.com /Dorset001^


### Mechanic Accounts
- **Mechanic 1**: mechanic1@carservice.com / Dorset001^
- **Mechanic 2**: mechanic2@carservice.com / Dorset001^


## 🧪 Running Tests

### Unit Tests
```bash
cd CarManagementApp.Tests
dotnet test
```

### Test Coverage
- Service cost calculation logic
- Business rule validation
- Entity relationship testing







## 🗄️ Database Schema

### Core Entities
- **Customer**: Personal information and contact details
- **Car**: Vehicle registration and customer association
- **Mechanic**: Service provider profiles and specializations
- **ServiceRecord**: Work history, costs, and completion status

### Relationships
- Customer → Cars (One-to-Many)
- Car → ServiceRecords (One-to-Many)
- Mechanic → ServiceRecords (One-to-Many)

## 💰 Cost Calculation Example

Service cost is calculated as:
```
Hours Worked: 2.5 hours
Rounded Up: 3 hours
Rate: €75 per hour
Total Cost: 3 × €75 = €225
```




## 🔧 Configuration

### App Settings
- Database connection string
- Identity configuration
- Logging levels
- Development/Production environments

### Environment Variables
- `ASPNETCORE_ENVIRONMENT`: Development/Production
- `ConnectionStrings__DefaultConnection`: Database connection

## 🚨 Troubleshooting

### Common Issues
1. **Database Connection Error**
   - Verify MySQL is running
   - Check connection string in `appsettings.json`

2. **Migration Errors**
   - Delete existing database
   - Run `dotnet ef database update`

3. **Authentication Issues**
   - Clear browser cookies
   - Verify user exists in database













