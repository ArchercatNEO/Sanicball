{ pkgs, ... }:
{
  projectRootFile = "flake.nix";

  settings.globals.excludes = [
    "./direnv/**"
  ];

  programs.nixfmt.enable = true;
  programs.csharpier.enable = true;
}
