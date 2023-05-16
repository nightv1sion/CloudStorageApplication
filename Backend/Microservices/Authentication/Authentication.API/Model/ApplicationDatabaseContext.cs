﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authentication.API.Model;

public class ApplicationDatabaseContext : IdentityDbContext<User>
{
    public ApplicationDatabaseContext(
        DbContextOptions<ApplicationDatabaseContext> options) : base(options)
    {
        
    }
}