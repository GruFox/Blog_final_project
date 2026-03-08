using Blog_final_project.Data;
using Blog_final_project.Interfaces;
using Blog_final_project.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog_final_project.Repositoties;

public class TagRepository : ITagRepository
{
    private readonly BlogDbContext _context;

    public TagRepository(BlogDbContext context)
    {
        _context = context;
    }

    public async Task<List<Tag>> ShowTagsAsync()
    {
        return await _context.Tags
            .Include(t => t.Articles)
            .ToListAsync();
    }

    public async Task CreateTagAsync(string name)
    {
        var tag = new Tag { Name = name };

        _context.Tags.Add(tag);
        await _context.SaveChangesAsync();
    }
    public async Task<bool> TagExistsAsync(string name)
    {
        return await _context.Tags.AnyAsync(t => t.Name == name);
    }

    public async Task<List<Tag>> GetTagsByIdsAsync(List<int> selectedTagIds)
    {
        return await _context.Tags.Where(t => selectedTagIds.Contains(t.Id))
            .ToListAsync();
    }

    public async Task<Tag?> GetTagByIdAsync(int id)
    {
        return await _context.Tags
            .Include(t => t.Articles)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
