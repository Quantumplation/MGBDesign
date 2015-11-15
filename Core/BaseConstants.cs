namespace Core
{
    /// <summary>
    /// Base class for the results of queries
    /// </summary>
    /// <typeparam name="TContext">Type of the context this query is being made in</typeparam>
    /// <typeparam name="TSelf">Type of self</typeparam>
    public abstract class BaseConstants<TContext, TSelf>
        where TContext : BaseContext<TSelf, TContext>
        where TSelf : BaseConstants<TContext, TSelf>
    {
        protected readonly TContext Context;

        protected BaseConstants(TContext context)
        {
            Context = context;
        }
    }
}
