{
  fetchFromGitHub,
  autoPatchelfHook,
  buildDotnetModule,
  dotnetCorePackages,
  bash,
  python3,
  moreutils,
  jq,
  clang,
  openssl,
  icu,
  krb5,
  ...
}:  let 
  dotnet-version = dotnetCorePackages.sdk_8_0.version;
in 
buildDotnetModule rec {
  pname = "godot-dotnet";
  version = "4.4";
  commit = "eff114039efe1c4b4de2f69c337e7d14954927ef";

  src = fetchFromGitHub {
    owner = "raulsntos";
    repo = "godot-dotnet";
    rev = "master";
    hash = "sha256-X5Lkno+wjSPfKsdBkUyv7Hu4IFLrANtf55+sN++AMk4=";
  };

  dotnet-sdk = dotnetCorePackages.sdk_8_0;
  dotnet-runtime = dotnetCorePackages.runtime_8_0;
  projectFile = [
    "./Godot.sln"
    "./eng/common/tasks/Tasks.csproj"
  ];
  nugetDeps = ./deps.nix;
  packNupkg = true;

  nativeBuildInputs = [
    autoPatchelfHook
    python3
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

  preConfigure = ''
    jq '.sdk.version = "${dotnet-version}" | .tools.dotnet = "${dotnet-version}"' global.json | sponge global.json
  '';

  buildPhase = ''
    ${bash}/bin/bash ./eng/common/build.sh --productBuild --warnaserror false \
      /p:GenerateGodotBindings=true \
      /p:RepositoryUrl=https://github.com/raulsntos/godot-dotnet \
      /p:RepositoryType=github \
      /p:RepositoryCommit=${commit}
  '';

  installPhase = ''
    mkdir -p $out/share
    ${bash}/bin/bash ./eng/common/build.sh --productBuild --warnaserror false --pushNupkgsLocal $out/share \
      /p:RepositoryUrl=https://github.com/raulsntos/godot-dotnet \
      /p:RepositoryType=github \
      /p:RepositoryCommit=${commit}
  '';
}
