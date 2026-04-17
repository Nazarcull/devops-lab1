using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TaskService.Data;

#nullable disable

namespace TaskService.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20240101000000_InitialCreate")]
partial class InitialCreate
{
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasAnnotation("ProductVersion", "8.0.0")
            .HasAnnotation("Relational:MaxIdentifierLength", 63);

        NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

        modelBuilder.Entity("TaskService.Models.TaskItem", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("integer")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
            b.Property<DateTime?>("CompletedAt").HasColumnType("timestamp with time zone");
            b.Property<DateTime>("CreatedAt").HasColumnType("timestamp with time zone");
            b.Property<string>("Description").HasMaxLength(1000).HasColumnType("character varying(1000)");
            b.Property<bool>("IsCompleted").HasColumnType("boolean");
            b.Property<int>("Priority").HasColumnType("integer");
            b.Property<string>("Title").IsRequired().HasMaxLength(200).HasColumnType("character varying(200)");
            b.HasKey("Id");
            b.ToTable("Tasks");
        });
    }
}
