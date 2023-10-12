using System.Text;
using System.Text.Json;

class ApiResponse
{
    public int? statuscode {  get; set; }
    public string? responsebody { get; set; }
}
class CreateTokenResponse : ApiResponse
{
    public int? expmonth {  get; set; }
    public int? expyear {  get; set; }
    public string? scheme { get; set; }
    public string? token { get; set; }
    public string? tokenid { get; set; }

}

class CreateCryptogramResponse : ApiResponse
{
    public string ?cryptogram { get; set; }
    public string? token { get; set; }
    public int? expmonth { get; set; }
    public int? expyear { get; set; }
    public string? par { get; set; }
    public string? paymentdataid { get; set; }
    public string? scheme { get; set; }
}

class GetStatusResponse : ApiResponse
{
    public string? tokenid { get; set; }
    public string? token { get; set; }
    public int? expmonth { get; set; }
    public int? expyear { get; set; }
    public string? status { get; set; }
    public string? scheme { get; set; }
}




class Cardtokenshttp{
    public static T Tx<T>(HttpMethod method, string payload, string apiUrl, string apikey)
    {
        int statuscode = -1;
        string responseBody;


        // Create an instance of HttpClient
        using (HttpClient client = new HttpClient())
        {
            try
            {
                var guid = Guid.NewGuid();
                // Create a HttpRequestMessage
                HttpRequestMessage request = new HttpRequestMessage(method, apiUrl);

                // Create a StringContent with the JSON string and specify the content type
                var content = new StringContent(payload, Encoding.UTF8, "application/json");

                // Add custom headers to the request (example: Authorization header) 
                request.Headers.Add("x-api-key", apikey);
                request.Headers.Add("x-request-id", guid.ToString());
                request.Headers.Add("User-Agent", "Cardtokens/1.0");

                // Set the content of the request
                request.Content = content;

                // Send the HTTP POST request and block until it completes
                HttpResponseMessage response = client.SendAsync(request).Result;

                responseBody = response.Content.ReadAsStringAsync().Result;
                statuscode = (int)response.StatusCode;
                Console.WriteLine(responseBody);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("POST request was successful.");
                    T res = JsonSerializer.Deserialize<T>(responseBody);
                    (res as ApiResponse).responsebody = responseBody;
                    (res as ApiResponse).statuscode = statuscode;
                    return res;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }


            throw new HttpRequestException("API request failed with status code: " + statuscode);
        }
    }
}