using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

public static class AutentificationEndpoint
{
    public static void MapAutentificationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/register", async (UserManager<ApplicationUser> userManager, RegisterModel model) =>
        {
            /*if (model.password != model.confirmPassword)
            {
                return Results.BadRequest("Passwords must match");
            }*/

            var user = new ApplicationUser
            {
                UserName = model.email,
                Email = model.email,
                FullName = model.fullName
            };

            var result = await userManager.CreateAsync(user, model.password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "User");
                return Results.Ok("Successfully registered user");
            }

            return Results.BadRequest(result.Errors);
        })
        .WithTags("Autentification");

        app.MapPost("/api/login", async (UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration config, LoginModel model) =>
        {
            var user = await userManager.FindByEmailAsync(model.email);
            if (user == null || !await userManager.CheckPasswordAsync(user, model.password))
            {
                return Results.Unauthorized();
            }

            var roles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim (ClaimTypes.NameIdentifier, user.Id),
                new Claim (ClaimTypes.Name, user.UserName),
                new Claim (ClaimTypes.Email, user.Email),
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("m3di4lyt1cm4st3rk3yf0rm4p1ngap1pr0j3ct"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: "medialytic.maping.com",
                audience: "medialytic.maping.com",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return Results.Ok(new { token = tokenHandler.WriteToken(token) });
        })
        .WithTags("Autentification");
    }
}