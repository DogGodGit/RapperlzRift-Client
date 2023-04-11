using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Xml.Linq;

namespace PubKit
{
	public static class PKUtil
	{
		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static member functions

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
	}
}
