using Microsoft.EntityFrameworkCore;

namespace RepairShop.Tests;

public static class TestExtensions
{
    public static Mock<DbSet<T>> GenerateDbSetMock<T>(this IEnumerable<T> collection) where T : class
    {
        var collectionQueryable = collection.AsQueryable();
        var mockSet = new Mock<DbSet<T>>();
        mockSet.As<IQueryable<T>>().Setup(x => x.Provider).Returns(collectionQueryable.Provider);
        mockSet.As<IQueryable<T>>().Setup(x => x.Expression).Returns(collectionQueryable.Expression);
        mockSet.As<IQueryable<T>>().Setup(x => x.ElementType).Returns(collectionQueryable.ElementType);
        mockSet.As<IQueryable<T>>().Setup(x => x.GetEnumerator()).Returns(collectionQueryable.GetEnumerator());

        return mockSet;
    }
}