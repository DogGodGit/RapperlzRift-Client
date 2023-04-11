using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-05-24)
//---------------------------------------------------------------------------------------------------

public class CsLobbyHero
{
    Guid m_guidHero;
    string m_strName;
    int m_nLevel;
    int m_nNationId;
    long m_lBattlePower;
    bool m_bNamingTutorialCompleted;
    int m_nJobId;
    int m_nEquippedWeaponGearId;
    int m_nEquippedWeaponEnchantLevel;
    int m_nEquippedArmorGearId;
    int m_nEquippedArmorEnchantLevel;
    int m_nEquippedWingId;

    bool m_bIsSelected;

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

    //---------------------------------------------------------------------------------------------------
    public Guid HeroId
    {
        get { return m_guidHero; }
        set { m_guidHero = value; }
    }

    public string Name
    {
        get { return m_strName; }
        set { m_strName = value; }
    }

    public int Level
    {
        get { return m_nLevel; }
        set { m_nLevel = value; }
    }

    public int NationId
    {
        get { return m_nNationId; }
        set { m_nNationId = value; }
    }

    public long BattlePower
    {
        get { return m_lBattlePower; }
        set { m_lBattlePower = value; }
    }

    public bool NamingTutorialCompleted
    {
        get { return m_bNamingTutorialCompleted; }
        set { m_bNamingTutorialCompleted = value; }
    }

    public int JobId
    {
        get { return m_nJobId; }
    }

    public int EquippedWeaponGearId
    {
        get { return m_nEquippedWeaponGearId; }
    }

    public int EquippedWeaponEnchantLevel
    {
        get { return m_nEquippedWeaponEnchantLevel; }
    }

    public int EquippedArmorGearId
    {
        get { return m_nEquippedArmorGearId; }
    }

    public int EquippedArmorEnchantLevel
    {
        get { return m_nEquippedArmorEnchantLevel; }
    }

    public int EquippedWingId
    {
        get { return m_nEquippedWingId; }
    }

    public int CustomPresetHair
    {
        get { return m_nCustomPresetHair; }
        set { m_nCustomPresetHair = value; }
    }

    public int CustomFaceJawHeight
    {
        get { return m_nCustomFaceJawHeight; }
        set { m_nCustomFaceJawHeight = value; }
    }

    public int CustomFaceJawWidth
    {
        get { return m_nCustomFaceJawWidth; }
        set { m_nCustomFaceJawWidth = value; }
    }

    public int CustomFaceJawEndHeight
    {
        get { return m_nCustomFaceJawEndHeight; }
        set { m_nCustomFaceJawEndHeight = value; }
    }

    public int CustomFaceWidth
    {
        get { return m_nCustomFaceWidth; }
        set { m_nCustomFaceWidth = value; }
    }

    public int CustomFaceEyebrowHeight
    {
        get { return m_nCustomFaceEyebrowHeight; }
        set { m_nCustomFaceEyebrowHeight = value; }
    }

    public int CustomFaceEyebrowRotation
    {
        get { return m_nCustomFaceEyebrowRotation; }
        set { m_nCustomFaceEyebrowRotation = value; }
    }

    public int CustomFaceEyesWidth
    {
        get { return m_nCustomFaceEyesWidth; }
        set { m_nCustomFaceEyesWidth = value; }
    }

    public int CustomFaceNoseHeight
    {
        get { return m_nCustomFaceNoseHeight; }
        set { m_nCustomFaceNoseHeight = value; }
    }

    public int CustomFaceNoseWidth
    {
        get { return m_nCustomFaceNoseWidth; }
        set { m_nCustomFaceNoseWidth = value; }
    }

    public int CustomFaceMouthHeight
    {
        get { return m_nCustomFaceMouthHeight; }
        set { m_nCustomFaceMouthHeight = value; }
    }

    public int CustomFaceMouthWidth
    {
        get { return m_nCustomFaceMouthWidth; }
        set { m_nCustomFaceMouthWidth = value; }
    }

    public int CustomBodyHeadSize
    {
        get { return m_nCustomBodyHeadSize; }
        set { m_nCustomBodyHeadSize = value; }
    }

    public int CustomBodyArmsLength
    {
        get { return m_nCustomBodyArmsLength; }
        set { m_nCustomBodyArmsLength = value; }
    }

    public int CustomBodyArmsWidth
    {
        get { return m_nCustomBodyArmsWidth; }
        set { m_nCustomBodyArmsWidth = value; }
    }

    public int CustomBodyChestSize
    {
        get { return m_nCustomBodyChestSize; }
        set { m_nCustomBodyChestSize = value; }
    }

    public int CustomBodyWaistWidth
    {
        get { return m_nCustomBodyWaistWidth; }
        set { m_nCustomBodyWaistWidth = value; }
    }

    public int CustomBodyHipsSize
    {
        get { return m_nCustomBodyHipsSize; }
        set { m_nCustomBodyHipsSize = value; }
    }

    public int CustomBodyPelvisWidth
    {
        get { return m_nCustomBodyPelvisWidth; }
        set { m_nCustomBodyPelvisWidth = value; }
    }

