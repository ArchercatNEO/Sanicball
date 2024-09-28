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
            godot = super.godot_4.overrideAttrs (finalAttrs: prevAttrs: {
              version = "4.4-dev2";
              src = pkgs.fetchFromGitHub {
                owner = "godotengine";
                repo = "godot";
                rev = "97ef3c837263099faf02d8ebafd6c77c94d2aaba";
                hash = "sha256-atLDiSjmHD7JCrPqvQEUmKJVNnv6wuCnleSIHjImU/g=";
              };
            });
            
            godot-template = super.callPackage ./nix/godot-template/package.nix {};
            godot-dotnet = super.callPackage ./nix/godot-dotnet/package.nix {};
          })
        ];
      in {
        devShells.default = import ./nix/shell.nix pkgs;

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
