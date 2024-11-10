{
  lib,
  mkShell,
  stdenv,
  dotnetCorePackages,
  blender,
  nuget-to-nix,
  gdb,
  godot,
  openssl,
  python3,
  zlib,
  ...
}:
let
  dotnet =
    with dotnetCorePackages;
    combinePackages [
      sdk_8_0
      sdk_9_0
    ];

  libraries = lib.makeLibraryPath [
    stdenv.cc.cc.lib
    dotnet.icu
    openssl
    zlib
  ];
in
mkShell {
  name = "sanicball-shell";

  packages = [
    blender
    dotnet
    nuget-to-nix
    godot
    python3
  ];

  env = {
    NIX_LD_LIBRARY_PATH = libraries;
    NIX_DBG = "${gdb}";
    NIX_GODOT = "${godot}";

    DOTNET_ROOT = "${dotnet}";
    DOTNET_NOLOGO = true;
    DOTNET_CLI_TELEMETRY_OPTOUT = true;
  };
}
