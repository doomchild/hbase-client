// Copyright (c) 2013, The Tribe
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 
//  * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// 
//  * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the
//    documentation and/or other materials provided with the distribution.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED
// TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
// PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
// LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;

using FluentAssertions;

using HBase.Stargate.Client.Api;

using Patterns.Testing.Moq;

using TechTalk.SpecFlow;

using _specs.Models;

namespace _specs.Steps
{
	[Binding]
	public class ResourceBuilding
	{
		private readonly ResourceBuilderContext _builderContext;
		private readonly HBaseContext _hBase;
		private readonly ErrorContext _errors;
		private readonly IMoqContainer _container;

		public ResourceBuilding(ResourceBuilderContext builderContext, HBaseContext hBase, ErrorContext errors, IMoqContainer container)
		{
			_builderContext = builderContext;
			_hBase = hBase;
			_errors = errors;
			_container = container;
		}

		[Given(@"I have everything I need to test a resource builder in isolation, assuming a false-row-key of ""(.+)""")]
		public void SetupResourceBuilder(string falseRowKey)
		{
			_container.Update<IStargateOptions>(new StargateOptions {FalseRowKey = falseRowKey});
			_container.Update<IResourceBuilder, ResourceBuilder>();
		}

		[When(@"I build a resource name for Cell or Row queries using my query")]
		public void BuildCellOrRowQuery()
		{
			try
			{
				_builderContext.ResourceUri = _container.Create<IResourceBuilder>().BuildCellOrRowQuery(_hBase.Query);
			}
			catch (Exception error)
			{
				_errors.CaughtErrors = new[] {error};
			}
		}

		[When(@"I build a resource name for storing single values using my identifier")]
		public void BuildSingleValueStorage()
		{
			try
			{
				_builderContext.ResourceUri = _container.Create<IResourceBuilder>().BuildSingleValueAccess(_hBase.Identifier);
			}
			catch (Exception error)
			{
				_errors.CaughtErrors = new[] {error};
			}
		}

		[When(@"I build a resource name for storing multiple values using my identifier")]
		public void BuildMultipleValueStorage()
		{
			try
			{
				_builderContext.ResourceUri = _container.Create<IResourceBuilder>().BuildBatchInsert(_hBase.Identifier);
			}
			catch (Exception error)
			{
				_errors.CaughtErrors = new[] { error };
			}
		}

		[When(@"I build a resource name for deleting items using my identifier")]
		public void BuildDeleteItem()
		{
			try
			{
				_builderContext.ResourceUri = _container.Create<IResourceBuilder>().BuildDeleteItem(_hBase.Identifier);
			}
			catch (Exception error)
			{
				_errors.CaughtErrors = new[] { error };
			}
		}

		[Then(@"the resulting resource name should match the expected (.*)")]
		public void CheckBuiltResourceMatches(string value)
		{
			var left = string.IsNullOrEmpty(_builderContext.ResourceUri) ? null : _builderContext.ResourceUri;
			var right = string.IsNullOrEmpty(value) ? null : value;

			left.Should().Be(right);
		}
	}
}