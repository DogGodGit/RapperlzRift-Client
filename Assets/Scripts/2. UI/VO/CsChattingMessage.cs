using System;
using System.Text;
using ClientCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-04)
//---------------------------------------------------------------------------------------------------

public enum EnNoticeType
{
    None = 0,
    GuildApply,
    GuildEvent,
    Party,
	Taunting,
    GuildBlessingBuff,
	SystemMessage,
}


public class CsChattingMessage : IComparable
{
    int m_nNotcieType;
    int m_nType;
    string[] m_astrMessage;
    string m_strNotcieMessage;
    PDChattingLink m_pDChattingLink;
    CsSimpleHero m_csSimpleHeroSender;
    CsSimpleHero m_csSimpleHeroTarget;
    CsChattingType m_csChatType;
    string m_strChattingMessage;
    string m_strGearName;
    DateTime m_dtDateTime;

    Guid m_guidGuildId;
    string m_strGuildName;
    Guid m_guidHeroId;
    string m_strHeroName;
    int m_nContentId;
    int m_nContinentId;
	int m_nNationId;
	Vector3 m_vtTrueHeroPosition;
    bool m_bIsBlessingBuffRunning;

    //---------------------------------------------------------------------------------------------------
    public EnChattingType ChattingType
    {
        get { return (EnChattingType)m_nType; }
    }

    public EnNoticeType NoticeType
    {
        get { return (EnNoticeType)m_nNotcieType; }
    }

    public string[] Messages
    {
        get { return m_astrMessage; }
    }

    public PDChattingLink ChattingLink
    {
        get { return m_pDChattingLink; }
    }

    public CsSimpleHero Sender
    {
        get { return m_csSimpleHeroSender; }
    }

    public CsSimpleHero Target
    {
        get { return m_csSimpleHeroTarget; }
    }

    public string ChattingMessage
    {
        get { return m_strChattingMessage; }
    }

    public string GearName
    {
        get { return m_strGearName; }
    }

    public DateTime DateTime
    {
        get { return m_dtDateTime; }
    }

    public Guid GuildId
    {
        get { return m_guidGuildId; }
    }

    public string GuildName
    {
        get { return m_strGuildName; }
    }

    public Guid HeroId
    {
        get { return m_guidHeroId; }
    }

    public string HeroName
    {
        get { return m_strHeroName; }
    }

    public int ContentId
    {
        get { return m_nContentId; }
    }

    public int ContinentId
    {
        get { return m_nContinentId; }
    }

	public int NationId
	{
		get { return m_nNationId; }
	}

    public CsChattingType ChatType
    {
        get { return m_csChatType; }
    }

	public Vector3 TrueHeroPosition
	{
		get { return m_vtTrueHeroPosition; }
	}

    public bool IsBlessingBuffRunning
    {
        get { return m_bIsBlessingBuffRunning; }
    }

    //---------------------------------------------------------------------------------------------------
    public CsChattingMessage(int nType, string[] astrMessage, PDChattingLink pDChattingLink, PDSimpleHero sender, PDSimpleHero target)
    {
        m_nNotcieType = 0;
        m_nType = nType;
        m_csChatType = CsGameData.Instance.GetChattingType((EnChattingType)nType);
        m_astrMessage = astrMessage;
        m_pDChattingLink = pDChattingLink;
        m_csSimpleHeroSender = new CsSimpleHero(sender);
        if (target != null)
            m_csSimpleHeroTarget = new CsSimpleHero(target);

        UpdateChattingMessage(astrMessage, pDChattingLink);

        m_dtDateTime = CsGameData.Instance.MyHeroInfo.CurrentDateTime;
    }

    //---------------------------------------------------------------------------------------------------
    //길드 가입
    public CsChattingMessage(int nType, int nNoticeType, Guid guidGuildId, string strGuildName, Guid guidHeroId, string strHeroName, int nContinentId)
    {
        m_nType = nType;
        m_csChatType = CsGameData.Instance.GetChattingType((EnChattingType)nType);
        m_nNotcieType = nNoticeType;
        m_guidGuildId = guidGuildId;
        m_strGuildName = strGuildName;
        m_guidHeroId = guidHeroId;
        m_strHeroName = strHeroName;
        m_nContinentId = nContinentId;
        m_dtDateTime = CsGameData.Instance.MyHeroInfo.CurrentDateTime;
    }

    //---------------------------------------------------------------------------------------------------
    //길드 이벤트
    public CsChattingMessage(int nType, int nNoticeType, PDSimpleHero sender, int nContentId)
    {
        m_nType = nType;
        m_csChatType = CsGameData.Instance.GetChattingType((EnChattingType)nType);
        m_nNotcieType = nNoticeType;
        m_csSimpleHeroSender = new CsSimpleHero(sender);
        m_nContentId = nContentId;
        m_dtDateTime = CsGameData.Instance.MyHeroInfo.CurrentDateTime;
    }

