using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using ReadCSharpApplication.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class Program
{
    static async Task Main(string[] args)
    {
        // To get all methods from the whole SLN
        //await GetMethodsClass.getAllMethods();

        // Reads all the method of the given class and its method dependencies and saves the code to a file.
        await ReadCSFile2.GetAllMethodsInClass("Assign"); 
        //await ReadCSFile.GetAllMethodsInClass("Assign"); 
    }

}

