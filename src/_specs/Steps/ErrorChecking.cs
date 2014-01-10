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
using System.Linq;

using FluentAssertions;

using TechTalk.SpecFlow;

using _specs.Models;

namespace _specs.Steps
{
	[Binding]
	public class ErrorChecking
	{
		private readonly ErrorContext _errors;
		private readonly ResourceContext _resources;

		public ErrorChecking(ErrorContext errors, ResourceContext resources)
		{
			_errors = errors;
			_resources = resources;
		}

		[Then(@"the operation (should|should not) have succeeded")]
		public void CheckExceptionExists(bool success)
		{
			if (success) _errors.CaughtErrors.Should().BeEmpty();
			else _errors.CaughtErrors.Should().NotBeEmpty();

			_errors.OutcomeViewedAsSuccessful = success;
		}

		[Then(@"if there was an exception, it should have been the expected (.*) type")]
		public void CheckExceptionType(string type)
		{
			if (_errors.CaughtErrors == null || !_errors.HasErrors) return;

			_errors.CaughtErrors.Should().HaveCount(1);
			_errors.CaughtErrors.ElementAt(0).GetType().Name.Should().Be(type);
		}

		[Then(@"if there was an exception, it should have had the expected exception (.*)")]
		public void CheckExceptionMessage(string messageResource)
		{
			if (_errors.CaughtErrors == null || !_errors.HasErrors) return;

			_errors.CaughtErrors.Should().HaveCount(1);
			_errors.CaughtErrors.ElementAt(0).Message.Should().Be(_resources.GetString(messageResource));
		}

		[Then(@"there should have been an? (.*)Exception with a message equivalent to the resource called ""(.+)""")]
		public void CheckAssumedExceptionType(string modifier, string resource)
		{
			string expectedTypeName = string.Format("{0}Exception", modifier);
			_errors.HasErrors.Should().BeTrue("I expected an error to be thrown");
			_errors.CaughtErrors.Count().Should().Be(1);
			Exception exception = _errors.CaughtErrors.Single();
			exception.GetType().Name.Should().Be(expectedTypeName);
			exception.Message.Should().Be(_resources.GetString(resource));
		}
	}
}