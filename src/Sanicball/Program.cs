using System.Runtime.InteropServices;
using Godot;
using Godot.Bridge;
using Serilog;

[assembly: DisableGodotEntryPointGeneration]

namespace Sanicball;

public static class Program
{
    [UnmanagedCallersOnly(EntryPoint = "init")]
    internal static bool Init(nint getProcAddress, nint library, nint initialization)
    {
        using var logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Godot()
            .CreateLogger();
        Log.Logger = logger;

        GodotBridge.Initialize(
            getProcAddress,
            library,
            initialization,
            config =>
            {
                config.SetMinimumLibraryInitializationLevel(InitializationLevel.Scene);
                config.RegisterInitializer(ClassDBExtensions.InitializeUserTypes);
                config.RegisterTerminator(ClassDBExtensions.DeinitializeUserTypes);
            }
        );
        return true;
    }
}
