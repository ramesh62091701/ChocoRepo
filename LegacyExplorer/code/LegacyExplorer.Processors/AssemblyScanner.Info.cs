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

namespace LegacyExplorer.Processors
{
    public partial class AssemblyScanner : IScanner<ScannerInput, ScannerOutput>
    {
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
                Name = method.Name
            };

            if (method.ReflectedType.Name == "CsvExporter`1" || method.ReflectedType.Name == "CsvExporter")
            {
                int lineCount = 0;
                if (method.Name == "ExportToCsv" || method.Name == "ExportCollectionToCsv")
                {

                    System.Reflection.MethodBody methodBody = method.GetMethodBody();

                    byte[] instructionByteArray = methodBody.GetILAsByteArray();

                    for (int i = 0; i <= methodBody.GetILAsByteArray().Length - 1; i++)
                    {

                        if (instructionByteArray[i] == (byte)'\n')
                            lineCount++;

                    }
                }
                //netMethod.LineCount = $"Method {method.Name} and Line Count: {lineCount}";
                netMethod.LineCount = lineCount.ToString();
                Console.WriteLine($"Method Name: {method.Name}, Line Count: {netMethod.LineCount}");
            }
               
            return netMethod;

        }



        public int CountMethodLines(MethodDefinition method)
        {
            int lineCount = 0;
            int previousLineNumber = -1;
            //method.DebugInformation.SequencePoints;
            //method.Body.Instructions.
            //Mono.Cecil.Cil.MethodBody methodBody = method.Body;

            foreach (Instruction instruction in method.Body.Instructions)
            {

                SequencePoint sequencePoint = method.DebugInformation.GetSequencePoint(instruction);

                if (sequencePoint != null && sequencePoint.StartLine != previousLineNumber)
                {
                    //Increase the line count when a new line is encountered
                    previousLineNumber = sequencePoint.StartLine;
                    lineCount++;
                }
                else if (instruction != null && IsSignificantSequencePoint(instruction))
                    lineCount++;
                //else if (instruction != null && IsSignificantOpcodeBasedOnly(instruction.OpCode))
                //    lineCount++;
                else
                {

                    // Handle instructions without source code information here
                    Console.WriteLine($"Instruction Offset: {instruction.Offset}");
                    Console.WriteLine("No associated source code information.");
                    Console.WriteLine();
                }
            }

            return lineCount;
        }

        public bool IsSignificantSequencePoint(Instruction ins)
        {// SequencePoint sp

            //private const int HiddenLine = 0xfeefee;
            if (ins.OpCode == OpCodes.Nop)
                return false;
            if (ins.OpCode == OpCodes.Dup)
                return false;
            //if (ins.OpCode == OpCodes.Stloc_0 && ins.Operand == null)
            //    return false;
            //if (ins.OpCode == OpCodes.Ldloc_0 && ins.Operand == null)
            //    return false;
            //if (ins.OpCode == OpCodes.Ldarg_1 && ins.Operand == null)
            //    return false;
            //if (ins.OpCode == OpCodes.Newobj) //ins.OpCode == OpCodes.Callvirt
            //    return false;
            //if (ins.OpCode == OpCodes.Stloc_S || ins.OpCode == OpCodes.Ldloc_S)
            //    return false;
            if (ins.OpCode == OpCodes.Ret && ins.Next == null)
                return false;
            //if (ins.OpCode != OpCodes.Callvirt)
            //    return false;
            return true;
        }

        public bool IsSignificantOpcodeBasedOnly(OpCode opcode)
        {

            // List of opcodes that typically correspond to significant instructions
            // You may need to adjust this list based on your specific criteria
            OpCode[] significantOpcodes = new OpCode[]
            {
            OpCodes.Call, OpCodes.Callvirt, OpCodes.Newobj, OpCodes.Ldarg,
            OpCodes.Ldloc, OpCodes.Starg, OpCodes.Stloc, OpCodes.Br, OpCodes.Br_S,OpCodes.Refanytype,
            OpCodes.Sizeof, OpCodes.Initblk, OpCodes.Cpblk, OpCodes.Endfilter, OpCodes.Localloc
                // Add more opcodes as needed
            };

            return Array.Exists(significantOpcodes, op => op == opcode);
        }
        public void GetInstructionCountByMonoCecil()
        {

            //string assemblyPath = input.AssemblyPath; // Replace with your assembly path
            //string typeName = "LegacyExplorer.ConsoleApp.TestProgram3"; // Replace with the actual namespace and type name
            //string methodName = "Test"; // Replace with the actual method name

            //////// Load the assembly using Mono.Cecil
            //////AssemblyDefinition assembly1 = AssemblyDefinition.ReadAssembly(assemblyPath);

            //////// Find the type containing the method
            //////TypeDefinition type = assembly1.MainModule.Types.Single(t => t.FullName == typeName);

            ////////Find the method you want to analyze
            //////MethodDefinition method1 = type.Methods.Single(m => m.Name == methodName);

            //////// Create a DecompilationOptions instance
            //////var decompilationOptions = new DecompilationOptions();

            //////// Create a CSharpDecompiler instance
            //////var decompiler = new CSharpDecompiler(assembly.MainModule, decompilationOptions);

            //////// Decompile the method to C# code
            //////var syntaxTree = decompiler.DecompileMethod(targetMethod);

            //////// Output the decompiled code
            //////Console.WriteLine(syntaxTree.ToString());

            //////// Get the method's debug information using Mono.Cecil and PDB
            //////ISymbolReaderProvider symbolReaderProvider = new PdbReaderProvider();
            //////ISymbolReader symbolReader = symbolReaderProvider.GetSymbolReader(assembly1.MainModule, assemblyPath);

            //////if (symbolReader != null)
            //////{
            //////    foreach (var instruction in method1.Body.Instructions)
            //////    {
            //////        // Retrieve the SequencePoint for the instruction
            //////        MethodDebugInformation debugInformation = symbolReader.Read(method1);//, (int)instruction.Offset
            //////        if (debugInformation != null)
            //////        {
            //////            SequencePoint sequencePoint1 = debugInformation.GetSequencePoint(instruction);
            //////            if (sequencePoint1 != null) //&& sequencePoint1.IsSignificant
            //////            {
            //////                Console.WriteLine($"Instruction Offset: {instruction.Offset}");
            //////                Console.WriteLine($"Source File: {sequencePoint1.Document.Url}");
            //////                Console.WriteLine($"Start Line: {sequencePoint1.StartLine}");
            //////                Console.WriteLine($"End Line: {sequencePoint1.EndLine}");
            //////                Console.WriteLine();
            //////            }
            //////        }
            //////    }

            //////    symbolReader.Dispose();
            //////}
            //////else
            //////{
            //////    Console.WriteLine($"Debug information not found for {typeName}.{methodName}.");
            //////}



            ////

        }
        public void ReadAllDecompiledCodeFromAssembly(NetAssembly assembly)
        {
            byte[] ilBytes = File.ReadAllBytes(assembly.Location);//.methodBody.GetILAsByteArray();
            string methodBodyText = BitConverter.ToString(ilBytes).Replace("-", " ");
            //Encoding.UTF8.GetString(methodBodyText);// 

            // Load the IL code into a memory stream
            using (MemoryStream ms = new MemoryStream(ilBytes))
            {
                // Create a module definition for the IL code
                var module = ModuleDefinition.ReadModule(ms);


                // Create a decompiler settings instance
                var decompilerSettings = new DecompilerSettings();

                // Create a CSharpDecompiler instance
                var decompiler = new CSharpDecompiler(assembly.FileName, decompilerSettings);

                // Decompilation process
                var syntaxTree = decompiler.DecompileWholeModuleAsString();
                Console.WriteLine(syntaxTree);
            }

            Console.WriteLine(methodBodyText);


        }

    }

}
