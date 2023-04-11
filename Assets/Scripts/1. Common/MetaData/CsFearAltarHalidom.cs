using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-07)
//---------------------------------------------------------------------------------------------------

public class CsFearAltarHalidom
{
	int m_nHalidomId;
	CsFearAltarHalidomElemental m_csFearAltarHalidomElemental;
	CsFearAltarHalidomLevel m_csFearAltarHalidomLevel;
	string m_strImageName;

	//---------------------------------------------------------------------------------------------------
	public int HalidomId
	{
		get { return m_nHalidomId; }
	}

	public CsFearAltarHalidomElemental FearAltarHalidomElemental
	{
		get { return m_csFearAltarHalidomElemental; }
	}

	public CsFearAltarHalidomLevel FearAltarHalidomLevel
	{
		get { return m_csFearAltarHalidomLevel; }
	}

	public string ImageName
	{
		get { return m_strImageName; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsFearAltarHalidom(WPDFearAltarHalidom fearAltarHalidom)
	{
		m_nHalidomId = fearAltarHalidom.halidomId;
		m_csFearAltarHalidomElemental = CsGameData.Instance.GetFearAltarHalidomElemental(fearAltarHalidom.halidomElementalId);
		m_csFearAltarHalidomLevel = CsGameData.Instance.GetFearAltarHalidomLevel(fearAltarHalidom.halidomLevel);
		m_strImageName = fearAltarHalidom.imageName;
	}
}
