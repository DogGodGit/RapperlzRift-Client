using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml.Linq;

namespace PubKit
{
	public static class PKResourceUtil
	{
		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constants
		
		//
		// Root folder
		//
		
		public const string	kFolder_Root		= "PubKit";

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static member functions

		//
		// Config
		//

		public static XElement LoadConfig(string sName)
		{
			return LoadXml(string.Format("{0}/{1}", kFolder_Root, sName));
		}

		//
		// Utilities
		//

		public static XElement LoadXml(string sName)
		{
			TextAsset xmlAsset = Resources.Load<TextAsset>(sName);
			
			if (xmlAsset == null)
				return null;
			
			return PKUtil.LoadXml(xmlAsset.text);
		}
	}
}
