using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using crossblog.tests.Collections;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace crossblog.tests.Infrastructure
{
    internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        internal TestAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        public IQueryable CreateQuery(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return new TestAsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {

            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return new TestAsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return _inner.Execute<TResult>(expression);
        }

        public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return new TestAsyncEnumerable<TResult>(expression);
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return Task.FromResult(Execute<TResult>(expression));
        }

    }
}