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
using System.Runtime.Remoting;
using NLog;
using ICSharpCode.Decompiler.CSharp.Syntax;

namespace LegacyExplorer.Processors
{
    public partial class AssemblyScanner : IScanner<ScannerInput, ScannerOutput>
    {
        private ILineCount<MethodInfo, int> iLineCount = null;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private IConfiguration iconfiguration = null;

        public AssemblyScanner(ILineCount<MethodInfo,int> iLineCount)
        {
            this.iLineCount = iLineCount;

            iconfiguration = new ConfigurationUtility("NLog.config");
            iconfiguration.LoadNLogConfiguration();
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
            string className = "AssemblyScanner.Info";
            string methodName = "GetAssemblyInfo(Assembly)";
            
            logger.Info($"Class:{className},method:{methodName} Starts");

            NetAssembly netAssembly = new NetAssembly();
            try
            {
                
                netAssembly.Name = assembly.FullName;
                netAssembly.FileName = (assembly.Location.Split('\\')[assembly.Location.Split('\\').Length - 1]).Split(',')[0];
                netAssembly.Location = assembly.Location;
                netAssembly.Type = !String.IsNullOrEmpty(assembly.GetType().BaseType.AssemblyQualifiedName) ?
                    assembly.GetType().BaseType.AssemblyQualifiedName : string.Empty;
                netAssembly.Version = assembly.GetName().Version.ToString();


                object[] targetFrameworks = assembly.GetCustomAttributes(typeof(System.Runtime.Versioning.TargetFrameworkAttribute), false);
                //targetFrameworks = null;
                if (targetFrameworks.Length > 0)
                {
                    netAssembly.Framework = ((System.Runtime.Versioning.TargetFrameworkAttribute)targetFrameworks[0]).FrameworkName;
                }
                object[] assemblyTitles = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);

                if (assemblyTitles.Length > 0)
                {
                    netAssembly.Title = ((AssemblyTitleAttribute)assemblyTitles[0]).Title;
                }

                object[] assemblyCompanys = assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (assemblyCompanys.Length > 0)
                {
                    netAssembly.Company = ((AssemblyCompanyAttribute)assemblyCompanys[0]).Company;
                }


                object[] assemblyCopyrights = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (assemblyCopyrights.Length > 0)
                {
                    netAssembly.Copyright = ((AssemblyCopyrightAttribute)assemblyCopyrights[0]).Copyright;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Class:{className}, Method:{methodName},Error Message:{ex.Message}");
            }
            logger.Info($"Class:{className},method:{methodName} Ends");
          

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
                LineCount = this.iLineCount.GetMethodLineCount(method).ToString()
            };

            return netMethod;
        }
    }

}
