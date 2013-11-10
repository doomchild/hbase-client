Feature: Identifier Matching
	In order to determine whether two identifiers are similar
	As an hbase-client consumer
	I want to compare full identifiers with partial ones and get predictable results

Scenario: Successful Match; Table Only
	Given I have an identifier consisting of the following values:
		| table | row  | column  | qualifier  | timestamp |
		| test  | row1 | column1 | qualifier1 | 100       |
	And I have a second identifier consisting of the following values:
		| table |
		| test  |
	When I match the second identifier to the first one
	Then the identifier match should have succeeded

Scenario: Failed Match; Table Only
	Given I have an identifier consisting of the following values:
		| table | row  | column  | qualifier  | timestamp |
		| test  | row1 | column1 | qualifier1 | 100       |
	And I have a second identifier consisting of the following values:
		| table |
		| foo   |
	When I match the second identifier to the first one
	Then the identifier match should not have succeeded

Scenario: Successful Match; Table and Row
	Given I have an identifier consisting of the following values:
		| table | row  | column  | qualifier  | timestamp |
		| test  | row1 | column1 | qualifier1 | 100       |
	And I have a second identifier consisting of the following values:
		| table | row  |
		| test  | row1 |
	When I match the second identifier to the first one
	Then the identifier match should have succeeded

Scenario: Failed Match; Table and Row
	Given I have an identifier consisting of the following values:
		| table | row  | column  | qualifier  | timestamp |
		| test  | row1 | column1 | qualifier1 | 100       |
	And I have a second identifier consisting of the following values:
		| table | row |
		| test  | foo |
	When I match the second identifier to the first one
	Then the identifier match should not have succeeded

Scenario: Successful Match; Table, Row, and Column
	Given I have an identifier consisting of the following values:
		| table | row  | column  | qualifier  | timestamp |
		| test  | row1 | column1 | qualifier1 | 100       |
	And I have a second identifier consisting of the following values:
		| table | row  | column  |
		| test  | row1 | column1 |
	When I match the second identifier to the first one
	Then the identifier match should have succeeded

Scenario: Failed Match; Table, Row, and Column
	Given I have an identifier consisting of the following values:
		| table | row  | column  | qualifier  | timestamp |
		| test  | row1 | column1 | qualifier1 | 100       |
	And I have a second identifier consisting of the following values:
		| table | row  | column |
		| test  | row1 | foo    |
	When I match the second identifier to the first one
	Then the identifier match should not have succeeded

Scenario: Successful Match; Table, Row, Column, and Qualifier
	Given I have an identifier consisting of the following values:
		| table | row  | column  | qualifier  | timestamp |
		| test  | row1 | column1 | qualifier1 | 100       |
	And I have a second identifier consisting of the following values:
		| table | row  | column  | qualifier  |
		| test  | row1 | column1 | qualifier1 |
	When I match the second identifier to the first one
	Then the identifier match should have succeeded

Scenario: Failed Match; Table, Row, Column, and Qualifier
	Given I have an identifier consisting of the following values:
		| table | row  | column  | qualifier  | timestamp |
		| test  | row1 | column1 | qualifier1 | 100       |
	And I have a second identifier consisting of the following values:
		| table | row  | column  | qualifier |
		| test  | row1 | column1 | foo       |
	When I match the second identifier to the first one
	Then the identifier match should not have succeeded

Scenario: Successful Match; Table, Row, Column, Qualifier, and Timestamp
	Given I have an identifier consisting of the following values:
		| table | row  | column  | qualifier  | timestamp |
		| test  | row1 | column1 | qualifier1 | 100       |
	And I have a second identifier consisting of the following values:
		| table | row  | column  | qualifier  | timestamp |
		| test  | row1 | column1 | qualifier1 | 100       |
	When I match the second identifier to the first one
	Then the identifier match should have succeeded

Scenario: Failed Match; Table, Row, Column, Qualifier, and Timestamp
	Given I have an identifier consisting of the following values:
		| table | row  | column  | qualifier  | timestamp |
		| test  | row1 | column1 | qualifier1 | 100       |
	And I have a second identifier consisting of the following values:
		| table | row  | column  | qualifier  | timestamp |
		| test  | row1 | column1 | qualifier1 | 200       |
	When I match the second identifier to the first one
	Then the identifier match should not have succeeded