Feature: HBase Protocol Buffers (protobuf)
	In order to work with HBase content in the protobuf format
	As an application developer
	I want to be able to create and parse HBase protobuf content using basic information

Background: 
	Given I have everything I need to test a content converter for protobuf

Scenario Outline: Create protobuf for single cell
	Given I have a cell with a <row>, <column>, <qualifier>, <timestamp>, and <value>
	When I convert my cell to raw content
	Then my raw protobuf content should be equivalent to the resource called "<expected protobuf>"
Examples:
	| row | column | qualifier | timestamp | value       | expected protobuf                 |
	| 1   | alpha  |           |           | hello world | HBaseProtobuf_1Alpha_HelloWorld   |
	| 1   | alpha  | x         |           | hello world | HBaseProtobuf_1AlphaX_HelloWorld  |
	| 1   | alpha  |           | 4         | hello world | HBaseProtobuf_1Alpha4_HelloWorld  |
	| 1   | alpha  | x         | 4         | hello world | HBaseProtobuf_1AlphaX4_HelloWorld |

Scenario Outline: Parse protobuf for single cell
	Given I have raw content equal to the resource called "<initial protobuf>"
	When I convert my raw content to a cell
	Then my cell should have a <row>, <column>, <qualifier>, <timestamp>, and <value>
Examples:
	| initial protobuf                  | row | column | qualifier | timestamp | value       |
	| HBaseProtobuf_1Alpha_HelloWorld   | 1   | alpha  |           |           | hello world |
	| HBaseProtobuf_1AlphaX_HelloWorld  | 1   | alpha  | x         |           | hello world |
	| HBaseProtobuf_1Alpha4_HelloWorld  | 1   | alpha  |           | 4         | hello world |
	| HBaseProtobuf_1AlphaX4_HelloWorld | 1   | alpha  | x         | 4         | hello world |

Scenario: Create protobuf for a set of cells
	Given I have created a set of cells
	And I have added a cell to my set with the following properties:
		| row | column | qualifier | timestamp | value       |
		| 1   | alpha  |           |           | hello world |
	And I have added a cell to my set with the following properties:
		| row | column | qualifier | timestamp | value       |
		| 1   | alpha  | x         |           | hello world |
	And I have added a cell to my set with the following properties:
		| row | column | qualifier | timestamp | value       |
		| 1   | alpha  |           | 4         | hello world |
	And I have added a cell to my set with the following properties:
		| row | column | qualifier | timestamp | value       |
		| 1   | alpha  | x         | 4         | hello world |
	When I convert my set of cells to raw content
	Then my raw protobuf content should be equivalent to the resource called "HBaseProtobuf_Set_HelloWorld"

Scenario: Parse protobuf for a set of cells
	Given I have raw content equal to the resource called "HBaseProtobuf_Set_HelloWorld"
	When I convert my raw content to a set of cells
	Then my set should contain 4 cells
	And one of the cells in my set should have the following properties:
		| row | column | qualifier | timestamp | value       |
		| 1   | alpha  |           |           | hello world |
	And one of the cells in my set should have the following properties:
		| row | column | qualifier | timestamp | value       |
		| 1   | alpha  | x         |           | hello world |
	And one of the cells in my set should have the following properties:
		| row | column | qualifier | timestamp | value       |
		| 1   | alpha  |           | 4         | hello world |
	And one of the cells in my set should have the following properties:
		| row | column | qualifier | timestamp | value       |
		| 1   | alpha  | x         | 4         | hello world |