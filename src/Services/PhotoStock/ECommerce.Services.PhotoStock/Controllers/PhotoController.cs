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
        
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos");

        if (!System.IO.Directory.Exists(path))
            System.IO.Directory.CreateDirectory(path);
        
        path = Path.Combine(path, photo.FileName);

        using var stream = new FileStream(path, FileMode.Create);
        await photo.CopyToAsync(stream, cancellationToken);

        var returnPath = $"photos/{photo.FileName}";

        PhotoDTO photoDto = new() { Url = returnPath };

        return CreateActionResultInstance(Response<PhotoDTO>.Success(photoDto, 200));
    }

    public IActionResult PhotoDelete(string photoUrl)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photoUrl);

        if (!System.IO.File.Exists(path))
            return CreateActionResultInstance(Response<NoContent>.Fail("Photo not found", 404));
            
        System.IO.File.Delete(path);

        return CreateActionResultInstance(Response<NoContent>.Success(204));
    }
}
