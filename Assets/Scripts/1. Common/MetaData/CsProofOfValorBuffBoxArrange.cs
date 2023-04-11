using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-05-03)
//---------------------------------------------------------------------------------------------------

public class CsProofOfValorBuffBoxArrange
{
	int m_nArrangeId;
	int m_nBuffBoxId;
	float m_flXPosition;
	float m_flYPosition;
	float m_flZPosition;
	float m_flRadius;
	int m_nYRotationType;
	float m_flYRotation;
	float m_flAcquisitionRange;

	//---------------------------------------------------------------------------------------------------
	public int ArrangeId
	{
		get { return m_nArrangeId; }
	}

	public int BuffBoxId
	{
		get { return m_nBuffBoxId; }
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

	public float Radius
	{
		get { return m_flRadius; }
	}

	public int YRotationType
	{
		get { return m_nYRotationType; }
	}

	public float YRotation
	{
		get { return m_flYRotation; }
	}

	public float AcquisitionRange
	{
		get { return m_flAcquisitionRange; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsProofOfValorBuffBoxArrange(WPDProofOfValorBuffBoxArrange proofOfValorBuffBoxArrange)
	{
		m_nArrangeId = proofOfValorBuffBoxArrange.arrangeId;
		m_nBuffBoxId = proofOfValorBuffBoxArrange.buffBoxId;
		m_flXPosition = proofOfValorBuffBoxArrange.xPosition;
		m_flYPosition = proofOfValorBuffBoxArrange.yPosition;
		m_flZPosition = proofOfValorBuffBoxArrange.zPosition;
		m_flRadius = proofOfValorBuffBoxArrange.radius;
		m_nYRotationType = proofOfValorBuffBoxArrange.yRotationType;
		m_flYRotation = proofOfValorBuffBoxArrange.yRotation;
		m_flAcquisitionRange = proofOfValorBuffBoxArrange.acquisitionRange;
	}
}
