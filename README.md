<h3>
  FlightAPI is a ASP.NET Core Web API application designed to manage flights.
  It provides a comprehensive set of features for creating, reading, updating, and deleting flight information.
  The application is built with a focus on clean architecture, separating concerns into different layers including service and repository layers.
</h3>

You can find the frontend for this app [here](https://github.com/Timek12/FlightWeb).

**Functional Features**
-	Flight CRUD operations
-	JWT authentication
</br>

**Architecture and Design**
-	Service layer
-	Repository layer
</br>

**Testing**
- xUnit
- Mocking with Moq 
-	Repository layer unit tests
-	Service layer unit tests
-	Controller layer unit tests

</br>

**Error Handling**
-	Exception handling middleware
-	Custom exceptions
</br>

**Logging**
-	Serilog
</br>

**Data Access**
-	Entity Framework
-	Migrations
-	Seeding data into tables
</br>

**Data Models**
-	DTOs
-	Entity models

</br>

**Object Mapping**
-	AutoMapper
</br>

## Running the project locally

### 1. Clone repository: 
```
git clone https://github.com/Timek12/FlightAPI.git
```
### 2. Open .sln file 

### 3. Add connection string in appsettings.json file
![image](https://github.com/Timek12/FlightAPI/assets/105653616/5bc314fd-b8db-4cf1-87e7-743702345eea)


### 4. Open Package Manager Console
(VS 2022: Tools -> Nuget Package Manager -> Package Manager Console)

### 5. Run command:
```
update-database
```
### 6. Run Project

