using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_to_React_Migration
{
    public class Options
    {

        [Option('f', "aspx-file",  HelpText = "Path to aspx file to read")]
        public required string FilePath { get; set; }

        [Option('i', "image-file",  HelpText = "Path to image file to read")]
        public required string ImagePath { get; set; }

        [Option('n', "new-project-path", Required = true, HelpText = "Path to create the new project")]
        public required string PathForNewProject { get; set; }
    }
}
