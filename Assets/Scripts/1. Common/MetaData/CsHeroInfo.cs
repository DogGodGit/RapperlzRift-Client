using System.Collections.Generic;
using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 김경훈 (2018-01-23)
//---------------------------------------------------------------------------------------------------

public class CsHeroInfo : CsHeroBase
{
    bool m_bisLoggedIn;
    long m_lBattlePower;
    int m_nEquippedWingId;
    int m_nMainGearEnchantLevelSetNo;
    int m_nSubGearSoulstoneLevelSetNo;

	int m_nWingStep;
	int m_nWingLevel;
	List<int> m_listWing;

    CsHeroMainGear m_csHeroMainGearEquippedWeapon;
    CsHeroMainGear m_csHeroMainGearEquippedArmor;

    List<CsHeroSubGear> m_listEquippedHeroSubGears;
    List<CsHeroWingPart> m_listHeroWingParts;
    List<CsAttrValue> m_listRealAttrValues;

	Guid m_guidGuildId;
	string m_strGuildName;
	int m_nGuildMemberGrade;

	// Customizing
	int m_nCustomPresetHair;

	int m_nCustomFaceJawHeight;
	int m_nCustomFaceJawWidth;
	int m_nCustomFaceJawEndHeight;
	int m_nCustomFaceWidth;
	int m_nCustomFaceEyebrowHeight;
	int m_nCustomFaceEyebrowRotation;
	int m_nCustomFaceEyesWidth;
	int m_nCustomFaceNoseHeight;
	int m_nCustomFaceNoseWidth;
	int m_nCustomFaceMouthHeight;
	int m_nCustomFaceMouthWidth;

	int m_nCustomBodyHeadSize;
	int m_nCustomBodyArmsLength;
	int m_nCustomBodyArmsWidth;
	int m_nCustomBodyChestSize;
	int m_nCustomBodyWaistWidth;
	int m_nCustomBodyHipsSize;
	int m_nCustomBodyPelvisWidth;
	int m_nCustomBodyLegsLength;
	int m_nCustomBodyLegsWidth;

	int m_nCustomColorSkin;
	int m_nCustomColorEyes;
	int m_nCustomColorBeardAndEyebrow;
	int m_nCustomColorHair;

    int m_nEquippedCostumeId;
    int m_nAppliedCostumeEffectId;

	int m_nDisplayTitleId;

	//---------------------------------------------------------------------------------------------------
	public bool isLoggedIn
    {
        get { return m_bisLoggedIn; }
    }

    public long BattlePower
    {
        get { return m_lBattlePower; }
    }

    public int EquippedWingId
    {
        get { return m_nEquippedWingId; }
    }

    public CsHeroMainGear HeroMainGearEquippedWeapon
    {
        get { return m_csHeroMainGearEquippedWeapon; }
    }

    public CsHeroMainGear HeroMainGearEquippedArmor
    {
        get { return m_csHeroMainGearEquippedArmor; }
    }

    public List<CsHeroSubGear> EquippedHeroSubGearsList
    {
        get { return m_listEquippedHeroSubGears; }
    }

    public List<CsHeroWingPart> HeroWingPartsList
    {
        get { return m_listHeroWingParts; }
    }

    public List<CsAttrValue> RealAttrValuesList
    {
        get { return m_listRealAttrValues; }
    }

    public int MainGearEnchantLevelSetNo
    {
        get { return m_nMainGearEnchantLevelSetNo; }
    }

    public int SubGearSoulstoneLevelSetNo
    {
        get { return m_nSubGearSoulstoneLevelSetNo; }
    }

	public int WingStep
	{
		get { return m_nWingStep; }
	}

	public int WingLevel
	{
		get { return m_nWingLevel; }
	}

	public List<int> WingList
	{
		get { return m_listWing; }
	}

	public Guid GuildId
	{
		get { return m_guidGuildId; }
	}

	public string GuildName
	{
		get { return m_strGuildName; }
	}

	public int GuildMemberGrade
	{
		get { return m_nGuildMemberGrade; }
	}

	public int CustomPresetHair
	{
		get { return m_nCustomPresetHair; }
	}

	public int CustomFaceJawHeight
	{
		get { return m_nCustomFaceJawHeight; }
	}

	public int CustomFaceJawWidth
	{
		get { return m_nCustomFaceJawWidth; }
	}

	public int CustomFaceJawEndHeight
	{
		get { return m_nCustomFaceJawEndHeight; }
	}

	public int CustomFaceWidth
	{
		get { return m_nCustomFaceWidth; }
	}

	public int CustomFaceEyebrowHeight
	{
		get { return m_nCustomFaceEyebrowHeight; }
	}

	public int CustomFaceEyebrowRotation
	{
		get { return m_nCustomFaceEyebrowRotation; }
	}

