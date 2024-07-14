{
  lib,
  stdenv,
  fetchFromGitHub,
  pkg-config,
  autoPatchelfHook,
  installShellFiles,
  scons,
  dotnet-sdk_8,
  vulkan-loader,
  libGL,
  xorg,
  libxkbcommon,
  alsa-lib,
  libpulseaudio,
  dbus,
  speechd,
  fontconfig,
  udev,
}: let
  system = "x86_64-linux";
  mkSconsFlagsFromAttrSet = lib.mapAttrsToList (
    k: v:
      if builtins.isString v
      then "${k}=${v}"
      else "${k}=${builtins.toJSON v}"
  );
in
  stdenv.mkDerivation {
    pname = "godot";
    version = "4.3-beta";

    src = fetchFromGitHub {
      owner = "godotengine";
      repo = "godot";
      rev = "97b8ad1af0f2b4a216f6f1263bef4fbc69e56c7b";
      hash = "sha256-Q8Y6tHASBA47e/61GrKX1IXR6l9msufJ2bFSgkaE4VQ=";
    };

    nativeBuildInputs = [
      pkg-config
      autoPatchelfHook
      installShellFiles
      scons
    ];

    buildInputs = [
      dotnet-sdk_8
    ];

    runtimeDependencies = [
      vulkan-loader
      libGL
      xorg.libX11
      xorg.libXcursor
      xorg.libXinerama
      xorg.libXext
      xorg.libXrandr
      xorg.libXrender
      xorg.libXi
      xorg.libXfixes
      libxkbcommon
      alsa-lib
      libpulseaudio
      dbus
      dbus.lib
      speechd
      fontconfig
      fontconfig.lib
      udev
    ];

    enableParallelBuilding = true;

    BUILD_NAME = "nixpkgs";

    sconsFlags = mkSconsFlagsFromAttrSet {
      # Options from 'SConstruct'
      production = true; # Set defaults to build Godot for use in production
      platform = "linuxbsd";
      target = "editor";
      precision = "single"; # Floating-point precision level
      debug_symbols = true;
      module_mono_enable = "yes";

      # Options from 'platform/linuxbsd/detect.py'
      pulseaudio = true; # Use PulseAudio
      dbus = true; # Use D-Bus to handle screensaver and portal desktop settings
      speechd = true; # Use Speech Dispatcher for Text-to-Speech support
      fontconfig = true; # Use fontconfig for system fonts support
      udev = true; # Use udev for gamepad connection callbacks
      touch = true; # Enable touch events
    };

    dontStrip = true;

    outputs = ["out" "man"];

    preConfigure = ''
      mkdir -p .git
      echo AAAAAAAAAAAA > .git/HEAD
    '';

    preInstall = ''
      ./bin/godot.* --headless --generate-mono-glue modules/mono/glue
      ./modules/mono/build_scripts/build_assemblies.py --godot-output-dir=./bin
      ./modules/mono/build_scripts/build_assemblies.py --godot-output-dir ./bin --push-nupkgs-local ./lib
    '';

    installPhase = ''
      runHook preInstall

      mkdir -p "$out/lib"
      mkdir -p "$out/bin"
      cp ./bin/godot.* $out/lib/godot4
      cp -r ./bin/GodotSharp $out/lib
      ln -s $out/bin/godot4 $out/lib/godot4

      installManPage misc/dist/linux/godot.6

      mkdir -p "$out"/share/{applications,icons/hicolor/scalable/apps}
      cp misc/dist/linux/org.godotengine.Godot.desktop "$out/share/applications/org.godotengine.Godot4.desktop"
      substituteInPlace "$out/share/applications/org.godotengine.Godot4.desktop" \
        --replace "Exec=godot" "Exec=$out/bin/godot4" \
        --replace "Godot Engine" "Godot Engine 4"
      cp icon.svg "$out/share/icons/hicolor/scalable/apps/godot.svg"
      cp icon.png "$out/share/icons/godot.png"

      runHook postInstall
    '';
  }
