# Demo Product Catalog API

This API is for demonstrating basic functionality with ASP.NET Core web API's.

The API and the database are containerized using Docker.
The framework is netcoreapp3.1 and the database is SQL Server.

I used EntityFrameworkCore as the ORM with small SQL queries for search.

I enabled Swagger documentation for an easy UI over the API.
Open a browser to 'http://localhost:8080' while the API is running with Docker.

I set the CORS policy to allow everything since this is a demo API

## Running the API

You will need the Docker CLI and Docker-Compose CLI to run this API out of the box.

`NOTE:` If you do not want to use Docker, then you will need to change the server in the appsettings
connectionString to 'localhost' from 'db' and run SQL Server locally.

With Docker-compose, you should be able to simply run

```bash
> docker-compose build
> docker-compose up
```

Here are the images to pull from Docker if the compose file is getting hung from long downloads

* .NET SDK: mcr.microsoft.com/dotnet/core/sdk:3.1
* ASP.NET: mcr.microsoft.com/dotnet/core/aspnet:3.1
* SQL Server: mcr.microsoft.com/mssql/server:2019-latest

if the `build` or  `up` commands fail, simply terminate the process and rerun them

## API Design

The objective of this API was to have a Product Catalog which also stores price change history.
My thoughts on this were to first work out the details pertaining to the products.
I exposed 6 endpoints for products following REST standards

* GET    /products         => fetch all products
* GET    /products/:id     => fetch specific product
* GET    /products/search  => fetch products with similiar name
* POST   /products         => post product
* PUT    /products/:id     => update product
* DELETE /products/:id     => remove product

After the product side was worked out, I thought through scenarios or stories for the Price Log.
I decided that we would only need two GET endpoints for the Price Log and have the product update service handle adding new logs.

* GET /pricelog            => fetch all price logs
* GET /pricelog/:productId => fetch all price logs for a specific product

As Product PUT's are sent, logs are added for the price changes.
Currently it logs every PUT, even if it isn't the price, but I deemed that was fine since use cases for updating the description or any other detail could be looked at as additional features.
I also choose not to use any validation on the database since the scope of this demo is API related and not database related.

## Testing

I build a unit test project and wrote tests for the successes of the main ProductService methods.
These are local in memory unit tests using SQLite.
I decided against any additional testing since I had limited time for this project and it would be very similiar to the testing I already had in place.

## Additional API work not included with my production API's

### Integrated Acceptance Testing

Almost all of my API's have an additional third project for acceptance testing.
In this project I spin up a new Docker container for the DB, update it to the current migrations, seed it, then test that DB.
In these tests I use a TestClient to simulate an HTTP request and follow the path from the controller to service to DB and back.

### GraphQL - Hot Chocolate

I am a huge advocate for GraphQL and usually always include a GraphQL endpoint.
I decided to keep the scope of this demo restricted to RESTful since that covered the scope of the task.
My work with API's need functionality for both desktop and mobile, and GraphQL is perfect for that.

### CI/CD

My current workflow is with Bitbucket triggers and a Jenkins pipeline.
Since there will be no continued development on this project, I decided that would be out of scope as well.

### Integrations for AWS Lambda

I currently host all my API's as Lambda functions on AWS.
