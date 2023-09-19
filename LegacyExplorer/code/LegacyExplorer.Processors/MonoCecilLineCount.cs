using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler;
using LegacyExplorer.Processors.Interfaces;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyExplorer.Processors
{
    public class MonoCecilLineCount : ILineCount<MethodDefinition,int>
    {
        public int GetMethodLineCount(MethodDefinition method)
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
