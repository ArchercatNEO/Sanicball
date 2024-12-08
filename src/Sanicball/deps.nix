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
    version = "3.11.0";
    hash = "sha256-hQ2l6E6PO4m7i+ZsfFlEx+93UsLPo4IY3wDkNG11/Sw=";
  })
  (fetchNuGet {
    pname = "Microsoft.CodeAnalysis.Analyzers";
    version = "3.3.4";
    hash = "sha256-qDzTfZBSCvAUu9gzq2k+LOvh6/eRvJ9++VCNck/ZpnE=";
  })
  (fetchNuGet {
    pname = "Microsoft.CodeAnalysis.Common";
    version = "4.12.0";
    hash = "sha256-mm/OKG3zPLAeTVGZtuLxSG+jpQDOchn1oyHqBBJW2Ho=";
  })
  (fetchNuGet {
    pname = "Microsoft.CodeAnalysis.CSharp";
    version = "4.12.0";
    hash = "sha256-m1i1Q5pyEq4lAoYjNE9baEjTplH8+bXx5wSA+eMmehk=";
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
    version = "4.1.0";
    hash = "sha256-r89nJ5JE5uZlsRrfB8QJQ1byVVfCWQbySKQ/m9PYj0k=";
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
    version = "8.0.0";
    hash = "sha256-F7OVjKNwpqbUh8lTidbqJWYi476nsq9n+6k0+QVRo3w=";
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
    pname = "System.Numerics.Vectors";
    version = "4.5.0";
    hash = "sha256-qdSTIFgf2htPS+YhLGjAGiLN8igCYJnCCo6r78+Q+c8=";
  })
  (fetchNuGet {
    pname = "System.Reflection.Metadata";
    version = "8.0.0";
    hash = "sha256-dQGC30JauIDWNWXMrSNOJncVa1umR1sijazYwUDdSIE=";
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
