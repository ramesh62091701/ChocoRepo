using LegacyExplorer.Processors.Models;
using LegacyExplorer.Processors.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace LegacyExplorer.Processors
{
    public class TypeScanner : IScanner<ScannerInput, ScannerOutput>
    {
        public ScannerOutput Scan(ScannerInput input)
        {

            ScannerInputValidator inputValidator = new ScannerInputValidator();
            var validator = inputValidator.Validate(input);

            if (validator.IsValid)
                throw new Exception(string.Join("\n", validator.Errors));


            ScannerOutput output = new ScannerOutput();

            try
            {
                // Load the assembly from the DLL file.
                Assembly assembly = Assembly.LoadFrom(input.AssemblyPath);

                //Get assembly info
                NetAssembly netAssembly = GetAssemblyInfo(assembly);

                output.Assemblies.Add(netAssembly);

                //Get assembly references
                netAssembly.References = GetAssemblyReferenceInfo(assembly);

                // Get all types
                IEnumerable<TypeInfo> allTypes = assembly.DefinedTypes.ToArray();

                foreach (Type typeClass in allTypes)
                {
                    // Get type info
                    NetType netType = GetTypeInfo(typeClass);

                    //Get fields
                    foreach (FieldInfo field in typeClass.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                    {
                        NetField netField = GetFieldInfo(field);
                        netType.Fields.Add(netField);

                    }

                    //Get fields
                    foreach (MethodInfo method in typeClass.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))
                    {
                        NetMethod netMethod = GetMethodsInfo(method);
                        netType.Methods.Add(netMethod);
                    }

                    netAssembly.Types.Add(netType);

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception while scanning", ex);
            }

            return output;
        }



        public NetAssembly GetAssemblyInfo(Assembly assembly)
        {
            NetAssembly netAssembly = new NetAssembly();
            netAssembly.Name = assembly.GetName().Name;
            netAssembly.FileName = (assembly.Location.Split('\\')[assembly.Location.Split('\\').Length - 1]).Split(',')[0];
            netAssembly.Location = assembly.Location;

            return netAssembly;
        }

        public List<NetReference> GetAssemblyReferenceInfo(Assembly assembly)
        {
            List<NetReference> references = new List<NetReference>();

            var referenceAssemblies = assembly.GetReferencedAssemblies();
            foreach (var reference in referenceAssemblies)
            {
                NetReference netRef = new NetReference();
                netRef.Name = reference.Name;
                references.Add(netRef);
            }

            return references;
        }

        public NetType GetTypeInfo(Type typeClass)
        {

            NetType netType = new NetType();
            netType.Name = typeClass.Name;
            netType.Namespage = typeClass.Namespace;
            netType.TypeOfType = typeClass.GetType().Name;

            return netType;

        }
        public NetField GetFieldInfo(FieldInfo field)
        {

            NetField netField = new NetField();

            netField.Name = field.Name;
            netField.FieldType = field.GetType().Name;

            return netField;

        }
        public NetMethod GetMethodsInfo(MethodInfo method)
        {

            NetMethod netMethod = new NetMethod();
            netMethod.Name = method.Name;

            return netMethod;

        }

    }

}
