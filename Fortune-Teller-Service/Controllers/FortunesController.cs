﻿
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using FortuneTellerService.Models;

namespace FortuneTellerService.Controllers
{
    [Route("api/[controller]")]
    public class FortunesController : Controller
    {
        private IFortuneRepository _fortunes;

        public FortunesController(IFortuneRepository fortunes)
        {
            _fortunes = fortunes;
        }

        // GET: api/fortunes
        [HttpGet]
        public IEnumerable<Fortune> Get()
        {
            return _fortunes.GetAll();
        }

        // GET api/fortunes/random
        [HttpGet("random")]
        public Fortune Random()
        {
            return _fortunes.RandomFortune();
        }
    }
}
