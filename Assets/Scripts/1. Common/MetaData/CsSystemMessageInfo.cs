using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-10-08)
//---------------------------------------------------------------------------------------------------

public enum EnSystemMessage
{
	MainGearAcquirement = 1,
	MainGearEnchantment = 2,
	CreatureCardAcquirement = 3,
	CreatureAcquirement = 4,
	CreatureInjection = 5,
	CostumeEnchantment = 6,
}

public class CsSystemMessageInfo
{
	/*
	1 : 메인장비획득
	- 조건값 : 등급(이상)

	2 : 메인장비강화
	- 조건값 : 강화레벨

	3 : 크리처카드획득
	- 조건값 : 크리처카드등급(이상)

	4 : 크리처획득
	- 조건값 : 크리처등급(이상)

	5 : 크리처주입
	- 조건값 : 주입레벨

	6 : 코스튬강화
	- 조건값 : 강화레벨
	*/
	int m_nMessageId;
	string m_strMessage;

	//---------------------------------------------------------------------------------------------------
	public int MessageId
	{
		get { return m_nMessageId; }
	}

	public EnSystemMessage SystemMessageId
	{
		get { return (EnSystemMessage)m_nMessageId; }
	}

	public string Message
	{
		get { return m_strMessage; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSystemMessageInfo(WPDSystemMessage systemMessage)
	{
		m_nMessageId = systemMessage.messageId;
		m_strMessage = CsConfiguration.Instance.GetString(systemMessage.messageKey);
	}

}
