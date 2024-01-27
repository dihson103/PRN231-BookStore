using App_client.Dtos.Authors;
using App_client.Dtos.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace App_client.Controllers
{
    [Authorize(Policy = "Admin")]
    public class AuthorsController : Controller
    {
        private static string authorBaseUrl = "https://localhost:7070/api/Authors";
        public async Task<IActionResult> Index()
        {
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(authorBaseUrl))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string responseJson = await response.Content.ReadAsStringAsync();
                        var successResponse = JsonConvert.DeserializeObject<SuccessResponse<List<AuthorResponse>>>(responseJson);
                        var authorResponses = successResponse.Data as List<AuthorResponse>;

                        return View(authorResponses);
                    }

                    string errorJson = await response.Content.ReadAsStringAsync();
                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorJson);
                    ModelState.AddModelError("", errorResponse.Message);

                    return View(new List<AuthorResponse>());
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(string firstname, string lastname,
                                      string? phone, string? address,
                                      string? city, string? state,
                                      int? zip, string email)
        {
            var author = new AuthorCreateRequest()
            {
                FirstName = firstname,
                LastName = lastname,
                Phone = phone,
                Address = address,
                City = city,
                State = state,
                Zip = zip,
                EmailAddress = email
            };

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsJsonAsync(authorBaseUrl, author);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Authors");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to add author.");
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception for troubleshooting
                ModelState.AddModelError("", "An error occurred while processing your request.");
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(int authorId, string firstname, string lastname,
                                      string? phone, string? address,
                                      string? city, string? state,
                                      int? zip, string email)
        {
            var author = new AuthorUpdateRequest()
            {
                AuthorId= authorId,
                FirstName = firstname,
                LastName = lastname,
                Phone = phone,
                Address = address,
                City = city,
                State = state,
                Zip = zip,
                EmailAddress = email
            };

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.PutAsJsonAsync(authorBaseUrl + "/" + authorId, author);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Authors");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to update author.");
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception for troubleshooting
                ModelState.AddModelError("", "An error occurred while processing your request.");
                return View();
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            using (var client = new HttpClient())
            {
                var response = await client.DeleteAsync(authorBaseUrl + "/" + id);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Authors");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to delete author.");
                    return View();
                }
            }
        }

    }
}
