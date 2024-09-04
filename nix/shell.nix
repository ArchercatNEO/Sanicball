{
  lib,
  stdenv,
  mkShell,
  blender,
  dotnetCorePackages,
  godot,
  zlib,
  openssl,
  icu,
  ...
}: let
  dotnet = with dotnetCorePackages;
    combinePackages [
      sdk_6_0
      sdk_8_0
    ];
in
  mkShell {
    name = "sanicball-shell";

    packages = [
      blender
      dotnet
      stdenv.cc
      godot
    ];

    env = {
      NIX_LD = "${stdenv.cc}/nix-support/dynamic-linker";
      NIX_LD_LIBRARY_PATH = lib.makeLibraryPath [
        zlib
        zlib.dev
        openssl
        icu
      ];
      LIBRARY_PATH = lib.makeLibraryPath [
        zlib
      ];
    };

    shellHook = ''
      DOTNET_ROOT=${dotnet}

      DOTNET_NOLOGO=1
      DOTNET_CLI_TELEMETRY_OPTOUT=1
    '';
  }
