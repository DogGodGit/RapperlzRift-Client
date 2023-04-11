//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-30)
//---------------------------------------------------------------------------------------------------

public class CsAttrValue
{
    CsAttr m_csAttr;
    int m_nValue;

    //---------------------------------------------------------------------------------------------------
    public CsAttr Attr
    {
        get { return m_csAttr; }
    }

    public int Value
    {
        get { return m_nValue; }
    }

    public int BattlePowerValue
    {
        get { return m_csAttr.BattlePowerFactor * m_nValue; }
    }

    //---------------------------------------------------------------------------------------------------
    public CsAttrValue(CsAttr csAttr, int nValue)
    {
        m_csAttr = csAttr;
        m_nValue = nValue;
    }
}
