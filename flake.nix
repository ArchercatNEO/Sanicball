{
  description = "The sanicball dev environment";

  inputs = {
    nixpkgs.url = "github:nixos/nixpkgs/nixos-unstable";
    flake-utils.url = "github:numtide/flake-utils";
  };

  outputs = {
    self,
    nixpkgs,
    flake-utils,
  }:
    flake-utils.lib.eachSystem ["x86_64-linux"] (
      system: let
        pkgs = import nixpkgs {inherit system overlays;};
        overlays = [
          (self: super: {
            godot = super.godot_4;
            godot-template = super.callPackage ./nix/godot-template.nix {};
            godot-dotnet = super.callPackage ./nix/godot-dotnet/package.nix {};
          })
        ];
      in {
        devShells.default = pkgs.callPackage ./nix/shell.nix {};

        packages = rec {
          default = sanicball;
          sanicball = pkgs.callPackage ./Sanicball {};
          templates = pkgs.godot-template;
          gdextension = pkgs.godot-dotnet;
        };

        apps = {
          default = {
            type = "app";
            program = "${self.packages."${system}".sanicball}";
          };
        };
      }
    );
}
