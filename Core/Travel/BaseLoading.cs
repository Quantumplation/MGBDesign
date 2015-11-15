namespace Core.Travel
{
    /// <summary>
    /// Context for queries involving loading ships (i.e. flat delay before actual movement commences)
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TSelf"></typeparam>
    public abstract class BaseLoadingContext<TResult, TSelf> : BaseContext<TResult, TSelf>
        where TResult : BaseConstants<TSelf, TResult>
        where TSelf : BaseContext<TResult, TSelf>
    {
        /// <summary>
        /// Number of ships waiting to travel
        /// </summary>
        public uint Travellers { get; set; }
    }

    public abstract class BaseLoadingConstants<TContext, TSelf>
        : BaseConstants<TContext, TSelf>
        where TContext : BaseContext<TSelf, TContext>
        where TSelf : BaseConstants<TContext, TSelf>
    {
        /// <summary>
        /// Time to wait before depature
        /// </summary>
        public abstract decimal DepartureDelay { get; }

        protected BaseLoadingConstants(TContext context)
            : base(context)
        {
        }
    }
}
