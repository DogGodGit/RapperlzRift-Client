using System;
using System.Collections.Generic;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 김용재 (2018-10-10)
//---------------------------------------------------------------------------------------------------

public class CsHeroAnkouTombBestRecord
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
    public CsHeroAnkouTombBestRecord(PDHeroAnkouTombBestRecord heroAnkouTombBestRecord)
    {
        m_guidHeroId = heroAnkouTombBestRecord.heroId;
        m_strHeroName = heroAnkouTombBestRecord.heroName;
        m_nJobId = heroAnkouTombBestRecord.heroJobId;
        m_nHeroNationId = heroAnkouTombBestRecord.heroNationId;
        m_nDifficulty = heroAnkouTombBestRecord.difficulty;
        m_nPoint = heroAnkouTombBestRecord.point;
    }

    //---------------------------------------------------------------------------------------------------
    public CsHeroAnkouTombBestRecord(Guid guidHeroId, string strHeroName, int nJobId, int nHeroNationId, int nDifficulty, int nPoint)
    {
        m_guidHeroId = guidHeroId;
        m_strHeroName = strHeroName;
        m_nJobId = nJobId;
        m_nHeroNationId = nHeroNationId;
        m_nDifficulty = nDifficulty;
        m_nPoint = nPoint;
    }
}
