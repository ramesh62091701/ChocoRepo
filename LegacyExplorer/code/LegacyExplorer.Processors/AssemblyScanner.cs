﻿using LegacyExplorer.Processors.Models;
using LegacyExplorer.Processors.Validators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace LegacyExplorer.Processors
{
    public class AssemblyScanner : IScanner<ScannerInput, ScannerOutput>
    {
        public ScannerOutput Scan(ScannerInput input)
        {

            ScannerInputValidator inputValidator = new ScannerInputValidator();
            var validator = inputValidator.Validate(input);

            //if (validator.IsValid)
            //    throw new Exception(string.Join("\n", validator.Errors));


            ScannerOutput output = new ScannerOutput();

            try
            {
                // Load the assembly from the DLL file.
                Assembly assembly = Assembly.LoadFrom(input.AssemblyPath);

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
                    foreach (MethodInfo method in typeClass.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))
                    {
                        NetMethod netMethod = GetMethodsInfo(method);
                        netMethod.TypeId = netType.Id;
                        output.Methods.Add(netMethod);
                    }

                    output.Types.Add(netType);

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception while scanning", ex);
            }

            return output;
        }

        public IEnumerable<Type> GetAssemblyTypes(Assembly assembly)
        {
            //IEnumerable<Type> allTypes = assembly.GetTypes().Where(type => type.IsClass).ToList();

            //var allTypes = assembly.GetTypes()
            //                           .Where(type => type.IsClass 
            //                               && !type.IsNested 
            //                               && !type.IsGenericType 
            //                               && !type.IsDefined(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), false))
            //                           .ToList();

            var allTypes = assembly.GetTypes()
                                      .Where(type => type.IsClass
                                          && !type.IsDefined(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), false))
                                      .ToList();

            return allTypes;
        }

        public NetAssembly GetAssemblyInfo(Assembly assembly)
        {
            NetAssembly netAssembly = new NetAssembly();
            netAssembly.Name = assembly.FullName;
            netAssembly.FileName = (assembly.Location.Split('\\')[assembly.Location.Split('\\').Length - 1]).Split(',')[0];
            netAssembly.Location = assembly.Location;

            return netAssembly;
        }

        public NetReference GetAssemblyReferenceInfo(AssemblyName assemblyName)
        {
            NetReference netRef = new NetReference();
            netRef.Name = assemblyName.Name;

            return netRef;
        }

        public NetType GetTypeInfo(Type typeClass)
        {

            NetType netType = new NetType();
            netType.Name = typeClass.Name;
            netType.FullName = typeClass.FullName;
            netType.Namespace = typeClass.Namespace;
            netType.TypeOfType = typeClass.BaseType.FullName;
            return netType;

        }

        public NetBaseClass GetBaseTypeInfo(Type typeClass)
        {

            NetBaseClass netBaseClass = new NetBaseClass();
            netBaseClass.Name = typeClass.Name;
            netBaseClass.FullName = typeClass.FullName;
            netBaseClass.Namespace = typeClass.Namespace;
            netBaseClass.TypeOfType = typeClass.GetType().Name;

            return netBaseClass;

        }

        public List<Type> GetBaseClasses(Type typeClass)
        {

            List<Type> baseClasses = new List<Type>();

            Type currentType = typeClass.BaseType;
            while (currentType != null)
            {
                baseClasses.Add(currentType);
                currentType = currentType.BaseType;
            }

            return baseClasses;
        }

        public NetField GetFieldInfo(FieldInfo field)
        {

            NetField netField = new NetField();

            netField.Name = field.Name;
            netField.FieldType = field.GetType().Name;

            return netField;

        }

        public NetProperty GetPropertyInfo(PropertyInfo property)
        {

            NetProperty netProperty = new NetProperty();

            netProperty.Name = property.Name;
            netProperty.PropertyType = property.PropertyType.Name;

            return netProperty;

        }

        public NetMethod GetMethodsInfo(MethodInfo method)
        {

            NetMethod netMethod = new NetMethod();
            netMethod.Name = method.Name;

            return netMethod;

        }

    }

}
