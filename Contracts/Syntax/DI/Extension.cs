namespace Nuisho.ChainLead.Contracts.Syntax.DI
{
    using Microsoft.Extensions.DependencyInjection;

    public static class Extension
    {
        internal class CallToken;

        public static IServiceCollection ConfigureChainLeadSyntax(
            this IServiceCollection services)
        {
            return services.AddSingleton(provider =>
            {
                ChainLeadSyntax.Configure(
                    provider.GetRequiredService<IHandlerMath>(),
                    provider.GetRequiredService<IConditionMath>());

                return new CallToken();
            });
        }
    }
}
