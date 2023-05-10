﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WheresNannyApi.Data.Context;

#nullable disable

namespace WheresNannyApi.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230507162108_HiringTimeService")]
    partial class HiringTimeService
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("WheresNannyApi.Domain.Entities.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Cep")
                        .IsRequired()
                        .HasColumnType("varchar(8)");

                    b.Property<string>("Complement")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("HouseNumber")
                        .HasColumnType("varchar(4)");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PersonId")
                        .IsUnique();

                    b.ToTable("Addresses", (string)null);
                });

            modelBuilder.Entity("WheresNannyApi.Domain.Entities.CommentRank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Comment")
                        .HasColumnType("varchar(50)");

                    b.Property<int>("NannyWhoRecieveTheCommentId")
                        .HasColumnType("int");

                    b.Property<int>("PersonWhoCommentId")
                        .HasColumnType("int");

                    b.Property<float>("RankStarsCounting")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("NannyWhoRecieveTheCommentId");

                    b.HasIndex("PersonWhoCommentId");

                    b.ToTable("CommentsRank", (string)null);
                });

            modelBuilder.Entity("WheresNannyApi.Domain.Entities.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("BirthdayDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Cellphone")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasColumnType("varchar(11)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Fullname")
                        .IsRequired()
                        .HasColumnType("varchar(150)");

                    b.Property<string>("ImageUri")
                        .IsRequired()
                        .HasColumnType("varchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Persons", (string)null);
                });

            modelBuilder.Entity("WheresNannyApi.Domain.Entities.Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("HiringDate")
                        .HasColumnType("datetime");

                    b.Property<int>("NannyId")
                        .HasColumnType("int");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal");

                    b.Property<TimeSpan>("ServiceFinishHour")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.HasIndex("NannyId");

                    b.HasIndex("PersonId");

                    b.ToTable("Services", (string)null);
                });

            modelBuilder.Entity("WheresNannyApi.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime?>("CreatedIn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ExpiresIn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Token")
                        .HasColumnType("varchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("WheresNannyApi.Domain.Entities.Nanny", b =>
                {
                    b.HasBaseType("WheresNannyApi.Domain.Entities.Person");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.Property<float>("RankAvegerageStars")
                        .HasColumnType("real");

                    b.Property<double>("ServicePrice")
                        .HasColumnType("float");

                    b.HasIndex("PersonId")
                        .IsUnique()
                        .HasFilter("[PersonId] IS NOT NULL");

                    b.ToTable("Nannys", (string)null);
                });

            modelBuilder.Entity("WheresNannyApi.Domain.Entities.Address", b =>
                {
                    b.HasOne("WheresNannyApi.Domain.Entities.Person", "Person")
                        .WithOne("Address")
                        .HasForeignKey("WheresNannyApi.Domain.Entities.Address", "PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("WheresNannyApi.Domain.Entities.CommentRank", b =>
                {
                    b.HasOne("WheresNannyApi.Domain.Entities.Nanny", "NannyWhoRecieveTheComment")
                        .WithMany("CommentsRankNanny")
                        .HasForeignKey("NannyWhoRecieveTheCommentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WheresNannyApi.Domain.Entities.Person", "PersonWhoComment")
                        .WithMany("CommentsRank")
                        .HasForeignKey("PersonWhoCommentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("NannyWhoRecieveTheComment");

                    b.Navigation("PersonWhoComment");
                });

            modelBuilder.Entity("WheresNannyApi.Domain.Entities.Person", b =>
                {
                    b.HasOne("WheresNannyApi.Domain.Entities.User", "User")
                        .WithOne("Person")
                        .HasForeignKey("WheresNannyApi.Domain.Entities.Person", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WheresNannyApi.Domain.Entities.Service", b =>
                {
                    b.HasOne("WheresNannyApi.Domain.Entities.Nanny", "NannyService")
                        .WithMany("ServicesNanny")
                        .HasForeignKey("NannyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WheresNannyApi.Domain.Entities.Person", "PersonService")
                        .WithMany("ServicesPerson")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("NannyService");

                    b.Navigation("PersonService");
                });

            modelBuilder.Entity("WheresNannyApi.Domain.Entities.Nanny", b =>
                {
                    b.HasOne("WheresNannyApi.Domain.Entities.Person", null)
                        .WithOne()
                        .HasForeignKey("WheresNannyApi.Domain.Entities.Nanny", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("WheresNannyApi.Domain.Entities.Person", "Person")
                        .WithOne("Nanny")
                        .HasForeignKey("WheresNannyApi.Domain.Entities.Nanny", "PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("WheresNannyApi.Domain.Entities.Person", b =>
                {
                    b.Navigation("Address");

                    b.Navigation("CommentsRank");

                    b.Navigation("Nanny");

                    b.Navigation("ServicesPerson");
                });

            modelBuilder.Entity("WheresNannyApi.Domain.Entities.User", b =>
                {
                    b.Navigation("Person");
                });

            modelBuilder.Entity("WheresNannyApi.Domain.Entities.Nanny", b =>
                {
                    b.Navigation("CommentsRankNanny");

                    b.Navigation("ServicesNanny");
                });
#pragma warning restore 612, 618
        }
    }
}
