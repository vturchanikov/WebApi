using AutoMapper;
using ReviewApp.Data;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Repository;

public class OwnerRepository : Repository, IOwnerRepository
{
    public OwnerRepository(DataContext context, IMapper mapper) : base(context, mapper)
    {

    }

    public Owner GetOwner(int ownerId)
    {
        return _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
    }

    public ICollection<Owner> GetOwnerOfAPokemon(int pokeId)
    {
        return _context.PokemonOwners.Where(p => p.Pokemon.Id == pokeId).Select(o => o.Owner).ToList();
    }

    public ICollection<Owner> GetOwners()
    {
        return _context.Owners.ToList();
    }

    public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
    {
        return _context.PokemonOwners.Where(p => p.Owner.Id == ownerId).Select(p => p.Pokemon).ToList();
    }

    public bool OwnerExists(int ownerId)
    {
        return _context.Owners.Any(o => o.Id == ownerId);
    }

    public bool CreateOwner(Owner owner)
    {
        _context.Add(owner);

        return Save();
    }

    public bool UpdateOwner(Owner owner)
    {
        _context.Update(owner);

        return Save();
    }

    public bool DeleteOwner(Owner owner)
    {
        _context.Remove(owner);

        return Save();
    }
}
