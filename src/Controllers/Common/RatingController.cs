﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sodoff.Model;
using sodoff.Schema;
using sodoff.Util;

namespace sodoff.Controllers.Common;

public class RatingController : Controller
{

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/Ratingwebservice.asmx/GetAverageRatingForRoom")]
    public IActionResult GetAverageRatingForRoom([FromForm] string apiToken, [FromForm] string request)
    {
        // TODO: This is a placeholder
        return Ok(5);
    }

}
