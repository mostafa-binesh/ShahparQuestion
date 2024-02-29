using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    internal class TextFileEmployeeLogRepository: IEmployeeLogRepository
    {
        private string _filePath;

        public TextFileEmployeeLogRepository(string filePath)
        {
            _filePath = filePath;
        }

        public IEnumerable<EmployeeRecord> GetEmployeeLogs()
        {
            // Implement the file parsing logic here
        }
    }
}
