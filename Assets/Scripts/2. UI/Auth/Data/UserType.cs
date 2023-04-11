using UnityEngine;
using System.Collections;

public static class UserType
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Constants

	public const int	kType_Guest		= 1;
	public const int	kType_Facebook	= 101;
	public const int	kType_Google	= 102;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Static member functions

	public static bool IsDefined(int nValue)
	{
		return nValue == kType_Guest || nValue == kType_Facebook || nValue == kType_Google;
	}
}
