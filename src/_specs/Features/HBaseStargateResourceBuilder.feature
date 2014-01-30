Feature: HBase Stargate Resource Builder
	In order to avoid using the wrong resource URI for a particular operation
	As a Stargate Client consumer
	I want to be sure that the resource URI is correct for every situation

Background:
	Given I have everything I need to test a resource builder in isolation, assuming a false-row-key of "newRow"

Scenario Outline: Cell or Row query
	Given I have a cell query consisting of a <table>, a <row>, a <column>, a <qualifier>, a <begin> timestamp, a <end> timestamp, and a <max> number of versions
	When I build a resource name for Cell or Row queries using my query
	Then the resulting resource name should match the expected <resource>
	And the operation <should or should not> have succeeded
	And if there was an exception, it should have been the expected <exception> type
	And if there was an exception, it should have had the expected exception <message>
Examples:
	| table | row | column | qualifier | begin | end | max | resource               | should or should not | exception         | message                                        |
	|       |     |        |           |       |     |     |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       |     |        |           |       |     | 2   |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       |     |        |           |       | 5   |     |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       |     |        |           |       | 5   | 2   |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       |     |        |           | 4     |     |     |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       |     |        |           | 4     |     | 2   |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       |     |        |           | 4     | 5   |     |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       |     |        |           | 4     | 5   | 2   |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       |     |        | x         |       |     |     |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       |     |        | x         |       |     | 2   |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       |     |        | x         |       | 5   |     |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       |     |        | x         |       | 5   | 2   |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       |     |        | x         | 4     |     |     |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       |     |        | x         | 4     |     | 2   |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       |     |        | x         | 4     | 5   |     |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       |     |        | x         | 4     | 5   | 2   |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       |     | alpha  |           |       |     |     |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       |     | alpha  |           |       |     | 2   |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       |     | alpha  |           |       | 5   |     |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       |     | alpha  |           |       | 5   | 2   |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       |     | alpha  |           | 4     |     |     |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       |     | alpha  |           | 4     |     | 2   |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       |     | alpha  |           | 4     | 5   |     |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       |     | alpha  |           | 4     | 5   | 2   |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       |     | alpha  | x         |       |     |     |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       |     | alpha  | x         |       |     | 2   |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       |     | alpha  | x         |       | 5   |     |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       |     | alpha  | x         |       | 5   | 2   |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       |     | alpha  | x         | 4     |     |     |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       |     | alpha  | x         | 4     |     | 2   |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       |     | alpha  | x         | 4     | 5   |     |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       |     | alpha  | x         | 4     | 5   | 2   |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       | 1   |        |           |       |     |     |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       | 1   |        |           |       |     | 2   |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       | 1   |        |           |       | 5   |     |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       | 1   |        |           |       | 5   | 2   |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       | 1   |        |           | 4     |     |     |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       | 1   |        |           | 4     |     | 2   |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       | 1   |        |           | 4     | 5   |     |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       | 1   |        |           | 4     | 5   | 2   |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       | 1   |        | x         |       |     |     |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       | 1   |        | x         |       |     | 2   |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       | 1   |        | x         |       | 5   |     |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       | 1   |        | x         |       | 5   | 2   |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       | 1   |        | x         | 4     |     |     |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       | 1   |        | x         | 4     |     | 2   |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       | 1   |        | x         | 4     | 5   |     |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       | 1   |        | x         | 4     | 5   | 2   |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       | 1   | alpha  |           |       |     |     |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       | 1   | alpha  |           |       |     | 2   |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       | 1   | alpha  |           |       | 5   |     |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       | 1   | alpha  |           |       | 5   | 2   |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       | 1   | alpha  |           | 4     |     |     |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       | 1   | alpha  |           | 4     |     | 2   |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       | 1   | alpha  |           | 4     | 5   |     |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       | 1   | alpha  |           | 4     | 5   | 2   |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       | 1   | alpha  | x         |       |     |     |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       | 1   | alpha  | x         |       |     | 2   |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       | 1   | alpha  | x         |       | 5   |     |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       | 1   | alpha  | x         |       | 5   | 2   |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       | 1   | alpha  | x         | 4     |     |     |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       | 1   | alpha  | x         | 4     |     | 2   |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       | 1   | alpha  | x         | 4     | 5   |     |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	|       | 1   | alpha  | x         | 4     | 5   | 2   |                        | should not           | ArgumentException | ResourceBuilder_MinimumForCellOrRowQueryNotMet |
	| test  |     |        |           |       |     |     | test/*                 | should               |                   |                                                |
	| test  |     |        |           |       |     | 2   | test/*?v=2             | should               |                   |                                                |
	| test  |     |        |           |       | 5   |     | test/*/*/5             | should               |                   |                                                |
	| test  |     |        |           |       | 5   | 2   | test/*/*/5?v=2         | should               |                   |                                                |
	| test  |     |        |           | 4     |     |     | test/*                 | should               |                   |                                                |
	| test  |     |        |           | 4     |     | 2   | test/*?v=2             | should               |                   |                                                |
	| test  |     |        |           | 4     | 5   |     | test/*/*/4,5           | should               |                   |                                                |
	| test  |     |        |           | 4     | 5   | 2   | test/*/*/4,5?v=2       | should               |                   |                                                |
	| test  |     |        | x         |       |     |     | test/*                 | should               |                   |                                                |
	| test  |     |        | x         |       |     | 2   | test/*?v=2             | should               |                   |                                                |
	| test  |     |        | x         |       | 5   |     | test/*/*/5             | should               |                   |                                                |
	| test  |     |        | x         |       | 5   | 2   | test/*/*/5?v=2         | should               |                   |                                                |
	| test  |     |        | x         | 4     |     |     | test/*                 | should               |                   |                                                |
	| test  |     |        | x         | 4     |     | 2   | test/*?v=2             | should               |                   |                                                |
	| test  |     |        | x         | 4     | 5   |     | test/*/*/4,5           | should               |                   |                                                |
	| test  |     |        | x         | 4     | 5   | 2   | test/*/*/4,5?v=2       | should               |                   |                                                |
	| test  |     | alpha  |           |       |     |     | test/*/alpha           | should               |                   |                                                |
	| test  |     | alpha  |           |       |     | 2   | test/*/alpha?v=2       | should               |                   |                                                |
	| test  |     | alpha  |           |       | 5   |     | test/*/alpha/5         | should               |                   |                                                |
	| test  |     | alpha  |           |       | 5   | 2   | test/*/alpha/5?v=2     | should               |                   |                                                |
	| test  |     | alpha  |           | 4     |     |     | test/*/alpha           | should               |                   |                                                |
	| test  |     | alpha  |           | 4     |     | 2   | test/*/alpha?v=2       | should               |                   |                                                |
	| test  |     | alpha  |           | 4     | 5   |     | test/*/alpha/4,5       | should               |                   |                                                |
	| test  |     | alpha  |           | 4     | 5   | 2   | test/*/alpha/4,5?v=2   | should               |                   |                                                |
	| test  |     | alpha  | x         |       |     |     | test/*/alpha:x         | should               |                   |                                                |
	| test  |     | alpha  | x         |       |     | 2   | test/*/alpha:x?v=2     | should               |                   |                                                |
	| test  |     | alpha  | x         |       | 5   |     | test/*/alpha:x/5       | should               |                   |                                                |
	| test  |     | alpha  | x         |       | 5   | 2   | test/*/alpha:x/5?v=2   | should               |                   |                                                |
	| test  |     | alpha  | x         | 4     |     |     | test/*/alpha:x         | should               |                   |                                                |
	| test  |     | alpha  | x         | 4     |     | 2   | test/*/alpha:x?v=2     | should               |                   |                                                |
	| test  |     | alpha  | x         | 4     | 5   |     | test/*/alpha:x/4,5     | should               |                   |                                                |
	| test  |     | alpha  | x         | 4     | 5   | 2   | test/*/alpha:x/4,5?v=2 | should               |                   |                                                |
	| test  | 1   |        |           |       |     |     | test/1                 | should               |                   |                                                |
	| test  | 1   |        |           |       |     | 2   | test/1?v=2             | should               |                   |                                                |
	| test  | 1   |        |           |       | 5   |     | test/1/*/5             | should               |                   |                                                |
	| test  | 1   |        |           |       | 5   | 2   | test/1/*/5?v=2         | should               |                   |                                                |
	| test  | 1   |        |           | 4     |     |     | test/1                 | should               |                   |                                                |
	| test  | 1   |        |           | 4     |     | 2   | test/1?v=2             | should               |                   |                                                |
	| test  | 1   |        |           | 4     | 5   |     | test/1/*/4,5           | should               |                   |                                                |
	| test  | 1   |        |           | 4     | 5   | 2   | test/1/*/4,5?v=2       | should               |                   |                                                |
	| test  | 1   |        | x         |       |     |     | test/1                 | should               |                   |                                                |
	| test  | 1   |        | x         |       |     | 2   | test/1?v=2             | should               |                   |                                                |
	| test  | 1   |        | x         |       | 5   |     | test/1/*/5             | should               |                   |                                                |
	| test  | 1   |        | x         |       | 5   | 2   | test/1/*/5?v=2         | should               |                   |                                                |
	| test  | 1   |        | x         | 4     |     |     | test/1                 | should               |                   |                                                |
	| test  | 1   |        | x         | 4     |     | 2   | test/1?v=2             | should               |                   |                                                |
	| test  | 1   |        | x         | 4     | 5   |     | test/1/*/4,5           | should               |                   |                                                |
	| test  | 1   |        | x         | 4     | 5   | 2   | test/1/*/4,5?v=2       | should               |                   |                                                |
	| test  | 1   | alpha  |           |       |     |     | test/1/alpha           | should               |                   |                                                |
	| test  | 1   | alpha  |           |       |     | 2   | test/1/alpha?v=2       | should               |                   |                                                |
	| test  | 1   | alpha  |           |       | 5   |     | test/1/alpha/5         | should               |                   |                                                |
	| test  | 1   | alpha  |           |       | 5   | 2   | test/1/alpha/5?v=2     | should               |                   |                                                |
	| test  | 1   | alpha  |           | 4     |     |     | test/1/alpha           | should               |                   |                                                |
	| test  | 1   | alpha  |           | 4     |     | 2   | test/1/alpha?v=2       | should               |                   |                                                |
	| test  | 1   | alpha  |           | 4     | 5   |     | test/1/alpha/4,5       | should               |                   |                                                |
	| test  | 1   | alpha  |           | 4     | 5   | 2   | test/1/alpha/4,5?v=2   | should               |                   |                                                |
	| test  | 1   | alpha  | x         |       |     |     | test/1/alpha:x         | should               |                   |                                                |
	| test  | 1   | alpha  | x         |       |     | 2   | test/1/alpha:x?v=2     | should               |                   |                                                |
	| test  | 1   | alpha  | x         |       | 5   |     | test/1/alpha:x/5       | should               |                   |                                                |
	| test  | 1   | alpha  | x         |       | 5   | 2   | test/1/alpha:x/5?v=2   | should               |                   |                                                |
	| test  | 1   | alpha  | x         | 4     |     |     | test/1/alpha:x         | should               |                   |                                                |
	| test  | 1   | alpha  | x         | 4     |     | 2   | test/1/alpha:x?v=2     | should               |                   |                                                |
	| test  | 1   | alpha  | x         | 4     | 5   |     | test/1/alpha:x/4,5     | should               |                   |                                                |
	| test  | 1   | alpha  | x         | 4     | 5   | 2   | test/1/alpha:x/4,5?v=2 | should               |                   |                                                |

