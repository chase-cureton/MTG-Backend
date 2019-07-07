MTG Services Project

Project Structure:
- DataClass: Contains project models with DB table analog

- DataRepository: Contains dao pattern with underlying DynamoDbContext for interacting with database.
				  Contains data access objects built from project models 

- Helpers: Holds all outside of project focus services
		   - ex. AWS Services: [S3, DynamoDb, SecretsManager], HttpService, etc..

- Services: Holds business logic and core service APIs