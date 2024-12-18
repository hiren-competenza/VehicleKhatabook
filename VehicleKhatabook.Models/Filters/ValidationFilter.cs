﻿using FluentValidation;
using Microsoft.AspNetCore.Http;
using VehicleKhatabook.Models.Common;

namespace VehicleKhatabook.Models.Filters
{
    public class ValidationFilter<T> : IEndpointFilter where T : class
    {
        private readonly IValidator<T> _validator;

        public ValidationFilter(IValidator<T> validator)
        {
            _validator = validator;
        }

        public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var body = context.Arguments.SingleOrDefault(arg => arg?.GetType() == typeof(T)) as T;
            if (body == null)
            {
                return Results.BadRequest("Invalid Request Body");
            }

            var validationResult = await _validator.ValidateAsync(body);
            if (!validationResult.IsValid)
            {
                //var errors = validationResult.Errors.Select(error => error.ErrorMessage).ToArray();
                var errorMessage = string.Join("; ", validationResult.Errors.Select(error => error.ErrorMessage));
                return Results.BadRequest(ApiResponse<string>.FailureResponse($"Validation failed: {errorMessage}"));
            }

            return await next(context);
        }
    }
}
