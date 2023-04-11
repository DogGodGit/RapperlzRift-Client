using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-05-03)
//---------------------------------------------------------------------------------------------------

public class CsProofOfValorBossMonsterArrange
{
	int m_nProofOfValorBossMonsterArrangeId;
	bool m_bIsSpecial;
	int m_nStarGrade;
	CsMonsterArrange m_csMonsterArrange;
	string m_strDescription;
	int m_nRewardSoulPowder;
	int m_nSpecialRewardSoulPowder;

	//---------------------------------------------------------------------------------------------------
	public int ProofOfValorBossMonsterArrangeId
	{
		get { return m_nProofOfValorBossMonsterArrangeId; }
	}

	public bool IsSpecial
	{
		get { return m_bIsSpecial; }
	}

	public int StarGrade
	{
		get { return m_nStarGrade; }
	}

	public CsMonsterArrange MonsterArrange
	{
		get { return m_csMonsterArrange; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public int RewardSoulPowder
	{
		get { return m_nRewardSoulPowder; }
	}

	public int SpecialRewardSoulPowder
	{
		get { return m_nSpecialRewardSoulPowder; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsProofOfValorBossMonsterArrange(WPDProofOfValorBossMonsterArrange proofOfValorBossMonsterArrange)
	{
		m_nProofOfValorBossMonsterArrangeId = proofOfValorBossMonsterArrange.proofOfValorBossMonsterArrangeId;
		m_bIsSpecial = proofOfValorBossMonsterArrange.isSpecial;
		m_nStarGrade = proofOfValorBossMonsterArrange.starGrade;
		m_csMonsterArrange = CsGameData.Instance.GetMonsterArrange(proofOfValorBossMonsterArrange.monsterArrangeId);
		m_strDescription = CsConfiguration.Instance.GetString(proofOfValorBossMonsterArrange.descriptionKey);
		m_nRewardSoulPowder = proofOfValorBossMonsterArrange.rewardSoulPowder;
		m_nSpecialRewardSoulPowder = proofOfValorBossMonsterArrange.specialRewardSoulPowder;
	}
}
