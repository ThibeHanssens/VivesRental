namespace Vives.Services.Model
{
    public class PagedServiceResult<TEntity, TFilter>:ServiceResult<IList<TEntity>>
    {
        public required Paging Paging { get; set; }
        public Sorting? Sorting { get; set; }
        public TFilter? Filter { get; set; }
        public int TotalCount { get; set; }
    }
}
