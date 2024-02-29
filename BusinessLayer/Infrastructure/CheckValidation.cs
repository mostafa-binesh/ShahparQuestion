using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ShahparQuestion.Infrastructure
{
    public class CheckValidation
    {
        public bool IsTextExtension(string fileExtension)
        {
            if (string.Equals(fileExtension, ".txt", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
    }
}
