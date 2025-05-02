# Baking Sisters Web Application

## Project Structure

This solution consists of two main projects:

1. **BakingSisters.Api** - The backend API project that handles data access and business logic
2. **BakingSisters.Web** - The frontend Blazor web application that provides the user interface

## Database Architecture

Both projects share a single database context and schema:

- **BakeryDbContext** - Located in the BakingSisters.Api project, this is the single source of truth for database schema and operations
- The Web project references the API project to use its models and database context

## Key Components

### Unified Data Access

We've consolidated the database contexts to avoid duplication and ensure consistency:

- Removed `ApplicationDbContext` from the Web project
- Both projects now use `BakeryDbContext` from the API project
- All database migrations are managed in the API project
- Database seeding is also centralized in the API project

### Authentication & User Management

- User authentication is handled via the API's AuthService
- The Web project uses the API's User model for consistency
- Login/registration functionality in the Web project calls the API endpoints

## Development Guidelines

1. **Database Changes**:
   - All entity changes should be made in the API project's model classes
   - Run migrations from the API project using: `dotnet ef migrations add [MigrationName]`
   - Apply migrations using: `dotnet ef database update`

2. **Shared Models**:
   - Use the models from the API project in the Web project
   - Add new models to the API project, not the Web project

3. **Service References**:
   - The Web project references the API project, not vice versa
   - Avoid circular dependencies by keeping a clear separation of concerns 