{
  self,
  lib,
  stdenv,
  dotnetCorePackages,
  godot,
  godot-template,
  gcc,
  icu,
  openssl,
  zlib,
  ...
}:
let
  attrs = {
    pname = "sanicball";
    version = "0.0.0";

    src = self;

    nativeBuildInputs = [
      gcc
      dotnetCorePackages.sdk_9_0
    ];

    buildInputs = [
      godot-template
      icu
      openssl
      zlib
    ];

    configurePhase = ''
      mkdir -p ./templates
      ln -s ${godot-template}/bin/ ./templates/
    '';

    buildPhase = ''
      mkdir -p $out/bin
      dotnet publish -c Release
      cd ./src/Sanicball
      ${godot}/bin/godot4 --headless --export-release Linux $out/bin
    '';
  };
in
attrs
|> dotnetCorePackages.addNuGetDeps {
  nugetDeps = ./deps.nix;
  overrideFetchAttrs = old: rec {
    runtimeIds = map (system: dotnetCorePackages.systemToDotnetRid system) old.meta.platforms;
    buildInputs =
      old.buildInputs
      ++ lib.concatLists (
        lib.attrValues (lib.getAttrs runtimeIds dotnetCorePackages.sdk_9_0.targetPackages)
      );
  };
}
|> stdenv.mkDerivation
