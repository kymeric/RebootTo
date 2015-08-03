using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace RebootTo
{
    public class BcdService
    {
        public async Task<List<BootEntry>> GetBootEntriesAsync()
        {
            var output = await ExecuteBcdEditAsync();
            var lines = output.Split('\n').Select(l => l.Trim()).ToList();
            var separatorIndexes = lines.Select((l, i) => l == String.Empty ? i : -1).Where(i => i > 0).OrderBy(i => i);

            var entries = new List<BootEntry>();
            var firstLine = 1;
            foreach (var separatorIndex in separatorIndexes)
            {
                var entryLines = lines.Skip(firstLine).Take(separatorIndex - firstLine).ToArray();
                if (entryLines.Length < 2)
                    throw new InvalidOperationException($"Error parsing entry from {firstLine} to {separatorIndex}: " + String.Join("\r\n", entryLines));
                if (entryLines[1].IndexOf('-') == -1)
                    throw new InvalidOperationException($"Missing separator line from entry at {firstLine} to {separatorIndex}: " + String.Join("\r\n", entryLines));

                var entry = new BootEntry(entryLines);
                if (entry.Description != "Windows Boot Manager")
                    entries.Add(entry);
                firstLine = separatorIndex + 1;
            }
            return entries;
        }

        public async Task RebootAsync(BootEntry entry)
        {
            var output = await ExecuteBcdEditAsync("/bootsequence", entry.Identifier);
            if (output.Trim() != "The operation completed successfully.")
                throw new InvalidOperationException("Unable to set boot entry: " + output.Trim());
            output = await ExecuteAsync("shutdown.exe", "-r", "-t", "0");
            Application.Current.Shutdown();
        }

        private Task<string> ExecuteBcdEditAsync(params string[] parameters)
        {
            return ExecuteAsync(@"bcdedit.exe", parameters);
        }

        private Task<string> ExecuteAsync(string program, params string[] parameters)
        {
            var args = String.Join(" ", parameters);
            var startInfo = new ProcessStartInfo(program, args)
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            var tcs = new TaskCompletionSource<string>();
            var process = Process.Start(startInfo);
            process.EnableRaisingEvents = true;
            process.Exited += (sender, eventArgs) =>
            {
                var output = process.StandardOutput.ReadToEnd();
                if (process.ExitCode != 0)
                {
                    var error = output + "\r\nERROR\r\n" + process.StandardError.ReadToEnd();
                    tcs.SetException(new InvalidOperationException($"Error executing: {program} {args}:\r\n{error}"));
                }
                else
                {
                    tcs.SetResult(output);
                }
            };
            return tcs.Task;
        }
    }
}
