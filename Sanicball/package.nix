{
  lib,
  stdenv,
  godot,
  godot-template,
  ...
}: stdenv.mkDerivation {
  pname = "sanicball";
  version = "0.0.0";

  src = ./.;

  configurePhase = ''
    mkdir ./templates
    ln -s ${godot-template}/bin/godot4 ./templates/x86_64-linux-release
  '';

  buildPhase = ''
    mkdir -p $out/bin
    ${godot}/bin/godot4 --export-release Linux $out/bin
  '';
}