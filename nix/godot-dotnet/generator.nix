{
  pname,
  version,
  commit,
  src,
  nativeBuildInputs,
  preConfigure,
  dotnetPackFlags,
  buildDotnetModule,
  dotnetCorePackages,
  icu,
  krb5,
  openssl,
  ...
}: buildDotnetModule {
  pname = pname + "-source-generators";
  inherit version commit src;

  projectFile = "./src/Godot.SourceGenerators/Godot.SourceGenerators.csproj";
  nugetDeps = ./deps.nix;
  
  dotnet-sdk = dotnetCorePackages.sdk_8_0;
  dotnet-runtime = dotnetCorePackages.runtime_8_0;

  inherit nativeBuildInputs;

  buildInputs = [
    icu
    krb5
    krb5.dev
    openssl
  ];

  inherit preConfigure;

  packNupkg = true;
  inherit dotnetPackFlags;

  dontFixup = true;
}
