using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppGene.Db.Model
{
    public partial class Employee
    {
        public int EmployeeID { get; set; }

        int employeeCode;
        [Index(IsUnique = true)]
        public int EmployeeCode
        {
            get { return employeeCode; }
            set
            {
                HandleSet(() =>
                {
                    this.employeeCode = value;
                }, "EmployeeCode");
            }
        }

        string employeeName;
        [StringLength(50, ErrorMessage = "Employee name is too long.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Employee name is required")]
        public string EmployeeName
        {
            get { return employeeName; }
            set
            {
                HandleSet(() =>
                {
                    this.employeeName = value;
                }, "EmployeeName");
            }
        }

        Genders gender;
        [Range(0, 1, ErrorMessage ="Gender must be female or male.")]
        public Genders Gender
        {
            get { return gender; }
            set
            {
                HandleSet(()=>
                { 
                    this.gender = value;
                }, "Gender");
            }
        }

        DateTime birthday;
        [Column(TypeName = "date")]
        public DateTime Birthday
        {
            get { return birthday; }
            set
            {
                HandleSet(() =>
                {
                    this.birthday = value;
                }, "Birthday");
            }
        }
    }
}
