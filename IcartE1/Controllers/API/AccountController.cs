using AutoMapper;
using IcartE.Data;
using IcartE1.Data;
using IcartE1.Models;
using IcartE1.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IcartE1.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;
        private readonly ITokenService _tokenService;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager
            , IMapper mapper, ApplicationDbContext dbContext, ITokenService tokenService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _dbContext = dbContext;
            _tokenService = tokenService;
        }


        [HttpGet("details")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]
        public async Task<IActionResult> GetUserDetailsAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId == null) return NotFound();

            var customer = await _dbContext.Customers.FindAsync(userId);
            return Ok(customer);

        }

        [HttpPatch("update-address")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]
        public async Task<IActionResult> UpdateAddressAsync([FromBody] UpdateAddressViewModel addressViewModel)
        {
            if (!ModelState.IsValid) return ValidationProblem();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return NotFound();

            var customer = await _dbContext.Customers.FindAsync(userId);
            customer.Address = addressViewModel.Address;
            customer.Longitude = addressViewModel.Longitude;
            customer.Latitude = addressViewModel.Latitude;

            await _dbContext.SaveChangesAsync();
            return Ok();
        }


        //POST: api/Account/login 
        [HttpPost("login")]

        public async Task<IActionResult> LoginAsync([FromForm] LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(loginViewModel.Email);
                if (user != null)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user: user, password: loginViewModel.Password, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        var authClaims = new List<Claim>
                    {
                        new Claim(type: ClaimTypes.NameIdentifier,value: user.Id),
                        new Claim(type: ClaimTypes.Name,value: user.Email),
                        new Claim(type: JwtRegisteredClaimNames.Jti,value: Guid.NewGuid().ToString()),

                    };
                        var roles = await _userManager.GetRolesAsync(user);
                        authClaims.AddRange(roles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));

                        var accessToken = _tokenService.GenerateAccessToken(authClaims);
                        var refreshToken = _tokenService.GenerateRefreshToken();

                        user.RefreshToken = refreshToken;
                        user.RefreshTokenExpirationDate = DateTime.Now.AddMonths(1);
                        await _dbContext.SaveChangesAsync();

                        return Ok(new { accessToken, refreshToken });
                    }
                }


                ModelState.AddModelError(key: "Credentials", errorMessage: "Invalid Credentials");
                return ValidationProblem(statusCode: 401);
            }

            return ValidationProblem();

        }

        // POST: api/Account/register
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromForm] RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Email = registerViewModel.Email,
                    UserName = registerViewModel.Email,
                    PhoneNumber = registerViewModel.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user: user, password: registerViewModel.Password);

                if (result.Succeeded)
                {
                   var roleResult= await _userManager.AddToRoleAsync(user, "Customer");
                    if (roleResult.Succeeded)
                    {
                        var customer = _mapper.Map<Customer>(registerViewModel);
                        customer.Id = user.Id;
                        await _dbContext.Customers.AddAsync(customer);
                        await _dbContext.SaveChangesAsync();

                        return StatusCode(201);
                    }
                    var j = 0;
                        foreach (var error in roleResult.Errors)
                        {

                            ModelState.AddModelError(key: j.ToString(), errorMessage: error.Description);
                            j++;
                        }
                        return ValidationProblem();
                    
              
                }
                var i = 0;
                foreach (var error in result.Errors)
                {

                    ModelState.AddModelError(key: i.ToString(), errorMessage: error.Description);
                    i++;
                }
                return ValidationProblem();
            }
            return ValidationProblem();

        }

        //POST: api/Account/refresh
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync([FromBody] RefreshModel refreshModel)
        {
            if (ModelState.IsValid)
            {
                var principal = _tokenService.GetPrincipalFromExpiredToken(refreshModel.AccessToken);
                var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null || user.RefreshToken != refreshModel.RefreshToken || user.RefreshTokenExpirationDate <= DateTime.Now)
                {
                    return BadRequest(new { error = "Refresh token is invalid or expired." });
                }
                var accessToken = _tokenService.GenerateAccessToken(principal.Claims);
                var refreshToken = _tokenService.GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                await _dbContext.SaveChangesAsync();

                return Ok(new { accessToken, refreshToken });
            }
            return ValidationProblem();
        }

        [HttpGet("logout")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles ="Customer")]
        public async Task<IActionResult> LogoutAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound(new { error = "User doesn't exist" });

            user.RefreshToken = null;
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
