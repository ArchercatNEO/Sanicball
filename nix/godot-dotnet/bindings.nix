{
  pname,
  version,
  commit,
  src,
  nativeBuildInputs,
  preConfigure,
  dotnetPackFlags,
  buildDotnetModule,
  dotnetCorePackages,
  clang,
  icu,
  krb5,
  openssl,
  ...
}:
buildDotnetModule {
  pname = pname + "-bindings";
  inherit version commit src;

  projectFile = "./src/Godot.Bindings/Godot.Bindings.csproj";
  nugetDeps = ./deps.nix;

  dotnet-sdk = dotnetCorePackages.sdk_8_0;
  dotnet-runtime = dotnetCorePackages.runtime_8_0;

  nativeBuildInputs = nativeBuildInputs ++ [
    clang
  ];

  buildInputs = [
    icu
    krb5
    openssl
  ];

  inherit preConfigure;

  preBuild = ''
    mkdir -p ./src/Godot.Bindings/Generated
    dotnet run --project ./src/Godot.BindingsGenerator/Godot.BindingsGenerator.csproj -- \
      --extension-api ./gdextension/extension_api.json \
      --extension-interface ./gdextension/gdextension_interface.h \
      -o ./src/Godot.Bindings/Generated
  '';

  packNupkg = true;


  dontFixup = true;
}
