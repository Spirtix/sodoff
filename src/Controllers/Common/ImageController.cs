using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sodoff.Attributes;
using sodoff.Model;
using sodoff.Schema;
using sodoff.Services;
using sodoff.Util;
using System;

namespace sodoff.Controllers.Common;
public class ImageController : Controller {

    private readonly DBContext ctx;
    private KeyValueService keyValueService;
    public ImageController(DBContext ctx, KeyValueService keyValueService) {
        this.ctx = ctx;
        this.keyValueService = keyValueService;
    }

    // SetImage and GetImage are defined in ContentController

    [HttpGet]
    [Route("RawImage/{VikingId}/{ImageType}/{ImageSlot}.jpg")]
    public IActionResult RawImage(string VikingId, string ImageType, int ImageSlot) {
        Image? image = ctx.Images.FirstOrDefault(e => e.Viking.Uid == Guid.Parse(VikingId) && e.ImageType == ImageType && e.ImageSlot == ImageSlot);
        if (image is null || image.ImageData is null) {
            return NotFound();
        }

        byte[] imageBytes = Convert.FromBase64String(image.ImageData);
        var imageStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
        return File(imageStream, "image/jpeg");
    }
}
