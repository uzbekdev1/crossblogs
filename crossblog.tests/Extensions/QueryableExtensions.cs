using System;
using System.Collections.Generic;
using System.Linq;
using crossblog.tests.Collections;
using crossblog.tests.Controllers;
using crossblog.tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace crossblog.tests.Extensions
{
    public static class QueryableExtensions
    {
        public static Mock<DbSet<T>> ToAsyncDbSetMock<T>(this IEnumerable<T> source) where T : class
        { 
            var data = source.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();

            mockSet.As<IAsyncEnumerable<T>>().Setup(m => m.GetEnumerator()).Returns(new TestAsyncEnumerator<T>(data.GetEnumerator()));
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<T>(data.Provider));
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            return mockSet;
        }
    }
}