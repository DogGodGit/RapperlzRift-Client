using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-07)
//---------------------------------------------------------------------------------------------------

public class CsFearAltarHalidomLevel
{
	int m_nHalidomLevel;

	//---------------------------------------------------------------------------------------------------
	public int HalidomLevel
	{
		get { return m_nHalidomLevel; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsFearAltarHalidomLevel(WPDFearAltarHalidomLevel fearAltarHalidomLevel)
	{
		m_nHalidomLevel = fearAltarHalidomLevel.halidomLevel;
	}
}
