using System;
using System.Collections.Generic;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 김용재 (2018-10-10)
//---------------------------------------------------------------------------------------------------

public class CsHeroTradeShipBestRecord 
{
    Guid m_guidHeroId;

    string m_strHeroName;

    int m_nJobId;
    int m_nHeroNationId;
    int m_nDifficulty;
    int m_nPoint;

    //---------------------------------------------------------------------------------------------------
    public Guid HeroId
    {
        get { return m_guidHeroId; }
        set { m_guidHeroId = value; }
    }

    public string HeroName
    {
        get { return m_strHeroName; }
        set { m_strHeroName = value; }
    }

    public int JobId
    {
        get { return m_nJobId; }
        set { m_nJobId = value; }
    }

    public int HeroNationId
    {
        get { return m_nHeroNationId; }
        set { m_nHeroNationId = value; }
    }

    public int Difficulty
    {
        get { return m_nDifficulty; }
    }

    public int Point
    {
        get { return m_nPoint; }
        set { m_nPoint = value; }
    }

    //---------------------------------------------------------------------------------------------------
    public CsHeroTradeShipBestRecord(PDHeroTradeShipBestRecord heroTradeShipBestRecord)
    {
        m_guidHeroId = heroTradeShipBestRecord.heroId;
        m_strHeroName = heroTradeShipBestRecord.heroName;
        m_nJobId = heroTradeShipBestRecord.heroJobId;
        m_nHeroNationId = heroTradeShipBestRecord.heroNationId;
        m_nDifficulty = heroTradeShipBestRecord.difficulty;
        m_nPoint = heroTradeShipBestRecord.point;
    }

    //---------------------------------------------------------------------------------------------------
    public CsHeroTradeShipBestRecord(Guid guidHeroId, string strHeroName, int nJobId, int nHeroNationId, int nDifficulty, int nPoint)
    {
        m_guidHeroId = guidHeroId;
        m_strHeroName = strHeroName;
        m_nJobId = nJobId;
        m_nHeroNationId = nHeroNationId;
        m_nDifficulty = nDifficulty;
        m_nPoint = nPoint;
    }
}
