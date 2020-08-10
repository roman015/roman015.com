using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class UrlShortenerContext : DbContext
{
    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }

    public UrlShortenerContext(DbContextOptions<UrlShortenerContext> options) :base(options)
    { }    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<ShortenedUrl>()
                .HasKey(b => b.UrlGuid)
                .HasName("PrimaryKey_ShortenedUrls"); ;

        modelBuilder.Entity<ShortenedUrl>()
               .Property(b => b.UrlGuid)
               .IsRequired()
               .ValueGeneratedOnAdd();

        modelBuilder.Entity<ShortenedUrl>()
                .Property(b => b.DestinaltionUrl)
                .IsRequired();

        modelBuilder.Entity<ShortenedUrl>()
                .Property(b => b.SourceUrl)
                .IsRequired();

        modelBuilder.Entity<ShortenedUrl>()
                .HasIndex(b => b.SourceUrl)
                .IsUnique();

        modelBuilder.Entity<ShortenedUrl>()
                .Property(b => b.Timestamp)
                .IsRequired();
    }
}

public class ShortenedUrl
{
    public Guid UrlGuid { get; set; }
    public string DestinaltionUrl { get; set; }
    public string SourceUrl { get; set; }
    public DateTime Timestamp { get; set; }
}