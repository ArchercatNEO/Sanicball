{
  callPackage,
  runCommand,
  fetchFromGitHub,
  dotnetCorePackages,
  jq,
  moreutils,
  ...
}:
let
  pname = "godot-dotnet";
  version = "4.4";
  commit = "eff114039efe1c4b4de2f69c337e7d14954927ef";
  dotnet-version = dotnetCorePackages.sdk_8_0.version;

  args = {
    inherit pname version commit;

    src = fetchFromGitHub {
      owner = "raulsntos";
      repo = "godot-dotnet";
      rev = commit;
      hash = "sha256-X5Lkno+wjSPfKsdBkUyv7Hu4IFLrANtf55+sN++AMk4=";
    };

    nativeBuildInputs = [
      jq
      moreutils
    ];

    preConfigure = "jq '.sdk.version = \"${dotnet-version}\" | .tools.dotnet = \"${dotnet-version}\"' global.json | sponge global.json";

    dotnetPackFlags = [
      "/p:RuntimeIdentifier=linux-x64"
      "/p:RepositoryUrl=https://github.com/raulsntos/godot-dotnet"
      "/p:RepositoryType=github"
      "/p:RepositoryCommit=${commit}"
    ];
  };

  bindings = callPackage ./bindings.nix args;
  generator = callPackage ./generator.nix args;

in
runCommand (pname + "-" + version) { } ''
  mkdir $out
  ln -s ${bindings}/share/nuget/source/ $out/godot.bindings
  ln -s ${generator}/share/nuget/source/ $out/godot.sourcegenerators
''
