{
  buildDotnetModule,
  dotnetCorePackages,
  godot-dotnet,
  ...
}: buildDotnetModule {
  pname = "serilog.sinks.godot";
  version = "1.0.0";

  src = ./.;
  projectFile = "./Serilog.Sinks.Godot.csproj";
  nugetDeps = ./deps.nix;

  dotnet-sdk = dotnetCorePackages.sdk_9_0;
  dotnet-runtime = dotnetCorePackages.runtime_9_0;

  projectReferences = [
    godot-dotnet
  ];

  packNupkg = true;
}