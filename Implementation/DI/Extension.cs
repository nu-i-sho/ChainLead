namespace ChainLead.Implementation.DI
{
    using ChainLead.Contracts;
    using Microsoft.Extensions.DependencyInjection;

    public static class Extension
    {
        public static IServiceCollection AddConditionMath(
            this IServiceCollection services) =>
                services.AddSingleton<IConditionMath, ConditionMath>();

        public static IServiceCollection AddHandlerMath(
            this IServiceCollection services) =>
                services.AddSingleton<IHandlerMath, HandlerMath>();
    }
}
