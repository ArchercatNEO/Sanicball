{
  fetchFromGitHub,
  dotnetCorePackages,
  ...
}:
dotnetCorePackages.buildDotnetModule rec {
  pname = "godot-dotnet";
  version = "4.4";
  rev = "55bdf39f9f0d62ab9ab3c49931926dc6e2cd5905";

  src = fetchFromGitHub {
    owner = "raulsntos";
    repo = "godot-dotnet";
    inherit rev;
    hash = "sha256-tBCh/+w1YBF0Lotu988No+QkezWiKZ4aybN81zAYLlQ=";
  };

  projectFile = "./Godot.sln";

  dotnet-sdk = dotnetCorePackages.sdk_9_0;
  dotnet-runtime = dotnetCorePackages.runtime_9_0;
  nugetDeps = ./deps.nix;

  buildPhase = ''
    ./eng/common/cibuild.sh --warnaserror false /p:GenerateGodotBindings=true
  '';
}
