# Net 5 Template
This template is a starting point for different projects based on .Net 5, it uses Clean Architecture/Hexagonal/Onion based on loosely-coupled, dependency-inverted, Domain Driven Design, Command Query Responsibility Segregation (CQRS) and SOLID principles.

You can create a microservice with each template and dockerize it (according your needs), or use in a single monolithic solution.

## Templates
The templates are divided in branches, and are intended for use with the dotnet template installer. You can checkout the branch with the features you need and clone it.

We recommend the use of the dotnet template installer, with this the project namespace and objects will be renamed with your project name.

> Working on it!

|Branch Name|Description|
|---|---|
|allfeatures|All features for use as a monolith|
|master|All features for use as a monolith|
|EmailService|Email service that receives requests from EventBus (RabbitMQ)|
|ImageService|Image service API without Database for store and serve images|
|ApiWithDB|Simple API with their own Database|
|SimpleApi|Simple API without use of Database|
|ApiWithDBAndEventProducer|API with Database and a producer that send messages to RabbitMQ|
|ApiWithDBAndEventConsumer|API with Database and a consumer that receives messages to RabbitMQ|
|ApiAndEventProducer|API without Database and a producer that send messages to RabbitMQ|
|ApiAndEventConsumer|API without Database and a consumer that receives messages to RabbitMQ|
|ApiWithIdentity|API with Identity services using JWT, has a database to store and manage users and tokens|


## Features in templates
The main features on this templates are:
- CQRS with MediatR
- Event Bus with RabbitMQ over MassTransit
- OpenAPI using Swagger
- Authentication and Authorization over JWT using roles with of ASP.NET Identity
- Hosted Services (With Queues and Timed jobs)
- DataBase Context with Migrations using EF (you can use Dapper if you like)
- Email with MailKit
- WebSockets with SignalR
- Simple Image Processing
- Logging with Serilog
- Caching (MemoryCache for CachedEntities)
- Health Checks

## Getting Started
This project is divided in branches, each one has a different configuration for use on specific project types, so you can pick the template according to your proyect or microservice. Also, you'll find in branch `allfeatures` a sample of all tools.

Once you are in the branch you want to use, you can clone or use the dotnet template installer tools. 

```bash
#### install template on dotnet (place on solution folder parent)
dotnet new --install .\Net5Template

#### uninstall template
dotnet new -u .\Net5Template

#### create new project with custom name
dotnet new Net5Template --projectname "NewProjectName" --output NewProjectName
```

If you create a solution with multiple microservices remove the `.sln` file after installing template, and add to your existing solution.

## Goals 🚀
The goal of this repository is having a production ready templates for real world use. Work with microservices could be rough, having some good practices and similar base framework this can be achieved.

## Design Decisions and Dependencies
This is not and pure DDD project, eg: in these samples we are not using Domain Events, Aggregates, etc. although using the MediatR you can adapt it for use within the Domain.

Is a flexible solution and does not include every tool, framework or feature, you can modify it to use your preferred tools and practices. It does not include Unit/Functional/Integration Tests,
Analytics, Monitoring, etc. All of these can be added easily. The main idea is to keep these templates simple and with few dependencies, extendable to your needs.

## Share and contribute ⭐
If you like or use this project on your solutions please give a star, Thank you in advance!
This repo is an exercise, if you like to contribute and enhance this project you're welcome to contribute. You can fork this repo too.