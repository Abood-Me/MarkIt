using MarkItDesktop.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MarkItDesktop.ViewModels
{
    public abstract class BaseValidationViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<ValidationResult>> Errors = new();
        public bool HasErrors => Errors.Any(e => e.Value.Any());

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            if (propertyName is null)
                return null;

            return Errors.ContainsKey(propertyName) ? Errors[propertyName] : null;
        }

        public void ValidateProperty<T>(T? propertyValue, [CallerArgumentExpression("propertyValue")] string? propertyName = null)
        {
            if (!Errors.ContainsKey(propertyName!))
                Errors[propertyName!] = new List<ValidationResult>();

            Errors[propertyName!].Clear();

            Validator.TryValidateProperty(
                propertyValue,
                new ValidationContext(this, null) { MemberName = propertyName },
                Errors[propertyName!]);

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

    }
}
