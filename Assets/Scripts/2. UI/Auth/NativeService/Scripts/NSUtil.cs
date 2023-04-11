using UnityEngine;
using System;
using System.Collections;
using System.Text;

namespace NativeService
{
	public static class NSUtil
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
	}
}
