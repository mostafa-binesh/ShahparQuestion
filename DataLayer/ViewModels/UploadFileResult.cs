using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class UploadFileResult
    {
        public IEnumerable<string> StructureError { get; set; } = new List<string>();
        public IEnumerable<string> ValidationErrors { get; set; } = new List<string>();
        public bool GeneratingResultSuccesful => !StructureError.Any() && !ValidationErrors.Any() && String.IsNullOrEmpty(ErrorMessage);
        public bool ValidationSuccessful => !StructureError.Any() && !ValidationErrors.Any();
        public bool FileUploaded { get; set; } = false;
        public string ErrorMessage { get; set; } = String.Empty;
    }
}
