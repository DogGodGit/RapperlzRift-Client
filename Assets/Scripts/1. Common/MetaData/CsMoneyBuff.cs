using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-22)
//---------------------------------------------------------------------------------------------------

public class CsMoneyBuff
{
	int m_nBuffId;
	string m_strName;
	string m_strDescription;
	int m_nLifetime;
	int m_nMoneyType;
	int m_nMoneyAmount;

	List<CsMoneyBuffAttr> m_listCsMoneyBuffAttr;

	//---------------------------------------------------------------------------------------------------
	public int BuffId
	{
		get { return m_nBuffId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public int Lifetime
	{
		get { return m_nLifetime; }
	}

	public int MoneyType
	{
		get { return m_nMoneyType; }
	}

	public int MoneyAmount
	{
		get { return m_nMoneyAmount; }
	}

	public List<CsMoneyBuffAttr> MoneyBuffAttrList
	{
		get { return m_listCsMoneyBuffAttr; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMoneyBuff(WPDMoneyBuff moneyBuff)
	{
		m_nBuffId = moneyBuff.buffId;
		m_strName = CsConfiguration.Instance.GetString(moneyBuff.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(moneyBuff.descriptionKey);
		m_nLifetime = moneyBuff.lifetime;
		m_nMoneyType = moneyBuff.moneyType;
		m_nMoneyAmount = moneyBuff.moneyAmount;

		m_listCsMoneyBuffAttr = new List<CsMoneyBuffAttr>();
	}
}
