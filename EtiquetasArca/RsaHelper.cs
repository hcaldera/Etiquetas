using System.Security.Cryptography;
using System.Text;

namespace EtiquetasArca
{
    internal class RsaHelper
    {
        private readonly RSACryptoServiceProvider RsaService;

        public RsaHelper()
        {
            RsaService = new RSACryptoServiceProvider
            {
                PersistKeyInCsp = false
            };
        }

        /**
         * Loads an RSA key from an XML string.
         * 
         * @param xml The XML string containing the RSA key.
         */
        public void LoadKey(string xml)
        {
            RsaService.FromXmlString(xml);
        }

        /**
         * Encrypts a message using the current RSA key and returns the encrypted data as a byte array.
         * 
         * @param message The message to encrypt.
         * @return The encrypted message as a byte array.
         */
        public byte[] EncryptMessage(string message)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                return RsaService.Encrypt(data, false);
            }
            catch
            {
                return [];
            }
        }

        /**
         * Decrypts an encrypted message using the current RSA key and returns the decrypted data as a string.
         * 
         * @param encryptedMessage The encrypted message to decrypt.
         * @return The decrypted message as a string.
         */
        public string DecryptMessage(byte[] encryptedMessage)
        {
            try
            {
                byte[] decryptedData = RsaService.Decrypt(encryptedMessage, false);
                return Encoding.UTF8.GetString(decryptedData);
            }
            catch
            {
                return string.Empty;
            }
        }

        /**
         * Signs a message using the current RSA key and returns the signature as a byte array.
         * 
         * @param message The message to sign.
         * @return The signature of the message as a byte array.
         */
        public byte[] SignMessage(string message)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                return RsaService.SignData(data, "SHA256");
            }
            catch
            {
                return [];
            }
        }

        /**
         * Verifies the signature of a message using the current RSA key.
         * 
         * @param message The original message.
         * @param signature The signature to verify.
         * @return True if the signature is valid, false otherwise.
         */
        public bool VerifySignature(string message, byte[] signature)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                return RsaService.VerifyData(data, "SHA256", signature);
            }
            catch
            {
                return false;
            }
        }
    }
}
