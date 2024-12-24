using Microsoft.AspNetCore.Authorization;
using ProjetoMvc.Models.Enum;
using System.Security.Claims;

namespace ProjetoMvc.Validators
{
    public class PermissionRequirement(params UserPermissionEnum[] requiredPermissions) : IAuthorizationRequirement
    {
        public List<UserPermissionEnum> RequiredPermissions { get; } = [.. requiredPermissions];
    }

    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var permissionClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

            if (permissionClaim != null)
            {
                var userPermission = Enum.Parse<UserPermissionEnum>(permissionClaim.Value);

                // Verifica se o usuário tem uma das permissões (as que podem foram adicionadas no program)
                if (requirement.RequiredPermissions.Contains(userPermission))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }

    public static class UserExtensions
    {
        public static bool IsAdminOrDeveloper(this ClaimsPrincipal user)
        {
            var userPermission = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (Enum.TryParse(userPermission, out UserPermissionEnum permission))
            {
                return permission == UserPermissionEnum.Admin || permission == UserPermissionEnum.Developer;
            }
            return false;
        }
    }
}
