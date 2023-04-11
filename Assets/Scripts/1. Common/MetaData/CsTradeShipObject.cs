using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-10-01)
//---------------------------------------------------------------------------------------------------

public class CsTradeShipObject
{
	int m_nDifficulty;
	int m_nObjectId;
	long m_nMonsterArrangeId;
	float m_flXPosition;
	float m_flYPosition;
	float m_flZPosition;
	float m_flYRotation;
	int m_nActivationStepNo;
	bool m_bHitMessageDisplayable;
	int m_nHitMessageDisplayInterval;
	int m_nPoint;

	//---------------------------------------------------------------------------------------------------
	public int Difficulty
	{
		get { return m_nDifficulty; }
	}

	public int ObjectId
	{
		get { return m_nObjectId; }
	}

	public long MonsterArrangeId
	{
		get { return m_nMonsterArrangeId; }
	}

	public float XPosition
	{
		get { return m_flXPosition; }
	}

	public float YPosition
	{
		get { return m_flYPosition; }
	}

	public float ZPosition
	{
		get { return m_flZPosition; }
	}

	public float YRotation
	{
		get { return m_flYRotation; }
	}

	public int ActivationStepNo
	{
		get { return m_nActivationStepNo; }
	}

	public bool HitMessageDisplayable
	{
		get { return m_bHitMessageDisplayable; }
	}

	public int HitMessageDisplayInterval
	{
		get { return m_nHitMessageDisplayInterval; }
	}

	public int Point
	{
		get { return m_nPoint; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsTradeShipObject(WPDTradeShipObject tradeShipObject)
	{
		m_nDifficulty = tradeShipObject.difficulty;
		m_nObjectId = tradeShipObject.objectId;
		m_nMonsterArrangeId = tradeShipObject.monsterArrangeId;
		m_flXPosition = tradeShipObject.xPosition;
		m_flYPosition = tradeShipObject.yPosition;
		m_flZPosition = tradeShipObject.zPosition;
		m_flYRotation = tradeShipObject.yRotation;
		m_nActivationStepNo = tradeShipObject.activationStepNo;
		m_bHitMessageDisplayable = tradeShipObject.hitMessageDisplayable;
		m_nHitMessageDisplayInterval = tradeShipObject.hitMessageDisplayInterval;
		m_nPoint = tradeShipObject.point;
	}
}
