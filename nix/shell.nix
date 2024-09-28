pkgs: let
  dotnet = with pkgs.dotnetCorePackages;
    combinePackages [
      sdk_8_0
      sdk_9_0
    ];
  
  libraries = pkgs.lib.makeLibraryPath [
    pkgs.stdenv.cc.cc.lib
    pkgs.stdenv.cc.libc
    dotnet.passthru.icu
    pkgs.openssl
    pkgs.zlib
  ];
in
  pkgs.mkShell {
    name = "sanicball-shell";

    nativeBuildInputs = [
      pkgs.blender
      dotnet
      pkgs.stdenv.cc
      pkgs.godot
    ];

    env = {
      NIX_LD_LIBRARY_PATH = libraries;
      LIBRARY_PATH = libraries;
    };

    shellHook = ''
      DOTNET_ROOT=${dotnet}

      DOTNET_NOLOGO=1
      DOTNET_CLI_TELEMETRY_OPTOUT=1
    '';
  }
