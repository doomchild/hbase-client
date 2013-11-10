# hbase-client Changelog #

The following changelog details the changes made with each release of hbase-client.

## 1.0-beta.3 ##
Author of this release: [John Batte](https://github.com/jbatte47)

- Added ability to perform a "fuzzy match" between `Identifier` instances ([Issue 34](https://github.com/TheTribe/hbase-client/issues/34))
- Added ability to extract single values from `CellSet` instances ([Issue 35](https://github.com/TheTribe/hbase-client/issues/35))
- Refactored `Identifier` and model extensions ([Issue 33](https://github.com/TheTribe/hbase-client/issues/33))

## 1.0-beta.2 ##
Author of this release: [John Batte](https://github.com/jbatte47)

- Table administration support ([Issue 11](https://github.com/TheTribe/hbase-client/issues/11))
 - Create a table
 - Enumerate all tables
 - Delete a table
- Added scanner support ([Issue 12](https://github.com/TheTribe/hbase-client/issues/12))
 - Create a scanner
 - Filter support: `ColumnPrefixFilter`, `ColumnRangeFilter`, `FamilyFilter`,  
`QualifierFilter`, `RowFilter`, `FirstKeyOnlyFilter`, `InclusiveStopFilter`,  
`MultipleColumnPrefixFilter`, `PageFilter`, `PrefixFilter`, `SingleColumnValueFilter`,  
`TimestampsFilter`, `FilterList`
 - Read a result from a scanner (Manual or via disposable/enumerable object)
 - Delete a scanner (Manual or via disposable/enumerable object)
- Fixed NuGet package generation issues ([Issue 22](https://github.com/TheTribe/hbase-client/issues/22))

## 1.0-beta.1 ##
Authors of this release: [John Batte](https://github.com/jbatte47), [David Brandon](https://github.com/binaryberserker)

- Basic API model
- XML payload support
- Domain-aware resource URI builder
- Extensible REST response error provider
- Programmatic and configuration-based setup
- Autofac integration