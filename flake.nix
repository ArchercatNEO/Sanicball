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
            godot = import ./nix/godot/package.nix pkgs;
            godot-template = import ./nix/godot-template/package.nix pkgs;
            godot-dotnet = super.callPackage ./nix/godot-dotnet/package.nix {};
          })
        ];
      in {
        devShells.default = import ./nix/shell.nix pkgs;

        packages = rec {
          default = sanicball;
          sanicball = pkgs.callPackage ./Sanicball {};
          inner = pkgs.callPackage ./Sanicball/dotnet.nix {};
          serilog = pkgs.callPackage ./Serilog.Sinks.Godot/package.nix {};
          editor = pkgs.godot;
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
