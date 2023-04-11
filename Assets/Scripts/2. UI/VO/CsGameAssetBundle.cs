//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-05-24)
//---------------------------------------------------------------------------------------------------

public class CsGameAssetBundle
{
    int m_nBundleNo;
    string m_strFileName;
    int m_nAndroidVer;
    int m_nIosVer;

    //---------------------------------------------------------------------------------------------------
    public int BundleNo
    {
        get { return m_nBundleNo; }
    }

    public string FileName
    {
        get { return m_strFileName; }
    }

    public int AndroidVer
    {
        get { return m_nAndroidVer; }
    }

    public int IosVer
    {
        get { return m_nIosVer; }
    }

    //---------------------------------------------------------------------------------------------------
    public CsGameAssetBundle(int nBundleNo, string strFileName, int nAndroidVer, int nIosVer)
    {
        m_nBundleNo = nBundleNo;
        m_strFileName = strFileName;
        m_nAndroidVer = nAndroidVer;
        m_nIosVer = nIosVer;
    }
}
