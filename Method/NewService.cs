using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Haunt.Method
{
    public class NewService
    {
        // Service Control Manager (SCM) access rights
        private const int SC_MANAGER_CREATE_SERVICE = 0x0002;
        private const int SC_MANAGER_ALL_ACCESS = 0xF003F;

        // Service access rights
        private const int SERVICE_WIN32_OWN_PROCESS = 0x00000010;
        private const int SERVICE_AUTO_START = 0x00000002;
        private const int SERVICE_ERROR_NORMAL = 0x00000001;
        private const int SERVICE_ALL_ACCESS = 0xF01FF;

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern IntPtr OpenSCManager(string machineName, string databaseName, int desiredAccess);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern IntPtr CreateService(IntPtr hSCManager, string lpServiceName, string lpDisplayName,
            int dwDesiredAccess, int dwServiceType, int dwStartType, int dwErrorControl, string lpBinaryPathName,
            string lpLoadOrderGroup, IntPtr lpdwTagId, string lpDependencies, string lpServiceStartName, string lpPassword);

        [DllImport("advapi32.dll")]
        private static extern int CloseServiceHandle(IntPtr hSCObject);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int StartService(IntPtr hService, int dwNumServiceArgs, string[] lpServiceArgVectors);


        public NewService()
        {
            Console.Write("Please input a Service Name: ");
            string serviceName = Console.ReadLine();

            Console.Write("Please input a Display Name for your service: ");
            string displayName = Console.ReadLine();

            Console.Write("Please input a path to the binary of your service: ");
            string binaryPath = Console.ReadLine();

            CreateNewService(serviceName, displayName, binaryPath);
        }

        private void CreateNewService(string serviceName, string displayName, string binaryPath)
        {
            // Open a handle to the SCM
            IntPtr scmHandle = OpenSCManager(null, null, SC_MANAGER_CREATE_SERVICE | SC_MANAGER_ALL_ACCESS);
            if (scmHandle == IntPtr.Zero)
            {
                Console.WriteLine("Failed to open SCM.");
                return;
            }

            try
            {
                // Create the service
                IntPtr serviceHandle = CreateService(
                    scmHandle,
                    serviceName,
                    displayName,
                    SERVICE_ALL_ACCESS,
                    SERVICE_WIN32_OWN_PROCESS,
                    SERVICE_AUTO_START,
                    SERVICE_ERROR_NORMAL,
                    binaryPath,
                    null,
                    IntPtr.Zero,
                    null,
                    null,
                    null
                );

                if (serviceHandle == IntPtr.Zero)
                {
                    Console.WriteLine("Failed to create service.");
                }
                else
                {
                    if (StartService(serviceHandle, 0, null) == 0)
                    {
                        Console.WriteLine("Potential Error.");
                    }

                    CloseServiceHandle(serviceHandle);
                }
            }
            finally
            {
                CloseServiceHandle(scmHandle);
            }
        }

    }
}
