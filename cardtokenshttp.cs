using System.Text;
using System.Text.Json;

//
// This is the overall Cardtokens API response object.
//
class ApiResponse
{
    public int? statuscode {  get; set; }
    public string? responsebody { get; set; }
}

//
// Object used to process create token response data
//
class CreateTokenResponse : ApiResponse
{
    public int? expmonth {  get; set; }
    public int? expyear {  get; set; }
    public string? scheme { get; set; }
    public string? token { get; set; }
    public string? tokenid { get; set; }

}

//
// Object used to process create cryptogram response data
//
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

//
// Object used to process token status response data
//
class GetStatusResponse : ApiResponse
{
    public string? tokenid { get; set; }
    public string? token { get; set; }
    public int? expmonth { get; set; }
    public int? expyear { get; set; }
    public string? status { get; set; }
    public string? scheme { get; set; }
}

//
// This class handles the HTTP communication towards the Cardtokens API
//
class Cardtokenshttp{

    //
    // This function is used to call Cardtokens
    // <param-name="method">The HTTP method to use - POST, GET, DELETE</param>
    // <param-name="payload">The JSON data to send to Cardtokens using POST</param>
    // <param-name="apiUrl">The full url to the Cardtokens endpoint</param>
    // <param-name="apikey">The apikey retrieved from Cardtokens used for authentication</param>
    //
    public static T Tx<T>(HttpMethod method, string payload, string apiUrl, string apikey)
    {
        int statuscode = -1;    // the http statuscode received from Cardtokens
        string responseBody;    // the response body received from Cardtokens


        // Create an instance of HttpClient
        using (HttpClient client = new HttpClient())
        {
            try
            {
                //
                // Send in an unique GUID for each HTTP request
                //
                var guid = Guid.NewGuid();
                
                //
                // Create a HttpRequestMessage
                //
                HttpRequestMessage request = new HttpRequestMessage(method, apiUrl);

                //
                // Create a StringContent with the JSON string and specify the content type
                //
                var content = new StringContent(payload, Encoding.UTF8, "application/json");

                //
                // Add custom headers to the request: apikey, request-id and user-agent
                // user-agent is required otherwise the request is blocked by the SHIELD
                //
                request.Headers.Add("x-api-key", apikey);
                request.Headers.Add("x-request-id", guid.ToString());
                request.Headers.Add("User-Agent", "Cardtokens/1.0");

                //
                // Set the content of the request
                //
                request.Content = content;

                //
                // Send the HTTP POST request and block until it completes
                //
                HttpResponseMessage response = client.SendAsync(request).Result;

                //
                // Get the resposne body from Cardtokens
                //
                responseBody = response.Content.ReadAsStringAsync().Result;

                //
                // Get the http status code from Cardtokens
                //
                statuscode = (int)response.StatusCode;

                //
                // Check if the request was successful
                //
                if (response.IsSuccessStatusCode)
                {
                    //
                    // Deserialize the JSON response into the T type
                    //
                    T res = JsonSerializer.Deserialize<T>(responseBody);
                    //
                    // set the response body from Cardtokens
                    //
                    (res as ApiResponse).responsebody = responseBody;
                    //
                    // Set the status code from Cardtokens
                    //
                    (res as ApiResponse).statuscode = statuscode;
                    //
                    // Success - return all data
                    //
                    return res;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }

            //
            // Something went wront - either communication error or serialization error
            //
            throw new HttpRequestException("API request failed with status code: " + statuscode);
        }
    }
}