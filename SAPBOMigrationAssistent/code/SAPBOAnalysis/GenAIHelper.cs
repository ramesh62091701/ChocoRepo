using SAPBOAnalysis.Models.GenAI;

namespace SAPBOAnalysis
{
    public static class GenAIHelper
    {
        public static async Task<string> GetCompletionResponse(string request, string url)
        {
            var completionRequest = new CompletionRequest();
            completionRequest.Model = "text-davinci-003";
            completionRequest.Platform = "0";
            completionRequest.Temperature = 0;
            completionRequest.Prompt = request;
            completionRequest.MaxTokens = 4000;
            completionRequest.MaxTokensToSample = 1000;
            completionRequest.N = 1;

            try
            {
                var response = await new HttpHelper().Execute<CompletionRequest, CompletionResponse>(completionRequest, HttpMethod.Post, "application/json", null, url);
                return response.value.isSuccess ? response.value.response! : string.Empty;
            }
            catch (Exception)
            {
                Logger.Log("Failed to analyze QueryPlan");
                return string.Empty;
            }

        }
    }
}
