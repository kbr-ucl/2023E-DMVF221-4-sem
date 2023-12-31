﻿using Microsoft.EntityFrameworkCore;
using UserService.Entities;

namespace UserService.Data;

public class PostServiceContext : DbContext
{
    public PostServiceContext(DbContextOptions<PostServiceContext> options)
        : base(options)
    {
    }

    public DbSet<User> User { get; set; }
    public DbSet<Post> Post { get; set; }
}