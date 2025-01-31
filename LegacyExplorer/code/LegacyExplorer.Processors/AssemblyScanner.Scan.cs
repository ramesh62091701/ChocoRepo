﻿using LegacyExplorer.Processors;
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

        public void ScanAssemebly(Assembly assembly, ScannerOutput output)
        {
            string methodName = "GetAssemblyInfo(Assembly)";

            logger.Info($"Class:{className},method:{methodName} Starts");
            try
            {

                //Get assembly info
                NetAssembly netAssembly = GetAssemblyInfo(assembly);
                Console.WriteLine($"{netAssembly.Id}");

                output.Assemblies.Add(netAssembly);

                //Get assembly references
                var referenceAssemblies = assembly.GetReferencedAssemblies();
                foreach (var reference in referenceAssemblies)
                {
                    NetReference netRef = GetAssemblyReferenceInfo(reference);
                    netRef.AssemblyId = netAssembly.Id;
                    output.References.Add(netRef);
                }

                // Get all types
                IEnumerable<Type> allTypes = GetAssemblyTypes(assembly);

                foreach (Type typeClass in allTypes)
                {
                    // Get type info
                    NetType netType = GetTypeInfo(typeClass);
                    netType.AssemblyId = netAssembly.Id;

                    var baseClasses = GetBaseClasses(typeClass);
                    foreach (Type baseClass in baseClasses)
                    {
                        NetBaseClass netBaseClass = GetBaseTypeInfo(baseClass);
                        netBaseClass.TypeId = netType.Id;
                        netBaseClass.TypeName = netType.Name;
                        if(netBaseClass.Name != "Object")
                            output.BaseClasses.Add(netBaseClass);
                    }


                    ////Get fields  --commenting due to bring deferent details of field instead of exact name, type of field. 
                    ///added Get properties block below
                    foreach (FieldInfo field in typeClass.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                    {
                        NetField netField = GetFieldInfo(field);
                        netField.TypeId = netType.Id;
                        output.Fields.Add(netField);

                    }

                    //Get properties
                    foreach (PropertyInfo property in typeClass.GetProperties())
                    {
                        NetProperty netProperty = GetPropertyInfo(property);
                        netProperty.TypeId = netType.Id;
                        output.Properties.Add(netProperty);

                    }
                    //Get fields
                    foreach (MethodInfo method in typeClass.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance))
                    {
                        NetMethod netMethod = GetMethodsInfo(method);
                        netMethod.TypeId = netType.Id;
                        netMethod.TypeName = netType.FullName;
                        if (!string.IsNullOrEmpty(netMethod.LineCount))
                            if (int.Parse(netMethod.LineCount) > 0)
                                output.Methods.Add(netMethod);
                    }

                    output.Types.Add(netType);

                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Class:{className}, Method:{methodName},Error Message:{ex.Message}");
            }
            logger.Info($"Class:{className},method:{methodName} Ends");
        }
              
    }

}
