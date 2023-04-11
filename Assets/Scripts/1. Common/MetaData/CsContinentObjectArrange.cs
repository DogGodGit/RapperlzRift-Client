using WebCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-18)
//---------------------------------------------------------------------------------------------------

public class CsContinentObjectArrange
{
	int m_nContinentId;
	int m_nArrangeNo;
	int m_nObjectId;
	Vector3 m_vtPosition;
	int m_nYRotationType;
	float m_flYRotation;

	//---------------------------------------------------------------------------------------------------
	public int ContinentId
	{
		get { return m_nContinentId; }
	}

	public int ArrangeNo
	{
		get { return m_nArrangeNo; }
	}

	public int ObjectId
	{
		get { return m_nObjectId; }
	}

	public Vector3 Position
	{
		get { return m_vtPosition; }
	}
	public int YRotationType
	{
		get { return m_nYRotationType; }
	}

	public float YRotation
	{
		get { return m_flYRotation; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsContinentObjectArrange(WPDContinentObjectArrange continentObjectArrange)
	{
		m_nContinentId = continentObjectArrange.continentId;
		m_nArrangeNo = continentObjectArrange.arrangeNo;
		m_nObjectId = continentObjectArrange.objectId;
		m_vtPosition = new Vector3((float)continentObjectArrange.xPosition, (float)continentObjectArrange.yPosition, (float)continentObjectArrange.zPosition);
		m_nYRotationType = continentObjectArrange.yRotationType;
		m_flYRotation = continentObjectArrange.yRotation;
	}
}
