# language: en-US
# Page titles verified against live site on 2026-05-09

Feature: Services Navigation
  As a visitor to the EPAM site
  I want to navigate to a Services category from the main menu
  So that I can read about EPAM's offering in that area

  Scenario Outline: Navigate to a service category and validate landing page
    Given I am on the EPAM home page
    When I open the Services menu and select "<category>"
    Then the page heading should be "<expected_title>"
    And the "Our Related Expertise" section should be displayed

    Examples:
      | category       | expected_title |
      | Generative AI  | Generative AI  |
      | Responsible AI | Responsible AI |
