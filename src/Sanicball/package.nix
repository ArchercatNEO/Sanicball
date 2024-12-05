{
  self,
  stdenv,
  callPackage,
  godot,
  godot-template,
  ...
}:
let
  dotnet = callPackage ./dotnet.nix {
    inherit self;
  };
in
stdenv.mkDerivation {
  pname = "sanicball";
  version = "0.0.0";

  src = self;

  configurePhase = ''
    mkdir ./templates
    ln -s ${godot-template}/bin/godot4 ./templates/x86_64-linux-release
  '';

  buildPhase = ''
    mkdir -p $out/bin
    ${godot}/bin/godot4 --export-release Linux $out/bin
  '';

  passthru = {
    inherit dotnet;
  };
}
