{
  description = "The sanicball dev environment";

  inputs = {
    nixpkgs.url = "github:nixos/nixpkgs/nixos-23.11";
  };

  outputs = { self, nixpkgs }:
  let 
    system = "x86_64-linux"; 
    pkgs = import nixpkgs {
        inherit system;
      };
  in {
    devShells."${system}".default = pkgs.mkShell {
      
      packages = with pkgs; [
        dotnet-sdk_8
        godot_4
      ];

      shellHook = ''
        dotnet ${pkgs.dotnet-sdk_8}/bin/dotnet --version
      '';
    };
  };
}
