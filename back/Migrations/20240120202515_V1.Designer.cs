﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Models;

#nullable disable

namespace GameHeaven.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20240120202515_V1")]
    partial class V1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Models.Game", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Genre")
                        .HasColumnType("int");

                    b.Property<int>("Platform")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Models.Game_GamingList", b =>
                {
                    b.Property<int>("GameID")
                        .HasColumnType("int");

                    b.Property<int>("GamingListID")
                        .HasColumnType("int");

                    b.HasKey("GameID", "GamingListID");

                    b.HasIndex("GamingListID");

                    b.ToTable("Game_GamingLists");
                });

            modelBuilder.Entity("Models.GamingList", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("CreatorOfGamingListID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("CreatorOfGamingListID");

                    b.ToTable("GamingLists");
                });

            modelBuilder.Entity("Models.Player", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Password")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Models.PlayerGame", b =>
                {
                    b.Property<int>("PlayerID")
                        .HasColumnType("int");

                    b.Property<int>("GameID")
                        .HasColumnType("int");

                    b.HasKey("PlayerID", "GameID");

                    b.HasIndex("GameID");

                    b.ToTable("PlayerGames");
                });

            modelBuilder.Entity("Models.Review", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CreatorOfReviewID")
                        .HasColumnType("int");

                    b.Property<float>("Rating")
                        .HasColumnType("real");

                    b.Property<int?>("ReviewedGameID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("CreatorOfReviewID");

                    b.HasIndex("ReviewedGameID");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("Models.Game_GamingList", b =>
                {
                    b.HasOne("Models.Game", "Game")
                        .WithMany("GameListsOfGame")
                        .HasForeignKey("GameID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.GamingList", "GamingList")
                        .WithMany("GamesInGamingList")
                        .HasForeignKey("GamingListID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("GamingList");
                });

            modelBuilder.Entity("Models.GamingList", b =>
                {
                    b.HasOne("Models.Player", "CreatorOfGamingList")
                        .WithMany("GamingListsOfPlayer")
                        .HasForeignKey("CreatorOfGamingListID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatorOfGamingList");
                });

            modelBuilder.Entity("Models.PlayerGame", b =>
                {
                    b.HasOne("Models.Game", "Game")
                        .WithMany("PlayersOfGame")
                        .HasForeignKey("GameID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Player", "Player")
                        .WithMany("GamesOfPlayer")
                        .HasForeignKey("PlayerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Models.Review", b =>
                {
                    b.HasOne("Models.Player", "CreatorOfReview")
                        .WithMany("ReviewsOfPlayer")
                        .HasForeignKey("CreatorOfReviewID");

                    b.HasOne("Models.Game", "ReviewedGame")
                        .WithMany("ReviewsOfGame")
                        .HasForeignKey("ReviewedGameID");

                    b.Navigation("CreatorOfReview");

                    b.Navigation("ReviewedGame");
                });

            modelBuilder.Entity("Models.Game", b =>
                {
                    b.Navigation("GameListsOfGame");

                    b.Navigation("PlayersOfGame");

                    b.Navigation("ReviewsOfGame");
                });

            modelBuilder.Entity("Models.GamingList", b =>
                {
                    b.Navigation("GamesInGamingList");
                });

            modelBuilder.Entity("Models.Player", b =>
                {
                    b.Navigation("GamesOfPlayer");

                    b.Navigation("GamingListsOfPlayer");

                    b.Navigation("ReviewsOfPlayer");
                });
#pragma warning restore 612, 618
        }
    }
}
