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
    flake-utils.lib.eachSystem [ "x86_64-linux" ] (system: let
      pkgs = import nixpkgs {inherit system overlays;};
      overlays = [
        (self: super: {
          godot = super.callPackage ./godot2.nix {};
        })
      ];
    in {
      devShells = pkgs.callPackage ./shell.nix {};

      packages = rec {
        default = sanicball;
        sanicball = pkgs.callPackage ./Sanicball {};
      };

      apps = {
        default = {
          type = "app";
          program = "${self.packages."${system}".sanicball}";
        };
      };
    });
}
