using FluentValidation;
using LegacyExplorer.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LegacyExplorer.Processors.Validators
{

    public class ScannerInputValidator : AbstractValidator<ScannerInput>
    {
        public ScannerInputValidator()
        {
            RuleFor(x => x.AssemblyPaths).NotNull().WithMessage("Path cannot be null");
            RuleFor(x => x.AssemblyPaths).NotEmpty().WithMessage("Path cannot be empty");
            //RuleFor(x => x.AssemblyPath).Matches(@"^(([a-zA-Z]:)|(\))(\{1}|((\{1})[^\]([^/:*?<>""|]*))+)$").WithMessage("Path is not valid");
        }
    }
    public class FilePathValidator : AbstractValidator<string>
    {
        public FilePathValidator()
        {
            Regex r = new Regex(@"^(([a-zA-Z]:)|(\))(\{1}|((\{1})[^\]([^/:*?<>""|]*))+)$");
            RuleFor(path => r.IsMatch(path)).Equal(true);
        }
    }
}

