{ fetchNuGet }:
[
  (fetchNuGet {
    pname = "ClangSharp";
    version = "18.1.0";
    hash = "sha256-ggWWS6ONcjH7TmD8kO0RbWUVVrm1LgowHzi3bGOge38=";
  })
  (fetchNuGet {
    pname = "ClangSharp.Interop";
    version = "18.1.0";
    hash = "sha256-RQ6ngH9JlWYFljCJJ1eEea8hln07uoCyGXRKa4H2Dp0=";
  })
  (fetchNuGet {
    pname = "ClangSharp.PInvokeGenerator";
    version = "18.1.0";
    hash = "sha256-pASpO9N/KgX36/R+8fbRPzsq49nohheBDRL99YBuzlk=";
  })
  (fetchNuGet {
    pname = "coverlet.collector";
    version = "6.0.2";
    hash = "sha256-LdSQUrOmjFug47LjtqgtN2MM6BcfG0HR5iL+prVHlDo=";
  })
  (fetchNuGet {
    pname = "DiffPlex";
    version = "1.7.2";
    hash = "sha256-Vsn81duAmPIPkR40h5bEz7hgtF5Kt5nAAGhQZrQbqxE=";
  })
  (fetchNuGet {
    pname = "envdte";
    version = "17.8.37221";
    hash = "sha256-jKSZceeB+/IwrOqZ3USX6HeSQtxKCPs9VzA733NHEEU=";
  })
  (fetchNuGet {
    pname = "Humanizer.Core";
    version = "2.14.1";
    hash = "sha256-EXvojddPu+9JKgOG9NSQgUTfWq1RpOYw7adxDPKDJ6o=";
  })
  (fetchNuGet {
    pname = "JetBrains.Rider.PathLocator";
    version = "1.0.11";
    hash = "sha256-7rEL+fbcFyG5sZuhkRGLtAgqyqktjYT0HgKrOBUJGk4=";
  })
  (fetchNuGet {
    pname = "libclang";
    version = "18.1.3.2";
    hash = "sha256-/j/+jkBtn+byQv8BQR9D1nArbRYO4iFe32nUd3y4mq4=";
  })
  (fetchNuGet {
    pname = "libclang.runtime.linux-x64";
    version = "18.1.3.2";
    hash = "sha256-GSWOM8LnhcWeaux9mFGTik4sDE/yfSqHj0OokMLC004=";
  })
  (fetchNuGet {
    pname = "libClangSharp";
    version = "18.1.3.1";
    hash = "sha256-q2C27wOOQtnV6Gi4PAkCTg0TKQzOLxZj9BDSjHI5Rk0=";
  })
  (fetchNuGet {
    pname = "libClangSharp.runtime.linux-x64";
    version = "18.1.3.1";
    hash = "sha256-aOl/4qi95618ZcVFsyDe7J4w8iI1+VpdNfj3yJVwleQ=";
  })
  (fetchNuGet {
    pname = "Microsoft.Bcl.AsyncInterfaces";
    version = "7.0.0";
    hash = "sha256-1e031E26iraIqun84ad0fCIR4MJZ1hcQo4yFN+B7UfE=";
  })
  (fetchNuGet {
    pname = "Microsoft.Build";
    version = "17.10.4";
    hash = "sha256-yaElGdmgcELCXR5fIe5/ingMx2qS/PM3tZGTPNHHjXo=";
  })
  (fetchNuGet {
    pname = "Microsoft.Build.Framework";
    version = "17.10.4";
    hash = "sha256-J9N2VDoyd2P0e4PLhI3XsYsiyE0vKSIerhglV0FW+Bk=";
  })
  (fetchNuGet {
    pname = "Microsoft.Build.Locator";
    version = "1.7.8";
    hash = "sha256-VhZ4jiJi17Cd5AkENXL1tjG9dV/oGj0aY67IGYd7vNs=";
  })
  (fetchNuGet {
    pname = "Microsoft.Build.Utilities.Core";
    version = "17.10.4";
    hash = "sha256-eHEObY1YqK/+hOJmUzSu7u/dKp/OX5qQOWk07rEUReQ=";
  })
  (fetchNuGet {
    pname = "Microsoft.CodeAnalysis.Analyzer.Testing";
    version = "1.1.2-beta1.24169.1";
    hash = "sha256-G+Ka1enMqarIJn3Z93ABMcNccnDf0jontksajysXkx8=";
    url = "https://pkgs.dev.azure.com/dnceng/9ee6d478-d288-47f7-aacc-f6e6d082ae6d/_packaging/d1622942-d16f-48e5-bc83-96f4539e7601/nuget/v3/flat2/microsoft.codeanalysis.analyzer.testing/1.1.2-beta1.24169.1/microsoft.codeanalysis.analyzer.testing.1.1.2-beta1.24169.1.nupkg";
  })
  (fetchNuGet {
    pname = "Microsoft.CodeAnalysis.Analyzers";
    version = "3.3.4";
    hash = "sha256-qDzTfZBSCvAUu9gzq2k+LOvh6/eRvJ9++VCNck/ZpnE=";
  })
  (fetchNuGet {
    pname = "Microsoft.CodeAnalysis.CodeFix.Testing";
    version = "1.1.2-beta1.24169.1";
    hash = "sha256-KemMgw/upJvrR5UoWQGewT9xUpJE8N1HQ7PaQRxI8xk=";
    url = "https://pkgs.dev.azure.com/dnceng/9ee6d478-d288-47f7-aacc-f6e6d082ae6d/_packaging/d1622942-d16f-48e5-bc83-96f4539e7601/nuget/v3/flat2/microsoft.codeanalysis.codefix.testing/1.1.2-beta1.24169.1/microsoft.codeanalysis.codefix.testing.1.1.2-beta1.24169.1.nupkg";
  })
  (fetchNuGet {
    pname = "Microsoft.CodeAnalysis.Common";
    version = "4.8.0";
    hash = "sha256-3IEinVTZq6/aajMVA8XTRO3LTIEt0PuhGyITGJLtqz4=";
  })
  (fetchNuGet {
    pname = "Microsoft.CodeAnalysis.CSharp";
    version = "4.8.0";
    hash = "sha256-MmOnXJvd/ezs5UPcqyGLnbZz5m+VedpRfB+kFZeeqkU=";
  })
  (fetchNuGet {
    pname = "Microsoft.CodeAnalysis.CSharp.Analyzer.Testing";
    version = "1.1.2-beta1.24169.1";
    hash = "sha256-OiK0obVad78no9//zT7amyLM+Pd8HxqUaUo6NnGCCKQ=";
    url = "https://pkgs.dev.azure.com/dnceng/9ee6d478-d288-47f7-aacc-f6e6d082ae6d/_packaging/d1622942-d16f-48e5-bc83-96f4539e7601/nuget/v3/flat2/microsoft.codeanalysis.csharp.analyzer.testing/1.1.2-beta1.24169.1/microsoft.codeanalysis.csharp.analyzer.testing.1.1.2-beta1.24169.1.nupkg";
  })
  (fetchNuGet {
    pname = "Microsoft.CodeAnalysis.CSharp.CodeFix.Testing";
    version = "1.1.2-beta1.24169.1";
    hash = "sha256-gStX8hjxiewBUW53cbjrgOI9cK2JRpgxOtM2wEfc1QQ=";
    url = "https://pkgs.dev.azure.com/dnceng/9ee6d478-d288-47f7-aacc-f6e6d082ae6d/_packaging/d1622942-d16f-48e5-bc83-96f4539e7601/nuget/v3/flat2/microsoft.codeanalysis.csharp.codefix.testing/1.1.2-beta1.24169.1/microsoft.codeanalysis.csharp.codefix.testing.1.1.2-beta1.24169.1.nupkg";
  })
  (fetchNuGet {
    pname = "Microsoft.CodeAnalysis.CSharp.SourceGenerators.Testing";
    version = "1.1.2-beta1.24169.1";
    hash = "sha256-ygX05LGVCrdlUCwg8JxikhC0LQyKV8vEBz7qoOcnt1I=";
    url = "https://pkgs.dev.azure.com/dnceng/9ee6d478-d288-47f7-aacc-f6e6d082ae6d/_packaging/d1622942-d16f-48e5-bc83-96f4539e7601/nuget/v3/flat2/microsoft.codeanalysis.csharp.sourcegenerators.testing/1.1.2-beta1.24169.1/microsoft.codeanalysis.csharp.sourcegenerators.testing.1.1.2-beta1.24169.1.nupkg";
  })
  (fetchNuGet {
    pname = "Microsoft.CodeAnalysis.CSharp.Workspaces";
    version = "1.0.1";
    hash = "sha256-aAYSZLTSfbuJ0Yh4N0zVEgnbbIOGSnsgn1PH3Ceo9iY=";
  })
  (fetchNuGet {
    pname = "Microsoft.CodeAnalysis.CSharp.Workspaces";
    version = "3.8.0";
    hash = "sha256-i6PTXkHepgTXseFFg57iRh5thKtKYc9CH11y/qzDy8k=";
  })
  (fetchNuGet {
    pname = "Microsoft.CodeAnalysis.CSharp.Workspaces";
    version = "4.8.0";
    hash = "sha256-WNzc+6mKqzPviOI0WMdhKyrWs8u32bfGj2XwmfL7bwE=";
  })
  (fetchNuGet {
    pname = "Microsoft.CodeAnalysis.ResxSourceGenerator";
    version = "3.11.0-beta1.24324.1";
    hash = "sha256-+YEdFt4em1e8SMC3PnRAwOfyMZLwWPJo7/loTt7l9kQ=";
  })
  (fetchNuGet {
    pname = "Microsoft.CodeAnalysis.SourceGenerators.Testing";
    version = "1.1.2-beta1.24169.1";
    hash = "sha256-n+EX77Lb6lt0wPIEyiBtXcRu3kgD3fl5d5XzSK+m2vQ=";
    url = "https://pkgs.dev.azure.com/dnceng/9ee6d478-d288-47f7-aacc-f6e6d082ae6d/_packaging/d1622942-d16f-48e5-bc83-96f4539e7601/nuget/v3/flat2/microsoft.codeanalysis.sourcegenerators.testing/1.1.2-beta1.24169.1/microsoft.codeanalysis.sourcegenerators.testing.1.1.2-beta1.24169.1.nupkg";
  })
  (fetchNuGet {
    pname = "Microsoft.CodeAnalysis.Workspaces.Common";
    version = "1.0.1";
    hash = "sha256-/SYPkq5LhOoEWi+rcBZDyQL2U0cQk2YrykNJODrRLVs=";
  })
  (fetchNuGet {
    pname = "Microsoft.CodeAnalysis.Workspaces.Common";
    version = "3.8.0";
    hash = "sha256-3D7xV3V1WsUU9OMMEOj+z9GouCDKXSBC4Z/Szs/OcWE=";
  })
  (fetchNuGet {
    pname = "Microsoft.CodeAnalysis.Workspaces.Common";
    version = "4.8.0";
    hash = "sha256-X8R4SpWVO/gpip5erVZf5jCCx8EX3VzIRtNrQiLDIoM=";
  })
  (fetchNuGet {
    pname = "Microsoft.CodeCoverage";
    version = "17.10.0";
    hash = "sha256-yQFwqVChRtIRpbtkJr92JH2i+O7xn91NGbYgnKs8G2g=";
  })
  (fetchNuGet {
    pname = "Microsoft.DotNet.ILCompiler";
    version = "9.0.0";
    hash = "sha256-1DTOB+GEeDoeh9H1Q09OR3fXFQn0lniBTRyDsVN+gUY=";
  })
  (fetchNuGet {
    pname = "Microsoft.NET.ILLink.Tasks";
    version = "9.0.0";
    hash = "sha256-23+lxHpxVh7Me942mSjHxQIdR6akX06ZKAUp3oziJ+w=";
  })
  (fetchNuGet {
    pname = "Microsoft.NET.StringTools";
    version = "17.10.4";
    hash = "sha256-nXY7YaIx6sXn7aMqpF4bW4d2J5U1KNb9sXqRSd8MpOc=";
  })
  (fetchNuGet {
    pname = "Microsoft.NET.Test.Sdk";
    version = "17.10.0";
    hash = "sha256-rkHIqB2mquNXF89XBTFpUL2z5msjTBsOcyjSBCh36I0=";
  })
  (fetchNuGet {
    pname = "Microsoft.NETCore.Platforms";
    version = "1.0.1";
    hash = "sha256-mZotlGZqtrqDSoBrZhsxFe6fuOv5/BIo0w2Z2x0zVAU=";
  })
  (fetchNuGet {
    pname = "Microsoft.NETCore.Platforms";
    version = "1.1.0";
    hash = "sha256-FeM40ktcObQJk4nMYShB61H/E8B7tIKfl9ObJ0IOcCM=";
  })
  (fetchNuGet {
    pname = "Microsoft.NETCore.Platforms";
    version = "2.0.0";
    hash = "sha256-IEvBk6wUXSdyCnkj6tHahOJv290tVVT8tyemYcR0Yro=";
  })
  (fetchNuGet {
    pname = "Microsoft.NETCore.Platforms";
    version = "5.0.0";
    hash = "sha256-LIcg1StDcQLPOABp4JRXIs837d7z0ia6+++3SF3jl1c=";
  })
  (fetchNuGet {
    pname = "Microsoft.NETCore.Targets";
    version = "1.0.1";
    hash = "sha256-lxxw/Gy32xHi0fLgFWNj4YTFBSBkjx5l6ucmbTyf7V4=";
  })
  (fetchNuGet {
    pname = "Microsoft.NETCore.Targets";
    version = "1.1.0";
    hash = "sha256-0AqQ2gMS8iNlYkrD+BxtIg7cXMnr9xZHtKAuN4bjfaQ=";
  })
  (fetchNuGet {
    pname = "Microsoft.TestPlatform.ObjectModel";
    version = "17.10.0";
    hash = "sha256-3YjVGK2zEObksBGYg8b/CqoJgLQ1jUv4GCWNjDhLRh4=";
  })
  (fetchNuGet {
    pname = "Microsoft.TestPlatform.TestHost";
    version = "17.10.0";
    hash = "sha256-+yzP3FY6WoOosSpYnB7duZLhOPUZMQYy8zJ1d3Q4hK4=";
  })
  (fetchNuGet {
    pname = "Microsoft.VisualStudio.Composition";
    version = "16.1.8";
    hash = "sha256-yFT4t3Uk31R5EPdAxxsTAmRuiv58MlYoYL4JT1ywlHQ=";
  })
  (fetchNuGet {
    pname = "Microsoft.VisualStudio.Composition.NetFxAttributes";
    version = "16.1.8";
    hash = "sha256-FFemIG+m8RWUPo5W+kCHPh5Yn4fGS+tpjGiQTcT0sAE=";
  })
  (fetchNuGet {
    pname = "Microsoft.VisualStudio.Interop";
    version = "17.8.37221";
    hash = "sha256-n4gHogs2T+RxO7OCuFiITu90KTychqZycQMqPbukNuE=";
  })
  (fetchNuGet {
    pname = "Microsoft.VisualStudio.SolutionPersistence";
    version = "1.0.9";
    hash = "sha256-nHtOU1/brWxDCAHSHHLDQzvR1hgpBeyQqjV0KByMYvM=";
  })
  (fetchNuGet {
    pname = "Microsoft.VisualStudio.Validation";
    version = "15.0.82";
    hash = "sha256-7JFaA/HZHVjsEtTh/iHDRLi5RcuA39KKvvCkuI4JQFc=";
  })
  (fetchNuGet {
    pname = "Microsoft.Win32.Registry";
    version = "5.0.0";
    hash = "sha256-9kylPGfKZc58yFqNKa77stomcoNnMeERXozWJzDcUIA=";
  })
  (fetchNuGet {
    pname = "NETStandard.Library";
    version = "2.0.3";
    hash = "sha256-Prh2RPebz/s8AzHb2sPHg3Jl8s31inv9k+Qxd293ybo=";
  })
  (fetchNuGet {
    pname = "Newtonsoft.Json";
    version = "13.0.1";
    hash = "sha256-K2tSVW4n4beRPzPu3rlVaBEMdGvWSv/3Q1fxaDh4Mjo=";
  })
  (fetchNuGet {
    pname = "NuGet.Common";
    version = "6.3.4";
    hash = "sha256-GDzEyx9/wdVOUAri94uoDjChmfDnBhI90nBfzoHarts=";
  })
  (fetchNuGet {
    pname = "NuGet.Configuration";
    version = "6.3.4";
    hash = "sha256-qXIONIKcCIXJUmNJQs7MINQ18qIEUByTtW5xsORoZoc=";
  })
  (fetchNuGet {
    pname = "NuGet.Frameworks";
    version = "6.3.4";
    hash = "sha256-zqogus3HXQYSiqfnhVH2jd2VZXa+uTsmaw/uwD8dlgY=";
  })
  (fetchNuGet {
    pname = "NuGet.Packaging";
    version = "6.3.4";
    hash = "sha256-1LKM5vgfNKn8v2LcqialwmcynACISR57q13n7I2lQbU=";
  })
  (fetchNuGet {
    pname = "NuGet.Protocol";
    version = "6.3.4";
    hash = "sha256-j3L4bDzM+0/U4dm9q914DNpOzPpOPWhaolfOFKosdAQ=";
  })
  (fetchNuGet {
    pname = "NuGet.Resolver";
    version = "6.3.4";
    hash = "sha256-rXYXgdJMtwne3skk4jMgqyZlwh3QCTX9hIHvvXafxUM=";
  })
  (fetchNuGet {
    pname = "NuGet.Versioning";
    version = "6.3.4";
    hash = "sha256-6CMYVQeGfXu+xner3T3mgl/iQfXiYixoHizmrNA6bvQ=";
  })
  (fetchNuGet {
    pname = "runtime.linux-x64.Microsoft.DotNet.ILCompiler";
    version = "9.0.0";
    hash = "sha256-3xkJe6dOfJnG4LhN/147lFFDekYujwPwP0OknKH0wmc=";
  })
  (fetchNuGet {
    pname = "System.Buffers";
    version = "4.5.1";
    hash = "sha256-wws90sfi9M7kuCPWkv1CEYMJtCqx9QB/kj0ymlsNaxI=";
  })
  (fetchNuGet {
    pname = "System.Collections";
    version = "4.0.11";
    hash = "sha256-puoFMkx4Z55C1XPxNw3np8nzNGjH+G24j43yTIsDRL0=";
  })
  (fetchNuGet {
    pname = "System.Collections.Concurrent";
    version = "4.0.12";
    hash = "sha256-zIEM7AB4SyE9u6G8+o+gCLLwkgi6+3rHQVPdn/dEwB8=";
  })
  (fetchNuGet {
    pname = "System.Collections.Immutable";
    version = "7.0.0";
    hash = "sha256-9an2wbxue2qrtugYES9awshQg+KfJqajhnhs45kQIdk=";
  })
  (fetchNuGet {
    pname = "System.Collections.Immutable";
    version = "8.0.0";
    hash = "sha256-F7OVjKNwpqbUh8lTidbqJWYi476nsq9n+6k0+QVRo3w=";
  })
  (fetchNuGet {
    pname = "System.CommandLine";
    version = "2.0.0-beta4.24209.3";
    hash = "sha256-u2jJfGip9LyWJop5qUgQxUvnnXI3AETLuarrmE6zhEw=";
    url = "https://pkgs.dev.azure.com/dnceng/9ee6d478-d288-47f7-aacc-f6e6d082ae6d/_packaging/516521bf-6417-457e-9a9c-0a4bdfde03e7/nuget/v3/flat2/system.commandline/2.0.0-beta4.24209.3/system.commandline.2.0.0-beta4.24209.3.nupkg";
  })
  (fetchNuGet {
    pname = "System.ComponentModel.Composition";
    version = "4.5.0";
    hash = "sha256-xxeZs1zIkhl2ZXU8CaOtCkMX1N290IK7bbHYeEKD0aQ=";
  })
  (fetchNuGet {
    pname = "System.Composition";
    version = "1.0.31";
    hash = "sha256-wcQEG6MCRa1S03s3Yb3E3tfsIBZid99M7WDhcb48Qik=";
  })
  (fetchNuGet {
    pname = "System.Composition";
    version = "7.0.0";
    hash = "sha256-YjhxuzuVdAzRBHNQy9y/1ES+ll3QtLcd2o+o8wIyMao=";
  })
  (fetchNuGet {
    pname = "System.Composition.AttributedModel";
    version = "7.0.0";
    hash = "sha256-3s52Dyk2J66v/B4LLYFBMyXl0I8DFDshjE+sMjW4ubM=";
  })
  (fetchNuGet {
    pname = "System.Composition.Convention";
    version = "7.0.0";
    hash = "sha256-N4MkkBXSQkcFKsEdcSe6zmyFyMmFOHmI2BNo3wWxftk=";
  })
  (fetchNuGet {
    pname = "System.Composition.Hosting";
    version = "7.0.0";
    hash = "sha256-7liQGMaVKNZU1iWTIXvqf0SG8zPobRoLsW7q916XC3M=";
  })
  (fetchNuGet {
    pname = "System.Composition.Runtime";
    version = "7.0.0";
    hash = "sha256-Oo1BxSGLETmdNcYvnkGdgm7JYAnQmv1jY0gL0j++Pd0=";
  })
  (fetchNuGet {
    pname = "System.Composition.TypedParts";
    version = "7.0.0";
    hash = "sha256-6ZzNdk35qQG3ttiAi4OXrihla7LVP+y2fL3bx40/32s=";
  })
  (fetchNuGet {
    pname = "System.Configuration.ConfigurationManager";
    version = "8.0.0";
    hash = "sha256-xhljqSkNQk8DMkEOBSYnn9lzCSEDDq4yO910itptqiE=";
  })
  (fetchNuGet {
    pname = "System.Diagnostics.Debug";
    version = "4.0.11";
    hash = "sha256-P+rSQJVoN6M56jQbs76kZ9G3mAWFdtF27P/RijN8sj4=";
  })
  (fetchNuGet {
    pname = "System.Diagnostics.EventLog";
    version = "8.0.0";
    hash = "sha256-rt8xc3kddpQY4HEdghlBeOK4gdw5yIj4mcZhAVtk2/Y=";
  })
  (fetchNuGet {
    pname = "System.Diagnostics.Tracing";
    version = "4.1.0";
    hash = "sha256-JA0jJcLbU3zh52ub3zweob2EVHvxOqiC6SCYHrY5WbQ=";
  })
  (fetchNuGet {
    pname = "System.Dynamic.Runtime";
    version = "4.0.11";
    hash = "sha256-qWqFVxuXioesVftv2RVJZOnmojUvRjb7cS3Oh3oTit4=";
  })
  (fetchNuGet {
    pname = "System.Formats.Asn1";
    version = "5.0.0";
    hash = "sha256-9nL3dN4w/dZ49W1pCkTjRqZm6Dh0mMVExNungcBHrKs=";
  })
  (fetchNuGet {
    pname = "System.Globalization";
    version = "4.0.11";
    hash = "sha256-rbSgc2PIEc2c2rN6LK3qCREAX3DqA2Nq1WcLrZYsDBw=";
  })
  (fetchNuGet {
    pname = "System.IO";
    version = "4.1.0";
    hash = "sha256-V6oyQFwWb8NvGxAwvzWnhPxy9dKOfj/XBM3tEC5aHrw=";
  })
  (fetchNuGet {
    pname = "System.IO";
    version = "4.3.0";
    hash = "sha256-ruynQHekFP5wPrDiVyhNiRIXeZ/I9NpjK5pU+HPDiRY=";
  })
  (fetchNuGet {
    pname = "System.IO.Pipelines";
    version = "7.0.0";
    hash = "sha256-W2181khfJUTxLqhuAVRhCa52xZ3+ePGOLIPwEN8WisY=";
  })
  (fetchNuGet {
    pname = "System.Linq";
    version = "4.1.0";
    hash = "sha256-ZQpFtYw5N1F1aX0jUK3Tw+XvM5tnlnshkTCNtfVA794=";
  })
  (fetchNuGet {
    pname = "System.Linq.Expressions";
    version = "4.1.0";
    hash = "sha256-7zqB+FXgkvhtlBzpcZyd81xczWP0D3uWssyAGw3t7b4=";
  })
  (fetchNuGet {
    pname = "System.Memory";
    version = "4.5.5";
    hash = "sha256-EPQ9o1Kin7KzGI5O3U3PUQAZTItSbk9h/i4rViN3WiI=";
  })
  (fetchNuGet {
    pname = "System.Numerics.Vectors";
    version = "4.4.0";
    hash = "sha256-auXQK2flL/JpnB/rEcAcUm4vYMCYMEMiWOCAlIaqu2U=";
  })
  (fetchNuGet {
    pname = "System.ObjectModel";
    version = "4.0.12";
    hash = "sha256-MudZ/KYcvYsn2cST3EE049mLikrNkmE7QoUoYKKby+s=";
  })
  (fetchNuGet {
    pname = "System.Reflection";
    version = "4.1.0";
    hash = "sha256-idZHGH2Yl/hha1CM4VzLhsaR8Ljo/rV7TYe7mwRJSMs=";
  })
  (fetchNuGet {
    pname = "System.Reflection";
    version = "4.3.0";
    hash = "sha256-NQSZRpZLvtPWDlvmMIdGxcVuyUnw92ZURo0hXsEshXY=";
  })
  (fetchNuGet {
    pname = "System.Reflection.Emit";
    version = "4.0.1";
    hash = "sha256-F1MvYoQWHCY89/O4JBwswogitqVvKuVfILFqA7dmuHk=";
  })
  (fetchNuGet {
    pname = "System.Reflection.Emit";
    version = "4.3.0";
    hash = "sha256-5LhkDmhy2FkSxulXR+bsTtMzdU3VyyuZzsxp7/DwyIU=";
  })
  (fetchNuGet {
    pname = "System.Reflection.Emit.ILGeneration";
    version = "4.0.1";
    hash = "sha256-YG+eJBG5P+5adsHiw/lhJwvREnvdHw6CJyS8ZV4Ujd0=";
  })
  (fetchNuGet {
    pname = "System.Reflection.Emit.ILGeneration";
    version = "4.3.0";
    hash = "sha256-mKRknEHNls4gkRwrEgi39B+vSaAz/Gt3IALtS98xNnA=";
  })
  (fetchNuGet {
    pname = "System.Reflection.Emit.Lightweight";
    version = "4.0.1";
    hash = "sha256-uVvNOnL64CPqsgZP2OLqNmxdkZl6Q0fTmKmv9gcBi+g=";
  })
  (fetchNuGet {
    pname = "System.Reflection.Extensions";
    version = "4.0.1";
    hash = "sha256-NsfmzM9G/sN3H8X2cdnheTGRsh7zbRzvegnjDzDH/FQ=";
  })
  (fetchNuGet {
    pname = "System.Reflection.Metadata";
    version = "1.3.0";
    hash = "sha256-a/RQr++mSsziWaOTknicfIQX/zJrwPFExfhK6PM0tfg=";
  })
  (fetchNuGet {
    pname = "System.Reflection.Metadata";
    version = "1.6.0";
    hash = "sha256-JJfgaPav7UfEh4yRAQdGhLZF1brr0tUWPl6qmfNWq/E=";
  })
  (fetchNuGet {
    pname = "System.Reflection.Metadata";
    version = "7.0.0";
    hash = "sha256-GwAKQhkhPBYTqmRdG9c9taqrKSKDwyUgOEhWLKxWNPI=";
  })
  (fetchNuGet {
    pname = "System.Reflection.Metadata";
    version = "8.0.0";
    hash = "sha256-dQGC30JauIDWNWXMrSNOJncVa1umR1sijazYwUDdSIE=";
  })
  (fetchNuGet {
    pname = "System.Reflection.MetadataLoadContext";
    version = "8.0.0";
    hash = "sha256-jS5XPZiHjY2CJFnLSxL6U7lMrU3ZknvB4EOgMbG0LEo=";
  })
  (fetchNuGet {
    pname = "System.Reflection.Primitives";
    version = "4.0.1";
    hash = "sha256-SFSfpWEyCBMAOerrMCOiKnpT+UAWTvRcmoRquJR6Vq0=";
  })
  (fetchNuGet {
    pname = "System.Reflection.Primitives";
    version = "4.3.0";
    hash = "sha256-5ogwWB4vlQTl3jjk1xjniG2ozbFIjZTL9ug0usZQuBM=";
  })
  (fetchNuGet {
    pname = "System.Reflection.TypeExtensions";
    version = "4.1.0";
    hash = "sha256-R0YZowmFda+xzKNR4kKg7neFoE30KfZwp/IwfRSKVK4=";
  })
  (fetchNuGet {
    pname = "System.Reflection.TypeExtensions";
    version = "4.3.0";
    hash = "sha256-4U4/XNQAnddgQIHIJq3P2T80hN0oPdU2uCeghsDTWng=";
  })
  (fetchNuGet {
    pname = "System.Resources.ResourceManager";
    version = "4.0.1";
    hash = "sha256-cZ2/3/fczLjEpn6j3xkgQV9ouOVjy4Kisgw5xWw9kSw=";
  })
  (fetchNuGet {
    pname = "System.Runtime";
    version = "4.1.0";
    hash = "sha256-FViNGM/4oWtlP6w0JC0vJU+k9efLKZ+yaXrnEeabDQo=";
  })
  (fetchNuGet {
    pname = "System.Runtime";
    version = "4.3.0";
    hash = "sha256-51813WXpBIsuA6fUtE5XaRQjcWdQ2/lmEokJt97u0Rg=";
  })
  (fetchNuGet {
    pname = "System.Runtime.CompilerServices.Unsafe";
    version = "4.5.3";
    hash = "sha256-lnZMUqRO4RYRUeSO8HSJ9yBHqFHLVbmenwHWkIU20ak=";
  })
  (fetchNuGet {
    pname = "System.Runtime.CompilerServices.Unsafe";
    version = "6.0.0";
    hash = "sha256-bEG1PnDp7uKYz/OgLOWs3RWwQSVYm+AnPwVmAmcgp2I=";
  })
  (fetchNuGet {
    pname = "System.Runtime.Extensions";
    version = "4.1.0";
    hash = "sha256-X7DZ5CbPY7jHs20YZ7bmcXs9B5Mxptu/HnBUvUnNhGc=";
  })
  (fetchNuGet {
    pname = "System.Security.AccessControl";
    version = "4.5.0";
    hash = "sha256-AFsKPb/nTk2/mqH/PYpaoI8PLsiKKimaXf+7Mb5VfPM=";
  })
  (fetchNuGet {
    pname = "System.Security.AccessControl";
    version = "5.0.0";
    hash = "sha256-ueSG+Yn82evxyGBnE49N4D+ngODDXgornlBtQ3Omw54=";
  })
  (fetchNuGet {
    pname = "System.Security.Cryptography.Cng";
    version = "5.0.0";
    hash = "sha256-nOJP3vdmQaYA07TI373OvZX6uWshETipvi5KpL7oExo=";
  })
  (fetchNuGet {
    pname = "System.Security.Cryptography.Pkcs";
    version = "5.0.0";
    hash = "sha256-kq/tvYQSa24mKSvikFK2fKUAnexSL4PO4LkPppqtYkE=";
  })
  (fetchNuGet {
    pname = "System.Security.Cryptography.ProtectedData";
    version = "4.4.0";
    hash = "sha256-Ri53QmFX8I8UH0x4PikQ1ZA07ZSnBUXStd5rBfGWFOE=";
  })
  (fetchNuGet {
    pname = "System.Security.Cryptography.ProtectedData";
    version = "8.0.0";
    hash = "sha256-fb0pa9sQxN+mr0vnXg1Igbx49CaOqS+GDkTfWNboUvs=";
  })
  (fetchNuGet {
    pname = "System.Security.Permissions";
    version = "4.5.0";
    hash = "sha256-Fa6dX6Gyse1A/RBoin8cVaHQePbfBvp6jjWxUXPhXKQ=";
  })
  (fetchNuGet {
    pname = "System.Security.Principal.Windows";
    version = "4.5.0";
    hash = "sha256-BkUYNguz0e4NJp1kkW7aJBn3dyH9STwB5N8XqnlCsmY=";
  })
  (fetchNuGet {
    pname = "System.Security.Principal.Windows";
    version = "5.0.0";
    hash = "sha256-CBOQwl9veFkrKK2oU8JFFEiKIh/p+aJO+q9Tc2Q/89Y=";
  })
  (fetchNuGet {
    pname = "System.Text.Encoding";
    version = "4.3.0";
    hash = "sha256-GctHVGLZAa/rqkBNhsBGnsiWdKyv6VDubYpGkuOkBLg=";
  })
  (fetchNuGet {
    pname = "System.Text.Encoding.CodePages";
    version = "7.0.0";
    hash = "sha256-eCKTVwumD051ZEcoJcDVRGnIGAsEvKpfH3ydKluHxmo=";
  })
  (fetchNuGet {
    pname = "System.Threading";
    version = "4.0.11";
    hash = "sha256-mob1Zv3qLQhQ1/xOLXZmYqpniNUMCfn02n8ZkaAhqac=";
  })
  (fetchNuGet {
    pname = "System.Threading.Channels";
    version = "7.0.0";
    hash = "sha256-Cu0gjQsLIR8Yvh0B4cOPJSYVq10a+3F9pVz/C43CNeM=";
  })
  (fetchNuGet {
    pname = "System.Threading.Tasks";
    version = "4.0.11";
    hash = "sha256-5SLxzFg1df6bTm2t09xeI01wa5qQglqUwwJNlQPJIVs=";
  })
  (fetchNuGet {
    pname = "System.Threading.Tasks";
    version = "4.3.0";
    hash = "sha256-Z5rXfJ1EXp3G32IKZGiZ6koMjRu0n8C1NGrwpdIen4w=";
  })
  (fetchNuGet {
    pname = "System.Threading.Tasks.Dataflow";
    version = "4.6.0";
    hash = "sha256-YYrT3GRzVBdendxt8FUDCnOBJi0nw/CJ9VrzcPJWLSg=";
  })
  (fetchNuGet {
    pname = "System.Threading.Tasks.Dataflow";
    version = "8.0.0";
    hash = "sha256-Q6fPtMPNW4+SDKCabJzNS+dw4B04Oxd9sHH505bFtQo=";
  })
  (fetchNuGet {
    pname = "System.Threading.Tasks.Extensions";
    version = "4.5.4";
    hash = "sha256-owSpY8wHlsUXn5xrfYAiu847L6fAKethlvYx97Ri1ng=";
  })
  (fetchNuGet {
    pname = "xunit";
    version = "2.8.1";
    hash = "sha256-NhKfxMikb42angTu9yRsaIuJxBJrJtrWP0bmcd405i4=";
  })
  (fetchNuGet {
    pname = "xunit.abstractions";
    version = "2.0.3";
    hash = "sha256-0D1y/C34iARI96gb3bAOG8tcGPMjx+fMabTPpydGlAM=";
  })
  (fetchNuGet {
    pname = "xunit.analyzers";
    version = "1.14.0";
    hash = "sha256-raqyuXO8VJ6qBOxtqRLJa/oMQ7YfgwaOK06onRome2Q=";
  })
  (fetchNuGet {
    pname = "xunit.assert";
    version = "2.8.1";
    hash = "sha256-PzTY1UeDK4t22o3GjAV1qV/zsXe/N4yOQsFu5l8Zu/A=";
  })
  (fetchNuGet {
    pname = "xunit.core";
    version = "2.8.1";
    hash = "sha256-BWKwaa6V/c+vLHoz46vhDv1wdMSgw2poObCB2lCWgeE=";
  })
  (fetchNuGet {
    pname = "xunit.extensibility.core";
    version = "2.8.1";
    hash = "sha256-0v8EF9Xt/QYX1nlxf5xBEu4UZ51eI+9xpkJP3xk3grU=";
  })
  (fetchNuGet {
    pname = "xunit.extensibility.execution";
    version = "2.8.1";
    hash = "sha256-3cG83t0ee9yvvCOPXG0XWcKqu3b3AJGkvHW9QGwiYVw=";
  })
  (fetchNuGet {
    pname = "xunit.runner.console";
    version = "2.8.1";
    hash = "sha256-+2l6oWmPSZ6r0Xj/mflmBekeEnUjddiqAFx/wNk2cuc=";
  })
  (fetchNuGet {
    pname = "xunit.runner.visualstudio";
    version = "2.8.1";
    hash = "sha256-ZcFsH0sPMhaoVRYfsST+SBER4F8CH1zpqVF9TJb7E5g=";
  })
]
