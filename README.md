# Employee API

A simple REST API for managing employees using .NET 9, Entity Framework Core, and SQL Server.

## Prerequisites

- .NET 9 SDK
- Visual Studio 2022 or Visual Studio Code
- SQL Server (LocalDB or higher for development)
- Azure SQL Database (for production deployment)

## Local Development Setup

1. Clone the repository
```bash
git clone <repository-url>
cd EmployeeApi
```

2. Ensure SQL Server LocalDB is installed or update the DefaultConnection string in appsettings.json to point to your SQL Server instance

3. Create the initial database migration
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

4. Run the application
```bash
dotnet run
```

The API will be available at `https://localhost:7234` (the port might be different on your machine).
Swagger documentation will be available at `https://localhost:7234/swagger`.

## Azure Deployment

1. Create an Azure SQL Database

2. Update the connection string in `appsettings.json`:
   - Replace the `AzureConnection` string with your actual Azure SQL connection string

3. Deploy to Azure App Service:
   - Use Visual Studio's Publish feature
   - Or use Azure CLI:
```bash
az webapp up --name <app-name> --resource-group <resource-group> --sku F1
```

## API Endpoints

- GET /api/employees - Get all employees
- GET /api/employees/{id} - Get an employee by ID
- POST /api/employees - Create a new employee
- PUT /api/employees/{id} - Update an employee
- DELETE /api/employees/{id} - Delete an employee

## Sample Request Body (POST/PUT)

```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "hireDate": "2024-03-20T00:00:00Z",
  "department": "IT"
}
```

## Development Notes

- The application uses SQL Server LocalDB for local development and Azure SQL for production
- Swagger UI is available in development environment for easy API testing
- All endpoints support async operations for better performance
- Input validation is implemented using Data Annotations
- The API follows RESTful conventions for endpoints and HTTP methods

## Database Configuration

- Local development uses SQL Server LocalDB
- Production uses Azure SQL Database
- Connection strings are configured in `appsettings.json`
- Entity Framework Core handles all database operations and migrations

## Error Handling

The API includes standard error responses:
- 400 Bad Request - Invalid input
- 404 Not Found - Resource doesn't exist
- 500 Internal Server Error - Server-side issues

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details