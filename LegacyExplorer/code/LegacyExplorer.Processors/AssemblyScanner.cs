using LegacyExplorer.Processors;
using LegacyExplorer.Processors.Models;
using LegacyExplorer.Processors.Validators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using NLog;

namespace LegacyExplorer.Processors
{
    public partial class AssemblyScanner : IScanner<ScannerInput, ScannerOutput>
    {
        //private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        

        public ScannerOutput Scan(ScannerInput input)
        {

            string methodName = "Scan(ScannerInput input)";

            logger.Info($"Class:{className},method:{methodName} Starts");


            ScannerInputValidator inputValidator = new ScannerInputValidator();
            var validator = inputValidator.Validate(input);

            //if (validator.IsValid)
            //    throw new Exception(string.Join("\n", validator.Errors));

            ScannerOutput output = new ScannerOutput();
            try
            {


                foreach (var assemblyFile in input.AssemblyPaths)
                {
                    try
                    {
                        if (File.Exists(assemblyFile))
                        {
                            Console.WriteLine($"{assemblyFile} file path found and scanning started...");
                            // Load the assembly from the DLL file.
                            Assembly assembly = Assembly.LoadFrom(assemblyFile);
                            ScanAssemebly(assembly, output);
                        }
                        else
                        {
                            Console.WriteLine($"{assemblyFile} file path not found");
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, $"Class:{className}, Method:{methodName},Error Message:{ex.Message}");
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Class:{className}, Method:{methodName},Error Message:{ex.Message}");
            }
            logger.Info($"Class:{className},method:{methodName} Ends");

            return output;
        }

    }
}
