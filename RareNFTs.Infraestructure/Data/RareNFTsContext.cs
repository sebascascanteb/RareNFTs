using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RareNFTs.Infraestructure.Models;

namespace RareNFTs.Infraestructure.Data;

public partial class RareNFTsContext : DbContext
{
    public RareNFTsContext(DbContextOptions<RareNFTsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Card> Card { get; set; }

    public virtual DbSet<Client> Client { get; set; }

    public virtual DbSet<ClientNft> ClientNft { get; set; }

    public virtual DbSet<Country> Country { get; set; }

    public virtual DbSet<InvoiceDetail> InvoiceDetail { get; set; }

    public virtual DbSet<InvoiceHeader> InvoiceHeader { get; set; }

    public virtual DbSet<Nft> Nft { get; set; }

    public virtual DbSet<Role> Role { get; set; }

    public virtual DbSet<User> User { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Card>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Genre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IdCountry)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Surname)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCountryNavigation).WithMany(p => p.Client)
                .HasForeignKey(d => d.IdCountry)
                .HasConstraintName("FK_Client_Country");
        });

        modelBuilder.Entity<ClientNft>(entity =>
        {
            entity.HasKey(e => new { e.IdClient, e.IdNft });

            entity.ToTable("ClientNFT");

            entity.Property(e => e.IdNft).HasColumnName("IdNFT");
            entity.Property(e => e.Date).HasColumnType("datetime");

            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.ClientNft)
                .HasForeignKey(d => d.IdClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientNFT_Client");

            entity.HasOne(d => d.IdNftNavigation).WithMany(p => p.ClientNft)
                .HasForeignKey(d => d.IdNft)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientNFT_NFT");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.Property(e => e.Id)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<InvoiceDetail>(entity =>
        {
            entity.HasKey(e => new { e.IdInvoice, e.Sequence });

            entity.Property(e => e.IdNft).HasColumnName("IdNFT");
            entity.Property(e => e.Price).HasColumnType("money");

            entity.HasOne(d => d.IdInvoiceNavigation).WithMany(p => p.InvoiceDetail)
                .HasForeignKey(d => d.IdInvoice)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceDetail_InvoiceHeader");

            entity.HasOne(d => d.IdNftNavigation).WithMany(p => p.InvoiceDetail)
                .HasForeignKey(d => d.IdNft)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceDetail_NFT");
        });

        modelBuilder.Entity<InvoiceHeader>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.Total).HasColumnType("money");

            entity.HasOne(d => d.IdCardNavigation).WithMany(p => p.InvoiceHeader)
                .HasForeignKey(d => d.IdCard)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceHeader_Card");

            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.InvoiceHeader)
                .HasForeignKey(d => d.IdClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceHeader_Client");
        });

        modelBuilder.Entity<Nft>(entity =>
        {
            entity.ToTable("NFT");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Author)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("money");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.User)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
