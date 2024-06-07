using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Runtime.CompilerServices;
using Extractor.Utils;

namespace Extractor.Service
{
    internal class FileOperations
    {

        public async Task<string> ReadFileContentAsync(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"The file '{filePath}' was not found.");
                }

                return await File.ReadAllTextAsync(filePath);
            }
            catch (Exception ex)
            {
                Logger.Log($"Error reading file: {ex.Message}");
                throw;
            }
        }
        
    }
}
