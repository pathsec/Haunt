using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haunt
{
    public class Util
    {
        public static void ExecuteCommand(string command)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo("cmd.exe")
                {
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = new Process())
                {
                    process.StartInfo = psi;
                    process.Start();

                    // Execute the command
                    process.StandardInput.WriteLine(command);
                    process.StandardInput.WriteLine("exit");
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing command: {ex.Message}");
            }
        }

        public static void PrintHelp()
        {
            Console.WriteLine("Haunt Persistence Toolkit by Jeremiah Turnage");
            Console.WriteLine("Inspired by SharPersist C# Windows Persistence Toolkit by Brett Hawkins (@h4wkst3r)");
            Console.WriteLine("Thanks to ChatGPT for doing the manual labor and reimplementing stuff like registry manipulation for me.");
            
            Console.WriteLine("\nThis toolkit was designed to be walked through via commandline, no need to learn flags for this program!");
        }
    }
}
