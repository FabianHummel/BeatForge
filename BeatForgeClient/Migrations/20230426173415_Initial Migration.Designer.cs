﻿// <auto-generated />
using BeatForgeClient.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BeatForgeClient.Migrations
{
    [DbContext(typeof(BeatForgeContext))]
    [Migration("20230426173415_Initial Migration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.16");

            modelBuilder.Entity("BeatForgeClient.Infrastructure.Channel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("SongId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("SongId");

                    b.ToTable("c_channel");
                });

            modelBuilder.Entity("BeatForgeClient.Infrastructure.Instrument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ChannelId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId")
                        .IsUnique();

                    b.ToTable("i_instrument");
                });

            modelBuilder.Entity("BeatForgeClient.Infrastructure.Note", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ChannelId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Duration")
                        .HasColumnType("TEXT");

                    b.Property<int>("Pitch")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Start")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.ToTable("n_note");
                });

            modelBuilder.Entity("BeatForgeClient.Infrastructure.Preferences", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("SongId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Volume")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("SongId");

                    b.ToTable("p_preferences");
                });

            modelBuilder.Entity("BeatForgeClient.Infrastructure.Song", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("s_song");
                });

            modelBuilder.Entity("BeatForgeClient.Infrastructure.Channel", b =>
                {
                    b.HasOne("BeatForgeClient.Infrastructure.Song", "Song")
                        .WithMany("Channels")
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Song");
                });

            modelBuilder.Entity("BeatForgeClient.Infrastructure.Instrument", b =>
                {
                    b.HasOne("BeatForgeClient.Infrastructure.Channel", "Channel")
                        .WithOne("Instrument")
                        .HasForeignKey("BeatForgeClient.Infrastructure.Instrument", "ChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Channel");
                });

            modelBuilder.Entity("BeatForgeClient.Infrastructure.Note", b =>
                {
                    b.HasOne("BeatForgeClient.Infrastructure.Channel", "Channel")
                        .WithMany("Notes")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Channel");
                });

            modelBuilder.Entity("BeatForgeClient.Infrastructure.Preferences", b =>
                {
                    b.HasOne("BeatForgeClient.Infrastructure.Song", "Song")
                        .WithMany()
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Song");
                });

            modelBuilder.Entity("BeatForgeClient.Infrastructure.Channel", b =>
                {
                    b.Navigation("Instrument")
                        .IsRequired();

                    b.Navigation("Notes");
                });

            modelBuilder.Entity("BeatForgeClient.Infrastructure.Song", b =>
                {
                    b.Navigation("Channels");
                });
#pragma warning restore 612, 618
        }
    }
}