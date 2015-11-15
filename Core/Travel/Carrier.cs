namespace Core.Travel
{
    /// <summary>
    /// Queries concerning FTL carrier ships
    /// </summary>
    public static class Carrier
    {
        /// <summary>
        /// Queries concerning carrier travel between two specific locations
        /// </summary>
        public static class Travel
        {
            public class Context : BaseTravelContext<Constants, Context>
            {
            }

            public abstract class Constants : BaseTravelConstants<Context, Constants>
            {
                protected Constants(Context context)
                    : base(context)
                {
                }
            }
        }

        public static class Loading
        {
            public class Context : BaseLoadingContext<Constants, Context>
            {
            }

            public abstract class Constants : BaseLoadingConstants<Context, Constants>
            {
                protected Constants(Context context)
                    : base(context)
                {
                }
            }
        }
    }
}
