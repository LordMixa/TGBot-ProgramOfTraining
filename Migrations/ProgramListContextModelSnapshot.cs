﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Teleg_training;

#nullable disable

namespace Teleg_training.Migrations
{
    [DbContext(typeof(ProgramListContext))]
    partial class ProgramListContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Teleg_training.DBEntities.DBAuthor", b =>
                {
                    b.Property<int>("AuthorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AuthorId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("AuthorId");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("Teleg_training.DBEntities.DBLike", b =>
                {
                    b.Property<int>("LikeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LikeId"));

                    b.Property<int>("ProgramListId")
                        .HasColumnType("int");

                    b.Property<long>("TGId")
                        .HasColumnType("bigint");

                    b.HasKey("LikeId");

                    b.HasIndex("ProgramListId");

                    b.HasIndex("TGId");

                    b.ToTable("DBLikes");
                });

            modelBuilder.Entity("Teleg_training.DBEntities.DBProduct", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"));

                    b.Property<double>("Calories")
                        .HasColumnType("float");

                    b.Property<double>("Carbohydrates")
                        .HasColumnType("float");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Fats")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Proteins")
                        .HasColumnType("float");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProductId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Teleg_training.DBEntities.DBProgramList", b =>
                {
                    b.Property<int>("ProgramId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProgramId"));

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Difficult")
                        .HasColumnType("float");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Mode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Program")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProgramId");

                    b.HasIndex("AuthorId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("ProgramLists");
                });

            modelBuilder.Entity("Teleg_training.DBEntities.DBUser", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("TGId")
                        .HasColumnType("bigint");

                    b.HasKey("UserId");

                    b.ToTable("DBUsers");
                });

            modelBuilder.Entity("Teleg_training.DBEntities.DBLike", b =>
                {
                    b.HasOne("Teleg_training.DBEntities.DBProgramList", "ProgramList")
                        .WithMany("Likes")
                        .HasForeignKey("ProgramListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Teleg_training.DBEntities.DBUser", "User")
                        .WithMany("Likes")
                        .HasForeignKey("TGId")
                        .HasPrincipalKey("TGId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProgramList");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Teleg_training.DBEntities.DBProgramList", b =>
                {
                    b.HasOne("Teleg_training.DBEntities.DBAuthor", "Author")
                        .WithMany("ProgramLists")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Teleg_training.DBEntities.DBAuthor", b =>
                {
                    b.Navigation("ProgramLists");
                });

            modelBuilder.Entity("Teleg_training.DBEntities.DBProgramList", b =>
                {
                    b.Navigation("Likes");
                });

            modelBuilder.Entity("Teleg_training.DBEntities.DBUser", b =>
                {
                    b.Navigation("Likes");
                });
#pragma warning restore 612, 618
        }
    }
}
