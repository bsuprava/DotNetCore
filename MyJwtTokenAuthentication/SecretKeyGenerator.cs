using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyJwtTokenAuthentication
{
    /*
     How to Create a Secret Key
    Manually: Use an online tool like RandomKeygen.
    Programmatically: Generate it in code or through the CLI.
    Environment Variables: Store it securely in environment variables.

    When to Use Each Approach
        i. ECDiffieHellman (Asymmetric Key):
        Used when you need public-private key cryptography for signing and verifying.
        Provides a higher level of security for distributed systems.
        
        ii. Random Symmetric Key:
        Used for simpler scenarios like HMAC-based JWT tokens (HS256).
        The same key is shared between the issuer and validator.
     
     */
    public static class SecretKeyGenerator
    {
        /*
         Warning (active)	SYSLIB0023	'RNGCryptoServiceProvider' is obsolete: 'RNGCryptoServiceProvider is obsolete. 
        To generate a random number, use one of the RandomNumberGenerator static methods instead.'	MyJwtTokenAuthentication

         */
        public static string GenerateSecretKey()
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                const int keySize = 32; // 256-bit key
                byte[] randomBytes = new byte[keySize]; 
                randomNumberGenerator.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);//Convert the binary key to a Base64 string for easy storage and transmission
            }
        }

        public static string GenerateCngSecretKey()
        {           
            //Warning(active)    CA1416 Using platform dependent API on a component makes the code no longer work across all platforms.
            //MyJwtTokenAuthentication D:\GIT\DotNet\DotNetCore\MyJwtTokenAuthentication\SecretKeyGenerator.cs 42

            // Use CngKey to generate a random key.it supports only on windows
            using (var cngKey = CngKey.Create(CngAlgorithm.ECDiffieHellmanP256)) //Creates a cryptographic key using the CngAlgorithm specified (e.g., ECDiffieHellmanP256).
            {
                // Export the key in binary format and encode as Base64
                var keyBytes = cngKey.Export(CngKeyBlobFormat.EccPrivateBlob);//The generated key is exported in a binary format using CngKeyBlobFormat.EccPrivateBlob
                return Convert.ToBase64String(keyBytes);//Convert the binary key to a Base64 string for easy storage and transmission
            }
        }
    }
}
