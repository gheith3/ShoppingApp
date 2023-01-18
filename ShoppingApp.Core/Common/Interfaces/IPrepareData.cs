namespace ShoppingApp.Core.Common.Interfaces;

public interface IPrepareData<T>
{
    IQueryable<T> GetQuery();
    IQueryable<T> GetFilterQuery(IQueryable<T> query, string? searchQuery = null, Dictionary<string, object>? filters = null);
    Task<T> GetRecord(string id);
}