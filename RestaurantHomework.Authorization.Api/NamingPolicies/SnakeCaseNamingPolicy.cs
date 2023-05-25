using System.Text;
using System.Text.Json;

namespace RestaurantHomework.Authorization.Api.NamingPolicies;

public class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        var sb = new StringBuilder
        {
            Capacity = 0,
            Length = 0
        };
        sb.Append(char.ToLowerInvariant(name[0]));
        for(int i = 1; i < name.Length; ++i) {
            var c = name[i];
            if(char.IsUpper(c)) 
            {
                sb.Append('_');
                sb.Append(char.ToLowerInvariant(c));
            } else 
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }
}