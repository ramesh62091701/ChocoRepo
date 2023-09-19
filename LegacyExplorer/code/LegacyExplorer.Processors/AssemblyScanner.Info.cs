using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler;
using LegacyExplorer.Processors;
using LegacyExplorer.Processors.Models;
using LegacyExplorer.Processors.Validators;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using LegacyExplorer.Processors.Interfaces;

namespace LegacyExplorer.Processors
{
    public partial class AssemblyScanner : IScanner<ScannerInput, ScannerOutput>
    {
        private ILineCount<MethodInfo> iInfoLineCount = null;
         
        public AssemblyScanner(ILineCount<MethodInfo> infoLineCount)
        {
            iInfoLineCount = infoLineCount;
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

            NetMethod netMethod = new NetMethod
            {
                Name = method.Name,
                LineCount = iInfoLineCount.GetMethodLineCount(method).ToString()
            };

            return netMethod;
        }
    }

}
