# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["VroomAPI/VroomAPI.csproj", "VroomAPI/"]
RUN dotnet restore "VroomAPI/VroomAPI.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/VroomAPI"
RUN dotnet build "VroomAPI.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "VroomAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Install necessary dependencies for Oracle client
RUN apt-get update && apt-get install -y libaio1 wget unzip && rm -rf /var/lib/apt/lists/*

EXPOSE 5189

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "VroomAPI.dll"]