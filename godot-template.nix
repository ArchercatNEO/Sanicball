{
  lib,
  stdenv,
  fetchzip,
  autoPatchelfHook,
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
  wayland,
  ...
}:

stdenv.mkDerivation {
    pname = "godot-template";
    version = "4.3-stable";

    src = fetchzip {
      pname = "export_templates";
      extension = "zip";
      url = "https://github.com/godotengine/godot/releases/download/4.3-stable/Godot_v4.3-stable_mono_export_templates.tpz";
      hash = "sha256-4cJL45RGi7jynmucUnRrb1VXpfa1QWxLYIqMFH8Znu4=";
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
      wayland
    ];

    installPhase = ''
      runHook preInstall
      mkdir -p $out/lib
      cp -r . $out/lib
      runHook postInstall
    '';
}