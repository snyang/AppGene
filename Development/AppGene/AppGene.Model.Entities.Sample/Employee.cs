using AppGene.Model.Entities.Sample;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppGene.Model.Entities
{
    public class Employee
    {
        public int EmployeeID { get; set; }

        [Index(IsUnique = true)]
        [DisplayFormat(DataFormatString = "0000")]
        public int EmployeeCode { get; set; }

        [StringLength(50, ErrorMessage = "Employee name is too long.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Employee name is required")]
        [Display(ResourceType = typeof(EntityResource),
            Name = "Employee_EmployeeName_Name",
            ShortName = "Employee_EmployeeName_ShortName",
            Description = "Employee_EmployeeName_Description",
            Prompt = "Employee_EmployeeName_Prompt")]
        [DefaultValue("New Employee")]
        public string EmployeeName { get; set; }

        [Range(0, 1, ErrorMessage = "Gender must be female or male.")]
        public Genders Gender { get; set; }

        [Column(TypeName = "date")]
        public DateTime Birthday { get; set; }
    }
}