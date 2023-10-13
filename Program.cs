using System;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

class Program
{
    //
    // The merchant created within the Cardtokens account
    //
    const string MERCHANTID = "523ca9d5eb9d4ce0a60b2a3f5eb3119d";

    //
    // The API key required to access the Cardtokens API
    //
    const string APIKEY = "95f734793a424ea4ae8d9dc0b8c1a4d7";

    //
    // The host of the Cardtokens API
    //
    const string HOST = "https://api.cardtokens.io";
    
    //
    // Public key used to encrypt the PAN
    //
    const string PUBLIC_KEY = "LS0tLS1CRUdJTiBQVUJMSUMgS0VZLS0tLS0NCk1JSUNJakFOQmdrcWhraUc5dzBCQVFFRkFBT0NBZzhBTUlJQ0NnS0NBZ0VBMW1zdE1QckZSVmQ4VE1HclkzMjQNCjJwcTQ2aFlFMFBieXcrTnB0MnRDSjBpRHkrWkxQWWJGMnVYTkg1UE9neGkzdDVIVTY2MVVTQThYOXp5N2pJTzANCjlpOGxRMkdoN1dpejlqZXpFVDBpVmNvUGovSFFrV1N1KzA5Y0RIUk5qUDJoaWtIWUEwOUlZc05vemo3eHR2ME4NCnJxbjZacWZ5amhOS1NrN2RUeUVVQ0xoaEwvTUVFRTZ0QUREVVJZb0tIVXFrVml0cFlzcE1HamlKNkFBSVlVZWENCk1DdkZ2cnhaSkFNSW5FbnY3THNhTHVBV21pdzRrOXM5M0x1MXdoM3A1bjR1a09pVWpRWEZ5Nm9NNzMwblpvb1MNCmR2U2lYUlR2UlFwMDkyZDAzbnY5Zk55cWgwM3ZoM2l5TFJja3RoVnc2ZklPN3p4cktjTXpoVmhzK3doUGVtMzkNCkRhU05oSjFrZUx4bzcyaDJIL01FMzRuQzNOSUhCUEhQZ1NBeHVDSjlCcXVVRW1idXdGMTc0eDlGOUhFYm5jRlkNClRTd1hmS3diN1cxZ0F1U1RlWmhKVXc1eDZ6a3ZUTmRTejRWaFFjT051SjJ6am1VdGdSK3FXc1NjOUh2N1RGREgNCjlQbCt5NmQxeVJ0Rmp2TmlqeGZQUmo5a1dKbVJvcnBVVExUMTh2dThlbzg1aWNLTVY1VmladDMweGxpc1RVTjANCjJOWkxjNG83TVdraHE1eGhGcXhmZDdTZXZEc1FLa0VpenlRbi9zOUpZNmsybEtQUG4wTXk1UjdURWtBZEhVREUNCklIc09qTXlrZnpwYVdoNldMK2RmRlRFVzE4MFNkRHdXbEFXaWtpYWhFT1NDRGVFMkpWTDluMjY3QzJkc0ZJZDYNCjVPczJKVjE5anl5b2VGQkhOQm11MFBjQ0F3RUFBUT09DQotLS0tLUVORCBQVUJMSUMgS0VZLS0tLS0NCg==";
    
    static void Main()
    {
        try
        {
            //
            // The Visa test PAN to use
            //
            var card_visa_test = new
            {
                pan = "4111111145551142",
                expmonth = 6,
                expyear = 2029,
                securitycode = "000"
            };

            //
            // The string to be encrypted
            //
            string jsonString = JsonSerializer.Serialize(card_visa_test);

            //
            // Convert the encrypted data to a Base64-encoded string
            //
            string encryptedText = CardtokensEncrypt.Encrypt(jsonString, PUBLIC_KEY);

            //
            // The create token request object. The enccard contains the encrypted PAN
            //
            var requestobj = new
            {
                enccard = encryptedText,
                clientwalletaccountemailaddress = "noreply@cardtokens.io",
                merchantid = MERCHANTID
            };

            //
            // The crete token JSON formatted request
            //
            string payload = JsonSerializer.Serialize(requestobj);

            //
            // Request Cardtokens aget the response into a CreateTokenResponse object
            //
            CreateTokenResponse tokenresponse = (CreateTokenResponse)Cardtokenshttp.Tx<CreateTokenResponse>(HttpMethod.Post, payload, HOST + "/api/token", APIKEY);

            //
            // If Cardtokens replies by success
            //
            if (tokenresponse.statuscode == 200)
            {
                Console.WriteLine("Token created by network token id " + tokenresponse.token);
                //
                // Create a cryptogram request object 
                //
                var cryptogramrequest = new
                {
                    reference = "cardtokensref",
                    unpredictablenumber = "99887766"
                };

                //
                // By the cryptogram request request Cardtokens including which token to request cryptogram for
                //
                CreateCryptogramResponse cryptoResponse = (CreateCryptogramResponse)Cardtokenshttp.Tx<CreateCryptogramResponse>(HttpMethod.Post, payload, HOST + "/api/token/" + tokenresponse.tokenid + "/cryptogram", APIKEY);

                //
                // If Cardtokens replies by success
                //
                if (cryptoResponse.statuscode == 200)
                {
                    Console.WriteLine("Cryptogram created by value: " + cryptoResponse.cryptogram);

                    //
                    // Now get status of the token by requesting the token status API
                    //
                    GetStatusResponse statusResponse = (GetStatusResponse)Cardtokenshttp.Tx<GetStatusResponse>(HttpMethod.Get, "", HOST + "/api/token/" + tokenresponse.tokenid + "/status", APIKEY);

                    //
                    // If Cardtokens replices by success
                    //
                    if (statusResponse.statuscode == 200)
                    {
                        Console.WriteLine("Token status: " + statusResponse.status);

                        //
                        // Finally create a token delete request
                        //
                        ApiResponse deleteResponse = (ApiResponse)Cardtokenshttp.Tx<ApiResponse>(HttpMethod.Delete, "", HOST + "/api/token/" + tokenresponse.tokenid + "/delete", APIKEY);

                        //
                        // If Cardtokens replies by success
                        //
                        if (deleteResponse.statuscode == 200)
                        {
                            Console.WriteLine("Token deleted..");

                            //
                            // Now get status of the token again by requesting the token status API
                            //
                            statusResponse = (GetStatusResponse)Cardtokenshttp.Tx<GetStatusResponse>(HttpMethod.Get, "", HOST + "/api/token/" + tokenresponse.tokenid + "/status", APIKEY);

                            //
                            // If Cardtokens replices by success
                            //
                            if (statusResponse.statuscode == 200)
                            {
                                Console.WriteLine("Token status: " + statusResponse.status);
                            }
                        }
                    }
                }
            }
        }
        catch (CryptographicException e)
        {
            Console.WriteLine("An error occurred: " + e.Message);
        }
    }

    

    
}