	public int CustomFaceEyesWidth
	{
		get { return m_nCustomFaceEyesWidth; }
	}

	public int CustomFaceNoseHeight
	{
		get { return m_nCustomFaceNoseHeight; }
	}

	public int CustomFaceNoseWidth
	{
		get { return m_nCustomFaceNoseWidth; }
	}

	public int CustomFaceMouthHeight
	{
		get { return m_nCustomFaceMouthHeight; }
	}

	public int CustomFaceMouthWidth
	{
		get { return m_nCustomFaceMouthWidth; }
	}

	public int CustomBodyHeadSize
	{
		get { return m_nCustomBodyHeadSize; }
	}

	public int CustomBodyArmsLength
	{
		get { return m_nCustomBodyArmsLength; }
	}

	public int CustomBodyArmsWidth
	{
		get { return m_nCustomBodyArmsWidth; }
	}

	public int CustomBodyChestSize
	{
		get { return m_nCustomBodyChestSize; }
	}

	public int CustomBodyWaistWidth
	{
		get { return m_nCustomBodyWaistWidth; }
	}

	public int CustomBodyHipsSize
	{
		get { return m_nCustomBodyHipsSize; }
	}

	public int CustomBodyPelvisWidth
	{
		get { return m_nCustomBodyPelvisWidth; }
	}

	public int CustomBodyLegsLength
	{
		get { return m_nCustomBodyLegsLength; }
	}

	public int CustomBodyLegsWidth
	{
		get { return m_nCustomBodyLegsWidth; }
	}

	public int CustomColorSkin
	{
		get { return m_nCustomColorSkin; }
	}

	public int CustomColorEyes
	{
		get { return m_nCustomColorEyes; }
	}

	public int CustomColorBeardAndEyebrow
	{
		get { return m_nCustomColorBeardAndEyebrow; }
	}

	public int CustomColorHair
	{
		get { return m_nCustomColorHair; }
	}

    public int EquippedCostumeId
    {
        get { return m_nEquippedCostumeId; }
    }

    public int AppliedCostumeEffectId
    {
        get { return m_nAppliedCostumeEffectId; }
    }

