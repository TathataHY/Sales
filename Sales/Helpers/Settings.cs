namespace Sales.Helpers
{
    using Plugin.Settings;
    using Plugin.Settings.Abstractions;

    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        #region Setting Constants

        private const string tokenType = "token_type";
        private const string accessToken = "access_token";
        private const string isRemembered = "is_remembered";
        private const string userASP = "user_asp";
        private static readonly string StringDefault = string.Empty;
        private static readonly bool BooleanDefault = false;

        #endregion


        public static string TokenType
        {
            get => AppSettings.GetValueOrDefault(tokenType, StringDefault);
            set => AppSettings.AddOrUpdateValue(tokenType, value);
        }
        public static string AccessToken
        {
            get => AppSettings.GetValueOrDefault(accessToken, StringDefault);
            set => AppSettings.AddOrUpdateValue(accessToken, value);
        }
        public static bool IsRemembered
        {
            get => AppSettings.GetValueOrDefault(isRemembered, BooleanDefault);
            set => AppSettings.AddOrUpdateValue(isRemembered, value);
        }
        public static string UserASP
        {
            get => AppSettings.GetValueOrDefault(userASP, StringDefault);
            set => AppSettings.AddOrUpdateValue(userASP, value);
        }
    }
}
