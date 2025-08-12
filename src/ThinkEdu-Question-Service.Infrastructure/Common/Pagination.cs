namespace ThinkEdu_Question_Service.Infrastructure.Common
{
    public static class Pagination
    {
        public static IQueryable<T> BuildQueryPagination<T>(IQueryable<T> sql, int? rows, int? page)
        {
            var pageSize = rows is null or <= 0 ? 10 : rows ?? 10;
            var pageIndex = page is null or <= 0 ? 1 : page ?? 1;

            return sql
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);
        }
    }
}