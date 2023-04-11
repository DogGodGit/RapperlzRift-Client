using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-14)
//---------------------------------------------------------------------------------------------------

public class CsWarMemoryWave
{
	int m_nWaveNo;
	int m_nStartDelayTime;
	int m_nClearPoint;
	int m_nTargetType;
	int m_nTargetArrangeKey;
	string m_strGuideImageName;
	string m_strGuideTitle;
	string m_strGuideContent;

	List<CsWarMemoryTransformationObject> m_listCsWarMemoryTransformationObject;

	//---------------------------------------------------------------------------------------------------
	public int WaveNo
	{
		get { return m_nWaveNo; }
	}
	
	public int StartDelayTime
	{
		get { return m_nStartDelayTime; }
	}

	public int ClearPoint
	{
		get { return m_nClearPoint; }
	}

	public int TargetType
	{
		get { return m_nTargetType; }
	}

	public int TargetArrangeKey
	{
		get { return m_nTargetArrangeKey; }
	}

	public string GuideImageName
	{
		get { return m_strGuideImageName; }
	}

	public string GuideTitle
	{
		get { return m_strGuideTitle; }
	}

	public string GuideContent
	{
		get { return m_strGuideContent; }
	}

	public List<CsWarMemoryTransformationObject> WarMemoryTransformationObjectList
	{
		get { return m_listCsWarMemoryTransformationObject; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWarMemoryWave(WPDWarMemoryWave warMemoryWave)
	{
		m_nWaveNo = warMemoryWave.waveNo;
		m_nStartDelayTime = warMemoryWave.startDelayTime;
		m_nClearPoint = warMemoryWave.clearPoint;
		m_nTargetType = warMemoryWave.targetType;
		m_nTargetArrangeKey = warMemoryWave.targetArrangeKey;
		m_strGuideImageName = warMemoryWave.guideImageName; ;
		m_strGuideTitle = CsConfiguration.Instance.GetString(warMemoryWave.guideTitleKey);
		m_strGuideContent = CsConfiguration.Instance.GetString(warMemoryWave.guideContentKey);

		m_listCsWarMemoryTransformationObject = new List<CsWarMemoryTransformationObject>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsWarMemoryTransformationObject GetTransformationObject(int nObjectId)
	{
		for (int i = 0; i < m_listCsWarMemoryTransformationObject.Count; i++)
		{
			if (m_listCsWarMemoryTransformationObject[i].TransformationObjectId == nObjectId)
			{
				return m_listCsWarMemoryTransformationObject[i];
			}
		}
		return null;
	}
}
