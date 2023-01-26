var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


var beers = new Beear[]
{
    new Beear("Ice", "Polar"),
    new Beear("Solera Azul", "Regional"),
    new Beear("Solera Verde", "Brahama"),
};

app.MapGet("/Beear/{quantity}", (int quantity) =>
{
    return beers.Take(quantity);
}).AddEndpointFilter(async (context, next) =>
{

    int quantity = context.GetArgument<int>(0);

    if (quantity <= 0)
    {
        return Results.Problem("Deben enviar un número mayor que cero");
    }
    return await next(context);
}).AddEndpointFilter<MyFilter>();


app.Run();

internal record Beear(string Name, string Brand);

public class MyFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        int quantity = context.GetArgument<int>(0);

        if (quantity > 20)
        {
            return Results.Problem("Deben enviar un número mayor menor que 20");
        }
        return await next(context);
    }
}