using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Infrastructure.Filters
{
    public class ValidationFilter : IAsyncActionFilter // Implementing IAsyncActionFilter to handle action execution asynchronously
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(e => e.Value.Errors.Any())
                    .ToDictionary(
                        e => e.Key,
                        e => e.Value.Errors.Select(err => err.ErrorMessage).ToArray()
                    );  // Create a dictionary of errors with property names as keys and error messages as values
                context.Result = new BadRequestObjectResult(errors);  // Return a BadRequest with validation errors
                return;
            }
            await next(); // Proceed to the next action if model state is valid
        }
    }
}
