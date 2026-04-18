using FluentValidation;
using Microsoft.EntityFrameworkCore;
using StaffManagement.Application.Common.Interfaces;
using StaffManagement.Application.Departments.Commands;
using StaffManagement.Application.Departments.Queries;
using StaffManagement.Application.Positions.Commands;
using StaffManagement.Application.Positions.Queries;
using StaffManagement.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IApplicationDbContext>(provider => 
provider.GetRequiredService<AppDbContext>());

builder.Services.AddScoped<CreateDepartmentCommandHandler>();
builder.Services.AddScoped<UpdateDepartmentCommandHandler>();
builder.Services.AddScoped<DeleteDepartmentCommandHandler>();
builder.Services.AddScoped<GetDepartmentByIdQueryHandler>();
builder.Services.AddScoped<GetDepartmentListQueryHandler>();
builder.Services.AddScoped<CreatePositionCommandHandler>();
builder.Services.AddScoped<UpdatePositionCommandHandler>();
builder.Services.AddScoped<DeletePositionCommandHandler>();
builder.Services.AddScoped<GetPositionByIdQueryHandler>();
builder.Services.AddScoped<GetPositionsListQueryHandler>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateDepartmentCommandValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();
