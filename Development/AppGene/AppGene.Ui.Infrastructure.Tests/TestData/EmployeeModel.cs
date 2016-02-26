﻿using AppGene.Common.Entities;
using System;

namespace AppGene.Ui.Infrastructure.Tests.TestData
{ 
    public class EmployeeModel
        : Employee
    {
        public int Age
        {
            get
            {
                int age = DateTime.Today.Year - base.Birthday.Year;
                return age > 0 ? age : 0;
            }
        }
    }
}