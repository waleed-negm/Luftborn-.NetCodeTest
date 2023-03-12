using AutoMapper;
using Core.Application.Dto;
using Core.Application.Dto.auth;
using Core.Application.Services;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly JWT _jwt;
        private readonly IEmailService _mailService;
        private readonly IMapper mapper;

        public AuthService(UserManager<User> userManager, IConfiguration configuration, IOptions<JWT> jwt, IEmailService mailService,IMapper mapper)
        {
            _userManager = userManager;
            _configuration = configuration;
            _jwt = jwt.Value;
            _mailService = mailService;
            this.mapper = mapper;
        }
        public async Task<ResponseDto> RegisterAsync(RegisterDto model)
        {
            try
            {
                ResponseDto res = new();
                if(model.Password != model.ConfirmPassword)
                {
                    res.Message = "confirm password dont match password";
                    return res;
                }
                if (await _userManager.FindByNameAsync(model.Username) is not null)
                {
                    res.Message = "Username is already registered!";
                    return res;
                }
                if (await _userManager.FindByEmailAsync(model.Email) is not null)
                {
                    res.Message = "Email is already registered!";
                    return res;
                }
                var user = new User
                {
                    UserName = model.Username,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    FirstName = model.FirstName,
                    LastName = model.LastName,

                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors) res.Message += $"{error.Description}\n";
                    return res;
                }
                var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
                var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);
                string url = $"{_configuration["AppUrl"]}/auth/confirmEmail?userid={user.Id}&token={validEmailToken}";
                string template = $"<!doctype html><html lang=\"en-US\"><head><meta content=\"text/html; charset=utf-8\" http-equiv=\"Content-Type\" />" +

                    $"<title>Welcome to Luftborn!</title>" +
                    $"<meta name=\"description\" content=\"confirm email.\"><style type=\"text/css\">a:hover{{text-decoration: underline !important;}}</style></head><body marginheight=\"0\" topmargin=\"0\" marginwidth=\"0\" style=\"margin: 0px;background-color: #f2f3f8;\" leftmargin=\"0\"><table cellspacing=\"0\" border=\"0\" cellpadding=\"0\" width=\"100%\" bgcolor=\"#f2f3f8\"style=\"@import url(https://fonts.googleapis.com/css?family=Rubik:300,400,500,700|Open+Sans:300,400,600,700); font-family: 'Open Sans', sans-serif;\"><tr><td><table style=\"background-color: #f2f3f8; max-width:670px;  margin:0 auto;\" width=\"100%\" border=\"0\"align=\"center\" cellpadding=\"0\" cellspacing=\"0\"><tr><td style=\"height:20px;\">&nbsp;</td></tr><tr><td><table width=\"95%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\"style=\"max-width:670px;background:#fff;border-radius:3px; text-align:center;-webkit-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);-moz-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);box-shadow:0 6px 18px 0 rgba(0,0,0,.06);\"><tr><td style=\"height:40px;\">&nbsp;</td></tr><tr><td style=\"padding:0 35px;\"><h1 style=\"color:#1e1e2d; font-weight:500; margin:0;font-size:32px;font-family:'Rubik',sans-serif;\">" +

                    $"Welcome to Luftborn!" +
                    $"</h1><span style=\"display:inline-block; vertical-align:middle; margin:29px 0 26px; border-bottom:1px solid #cecece; width:100px;\"></span><p style=\"color:#455056; font-size:15px;line-height:24px; margin:0;\">" +

                    $"you need to confirm your account. Just press the button below." +
                    $"</p><a href=\"{url}\"style=\"background:#f15a23;text-decoration:none !important; font-weight:500; margin-top:35px; color:#fff;text-transform:uppercase; font-size:14px;padding:10px 24px;display:inline-block;border-radius:50px;\">" +

                    $"Confirm Email</a>" +
                    $"</td></tr><tr><td style=\"height:40px;\">&nbsp;</td></tr></table></td><tr><td style=\"height:80px;\">&nbsp;</td></tr></table></td></tr></table></body></html>";

                await _mailService.SendEmailAsync(user.Email, "Confirm your email", template);
                await _userManager.AddToRoleAsync(user, "User");
                res.Status = true;
                res.Message = "account created sucessfully, please check your email!";
                return res;
            }
            catch (Exception ex) { return new ResponseDto() { Message = ex.Message, Body = ex }; }
        }
        
        public async Task<ResponseDto> GetTokenAsync(LoginCredentialsDto model)
        {
            try 
            {
                ResponseDto res = new();
                User? user = await _userManager.FindByNameAsync(model.UserName);
                user = user is null ? await _userManager.FindByEmailAsync(model.UserName) : user;
                if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    res.Message = "wrong username or password";
                    return res;
                }
                if (!user.EmailConfirmed)
                {
                    if (user.Email is null)
                    {
                        res.Message = "email not found, please contact software department";
                        return res;
                    }
                    var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
                    var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);
                    string url = $"{_configuration["AppUrl"]}/auth/confirmEmail?userid={user.Id}&token={validEmailToken}";
                    string template = $"<!doctype html><html lang=\"en-US\"><head><meta content=\"text/html; charset=utf-8\" http-equiv=\"Content-Type\" />" +

                        $"<title>Welcome to Luftborn!</title>" +
                        $"<meta name=\"description\" content=\"confirm email.\"><style type=\"text/css\">a:hover{{text-decoration: underline !important;}}</style></head><body marginheight=\"0\" topmargin=\"0\" marginwidth=\"0\" style=\"margin: 0px;background-color: #f2f3f8;\" leftmargin=\"0\"><table cellspacing=\"0\" border=\"0\" cellpadding=\"0\" width=\"100%\" bgcolor=\"#f2f3f8\"style=\"@import url(https://fonts.googleapis.com/css?family=Rubik:300,400,500,700|Open+Sans:300,400,600,700); font-family: 'Open Sans', sans-serif;\"><tr><td><table style=\"background-color: #f2f3f8; max-width:670px;  margin:0 auto;\" width=\"100%\" border=\"0\"align=\"center\" cellpadding=\"0\" cellspacing=\"0\"><tr><td style=\"height:20px;\">&nbsp;</td></tr><tr><td><table width=\"95%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\"style=\"max-width:670px;background:#fff;border-radius:3px; text-align:center;-webkit-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);-moz-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);box-shadow:0 6px 18px 0 rgba(0,0,0,.06);\"><tr><td style=\"height:40px;\">&nbsp;</td></tr><tr><td style=\"padding:0 35px;\"><h1 style=\"color:#1e1e2d; font-weight:500; margin:0;font-size:32px;font-family:'Rubik',sans-serif;\">" +

                        $"Welcome to Luftborn!" +
                        $"</h1><span style=\"display:inline-block; vertical-align:middle; margin:29px 0 26px; border-bottom:1px solid #cecece; width:100px;\"></span><p style=\"color:#455056; font-size:15px;line-height:24px; margin:0;\">" +

                        $"you need to confirm your account. Just press the button below." +
                        $"</p><a href=\"{url}\"style=\"background:#f15a23;text-decoration:none !important; font-weight:500; margin-top:35px; color:#fff;text-transform:uppercase; font-size:14px;padding:10px 24px;display:inline-block;border-radius:50px;\">" +

                        $"Confirm Email</a>" +
                        $"</td></tr><tr><td style=\"height:40px;\">&nbsp;</td></tr></table></td><tr><td style=\"height:80px;\">&nbsp;</td></tr></table></td></tr></table></body></html>";

                    await _mailService.SendEmailAsync(user.Email, "Confirm your email",template);
                    res.Message = "Please Confirm your Email";
                    return res;
                }
                JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);
                AuthDto authDto = new()
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    ExpiresOn = jwtSecurityToken.ValidTo,
                    UserName = $"{user.FirstName} {user.LastName}",
                    UserId = user.Id,
                    Email = user.Email,
                    Roles = _userManager.GetRolesAsync(user).Result.ToList()
                };
                res.Message = "logged in sucessfully";
                res.Body = authDto;
                res.Status = true;
                return res;
            }
            catch (Exception ex) { return new ResponseDto() { Message = ex.Message, Body = ex }; }
        }
        
        public async Task<ResponseDto> EditUser(string Id, UserDto model)
        {
            try { 
            ResponseDto res = new();
            if (Id != model.Id)
            {
                res.Message = "invalid user id.";
                return res;
            }
            User? user = await _userManager.FindByIdAsync(Id);
            if (user is null)
            {
                res.Message = "user not found";
                return res;
            }
                if (model.Email != user.Email)
                {
                    if (await _userManager.FindByEmailAsync(model.Email) is not null)
                    {
                        res.Message = "this email is assigned to another user";
                        return res;
                    }
                    user.Email = model.Email;
                    user.EmailConfirmed = false;
                    string confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    byte[] encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
                    string validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);
                    string url = $"{_configuration["AppUrl"]}/auth/confirmEmail?userid={user.Id}&token={validEmailToken}";
                    string template = $"<!doctype html><html lang=\"en-US\"><head><meta content=\"text/html; charset=utf-8\" http-equiv=\"Content-Type\" />" +

                   $"<title>Welcome to Luftborn!</title>" +
                   $"<meta name=\"description\" content=\"confirm email.\"><style type=\"text/css\">a:hover{{text-decoration: underline !important;}}</style></head><body marginheight=\"0\" topmargin=\"0\" marginwidth=\"0\" style=\"margin: 0px;background-color: #f2f3f8;\" leftmargin=\"0\"><table cellspacing=\"0\" border=\"0\" cellpadding=\"0\" width=\"100%\" bgcolor=\"#f2f3f8\"style=\"@import url(https://fonts.googleapis.com/css?family=Rubik:300,400,500,700|Open+Sans:300,400,600,700); font-family: 'Open Sans', sans-serif;\"><tr><td><table style=\"background-color: #f2f3f8; max-width:670px;  margin:0 auto;\" width=\"100%\" border=\"0\"align=\"center\" cellpadding=\"0\" cellspacing=\"0\"><tr><td style=\"height:20px;\">&nbsp;</td></tr><tr><td><table width=\"95%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\"style=\"max-width:670px;background:#fff;border-radius:3px; text-align:center;-webkit-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);-moz-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);box-shadow:0 6px 18px 0 rgba(0,0,0,.06);\"><tr><td style=\"height:40px;\">&nbsp;</td></tr><tr><td style=\"padding:0 35px;\"><h1 style=\"color:#1e1e2d; font-weight:500; margin:0;font-size:32px;font-family:'Rubik',sans-serif;\">" +

                   $"Welcome to Luftborn!" +
                   $"</h1><span style=\"display:inline-block; vertical-align:middle; margin:29px 0 26px; border-bottom:1px solid #cecece; width:100px;\"></span><p style=\"color:#455056; font-size:15px;line-height:24px; margin:0;\">" +

                   $"you need to confirm your account. Just press the button below." +
                   $"</p><a href=\"{url}\"style=\"background:#f15a23;text-decoration:none !important; font-weight:500; margin-top:35px; color:#fff;text-transform:uppercase; font-size:14px;padding:10px 24px;display:inline-block;border-radius:50px;\">" +

                   $"Confirm Email</a>" +
                   $"</td></tr><tr><td style=\"height:40px;\">&nbsp;</td></tr></table></td><tr><td style=\"height:80px;\">&nbsp;</td></tr></table></td></tr></table></body></html>";

                    await _mailService.SendEmailAsync(model.Email, "Confirm your new email", template); 
                }
                user.FirstName = model.FirstName; 
                user.LastName = model.LastName;
                user.PhoneNumber = model.PhoneNumber;
                user.UserName = model.UserName;
            await _userManager.UpdateAsync(user);
            res.Message = "user updated sucessfully";
            res.Status = true;
            res.Body = mapper.Map<UserDto>(user);
            return res;
            }
            catch (Exception ex) { return new ResponseDto() { Message = ex.Message, Body = ex }; }
        }

        public async Task<ResponseDto> ConfirmEmailAsync(string userId, string token)
        {
            try 
            {
                ResponseDto res = new();
                User? user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    res.Message = "user not found";
                    return res;
                }
                byte[] decodedToken = WebEncoders.Base64UrlDecode(token);
                string normalToken = Encoding.UTF8.GetString(decodedToken);
                IdentityResult result = await _userManager.ConfirmEmailAsync(user, normalToken);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors) res.Message += $"{error.Description},";
                    return res;
                }
                var jwtSecurityToken = await CreateJwtToken(user);
                res.Body= new AuthDto()
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    ExpiresOn = jwtSecurityToken.ValidTo,
                    Roles = _userManager.GetRolesAsync(user).Result.ToList(),
                    UserName = $"{user.FirstName} {user.LastName}",
                    UserId = user.Id,
                    Email = user.Email
                };
                res.Status = true;
                res.Message = "email confirmed sucessfully";
                return res;
            }
            catch (Exception ex){ return new ResponseDto() { Message = ex.Message, Body = ex };}
        }

        public async Task<ResponseDto> ForgetPasswordAsync(string email)
        {
            try 
            {
                ResponseDto res = new();
                User? user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    res.Message = "invalid email adress";
                    return res;    
                }
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var encodedToken = Encoding.UTF8.GetBytes(token);
                var validToken = WebEncoders.Base64UrlEncode(encodedToken);
                string url = $"{_configuration["AppUrl"]}/auth/resetPassword?email={email}&token={validToken}";
                string template = $"<!doctype html><html lang=\"en-US\"><head><meta content=\"text/html; charset=utf-8\" http-equiv=\"Content-Type\" />" +
                $"<title>Reset Password Email!</title>" +
                $"<meta name=\"description\" content=\"Reset Password.\"><style type=\"text/css\">a:hover{{text-decoration: underline !important;}}</style></head><body marginheight=\"0\" topmargin=\"0\" marginwidth=\"0\" style=\"margin: 0px;background-color: #f2f3f8;\" leftmargin=\"0\"><table cellspacing=\"0\" border=\"0\" cellpadding=\"0\" width=\"100%\" bgcolor=\"#f2f3f8\"style=\"@import url(https://fonts.googleapis.com/css?family=Rubik:300,400,500,700|Open+Sans:300,400,600,700); font-family: 'Open Sans', sans-serif;\"><tr><td><table style=\"background-color: #f2f3f8; max-width:670px;  margin:0 auto;\" width=\"100%\" border=\"0\"align=\"center\" cellpadding=\"0\" cellspacing=\"0\"><tr><td style=\"height:20px;\">&nbsp;</td></tr><tr><td><table width=\"95%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\"style=\"max-width:670px;background:#fff;border-radius:3px; text-align:center;-webkit-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);-moz-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);box-shadow:0 6px 18px 0 rgba(0,0,0,.06);\"><tr><td style=\"height:40px;\">&nbsp;</td></tr><tr><td style=\"padding:0 35px;\"><h1 style=\"color:#1e1e2d; font-weight:500; margin:0;font-size:32px;font-family:'Rubik',sans-serif;\">" +

                $"You have requested to reset your password!" +
                $"</h1><span style=\"display:inline-block; vertical-align:middle; margin:29px 0 26px; border-bottom:1px solid #cecece; width:100px;\"></span><p style=\"color:#455056; font-size:15px;line-height:24px; margin:0;\">" +

                $"We cannot simply send you your old password. A unique link to reset your password has been generated for you. To reset your password, click the following link and follow the instructions." +
                $"</p><a href=\"{url}\"style=\"background:#f15a23;text-decoration:none !important; font-weight:500; margin-top:35px; color:#fff;text-transform:uppercase; font-size:14px;padding:10px 24px;display:inline-block;border-radius:50px;\">" +

                $"Confirm Email</a>" +
                $"</td></tr><tr><td style=\"height:40px;\">&nbsp;</td></tr></table></td><tr><td style=\"height:80px;\">&nbsp;</td></tr></table></td></tr></table></body></html>";

                await _mailService.SendEmailAsync(email, "Reset Password", template);
                res.Status = true;
                res.Message = "Reset password URL has been sent to the email sucessfully!";
                return res;
            }
            catch (Exception ex) { return new ResponseDto() { Message = ex.Message, Body = ex }; }
        }

        public async Task<ResponseDto> ResetPasswordAsync(ResetPassowrdDto model)
        {
            try 
            {
                ResponseDto res = new();
                User? user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    res.Message = "invalid email adress";
                    return res;
                }
                if (model.NewPassword != model.ConfirmPassword)
                {
                    res.Message = "Password doesn't match its confirmation";
                    return res;
                }
                var decodedToken = WebEncoders.Base64UrlDecode(model.Token);
                string normalToken = Encoding.UTF8.GetString(decodedToken);
                var result = await _userManager.ResetPasswordAsync(user, normalToken, model.NewPassword);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors) res.Message += $"{error.Description},";
                    return res;
                };
                res.Status = true;
                res.Message = "Password has been reset sucessfully!";
                return res;
            }
            catch (Exception ex) { return new ResponseDto() { Message = ex.Message, Body = ex }; }
        }

        public async Task<ResponseDto> ChangePasswordAsync(string id, UpdatePassowrdDto model)
        {
            try
            {
                ResponseDto res = new();
                if (id != model.id)
                {
                    res.Message = "invalid user id.";
                    return res;
                }
                if (model.NewPassword != model.ConfirmPassword)
                {
                    res.Message = "confirm password dont match password";
                    return res;
                }
                User? user = await _userManager.FindByIdAsync(model.id); 
                if (user == null)
                {
                    res.Message = "user not found";
                    return res;
                }
                var result = await _userManager.ChangePasswordAsync(user,model.OldPassword, model.NewPassword);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors) res.Message += $"{error.Description},";
                    return res;
                };
                res.Status = true;
                res.Message = "Password has been updated sucessfully!";
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var encodedToken = Encoding.UTF8.GetBytes(token);
                var validToken = WebEncoders.Base64UrlEncode(encodedToken);
                string url = $"{_configuration["AppUrl"]}/auth/resetPassword?email={user.Email}&token={validToken}";
                string template = $"<!doctype html><html lang=\"en-US\"><head><meta content=\"text/html; charset=utf-8\" http-equiv=\"Content-Type\" />" +
                    
                    $"<title>update Password Confirmation Email</title>" +
                    
                    $"<meta name=\"description\" content=\"Update Password.\"><style type=\"text/css\">a:hover{{text-decoration: underline !important;}}</style></head><body marginheight=\"0\" topmargin=\"0\" marginwidth=\"0\" style=\"margin: 0px; background-color: #f2f3f8;\" leftmargin=\"0\"><table cellspacing=\"0\" border=\"0\" cellpadding=\"0\" width=\"100%\" bgcolor=\"#f2f3f8\"style=\"@import url(https://fonts.googleapis.com/css?family=Rubik:300,400,500,700|Open+Sans:300,400,600,700); font-family: 'Open Sans', sans-serif;\"><tr><td><table style=\"background-color: #f2f3f8; max-width:670px;  margin:0 auto;\" width=\"100%\" border=\"0\"align=\"center\" cellpadding=\"0\" cellspacing=\"0\"><tr><td style=\"height:20px;\">&nbsp;</td></tr><tr><td><table width=\"95%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\"style=\"max-width:670px;background:#fff; border-radius:3px; text-align:center;-webkit-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);-moz-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);box-shadow:0 6px 18px 0 rgba(0,0,0,.06);\"><tr><td style=\"height:40px;\">&nbsp;</td></tr><tr><td style=\"padding:0 35px;\"><h1 style=\"color:#1e1e2d; font-weight:500; margin:0;font-size:32px;font-family:'Rubik',sans-serif;\">" +
                    
                    $"Your password has been updated</h1>" +
                    
                    $"<span style=\"display:inline-block; vertical-align:middle; margin:29px 0 26px; border-bottom:1px solid #cecece; width:100px;\"></span><p style=\"color:#455056; font-size:15px;line-height:24px; margin:0;\">" +
                    
                    $"if it wasn't you dont worry just click the following link to create new secure password, if not just ignore this email.</p>" +
                    
                    $"<a href=\"{url}\"style=\"background:#f15a23;text-decoration:none !important; font-weight:500; margin-top:35px; color:#fff;text-transform:uppercase; font-size:14px;padding:10px 24px;display:inline-block;border-radius:50px;\">" +
                    
                    $"Reset Password</a>" +
                    
                    $"</td></tr><tr><td style=\"height:40px;\">&nbsp;</td></tr></table></td><tr><td style=\"height:20px;\">&nbsp;</td></tr><tr><td style=\"text-align:center;\"><p style=\"font-size:14px; color:rgba(69, 80, 86, 0.7411764705882353); line-height:18px; margin:0 0 0;\">&copy; <strong>www.oceandro.com</strong></p></td></tr><tr><td style=\"height:80px;\">&nbsp;</td></tr></table></td></tr></table></body></html>";

                await _mailService.SendEmailAsync(user.Email, "Password has been updated", template);
                return res;
            }
            catch (Exception ex) { return new ResponseDto() { Message = ex.Message, Body = ex }; }
        }

        public async Task<ResponseDto> GetUserById(string id)
        {
            try
            {
                ResponseDto res = new();
                User? user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    res.Message = "invalid user id";
                    return res;
                }
                res.Body = mapper.Map<UserDto>(user);
                res.Status = true;
                return res;
            }
            catch (Exception ex) { return new ResponseDto() { Message = ex.Message, Body = ex }; }
        }

        private async Task<JwtSecurityToken> CreateJwtToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            foreach (var role in roles) roleClaims.Add(new Claim("roles", role));
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("uid", user.Id),
            }
            .Union(userClaims)
            .Union(roleClaims);
            if (user.Email != null) claims.Append(new Claim("Email", user.Email));
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            return new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: signingCredentials);
        }
    }
}
