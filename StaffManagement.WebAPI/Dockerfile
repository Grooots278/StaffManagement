FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["StaffManagement.WebAPI/StaffManagement.WebAPI.csproj", "StaffManagement.WebAPI/"]
COPY ["StaffManagement.Application/StaffManagement.Application.csproj", "StaffManagement.Application/"]
COPY ["StaffManagement.Infrastructure/StaffManagement.Infrastructure.csproj", "StaffManagement.Infrastructure/"]
COPY ["StaffManagement.Domain/StaffManagement.Domain.csproj", "StaffManagement.Domain/"]

RUN dotnet restore "StaffManagement.WebAPI/StaffManagement.WebAPI.csproj"

COPY . .
WORKDIR "/src/StaffManagement.WebAPI"
RUN dotnet build "StaffManagement.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "StaffManagement.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StaffManagement.WebAPI.dll"]