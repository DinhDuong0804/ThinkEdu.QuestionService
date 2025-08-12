using System.Linq.Expressions;

namespace ThinkEdu_Question_Service.Application.Common
{         
    public interface IBaseRepository<T> where T : class
    {
        /***
           * Lớp cơ bản của Repository gồm các phương thức hay dùng đến.
           * Sử dụng GENERIC REPOSITORY: cho phép chúng ta định nghĩa một kiểu dữ liệu hoặc lớp mà không cần quan tâm đến kiểu dữ liệu chính xác của nó là gì => cho phép chúng ta định nghĩa một data structure dùng chung
       */

        IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false,
            params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<T>> GetListSortAsync(string? keySort, string sort, List<string> includes);
        Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includeProperties);
        Task<T?> GetByIdAsync(string id, bool trackChanges = false);
        Task<IEnumerable<T>> GetAllAsync(bool trackChanges = false);
        Task<T?> GetByIdAsync(int id, bool trackChanges = false);
        T? GetById(int id, bool trackChanges = false);
        Task<bool> ExitsByIdAsync(int id);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Delete(T entity);
        void RemoveRange(IEnumerable<T> entities);
        Task SaveDbSetAsync(CancellationToken cancellationToken = default);
        Task BulkSaveDbChangesAsync(CancellationToken cancellationToken = default);
    }
}