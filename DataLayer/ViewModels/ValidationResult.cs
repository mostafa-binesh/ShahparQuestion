﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.ViewModels
{
    public class ValidationResult
    {
        public bool IsValid => !Errors.Any();
        public List<string> Errors { get; set; } = new List<string>();

    }
}
