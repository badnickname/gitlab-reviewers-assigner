﻿// <auto-generated />
using GitlabMonitor.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GitlabMonitor.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20230405134534_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0-preview.2.23128.3");

            modelBuilder.Entity("GitlabMonitor.Model.Statistic.AssignedMergeRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("References")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ReviewerId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ReviewerId");

                    b.ToTable("AssignedMergeRequests");
                });

            modelBuilder.Entity("GitlabMonitor.Model.Statistic.Reviewer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Reviewers");
                });

            modelBuilder.Entity("GitlabMonitor.Model.Statistic.AssignedMergeRequest", b =>
                {
                    b.HasOne("GitlabMonitor.Model.Statistic.Reviewer", "Reviewer")
                        .WithMany("AssignedMergeRequests")
                        .HasForeignKey("ReviewerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reviewer");
                });

            modelBuilder.Entity("GitlabMonitor.Model.Statistic.Reviewer", b =>
                {
                    b.Navigation("AssignedMergeRequests");
                });
#pragma warning restore 612, 618
        }
    }
}
