using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-04)
//---------------------------------------------------------------------------------------------------

public enum EnChattingType
{
	Nation = 1,
	Alliance = 2,
	World = 3,
	Party = 4,
	Guild = 5,
	OneToOne = 6,
}

public class CsChattingType
{
	int m_nChattingType;
	string m_strName;
	string m_strColorCode;

	//---------------------------------------------------------------------------------------------------
	public EnChattingType ChattingType
	{
		get { return (EnChattingType)m_nChattingType; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string ColorCode
	{
		get { return m_strColorCode; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsChattingType(WPDChattingType chattingType)
	{
		m_nChattingType = chattingType.chattingType;
		m_strName = CsConfiguration.Instance.GetString(chattingType.nameKey);
		m_strColorCode = chattingType.colorCode;
	}
}
