namespace SQRL.Server
{
    public static class UrlSafeBase64Encoder
    {
        public static string Encode(string base64String)
        {
            if (base64String == null) return null;
            return base64String.Replace('+', '-').Replace('/', '_');
        }

        public static string Decode(string safeString)
        {
            if (safeString == null) return null;
            return safeString.Replace('-', '+').Replace('_', '/');
        }
    }
}