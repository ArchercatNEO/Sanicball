{ mkShell
, dotnet-sdk_8
, godot
, blender
, ...
}: {
  default = mkShell {
    name = "sanicball-generic-shell";

    packages = [
      dotnet-sdk_8
      godot
    ];
  };

  godot = mkShell {
    name = "sanicball-godot-shell";

    packages = [
      godot
      dotnet-sdk_8
    ];

    shellHook = ''
      godot ./Sanicball/project.godot
    '';
  };

  blender = mkShell {
    name = "sanicball-blender-shell";

    packages = [
      blender
    ];

    shellHook = ''
      blender
    '';
  };
}
