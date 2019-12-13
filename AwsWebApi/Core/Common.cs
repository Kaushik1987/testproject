using AwsWebApi.Models;
using FizzWare.NBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AwsWebApi.Core
{
    public static class Common
    {
        public static IEnumerable<Employee> EmployeesList { get; set; }
        public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, bool ascending)
        {
            return ascending ? source.OrderBy(keySelector) : source.OrderByDescending(keySelector);
        }       
    }
}