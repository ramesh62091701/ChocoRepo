using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LegacyExplorer.Processors.Interfaces;

namespace LegacyExplorer.Processors
{
    public class RefelectionLineCount : ILineCount<MethodInfo,int>
    {
        public int GetMethodLineCount(MethodInfo method) {
           
            int lineCount = 0;
            
            System.Reflection.MethodBody methodBody = method.GetMethodBody();
            if (methodBody != null)
            {
                byte[] instructionByteArray = methodBody.GetILAsByteArray();

                for (int i = 0; i <= methodBody.GetILAsByteArray().Length - 1; i++)
                {

                    if (instructionByteArray[i] == (byte)'\n')
                        lineCount++;

                }
            }
            Console.WriteLine($"Method Name: {method.Name}, Line Count: {lineCount}");

            return lineCount;
        }

    }
}
