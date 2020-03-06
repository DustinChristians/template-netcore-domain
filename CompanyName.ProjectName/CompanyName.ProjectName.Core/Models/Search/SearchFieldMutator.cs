using System;
using System.Linq;

namespace CompanyName.ProjectName.Core.Models.Search
{
    public delegate IQueryable<TItem> QueryMutator<TItem, TSearch>(IQueryable<TItem> items, TSearch search);

    public class SearchFieldMutator<TItem, TSearch>
    {
        public SearchFieldMutator(Predicate<TSearch> condition, QueryMutator<TItem, TSearch> mutator)
        {
            Condition = condition;
            Mutator = mutator;
        }

        public Predicate<TSearch> Condition { get; set; }
        public QueryMutator<TItem, TSearch> Mutator { get; set; }

        public IQueryable<TItem> Apply(TSearch search, IQueryable<TItem> query)
        {
            return Condition(search) ? Mutator(query, search) : query;
        }
    }
}
