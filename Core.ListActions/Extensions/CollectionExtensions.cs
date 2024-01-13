using Core.ListActions.DTO;
using Infrastructure.Storage.Models;

namespace Core.ListActions.Extensions;

public static class CollectionExtensions
{
    public static UserListElementDTO[] GetDtos(this ICollection<UserListElement> collection) =>
        collection
            .Select(record => new UserListElementDTO(record.Number, record.Data))
            .ToArray();
}