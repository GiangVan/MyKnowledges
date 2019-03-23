using System.IO;
using System.Security.Cryptography;

namespace YourExperience
{
    class AdvancedEncryptionStandard
    {
        static readonly byte[] salt = new byte[] { 0, 3, 0, 4, 1, 9, 9, 8, 1, 1, 0, 7, 1, 9, 9, 8};

        public static byte[] Hash(byte[] password)
        {
            Rfc2898DeriveBytes hash = new Rfc2898DeriveBytes(password, salt, 30000);
            return hash.GetBytes(32);
        }

        public static byte[] Encoding(string plainText, byte[] password)
        {
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                // Create a decryptor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(password, salt);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream.
            return encrypted;

        }

        public static string Decoding(byte[] cipherText, byte[] password)
        {
            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            try
            {
                using (RijndaelManaged rijAlg = new RijndaelManaged())
                {
                    // Create a decryptor to perform the stream transform.
                    ICryptoTransform decryptor = rijAlg.CreateDecryptor(password, salt);

                    // Create the streams used for decryption.
                    using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {

                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                }
            }
            catch
            {
                return null;
            }
            

            return plaintext;

        }
    }
}
