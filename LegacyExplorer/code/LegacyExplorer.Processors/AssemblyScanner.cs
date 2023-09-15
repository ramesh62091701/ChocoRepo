using LegacyExplorer.Processors;
using LegacyExplorer.Processors.Models;
using LegacyExplorer.Processors.Validators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace LegacyExplorer.Processors
{
    public partial class AssemblyScanner : IScanner<ScannerInput, ScannerOutput>
    {
        public ScannerOutput Scan(ScannerInput input)
        {

            ScannerInputValidator inputValidator = new ScannerInputValidator();
            var validator = inputValidator.Validate(input);

            //if (validator.IsValid)
            //    throw new Exception(string.Join("\n", validator.Errors));

            ScannerOutput output = new ScannerOutput();

            foreach (var assemblyFile in input.AssemblyPaths)
            {
                try
                {
                    // Load the assembly from the DLL file.
                    Assembly assembly = Assembly.LoadFrom(assemblyFile);
                    ScanAssemebly(assembly, output);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Exception while scanning", ex);
                }
            }

            return output;
        }

    }
}
