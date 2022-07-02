# Storyteller
Web application with REST API for writing role-play stories with other users. Project was made for Mobile and Web Application Development course.

## Technologies
* .NET 5
* ASP.NET Core
* Entity Framework Core
* Razor Pages
* MS SQL

## Features
* account creation
* user profile management
* characters creation
* creating stories with other users by writing posts
* adding characters to stories
* characters and stories management
* admin panel

## Launch

You need to create a database and set it in the `appsettings.json` file in the `TGF.WebAPI` directory.

To run the API go to the `TGF.WebAPI` directory and type `dotnet run` in the terminal. The API will run on `https://localhost:5001`.

To run the web app go to the `TGF.WebApp` directory and type `dotnet run` in the terminal. The app will run on `https://localhost:5000`.
