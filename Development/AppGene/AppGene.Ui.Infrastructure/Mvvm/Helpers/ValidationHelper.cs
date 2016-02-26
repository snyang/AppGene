using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AppGene.Ui.Infrastructure.Mvvm.Helpers
{
    public static class ValidationHelper
    {
        public static string ValidateProperty(Object entity, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return string.Empty;
            }

            var objectType = entity.GetType();
            var columnValue = objectType.GetProperty(propertyName).GetValue(entity, null);

            var validationContext = new ValidationContext(entity, null, null)
            {
                MemberName = propertyName
            };
            var validationResults = new List<ValidationResult>();

            Validator.TryValidateProperty(columnValue, validationContext, validationResults);

            if (validationResults.Count > 0)
            {
                return validationResults.First().ErrorMessage;
            }
            return string.Empty;
        }

        public static string ValidateObject(Object entity)
        {
            var validationContext = new ValidationContext(entity, null, null);
            var validationResults = new List<ValidationResult>();

            Validator.TryValidateObject(entity, validationContext, validationResults);

            if (validationResults.Count > 0)
            {
                var errors = validationResults.Select(r => r.ErrorMessage as string);
                return string.Join(Environment.NewLine, errors);
            }

            return string.Empty;
        }
    }
}