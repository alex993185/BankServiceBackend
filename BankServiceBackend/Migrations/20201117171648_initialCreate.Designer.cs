﻿// <auto-generated />
using System;
using BankServiceBackend.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BankServiceBackend.Migrations
{
    [DbContext(typeof(PostgresDbContext))]
    [Migration("20201117171648_initialCreate")]
    partial class initialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("AccountUser", b =>
                {
                    b.Property<long>("AccountsAccountNumber")
                        .HasColumnType("bigint");

                    b.Property<long>("UsersCustomerNumber")
                        .HasColumnType("bigint");

                    b.HasKey("AccountsAccountNumber", "UsersCustomerNumber");

                    b.HasIndex("UsersCustomerNumber");

                    b.ToTable("AccountUser");
                });

            modelBuilder.Entity("BankServiceBackend.Persistance.Entities.Account", b =>
                {
                    b.Property<long>("AccountNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityByDefaultColumn();

                    b.Property<double>("Credit")
                        .HasColumnType("double precision");

                    b.Property<double>("Dispo")
                        .HasColumnType("double precision");

                    b.Property<string>("HashedPin")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("AccountNumber");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("BankServiceBackend.Persistance.Entities.Transaction", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityByDefaultColumn();

                    b.Property<long?>("AccountNumber")
                        .HasColumnType("bigint");

                    b.Property<double>("AmountInEuro")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AccountNumber");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("BankServiceBackend.Persistance.Entities.TransferTransaction", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityByDefaultColumn();

                    b.Property<double>("AmountInEuro")
                        .HasColumnType("double precision");

                    b.Property<long?>("SourceAccountAccountNumber")
                        .HasColumnType("bigint");

                    b.Property<long?>("TargetAccountAccountNumber")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("SourceAccountAccountNumber");

                    b.HasIndex("TargetAccountAccountNumber");

                    b.ToTable("TransferTransactions");
                });

            modelBuilder.Entity("BankServiceBackend.Persistance.Entities.User", b =>
                {
                    b.Property<long>("CustomerNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityByDefaultColumn();

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("Gender")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("CustomerNumber");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AccountUser", b =>
                {
                    b.HasOne("BankServiceBackend.Persistance.Entities.Account", null)
                        .WithMany()
                        .HasForeignKey("AccountsAccountNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BankServiceBackend.Persistance.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersCustomerNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BankServiceBackend.Persistance.Entities.Transaction", b =>
                {
                    b.HasOne("BankServiceBackend.Persistance.Entities.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountNumber");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("BankServiceBackend.Persistance.Entities.TransferTransaction", b =>
                {
                    b.HasOne("BankServiceBackend.Persistance.Entities.Account", "SourceAccount")
                        .WithMany()
                        .HasForeignKey("SourceAccountAccountNumber");

                    b.HasOne("BankServiceBackend.Persistance.Entities.Account", "TargetAccount")
                        .WithMany()
                        .HasForeignKey("TargetAccountAccountNumber");

                    b.Navigation("SourceAccount");

                    b.Navigation("TargetAccount");
                });
#pragma warning restore 612, 618
        }
    }
}
