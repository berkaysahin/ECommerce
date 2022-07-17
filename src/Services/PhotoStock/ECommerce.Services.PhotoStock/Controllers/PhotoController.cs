using ECommerce.Services.PhotoStock.DTOs;
using ECommerce.Shared.ControllerBases;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Shared.DTOs;

namespace ECommerce.Services.PhotoStock.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PhotoController : CustomBaseController
{
    [HttpPost]
    public async Task<IActionResult> PhotoSave(IFormFile photo, CancellationToken cancellationToken)
    {
        if (photo is null || photo.Length < 1)
            return CreateActionResultInstance(Response<PhotoDTO>.Fail("Photo is empty", 400));

        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photo", photo.FileName);

        using var stream = new FileStream(path, FileMode.Create);
        await photo.CopyToAsync(stream, cancellationToken);

        var returnPath = $"photo/{photo.FileName}";

        PhotoDTO photoDto = new() { Url = returnPath };

        return CreateActionResultInstance(Response<PhotoDTO>.Success(photoDto, 200));
    }
}
