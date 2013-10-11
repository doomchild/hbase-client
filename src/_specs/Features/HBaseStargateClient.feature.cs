﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.18213
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace _specs.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("HBase Stargate Client")]
    public partial class HBaseStargateClientFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "HBaseStargateClient.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "HBase Stargate Client", "In order to read and write HBase data\r\nAs an application developer\r\nI want access" +
                    " to all available REST operations on HBase Stargate via a managed client API", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 6
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "server URL",
                        "content type",
                        "false row key"});
            table1.AddRow(new string[] {
                        "http://test-server:9999",
                        "text/xml",
                        "row"});
#line 7
 testRunner.Given("I have everything I need to test a disconnected HBase client, with the following " +
                    "options:", ((string)(null)), table1, "Given ");
#line 10
 testRunner.And("I have everything I need to test a content converter for XML", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 11
 testRunner.And("I have an HBase client", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Write a single value")]
        [NUnit.Framework.TestCaseAttribute("test", "1", "alpha", "", "", "POST", "test/1/alpha", null)]
        [NUnit.Framework.TestCaseAttribute("test", "1", "alpha", "x", "", "POST", "test/1/alpha:x", null)]
        [NUnit.Framework.TestCaseAttribute("test", "1", "alpha", "", "4", "POST", "test/1/alpha/4", null)]
        [NUnit.Framework.TestCaseAttribute("test", "1", "alpha", "x", "4", "POST", "test/1/alpha:x/4", null)]
        public virtual void WriteASingleValue(string table, string row, string column, string qualifier, string timestamp, string method, string resource, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Write a single value", exampleTags);
#line 22
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 23
 testRunner.Given(string.Format("I have an identifier consisting of a {0}, a {1}, a {2}, a {3}, and a {4}", table, row, column, qualifier, timestamp), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 24
 testRunner.When("I write the value \"hello world\" using my identifier", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 25
 testRunner.Then(string.Format("a REST request should have been submitted with the correct {0} and {1}", method, resource), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 26
 testRunner.And("the REST request should have contained 1 cell", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 27
 testRunner.And("one of the cells in the REST request should have had the value \"hello world\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Write multiple values")]
        public virtual void WriteMultipleValues()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Write multiple values", ((string[])(null)));
#line 35
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 36
 testRunner.Given("I have created a set of cells for the \"test\" table", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "row",
                        "column",
                        "qualifier",
                        "value"});
            table2.AddRow(new string[] {
                        "1",
                        "beta",
                        "x",
                        "hello world"});
#line 37
 testRunner.And("I have added a cell to my set with the following properties:", ((string)(null)), table2, "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "row",
                        "column",
                        "qualifier",
                        "value"});
            table3.AddRow(new string[] {
                        "1",
                        "beta",
                        "y",
                        "dlrow olleh"});
#line 40
 testRunner.And("I have added a cell to my set with the following properties:", ((string)(null)), table3, "And ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "row",
                        "column",
                        "qualifier",
                        "value"});
            table4.AddRow(new string[] {
                        "1",
                        "beta",
                        "z",
                        "lorum ipsum"});
#line 43
 testRunner.And("I have added a cell to my set with the following properties:", ((string)(null)), table4, "And ");
#line 46
 testRunner.When("I write the set of cells", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "method",
                        "resource"});
            table5.AddRow(new string[] {
                        "POST",
                        "test/row"});
#line 47
 testRunner.Then("a REST request should have been submitted with the following values:", ((string)(null)), table5, "Then ");
#line 50
 testRunner.And("the REST request should have contained 3 cells", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "row",
                        "column",
                        "qualifier",
                        "value"});
            table6.AddRow(new string[] {
                        "1",
                        "beta",
                        "x",
                        "hello world"});
#line 51
 testRunner.And("one of the cells in the REST request should have had the following values:", ((string)(null)), table6, "And ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "row",
                        "column",
                        "qualifier",
                        "value"});
            table7.AddRow(new string[] {
                        "1",
                        "beta",
                        "y",
                        "dlrow olleh"});
#line 54
 testRunner.And("one of the cells in the REST request should have had the following values:", ((string)(null)), table7, "And ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "row",
                        "column",
                        "qualifier",
                        "value"});
            table8.AddRow(new string[] {
                        "1",
                        "beta",
                        "z",
                        "lorum ipsum"});
