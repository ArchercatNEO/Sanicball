{
  lib,
  stdenv,
  patchelf,

  godot,
  godot-template,

  dotnetCorePackages,

  icu,
  ...
}: 
  stdenv.mkDerivation {
    pname = "sanicball";
    version = "0.2-alpha";

    src = ./.;

    nativeBuildInputs = [
      patchelf
      dotnetCorePackages.sdk_8_0
      godot
    ];

    buildInputs = [
      icu
    ];

    configurePhase = ''
      ln -s ${godot-template}/lib templates
    '';

    buildPhase = ''
      godot --verbose --headless --export-debug "Linux"
    '';

    installPhase = ''
      mkdir -p $out/bin
      cp -r ./bin $out/bin
    '';

    fixupPhase = ''
      patchelf --add-rpath ${icu}/lib $out/bin/data_Sanicball_linux_x86_64/libcoreclr.so
      patchelf --add-rpath ${icu}/lib $out/bin/data_Sanicball_linux_x86_64/libSystem.Globalization.Native.so
    '';

    meta = {
      homepage = "https://github.com/ArchercatNEO/Sanicball";
      description = "The incredibly fast racer";
      licesnse = lib.licenses.mit;
    };
  }
