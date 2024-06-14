using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;

namespace SR_Editor.Core.Utility
{
	public class UtilLisans
	{
		private const string hashAlgorithm = "MD5";

		private const int passwordIterations = 2;

		private const int keySize = 256;

		private static string passPhrase;

		private static string saltValue;

		private static string initVector;

		static UtilLisans()
		{
			UtilLisans.passPhrase = "P@se";
			UtilLisans.saltValue = "@se7@Pass3";
			UtilLisans.initVector = "@1B7c3D4e5F6g7H8";
		}

		public UtilLisans()
		{
		}

		private static string Decrypt(string cipherText)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(UtilLisans.initVector);
			byte[] numArray = Encoding.ASCII.GetBytes(UtilLisans.saltValue);
			byte[] numArray1 = Convert.FromBase64String(cipherText);
			PasswordDeriveBytes passwordDeriveByte = new PasswordDeriveBytes(UtilLisans.passPhrase, numArray, "MD5", 2);
			byte[] bytes1 = passwordDeriveByte.GetBytes(32);
			RijndaelManaged rijndaelManaged = new RijndaelManaged()
			{
				Mode = CipherMode.CBC
			};
			ICryptoTransform cryptoTransform = rijndaelManaged.CreateDecryptor(bytes1, bytes);
			MemoryStream memoryStream = new MemoryStream(numArray1);
			CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read);
			byte[] numArray2 = new byte[(int)numArray1.Length];
			int num = cryptoStream.Read(numArray2, 0, (int)numArray2.Length);
			memoryStream.Close();
			cryptoStream.Close();
			return Encoding.UTF8.GetString(numArray2, 0, num);
		}

		private static string Encrypt(string plainText)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(UtilLisans.initVector);
			byte[] numArray = Encoding.ASCII.GetBytes(UtilLisans.saltValue);
			byte[] bytes1 = Encoding.UTF8.GetBytes(plainText);
			PasswordDeriveBytes passwordDeriveByte = new PasswordDeriveBytes(UtilLisans.passPhrase, numArray, "MD5", 2);
			byte[] numArray1 = passwordDeriveByte.GetBytes(32);
			RijndaelManaged rijndaelManaged = new RijndaelManaged()
			{
				Mode = CipherMode.CBC
			};
			ICryptoTransform cryptoTransform = rijndaelManaged.CreateEncryptor(numArray1, bytes);
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
			cryptoStream.Write(bytes1, 0, (int)bytes1.Length);
			cryptoStream.FlushFinalBlock();
			byte[] array = memoryStream.ToArray();
			memoryStream.Close();
			cryptoStream.Close();
			return Convert.ToBase64String(array);
		}
        
		private static T JsonDeserialize<T>(string jsonString)
		{
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));
			MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
			return (T)dataContractJsonSerializer.ReadObject(memoryStream);
		}

		private static string JsonSerializer<T>(T t)
		{
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));
			MemoryStream memoryStream = new MemoryStream();
			dataContractJsonSerializer.WriteObject(memoryStream, t);
			string str = Encoding.UTF8.GetString(memoryStream.ToArray());
			memoryStream.Close();
			return str;
		}
        
	}
}