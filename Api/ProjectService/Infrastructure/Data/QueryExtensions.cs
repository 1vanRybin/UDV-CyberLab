using Microsoft.EntityFrameworkCore;

namespace Infrastucture.Data;

public static class QueryExtensions
{
    public static async Task<double> AverageOrDefaultAsync(this IQueryable<double> source)
    {
        if (await source.AnyAsync())
        {
            return await source.AverageAsync();
        }

        return 0;
    }
}