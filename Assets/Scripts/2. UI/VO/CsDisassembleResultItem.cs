//---------------------------------------------------------------------------------------------------
// 작성 : 김경훈 (2017-12-19)
//---------------------------------------------------------------------------------------------------

public class CsDisassembleResultItem
{
    CsItem m_csItem;
    int m_nCount;
    bool m_bOwned;

    //---------------------------------------------------------------------------------------------------
    public CsItem Item
    {
        get { return m_csItem; }
    }

    public int Count
    {
        get { return m_nCount; }
        set { m_nCount = value; }
    }

    public bool Owned
    {
        get { return m_bOwned; }
    }

    //---------------------------------------------------------------------------------------------------
    public CsDisassembleResultItem(CsItem csItem, int nCount, bool bOwned)
    {
        m_csItem = csItem;
        m_nCount = nCount;
        m_bOwned = bOwned;
    }
}
