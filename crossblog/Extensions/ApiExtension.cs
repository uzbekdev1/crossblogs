using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace crossblog.Extensions
{
    public static class ApiExtension
    {
        public static string GetErrors(this ModelStateDictionary modelState)
        {
            var error = string.Join(Environment.NewLine, modelState.SelectMany(x => x.Value.Errors).Select(x => x.ErrorMessage));

            return error;
        }

    }
}
