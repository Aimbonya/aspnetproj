using Microsoft.AspNetCore.Mvc;

namespace _253501_mammadov.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly string _imagePath;

        public FilesController(IWebHostEnvironment webHost)
        {
            _imagePath = Path.Combine(webHost.WebRootPath, "Images");
        }

        [HttpPost]
        public async Task<IActionResult> SaveFile(IFormFile file)
        {
            if (file is null || file.Length == 0)
            {
                return BadRequest("File is empty or not provided.");
            }

            var filePath = Path.Combine(_imagePath, file.FileName);
            var fileInfo = new FileInfo(filePath);

            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }

            using (var fileStream = fileInfo.Create())
            {
                await file.CopyToAsync(fileStream);
            }
             
            var host = HttpContext.Request.Host;
            var fileUrl = $"https://{host}/Images/{file.FileName}";

            return Ok(new { Url = fileUrl });
        }

        [HttpDelete]
        public IActionResult DeleteFile(string fileName)
        {
            var filePath = Path.Combine(_imagePath, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            System.IO.File.Delete(filePath);
            return Ok("File deleted successfully.");
        }
    }
}

