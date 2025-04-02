using System.Security.Principal;
using System.Text.Json;

namespace MotoApp2.Entities.Extensions;

public static class EntityExtensions
{
    public static T? Copy<T>(this T itemToCopy) where T : IEntity
    {
        var json = JsonSerializer.Serialize(itemToCopy);

        return JsonSerializer.Deserialize<T>(json);
    }
}

