﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fortune_Teller_UI.Services;

namespace Fortune_Teller_UI.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        IFortuneService _fortunes;

        public HomeController(IFortuneService fortunes)
        {
            _fortunes = fortunes;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("buzzgraph")]
        public async Task<string> getGraph()
        {
            return await _fortunes.GetBuzzgraphAsync();
        }

        [HttpPost("shuffle")]
        public string shuffle([FromBody] Mention req)
        {
            return _fortunes.shuffle(req);
        }
    }
}
