using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastucture;

public class ApplicationDbContext: IdentityDbContext<User,IdentityRole<Guid>,Guid>
{
    public ApplicationDbContext(DbContextOptions options)
        : base(options){
        Database.EnsureCreated();
    }
}