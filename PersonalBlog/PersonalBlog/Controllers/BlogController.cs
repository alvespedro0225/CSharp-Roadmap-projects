using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalBlog.Data;
using PersonalBlog.Models;

namespace PersonalBlog.Controllers;

public class BlogController(MyAppContext context) : Controller
{
    private readonly MyAppContext _context = context;

    // GET
    public async Task<IActionResult> Home()
    {
        var articles = await _context.Articles.Include(a => a.Author).
                                                            ToListAsync();
        return View(articles);
    }
    [Authorize(Roles = "User, Admin")]
    public IActionResult Create()
    {
        return View();
    }

    [Authorize(Roles = "User, Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(string title, string content)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity!.Name); 
        var article = new Article
        {
            Title = title,
            Content = content,
            PublishDate = DateTime.Now,
            UpdatedDate = DateTime.Now,
            AuthorId = user!.Id,
            Author = user
        };
        Console.WriteLine(article.ToString() + '\n' + '\n');
        if (!ModelState.IsValid)
            return View(article);
        
        _context.Articles.Add(article);
        await _context.SaveChangesAsync();
        return RedirectToAction("Home", "Blog");
    }

    
}