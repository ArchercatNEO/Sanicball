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
    flake-utils.lib.eachSystem ["x86_64-linux"] (system: let
      pkgs = import nixpkgs {inherit system overlays;};
      overlays = [
        (self: super: {
          godot = super.callPackage ./godot.nix {};
          godot-template = super.callPackage ./godot-template.nix {};
          })
      ];
    in {
        devShells = {
          inherit (pkgs.callPackage ./shell.nix {}) default blender godot;
        };

        packages = rec {
          default = sanicball;
          sanicball = pkgs.callPackage ./Sanicball {};
          templates = pkgs.godot-template;
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
