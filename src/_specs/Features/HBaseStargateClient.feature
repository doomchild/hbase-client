Feature: HBase Stargate Client
	In order to read and write HBase data
	As an application developer
	I want access to all available REST operations on HBase Stargate via a managed client API

Background:
	Given I have everything I need to test a disconnected HBase client, with the following options:
		| server URL              | content type | false row key |
		| http://test-server:9999 | text/xml     | row           |
	And I have everything I need to test a content converter for XML
	And I have an HBase client

Scenario: Create a table
	Given I have created a new table schema
	And I have set my table schema's name to "test"
	And I have added a column named "alpha" to my table schema
	When I create a table using my table schema
	Then a REST request for schema updates should have been submitted with the following values:
		| method | resource    | table | column |
		| PUT    | test/schema | test  | alpha  |

Scenario Outline: Write a single value
	Given I have an identifier consisting of a <table>, a <row>, a <column>, a <qualifier>, and a <timestamp>
	When I write the value "hello world" using my identifier
	Then a REST request should have been submitted with the correct <method> and <resource>
	And the REST request should have contained 1 cell
	And one of the cells in the REST request should have had the value "hello world"
Examples:
	| table | row | column | qualifier | timestamp | method | resource         |
	| test  | 1   | alpha  |           |           | POST   | test/1/alpha     |
	| test  | 1   | alpha  | x         |           | POST   | test/1/alpha:x   |
	| test  | 1   | alpha  |           | 4         | POST   | test/1/alpha/4   |
	| test  | 1   | alpha  | x         | 4         | POST   | test/1/alpha:x/4 |

Scenario: Write multiple values
	Given I have created a set of cells for the "test" table
	And I have added a cell to my set with the following properties:
		| row | column | qualifier | value       |
		| 1   | beta   | x         | hello world |
	And I have added a cell to my set with the following properties:
		| row | column | qualifier | value       |
		| 1   | beta   | y         | dlrow olleh |
	And I have added a cell to my set with the following properties:
		| row | column | qualifier | value       |
		| 1   | beta   | z         | lorum ipsum |
	When I write the set of cells
	Then a REST request should have been submitted with the following values:
		| method | resource |
		| POST   | test/row |
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
	Given I have a cell query consisting of a <table>, a <row>, a <column>, a <qualifier>, a <begin> timestamp, a <end> timestamp, and a <max> number of results
	When I read a row using my query
	Then a REST request should have been submitted with the correct <method> and <resource>
Examples:
	| table | row | column | qualifier | begin | end | max | method | resource               |
	| test  | 1   |        |           |       |     |     | GET    | test/1                 |
	| test  | 1   |        |           |       |     | 2   | GET    | test/1?v=2             |
	| test  | 1   |        |           |       | 5   |     | GET    | test/1/*/5             |
	| test  | 1   |        |           |       | 5   | 2   | GET    | test/1/*/5?v=2         |
	| test  | 1   |        |           | 4     |     |     | GET    | test/1                 |
	| test  | 1   |        |           | 4     |     | 2   | GET    | test/1?v=2             |
	| test  | 1   |        |           | 4     | 5   |     | GET    | test/1/*/4,5           |
	| test  | 1   |        |           | 4     | 5   | 2   | GET    | test/1/*/4,5?v=2       |
	| test  | 1   |        | x         |       |     |     | GET    | test/1                 |
	| test  | 1   |        | x         |       |     | 2   | GET    | test/1?v=2             |
	| test  | 1   |        | x         |       | 5   |     | GET    | test/1/*/5             |
	| test  | 1   |        | x         |       | 5   | 2   | GET    | test/1/*/5?v=2         |
	| test  | 1   |        | x         | 4     |     |     | GET    | test/1                 |
	| test  | 1   |        | x         | 4     |     | 2   | GET    | test/1?v=2             |
	| test  | 1   |        | x         | 4     | 5   |     | GET    | test/1/*/4,5           |
	| test  | 1   |        | x         | 4     | 5   | 2   | GET    | test/1/*/4,5?v=2       |
	| test  | 1   | alpha  |           |       |     |     | GET    | test/1/alpha           |
	| test  | 1   | alpha  |           |       |     | 2   | GET    | test/1/alpha?v=2       |
	| test  | 1   | alpha  |           |       | 5   |     | GET    | test/1/alpha/5         |
	| test  | 1   | alpha  |           |       | 5   | 2   | GET    | test/1/alpha/5?v=2     |
	| test  | 1   | alpha  |           | 4     |     |     | GET    | test/1/alpha           |
	| test  | 1   | alpha  |           | 4     |     | 2   | GET    | test/1/alpha?v=2       |
	| test  | 1   | alpha  |           | 4     | 5   |     | GET    | test/1/alpha/4,5       |
	| test  | 1   | alpha  |           | 4     | 5   | 2   | GET    | test/1/alpha/4,5?v=2   |
	| test  | 1   | alpha  | x         |       |     |     | GET    | test/1/alpha:x         |
	| test  | 1   | alpha  | x         |       |     | 2   | GET    | test/1/alpha:x?v=2     |
	| test  | 1   | alpha  | x         |       | 5   |     | GET    | test/1/alpha:x/5       |
	| test  | 1   | alpha  | x         |       | 5   | 2   | GET    | test/1/alpha:x/5?v=2   |
	| test  | 1   | alpha  | x         | 4     |     |     | GET    | test/1/alpha:x         |
	| test  | 1   | alpha  | x         | 4     |     | 2   | GET    | test/1/alpha:x?v=2     |
	| test  | 1   | alpha  | x         | 4     | 5   |     | GET    | test/1/alpha:x/4,5     |
	| test  | 1   | alpha  | x         | 4     | 5   | 2   | GET    | test/1/alpha:x/4,5?v=2 |
	
