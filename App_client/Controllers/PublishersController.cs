using App_client.Dtos.Common;
using App_client.Dtos.Publishers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace App_client.Controllers
{
    [Authorize(Policy = "Admin")]
    public class PublishersController : Controller
    {
        private static string publisherBaseUrl = "https://localhost:7070/api/Publishers";
        public async Task<IActionResult> Index()
        {
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(publisherBaseUrl))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string responseJson = await response.Content.ReadAsStringAsync();
                        var successResponse = JsonConvert.DeserializeObject<SuccessResponse<List<PublisherResponse>>>(responseJson);
                        var publisherResponses = successResponse.Data as List<PublisherResponse>;

                        return View(publisherResponses);
                    }

                    string errorJson = await response.Content.ReadAsStringAsync();
                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorJson);
                    ModelState.AddModelError("", errorResponse.Message);

                    return View(new List<PublisherResponse>());
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(string publisherName, string city, string? state, string? country) 
        {
            var publisher = new PublisherCreateRequest()
            {
                PublisherName = publisherName,
                City = city,
                State = state,
                Country = country
            };

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsJsonAsync(publisherBaseUrl, publisher);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Publishers");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to add publisher.");
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
        public async Task<IActionResult> Update(int publisherId, string publisherName, string city, string? state, string? country)
        {
            var publisher = new PublisherUpdateRequest()
            {
                PubId= publisherId,
                PublisherName = publisherName,
                City = city,
                State = state,
                Country = country
            };

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.PutAsJsonAsync(publisherBaseUrl + "/" + publisherId, publisher);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Publishers");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to update publisher.");
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
                var response = await client.DeleteAsync(publisherBaseUrl + "/" + id);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Publishers");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to delete publisher.");
                    return View();
                }
            }
        }
    }
}
