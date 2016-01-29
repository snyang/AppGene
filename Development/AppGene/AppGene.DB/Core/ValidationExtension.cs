using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGene.Db.Core
{
    public static class ValidationExtension
    {
        public static string ValidateColumn(this IDataErrorInfo obj, string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
            {
                return string.Empty;
            }

            var objectType = obj.GetType();
            var columnValue = objectType.GetProperty(columnName).GetValue(obj, null);

            var validationContext = new ValidationContext(obj, null, null) {
                                        MemberName = columnName
                                    };
            var validationResults = new List<ValidationResult>();

            Validator.TryValidateProperty(columnValue, validationContext, validationResults);

            if (validationResults.Count > 0)
            {
                return validationResults.First().ErrorMessage;
            }
            return string.Empty;
        }

        public static string ValidateObject(this IDataErrorInfo obj)
        {
            var objectType = obj.GetType();
            
            var validationContext = new ValidationContext(obj, null, null);
            var validationResults = new List<ValidationResult>();

            Validator.TryValidateObject(obj, validationContext, validationResults);

            if (validationResults.Count > 0)
            {
                var errors = validationResults.Select(r => r.ErrorMessage as string);
                return string.Join(Environment.NewLine, errors);
            }

            return string.Empty;
        }
    }
}
