#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Net5Template.WebAPI/Net5Template.WebAPI.csproj", "Net5Template.WebAPI/"]
COPY ["Net5Template.Infrastructure/Net5Template.Infrastructure.csproj", "Net5Template.Infrastructure/"]
COPY ["Net5Template.Core/Net5Template.Core.csproj", "Net5Template.Core/"]
COPY ["Net5Template.Application/Net5Template.Application.csproj", "Net5Template.Application/"]
RUN dotnet restore "Net5Template.WebAPI/Net5Template.WebAPI.csproj"
COPY . .
WORKDIR "/src/Net5Template.WebAPI"
RUN dotnet build "Net5Template.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Net5Template.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Net5Template.WebAPI.dll"]