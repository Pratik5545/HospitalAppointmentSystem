using HospitalAppointmentSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAppointmentSystem.Controllers;

public class PostsController : Controller
{
    private readonly IHttpClientFactory _factory;

    public PostsController(IHttpClientFactory factory)
    {
        _factory = factory;
    }

    // 2. Asynchronous Web Request: Retrieve posts asynchronously from jsonplaceholder
    public async Task<IActionResult> Index()
    {
        var client = _factory.CreateClient();
        try
        {
            var posts = await client.GetFromJsonAsync<List<Post>>("https://jsonplaceholder.typicode.com/posts");
            return View(posts ?? new List<Post>());
        }
        catch (HttpRequestException ex)
        {
            ViewBag.Error = $"Could not fetch posts: {ex.Message}. Please check your internet connection.";
            return View(new List<Post>());
        }
    }
}
