using Microsoft.EntityFrameworkCore;
using ThinkEdu_Question_Service.Application.Contracts.IRepository.Questions;
using ThinkEdu_Question_Service.Application.Models.Questions;
using ThinkEdu_Question_Service.Domain.Entities;
using ThinkEdu_Question_Service.Domain.Enums;
using ThinkEdu_Question_Service.Infrastructure.Common;
using ThinkEdu_Question_Service.Infrastructure.Persistence;

namespace ThinkEdu_Question_Service.Infrastructure.Repositories.Questions
{
    public class QuestionRepository : BaseRepository<Question>, IQuestionRepository
    {
        private readonly ThinkEduContext _context;
        public QuestionRepository(ThinkEduContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(Question, List<Question>)> GetQuestionByIdAsync(string questionId)
        {
            var question = await _context.Questions.AsNoTracking()
                                                   .Include(q => q.Answers)
                                                   .SingleOrDefaultAsync(q => q.Id == questionId && q.Status != EStatus.Delete.ToString());
                                                   

            var listChildQuestion = await _context.Questions.AsNoTracking()
                                                   .Where(q => q.ParentId == questionId && q.Status != EStatus.Delete.ToString())
                                                   .Include(q => q.Answers)
                                                   .ToListAsync();
            return (question!, listChildQuestion!);
        }
   
        public async Task<(int count, List<Question>)> GetListQuestionAsync(QuestionSearchModel model)
        {
            var query = _context.Questions.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(model.KeySearch))
            {
                var keySearch = model.KeySearch.Trim().ToLower();
                query = query
                    .Where(x => x.Title.ToLower().Contains(keySearch));
            }

            if (!string.IsNullOrEmpty(model.LessonId))
            {
                query = query.Where(q => q.LessonId == model.LessonId);
            }

            if (!string.IsNullOrEmpty(model.Type))
            {
                var type = model.Type.Trim().ToLower();
                query = query
                    .Where(q => q.Type.ToLower().Equals(type));
            }

            if (!string.IsNullOrEmpty(model.Level))
            {
                var level = model.Level.Trim().ToLower();
                query = query.Where(q => q.Level.ToLower().Equals(level));
            }

            if (!string.IsNullOrEmpty(model.Group))
            {
                var group = model.Group.Trim().ToLower();
                query = query.Where(q => q.Group.ToLower().Equals(group));
            }

            if (!string.IsNullOrEmpty(model.Status))
            {
                var status = model.Status.Trim().ToLower();
                query = query.Where(q => q.Status.ToLower().Equals(status));
            }

            query = query
                .Where(x => x.Status != EStatus.Delete.ToString())
                .OrderBy(x => x.Id);

            query = query.Where(q => q.ParentId == null || q.STT == 0);

            int count = await query.CountAsync();

            var all = model.GetFieldAll();
            if (!all)
            {
                query = Pagination.BuildQueryPagination<Question>(query,  model.Rows, model.Page);
            }

            var result = await query.ToListAsync();

            if (result.Count == 0)
            {
                return (0, []);
            }

            return (count, result);
        }

    }
}