namespace SQRL.Server
{
    public static class UrlSafeBase64Encoder
    {
        public static string Encode(string base64String)
        {
            return base64String.Replace('+', '-').Replace('/', '_');
        }

        public static string Decode(string safeString)
        {
            return safeString.Replace('-', '+').Replace('_', '/');
        }
    }
}