	public int DisplayTitleId
	{
		get { return m_nDisplayTitleId; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroInfo(Guid guidHeroId, string strName,	int nNationId, int nJobId, int nLevel, bool bisLoggedIn, long lBattlePower, int nEquippedWingId, int nMainGearEnchantLevelSetNo, 
                      int nSubGearSoulstoneLevelSetNo,	PDFullHeroMainGear csHeroMainGearEquippedWeapon, PDFullHeroMainGear csHeroMainGearEquippedArmor, 
                      PDFullHeroSubGear[] equippedHeroSubGears, PDHeroWingPart[] heroWingPart, PDAttrValuePair[] realAttrValues, int nWingStep, int nWingLevel, int[] wings,
					  Guid guildId, string guildName, int guildMemberGrade, int nCustomPresetHair,
					  int nCustomFaceJawHeight, int nCustomFaceJawWidth, int nCustomFaceJawEndHeight, int nCustomFaceWidth, int nCustomFaceEyebrowHeight,
					  int nCustomFaceEyebrowRotation, int nCustomFaceEyesWidth, int nCustomFaceNoseHeight, int nCustomFaceNoseWidth, int nCustomFaceMouthHeight,
					  int nCustomFaceMouthWidth, int nCustomBodyHeadSize, int nCustomBodyArmsLength, int nCustomBodyArmsWidth, int nCustomBodyChestSize,
					  int nCustomBodyWaistWidth, int nCustomBodyHipsSize, int nCustomBodyPelvisWidth, int nCustomBodyLegsLength, int nCustomBodyLegsWidth,
                      int nCustomColorSkin, int nCustomColorEyes, int nCustomColorBeardAndEyebrow, int nCustomColorHair, int nEquippedCostumeId, int nAppliedCostumeEffectId, int nDisplayTitleId)
        : base(guidHeroId, strName, nNationId, nJobId, nLevel)
    {
        m_bisLoggedIn = bisLoggedIn;
        m_lBattlePower = lBattlePower;
        m_nEquippedWingId = nEquippedWingId;
        m_nMainGearEnchantLevelSetNo = nMainGearEnchantLevelSetNo;
        m_nSubGearSoulstoneLevelSetNo = nSubGearSoulstoneLevelSetNo;

        if (csHeroMainGearEquippedWeapon != null)
        {
            m_csHeroMainGearEquippedWeapon = new CsHeroMainGear(csHeroMainGearEquippedWeapon);
        }
        else
        {
            m_csHeroMainGearEquippedWeapon = null;
        }

        if (csHeroMainGearEquippedArmor != null)
        {
            m_csHeroMainGearEquippedArmor = new CsHeroMainGear(csHeroMainGearEquippedArmor);
        }
        else
        {
            m_csHeroMainGearEquippedArmor = null;
        }

        m_listEquippedHeroSubGears = new List<CsHeroSubGear>();
        AddEquippedHeroSubGears(equippedHeroSubGears, true);

        m_listHeroWingParts = new List<CsHeroWingPart>();
        AddHeroWingPart(heroWingPart, true);

        m_listRealAttrValues = new List<CsAttrValue>();
        AddRealAttrValues(realAttrValues, true);

		m_nWingStep = nWingStep;
		m_nWingLevel = nWingLevel;
		m_listWing = new List<int>(wings);

		m_guidGuildId = guildId;
		m_strGuildName = guildName;
		m_nGuildMemberGrade = guildMemberGrade;

		// Customizing
		m_nCustomPresetHair = nCustomPresetHair;

		m_nCustomFaceJawHeight = nCustomFaceJawHeight;
		m_nCustomFaceJawWidth = nCustomFaceJawWidth;
		m_nCustomFaceJawEndHeight = nCustomFaceJawEndHeight;
		m_nCustomFaceWidth = nCustomFaceWidth;
		m_nCustomFaceEyebrowHeight = nCustomFaceEyebrowHeight;
		m_nCustomFaceEyebrowRotation = nCustomFaceEyebrowRotation;
		m_nCustomFaceEyesWidth = nCustomFaceEyesWidth;
		m_nCustomFaceNoseHeight = nCustomFaceNoseHeight;
		m_nCustomFaceNoseWidth = nCustomFaceNoseWidth;
		m_nCustomFaceMouthHeight = nCustomFaceMouthHeight;
		m_nCustomFaceMouthWidth = nCustomFaceMouthWidth;

		m_nCustomBodyHeadSize = nCustomBodyHeadSize;
		m_nCustomBodyArmsLength = nCustomBodyArmsLength;
		m_nCustomBodyArmsWidth = nCustomBodyArmsWidth;
		m_nCustomBodyChestSize = nCustomBodyChestSize;
		m_nCustomBodyWaistWidth = nCustomBodyWaistWidth;
		m_nCustomBodyHipsSize = nCustomBodyHipsSize;
		m_nCustomBodyPelvisWidth = nCustomBodyPelvisWidth;
		m_nCustomBodyLegsLength = nCustomBodyLegsLength;
		m_nCustomBodyLegsWidth = nCustomBodyLegsWidth;

		m_nCustomColorSkin = nCustomColorSkin;
		m_nCustomColorEyes = nCustomColorEyes;
		m_nCustomColorBeardAndEyebrow = nCustomColorBeardAndEyebrow;
		m_nCustomColorHair = nCustomColorHair;

        m_nEquippedCostumeId = nEquippedCostumeId;
        m_nAppliedCostumeEffectId = nAppliedCostumeEffectId;

		m_nDisplayTitleId = nDisplayTitleId;
	}

    //---------------------------------------------------------------------------------------------------
    public void AddEquippedHeroSubGears(PDFullHeroSubGear[] csHeroSubGears, bool bClearAll = false)
    {
        if (csHeroSubGears != null)
        {
            if (bClearAll)
            {
                m_listEquippedHeroSubGears.Clear();
            }

            for (int i = 0; i < csHeroSubGears.Length; i++)
            {
                m_listEquippedHeroSubGears.Add(new CsHeroSubGear(csHeroSubGears[i]));
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void AddHeroWingPart(PDHeroWingPart[] heroWingPart, bool bClearAll = false)
    {
        if (heroWingPart != null)
        {
            if (bClearAll)
            {
                m_listHeroWingParts.Clear();
            }

            for (int i = 0; i < heroWingPart.Length; i++)
            {
                m_listHeroWingParts.Add(new CsHeroWingPart(heroWingPart[i]));
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void AddRealAttrValues(PDAttrValuePair[] realAttrValues, bool bClearAll = false)
    {
        if (realAttrValues != null)
        {
            if (bClearAll)
            {
                m_listRealAttrValues.Clear();
            }

            for (int i = 0; i < realAttrValues.Length; i++)
            {
                CsAttr csAttr = CsGameData.Instance.GetAttr(realAttrValues[i].id);

                m_listRealAttrValues.Add(new CsAttrValue(csAttr, realAttrValues[i].value));
            }
            m_listRealAttrValues.Sort(SortToId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    int SortToId(CsAttrValue A, CsAttrValue B)
    {
        if (A.Attr.AttrId > B.Attr.AttrId) return 1;
        else if (A.Attr.AttrId < B.Attr.AttrId) return -1;
        else return 0;
    }

}
