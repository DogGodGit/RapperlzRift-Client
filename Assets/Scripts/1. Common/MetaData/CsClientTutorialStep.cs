using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-10)
//---------------------------------------------------------------------------------------------------

public class CsClientTutorialStep
{
	int m_nTutorialId;
	int m_nStep;
	string m_strText;
	float m_flTextXPosition;
	float m_flTextYPosition;
	float m_flArrowXPosition;
	float m_flArrowYPosition;
	float m_flArrowYRotation;
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
	public int TutorialId
	{
		get { return m_nTutorialId; }
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

	public float ArrowYRotation
	{
		get { return m_flArrowYRotation; }
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
	public CsClientTutorialStep(WPDClientTutorialStep clientTutorialStep)
	{
		m_nTutorialId = clientTutorialStep.tutorialId;
		m_nStep = clientTutorialStep.step;
		m_strText = CsConfiguration.Instance.GetString(clientTutorialStep.textKey);
		m_flTextXPosition = clientTutorialStep.textXPosition;
		m_flTextYPosition = clientTutorialStep.textYPosition;
		m_flArrowXPosition = clientTutorialStep.arrowXPosition;
		m_flArrowYPosition = clientTutorialStep.arrowYPosition;
		m_flArrowYRotation = clientTutorialStep.arrowYRotation;
		m_flClickXPosition = clientTutorialStep.clickXPosition;
		m_flClickYPosition = clientTutorialStep.clickYPosition;
		m_nClickWidth = clientTutorialStep.clickWidth;
		m_nClickHeight = clientTutorialStep.clickHeight;
		m_strEffectName = clientTutorialStep.effectName;
		m_flEffectXPosition = clientTutorialStep.effectXPosition;
		m_flEffectYPosition = clientTutorialStep.effectYPosition;
		m_nEffectWidth = clientTutorialStep.effectWidth;
		m_nEffectHeight = clientTutorialStep.effectHeight;
	}
}
