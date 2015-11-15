namespace Core
{
    /// <summary>
    /// A context of a query being performed by the game. Provides necessary information about the game state to answer the query
    /// </summary>
    /// <typeparam name="TResult">Type of the object which will answer queries from this context</typeparam>
    /// <typeparam name="TSelf">Type of self</typeparam>
    public abstract class BaseContext<TResult, TSelf>
        where TResult : BaseConstants<TSelf, TResult>
        where TSelf : BaseContext<TResult, TSelf>
    {
        public TResult Get(IRuleProvider rules)
        {
            return rules.Get<TResult, TSelf>((TSelf)this);
        }
    }
}
