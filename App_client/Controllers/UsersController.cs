using App_client.Dtos.Common;
using App_client.Dtos.Publishers;
using App_client.Dtos.Roles;
using App_client.Dtos.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace App_client.Controllers
{
    [Authorize(Policy = "Admin")]
    public class UsersController : Controller
    {
        private static string userBaseUrl = "https://localhost:7070/api/Users";
        private static string roleBaseUrl = "https://localhost:7070/api/Roles";
        private static string publisherBaseUrl = "https://localhost:7070/api/Publishers";
        public async Task<IActionResult> Index()
        {
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(userBaseUrl))
                {
                    if(response.IsSuccessStatusCode)
                    {
                        string responseJson = await response.Content.ReadAsStringAsync();
                        var successResponse = JsonConvert.DeserializeObject<SuccessResponse<List<UserResponse>>>(responseJson);
                        var userResponses = successResponse.Data as List<UserResponse>;

                        var roles = getRoles();
                        ViewData["RoleId"] = new SelectList(roles.Result, "RoleId", "RoleDesc");

                        var publishers = getPublishers();
                        ViewData["PubId"] = new SelectList(publishers.Result, "PubId", "PublisherName");

                        return View(userResponses);
                    }

                    string errorJson = await response.Content.ReadAsStringAsync();
                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorJson);

                    return View(new { Message = errorResponse.Message });
                }
            }
        }

        private async Task<List<RoleResponse>> getRoles()
        {
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(roleBaseUrl))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string responseJson = await response.Content.ReadAsStringAsync();
                        var successResponse = JsonConvert.DeserializeObject<SuccessResponse<List<RoleResponse>>>(responseJson);

                        return successResponse.Data;
                    }
                    return null;
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
        public async Task<IActionResult> Add(string email, string password, string? firstname, 
            string? middlename, string? lastName, string? source, int role, int publisherId, DateTime? hireDate)
        {
            var user = new UserAdminCreateRequest()
            {
                EmailAddress= email,
                Password= password,
                FirstName= firstname,
                MiddleName= middlename,
                LastName= lastName,
                Source= source,
                RoleId=role,
                PubId=publisherId,
                HireDate=hireDate
            };

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsJsonAsync(userBaseUrl, user);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Users");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Users", new { Message = "Failed to add user." });
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Users", new { Message = "An error occurred while processing your request." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, string email, string password, string? firstname,
            string? middlename, string? lastName, string? source, int publisherId, DateTime hireDate)
        {
            var user = new UserUpdateProfile()
            {
                UserId= id,
                EmailAddress = email,
                FirstName = firstname,
                MiddleName = middlename,
                LastName = lastName,
                Source = source,
                PubId = publisherId,
                HireDate = hireDate
            };

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.PutAsJsonAsync(userBaseUrl + "/" + id, user);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Users");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Users", new { Message = "Failed to update user." });
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Users", new { Message = "An error occurred while processing your request." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(int userId, int roleId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PatchAsync($"{userBaseUrl}/{userId}/change-user-role/{roleId}", null);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Users");
                }
                else
                {
                    return RedirectToAction("Index", "Users", new { Message = "Failed to update role." });
                }
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            using (var client = new HttpClient())
            {
                var response = await client.DeleteAsync(userBaseUrl + "/" + id);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Users");
                }
                else
                {
                    return RedirectToAction("Index", "Users", new { Message = "Failed to delete user." });
                }
            }
        }
    }
}
