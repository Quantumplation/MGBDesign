namespace Core.Travel
{
    /// <summary>
    /// Context for queries involving travelling from one location to another
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TSelf"></typeparam>
    public abstract class BaseTravelContext<TResult, TSelf> : BaseContext<TResult, TSelf>
        where TResult : BaseConstants<TSelf, TResult>
        where TSelf : BaseContext<TResult, TSelf>
    {
        /// <summary>
        /// Distance from start to end
        /// </summary>
        public decimal Distance { get; set; }
    }

    public abstract class BaseTravelConstants<TContext, TSelf>
        : BaseConstants<TContext, TSelf>
        where TContext : BaseContext<TSelf, TContext>
        where TSelf : BaseConstants<TContext, TSelf>
    {
        /// <summary>
        /// Speed of movement along this course
        /// </summary>
        public abstract decimal Speed { get; }

        protected BaseTravelConstants(TContext context)
            : base(context)
        {
        }
    }
}
