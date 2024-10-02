using System.ComponentModel.DataAnnotations;
using Core.BasicRoles;
using Microsoft.AspNetCore.Http;

namespace Core.ValidateAttributes.Roles;

public class RoleValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var httpContextAccessor  = validationContext.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
        var httpContext = httpContextAccessor?.HttpContext;
        
        var userRole = (UserRole?)value;
        var isAdmin = httpContext?.User.IsInRole(UserRole.ADMIN.ToString()) ?? false;
        
        if (userRole.Value != UserRole.USER && !isAdmin)
        {
            return new ValidationResult($"Only admins can assign roles other than {UserRole.USER.ToString()}.");
        }

        return ValidationResult.Success;
    }
}