using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-03)
//---------------------------------------------------------------------------------------------------

public class CsHeroBlessing
{
	long m_lInstanceId;
	CsBlessing m_csBlessing;
	CsBlessingTargetLevel m_csBlessingTargetLevel;
	Guid m_guidSenderHeroId;
	string m_strSenderName;

	//---------------------------------------------------------------------------------------------------
	public long InstanceId
	{
		get { return m_lInstanceId; }
	}

	public CsBlessing Blessing
	{
		get { return m_csBlessing; }
	}

	public CsBlessingTargetLevel BlessingTargetLevel
	{
		get { return m_csBlessingTargetLevel; }
	}

	public Guid SenderHeroId
	{
		get { return m_guidSenderHeroId; }
	}

	public string SenderName
	{
		get { return m_strSenderName; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroBlessing(PDHeroBlessing heroBlessing)
	{
		m_lInstanceId = heroBlessing.instanceId;
		m_csBlessing = CsGameData.Instance.GetBlessing(heroBlessing.blessingId);
		m_csBlessingTargetLevel = CsGameData.Instance.GetBlessingTargetLevel(heroBlessing.blessingTargetLevelId);
		m_guidSenderHeroId = heroBlessing.senderHeroId;
		m_strSenderName = heroBlessing.senderName;
	}
}
