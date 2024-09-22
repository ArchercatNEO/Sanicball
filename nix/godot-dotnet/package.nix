{
  fetchFromGitHub,
  autoPatchelfHook,
  buildDotnetModule,
  dotnetCorePackages,
  moreutils,
  jq,
  clang,
  openssl,
  icu,
  krb5,
  ...
}:
buildDotnetModule {
  pname = "godot-dotnet";
  version = "4.3";

  src = fetchFromGitHub {
    owner = "raulsntos";
    repo = "godot-dotnet";
    rev = "master";
    hash = "sha256-mi9fj7tUvSFu64pQqUlEvJJZz/3lXe7bFdCmxbpIPH0=";
  };

  dotnet-sdk = dotnetCorePackages.sdk_8_0;
  dotnet-runtime = dotnetCorePackages.runtime_8_0;
  projectFile = [
    "./src/Godot.Bindings/Godot.Bindings.csproj"
    "./src/Godot.SourceGenerators/Godot.SourceGenerators.csproj"
  ];
  nugetDeps = ./deps.nix;
  packNupkg = true;

  nativeBuildInputs = [
    autoPatchelfHook
    moreutils
    jq
    clang
  ];

  buildInputs = [
    openssl
    icu
    krb5
    krb5.dev
  ];

  dotnetBuildFlags = [
    "/p:GenerateGodotBindings=true"
  ];

  dotnetInstallFlags = [
    "/p:RepositoryUrl=https://github.com/raulsntos/godot-dotnet"
    "/p:RepositoryCommit=AAAAAAAAAA"
    "/p:RepositoryType=github"
  ];

  dotnetPackFlags = [
    "/p:RepositoryUrl=https://github.com/raulsntos/godot-dotnet"
    "/p:RepositoryCommit=AAAAAAAAAA"
    "/p:RepositoryType=github"
    "/p:RuntimeIdentifier=linux-x64"
  ];

  preConfigure = ''
    jq '.sdk.version = "8.0.303" | .tools.dotnet = "8.0.303"' global.json | sponge global.json
  '';
}
