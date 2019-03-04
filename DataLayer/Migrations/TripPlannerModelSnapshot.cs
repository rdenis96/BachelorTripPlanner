﻿// <auto-generated />
using System;
using DataLayer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataLayer.Migrations
{
    [DbContext(typeof(TripPlanner))]
    partial class TripPlannerModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DataLayer.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("FirstName");

                    b.Property<string>("Ip")
                        .IsRequired();

                    b.Property<string>("LastName");

                    b.Property<DateTime?>("LastOnline")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("Phone");

                    b.Property<DateTime>("RegisterDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DataLayer.Models.UserInterest", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.HasKey("UserId");

                    b.ToTable("UserInterests");

                    b.HasDiscriminator<string>("Discriminator").HasValue("UserInterest");
                });

            modelBuilder.Entity("DataLayer.Models.UserInterestCountryAndCity", b =>
                {
                    b.HasBaseType("DataLayer.Models.UserInterest");

                    b.Property<string>("Cities");

                    b.Property<int>("Countries");

                    b.HasDiscriminator().HasValue("UserInterestCountryAndCity");
                });

            modelBuilder.Entity("DataLayer.Models.UserInterestTouristAttraction", b =>
                {
                    b.HasBaseType("DataLayer.Models.UserInterest");

                    b.Property<string>("TouristAttractions");

                    b.HasDiscriminator().HasValue("UserInterestTouristAttraction");
                });

            modelBuilder.Entity("DataLayer.Models.UserInterestTransport", b =>
                {
                    b.HasBaseType("DataLayer.Models.UserInterest");

                    b.Property<int>("Transports");

                    b.HasDiscriminator().HasValue("UserInterestTransport");
                });

            modelBuilder.Entity("DataLayer.Models.UserInterestWeather", b =>
                {
                    b.HasBaseType("DataLayer.Models.UserInterest");

                    b.Property<int>("Weathers");

                    b.HasDiscriminator().HasValue("UserInterestWeather");
                });
#pragma warning restore 612, 618
        }
    }
}
