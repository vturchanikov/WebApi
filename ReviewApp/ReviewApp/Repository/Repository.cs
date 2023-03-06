using AutoMapper;
using ReviewApp.Data;

namespace ReviewApp.Repository;

public class Repository
{
    protected readonly DataContext _context;
    protected readonly IMapper _mapper;

    public Repository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();

        return saved > 0 ? true : false;
    }
}
