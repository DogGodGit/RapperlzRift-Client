using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-27)
//---------------------------------------------------------------------------------------------------

public class CsMenuContentTutorialStep
{
	int m_nContentId;
	int m_nStep;
	string m_strText;
	float m_flTextXPosition;
	float m_flTextYPosition;
	float m_flArrowXPosition;
	float m_flArrowYPosition;
	float m_flArrowZRotation;
	float m_flClickXPosition;
	float m_flClickYPosition;
	int m_nClickWidth;
	int m_nClickHeight;
	string m_strEffectName;
	float m_flEffectXPosition;
	float m_flEffectYPosition;
	int m_nEffectWidth;
	int m_nEffectHeight;

	//---------------------------------------------------------------------------------------------------
	public int ContentId
	{
		get { return m_nContentId; }
	}

	public int Step
	{
		get { return m_nStep; }
	}

	public string Text
	{
		get { return m_strText; }
	}

	public float TextXPosition
	{
		get { return m_flTextXPosition; }
	}

	public float TextYPosition
	{
		get { return m_flTextYPosition; }
	}

	public float ArrowXPosition
	{
		get { return m_flArrowXPosition; }
	}

	public float ArrowYPosition
	{
		get { return m_flArrowYPosition; }
	}

	public float ArrowZRotation
	{
		get { return m_flArrowZRotation; }
	}

	public float ClickXPosition
	{
		get { return m_flClickXPosition; }
	}

	public float ClickYPosition
	{
		get { return m_flClickYPosition; }
	}

	public int ClickWidth
	{
		get { return m_nClickWidth; }
	}

	public int ClickHeight
	{
		get { return m_nClickHeight; }
	}

	public string EffectName
	{
		get { return m_strEffectName; }
	}

	public float EffectXPosition
	{
		get { return m_flEffectXPosition; }
	}

	public float EffectYPosition
	{
		get { return m_flEffectYPosition; }
	}

	public int EffectWidth
	{
		get { return m_nEffectWidth; }
	}

	public int EffectHeight
	{
		get { return m_nEffectHeight; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMenuContentTutorialStep(WPDMenuContentTutorialStep menuContentTutorialStep)
	{
		m_nContentId = menuContentTutorialStep.contentId;
		m_nStep = menuContentTutorialStep.step;
		m_strText = CsConfiguration.Instance.GetString(menuContentTutorialStep.textKey);
		m_flTextXPosition = menuContentTutorialStep.textXPosition;
		m_flTextYPosition = menuContentTutorialStep.textYPosition;
		m_flArrowXPosition = menuContentTutorialStep.arrowXPosition;
		m_flArrowYPosition = menuContentTutorialStep.arrowYPosition;
		m_flArrowZRotation = menuContentTutorialStep.arrowZRotation;
		m_flClickXPosition = menuContentTutorialStep.clickXPosition;
		m_flClickYPosition = menuContentTutorialStep.clickYPosition;
		m_nClickWidth = menuContentTutorialStep.clickWidth;
		m_nClickHeight = menuContentTutorialStep.clickHeight;
		m_strEffectName = menuContentTutorialStep.effectName;
		m_flEffectXPosition = menuContentTutorialStep.effectXPosition;
		m_flEffectYPosition = menuContentTutorialStep.effectYPosition;
		m_nEffectWidth = menuContentTutorialStep.effectWidth;
		m_nEffectHeight = menuContentTutorialStep.effectHeight;
	}
}
