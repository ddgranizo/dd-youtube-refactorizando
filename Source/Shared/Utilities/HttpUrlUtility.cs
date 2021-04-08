using System;
using System.Linq;
using System.Text;

namespace Refactorizando.Shared.Utilities{
    public static class HttpUrlUtility {

        public static string CompleteUrlWithFinalSlahIfNeeded(this string url){

            if (url.Last() != '/')
            {
                return $"{url}/";
            }
            return url;
        }


        public static bool IsValidUrl(string uriName){
            return Uri.TryCreate(uriName, UriKind.Absolute, out Uri uriResult) 
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
       
    }
}