#region FreeBSD

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

#endregion

using System;

using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;

using HBase.Stargate.Client.Api;
using HBase.Stargate.Client.TypeConversion;

using Patterns.Autofac.Configuration;
using Patterns.Configuration;

using RestSharp.Injection.Autofac;

namespace HBase.Stargate.Client.Autofac
{
	/// <summary>
	///    Provides Autofac registrations for <see cref="IStargate" /> and surrounding services.
	/// </summary>
	public class StargateModule : Module
	{
		private readonly Func<IConfigurationSource, IStargateOptions> _configOptionsAccessor;
		private readonly IStargateOptions _options;

		/// <summary>
		///    Initializes a new instance of the <see cref="StargateModule" /> class.
		/// </summary>
		public StargateModule() {}

		/// <summary>
		///    Initializes a new instance of the <see cref="StargateModule" /> class.
		/// </summary>
		/// <param name="options">The options.</param>
		public StargateModule(IStargateOptions options)
		{
			_options = options;
		}

		/// <summary>
		///    Initializes a new instance of the <see cref="StargateModule" /> class.
		/// </summary>
		/// <param name="configOptionsAccessor">The config options accessor.</param>
		public StargateModule(Func<IConfigurationSource, IStargateOptions> configOptionsAccessor)
		{
			_configOptionsAccessor = configOptionsAccessor;
		}

		/// <summary>
		///    Overridden to add <see cref="IStargate" />-related registrations to the container.
		/// </summary>
		/// <param name="builder">
		///    The builder through which components can be
		///    registered.
		/// </param>
		/// <remarks>
		///    Note that this module registers the <see cref="RestSharpModule" /> from RestSharp.Injection.Autofac,
		///    and optionally the <see cref="ConfigurationModule" /> from Patterns.
		/// </remarks>
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterModule(new RestSharpModule());

			RegisterOptions(builder);
			RegisterResourceBuilder(builder);
			RegisterMimeConverterFactory(builder);
			RegisterErrorProvider(builder);
			RegisterSimpleValueConverter(builder);
			RegisterStargate(builder);
			RegisterMimeConverters(builder);
			RegisterScannerOptionsConverter(builder);
			RegisterCodec(builder);
		}

		/// <summary>
		/// Registers the codec.
		/// </summary>
		/// <param name="builder">The builder.</param>
		protected virtual IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterCodec(ContainerBuilder builder)
		{
			return builder.RegisterType<Base64Codec>().As<ICodec>();
		}

		/// <summary>
		/// Registers the scanner options converter.
		/// </summary>
		/// <param name="builder">The builder.</param>
		protected virtual IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterScannerOptionsConverter(ContainerBuilder builder)
		{
			return builder.RegisterType<ScannerOptionsConverter>().As<IScannerOptionsConverter>();
		}

		/// <summary>
		/// Registers the MIME converters.
		/// </summary>
		/// <param name="builder">The builder.</param>
		protected virtual IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterMimeConverters(ContainerBuilder builder)
		{
			return builder.RegisterAssemblyTypes(typeof (IMimeConverter).Assembly)
				.Where(type => typeof (IMimeConverter).IsAssignableFrom(type))
				.As<IMimeConverter>();
		}

		/// <summary>
		/// Registers the stargate.
		/// </summary>
		/// <param name="builder">The builder.</param>
		protected virtual IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterStargate(ContainerBuilder builder)
		{
			return builder.RegisterType<Api.Stargate>().As<IStargate>();
		}

		/// <summary>
		/// Registers the simple value converter.
		/// </summary>
		/// <param name="builder">The builder.</param>
		protected virtual IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterSimpleValueConverter(ContainerBuilder builder)
		{
			return builder.RegisterType<SimpleValueConverter>().As<ISimpleValueConverter>();
		}

		/// <summary>
		/// Registers the error provider.
		/// </summary>
		/// <param name="builder">The builder.</param>
		protected virtual IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterErrorProvider(ContainerBuilder builder)
		{
			return builder.RegisterType<ErrorProvider>().As<IErrorProvider>();
		}

		/// <summary>
		/// Registers the MIME converter factory.
		/// </summary>
		/// <param name="builder">The builder.</param>
		protected virtual IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterMimeConverterFactory(ContainerBuilder builder)
		{
			return builder.RegisterType<MimeConverterFactory>().As<IMimeConverterFactory>();
		}

		/// <summary>
		/// Registers the resource builder.
		/// </summary>
		/// <param name="builder">The builder.</param>
		protected virtual IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterResourceBuilder(ContainerBuilder builder)
		{
			return builder.RegisterType<ResourceBuilder>().As<IResourceBuilder>();
		}

		/// <summary>
		/// Registers the options.
		/// </summary>
		/// <param name="builder">The builder.</param>
		protected virtual IRegistrationBuilder<object, SimpleActivatorData, SingleRegistrationStyle> RegisterOptions(ContainerBuilder builder)
		{
			if (_options != null)
			{
				return builder.RegisterInstance(_options);
			}
			
			if (_configOptionsAccessor != null)
			{
				builder.RegisterModule(new ConfigurationModule());
				return builder.Register(context => _configOptionsAccessor(context.Resolve<IConfigurationSource>()));
			}

			return null;
		}
	}
}