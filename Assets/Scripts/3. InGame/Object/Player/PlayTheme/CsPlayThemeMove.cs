using UnityEngine;
using System;

public class CsPlayThemeMove : CsPlayTheme
{
	#region IAutoPlay

	//---------------------------------------------------------------------------------------------------
	public override EnAutoMode GetType() { return EnAutoMode.Move; }

	#endregion IAutoPlay

	//---------------------------------------------------------------------------------------------------
	protected bool MovePlayer(Vector3 vtPosition, float flRange, bool bTargetNpc)
	{
		if (Player.IsTargetInDistance(vtPosition, flRange))
		{
			return true;
		}

		Player.MoveToPos(vtPosition, flRange, bTargetNpc);
		return false;
	}
}
