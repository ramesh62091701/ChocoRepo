using Microsoft.Win32;
using System;
using System.Collections.Generic;

class DotNetSystem
{
    static void Main(string[] args)
    {
        //Console.WriteLine(".NET Core Versions installed on this machine:");
        //Console.WriteLine("---------------------------------------------");
        // Check for .NET Core 7 or later

        if (IsDotNetCoreVersionInstalled("7.0"))
        {
            Console.WriteLine(".NET Core 7 is installed in your system ");
        }


        else if (IsDotNetCoreVersionInstalled("v6.0"))
        {
            Console.WriteLine(".NET Framework 6.0 or later is installed");
        }

        // Check for .NET Framework 4.8 or later
        else if (IsDotNetCoreVersionInstalled("v4.8"))
        {
            Console.WriteLine(".NET Framework 4.8 or later is installed");
        }


        // Check for .NET Framework 4.7.2 or later
        else if (IsDotNetCoreVersionInstalled("v4.7.2"))
        {
            Console.WriteLine(".NET Framework 4.7.2 or later is installed");
        }

        // Check for .NET Framework 4.6.2 or later
        else if (IsDotNetCoreVersionInstalled("v4.6.2"))
        {
            Console.WriteLine(".NET Framework 4.6.2 or later is installed");
        }

        // Check for .NET Framework 4.5.1 or later
        else if (IsDotNetCoreVersionInstalled("v4.5.1"))
        {
            Console.WriteLine(".NET Framework 4.5.1 or later is installed");
        }

        else if (IsDotNetCoreVersionInstalled("v2.1.200"))
        {

            Console.WriteLine(".NET Framework 2.1.200 or later is installed");

        }
        else
        {
            Console.WriteLine("No .Net Version Found");
        }

        Console.ReadKey();
    }
    static bool IsDotNetCoreVersionInstalled(string version)
    {
        using (RegistryKey ndpKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\dotnet\Setup\InstalledVersions\x64"))
        {
            if (ndpKey != null)
            {
                string[] versionKeys = ndpKey.GetSubKeyNames();
                foreach (string versionKey in versionKeys)
                {
                    using (RegistryKey versionSubKey = ndpKey.OpenSubKey(versionKey))
                    {
                        string installedVersion = (string)versionSubKey.GetValue("Version");
                        if (installedVersion.StartsWith(version + "."))
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
}