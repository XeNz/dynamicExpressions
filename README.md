###dynamicExpressions

##What is this repository
This repo contains simple examples on how to dynamically build predicates which in turn get translated to SQL queries by EF Core.


##How to use this repository

` docker-compose up postgres` to run postgres 9.6 instance


` dotnet ef database update` to create database tables

` dotnet run` or run the application with your preferred IDE

` GET https://localhost:5001/example/expressions` to test out the examples

Be sure to check out the logging in console to get a grasp of how the predicates get translated to SQL

##Packages used
        <PackageReference Include="LinqKit.Microsoft.EntityFrameworkCore" Version="1.1.16" />
        <PackageReference Include="Microsoft.AspNetCore.App" Version="2.2.0" AllowExplicitVersion="true" />
        <PackageReference Include="Microsoft.AspNetCore.CookiePolicy" Version="2.2.0" />
        <PackageReference Include="Microsoft.AspNetCore.HttpsPolicy" Version="2.2.0" />
        <PackageReference Include="Microsoft.AspNetCore.Mvc" Version="2.2.0" />
        <PackageReference Include="Microsoft.AspNetCore.StaticFiles" Version="2.2.0" />
        <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="2.2.0" />