Scenario Outline: Read a single value
	Given I have an identifier consisting of a <table>, a <row>, a <column>, and a <qualifier>
	When I read a single value using my identifier
	Then a REST request should have been submitted with the correct <method> and <resource>
Examples:
	| table | row | column | qualifier | method | resource           |
	| test  | 1   | alpha  |           | GET    | test/1/alpha?v=1   |
	| test  | 1   | alpha  | x         | GET    | test/1/alpha:x?v=1 |

#Scenario: Create a scanner
#	Given I have set my context to a table called "test"
#	And I have set my context to a new scanner
#	When I create the scanner
#	Then a REST request should have been submitted with the following values:
#		| method | resource     |
#		| POST   | test/scanner |
#
#Scenario: Read a result from a scanner
#	Given I have set my context to a table called "test"
#	And I have set my context to a new scanner
#	When I read a result from the scanner
#	Then a REST request should have been submitted with the following values:
#		| method | resource            |
#		| GET    | test/scanner/abc123 |
#
#Scenario: Delete a scanner
#	Given I have set my context to a table called "test"
#	And I have set my context to a new scanner
#	When I delete the scanner
#	Then a REST request should have been submitted with the following values:
#		| method | resource            |
#		| DELETE | test/scanner/abc123 |

Scenario: Enumerate all tables
	When I read the names of all tables
	Then a REST request should have been submitted with the following values:
		| method | resource |
		| GET    |          |

Scenario: Get table schema
	Given I will always get a response with a status of "OK" and content equivalent to the resource called "HBaseXml_TableSchema1"
	When I get the schema of the "test" table
	Then a REST request should have been submitted with the following values:
		| method | resource    |
		| GET    | test/schema |

Scenario Outline: Delete a row, columm, or cell
	And I have an identifier consisting of a <table>, a <row>, a <column>, a <qualifier>, and a <timestamp>
	When I delete an item using my identifier
	Then a REST request should have been submitted with the correct <method> and <resource>
Examples:
	| table | row | column | qualifier | timestamp | method | resource         |
	| test  | 1   |        |           |           | DELETE | test/1           |
	| test  | 1   | alpha  |           |           | DELETE | test/1/alpha     |
	| test  | 1   | alpha  | x         |           | DELETE | test/1/alpha:x   |
	| test  | 1   | alpha  |           | 4         | DELETE | test/1/alpha/4   |
	| test  | 1   | alpha  | x         | 4         | DELETE | test/1/alpha:x/4 |

Scenario: Delete a table
	When I delete the "test" table
	Then a REST request should have been submitted with the following values:
		| method | resource    |
		| DELETE | test/schema |