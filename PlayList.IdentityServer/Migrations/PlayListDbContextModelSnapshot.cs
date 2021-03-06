// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PlayList.DataAccess;

namespace PlayList.IdentityServer.Migrations
{
    [DbContext(typeof(PlayListDbContext))]
    partial class PlayListDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.5");

            modelBuilder.Entity("PlayList.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("DateTime")
                        .HasColumnName("CreateTime");

                    b.Property<string>("CreateUser")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("CreateUser");

                    b.Property<string>("NickName")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<string>("RealName")
                        .HasColumnType("longtext");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasColumnName("Status");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("DateTime")
                        .HasColumnName("UpdateTime");

                    b.Property<string>("UpdateUser")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("UpdateUser");

                    b.HasKey("Id");

                    b.ToTable("Custoemr");
                });

            modelBuilder.Entity("PlayList.Models.Manager", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Address");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("DateTime")
                        .HasColumnName("CreateTime");

                    b.Property<string>("CreateUser")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("CreateUser");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("Password");

                    b.Property<string>("RealName")
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("RealName");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasColumnName("Status");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("DateTime")
                        .HasColumnName("UpdateTime");

                    b.Property<string>("UpdateUser")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("UpdateUser");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("UserName");

                    b.HasKey("Id");

                    b.ToTable("Manager");
                });

            modelBuilder.Entity("PlayList.Models.PlayInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("DateTime")
                        .HasColumnName("CreateTime");

                    b.Property<string>("CreateUser")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("CreateUser");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Note");

                    b.Property<string>("PlayName")
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("PlayName");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasColumnName("Status");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("DateTime")
                        .HasColumnName("UpdateTime");

                    b.Property<string>("UpdateUser")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("UpdateUser");

                    b.HasKey("Id");

                    b.ToTable("PlayInfo");
                });

            modelBuilder.Entity("PlayList.Models.Vote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("DateTime")
                        .HasColumnName("CreateTime");

                    b.Property<string>("CreateUser")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("CreateUser");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int")
                        .HasColumnName("CustomerId");

                    b.Property<int>("PlayId")
                        .HasColumnType("int")
                        .HasColumnName("PlayId");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasColumnName("Status");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("DateTime")
                        .HasColumnName("UpdateTime");

                    b.Property<string>("UpdateUser")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("UpdateUser");

                    b.HasKey("Id");

                    b.ToTable("Vote");
                });
#pragma warning restore 612, 618
        }
    }
}
