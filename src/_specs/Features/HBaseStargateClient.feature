Feature: HBase Stargate Client
	In order to read and write HBase data
	As an application developer
	I want access to all available REST operations on HBase Stargate via a managed client API

Background:
	Given I have everything I need to test an HBase client in isolation, with the following options:
		| server URL              | content type | false row key |
		| http://test-server:9999 | text/xml     | row           |
	And I have an HBase client

#Scenario: Create a table
#	Given I have created a new table definition
#	And I have set my table definition's name to "test"
#	And I have added a column named "alpha" to my table definition
#	When I create a table using my table definition
#	Then a REST request should have been submitted with the following values:
#		| method | url                                 | table | column |
#		| POST   | http://test-server:9999/test/schema | test  | alpha  |

Scenario Outline: Write a single value
	Given I have an identifier consisting of a <table>, a <row>, a <column>, a <qualifier>, and a <timestamp>
	When I write the value "hello world" using my identifier
	Then a REST request should have been submitted with the correct <method> and <url>
	And the REST request should have contained 1 cell
	And one of the cells in the REST request should have had the value "hello world"
Examples:
	| table | row | column | qualifier | timestamp | method | url                                      |
	| test  | 1   | alpha  |           |           | PUT    | http://test-server:9999/test/1/alpha     |
	| test  | 1   | alpha  | x         |           | PUT    | http://test-server:9999/test/1/alpha:x   |
	| test  | 1   | alpha  |           | 4         | PUT    | http://test-server:9999/test/1/alpha/4   |
	| test  | 1   | alpha  | x         | 4         | PUT    | http://test-server:9999/test/1/alpha:x/4 |

Scenario: Write multiple values
	Given I have created a set of cells
	And I have added a cell to my set with the following properties:
		| table | row | column | qualifier | value       |
		| test  | 1   | beta   | x         | hello world |
	And I have added a cell to my set with the following properties:
		| table | row | column | qualifier | value       |
		| test  | 1   | beta   | y         | dlrow olleh |
	And I have added a cell to my set with the following properties:
		| table | row | column | qualifier | value       |
		| test  | 1   | beta   | z         | lorum ipsum |
	When I write the set of cells
	Then a REST request should have been submitted with the following values:
		| method | url                              |
		| POST   | http://test-server:9999/test/row |
	And the REST request should have contained 3 cells
	And one of the cells in the REST request should have had the following values:
		| row | column | qualifier | value       |
		| 1   | beta   | x         | hello world |
	And one of the cells in the REST request should have had the following values:
		| row | column | qualifier | value       |
		| 1   | beta   | y         | dlrow olleh |
	And one of the cells in the REST request should have had the following values:
		| row | column | qualifier | value       |
		| 1   | beta   | z         | lorum ipsum |

Scenario Outline: Read a row
	Given I have an identifier consisting of a <table>, a <row>, a <column>, a <qualifier>, and a <timestamp>
	When I read a row using my identifier
	Then a REST request should have been submitted with the correct <method> and <url>
Examples:
	| table | row | column | qualifier | timestamp | method | url                                      |
	| test  | 1   |        |           |           | GET    | http://test-server:9999/test/1           |
	| test  | 1   | alpha  |           |           | GET    | http://test-server:9999/test/1/alpha     |
	| test  | 1   | alpha  | x         |           | GET    | http://test-server:9999/test/1/alpha:x   |
	| test  | 1   | alpha  |           | 4         | GET    | http://test-server:9999/test/1/alpha/4   |
	| test  | 1   | alpha  | x         | 4         | GET    | http://test-server:9999/test/1/alpha:x/4 |
	
Scenario Outline: Read a cell
	Given I have an identifier consisting of a <table>, a <row>, a <column>, a <qualifier>, and a <timestamp>
	When I read a cell using my identifier
	Then a REST request should have been submitted with the correct <method> and <url>
Examples:
	| table | row | column | qualifier | timestamp | method | url                                      |
	| test  | 1   |        |           |           | GET    | http://test-server:9999/test/1           |
	| test  | 1   | alpha  |           |           | GET    | http://test-server:9999/test/1/alpha     |
	| test  | 1   | alpha  | x         |           | GET    | http://test-server:9999/test/1/alpha:x   |
	| test  | 1   | alpha  |           | 4         | GET    | http://test-server:9999/test/1/alpha/4   |
	| test  | 1   | alpha  | x         | 4         | GET    | http://test-server:9999/test/1/alpha:x/4 |

#Scenario: Create a scanner
#	Given I have set my context to a table called "test"
#	And I have set my context to a new scanner
#	When I create the scanner
#	Then a REST request should have been submitted with the following values:
#		| method | url                                  |
#		| POST   | http://test-server:9999/test/scanner |
#
#Scenario: Read a result from a scanner
#	Given I have set my context to a table called "test"
#	And I have set my context to a new scanner
#	When I read a result from the scanner
#	Then a REST request should have been submitted with the following values:
#		| method | url                                         |
#		| GET    | http://test-server:9999/test/scanner/abc123 |
#
#Scenario: Delete a scanner
#	Given I have set my context to a table called "test"
#	And I have set my context to a new scanner
#	When I delete the scanner
#	Then a REST request should have been submitted with the following values:
#		| method | url                                         |
#		| DELETE | http://test-server:9999/test/scanner/abc123 |

#Scenario: Enumerate all tables
#	When I read the names of all tables
#	Then a REST request should have been submitted with the following values:
#		| method | url                     |
#		| GET    | http://test-server:9999 |

Scenario Outline: Delete a row, columm, or cell
	And I have an identifier consisting of a <table>, a <row>, a <column>, a <qualifier>, and a <timestamp>
	When I delete an item using my identifier
	Then a REST request should have been submitted with the correct <method> and <url>
Examples:
	| table | row | column | qualifier | timestamp | method | url                                      |
	| test  | 1   |        |           |           | DELETE | http://test-server:9999/test/1           |
	| test  | 1   | alpha  |           |           | DELETE | http://test-server:9999/test/1/alpha     |
	| test  | 1   | alpha  | x         |           | DELETE | http://test-server:9999/test/1/alpha:x   |
	| test  | 1   | alpha  |           | 4         | DELETE | http://test-server:9999/test/1/alpha/4   |
	| test  | 1   | alpha  | x         | 4         | DELETE | http://test-server:9999/test/1/alpha:x/4 |

#Scenario: Delete a table
#	Given I have set my context to a table called "test"
#	When I delete the table
#	Then a REST request should have been submitted with the following values:
#		| method | url                                 |
#		| DELETE | http://test-server:9999/test/schema |