Scenario Outline: Single value storage
	Given I have an identifier consisting of a <table>, a <row>, a <column>, a <qualifier>, and a <timestamp>
	When I build a resource name for storing single values using my identifier
	Then the resulting resource name should match the expected <resource>
	And the operation <should or should not> have succeeded
	And if there was an exception, it should have been the expected <exception> type
	And if there was an exception, it should have had the expected exception <message>
Examples:
	| table | row | column | qualifier | timestamp | resource         | should or should not | exception         | message                                           |
	|       |     |        |           |           |                  | should not           | ArgumentException | ResourceBuilder_MinimumForSingleValueAccessNotMet |
	|       |     |        |           | 4         |                  | should not           | ArgumentException | ResourceBuilder_MinimumForSingleValueAccessNotMet |
	|       |     |        | x         |           |                  | should not           | ArgumentException | ResourceBuilder_MinimumForSingleValueAccessNotMet |
	|       |     |        | x         | 4         |                  | should not           | ArgumentException | ResourceBuilder_MinimumForSingleValueAccessNotMet |
	|       |     | alpha  |           |           |                  | should not           | ArgumentException | ResourceBuilder_MinimumForSingleValueAccessNotMet |
	|       |     | alpha  |           | 4         |                  | should not           | ArgumentException | ResourceBuilder_MinimumForSingleValueAccessNotMet |
	|       |     | alpha  | x         |           |                  | should not           | ArgumentException | ResourceBuilder_MinimumForSingleValueAccessNotMet |
	|       |     | alpha  | x         | 4         |                  | should not           | ArgumentException | ResourceBuilder_MinimumForSingleValueAccessNotMet |
	|       | 1   |        |           |           |                  | should not           | ArgumentException | ResourceBuilder_MinimumForSingleValueAccessNotMet |
	|       | 1   |        |           | 4         |                  | should not           | ArgumentException | ResourceBuilder_MinimumForSingleValueAccessNotMet |
	|       | 1   |        | x         |           |                  | should not           | ArgumentException | ResourceBuilder_MinimumForSingleValueAccessNotMet |
	|       | 1   |        | x         | 4         |                  | should not           | ArgumentException | ResourceBuilder_MinimumForSingleValueAccessNotMet |
	|       | 1   | alpha  |           |           |                  | should not           | ArgumentException | ResourceBuilder_MinimumForSingleValueAccessNotMet |
	|       | 1   | alpha  |           | 4         |                  | should not           | ArgumentException | ResourceBuilder_MinimumForSingleValueAccessNotMet |
	|       | 1   | alpha  | x         |           |                  | should not           | ArgumentException | ResourceBuilder_MinimumForSingleValueAccessNotMet |
	|       | 1   | alpha  | x         | 4         |                  | should not           | ArgumentException | ResourceBuilder_MinimumForSingleValueAccessNotMet |
	| test  |     |        |           |           |                  | should not           | ArgumentException | ResourceBuilder_MinimumForSingleValueAccessNotMet |
	| test  |     |        |           | 4         |                  | should not           | ArgumentException | ResourceBuilder_MinimumForSingleValueAccessNotMet |
	| test  |     |        | x         |           |                  | should not           | ArgumentException | ResourceBuilder_MinimumForSingleValueAccessNotMet |
	| test  |     |        | x         | 4         |                  | should not           | ArgumentException | ResourceBuilder_MinimumForSingleValueAccessNotMet |
	| test  |     | alpha  |           |           |                  | should not           | ArgumentException | ResourceBuilder_MinimumForSingleValueAccessNotMet |
	| test  |     | alpha  |           | 4         |                  | should not           | ArgumentException | ResourceBuilder_MinimumForSingleValueAccessNotMet |
	| test  |     | alpha  | x         |           |                  | should not           | ArgumentException | ResourceBuilder_MinimumForSingleValueAccessNotMet |
	| test  |     | alpha  | x         | 4         |                  | should not           | ArgumentException | ResourceBuilder_MinimumForSingleValueAccessNotMet |
	| test  | 1   |        |           |           |                  | should not           | ArgumentException | ResourceBuilder_MinimumForSingleValueAccessNotMet |
	| test  | 1   |        |           | 4         |                  | should not           | ArgumentException | ResourceBuilder_MinimumForSingleValueAccessNotMet |
	| test  | 1   |        | x         |           |                  | should not           | ArgumentException | ResourceBuilder_MinimumForSingleValueAccessNotMet |
	| test  | 1   |        | x         | 4         |                  | should not           | ArgumentException | ResourceBuilder_MinimumForSingleValueAccessNotMet |
	| test  | 1   | alpha  |           |           | test/1/alpha     | should               |                   |                                                   |
	| test  | 1   | alpha  |           | 4         | test/1/alpha/4   | should               |                   |                                                   |
	| test  | 1   | alpha  | x         |           | test/1/alpha:x   | should               |                   |                                                   |
	| test  | 1   | alpha  | x         | 4         | test/1/alpha:x/4 | should               |                   |                                                   |

