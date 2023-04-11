using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------------------------------------------------
//작성: 최민수 (2018-10-05)
//-------------------------------------------------------------------------------------------------------

public class CsParentCategory : MonoBehaviour 
{
	int[] m_anChildCategoryId;
	int m_nMainCategoryId;

	bool m_bIsSelect;
	
	//---------------------------------------------------------------------------------------------------
	public int[] ChildCategoryId
	{
		get { return m_anChildCategoryId; }
		set { m_anChildCategoryId = value; }
	}

	public int MainCategoryId
	{
		get { return m_nMainCategoryId; }
		set { m_nMainCategoryId = value; }
	}

	public bool IsSelect
	{
		get { return m_bIsSelect; }
		set { m_bIsSelect = value; }
	}
}
