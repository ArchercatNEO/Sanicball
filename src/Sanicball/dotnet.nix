{
  self,
  lib,
  buildDotnetModule,
  dotnetCorePackages,
  gcc,
  icu,
  openssl,
  zlib,
  ...
}:
buildDotnetModule {
  pname = "sanicball-extension";
  version = "0.2-alpha";

  src = self;
  projectFile = "${self}/src/Sanicball/Sanicball.csproj";
  nugetDeps = ./deps.nix;

  dotnet-sdk = dotnetCorePackages.sdk_9_0;
  dotnet-runtime = dotnetCorePackages.runtime_9_0;

  nativeBuildInputs = [
    gcc
  ];

  buildInputs = [
    icu
    openssl
    zlib
  ];

  meta = {
    homepage = "https://github.com/ArchercatNEO/Sanicball";
    description = "The incredibly fast racer";
    licesnse = lib.licenses.mit;
  };
}
