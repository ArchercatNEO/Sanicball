{ lib
, stdenv
, fetchzip
, autoPatchelfHook
, vulkan-loader
, libGL
, xorg
, libxkbcommon
, alsa-lib
, libpulseaudio
, dbus
, speechd
, fontconfig
, udev
, ...
}:

stdenv.mkDerivation {
  pname = "godot";
  version = "4.3-beta";

  src = fetchzip {
    url = "https://github.com/godotengine/godot-builds/releases/download/4.3-beta3/Godot_v4.3-beta3_mono_linux_x86_64.zip";
    hash = "sha256-HHz/5FN+bqqz2OeSgI5upFFEdCUaOjVpaS4frHqWw7k=";
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
  ];
 
  sourceRoot = ".";

  installPhase = ''
    runHook preInstall
    mkdir -p $out/bin
    cp ./source/Godot_v4.3-beta3_mono_linux.x86_64 $out/bin/godot
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
