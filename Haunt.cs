using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haunt
{
    public class Haunt
    {
        public static void Main(string[] args)
        {
            string version = "Haunt 1.0";

            if (args.Length > 0) 
            {
                if (args[0] == "-help" || args[0] == "/help") { Util.PrintHelp(); }
            } 
            else
            {
                Console.WriteLine("Initiating the spooky factors! (" + version + ")");
                Console.WriteLine("Here is a list of persistence methods to choose from:\n");

                Console.WriteLine("1) Startup Folder [Local or Admin]");
                Console.WriteLine("2) Registry Techniques [Local or Admin]");
                Console.WriteLine("3) Scheduled Task [Admin]");
                Console.WriteLine("4) New Service [Admin]");

                Console.Write("\nPlease select one of the options. ");
                string persistenceMethodChosen = Console.ReadLine();

                if (int.TryParse(persistenceMethodChosen, out int selection))
                {
                    switch (selection)
                    {
                        //Startup Folder
                        case 1:
                            Method.StartupFolder startupFolderMethod = new Method.StartupFolder();
                            break;
                        //Registry Entry
                        case 2:
                            Console.WriteLine("You have selected the Registry Techniques method.");
                            Console.WriteLine("There are a few options to consider:\n");
                            Console.WriteLine("1) Run Key. [Local]");
                            Console.WriteLine("2) Run key. [Admin]");

                            Console.Write("\n");
                            string regMethod = Console.ReadLine();

                            Method.RegistryEntry registryEntryMethod = new Method.RegistryEntry(Convert.ToInt16(regMethod));
                            break;

                        case 3:
                            Console.WriteLine("You have selected the Scheduled Task method.");
                            Method.ScheduledTask scheduledTaskMethod = new Method.ScheduledTask();
                            break;
                        case 4:
                            Console.WriteLine("You have selected the New Service method.");
                            Method.NewService newServiceMethod = new Method.NewService();
                            break;

                        default:
                            Console.WriteLine("Invalid input, please try again with a selection indexed on the list.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input, please try again with a number selection.");
                }
            }
        }
    }
}
