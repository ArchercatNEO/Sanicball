{ fetchNuGet }:
[
  (fetchNuGet {
    pname = "Godot.Bindings";
    version = "4.4.0-nightly.24509.1";
    hash = "sha256-nS0S5qispFNXltcKq8nphzkXzUEODokjY6F4ZhK48sE=";
    url = "https://pkgs.dev.azure.com/godotengine/62f77a5d-d037-44c4-95bf-469bad79099d/_packaging/e89653fe-4a0b-4506-9b80-1bcd5c958c0b/nuget/v3/flat2/godot.bindings/4.4.0-nightly.24509.1/godot.bindings.4.4.0-nightly.24509.1.nupkg";
  })
  (fetchNuGet {
    pname = "Godot.SourceGenerators";
    version = "4.4.0-nightly.24509.1";
    hash = "sha256-2Ytkbsyx8RfFgmKrU3SBD5t1/P+OgucI6168U0/1aao=";
    url = "https://pkgs.dev.azure.com/godotengine/62f77a5d-d037-44c4-95bf-469bad79099d/_packaging/e89653fe-4a0b-4506-9b80-1bcd5c958c0b/nuget/v3/flat2/godot.sourcegenerators/4.4.0-nightly.24509.1/godot.sourcegenerators.4.4.0-nightly.24509.1.nupkg";
  })
  (fetchNuGet {
    pname = "Microsoft.CodeAnalysis.Analyzers";
    version = "3.3.4";
    hash = "sha256-qDzTfZBSCvAUu9gzq2k+LOvh6/eRvJ9++VCNck/ZpnE=";
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
    pname = "Microsoft.NETCore.Platforms";
    version = "1.1.0";
    hash = "sha256-FeM40ktcObQJk4nMYShB61H/E8B7tIKfl9ObJ0IOcCM=";
  })
  (fetchNuGet {
    pname = "NETStandard.Library";
    version = "2.0.3";
    hash = "sha256-Prh2RPebz/s8AzHb2sPHg3Jl8s31inv9k+Qxd293ybo=";
  })
  (fetchNuGet {
    pname = "runtime.linux-x64.Microsoft.DotNet.ILCompiler";
    version = "9.0.0";
    hash = "sha256-3xkJe6dOfJnG4LhN/147lFFDekYujwPwP0OknKH0wmc=";
  })
  (fetchNuGet {
    pname = "Serilog";
    version = "4.0.0";
    hash = "sha256-j8hQ5TdL1TjfdGiBO9PyHJFMMPvATHWN1dtrrUZZlNw=";
  })
  (fetchNuGet {
    pname = "Serilog";
    version = "4.0.2";
    hash = "sha256-vkd4s/PsKnnVzN1+f9haIP5LoxNWxnhdv3mBQYl/2Hc=";
  })
  (fetchNuGet {
    pname = "Serilog.Sinks.Console";
    version = "6.0.0";
    hash = "sha256-QH8ykDkLssJ99Fgl+ZBFBr+RQRl0wRTkeccQuuGLyro=";
  })
  (fetchNuGet {
    pname = "System.Buffers";
    version = "4.5.1";
    hash = "sha256-wws90sfi9M7kuCPWkv1CEYMJtCqx9QB/kj0ymlsNaxI=";
  })
  (fetchNuGet {
    pname = "System.Collections.Immutable";
    version = "7.0.0";
    hash = "sha256-9an2wbxue2qrtugYES9awshQg+KfJqajhnhs45kQIdk=";
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
    pname = "System.Reflection.Metadata";
    version = "7.0.0";
    hash = "sha256-GwAKQhkhPBYTqmRdG9c9taqrKSKDwyUgOEhWLKxWNPI=";
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
    pname = "System.Text.Encoding.CodePages";
    version = "7.0.0";
    hash = "sha256-eCKTVwumD051ZEcoJcDVRGnIGAsEvKpfH3ydKluHxmo=";
  })
  (fetchNuGet {
    pname = "System.Threading.Tasks.Extensions";
    version = "4.5.4";
    hash = "sha256-owSpY8wHlsUXn5xrfYAiu847L6fAKethlvYx97Ri1ng=";
  })
]
