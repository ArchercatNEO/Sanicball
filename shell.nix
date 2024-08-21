{
  mkShell,
  godot,
  dotnetCorePackages,
  blender,
  ...
}: {
  default = mkShell {
    name = "sanicball-generic-shell";

    packages = [
      godot
      dotnetCorePackages.sdk_8_0
    ];
  };

  godot = mkShell {
    name = "sanicball-godot-shell";

    packages = [
      godot
      dotnetCorePackages.sdk_8_0
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
