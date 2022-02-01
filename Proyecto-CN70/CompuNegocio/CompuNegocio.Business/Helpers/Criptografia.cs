using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Helpers
{
    public class Criptografia
    {
        /// <summary>
        /// Encripta una cadena de texto utilizando como llave la fecha del dia
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string Cipher(string text)
        {
            try
            {
                //MT: UTC to ve used with API v3
                return Cipher(text, DateTime.Now.ToUniversalTime().ToString("MM/dd/yyy"));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Decifra una cadena de texto utilizando como llave la fecha del dia
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string Decipher(string text)
        {
            try
            {
                //MT: UTC to ve used with API v3
                return Decipher(text, DateTime.Now.ToUniversalTime().ToString("MM/dd/yyy"));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Encripta una cadena de texto a partir de la llave específicada
        /// </summary>
        /// <param name="text">Cadena de texto a encriptar</param>
        /// <param name="key">Llave con la que se encriptará la cadena de texto</param>
        /// <returns>Cadena de texto encirptada</returns>
        public string Cipher(string text, string key)
        {
            byte[] keyArray;
            byte[] toEncryptArray;
            MD5CryptoServiceProvider hashmd5;
            TripleDESCryptoServiceProvider tdes;
            byte[] resultArray;

            toEncryptArray = UTF8Encoding.UTF8.GetBytes(text);
            hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();

            tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// Desencripta una cadena de texto a partir de su llave a texto plano
        /// </summary>
        /// <param name="text">Cadena de texto encriptado</param>
        /// <param name="key">Llave con la que fue encriptada la cadena de texto</param>
        /// <returns>Cadena de texto desencriptada</returns>
        public string Decipher(string text, string key)
        {
            byte[] keyArray;
            byte[] toEncryptArray;

            MD5CryptoServiceProvider hashmd5;
            TripleDESCryptoServiceProvider tdes;
            ICryptoTransform cTransform;
            byte[] resultArray;

            toEncryptArray = Convert.FromBase64String(text);

            hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();

            tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;
            cTransform = tdes.CreateDecryptor();
            try
            {
                resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            }
            catch (CryptographicException)
            {
                throw;
            }
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// Convierte una cadena de texto plano (UTF) a su equivalente hexadecimal
        /// utiliza el ejemplo : https://msdn.microsoft.com/en-us/library/bb311038.aspx
        /// </summary>
        /// <param name="utfValue">Cadena en utf a convertir</param>
        /// <returns>Cadenta Hexadecimal</returns>
        public string UtfToHex(string utfValue)
        {
            try
            {
                string hex = string.Empty;

                foreach (char letter in utfValue.ToCharArray())
                {
                    hex += String.Format("{0:X}", Convert.ToInt32(letter));
                }
                return hex;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Convierte una cadena hexadecimal a su equivalente utf
        /// utilizada el ejemplo : https://msdn.microsoft.com/en-us/library/bb311038.aspx
        /// </summary>
        /// <param name="hexValue">Cadena hexadecimal a convertir</param>
        /// <returns>Cadena utf</returns>
        public string HexToUtf(string hexValue)
        {
            try
            {
                string utf = string.Empty;
                List<string> hexPairs;

                hexPairs = new List<string>();
                for (int i = 0; i < hexValue.Count(); i += 2)
                {
                    hexPairs.Add(hexValue.Substring(i, 2));
                }

                foreach (string hex in hexPairs)
                {
                    utf += Char.ConvertFromUtf32(Convert.ToInt32(hex, 16));
                }

                return utf;

            }
            catch (Exception)
            {
                throw;
            }
        }

        private string RSAEncrypt(string text, string key)
        {
            byte[] plaintext = Encoding.Unicode.GetBytes(text);

            CspParameters cspParams = new CspParameters();
            cspParams.KeyContainerName = key;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(2048, cspParams))
            {
                byte[] encryptedData = RSA.Encrypt(plaintext, false);
                return Convert.ToBase64String(encryptedData);
            }
        }

        private string RSADecrypt(string text, string key)
        {
            byte[] encryptedData = Convert.FromBase64String(text);

            CspParameters cspParams = new CspParameters();
            cspParams.KeyContainerName = key;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(2048, cspParams))
            {
                byte[] decryptedData = RSA.Decrypt(encryptedData, false);
                return Encoding.Unicode.GetString(decryptedData);
            }
        }


        #region Certificate Functions

        /// <summary>
        /// Reads a certificate file
        /// </summary>
        /// <param name="certificateFile">Certificate physical file (*.cer)</param>
        /// <returns>Certificate as UTF8 string</returns>
        public string CerToPem(string certificateFile)
        {
            X509Certificate x509Certificate;
            StringBuilder pemCertificate;

            x509Certificate = new X509Certificate(certificateFile);
            pemCertificate = new StringBuilder();
            pemCertificate.AppendLine("-----BEGIN CERTIFICATE-----");
            pemCertificate.AppendLine(Convert.ToBase64String(x509Certificate.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks));
            pemCertificate.AppendLine("-----END CERTIFICATE-----");

            return pemCertificate.ToString();

        }

        /// <summary>
        /// Reads an encrypted private key file
        /// </summary>
        /// <param name="keyFile">Private key physical file (*.key)</param>
        /// <param name="password">Password to open the key file</param>
        /// <returns>Private key as UTF8 string</returns>
        public string KeyToPem(string keyFile, string password)
        {
            byte[] keyBytes;
            AsymmetricKeyParameter key;
            PemWriter pemWriter;
            StringWriter strWriter;

            //Cargo el archivo en arreglo de bytes
            keyBytes = File.ReadAllBytes(keyFile);

            //Lo abro utilizado el arreglo de bytes y la clave
            key = PrivateKeyFactory.DecryptKey(password.ToCharArray(), keyBytes);

            //Inicializo un stringWriter que será usado por el PemWriter
            strWriter = new StringWriter();

            //Inicializo el pemWriter pasandole el string writer para que lo utilice
            pemWriter = new PemWriter(strWriter);

            //Hago que el pemWriter me escriba la llave abierta en el stringWriter que le pase
            pemWriter.WriteObject(key);

            //Cierro la escritura del stringWriter para poder leerlo
            strWriter.Close();

            //Regreso lo que se haya escrito, en un simple string
            return strWriter.ToString();
        }

        /// <summary>
        /// Initialices a X509 Certificate from a certificate and private key on PEM format
        /// </summary>
        /// <param name="pemCertificate">Certificate in PEM format</param>
        /// <param name="pemKey">Private Key in PEM format</param>
        /// <returns>Certificate with a private key</returns>
        public X509Certificate2 GetCertificateFromPEM(string pemCertificate, string pemKey)
        {
            byte[] certBuffer = GetBytesFromPEM(pemCertificate, FileType.Certificate);
            byte[] keyBuffer = GetBytesFromPEM(pemKey, FileType.RsaPrivateKey);

            X509Certificate2 certificate = new X509Certificate2(certBuffer);

            RSACryptoServiceProvider prov = DecodeRsaPrivateKey(keyBuffer);
            certificate.PrivateKey = prov;

            return certificate;
        }

        #endregion

        #region Private utilities

        /// <summary>
        /// Converts a PEM file to a byte array
        /// </summary>
        /// <param name="pemString">PEM file in string UTF8 format</param>
        /// <param name="type">PEM fileType</param>
        /// <returns>Byte array equivalent to the pem file</returns>
        private byte[] GetBytesFromPEM(string pemString, FileType type)
        {
            string header; string footer;

            switch (type)
            {
                case FileType.Certificate:
                    header = "-----BEGIN CERTIFICATE-----";
                    footer = "-----END CERTIFICATE-----";
                    break;
                case FileType.RsaPrivateKey:
                    header = "-----BEGIN RSA PRIVATE KEY-----";
                    footer = "-----END RSA PRIVATE KEY-----";
                    break;
                default:
                    return null;
            }

            int start = pemString.IndexOf(header) + header.Length;
            int end = pemString.IndexOf(footer, start) - start;
            return Convert.FromBase64String(pemString.Substring(start, end));
        }

        /// <summary>
        /// It parses an RSA private key using the ASN.1 format
        /// </summary>
        /// <param name="privateKeyBytes">Byte array containing PEM string of private key.</param>
        /// <returns>An instance of <see cref="RSACryptoServiceProvider"/> rapresenting the requested private key.
        /// Null if method fails on retriving the key.</returns>
        private RSACryptoServiceProvider DecodeRsaPrivateKey(byte[] privateKeyBytes)
        {
            MemoryStream ms = new MemoryStream(privateKeyBytes);
            BinaryReader rd = new BinaryReader(ms);

            try
            {
                byte byteValue;
                ushort shortValue;

                shortValue = rd.ReadUInt16();

                switch (shortValue)
                {
                    case 0x8130:
                        // If true, data is little endian since the proper logical seq is 0x30 0x81
                        rd.ReadByte(); //advance 1 byte
                        break;
                    case 0x8230:
                        rd.ReadInt16();  //advance 2 bytes
                        break;
                    default:
                        Debug.Assert(false);     // Improper ASN.1 format
                        return null;
                }

                shortValue = rd.ReadUInt16();
                if (shortValue != 0x0102) // (version number)
                {
                    Debug.Assert(false);     // Improper ASN.1 format, unexpected version number
                    return null;
                }

                byteValue = rd.ReadByte();
                if (byteValue != 0x00)
                {
                    Debug.Assert(false);     // Improper ASN.1 format
                    return null;
                }

                // The data following the version will be the ASN.1 data itself, which in our case
                // are a sequence of integers.

                // In order to solve a problem with instancing RSACryptoServiceProvider
                // via default constructor on .net 4.0 this is a hack
                CspParameters parms = new CspParameters();
                parms.Flags = CspProviderFlags.NoFlags;
                parms.KeyContainerName = Guid.NewGuid().ToString().ToUpperInvariant();
                parms.ProviderType = ((Environment.OSVersion.Version.Major > 5) || ((Environment.OSVersion.Version.Major == 5) && (Environment.OSVersion.Version.Minor >= 1))) ? 0x18 : 1;

                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(parms);
                RSAParameters rsAparams = new RSAParameters();

                rsAparams.Modulus = rd.ReadBytes(DecodeIntegerSize(rd));

                // Argh, this is a pain.  From emperical testing it appears to be that RSAParameters doesn't like byte buffers that
                // have their leading zeros removed.  The RFC doesn't address this area that I can see, so it's hard to say that this
                // is a bug, but it sure would be helpful if it allowed that. So, there's some extra code here that knows what the
                // sizes of the various components are supposed to be.  Using these sizes we can ensure the buffer sizes are exactly
                // what the RSAParameters expect.  Thanks, Microsoft.
                RSAParameterTraits traits = new RSAParameterTraits(rsAparams.Modulus.Length * 8);

                rsAparams.Modulus = AlignBytes(rsAparams.Modulus, traits.size_Mod);
                rsAparams.Exponent = AlignBytes(rd.ReadBytes(DecodeIntegerSize(rd)), traits.size_Exp);
                rsAparams.D = AlignBytes(rd.ReadBytes(DecodeIntegerSize(rd)), traits.size_D);
                rsAparams.P = AlignBytes(rd.ReadBytes(DecodeIntegerSize(rd)), traits.size_P);
                rsAparams.Q = AlignBytes(rd.ReadBytes(DecodeIntegerSize(rd)), traits.size_Q);
                rsAparams.DP = AlignBytes(rd.ReadBytes(DecodeIntegerSize(rd)), traits.size_DP);
                rsAparams.DQ = AlignBytes(rd.ReadBytes(DecodeIntegerSize(rd)), traits.size_DQ);
                rsAparams.InverseQ = AlignBytes(rd.ReadBytes(DecodeIntegerSize(rd)), traits.size_InvQ);

                rsa.ImportParameters(rsAparams);
                return rsa;
            }
            catch (Exception)
            {
                Debug.Assert(false);
                return null;
            }
            finally
            {
                rd.Close();
            }
        }

        /// <summary>
        /// Enumeration of FileTypes that interact within the class
        /// </summary>
        private enum FileType
        {
            Certificate,
            RsaPrivateKey
        }

        /// <summary>
        /// Makes sure every portion complies with the ASN.1 standar size
        /// </summary>
        /// <param name="inputBytes">Byte array to align</param>
        /// <param name="alignSize">Expected size</param>
        /// <returns>Byte array aligned</returns>
        private byte[] AlignBytes(byte[] inputBytes, int alignSize)
        {
            int inputBytesSize = inputBytes.Length;

            if ((alignSize != -1) && (inputBytesSize < alignSize))
            {
                byte[] buf = new byte[alignSize];
                for (int i = 0; i < inputBytesSize; ++i)
                {
                    buf[i + (alignSize - inputBytesSize)] = inputBytes[i];
                }
                return buf;
            }
            else
            {
                return inputBytes;      // Already aligned, or doesn't need alignment
            }
        }

        /// <summary>
        /// This helper function parses an integer size from the reader using the ASN.1 format
        /// </summary>
        /// <param name="rd"></param>
        /// <returns></returns>
        private int DecodeIntegerSize(BinaryReader rd)
        {
            byte byteValue;
            int count;

            byteValue = rd.ReadByte();
            if (byteValue != 0x02)        // indicates an ASN.1 integer value follows
                return 0;

            byteValue = rd.ReadByte();
            if (byteValue == 0x81)
            {
                count = rd.ReadByte();    // data size is the following byte
            }
            else if (byteValue == 0x82)
            {
                byte hi = rd.ReadByte();  // data size in next 2 bytes
                byte lo = rd.ReadByte();
                count = BitConverter.ToUInt16(new[] { lo, hi }, 0);
            }
            else
            {
                count = byteValue;        // we already have the data size
            }

            //remove high order zeros in data
            while (rd.ReadByte() == 0x00)
            {
                count -= 1;
            }
            rd.BaseStream.Seek(-1, System.IO.SeekOrigin.Current);

            return count;
        }

        #endregion
    }
}
