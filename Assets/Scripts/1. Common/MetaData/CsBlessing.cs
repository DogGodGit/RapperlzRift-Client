using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-03)
//---------------------------------------------------------------------------------------------------

public class CsBlessing
{
	int m_nBlessingId;						// 1:간단한안부, 2:유망자
	string m_strName;
	string m_strDescription;
	int m_nMoneyType;                       // 1:골드, 2:다이아
	int m_nMoneyAmount;
	CsItemReward m_csItemRewardSender;
	CsGoldReward m_csGoldRewardReceiver;

	//---------------------------------------------------------------------------------------------------
	public int BlessingId
	{
		get { return m_nBlessingId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public int MoneyType
	{
		get { return m_nMoneyType; }
	}

	public int MoneyAmount
	{
		get { return m_nMoneyAmount; }
	}

	public CsItemReward SenderItemReward
	{
		get { return m_csItemRewardSender; }
	}

	public CsGoldReward ReceiverGoldReward
	{
		get { return m_csGoldRewardReceiver; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsBlessing(WPDBlessing blessing)
	{
		m_nBlessingId = blessing.blessingId;
		m_strName = CsConfiguration.Instance.GetString(blessing.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(blessing.descriptionKey);
		m_nMoneyType = blessing.moneyType;
		m_nMoneyAmount = blessing.moneyAmount;
		m_csItemRewardSender = CsGameData.Instance.GetItemReward(blessing.senderItemRewardId);
		m_csGoldRewardReceiver = CsGameData.Instance.GetGoldReward(blessing.receiverGoldRewardId);

	}
}
