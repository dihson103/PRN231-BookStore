using App_client.Dtos.Common;
using App_client.Dtos.Publishers;
using App_client.Dtos.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Security.Claims;

namespace App_client.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private static string userBaseUrl = "https://localhost:7070/api/Users";
        private static string publisherBaseUrl = "https://localhost:7070/api/Publishers";
        public async Task<IActionResult> Index()
        {
            var user = HttpContext.User;
            var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(userBaseUrl + $"/{id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string responseJson = await response.Content.ReadAsStringAsync();
                        var successResponse = JsonConvert.DeserializeObject<SuccessResponse<UserResponse>>(responseJson);
                        var userResponse = successResponse.Data as UserResponse;

                        var publishers = getPublishers();
                        ViewData["PubId"] = new SelectList(publishers.Result, "PubId", "PublisherName");

                        return View(userResponse);
                    }

                    string errorJson = await response.Content.ReadAsStringAsync();
                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorJson);

                    return View(new { Message = errorResponse.Message });
                }
            }
        }

        private async Task<List<PublisherResponse>> getPublishers()
        {
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(publisherBaseUrl))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string responseJson = await response.Content.ReadAsStringAsync();
                        var successResponse = JsonConvert.DeserializeObject<SuccessResponse<List<PublisherResponse>>>(responseJson);

                        return successResponse.Data;
                    }
                    return null;
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, string? firstName, string? lastName, string? middleName, string? source, 
            int? pubId, string email, DateTime? hireDate)
        {
            var user = new UserUpdateProfile()
            {
                UserId = id,
                EmailAddress = email,
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                Source = source,
                PubId = pubId,
                HireDate = hireDate
            };

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.PutAsJsonAsync(userBaseUrl + "/" + id, user);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Profile", new { Message = "Update user success." });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Profile", new { Message = "Failed to update user." });
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Profile", new { Message = "An error occurred while processing your request." });
            }
        }
    }
}
