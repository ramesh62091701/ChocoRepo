using LegacyExplorer.Processors.Export;
using LegacyExplorer.Processors.Models;
using LegacyExplorer.Processors.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Configuration;
//using Mono.Cecil.Decompiler;
//using Mono.Cecil.Decompiler.Cil;
using Mono.Cecil.Pdb;
using System.Configuration.Assemblies;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

//using ICSharpCode.Decompiler.IL;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.TypeSystem;
using ICSharpCode.Decompiler.Metadata;
using static System.Reflection.Metadata.Ecma335.MethodBodyStreamEncoder;
using System.Collections;
//using ICSharpCode.ILSpy.Options;


namespace LegacyExplorer.Processors
{
    public class TypeScanner : IScanner<ScannerInput, ScannerOutput>
    {
        public ScannerOutput Scan(ScannerInput input)
        {
            bool isMonoCecilPackage = false;
            ScannerInputValidator inputValidator = new ScannerInputValidator();
            var validator = inputValidator.Validate(input);

            //if (validator.IsValid)
            //    throw new Exception(string.Join("\n", validator.Errors));


            ScannerOutput output = new ScannerOutput();

            try
            {
                if (!isMonoCecilPackage) {
                    // Load the assembly from the DLL file.
                    Assembly assembly = Assembly.LoadFrom(input.AssemblyPath);

                    //Get assembly info
                    NetAssembly netAssembly = GetAssemblyInfo(assembly);
                    Console.WriteLine($"{netAssembly.Id}");

                    output.Assemblies.Add(netAssembly);

                    //Get assembly references
                    output.References = GetAssemblyReferenceInfo(assembly);


                    ////assigning root assembly Guid to all the reference assemblt as a reference
                    //netAssembly.References.ForEach(reff =>
                    //{

                    //    reff.AssemblyId = netAssembly.Id;
                    //});


                    // Get all types
                    IEnumerable<TypeInfo> allTypes = assembly.DefinedTypes.ToArray();

                    foreach (Type typeClass in allTypes)
                    {
                        // Get type info
                        NetType netType = GetTypeInfo(typeClass);

                        ////Get fields  --commenting due to bring deferent details of field instead of exact name, type of field. 
                        ///added Get properties block below
                        //foreach (FieldInfo field in typeClass.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                        //{
                        //    NetField netField = GetFieldInfo(field);
                        //    netType.Fields.Add(netField);

                        //}
                        //Get properties
                        foreach (PropertyInfo field in typeClass.GetProperties())
                        {
                            NetField netField = GetPropertyInfo(field);
                            output.Fields.Add(netField);

                        }
                        //Get fields
                        foreach (MethodInfo method in typeClass.GetMethods(BindingFlags.NonPublic | BindingFlags.Public |BindingFlags.Instance)) // 
                        {
                            //if ((method.Attributes.HasFlag(System.Reflection.MethodAttributes.Public
                            //    | System.Reflection.MethodAttributes.HideBySig)
                            //    && !method.Attributes.HasFlag(System.Reflection.MethodAttributes.SpecialName))
                            //    && method.MethodImplementationFlags.HasFlag(System.Reflection.MethodImplAttributes.IL))
                            //{
                                NetMethod netMethod = GetMethodsInfo(method, netAssembly);
                                output.Methods.Add(netMethod);
                            //}
                        }




                        output.Types.Add(netType);

                    }
                }
                else {
                    using (var assembly = AssemblyDefinition.ReadAssembly(input.AssemblyPath))
                    {


                        ////assigning root assembly Guid to all the reference assemblt as a reference
                        //netAssembly.References.ForEach(reff =>
                        //{

                        //    reff.AssemblyId = netAssembly.Id;
                        //});
                        
                        foreach (var typeClass in assembly.MainModule.Types)
                        {
                            if (typeClass != null) //CsvExporter`1, CsvExporter // && type.Name == "Program" || type.Name == "Program"
                            {

                                ////Get assembly info
                                //NetAssembly netAssembly1 = GetAssemblyInfo(assembly1);
                                //Console.WriteLine($"{netAssembly1.Id}");

                                //output.Assemblies.Add(netAssembly1);

                                ////Get assembly references
                                //output.References = GetAssemblyReferenceInfo(assembly1);
                                //// Get type info
                                //NetType netType = GetTypeInfo(typeClass);

                                //////Get fields  --commenting due to bring deferent details of field instead of exact name, type of field. 
                                /////added Get properties block below
                                ////foreach (FieldInfo field in typeClass.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                                ////{
                                ////    NetField netField = GetFieldInfo(field);
                                ////    netType.Fields.Add(netField);

                                ////}
                                ////Get properties
                                //foreach (PropertyInfo field in typeClass.GetProperties())
                                //{
                                //    NetField netField = GetPropertyInfo(field);
                                //    output.Fields.Add(netField);

                                //}

                                var methods = typeClass.Methods;//.SingleOrDefault(m => m.Name == methodName);
                                int lineCount = 0;
                                foreach (var method in typeClass.Methods)
                                {
                                    if (method != null)
                                    {
                                        lineCount = 0;
                                        //Console.WriteLine($"Declaring Type - {method.DeclaringType.Name}, Type Class - {typeClass.Name}, type - {type.Name}");

                                        NetMethod netMethod = new NetMethod();

                                        lineCount = CountMethodLines(method);

                                        netMethod.LineCount = $"Method {method.Name} and Line Count: {lineCount}";
                                        output.Methods.Add(netMethod);


                                        Console.WriteLine($"Method {method.Name} in {typeClass.Name} has {lineCount} lines of code.");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Method {method.Name} not found in {typeClass.Name}.");
                                    }

                                }
                            }
                            else
                            {
                                Console.WriteLine($"Type {typeClass.Name} not found in the assembly.");
                            }

                        }
                    }
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

        public NetField GetPropertyInfo(PropertyInfo property)
        {

            NetField netField = new NetField();

            netField.Name = property.Name;
            netField.FieldType = property.PropertyType.Name;

            return netField;

        }

        public NetMethod GetMethodsInfo(MethodInfo method, NetAssembly assembly)
        {

            NetMethod netMethod = new NetMethod();
            netMethod.Name = method.Name;

            if (method.ReflectedType.Name == "CsvExporter`1" || method.ReflectedType.Name == "CsvExporter")
            {
                int cnt = 0;
                if (method.Name == "ExportToCsv" || method.Name == "ExportCollectionToCsv")
                {

                    System.Reflection.MethodBody methodBody = method.GetMethodBody();
                    //int[] ar = { };

                    byte[] ar2 = methodBody.GetILAsByteArray().ToArray();
                    
                    for (int i =0; i<=methodBody.GetILAsByteArray().Length-1;i++)
                    {
                        
                        if (ar2[i] == (byte)'\n')
                            cnt++;

                    }
                }
                netMethod.LineCount = $"Method {method.Name} and Line Count: {cnt}";
                Console.WriteLine(netMethod.LineCount);

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