using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StaffManagement.Application.Common.Behavior;
using StaffManagement.Application.Common.Interfaces;
using StaffManagement.Application.Departments.Commands;
using StaffManagement.Application.Departments.Queries;
using StaffManagement.Application.Positions.Commands;
using StaffManagement.Application.Positions.Queries;
using StaffManagement.Infrastructure.Data;
using StaffManagement.WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IApplicationDbContext>(provider => 
provider.GetRequiredService<AppDbContext>());

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateDepartmentCommand).Assembly));

builder.Services.AddValidatorsFromAssemblyContaining<CreateDepartmentCommandValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();
