using App_client.Dtos.Common;
using App_client.Dtos.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace App_client.Controllers
{
    public class AuthController : Controller
    {
        private static string baseAuthUrl = "https://localhost:7070/api/Auth";
        public IActionResult Index()
        {
            var claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return RedirectToAction("Login", new { Message = "Please input both email and password!" });
            }

            var authRequest = new UserAuthRequest
            {
                EmailAddress = email,
                Password = password
            };

            using (var client = new HttpClient())
            {
                try
                {
                    using (var response = await client.PostAsJsonAsync(baseAuthUrl + "/login", authRequest))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var responseJson = await response.Content.ReadAsStringAsync();
                            var successResponse = JsonConvert.DeserializeObject<SuccessResponse<UserResponse>>(responseJson);

                            var user = successResponse.Data;

                            // Set authentication cookie
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                                new Claim(ClaimTypes.Name, user.FullName),
                                new Claim("Role", user.RoleId.ToString())
                                // Add other claims as needed
                            };

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                            var authProperties = new AuthenticationProperties
                            {
                                AllowRefresh = true,
                                IsPersistent = true
                            };

                            await HttpContext.SignInAsync(
                                CookieAuthenticationDefaults.AuthenticationScheme,
                                new ClaimsPrincipal(claimsIdentity),
                                authProperties
                            );

                            return RedirectToAction("Index", "Home");
                        }

                        var errorResponseJson = await response.Content.ReadAsStringAsync();
                        var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorResponseJson);
                        return RedirectToAction("Login", new { Message = errorResponse.Message });
                    }
                }
                catch (HttpRequestException)
                {
                    return RedirectToAction("Login", new { Message = "Error connecting to service." });
                }
                catch (JsonException)
                {
                    return RedirectToAction("Login", new { Message = "Error parsing the service response." });
                }
            }
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(string email, string password, string confirmPassword)
        {
            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return RedirectToAction("Register", new { Message = "Please input both email and password" });
            }

            if(password != confirmPassword)
            {
                return RedirectToAction("Register", new { Message = "Password and confirm password must be the same" });
            }

            var authRequest = new UserAuthRequest
            {
                EmailAddress = email,
                Password = password
            };

            using (var client = new HttpClient())
            {
                try
                {
                    using (var response = await client.PostAsJsonAsync(baseAuthUrl + "/register", authRequest))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var responseJson = await response.Content.ReadAsStringAsync();
                            var successResponse = JsonConvert.DeserializeObject<SuccessResponse<object>>(responseJson);
                            return RedirectToAction("Login", new {Message = successResponse.Message});
                        }

                        var errorResponseJson = await response.Content.ReadAsStringAsync();
                        var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorResponseJson);
                        return RedirectToAction("Register", new { Message = errorResponse.Message });
                    }
                }
                catch (HttpRequestException)
                {
                    return RedirectToAction("Register", new { Message = "Error connecting to service." });
                }
                catch (JsonException)
                {
                    return RedirectToAction("Register", new { Message = "Error parsing the service response." });
                }
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Forbidden()
        {
            return View();
        }
    }
}
