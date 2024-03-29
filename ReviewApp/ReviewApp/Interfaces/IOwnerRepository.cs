﻿using ReviewApp.Models;

namespace ReviewApp.Interfaces;

public interface IOwnerRepository
{
    ICollection<Owner> GetOwners();

    Owner GetOwner(int orderID);
    ICollection<Owner> GetOwnerOfAPokemon(int pokeId);
    ICollection<Pokemon> GetPokemonByOwner(int ownerId);
    bool OwnerExists(int ownerId);

    bool CreateOwner(Owner owner);
    bool UpdateOwner(Owner owner);
    bool DeleteOwner(Owner owner);

    bool Save();
}