	//---------------------------------------------------------------------------------------------------
	// 진정한 영웅 도발
	public CsChattingMessage(Guid guidHeroId, string strHeroName, int nNationId, int nContinentId, Vector3 vtPosition)
	{
		m_nNotcieType = (int)EnNoticeType.Taunting;
		m_nType = (int)EnChattingType.Nation;
		m_csChatType = CsGameData.Instance.GetChattingType(EnChattingType.Nation);
		m_guidHeroId = guidHeroId;
		m_strHeroName = strHeroName;

		m_nNationId = nNationId;
		m_nContinentId = nContinentId;

		m_vtTrueHeroPosition = vtPosition;

		m_dtDateTime = CsGameData.Instance.MyHeroInfo.CurrentDateTime;
	}

    //---------------------------------------------------------------------------------------------------
    public CsChattingMessage(bool bIsBlessingBuffRunning)
    {
        m_nType = (int)EnChattingType.Guild;
        m_nNotcieType = (int)EnNoticeType.GuildBlessingBuff;
        m_bIsBlessingBuffRunning = bIsBlessingBuffRunning;

        m_csChatType = CsGameData.Instance.GetChattingType(EnChattingType.Guild);
        m_dtDateTime = CsGameData.Instance.MyHeroInfo.CurrentDateTime;
    }

	//---------------------------------------------------------------------------------------------------
	// 시스템 메시지, 상황 알림
	public CsChattingMessage(string strChattingMessage)
	{
		m_nNotcieType = (int)EnNoticeType.SystemMessage;

		m_strChattingMessage = strChattingMessage;
	}

    //---------------------------------------------------------------------------------------------------
    public void Update(string[] astrMessage, PDChattingLink pDChattingLink)
    {
        m_astrMessage = astrMessage;
        m_pDChattingLink = pDChattingLink;

        UpdateChattingMessage(astrMessage, pDChattingLink);

        m_dtDateTime = CsGameData.Instance.MyHeroInfo.CurrentDateTime;
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateChattingMessage(string[] astrMessage, PDChattingLink pDChattingLink)
    {
        m_strChattingMessage = string.Empty;

        string strGearName = m_strGearName = string.Empty;

        if (pDChattingLink != null)
        {
            if (pDChattingLink.type == PDChattingLink.kType_MainGear)
            {
                PDMainGearChattingLink pDMainGearChattingLink = (PDMainGearChattingLink)pDChattingLink;
                CsMainGear csMainGear = CsGameData.Instance.GetMainGear(pDMainGearChattingLink.gear.mainGearId);

                m_strGearName = strGearName = string.Format("<color={0}>[{1}]</color>", csMainGear.MainGearGrade.ColorCode, csMainGear.Name);

            }
            else if (pDChattingLink.type == PDChattingLink.kType_SubGear)
            {
                PDSubGearChattingLink pDSubGearChattingLink = (PDSubGearChattingLink)pDChattingLink;
                CsSubGear csSubGear = CsGameData.Instance.GetSubGear(pDSubGearChattingLink.gear.subGearId);
                CsSubGearLevel csSubGearLevel = csSubGear.GetSubGearLevel(pDSubGearChattingLink.gear.level);
                CsSubGearName csSubGearName = csSubGear.GetSubGearName(csSubGearLevel.SubGearGrade.Grade);

                m_strGearName = strGearName = string.Format("<color={0}>[{1}]</color>", csSubGearLevel.SubGearGrade.ColorCode, csSubGearName.Name);
            }
            else
            {
                PDMountGearChattingLink pDMountGearChattingLink = (PDMountGearChattingLink)pDChattingLink;
                CsMountGear csMountGear = CsGameData.Instance.GetMountGear(pDMountGearChattingLink.gear.mountGearId);
                m_strGearName = strGearName = string.Format("<color={0}>[{1}]</color>", csMountGear.MountGearGrade.ColorCode, csMountGear.Name);
            }
        }

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < astrMessage.Length; ++i)
        {
            sb.Append(astrMessage[i]);

            if (i == 0 && strGearName != string.Empty)
            {
                sb.Append(strGearName);
            }
        }

        m_strChattingMessage = sb.ToString();
    }


    #region Interface(IComparable) implement
    //---------------------------------------------------------------------------------------------------
    public int CompareTo(object obj)
    {
        return m_dtDateTime.CompareTo(((CsChattingMessage)obj).DateTime) * (-1);
    }
    #endregion Interface(IComparable) implement
}
