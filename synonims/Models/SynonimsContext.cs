using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using synonims.Models;

#nullable disable

namespace synonims.Models
{
    public partial class SynonimsContext : DbContext
    {
        public SynonimsContext()
        {
        }

        public SynonimsContext(DbContextOptions<SynonimsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Word> Words { get; set; }
        public virtual DbSet<Match> Matches { get; set; }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
