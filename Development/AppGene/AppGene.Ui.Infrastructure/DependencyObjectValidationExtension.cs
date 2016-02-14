using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AppGene.Ui.Infrastructure
{
    public static class DependencyObjectValidationExtension
    {
        public static bool IsValid(this DependencyObject obj)
        {
            return !Validation.GetHasError(obj)
                && LogicalTreeHelper.GetChildren(obj)
                    .OfType<DependencyObject>()
                    .All(child => IsValid(child));
        }

        public static IEnumerable<ValidationError> GetErrors(this DependencyObject obj)
        {
            return Validation.GetErrors(obj)
                .Concat(LogicalTreeHelper.GetChildren(obj).OfType<DependencyObject>().SelectMany(d => d.GetErrors()))
                .ToList();
        }

        public static string GetErrors(this DependencyObject obj, string delimiter, bool distinct = true)
        {
            var errors = obj.GetErrors().Select(e => e.ErrorContent as string);

            if (distinct)
            {
                errors = errors.Distinct();
            }

            return string.Join(delimiter, errors);
        }
    }
}