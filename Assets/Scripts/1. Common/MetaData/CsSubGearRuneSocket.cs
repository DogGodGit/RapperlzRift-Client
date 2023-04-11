using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-13)
//---------------------------------------------------------------------------------------------------

public class CsSubGearRuneSocket
{
	int m_nSubGearId;						// 보조장비ID
	int m_nSocketIndex;						// 소켓인덱스
	int m_nRequiredSubGearLevel;			// 개방필요보조장비레벨
	string m_strEnableText;					// 장착가능텍스트키	
	string m_strBackgroundImageName;		// 배경이미지이름
	string m_strMiniBackgroundImageName;    // 미니배경이미지이름	

	List<CsSubGearRuneSocketAvailableItemType> m_listCsSubGearRuneSocketAvailableItemType;

	//---------------------------------------------------------------------------------------------------
	public int SubGearId
	{
		get { return m_nSubGearId; }
	}

	public int SocketIndex
	{
		get { return m_nSocketIndex; }
	}

	public int RequiredSubGearLevel
	{
		get { return m_nRequiredSubGearLevel; }
	}

	public string EnableText
	{
		get { return m_strEnableText; }
	}

	public string BackgroundImageName
	{
		get { return m_strBackgroundImageName; }
	}

	public string MiniBackgroundImageName
	{
		get { return m_strMiniBackgroundImageName; }
	}

	public List<CsSubGearRuneSocketAvailableItemType> SubGearRuneSocketAvailableItemTypeList
	{
		get { return m_listCsSubGearRuneSocketAvailableItemType; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSubGearRuneSocket(WPDSubGearRuneSocket subGearInscriptionSocket)
	{
		m_nSubGearId = subGearInscriptionSocket.subGearId;
		m_nSocketIndex = subGearInscriptionSocket.socketIndex;
		m_nRequiredSubGearLevel = subGearInscriptionSocket.requiredSubGearLevel;
		m_strEnableText = CsConfiguration.Instance.GetString(subGearInscriptionSocket.enableTextKey);
		m_strBackgroundImageName = subGearInscriptionSocket.backgroundImageName;
		m_strMiniBackgroundImageName = subGearInscriptionSocket.miniBackgroundImageName;

		m_listCsSubGearRuneSocketAvailableItemType = new List<CsSubGearRuneSocketAvailableItemType>();
	}
}
