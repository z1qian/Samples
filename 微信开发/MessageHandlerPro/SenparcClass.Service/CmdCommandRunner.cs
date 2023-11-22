using System.Diagnostics;

namespace SenparcClass.Service;
public class CmdCommandRunner
{
    public string RunCmdCommand(string command)
    {
        string output = "";

        Process process = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        process.StartInfo = startInfo;
        process.Start();
        process.StandardInput.WriteLine(command);
        process.StandardInput.Flush();
        process.StandardInput.Close();
        output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        return output;
    }
}