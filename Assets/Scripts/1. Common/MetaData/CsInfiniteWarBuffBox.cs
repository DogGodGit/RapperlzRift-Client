using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-26)
//---------------------------------------------------------------------------------------------------

public class CsInfiniteWarBuffBox
{
	int m_nBuffBoxId;
	string m_strPrefabName;
	float m_flOffenseFactor;
	float m_flDefenseFactor;
	float m_flHpRecoveryFactor;

	//---------------------------------------------------------------------------------------------------
	public int BuffBoxId
	{
		get { return m_nBuffBoxId; }
	}

	public string PrefabName
	{
		get { return m_strPrefabName; }
	}

	public float OffenseFactor
	{
		get { return m_flOffenseFactor; }
	}

	public float DefenseFactor
	{
		get { return m_flDefenseFactor; }
	}

	public float HpRecoveryFactor
	{
		get { return m_flHpRecoveryFactor; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsInfiniteWarBuffBox(WPDInfiniteWarBuffBox infiniteWarBuffBox)
	{
		m_nBuffBoxId = infiniteWarBuffBox.buffBoxId;
		m_strPrefabName = infiniteWarBuffBox.prefabName;
		m_flOffenseFactor = infiniteWarBuffBox.offenseFactor;
		m_flDefenseFactor = infiniteWarBuffBox.defenseFactor;
		m_flHpRecoveryFactor = infiniteWarBuffBox.hpRecoveryFactor;
	}
}
