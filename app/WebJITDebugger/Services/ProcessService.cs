using System.Diagnostics;
using System.Text;
using WebJITDebugger.Helpers;

namespace WebJITDebugger.Services
{
    public class ProcessService
    {
        public async Task<string> RunProcess(string path, string args = "", Dictionary<string, string> envVars = null, string workingDirectory = "")
        {
            var processStartInfo = new ProcessStartInfo()
            {
                FileName = path,
                Arguments = args,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
            };

            if (workingDirectory != null)
                processStartInfo.WorkingDirectory = workingDirectory;

            if (envVars != null)
            {
                foreach (var envVar in envVars)
                    processStartInfo.EnvironmentVariables[envVar.Key] = envVar.Value;
            }

            var process = new Process();
            process.StartInfo = processStartInfo;
            var result = process.Start();

            if (!result)
                throw new DisassemblerException("Process return false");

            var stringBuilder = new StringBuilder();
            process.OutputDataReceived += (sender, e) =>
            {
                stringBuilder.AppendLine(e.Data);
            };
            process.ErrorDataReceived += (sender, e) =>
            {
                stringBuilder.AppendLine(e.Data);
            };
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await WaitExitProcessAsync(process);

            return stringBuilder.ToString();
        }

        public Task WaitExitProcessAsync(Process process)
        {
            if (process.HasExited)
                return Task.CompletedTask;

            var tcs = new TaskCompletionSource<object>();
            process.EnableRaisingEvents = true;
            process.Exited += (sender, args) => tcs.TrySetResult(null);

            return process.HasExited ? Task.CompletedTask : tcs.Task;
        }
    }
}
