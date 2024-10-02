using Core.BasicRoles;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace Infrastucture;

public class ApplicationDbContext: IdentityDbContext<User,IdentityRole<Guid>,Guid>
{
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
        //todo napishi migration :)
        //potom mb, poka norm.
        Database.EnsureCreated();

        
    }
}