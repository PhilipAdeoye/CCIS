using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;

namespace CCMvc.Infrastructure
{
    public static class ModelStateHelper
    {
        public static IEnumerable ErrorsWithKeys(this ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return modelState
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors
                            .Select(e => e.ErrorMessage)
                            .ToArray()
                    ).Where(m => m.Value.Count() > 0);

            }
            return null;
        }

        public static IEnumerable Errors(this ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return modelState.Values
                    .SelectMany(m => m.Errors)
                    .Select(e => e.ErrorMessage);

            }
            return null;
        }
    }
}