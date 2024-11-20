{ fetchNuGet }:
[
  (fetchNuGet {
    pname = "Microsoft.DotNet.ILCompiler";
    version = "9.0.0-rc.2.24473.5";
    hash = "sha256-KlBEQYRSy00ZH+bFkG0xPpwT7qOmm47bBgeblYtI5G4=";
  })
  (fetchNuGet {
    pname = "Microsoft.NET.ILLink.Tasks";
    version = "9.0.0-rc.2.24473.5";
    hash = "sha256-FJ+4ZPyU6LZLGr0c/zffRGH2lpNrclbaC3V8a8u9kro=";
  })
  (fetchNuGet {
    pname = "runtime.linux-x64.Microsoft.DotNet.ILCompiler";
    version = "9.0.0-rc.2.24473.5";
    hash = "sha256-v5lk9Y0Krg3P6umdHnB+3RORoovXRzUkKjmd0WWLtB8=";
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
]
