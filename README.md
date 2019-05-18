### dynamicExpressions

## What is this repository
This repo contains simple examples on how to dynamically build predicates which in turn get translated to SQL queries by EF Core.


## How to use this repository

` docker-compose up postgres` to run postgres 9.6 instance


` dotnet ef database update` to create database tables

` dotnet run` or run the application with your preferred IDE

` GET https://localhost:5001/example/expressions` to test out the examples

Be sure to check out the logging in console to get a grasp of how the predicates get translated to SQL

## Packages used
LinqKit.Microsoft.EntityFrameworkCore Version=1.1.16

Microsoft.AspNetCore.App" Version=2.2.0

Npgsql.EntityFrameworkCore.PostgreSQL Version=2.2.0
