﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App_client.Controllers
{
    [Authorize(Policy = "Admin")]
    public class PublishersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
