# AccessABAC-JWT
## Introduction
This project implements JSON Web Token (JWT) and Attribute-Based Access Control (ABAC) methods for authentication and access control. It is developed using C# and .NET Framework.
## Folders
- Backend: Contains the backend logic and API implementation.
- Frontend: Contains the frontend components.
## API Endpoints
| Count | URL                                | Method | Descriptions                                                                                      | Access Policy                                                                                     |
|-------|------------------------------------|--------|---------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------|
| 1     | /api/access/resource/{id}          | GET    | Access details of a specific resource based on its ID.                                            | Owner can access high sensitivity; same department members can access medium or lower; public for low sensitivity.                                            |
| 2     | /api/access/AccessUserInfo         | GET    | Access information of the current user.                                                            | Admin role can access all user information.                                                      |
| 3     | /api/access/AccessAllUserInfo      | GET    | Access all user information (only accessible to admins).                                           | Admin role can access all user information.                                                      |
| 4     | /api/access/GetAllRoles            | GET    | Retrieve a list of all roles in the system (only accessible to admins).                            | Admin role can retrieve all roles in the system.                                                 |
| 5     | /api/access/add-role               | POST   | Add a new role to the system (only accessible to admins).                                          | Admin role can add new roles to the system.                                                      |
| 6     | /api/access/link-role-to-resource  | POST   | Link a role to a resource in the system (only accessible to admins).                               | Admin role can link roles to resources in the system.                                             |
| 7     | /api/access/link-user-to-role      | POST   | Link a user to a role in the system (only accessible to admins).                                   | Admin role can link users to roles in the system.                                                 |
| 8     | /api/access/add-user               | POST   | Add a new user to the system (only accessible to admins).                                          | Admin role can add new users to the system.                                                      |                          |
| 10    | /api/Auth/login                    | POST   | Login to the system.                                                                               | Anyone can log in to the system.                                                                  |
| 11    | /api/profileInfo/profile           | GET    | View personal profile information.                                                                 | Users can view their own personal profile information.                                           |
| 12    | /api/profileInfo/profile-edit      | PUT    | Edit personal profile information.                                                                 | Users can edit their own personal profile information.                                           |
## Test Accounts
| Count | Username | Password | Description                                      |
|-------|----------|----------|--------------------------------------------------|
| 1     | hoanganh | hoanganh | System admin, the most powerful role             |
| 2     | quocanh  | quocanh  | HR department                                    |
| 3     | admin    | admin    | IT department                                    |
| 4     | anhanh   | anhanh   | Finance department                               |
## Installation
### Prerequisites
- Visual Studio 2022 (ensure you have the latest version installed).
- .NET 6 SDK (included with Visual Studio 2022 or can be downloaded separately).
### Backend (API) Setup:
#### 1. Create a New ASP.NET Web API Project:
- Open Visual Studio 2022.
- Go to File -> New -> Project....
- Select ASP.NET Core Web Application.
- Name your project and click Create.
- Choose API as the project template and ensure .NET 6.x is selected.
#### 2. Configure HTTP Mode:
- By default, ASP.NET projects in Visual Studio usually run in HTTP mode during development.
- Ensure that your project properties (right-click on the project in Solution Explorer -> Properties) do not force HTTPS:
- Go to the Debug tab.
- Make sure Enable SSL is unchecked.
  
#### 3. Build and Run the Backend:
- Ensure your API controllers, models, and services are set up according to your project requirements.
- Press F5 or click Start to build and run your backend application.
- Verify that the API endpoints are accessible in HTTP mode.

### Frontend Setup:
#### 1. Create a New ASP.NET MVC or Razor Pages Project (Frontend):
- Go to File -> New -> Project....
- Select ASP.NET Core Web Application.
- Name your project and click Create.
- Choose MVC or Razor Pages as the project template and ensure .NET 6.x is selected.
#### 2. Configure HTTP Mode:
Follow the same steps as for the backend to ensure HTTPS is not forced in your project properties.

