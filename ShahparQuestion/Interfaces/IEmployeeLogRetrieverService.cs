﻿using ShahparQuestion.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShahparQuestion.Interfaces
{
    public interface IEmployeeLogRetrieverService
    {
        public EmployeeLogParsingResult GetEmployeeLogs();
    }
}