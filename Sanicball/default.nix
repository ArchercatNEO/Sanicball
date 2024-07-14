{
  debug ? true,
  lib,
  stdenv,
  bash,
  buildDotnetModule,
  dotnetCorePackages,
  godot,
  ...
}: let
  dotnet = buildDotnetModule {
    pname = "sanicball";
    version = "0.2-alpha";

    src = ./.;

    projectFile = "./Sanicball.csproj";
    dotnet-sdk = dotnetCorePackages.sdk_9_0;
    dotnet-runtime = dotnetCorePackages.runtime_9_0;
    nugetDeps = ./deps.nix;

    meta = {
      homepage = "https://github.com/ArchercatNEO/Sanicball";
      description = "The incredibly fast racer";
      licesnse = lib.licenses.mit;
    };
  };
in
  stdenv.mkDerivation {
    pname = "sanicball";
    version = "0.0";

    src = ./.;

    nativeBuildInputs = [
      dotnetCorePackages.sdk_9_0
    ];

    runtimeDependencies = [
      godot
    ];

    buildPhase = ''
      dotnet build
    '';

    installPhase = ''
      mkdir -p $out/share
      mkdir $out/bin
      cp -r . $out/share
      cp -r ./.godot $out/share/
      echo "#!${bash/bin/bash}" &> $out/bin/sanicball
      echo "godot4 --path $out/share/project.godot --verbose" &> $out/bin/sanicball
      chmod +x $out/bin/sanicball
    '';
  }
