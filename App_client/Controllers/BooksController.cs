using App_client.Dtos.Books;
using App_client.Dtos.Common;
using App_client.Dtos.Publishers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace App_client.Controllers
{
    [Authorize(Policy = "Admin")]
    public class BooksController : Controller
    {
        private static string publisherBaseUrl = "https://localhost:7070/api/Publishers";
        private static string bookBaseUrl = "https://localhost:7070/api/Books";

        private bool isUsedSearch(string url)
        {
            return url.Contains("?$");
        }

        public async Task<IActionResult> Index(string? searchName, double? searchPrice)
        {
            string url = bookBaseUrl + "/search";

            if (!string.IsNullOrEmpty(searchName))
            {
                string encodedSearchName = Uri.EscapeDataString(searchName);

                url += $"?$filter=contains(title, '{encodedSearchName}')";
            }

            if (searchPrice != null)
            {
                if (isUsedSearch(url))
                {
                    url += $" and ytdSales lt {searchPrice}";
                }
                else
                {
                    url += $"?$filter=ytdSales lt {searchPrice}";
                }
            }

            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string responseJson = await response.Content.ReadAsStringAsync();
                        var successResponse = JsonConvert.DeserializeObject<List<BookResponse>>(responseJson);
                        var userResponses = successResponse as List<BookResponse>;

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
        public async Task<IActionResult> Add(string title, string? advance, double? royalty, double ytdSales, 
            DateTime? publishedDate, int? publisher, string? notes)
        {
            var book = new BookCreateRequest()
            {
                Title = title,
                Advance = advance,
                Royalty = royalty,
                YtdSales = ytdSales,
                PublishedDate = publishedDate,
                PubId = publisher,
                Notes = notes
            };

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsJsonAsync(bookBaseUrl, book);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Books");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Books", new { Message = "Failed to add book." });
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Authors", new { Message = "An error occurred while processing your request." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, string title, string? advance, double? royalty, double ytdSales,
            DateTime? publishedDate, int? publisher, string? notes)
        {
            var book = new BookUpdateRequest()
            {
                BookId= id,
                Title = title,
                Advance = advance,
                Royalty = royalty,
                YtdSales = ytdSales,
                PublishedDate = publishedDate,
                PubId = publisher,
                Notes = notes
            };

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.PutAsJsonAsync(bookBaseUrl + "/" + id, book);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Books");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Books", new { Message = "Failed to update book." });
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Authors", new { Message = "An error occurred while processing your request." });
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            using (var client = new HttpClient())
            {
                var response = await client.DeleteAsync(bookBaseUrl + "/" + id);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Books");
                }
                else
                {
                    return RedirectToAction("Index", "Books", new { Message = "Failed to delete book." });
                }
            }
        }
    }
}
