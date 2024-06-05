using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Runtime.CompilerServices;

namespace ASP_to_React_Migration.Service
{
    internal class FileOperations
    {
        private Options _options;


        internal FileOperations(Options options)
        {
            _options = options;
        }
        public async Task<string> ReadFileContentAsync()
        {
            var filePath = _options.FilePath; 
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
                Console.WriteLine($"Error reading file: {ex.Message}");
                throw;
            }
        }
        
    }
}
