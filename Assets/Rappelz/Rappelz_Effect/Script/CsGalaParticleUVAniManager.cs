using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsGalaParticleUVAniManager : MonoBehaviour {

    public float m_fScrollSpeedX = 0.0f;
    public float m_fScrollSpeedY = 0.0f;

    public float m_fTilingX = 1;
    public float m_fTilingY = 1;

    public float m_fOffsetX = 0;
    public float m_fOffsetY = 0;

    public bool m_bUseSmoothDeltaTime = false;
    public bool m_bFixedTileSize = false;
    public bool m_bRepeat = true;
    public bool m_bAutoDestruct = false;

    protected Vector3 m_OriginalScale = new Vector3();
    protected Vector2 m_OriginalTiling = new Vector2();
    protected Vector2 m_EndOffset = new Vector2();
    protected Vector2 m_RepeatOffset = new Vector2();
    protected Renderer m_Renderer;

    private float m_fStartOffsetX = 0;
    private float m_fStartOffsetY = 0;

    // Use this for initialization

    bool IsCreatingEditObject()
    {
        GameObject main = GameObject.Find("_FXMaker");
        if (main == null)
            return false;
        GameObject effroot = GameObject.Find("_CurrentObject");
        if (effroot == null)
            return false;
        return (effroot.transform.childCount == 0);
    }

    void Awake()
    {
        m_fStartOffsetX = m_fOffsetX;
        m_fStartOffsetY = m_fOffsetY;
    }

    void OnEnable()
    {
        #if UNITY_EDITOR
            if (IsCreatingEditObject() == false)
                UpdateBillboard();
        #else
 		    UpdateBillboard();
        #endif

        m_fOffsetX = m_fStartOffsetX;
        m_fOffsetY = m_fStartOffsetY;
    }

    public void UpdateBillboard()
    {
        if (enabled)
            Update();
    }

    void Start () {

        if (GetComponent<Renderer>() != null)
        {
            m_Renderer = GetComponent<Renderer>();
            m_Renderer.material.mainTextureScale = new Vector2(m_fTilingX, m_fTilingY);
            // 0~1 value
            float offset;
            offset = m_fOffsetX + m_fTilingX;
            m_RepeatOffset.x = offset - (int)(offset);
            if (m_RepeatOffset.x < 0)
                m_RepeatOffset.x += 1;
            offset = m_fOffsetY + m_fTilingY;
            m_RepeatOffset.y = offset - (int)(offset);
            if (m_RepeatOffset.y < 0)
                m_RepeatOffset.y += 1;
            m_EndOffset.x = 1 - (m_fTilingX - (int)(m_fTilingX) + ((m_fTilingX - (int)(m_fTilingX)) < 0 ? 1 : 0));
            m_EndOffset.y = 1 - (m_fTilingY - (int)(m_fTilingY) + ((m_fTilingY - (int)(m_fTilingY)) < 0 ? 1 : 0));
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.main == null)
            return;
        if (m_Renderer != null)
        {
            if (m_bFixedTileSize)
            {
                if (m_fScrollSpeedX != 0 && m_OriginalScale.x != 0)
                    m_fTilingX = m_OriginalTiling.x * (transform.lossyScale.x / m_OriginalScale.x);
                if (m_fScrollSpeedY != 0 && m_OriginalScale.y != 0)
                    m_fTilingY = m_OriginalTiling.y * (transform.lossyScale.y / m_OriginalScale.y);
                GetComponent<Renderer>().material.mainTextureScale = new Vector2(m_fTilingX, m_fTilingY);
            }

            if (m_bUseSmoothDeltaTime)
            {
                m_fOffsetX += Time.smoothDeltaTime * m_fScrollSpeedX;
                m_fOffsetY += Time.smoothDeltaTime * m_fScrollSpeedY;
            }
            else
            {
                m_fOffsetX += Time.deltaTime * m_fScrollSpeedX;
                m_fOffsetY += Time.deltaTime * m_fScrollSpeedY;
            }

            if (m_bRepeat == false)
            {
                m_RepeatOffset.x += Time.deltaTime * m_fScrollSpeedX;
                if (m_RepeatOffset.x < 0 || 1 < m_RepeatOffset.x)
                {
                    m_fOffsetX = m_EndOffset.x;
                    enabled = false;
                }
                m_RepeatOffset.y += Time.deltaTime * m_fScrollSpeedY;
                if (m_RepeatOffset.y < 0 || 1 < m_RepeatOffset.y)
                {
                    m_fOffsetY = m_EndOffset.y;
                    enabled = false;
                }
            }
            m_Renderer.material.mainTextureOffset = new Vector2(m_fOffsetX - ((int)m_fOffsetX), m_fOffsetY - ((int)m_fOffsetY));
        }
    }

}

