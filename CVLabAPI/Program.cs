
using CVLabAPI.Data;
using CVLabAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CVLabAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddScoped<SkillsServices>();
            builder.Services.AddScoped<ProjectsServices>();


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection(); // ska kanske tas bort.
            app.UseAuthorization();
            app.MapControllers();



            app.MapPost("/skill", async (Skill skill, SkillsServices service) =>
            {
                await service.AddSkill(skill);
                return Results.Ok();
            });

            app.MapPost("/project", async (Project project, ProjectsServices service) =>
            {
                await service.AddProject(project);
                return Results.Ok();
            });

            app.MapGet("/skills", async (SkillsServices service) =>
            {
                var getAll = await service.GetSkills();
                return Results.Ok(getAll);
            });

            app.MapGet("/projects", async (ProjectsServices service) =>
            {
                var getAll = await service.GetProjects();
                return Results.Ok(getAll);
            });


            app.MapPut("skill/{id}", async (int id, Skill skill, SkillsServices service) =>
            {
                var updateSkill = await service.UpdateSkill(id, skill);
                if (updateSkill == null)
                    return Results.NotFound("Skill not found");
                return Results.Ok(updateSkill);
            });

            app.MapPut("/project/{id}", async (int id, Project project, ProjectsServices service) =>
            {
                var updateProject = await service.UpdateProject(id, project);
                if (updateProject == null)
                    return Results.NotFound("Could not find project");

                return Results.Ok(updateProject);
            });

            app.MapDelete("/skill/{id}", async (int id, SkillsServices service) =>
            {
                var deletedSkill = await service.DeleteSkill(id);
                if (deletedSkill == null)
                    return Results.NotFound("Skill not found");

                return Results.Ok(deletedSkill);
            });

            app.MapDelete("/project/{id}", async (int id, ProjectsServices service) =>
            {
                var deletedProject = await service.DeleteProject(id);
                if (deletedProject == null)
                    return Results.NotFound("Project not found");

                return Results.Ok(deletedProject);
            });

            app.Run();
        }
    }
}
