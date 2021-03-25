namespace Refactorizando.Server
{
    public static class Utilities
    {
        public static string Base64ForUrlEncode(string str)
        {
            var encbuff = Encoding.UTF8.GetBytes(str);
            return System.Web.HttpServerUtility.UrlTokenEncode(encbuff);
        }

        public static string Base64ForUrlDecode(string str)
        {
        var decbuff = HttpServerUtility.UrlTokenDecode(str);
        return decbuff != null ? Encoding.UTF8.GetString(decbuff) : null;
        }       
    }
}