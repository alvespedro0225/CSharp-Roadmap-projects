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
    
    public async Task<IActionResult> Article(int id)
    {
        var article = await _context.Articles.Include(a => a.Author).
                                        FirstOrDefaultAsync(a => a.Id == id);
        if (article == null) return NotFound();
        return View(article);
    }
    
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(string title, string content)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity!.Name); 
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

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var article = await _context.Articles.FirstOrDefaultAsync(a => a.Id == id);
        if (article == null) return NotFound();
        return View(article);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Edit(int id , string title, string content)
    {
        var article = await _context.Articles.FirstOrDefaultAsync(a => a.Id == id);
        if (article == null) return NotFound();
        article.Title = title;
        article.Content = content;
        article.UpdatedDate = DateTime.Now;
        _context.Articles.Update(article);
        await _context.SaveChangesAsync();
        return RedirectToAction("Home", "Blog");
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var article = await _context.Articles.FirstOrDefaultAsync(a => a.Id == id);
        if (article == null) return NotFound();
        return View(article);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirm(int id)
    {
        var article = await _context.Articles.FirstOrDefaultAsync(a => a.Id == id);
        if (article == null) return NotFound();
        _context.Articles.Remove(article);
        await _context.SaveChangesAsync();
        return RedirectToAction("Home", "Blog");
    }
    
}