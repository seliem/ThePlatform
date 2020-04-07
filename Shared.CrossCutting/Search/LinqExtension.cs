using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shared.CrossCutting.Search
{
    public static class LinqExtension
    {
        public static IQueryable<T> Filter<T>(this IQueryable<T> query, string fieldName, Type fieldType, object fieldValue, bool IsEqualExpr = true)
        {
            ParameterExpression c = Expression.Parameter(typeof(T), "b");
            object objValue = ChangeType(fieldValue, fieldType);

            Expression fieldExpression = null;
            if (IsEqualExpr)
                fieldExpression = Expression.Equal(Expression.Property(c, fieldName), Expression.Constant(objValue, fieldType));
            else
                fieldExpression = Expression.NotEqual(Expression.Property(c, fieldName), Expression.Constant(objValue, fieldType));

            Expression<Func<T, bool>> IsFieldExpression = Expression.Lambda<Func<T, bool>>(fieldExpression, c);

            query = query.Where(IsFieldExpression);
            return query;
        }

        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> source, SortCriteria sortCriteria)
        {
            if (sortCriteria != null && !string.IsNullOrEmpty(sortCriteria.SortColumn))
            {
                var param = Expression.Parameter(typeof(T), "item");
                var sortExpression = Expression.Lambda<Func<T, object>>
                    (Expression.Convert(Expression.Property(param, sortCriteria.SortColumn), typeof(object)), param);
                return (sortCriteria.IsDescending == true) ? source.AsQueryable<T>().OrderByDescending<T, object>(sortExpression) : source.AsQueryable<T>().OrderBy<T, object>(sortExpression);
            }
            return source;
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, SortCriteria sortCriteria)
        {
            if (sortCriteria != null && !string.IsNullOrEmpty(sortCriteria.SortColumn))
            {
                var param = Expression.Parameter(typeof(T), "item");
                var sortExpression = Expression.Lambda<Func<T, object>>
                    (Expression.Convert(Expression.Property(param, sortCriteria.SortColumn), typeof(object)), param);
                return (sortCriteria.IsDescending == true) ?
                    source.AsQueryable<T>().OrderByDescending<T, object>(sortExpression) :
                    source.AsQueryable<T>().OrderBy<T, object>(sortExpression);
            }
            return source;
        }

        private static object ChangeType(object value, Type conversionType)
        {

            if (conversionType == null)
            {
                throw new ArgumentNullException("conversionType");
            }

            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }
                NullableConverter nullableConverter = new NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }
            return Convert.ChangeType(value, conversionType, CultureInfo.InvariantCulture);
        }

    }
}
