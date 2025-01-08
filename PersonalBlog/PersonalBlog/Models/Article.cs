using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace PersonalBlog.Models;

public class Article
{
    public int Id { get; init; }
    [MaxLength(20)]
    [Required]
    public required string Title { get; set; }

    [MaxLength(200)]
    [Required]
    
    public required string Content { get; set; }

    public DateTime PublishDate { get; init; }
    public DateTime UpdatedDate { get; set; }
    [MaxLength(20)]
    public required string AuthorId { get; init; }
    [ForeignKey("AuthorId")]
    public required IdentityUser Author { get; init; }

    public override string ToString()
    {
        return 
            $"{Title}\n{Content}\n{PublishDate}\n{UpdatedDate}\n{Author}";
    }
}