{
    description = "The sanicball dev environment";

    inputs = {
        nixpkgs.url = "github:nixos/nixpkgs/nixos-23.11";
        nixunstable.url = "github:nixos/nixpkgs/nixos-unstable";
    };

    outputs = { self, nixpkgs, nixunstable }:
    let
        system = "x86_64-linux";
        pkgs = import nixunstable {
            inherit system;
        };
    in {

        packages."${system}".default = pkgs.mkPackage {

        };

        devShells."${system}".default = pkgs.mkShell {

            packages = with pkgs; [
                dotnet-sdk_8
                godot_4
            ];
        };
    };
}
