using Microsoft.Win32;
using Sonata.Assets.DotnetScanner.System.Entities;
using Sonata.Assets.Scanner.Core;

namespace Sonata.Assets.DotnetScanner.System
{
    public class SystemScanner : IScanner<SystemScannerInput, SystemScannerOutput>
    {
        SystemScannerOutput output = new SystemScannerOutput()
        {
            Result = new List<string>()
        };

        public SystemScannerOutput Scan(SystemScannerInput input)
        {
            output.SystemName = input.SystemName;

            try
            {
                //Show all the installed versions
                Get1To45VersionFromRegistry();
                Get45PlusFromRegistry();
                Get5PlusFromRegistry();
                return output;
            }
            catch (Exception ex)
            {
                output.Error = ex;
                return output;
            }
        }

        private void Get1To45VersionFromRegistry()
        {
            // Opens the registry key for the .NET Framework entry.
            using (RegistryKey ndpKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
            {
                if (ndpKey != null)
                {
                    foreach (string versionKeyName in ndpKey.GetSubKeyNames())
                    {
                        // Skip .NET Framework 4.5 version information.
                        if (versionKeyName == "v4")
                        {
                            continue;
                        }

                        if (versionKeyName.StartsWith("v"))
                        {

                            RegistryKey versionKey = ndpKey.OpenSubKey(versionKeyName);
                            // Get the .NET Framework version value.
                            string name = (string)versionKey.GetValue("Version", "");
                            // Get the service pack (SP) number.
                            string sp = versionKey.GetValue("SP", "").ToString();

                            // Get the installation flag, or an empty string if there is none.
                            string install = versionKey.GetValue("Install", "").ToString();
                            if (string.IsNullOrEmpty(install)) // No install info; it must be in a child subkey.
                                WriteVersion(name);
                            else
                            {
                                if (!(string.IsNullOrEmpty(sp)) && install == "1")
                                {
                                    WriteVersion(name, sp);
                                }
                            }
                            if (!string.IsNullOrEmpty(name))
                            {
                                continue;
                            }
                            foreach (string subKeyName in versionKey.GetSubKeyNames())
                            {
                                RegistryKey subKey = versionKey.OpenSubKey(subKeyName);
                                name = (string)subKey.GetValue("Version", "");
                                if (!string.IsNullOrEmpty(name))
                                    sp = subKey.GetValue("SP", "").ToString();

                                install = subKey.GetValue("Install", "").ToString();
                                if (string.IsNullOrEmpty(install)) //No install info; it must be later.
                                    WriteVersion(name);
                                else
                                {
                                    if (!(string.IsNullOrEmpty(sp)) && install == "1")
                                    {
                                        WriteVersion(name, sp);
                                    }
                                    else if (install == "1")
                                    {
                                        WriteVersion("V" + name);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private void Get45PlusFromRegistry()
        {
            const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

            using (RegistryKey ndpKey = Registry.LocalMachine.OpenSubKey(subkey))
            {
                if (ndpKey == null)
                    return;
                //First check if there's an specific version indicated
                if (ndpKey.GetValue("Version") != null)
                {
                    WriteVersion("V" + ndpKey.GetValue("Version").ToString());
                }
                else
                {
                    if (ndpKey != null && ndpKey.GetValue("Release") != null)
                    {
                        WriteVersion(
                            "V" + CheckFor45PlusVersion((int)ndpKey.GetValue("Release"))
                        );
                    }
                }
            }

            // Checking the version using >= enables forward compatibility.
            string CheckFor45PlusVersion(int releaseKey)
            {
                if (releaseKey >= 533325)
                    return "4.8.1";
                if (releaseKey >= 528040)
                    return "4.8";
                if (releaseKey >= 461808)
                    return "4.7.2";
                if (releaseKey >= 461308)
                    return "4.7.1";
                if (releaseKey >= 460798)
                    return "4.7";
                if (releaseKey >= 394802)
                    return "4.6.2";
                if (releaseKey >= 394254)
                    return "4.6.1";
                if (releaseKey >= 393295)
                    return "4.6";
                if (releaseKey >= 379893)
                    return "4.5.2";
                if (releaseKey >= 378675)
                    return "4.5.1";
                if (releaseKey >= 378389)
                    return "4.5";
                // This code should never execute. A non-null release key should mean
                // that 4.5 or later is installed.
                return "";
            }
        }
        private void Get5PlusFromRegistry()
        {
            const string subkey = @"SOFTWARE\dotnet\Setup\InstalledVersions";
            var baseKey = Registry.LocalMachine.OpenSubKey(subkey);
            if (baseKey.SubKeyCount == 0)
                return;

            foreach (var platformKey in baseKey.GetSubKeyNames())
            {
                using (var platform = baseKey.OpenSubKey(platformKey))
                {
                    Console.WriteLine($"Platform: {platform.Name.Substring(platform.Name.LastIndexOf("\\") + 1)}");
                    if (platform.SubKeyCount == 0)
                        continue;

                    var sharedHost = platform.OpenSubKey("sharedhost");
                    foreach (var version in sharedHost.GetValueNames())
                        //Console.WriteLine("{0,-8}: {1}", version, sharedHost.GetValue(version));
                        WriteVersion("V" + sharedHost.GetValue("Version").ToString());
                }
            }
        }

        //Writes the version
        private void WriteVersion(string version, string spLevel = "")
        {
            version = version.Trim();
            if (string.IsNullOrEmpty(version))
                return;

            string spLevelString = "";
            if (!string.IsNullOrEmpty(spLevel))
                spLevelString = " Service Pack " + spLevel;

            output.Result.Add($"{version}{spLevelString}");
        }
    }
}
