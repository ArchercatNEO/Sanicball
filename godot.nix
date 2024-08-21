{
  lib,
  stdenv,
  fetchzip,
  autoPatchelfHook,
  vulkan-loader,
  libGL,
  xorg,
  libxkbcommon,
  libdecor,
  alsa-lib,
  libpulseaudio,
  dbus,
  speechd,
  fontconfig,
  udev,
  wayland,
  ...
}:

stdenv.mkDerivation {
    pname = "godot";
    version = "4.3-stable";

    src = fetchzip {
      url = "https://github.com/godotengine/godot/releases/download/4.3-stable/Godot_v4.3-stable_mono_linux_x86_64.zip";
      hash = "sha256-L32cwE/E1aEAz6t3SlO0k/QQuKRt/8lJntfdCYVdGCE=";
    };

    nativeBuildInputs = [
      autoPatchelfHook
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
      libdecor
      wayland
    ];

    sourceRoot = ".";

    installPhase = ''
      runHook preInstall
      mkdir -p $out/bin
      cp ./source/Godot_* $out/bin/godot
      cp -r ./source/GodotSharp $out/bin
      chmod 0755 $out/bin/godot
      runHook postInstall
    '';

    meta = {
      homepage = "https://godot.org";
      description = "An open source 2D and 3D engine";
      license = lib.licenses.mit;
      platforms = lib.platforms.linux;
    };
}
