using ECommerce.Services.PhotoStock.Controllers;
using ECommerce.Services.PhotoStock.DTOs;
using ECommerce.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Services.PhotoStock.UnitTests.Controllers;

public class PhotoControllerTests
{
    private PhotoController _sut;

    public PhotoControllerTests()
    {
        _sut = new PhotoController();
    }
    
    #region PhotoSave

    [Test]
    public async Task PhotoSave_ShouldReturnFilePath_WhenFileCreated()
    {
        // Arrange
        
        //Setup mock file using a memory stream
        var content = "Test File";
        var fileName = "test.jpg";
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(content);
        writer.Flush();
        stream.Position = 0;
        
        //create FormFile with desired data
        IFormFile file = new FormFile(stream, 0, stream.Length, "test", fileName);
        
        _sut = new PhotoController();
        
        // Act
        var result =  (ObjectResult)(await _sut.PhotoSave(file, CancellationToken.None));

        // Assert
        Assert.NotNull(((Response<PhotoDTO>)result.Value).Data.Url);
    }

    [Test]
    public async Task PhotoSave_ShouldReturnFail_WhenFileNotCreated()
    {
        // Arrange
        _sut = new PhotoController();
        
        // Act
        var result = (ObjectResult)(await _sut.PhotoSave(null, CancellationToken.None));

        // Assert
        Assert.False(((Response<PhotoDTO>)result.Value).IsSuccessful);
    }

    #endregion

    #region PhotoDelete

    [Test]
    public void PhotoDelete_ShouldReturnIsSuccessfulTrue_WhenPhotoDeleted()
    {
        // Arrange
        _sut = new PhotoController();
        
        // Act
        var result = (ObjectResult)(_sut.PhotoDelete("test.jpg"));

        // Assert
        Assert.True(((Response<NoContent>)(result.Value)).IsSuccessful);
    }

    [Test]
    public void PhotoDelete_ShouldReturnIsSuccessFalse_WhenPhotoNotDeleted()
    {
        // Arrange
        _sut = new PhotoController();
        
        // Act
        var result = (ObjectResult)(_sut.PhotoDelete("test.jpg"));

        // Assert
        Assert.False(((Response<NoContent>)(result.Value)).IsSuccessful);
    }

    #endregion
}
