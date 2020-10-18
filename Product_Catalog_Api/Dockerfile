# Get base SDK image from Microsoft
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /Product_Catalog_Api

# Copy only the csproj file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy project files and build release
COPY . ./
RUN dotnet publish -c Release -o out

# Generate runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /Product_Catalog_Api
EXPOSE 80/tcp
COPY --from=build-env /Product_Catalog_Api/out .
ENTRYPOINT ["dotnet", "Product_Catalog_Api.dll"]
