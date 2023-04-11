using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-23)
//---------------------------------------------------------------------------------------------------

public class CsCreatureCardGrade
{
	int m_nGrade;
	string m_strColorCode;
	int m_nSaleSoulPowder;
	int m_nDisassembleSoulPowder;
	int m_nCompositionSoulPowder;

	//---------------------------------------------------------------------------------------------------
	public int Grade
	{
		get { return m_nGrade; }
	}

	public string ColorCode
	{
		get { return m_strColorCode; }
	}

	public int SaleSoulPowder
	{
		get { return m_nSaleSoulPowder; }
	}

	public int DisassembleSoulPowder
	{
		get { return m_nDisassembleSoulPowder; }
	}

	public int CompositionSoulPowder
	{
		get { return m_nCompositionSoulPowder; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureCardGrade(WPDCreatureCardGrade creatureCardGrade)
	{
		m_nGrade = creatureCardGrade.grade;
		m_strColorCode = creatureCardGrade.colorCode;
		m_nSaleSoulPowder = creatureCardGrade.saleSoulPowder;
		m_nDisassembleSoulPowder = creatureCardGrade.disassembleSoulPowder;
		m_nCompositionSoulPowder = creatureCardGrade.compositionSoulPowder;
	}
}
