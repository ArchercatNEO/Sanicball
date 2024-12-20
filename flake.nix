{
  description = "The sanicball dev environment";

  nixConfig = {
    experimental-features = [
      "flakes"
      "nix-command"
      "pipe-operators"
    ];
  };

  inputs = {
    nixpkgs.url = "github:nixos/nixpkgs/nixos-unstable";
    flake-utils.url = "github:numtide/flake-utils";

    treefmt = {
      url = "github:numtide/treefmt-nix";
      inputs.nixpkgs.follows = "nixpkgs";
    };
  };

  outputs =
    {
      self,
      nixpkgs,
      flake-utils,
      treefmt,
    }:
    flake-utils.lib.eachSystem [ "x86_64-linux" ] (
      system:
      let
        pkgs = import nixpkgs { inherit system; };

        godot = pkgs.callPackage ./nix/godot/package.nix { };

        godot-template = godot.override {
          withTarget = "template_debug";
        };

        godot-dotnet = pkgs.callPackage ./nix/godot-dotnet/package.nix { };

        callPackage = pkgs.lib.callPackageWith (
          pkgs
          // {
            inherit
              self
              godot
              godot-template
              godot-dotnet
              ;
          }
        );

        packages = {
          sanicball = callPackage ./src/Sanicball/package.nix { };
          serilog = callPackage ./src/Serilog.Sinks.Godot/package.nix { };
          editor = godot;
          templates = godot-template;
          gdextension = godot-dotnet;
        };
      in
      {
        devShells.default = callPackage ./shell.nix { };

        packages = packages // {
          default = pkgs.linkFarm "sanicball-artifacts" {
            sanicball = packages.sanicball;
            godot = godot;
            godot-dotnet = godot-dotnet;
          };
        };

        formatter = (treefmt.lib.evalModule pkgs ./treefmt.nix).config.build.wrapper;
      }
    );
}
