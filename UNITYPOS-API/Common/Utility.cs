using Newtonsoft.Json;

namespace UNITYPOS_API.Common
{
    public class Utility
    {
        public static string GetResult(string Result)
        {
            var response = string.Empty;

            if (string.IsNullOrEmpty(Result))
            {
                Result = JsonConvert.SerializeObject(new object());
                response = "{\"result\" :" + Result + ",\"ErrorInfo\":{ \"Message\":false, \"ErrorCode\":null,\"ErrorMessage\" : null, \"ErrorType\": null, \"InnerException\": null }}";
            }
            else
                response = "{\"result\" :" + Result + ",\"ErrorInfo\":{\"Message\":true, \"ErrorCode\":null,\"ErrorMessage\" : null, \"ErrorType\": null }}";

            return response;

        }


    }
}
