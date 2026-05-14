Shifts Logger Application

📌 Overview
This is my first project working with Web APIs.
The application consists of:

A Web API built with ASP.NET Core and Entity Framework Core to perform CRUD operations on shifts.

A Console UI application that consumes the API and allows users to interact with shifts and employees.

The goal is to simulate a real-world shift management system with clean code, edge-case handling, and practical rules.

⚙️ Features
Web API
CRUD operations: GET, POST, PUT, PATCH, DELETE

Database access via Entity Framework Core

Automatic calculation of shift duration in SQL (only start and end times need to be stored)

Console UI
Displays a list of employees with available IDs

Allows users to:

Create shifts

Start time = DateTime.Now

End time = random hours/minutes added (up to 12 hours, overnight shifts allowed)

Max 2 shifts per employee per day, no overlaps

View shifts

Paginated list of all shifts

Filter by date, duration, or employee ID

Sort by date, duration, or employee ID

View shifts longer/shorter than a given duration

Update shifts (full or partial) with validation rules

Delete shifts that started or ended on the current day

User feedback messages for successful or invalid operations

🛠 Rules & Validations
Shifts cannot exceed 12 hours

Employees can have max 2 shifts per day

Shifts must not overlap

Duration is always calculated automatically in the database

🚀 Getting Started
Clone the repository

Important AutoMapper Setup:

Before running migrations, AutoMapper must be upgraded to version 15 or 16. Lower versions (like 12) are vulnerable and will not work with EF migrations.
Before running the application, AutoMapper must be set to version 11 or 12, matching the version of its extension. If versions mismatch, the app will fail to run.
This step is required until a cleaner fix is implemented.

Set up the database with EF Core migrations

Run the Web API project

Run the Console UI project to interact with the API

📖 Notes
This project is a learning exercise in building APIs and consuming them with a client application.

The focus is on writing clean, organized, and testable code.

Improvements will continue as I learn more about architecture, best practices, and testing.
