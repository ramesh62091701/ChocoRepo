using dnlib.DotNet;
using dnSpy.Decompiler.Settings;
using dnSpy.Decompiler;
using dnSpy.Decompiler.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.Decompiler;

namespace CodelExplorerLib
{
    public class Service
    {
        public void Process(string path)
        {

        }

        private void Decompile(string path)
        {
        }

        public static void DecompileAssembly(string assemblyPath)
        { // Load the assembly using dnlib
            var module = ModuleDefMD.Load(assemblyPath);

            // Create the decompiler instance
            var decompiler = new ICSharpCode.Decompiler.CSharp.CSharpDecompiler(assemblyPath, new DecompilerSettings());

            // Decompile the assembly
            var decompiledCode = decompiler.DecompileWholeModuleAsString();

            // Save the decompiled code to a file
            //System.IO.File.WriteAllText(outputPath, decompiledCode);
        }
    }
    
}
