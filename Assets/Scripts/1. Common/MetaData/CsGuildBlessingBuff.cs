using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-11)
//---------------------------------------------------------------------------------------------------

public class CsGuildBlessingBuff
{
	int m_nBuffId;
	string m_strName;
	string m_strDescription;
	float m_flExpRewardFactor;
	int m_nDuration;
	int m_nDia;

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

	public float ExpRewardFactor
	{
		get { return m_flExpRewardFactor; }
	}

	public int Duration
	{
		get { return m_nDuration; }
	}

	public int Dia
	{
		get { return m_nDia; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildBlessingBuff(WPDGuildBlessingBuff guildBlessingBuff)
	{
		m_nBuffId = guildBlessingBuff.buffId;
		m_strName = CsConfiguration.Instance.GetString(guildBlessingBuff.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(guildBlessingBuff.descriptionKey);
		m_flExpRewardFactor = guildBlessingBuff.expRewardFactor;
		m_nDuration = guildBlessingBuff.duration;
		m_nDia = guildBlessingBuff.dia;
	}
}
