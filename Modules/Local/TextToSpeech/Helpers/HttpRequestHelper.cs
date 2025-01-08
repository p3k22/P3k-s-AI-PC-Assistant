namespace TextToSpeech.Helpers;

public static class HttpRequestHelper
{
   /// <summary>
   ///    Send a HttpRequestMessage asynchronously and return the response
   /// </summary>
   /// <param name="httpClient">Client to use to send message</param>
   /// <param name="requestMessage">Message to send</param>
   /// <param name="cancellationToken">Token for cancelling prematurely</param>
   /// <returns></returns>
   public static async Task<HttpResponseMessage?> SendRequestAsync(
      HttpClient httpClient,
      HttpRequestMessage requestMessage,
      CancellationToken cancellationToken = default)
   {
      try
      {
         var response = await httpClient.SendAsync(
                        requestMessage,
                        HttpCompletionOption.ResponseHeadersRead,
                        cancellationToken);
         return response.IsSuccessStatusCode ? response : null;
      }
      catch (TaskCanceledException)
      {
         return null;
      }
      catch (Exception)
      {
         return null;
      }
   }
}