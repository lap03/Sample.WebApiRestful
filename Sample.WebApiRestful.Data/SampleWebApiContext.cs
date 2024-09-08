﻿using Microsoft.EntityFrameworkCore;
using Sample.WebApiRestful.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.WebApiRestful.Data
{
    public class SampleWebApiContext : DbContext
    {
        public SampleWebApiContext(DbContextOptions<SampleWebApiContext> options) : base (options) { }

        public DbSet<Categories> Categories { get; set; }
        public DbSet<Products> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}