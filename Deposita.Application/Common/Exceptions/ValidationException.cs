using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deposita.Application.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : base("One or more validation failures have occurred.")
        {
            Errors = failures
                .GroupBy(f => f.PropertyName, f => f.ErrorMessage)
                .ToDictionary(g => g.Key, g => g.ToArray());
        }
    }
}
