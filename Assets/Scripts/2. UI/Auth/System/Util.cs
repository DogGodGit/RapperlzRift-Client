using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Security.Cryptography;
using Unity.IO.Compression;

public static class Util
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Static member functions

	public static T CreateComponent<T>() where T : Component
	{
		return CreateComponent<T>(typeof(T).Name);
	}

	public static T CreateComponent<T>(string sName) where T : Component
	{
		GameObject gameObject = new GameObject();
		gameObject.name = sName;
		
		return gameObject.AddComponent<T>();
	}

	public static T Instantiate<T>(GameObject source) where T : Component
	{
		return ((GameObject)GameObject.Instantiate(source)).GetComponent<T>();
	}

	public static XElement LoadXml(string sXml)
	{
		XElement xml = null;

		using (StringReader reader = new StringReader(sXml))
		{
			xml = XElement.Load(reader);
		}
			
		return xml;
	}

	public static void Log(object message)
	{
		Debug.Log(message);
	}

	public static void LogError(object message)
	{
		Debug.LogError(message);
	}

	public static void LogWarning(object message)
	{
		Debug.LogWarning(message);
	}

	public static void LogException(Exception exception)
	{
		Debug.LogException(exception);
	}

	public static string ToString(Exception ex)
	{
		if (ex == null)
			return null;

		StringBuilder sb = new StringBuilder();

		sb.AppendFormat("[{0}: {1}]", ex.GetType().ToString(), ex.Message);

		sb.Append(Environment.NewLine);
		sb.AppendFormat("# StackTrace");
		sb.Append(Environment.NewLine);
		sb.Append(ex.StackTrace);

		if (ex.InnerException != null)
		{
			sb.Append(Environment.NewLine);
			sb.Append(ToString(ex.InnerException));
		}

		return sb.ToString();
	}

	public static bool TryParseGuid(string sText, out Guid result)
	{
		result = Guid.Empty;

		try
		{
			result = new Guid(sText);
		}
		catch (Exception)
		{
			return false;
		}

		return true;
	}

	public static string GetHashValue(string sMessage, string sType)
	{
		RijndaelManaged rm = new RijndaelManaged ();
		rm.Mode = CipherMode.CBC;
		rm.Padding = PaddingMode.PKCS7;
		rm.KeySize = 128;
		rm.BlockSize = 128;

		byte[] sourceBytes = Encoding.UTF8.GetBytes ("dpdldldptm!@#");
		byte[] destinationBytes = new byte[16];

		int len = sourceBytes.Length;

		if (len > destinationBytes.Length)
			len = destinationBytes.Length;

		Array.Copy (sourceBytes, destinationBytes, len);
		rm.Key = destinationBytes;
		rm.IV = destinationBytes;

		ICryptoTransform transform = rm.CreateEncryptor ();

		byte[] plainText = sType.Equals ("E") ? Encoding.UTF8.GetBytes (sMessage) : rm.CreateDecryptor ().TransformFinalBlock (Convert.FromBase64String (sMessage), 0, Convert.FromBase64String (sMessage).Length);

		return sType.Equals ("E") ? Convert.ToBase64String (transform.TransformFinalBlock (plainText, 0, plainText.Length)) : Encoding.UTF8.GetString (plainText);
	}

	public static string UnZipFromBase64(string Compressedvalue)
	{
		byte[] byteArray = Convert.FromBase64String(Compressedvalue);

		MemoryStream stream = new MemoryStream(byteArray);
		Stream sourceStream = stream;

		if (sourceStream == null)
		{
			throw new ArgumentException();
		}

		if (!sourceStream.CanRead)
		{
			throw new ArgumentException();
		}

		MemoryStream memoryStream = new MemoryStream();
		const int bufferSize = 31457280;   // 30MB

		using (GZipStream gzipStream = new GZipStream(sourceStream, CompressionMode.Decompress))
		{
			byte[] buffer = new byte[bufferSize];

			int bytesRead = 0;

			do
			{
				bytesRead = gzipStream.Read(buffer, 0, bufferSize);
			}
			while (bytesRead == bufferSize);

			memoryStream.Write(buffer, 0, bytesRead);
		}
		byte[] btArraymemoryStream = memoryStream.ToArray();

		stream.Close();
		stream.Dispose();
		sourceStream.Close();
		sourceStream.Dispose();
		memoryStream.Close();
		memoryStream.Dispose();

		return Encoding.UTF8.GetString(btArraymemoryStream);
	}
}
