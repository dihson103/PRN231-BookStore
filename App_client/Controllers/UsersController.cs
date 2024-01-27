﻿using App_client.Dtos.Common;
using App_client.Dtos.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace App_client.Controllers
{
    [Authorize(Policy = "Admin")]
    public class UsersController : Controller
    {
        private static string userBaseUrl = "https://localhost:7070/api/Users";
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

                        return View(userResponses);
                    }

                    string errorJson = await response.Content.ReadAsStringAsync();
                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorJson);

                    return View(new { Message = errorResponse.Message });
                }
            }
        }
    }
}