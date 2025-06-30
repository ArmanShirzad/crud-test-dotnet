Feature: Customer Management

  As a user
  I want to manage customers
  So that I can perform CRUD operations

  Scenario: Add a new customer
    Given I have navigated to the Add Customer page
    When I fill in the customer details with valid information
    And I submit the form
    Then the customer should be successfully added
    And the customer should appear in the Customer List
