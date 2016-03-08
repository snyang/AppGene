using AppGene.Common.Entities;
using AppGene.Common.Entities.Infrastructure.Annotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace AppGene.Common.Entities.Models
{
    public class EmployeeModel
        : Employee
    {
        [ComputeRelationship(ReferenceColumn = "Birthday")]
        public int Age
        {
            get
            {
                int age = DateTime.Today.Year - base.Birthday.Year;
                return age > 0 ? age : 0;
            }
        }

        //[ComputeRelationship(ComputedColumn = "FemaleBenefit")]
        //public new Gender Gender
        //{
        //    get
        //    {
        //        return base.Gender;
        //    }
        //    set
        //    {
        //        base.Gender = value;
        //        if (base.Gender!= Gender.Female)
        //        {
        //            base.FemaleBenefit = 0;
        //        }
        //    }
        //}

        [ComputeRelationship(ReferenceColumn = "Gender")]
        public bool FemaleBenefit_IsReadOnly
        {
            get
            {
                return Gender != Gender.Female;
            }
        }

    }
}
