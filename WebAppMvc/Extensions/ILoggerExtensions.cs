namespace WebAppMvc.Extensions
{
    public static class ILoggerExtensions
    {
        public static void Info(this ILogger logger, string s)
        {
            logger.LogInformation(s);
        }
    }
}
