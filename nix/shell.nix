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

    nativeBuildInputs = [
      blender
      dotnetCorePackages.sdk_9_0
      stdenv.cc
      godot
    ];

    buildInputs = [
      icu
      openssl
      zlib
      zlib.dev
    ];

    env = {
      NIX_LD_LIBRARY_PATH = lib.makeLibraryPath [
        zlib
        zlib.dev
        stdenv.cc.cc.lib
        openssl
        icu
      ];
      LIBRARY_PATH = lib.makeLibraryPath [
        icu
        openssl
        zlib
        zlib.dev
      ];
    };

    shellHook = ''
      DOTNET_ROOT=${dotnet}

      DOTNET_NOLOGO=1
      DOTNET_CLI_TELEMETRY_OPTOUT=1
    '';
  }