    public int CustomBodyLegsLength
    {
        get { return m_nCustomBodyLegsLength; }
        set { m_nCustomBodyLegsLength = value; }
    }

    public int CustomBodyLegsWidth
    {
        get { return m_nCustomBodyLegsWidth; }
        set { m_nCustomBodyLegsWidth = value; }
    }

    public int CustomColorSkin
    {
        get { return m_nCustomColorSkin; }
        set { m_nCustomColorSkin = value; }
    }

    public int CustomColorEyes
    {
        get { return m_nCustomColorEyes; }
        set { m_nCustomColorEyes = value; }
    }

    public int CustomColorBeardAndEyebrow
    {
        get { return m_nCustomColorBeardAndEyebrow; }
        set { m_nCustomColorBeardAndEyebrow = value; }
    }

    public int CustomColorHair
    {
        get { return m_nCustomColorHair; }
        set { m_nCustomColorHair = value; }
    }

    public int EquippedCostumeId
    {
        get { return m_nEquippedCostumeId; }
    }

    public int AppliedCostumeEffectId
    {
        get { return m_nAppliedCostumeEffectId; }
    }

    //---------------------------------------------------------------------------------------------------
    public CsLobbyHero(PDLobbyHero pDLobbyHero)
    {
        m_guidHero = pDLobbyHero.id;
        m_strName = pDLobbyHero.name;
        m_nLevel = pDLobbyHero.level;
        m_nNationId = pDLobbyHero.nationId;
        m_lBattlePower = pDLobbyHero.battlePower;
        m_bNamingTutorialCompleted = pDLobbyHero.namingTutorialCompleted;
        m_nJobId = pDLobbyHero.jobId;

        if (pDLobbyHero.equippedWeapon != null)
        {
            m_nEquippedWeaponGearId = pDLobbyHero.equippedWeapon.mainGearId;
            m_nEquippedWeaponEnchantLevel = pDLobbyHero.equippedWeapon.enchantLevel;
        }

        if (pDLobbyHero.equippedArmor != null)
        {
            m_nEquippedArmorGearId = pDLobbyHero.equippedArmor.mainGearId;
            m_nEquippedArmorEnchantLevel = pDLobbyHero.equippedArmor.enchantLevel;
        }
        m_nEquippedWingId = pDLobbyHero.equippedWingId;

		// Customizing
		m_nCustomPresetHair = pDLobbyHero.customPresetHair;

        m_nCustomFaceJawHeight = pDLobbyHero.customFaceJawHeight;
        m_nCustomFaceJawWidth = pDLobbyHero.customFaceJawWidth;
        m_nCustomFaceJawEndHeight = pDLobbyHero.customFaceJawEndHeight;
        m_nCustomFaceWidth = pDLobbyHero.customFaceWidth;
        m_nCustomFaceEyebrowHeight = pDLobbyHero.customFaceEyebrowHeight;
        m_nCustomFaceEyebrowRotation = pDLobbyHero.customFaceEyebrowRotation;
        m_nCustomFaceEyesWidth = pDLobbyHero.customFaceEyesWidth;
        m_nCustomFaceNoseHeight = pDLobbyHero.customFaceNoseHeight;
        m_nCustomFaceNoseWidth = pDLobbyHero.customFaceNoseWidth;
        m_nCustomFaceMouthHeight = pDLobbyHero.customFaceMouthHeight;
        m_nCustomFaceMouthWidth = pDLobbyHero.customFaceMouthWidth;

        m_nCustomBodyHeadSize = pDLobbyHero.customBodyHeadSize;
        m_nCustomBodyArmsLength = pDLobbyHero.customBodyArmsLength;
        m_nCustomBodyArmsWidth = pDLobbyHero.customBodyArmsWidth;
        m_nCustomBodyChestSize = pDLobbyHero.customBodyChestSize;
        m_nCustomBodyWaistWidth = pDLobbyHero.customBodyWaistWidth;
        m_nCustomBodyHipsSize = pDLobbyHero.customBodyHipsSize;
        m_nCustomBodyPelvisWidth = pDLobbyHero.customBodyPelvisWidth;
        m_nCustomBodyLegsLength = pDLobbyHero.customBodyLegsLength;
        m_nCustomBodyLegsWidth = pDLobbyHero.customBodyLegsWidth;

        m_nCustomColorSkin = pDLobbyHero.customColorSkin;
        m_nCustomColorEyes = pDLobbyHero.customColorEyes;
        m_nCustomColorBeardAndEyebrow = pDLobbyHero.customColorBeardAndEyebrow;
        m_nCustomColorHair = pDLobbyHero.customColorHair;

        m_nEquippedCostumeId = pDLobbyHero.equippedCostumeId;
        m_nAppliedCostumeEffectId = pDLobbyHero.appliedCostumeEffectId;
    }

    public CsLobbyHero(int nJobId)
    {
        m_nJobId = nJobId;
        m_bNamingTutorialCompleted = false;
    }
}
