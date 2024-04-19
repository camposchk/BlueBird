using Nova_pasta.Model;
using Microsoft.EntityFrameworkCore;

string[] nomes = File.ReadAllLines("./nomes.txt");
string[] sobrenomes = File.ReadAllLines("./sobrenomes.txt");

Parallel.For(0, 1000, _ =>
{
    BirdBlueContext ctx = new();
    for (int i = 0; i < 1000; i++)
    {
        var user = nomes[Random.Shared.Next(nomes.Length - 1)] + " " + sobrenomes[Random.Shared.Next(sobrenomes.Length - 1)];
        var def = user.ToLower().Replace(" ", "");
        ctx.Database.ExecuteSql($"exec addUser {user}, {def + "@gmail.com"}, {def + "123"}");
    }
    ctx.SaveChanges();
});

Parallel.For(0, 1000, _ =>
{
    BirdBlueContext ctx = new();
    for (int i = 0; i < 1000; i++)
    {
        ctx.Database.ExecuteSql($"exec createPost {Random.Shared.Next(1000006)}, {"Flamengo."}");
    }
    ctx.SaveChanges();
});

Parallel.For(0, 1000, _ =>
{
    BirdBlueContext ctx = new();
    for (int i = 0; i < 100; i++)
    {
        ctx.Database.ExecuteSql($"exec likeButton {Random.Shared.Next(1000006)}, {Random.Shared.Next(1000000)}");
    }
    ctx.SaveChanges();
});

Parallel.For(0, 1000, _ =>
{
    BirdBlueContext ctx = new();
    for (int i = 0; i < 10000; i++)
    {
        try
        {
            ctx.Database.ExecuteSql($"exec followUser {Random.Shared.Next(1000005)}, {Random.Shared.Next(1000005)}");
        }
        catch { }
    }
    ctx.SaveChanges();

});
