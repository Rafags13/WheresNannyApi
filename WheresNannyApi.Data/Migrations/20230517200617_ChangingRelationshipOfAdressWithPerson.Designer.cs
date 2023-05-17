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
    [Migration("20230517200617_ChangingRelationshipOfAdressWithPerson")]
    partial class ChangingRelationshipOfAdressWithPerson
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

                    b.HasKey("Id");

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

                    b.Property<int?>("NannyWhoRecieveTheCommentId")
                        .HasColumnType("int");

                    b.Property<int?>("PersonWhoCommentId")
                        .HasColumnType("int");

                    b.Property<float>("RankStarsCounting")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("NannyWhoRecieveTheCommentId");

                    b.HasIndex("PersonWhoCommentId");

                    b.ToTable("CommentsRank", (string)null);
                });

            modelBuilder.Entity("WheresNannyApi.Domain.Entities.Document", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("DocumentTypeId")
                        .HasColumnType("int");

                    b.Property<string>("FileInBase64")
                        .IsRequired()
                        .HasColumnType("varchar(max)");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DocumentTypeId");

                    b.HasIndex("PersonId");

                    b.ToTable("Documents", (string)null);
                });

            modelBuilder.Entity("WheresNannyApi.Domain.Entities.DocumentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(250)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("DocumentTypes", (string)null);
                });

            modelBuilder.Entity("WheresNannyApi.Domain.Entities.Nanny", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("ApprovedToWork")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.Property<double>("ServicePrice")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("PersonId")
                        .IsUnique();

                    b.ToTable("Nannys", (string)null);
                });

            modelBuilder.Entity("WheresNannyApi.Domain.Entities.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

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

                    b.HasIndex("AddressId");

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

            modelBuilder.Entity("WheresNannyApi.Domain.Entities.CommentRank", b =>
                {
                    b.HasOne("WheresNannyApi.Domain.Entities.Nanny", "NannyWhoRecieveTheComment")
                        .WithMany("CommentsRankNanny")
                        .HasForeignKey("NannyWhoRecieveTheCommentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("WheresNannyApi.Domain.Entities.Person", "PersonWhoComment")
                        .WithMany("CommentsRank")
                        .HasForeignKey("PersonWhoCommentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("NannyWhoRecieveTheComment");

                    b.Navigation("PersonWhoComment");
                });

            modelBuilder.Entity("WheresNannyApi.Domain.Entities.Document", b =>
                {
                    b.HasOne("WheresNannyApi.Domain.Entities.DocumentType", "TypeFromDocument")
                        .WithMany("DocumentsWhoHaveThisDocumentType")
                        .HasForeignKey("DocumentTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WheresNannyApi.Domain.Entities.Person", "PersonDocumentOwner")
                        .WithMany("PersonDocuments")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PersonDocumentOwner");

                    b.Navigation("TypeFromDocument");
                });

            modelBuilder.Entity("WheresNannyApi.Domain.Entities.Nanny", b =>
                {
                    b.HasOne("WheresNannyApi.Domain.Entities.Person", "Person")
                        .WithOne("Nanny")
                        .HasForeignKey("WheresNannyApi.Domain.Entities.Nanny", "PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("WheresNannyApi.Domain.Entities.Person", b =>
                {
                    b.HasOne("WheresNannyApi.Domain.Entities.Address", "Address")
                        .WithMany("PersonsWhoHasThisAddress")
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WheresNannyApi.Domain.Entities.User", "User")
                        .WithOne("Person")
                        .HasForeignKey("WheresNannyApi.Domain.Entities.Person", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

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

            modelBuilder.Entity("WheresNannyApi.Domain.Entities.Address", b =>
                {
                    b.Navigation("PersonsWhoHasThisAddress");
                });

            modelBuilder.Entity("WheresNannyApi.Domain.Entities.DocumentType", b =>
                {
                    b.Navigation("DocumentsWhoHaveThisDocumentType");
                });

            modelBuilder.Entity("WheresNannyApi.Domain.Entities.Nanny", b =>
                {
                    b.Navigation("CommentsRankNanny");

                    b.Navigation("ServicesNanny");
                });

            modelBuilder.Entity("WheresNannyApi.Domain.Entities.Person", b =>
                {
                    b.Navigation("CommentsRank");

                    b.Navigation("Nanny");

                    b.Navigation("PersonDocuments");

                    b.Navigation("ServicesPerson");
                });

            modelBuilder.Entity("WheresNannyApi.Domain.Entities.User", b =>
                {
                    b.Navigation("Person");
                });
#pragma warning restore 612, 618
        }
    }
}
