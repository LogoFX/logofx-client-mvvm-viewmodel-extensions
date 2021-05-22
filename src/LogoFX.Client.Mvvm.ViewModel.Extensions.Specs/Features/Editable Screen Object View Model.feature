Feature: Editable Screen Object View Model
	As an app developer
	I would like the framework to properly manage the state after the model changes
	So that I am able to develop apps faster

Scenario: Changing property's value during editing should raise notifications	
	When I open the application
	And I use editable screen object view model
	And I set the name to be a valid name
	Then A dirty notification is raised
	And A changes cancellation notification is raised