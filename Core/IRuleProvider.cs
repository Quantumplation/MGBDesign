namespace Core
{
    public interface IRuleProvider
    {
        TConstants Get<TConstants, TContext>(TContext contextA)
            where TConstants : BaseConstants<TContext, TConstants>
            where TContext : BaseContext<TConstants, TContext>;
    }
}
