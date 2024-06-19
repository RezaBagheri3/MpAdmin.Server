using Microsoft.EntityFrameworkCore;
using MpAdmin.Server.DAL.Context;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors();

builder.Services.AddControllers();

builder.Services.AddDbContext<MpAdminContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"));
    }
);

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

var app = builder.Build();


var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<MpAdminContext>();
dbContext.Database.Migrate();


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.DisplayRequestDuration();
    options.DocExpansion(DocExpansion.None);
});
//}

app.UseCors(opt => opt
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();