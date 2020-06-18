## Introduction

First of all, as you must know, DDD is not an architecture. DDD (Domain Driven Design) is a software modeling whose objective is to facilitate the implementation of complex rules and processes, where it aims to divide responsibilities by layers and is independent of the technology used. Knowing this concept, I present an architecture that can be used as a basis for building an API (Application Programming Interface) using .NET Core 3.1.

## The Architeture

![Architeture of the project](docs/Project_Architeture.png)

- Application layer: responsible for the main project, as this is where API controllers and services will be developed. It has the function of receiving all requests and directing them to a service to perform a certain action. It has references from the Service and Domain layers.
- Domain layer: responsible for implementing classes and models, which will be mapped to the database, in addition to obtaining declarations of interfaces, constants, DTOs (Data Transfer Object) and enums.
- Service layer: it would be the “heart” of the project, as it is where all business rules and validations are made, before data persist in the database. 
It has references from the Domain, Infra.Data and Infra.CrossCutting layers.
- Infrastructure layer: divided into three sub-layers
    - Data: performs the persistence with the database.
    - Cross-Cutting: a separate layer that does not obey the layer hierarchy. As its name says, this layer crosses the entire hierarchy. It contains the functionalities that can be used in any part of the code, such as documents validation, consumption of external API and use of some security.
    - Logging: responsible to make logs to the application.
It has references from the Domain layer.
- Background: responsible for the background jobs and processing, where you can program tasks to execute in a time interval or even integrate with a message-broker.

## Generic Classes

I implemented generic classes to streamline the project's development process. Therefore, you can change which are the basic properties of your application and choose whether or not to extend the generic classes that will assist in making the application's CRUDs

## Requirements

To compile the solution, SDK 3.1 must be installed on the machine. If you don't have it yet, [download .NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)

## Database

In this project I am using SQLServer. Therefore, to run the project in your local environment, it is necessary to have installed some version of SQLServer, which you can find on the page: [SQL Server download](https://www.microsoft.com/en-us/sql-server/sql-server-downloads). I left a folder inside the project where you can place the migrates as you build the application. There you will find the first script, the one for creating the database.

- sample-net-core-ddd\Sample.Infra\Sample.Infra.Data\Migrate

## How to run the application

1. Execute the migrates
2. Set connection string (in *appsettings.json*, for example)
3. Run!