#line 57
 testRunner.And("one of the cells in the REST request should have had the following values:", ((string)(null)), table8, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read a row")]
        [NUnit.Framework.TestCaseAttribute("test", "1", "", "", "", "", "GET", "test/1", null)]
        [NUnit.Framework.TestCaseAttribute("test", "1", "", "", "", "5", "GET", "test/1/*/5", null)]
        [NUnit.Framework.TestCaseAttribute("test", "1", "", "", "4", "", "GET", "test/1", null)]
        [NUnit.Framework.TestCaseAttribute("test", "1", "", "", "4", "5", "GET", "test/1/*/4,5", null)]
        [NUnit.Framework.TestCaseAttribute("test", "1", "", "x", "", "", "GET", "test/1", null)]
        [NUnit.Framework.TestCaseAttribute("test", "1", "", "x", "", "5", "GET", "test/1/*/5", null)]
        [NUnit.Framework.TestCaseAttribute("test", "1", "", "x", "4", "", "GET", "test/1", null)]
        [NUnit.Framework.TestCaseAttribute("test", "1", "", "x", "4", "5", "GET", "test/1/*/4,5", null)]
        [NUnit.Framework.TestCaseAttribute("test", "1", "alpha", "", "", "", "GET", "test/1/alpha", null)]
        [NUnit.Framework.TestCaseAttribute("test", "1", "alpha", "", "", "5", "GET", "test/1/alpha/5", null)]
        [NUnit.Framework.TestCaseAttribute("test", "1", "alpha", "", "4", "", "GET", "test/1/alpha", null)]
        [NUnit.Framework.TestCaseAttribute("test", "1", "alpha", "", "4", "5", "GET", "test/1/alpha/4,5", null)]
        [NUnit.Framework.TestCaseAttribute("test", "1", "alpha", "x", "", "", "GET", "test/1/alpha:x", null)]
        [NUnit.Framework.TestCaseAttribute("test", "1", "alpha", "x", "", "5", "GET", "test/1/alpha:x/5", null)]
        [NUnit.Framework.TestCaseAttribute("test", "1", "alpha", "x", "4", "", "GET", "test/1/alpha:x", null)]
        [NUnit.Framework.TestCaseAttribute("test", "1", "alpha", "x", "4", "5", "GET", "test/1/alpha:x/4,5", null)]
        public virtual void ReadARow(string table, string row, string column, string qualifier, string begin, string end, string method, string resource, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read a row", exampleTags);
#line 61
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 62
 testRunner.Given(string.Format("I have a cell query consisting of a {0}, a {1}, a {2}, a {3}, a {4} timestamp, an" +
                        "d a {5} timestamp", table, row, column, qualifier, begin, end), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 63
 testRunner.When("I read a row using my query", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 64
 testRunner.Then(string.Format("a REST request should have been submitted with the correct {0} and {1}", method, resource), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read a single value")]
        [NUnit.Framework.TestCaseAttribute("test", "1", "alpha", "", "GET", "test/1/alpha", null)]
        [NUnit.Framework.TestCaseAttribute("test", "1", "alpha", "x", "GET", "test/1/alpha:x", null)]
        public virtual void ReadASingleValue(string table, string row, string column, string qualifier, string method, string resource, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read a single value", exampleTags);
#line 84
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 85
 testRunner.Given(string.Format("I have an identifier consisting of a {0}, a {1}, a {2}, and a {3}", table, row, column, qualifier), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 86
 testRunner.When("I read a single value using my identifier", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 87
 testRunner.Then(string.Format("a REST request should have been submitted with the correct {0} and {1}", method, resource), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Delete a row, columm, or cell")]
        [NUnit.Framework.TestCaseAttribute("test", "1", "", "", "", "DELETE", "test/1", null)]
        [NUnit.Framework.TestCaseAttribute("test", "1", "alpha", "", "", "DELETE", "test/1/alpha", null)]
        [NUnit.Framework.TestCaseAttribute("test", "1", "alpha", "x", "", "DELETE", "test/1/alpha:x", null)]
        [NUnit.Framework.TestCaseAttribute("test", "1", "alpha", "", "4", "DELETE", "test/1/alpha/4", null)]
        [NUnit.Framework.TestCaseAttribute("test", "1", "alpha", "x", "4", "DELETE", "test/1/alpha:x/4", null)]
        public virtual void DeleteARowColummOrCell(string table, string row, string column, string qualifier, string timestamp, string method, string resource, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Delete a row, columm, or cell", exampleTags);
#line 123
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 124
 testRunner.And(string.Format("I have an identifier consisting of a {0}, a {1}, a {2}, a {3}, and a {4}", table, row, column, qualifier, timestamp), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 125
 testRunner.When("I delete an item using my identifier", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 126
 testRunner.Then(string.Format("a REST request should have been submitted with the correct {0} and {1}", method, resource), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
