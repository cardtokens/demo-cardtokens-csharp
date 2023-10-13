using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

//
// This class is used to encrypt the card holder data transmitted to Cardtokens.
//
class CardtokensEncrypt
{
    //
    // Use this function to encrypt the card holder data transmitted to Cardtokens
    // <param-name="plaintext">The JSON data to encrypt</param>
    // <param-name="publicKey">The RSA public key retrieved from the Cardtokens backoffice</param>
    //
    public static string Encrypt(string plaintext, string publicKey)
    {
        try
        {
            //
            // Create an instance of RSA
            //
            using (RSA rsa = RSA.Create())
            {
                //
                // Generate a new RSA key pair (public and private keys)
                //
                rsa.KeySize = 2048; // Adjust the key size as needed

                //
                // Convert the public key to an RSA object
                //
                using (RSA rsaPublic = RSA.Create())
                {
                    //
                    // Decode the Base64 string
                    //
                    byte[] data = Convert.FromBase64String(publicKey);

                    //
                    // Convert the byte array to a string using UTF-8 encoding
                    //
                    string pemstring = Encoding.UTF8.GetString(data);

                    //
                    // Import the RSA public key
                    //
                    rsaPublic.ImportFromPem(pemstring);

                    // The plaintext data to be encrypted
                    byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);

                    //
                    // Encrypt using RSA with either OAEP / SHA-512 or PKCS1 (the two options supported)
                    // RSAEncryptionPadding.Pkcs1
                    // RSAEncryptionPadding.OaepSHA512
                    //
                    var encryptionpadding = RSAEncryptionPadding.Pkcs1;
                    byte[] encryptedData = rsaPublic.Encrypt(plaintextBytes, encryptionpadding);

                    //
                    // Convert the encrypted data to a Base64-encoded string
                    //
                    string encryptedBase64 = Convert.ToBase64String(encryptedData);

                    //
                    // Return the encrypted base64 string 
                    //
                    return encryptedBase64;
                }
            }
        }
        catch (CryptographicException e)
        {
            //
            // On any encryption error - throw the error
            //
            Console.WriteLine("An error occurred: " + e.Message);
            throw e;
        }
    }
}
