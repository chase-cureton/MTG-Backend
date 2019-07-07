# MTG-Backend

Magic the Gathering Deck Building Tool

This is meant to be a tool to help facilitate Magic the Gathering (Commander) deck building by providing access to real-time metrics as the user is building the user is building the deck.
A user will have access to a search bar to search against a database of cards and is then able to drag and drop from those selections into a deck being constructed. As cards are being added to the deck, the application will track and display "card role types" as they are being filled.

Ex. A user is building a new deck with many giant creatures and so naturally, a good number (8-10+) of mana ramp cards will be need included in the commander deck to accomodate playing big cost creatures. This tool will allow for this user to search for the the mana ramp cards needed for their deck, drag and drop any of the returned results into the deck under construction and the tool will count the sources of mana ramp to display when the quota has been filled.

This concept can then be applied across any number of different themes in Magic the Gathering commander format, from Card Draw to Graveyard recursion, to interaction with opponent boards such as removal.

Once a user has created a deck, they will be able to place an order for those cards with CardKingdom or TCGPlayer by clicking a button and the tool redirecting the user to a checkout screen with the cards in the online cart.


MTG Services Project Structure

Project Structure:
- DataClass: Contains project models with DB table analog

- DataRepository: Contains dao pattern with underlying DynamoDbContext for interacting with database.
				  Contains data access objects built from project models 

- Helpers: Holds all outside of project focus services
		   - ex. AWS Services: [S3, DynamoDb, SecretsManager], HttpService, etc..

- Services: Holds business logic and core service APIs
