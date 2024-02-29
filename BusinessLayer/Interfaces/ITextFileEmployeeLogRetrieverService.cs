using DataLayer.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface ITextFileEmployeeLogRetrieverService : IEmployeeLogRetrieverService
    {
        void SetFile(IFormFile file);
    }
}
