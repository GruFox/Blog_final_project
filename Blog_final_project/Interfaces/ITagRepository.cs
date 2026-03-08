using Blog_final_project.Models;

namespace Blog_final_project.Interfaces;

public interface ITagRepository
{
    Task<List<Tag>> ShowTagsAsync();
    Task CreateTagAsync(string name);
    Task<bool> TagExistsAsync(string name);
    Task<List<Tag>> GetTagsByIdsAsync(List<int> selectedTagIds);
    Task<Tag?> GetTagByIdAsync(int id);
    Task SaveAsync();
}
