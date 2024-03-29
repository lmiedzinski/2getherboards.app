﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TogetherBoardsApp.Backend.Infrastructure.Database.EntityFramework;

#nullable disable

namespace TogetherBoardsApp.Backend.Infrastructure.Database.EntityFramework.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TogetherBoardsApp.Backend.Domain.UserAccounts.UserAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("email");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("name");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)")
                        .HasColumnName("password_hash");

                    b.Property<uint>("version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("user_accounts", (string)null);
                });

            modelBuilder.Entity("TogetherBoardsApp.Backend.Infrastructure.OutboxPattern.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("json")
                        .HasColumnName("content");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at_utc");

                    b.Property<string>("Error")
                        .HasColumnType("text")
                        .HasColumnName("error");

                    b.Property<DateTime?>("ProcessedAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("processed_at_utc");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.HasKey("Id");

                    b.ToTable("outbox_messages", (string)null);
                });

            modelBuilder.Entity("TogetherBoardsApp.Backend.Domain.UserAccounts.UserAccount", b =>
                {
                    b.OwnsOne("TogetherBoardsApp.Backend.Domain.UserAccounts.UserAccountRefreshToken", "RefreshToken", b1 =>
                        {
                            b1.Property<Guid>("UserAccountId")
                                .HasColumnType("uuid");

                            b1.Property<DateTime>("ExpirationDateUtc")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("refresh_token_expiration_date_utc");

                            b1.Property<bool>("IsActive")
                                .HasColumnType("boolean")
                                .HasColumnName("refresh_token_is_active");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(1024)
                                .HasColumnType("character varying(1024)")
                                .HasColumnName("refresh_token_value");

                            b1.HasKey("UserAccountId");

                            b1.ToTable("user_accounts");

                            b1.WithOwner()
                                .HasForeignKey("UserAccountId");
                        });

                    b.Navigation("RefreshToken");
                });
#pragma warning restore 612, 618
        }
    }
}
