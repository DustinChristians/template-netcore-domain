namespace CompanyName.ProjectName.Core.Models.Helpers
{
    public class AsyncTryGetResult<T>
    {
        public bool Successful { get; set; }
        public T Value { get; set; }
    }
}