Scenario Outline: Write multiple values
	Given I have an identifier consisting of a <table>
	When I build a resource name for storing multiple values using my identifier
	Then the resulting resource name should match the expected <resource>
	And the operation <should or should not> have succeeded
	And if there was an exception, it should have been the expected <exception> type
	And if there was an exception, it should have had the expected exception <message>
Examples: 
	| table | resource    | should or should not | exception         | message                                     |
	|       |             | should not           | ArgumentException | ResourceBuilder_MinimumForBatchInsertNotMet |
	| test  | test/newRow | should               |                   |                                             |

Scenario Outline: Delete an item
	Given I have an identifier consisting of a <table>, a <row>, a <column>, a <qualifier>, and a <timestamp>
	When I build a resource name for deleting items using my identifier
	Then the resulting resource name should match the expected <resource>
	And the operation <should or should not> have succeeded
	And if there was an exception, it should have been the expected <exception> type
	And if there was an exception, it should have had the expected exception <message>
Examples:
	| table | row | column | qualifier | timestamp | resource         | should or should not | exception         | message                                    |
	|       |     |        |           |           |                  | should not           | ArgumentException | ResourceBuilder_MinimumForDeleteItemNotMet |
	|       |     |        |           | 4         |                  | should not           | ArgumentException | ResourceBuilder_MinimumForDeleteItemNotMet |
	|       |     |        | x         |           |                  | should not           | ArgumentException | ResourceBuilder_MinimumForDeleteItemNotMet |
	|       |     |        | x         | 4         |                  | should not           | ArgumentException | ResourceBuilder_MinimumForDeleteItemNotMet |
	|       |     | alpha  |           |           |                  | should not           | ArgumentException | ResourceBuilder_MinimumForDeleteItemNotMet |
	|       |     | alpha  |           | 4         |                  | should not           | ArgumentException | ResourceBuilder_MinimumForDeleteItemNotMet |
	|       |     | alpha  | x         |           |                  | should not           | ArgumentException | ResourceBuilder_MinimumForDeleteItemNotMet |
	|       |     | alpha  | x         | 4         |                  | should not           | ArgumentException | ResourceBuilder_MinimumForDeleteItemNotMet |
	|       | 1   |        |           |           |                  | should not           | ArgumentException | ResourceBuilder_MinimumForDeleteItemNotMet |
	|       | 1   |        |           | 4         |                  | should not           | ArgumentException | ResourceBuilder_MinimumForDeleteItemNotMet |
	|       | 1   |        | x         |           |                  | should not           | ArgumentException | ResourceBuilder_MinimumForDeleteItemNotMet |
	|       | 1   |        | x         | 4         |                  | should not           | ArgumentException | ResourceBuilder_MinimumForDeleteItemNotMet |
	|       | 1   | alpha  |           |           |                  | should not           | ArgumentException | ResourceBuilder_MinimumForDeleteItemNotMet |
	|       | 1   | alpha  |           | 4         |                  | should not           | ArgumentException | ResourceBuilder_MinimumForDeleteItemNotMet |
	|       | 1   | alpha  | x         |           |                  | should not           | ArgumentException | ResourceBuilder_MinimumForDeleteItemNotMet |
	|       | 1   | alpha  | x         | 4         |                  | should not           | ArgumentException | ResourceBuilder_MinimumForDeleteItemNotMet |
	| test  |     |        |           |           |                  | should not           | ArgumentException | ResourceBuilder_MinimumForDeleteItemNotMet |
	| test  |     |        |           | 4         |                  | should not           | ArgumentException | ResourceBuilder_MinimumForDeleteItemNotMet |
	| test  |     |        | x         |           |                  | should not           | ArgumentException | ResourceBuilder_MinimumForDeleteItemNotMet |
	| test  |     |        | x         | 4         |                  | should not           | ArgumentException | ResourceBuilder_MinimumForDeleteItemNotMet |
	| test  |     | alpha  |           |           |                  | should not           | ArgumentException | ResourceBuilder_MinimumForDeleteItemNotMet |
	| test  |     | alpha  |           | 4         |                  | should not           | ArgumentException | ResourceBuilder_MinimumForDeleteItemNotMet |
	| test  |     | alpha  | x         |           |                  | should not           | ArgumentException | ResourceBuilder_MinimumForDeleteItemNotMet |
	| test  |     | alpha  | x         | 4         |                  | should not           | ArgumentException | ResourceBuilder_MinimumForDeleteItemNotMet |
	| test  | 1   |        |           |           | test/1           | should               |                   |                                            |
	| test  | 1   |        |           | 4         | test/1/*/4       | should               |                   |                                            |
	| test  | 1   |        | x         |           | test/1           | should               |                   |                                            |
	| test  | 1   |        | x         | 4         | test/1/*/4       | should               |                   |                                            |
	| test  | 1   | alpha  |           |           | test/1/alpha     | should               |                   |                                            |
	| test  | 1   | alpha  |           | 4         | test/1/alpha/4   | should               |                   |                                            |
	| test  | 1   | alpha  | x         |           | test/1/alpha:x   | should               |                   |                                            |
	| test  | 1   | alpha  | x         | 4         | test/1/alpha:x/4 | should               |                   |                                            |