using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-12)
//---------------------------------------------------------------------------------------------------

public class CsWisdomTempleArrangePosition
{
	int m_nRow;
	int m_nCol;
	float m_flXPosition;
	float m_flYPosition;
	float m_flZPosition;
	float m_flYRotation;

	//---------------------------------------------------------------------------------------------------
	public int Row
	{
		get { return m_nRow; }
	}
	 
	public int Col
	{
		get { return m_nCol; }
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

	//---------------------------------------------------------------------------------------------------
	public CsWisdomTempleArrangePosition(WPDWisdomTempleArrangePosition wisdomTempleArrangePosition)
	{
		m_nRow = wisdomTempleArrangePosition.row;
		m_nCol = wisdomTempleArrangePosition.col;
		m_flXPosition = wisdomTempleArrangePosition.xPosition;
		m_flYPosition = wisdomTempleArrangePosition.yPosition;
		m_flZPosition = wisdomTempleArrangePosition.zPosition;
		m_flYRotation = wisdomTempleArrangePosition.yRotation;
	}
}
