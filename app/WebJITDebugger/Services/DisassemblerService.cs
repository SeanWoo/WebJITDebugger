using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using WebJITDebugger.Helpers;

namespace WebJITDebugger.Services;

public class DisassemblerService
{
    private const string DEFAULT_CORERUN_PATH = @"/app/runtime/artifacts/bin/coreclr/Linux.x64.Checked/corerun";
    private const string DEFAULT_CORE_LIBRARES = @"/usr/share/dotnet/shared/Microsoft.NETCore.App/7.0.4";
    private const string DEFAULT_RESOURCES_PATH = @"/app/Resources";

    private readonly ProcessService _processService;
    public DisassemblerService(ProcessService processService)
    {
        _processService = processService;
    }
    /// <summary>
    /// D:\jit\runtime\artifacts\bin\coreclr\windows.x64.Checked\CoreRun.exe "TestJIT.dll" "TestJIT.Program" "Test" True
    /// 
    /// 
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    /// <exception cref="DisassemblerException"></exception>
    public async Task<string> DisassemblyCode(string code)
    {
        await WriteCodeToFiles(code);

        var buildOutput = await BuildProject();

        if (!buildOutput.Contains("Build succeeded."))
            throw new DisassemblerException("Build failed: \n\n" + buildOutput);

        var assemblyCode = await DisassemblyMethod();

        return assemblyCode;
    }

    private async Task WriteCodeToFiles(string code)
    {
        string filePath = Path.Combine(DEFAULT_RESOURCES_PATH, "Program.cs");
        await File.WriteAllTextAsync(filePath, code);
    }

    private async Task<string> BuildProject()
    {
        var envVars = new Dictionary<string, string>();
        envVars["DOTNET_SKIP_FIRST_TIME_EXPERIENCE"] = "1";
        envVars["DOTNET_CLI_TELEMETRY_OPTOUT"] = "1";

        return await _processService.RunProcess(
            path: "dotnet",
            args: "build -f net7.0 -c Release -o bin\\WebDisasmo --no-self-contained /p:RuntimeIdentifier=\"\" /p:RuntimeIdentifiers=\"\" /p:WarningLevel=0 /p:TreatWarningsAsErrors=false",
            envVars: envVars,
            workingDirectory: DEFAULT_RESOURCES_PATH);
    }

    private async Task<string> DisassemblyMethod()
    {
        var envVars = new Dictionary<string, string>();
        envVars["CORE_LIBRARIES"] = DEFAULT_CORE_LIBRARES;
        envVars["DOTNET_JitDisasm"] = "*Program:Main";
        envVars["DOTNET_TieredPG"] = "0";
        envVars["DOTNET_TieredCompilation"] = "0";
        envVars["DOTNET_JitDiffableDasm"] = "0";
        envVars["DOTNET_ReadyToRun"] = "1";
        envVars["DOTNET_TieredPGO_InstrumentOnlyHotCode"] = "0";

        var workingDirectory = Path.Combine(DEFAULT_RESOURCES_PATH, "bin", "WebDisasmo");

        var runtimeFolderPath = Environment.GetEnvironmentVariable("RUNTIME_PATH");
        if (runtimeFolderPath is null)
            return string.Empty;
        
        var exePath = Path.Combine(runtimeFolderPath, @"\artifacts\bin\coreclr\windows.x64.Checked\CoreRun.exe");

        var result = await _processService.RunProcess(
            path: DEFAULT_CORERUN_PATH,
            args: "DisasmoLoader4.dll LaunchSite.dll Program Main True",
            envVars: envVars,
            workingDirectory: workingDirectory);

        var blocks = result.Split("; ============================================================")
            .Where(x => !x.Contains("DisasmoLoader"));

        var lines = string.Join('\n', blocks)
            .Split("\n")
            .Where(x => !x.StartsWith(';') && !string.IsNullOrWhiteSpace(x));


        return string.Join('\n', lines);
    }
}