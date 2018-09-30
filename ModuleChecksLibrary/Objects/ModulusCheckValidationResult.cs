using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleChecksLibrary
{
    public class ModulusCheckValidationResult
    {
        public bool IsCheckValid { get; set; }
        public int? ExceptionNotProcessed { get; set; }
        public string WeightDigits { get; set; }
        public int  Remainder { get; set; }
        public bool IsSortCodeValid { get; set; }
        public bool IsAccountNoValid { get; set; }
    }
}
