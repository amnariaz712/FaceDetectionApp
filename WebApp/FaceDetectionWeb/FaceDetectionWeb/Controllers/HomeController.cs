using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FaceDetectionWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Detect(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { error = "No image received." });
            }

            var client = _httpClientFactory.CreateClient("PythonApi");

            using var content = new MultipartFormDataContent();

            using var stream = file.OpenReadStream();

            using var fileContent = new StreamContent(stream);

            fileContent.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue(
                    file.ContentType ?? "image/jpeg"
                );

            content.Add(fileContent, "file", file.FileName);

            var response = await client.PostAsync("detect", content);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode(
                    (int)response.StatusCode,
                    new { error = "Python face detection failed." }
                );
            }

            var json = await response.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(json);

            return Json(document.RootElement.Clone());
        }
    }
}