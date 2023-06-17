Feature: Shopping List
	A Shopping list to organize buying necessities and other things

Scenario: Adding an Item that does not exist
	Given a shopping list
	And a product
	And a client to make http calls with
	When an item is added to the shopping list with the product's id using the client
	Then the resulting statuscode should be "Created"
	And the location header should be set
