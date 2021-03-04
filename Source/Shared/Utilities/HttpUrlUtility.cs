using System.Linq;

namespace Refactorizando.Shared.Utilities{
    public static class HttpUrlUtility {

        public static string CompleteUrlWithFinalSlahIfNeeded(this string url){

            if (url.Last() != '/')
            {
                return $"{url}/";
            }
            return url;
        }
    }
}