using AutoMapper;
using ReviewApp.Data;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Repository;

public class CategoryRepository : Repository, ICategoryRepository
{
    public CategoryRepository(DataContext context, IMapper mapper) : base(context, mapper)
    {

    }

    public bool CategoryExists(int id)
    {
        return _context.Categories.Any(c => c.Id == id);
    }

    public ICollection<Category> GetCategories()
    {
        return _context.Categories.ToList();
    }

    public Category GetCategory(int id)
    {
        return _context.Categories.Where(e => e.Id == id).FirstOrDefault();
    }

    public ICollection<Pokemon> GetPokemonByCategory(int categoryId)
    {
        return _context.PokemonCategories.Where(e => e.CategoryId == categoryId).Select(c => c.Pokemon).ToList();
    }

    public bool CreateCategory(Category category)
    {
        _context.Add(category);

        return Save();
    }

    public bool UpdateCategory(Category category)
    {
        _context.Update(category);

        return Save();
    }

    public bool DeleteCategory(Category categotry)
    {
        _context.Remove(categotry);

        return Save();
    }
}
