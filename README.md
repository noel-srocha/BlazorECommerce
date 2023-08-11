# BlazorECommerce

BlazorECommerce is a sample project that demonstrates how to build an e-commerce website using Blazor WebAssembly and ASP.NET Core Web API. It features a product catalog, a shopping cart, a checkout process, and user authentication. Credit to [Patrick God](https://github.com/patrickgod) who helped me understand how Blazor works and how to use the best programming practices to better maintain software in general.

## Getting Started

To run this project, you need to have the following prerequisites installed:

- .NET 7 SDK
- Visual Studio 2022 or Visual Studio Code
- SQL Server Express LocalDB

You also need to clone this repository to your local machine:

```bash
git clone https://github.com/noel-srocha/BlazorECommerce.git
```

## Building the Project

To build the project, you need to restore the NuGet packages, apply the database migrations, and run the web applications.

### Restore NuGet Packages

Open a terminal in the root folder of the project and run the following command:

```bash
dotnet restore
```

This will restore all the dependencies for the Blazor client and the Web API projects.

### Apply Database Migrations

Open a terminal in the `src/BlazorECommerce.Server` folder and run the following command:

```bash
dotnet ef database update
```

This will create the database and apply the migrations for the Web API project. The database connection string is configured in the `appsettings.json` file.

### Run Web Applications

You can run the web applications using Visual Studio 2022 or Visual Studio Code.

#### Visual Studio 2022

Open the `BlazorECommerce.sln` file in Visual Studio 2022 and select `Multiple startup projects` in the Solution Explorer. Set both `BlazorECommerce.Client` and `BlazorECommerce.Server` as `Start` projects. Then press F5 to run them.

#### Visual Studio Code

Open two instances of Visual Studio Code, one for the `src/BlazorECommerce.Client` folder and one for the `src/BlazorECommerce.Server` folder. In each instance, open a terminal and run the following command:

```bash
dotnet watch run
```

This will start both web applications with hot reload enabled.

## Contributing

We welcome contributions from anyone who is interested in improving this project. You can contribute by reporting issues, submitting pull requests, or providing feedback.

Thank you for your interest in BlazorECommerce!
```
