#region FreeBSD

// Copyright (c) 2014, The Tribe
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

using HBase.Stargate.Client.Api;
using HBase.Stargate.Client.TypeConversion;

using Ninject;
using Ninject.Modules;
using Ninject.Extensions.Conventions;

using Patterns.Configuration;

using RestSharp.Injection.Ninject;

namespace HBase.Stargate.Client.Ninject
{
    /// <summary>
    /// Provides Ninject bindings for <see cref="IStargate"/> and surrounding services.
    /// </summary>
    public class StargateModule : NinjectModule
    {
        private readonly Func<IConfigurationSource, IStargateOptions> _configOptionsAccessor;
        private readonly IStargateOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="StargateModule"/> class.
        /// </summary>
        public StargateModule()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StargateModule" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public StargateModule(IStargateOptions options)
        {
            _options = options;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StargateModule" /> class.
        /// </summary>
        /// <param name="configOptionsAccessor">The config options accessor.</param>
        public StargateModule(Func<IConfigurationSource, IStargateOptions> configOptionsAccessor)
        {
            _configOptionsAccessor = configOptionsAccessor;
        }

        /// <summary>
        /// Overridden to add <see cref="IStargate"/>-related bindings to the container.
        /// </summary>
        /// <remarks>
        /// Note that this module registers the <see cref="RestSharpModule" /> from RestSharp.Injection.Ninject.
        /// </remarks>
        public override void Load()
        {
            Kernel.Load(new INinjectModule[] { (INinjectModule)new RestSharpModule() });

            if(_options != null)
                Kernel.Bind<IStargateOptions>().ToConstant(_options);

            if(_configOptionsAccessor != null)
                Kernel.Bind<IStargateOptions>().ToMethod(context => _configOptionsAccessor(context.Kernel.Get<IConfigurationSource>()));

            Kernel.Bind<IResourceBuilder>().To<ResourceBuilder>();
            Kernel.Bind<IMimeConverterFactory>().To<MimeConverterFactory>();
            Kernel.Bind<IErrorProvider>().To<ErrorProvider>();
            Kernel.Bind<ISimpleValueConverter>().To<SimpleValueConverter>();
            Kernel.Bind<IStargate>().To<Api.Stargate>();
            Kernel.Bind(x => x.FromThisAssembly().SelectAllClasses().InheritedFrom<IMimeConverter>().BindToSelf());
            Kernel.Bind<IScannerOptionsConverter>().To<ScannerOptionsConverter>();
            Kernel.Bind<ICodec>().To<Base64Codec>();
        }
    }
}
