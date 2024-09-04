{
  lib,
  godot,
  godot-template,
  godot-dotnet,
  buildDotnetModule,
  dotnetCorePackages,
  ...
}:
buildDotnetModule {
  pname = "sanicball";
  version = "0.2-alpha";

  src = ./.;

  dotnet-sdk = dotnetCorePackages.sdk_8_0;
  dotnet-runtime = dotnetCorePackages.runtime_8_0;
  projectFile = ./Sanicball.csproj;
  nugetDeps = ./deps.nix;

  nativeBuildInputs = [
    godot
  ];

  buildInputs = [
    godot-template
  ];

  projectReferences = [
    godot-dotnet
  ];

  meta = {
    homepage = "https://github.com/ArchercatNEO/Sanicball";
    description = "The incredibly fast racer";
    licesnse = lib.licenses.mit;
  };
}
