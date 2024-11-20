{
  lib,
  fetchFromGitHub,
  godot_4,
  brotli,
  embree,
  enet,
  freetype,
  glslang,
  graphite2,
  harfbuzz,
  harfbuzzFull,
  icu,
  libogg,
  libpng,
  libtheora,
  libvorbis,
  libwebp,
  mbedtls,
  miniupnpc,
  openxr-loader,
  pcre2,
  recastnavigation,
  wslay,
  zlib,
  zstd,
  ...
}:
let
  mkSconsFlagsFromAttrSet = lib.mapAttrsToList (
    k: v: if builtins.isString v then "${k}=${v}" else "${k}=${builtins.toJSON v}"
  );
in
godot_4.overrideAttrs
  (
    finalAttrs: prevAttrs: rec {
      version = "4.4-dev4";
      commitHash = "36e6207bb48fafd43b98a458d0c461ad982b4e68";

      src = fetchFromGitHub {
        owner = "godotengine";
        repo = "godot";
        rev = commitHash;
        hash = "sha256-AHzTiO+vItA0p42HtMgHg55V0vhOP+aIDTncPVFie70=";
      };

      sconsFlags =
        prevAttrs.sconsFlags
        ++ (mkSconsFlagsFromAttrSet {
          dev_build = true;
          dev_mode = true;

          #deprecated = true; gdextension breaks without this

          use_sowrap = true; # libXi breaks without this
          builtin_brotli = false;
          #all commented options simply disable the assosiated third party
          #builtin_certs = false;
          #builtin_clipper2 = false;
          builtin_embree = true; # no pkg-config
          builtin_enet = false;
          builtin_freetype = false;
          builtin_glslang = true; # no pkg-config
          builtin_graphite = false;
          builtin_harfbuzz = false;
          builtin_icu4c = false;
          builtin_libogg = false;
          builtin_libpng = false;
          builtin_libtheora = false;
          builtin_libvorbis = false;
          builtin_libwebp = false;
          #builtin_msdfgen = false;
          builtin_mbedtls = true; # no pkg-config
          builtin_miniupnpc = true; # no pkg-config
          builtin_openxr = false;
          builtin_pcre2 = false;
          builtin_pcre2_with_jit = false;
          builtin_recastnavigation = true; # no pkg-config
          #builtin_rvo2_2d = false;
          #builtin_rvo2_3d = false;
          #builtin_xatlas = false;
          builtin_wslay = false;
          builtin_zlib = false;
          builtin_zstd = false;
        }) ++ [
          "linkflags=-Wl,--build-id"
          #"linkflags=-L${embree}/lib"
          #"ccflags=-I${embree}/include"
          #"linkflags=-L${glslang}/lib"
          #"ccflags=-I${glslang.dev}/include"
          #"linkflags=-L${mbedtls}/lib"
          #"ccflags=-I${mbedtls}/include"
          #"linkflags=-L${miniupnpc}/lib"
          #"ccflags=-I${miniupnpc}/include"
          #"linkflags=-L${recastnavigation}/lib"
          #"ccflags=-I${recastnavigation}/include"
          #"ccflags=-I${xorg.libXfixes.dev}/include"
        ];

      buildInputs =
        (prevAttrs.buildInputs or [ ])
        ++ prevAttrs.runtimeDependencies
        ++ [
          brotli
          embree
          enet
          freetype
          glslang
          graphite2
          harfbuzz
          harfbuzzFull
          icu
          libogg
          libpng
          libtheora
          libvorbis
          libwebp
          mbedtls
          miniupnpc
          openxr-loader
          pcre2
          recastnavigation
          wslay
          zlib
          zstd
        ];
    }
  )
