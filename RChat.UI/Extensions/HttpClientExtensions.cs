namespace RChat.UI.Extensions
{
    public static class HttpClientExtensions
    {
        public static void ConfigureHttpClient(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(configuration["ApiHost"]!)});
        }

    }
}
