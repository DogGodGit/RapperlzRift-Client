using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum EnUIAnimationType
{
    None = 0,
    MoveOn,
    MoveOff,
    Scale,
    Quest,
    QuestClear,
}

public class CsUiAnimation : MonoBehaviour
{

    [Header("타겟")]
    [SerializeField]
    RectTransform m_rectTarget;

    [Header("애니메이션 타입")]
    [SerializeField]
    EnUIAnimationType m_enUIAnimationType = EnUIAnimationType.None;

    [Header("이동")]
    [SerializeField] Vector2 m_v2MoveStartPos = Vector2.zero;
    [SerializeField] Vector2 m_v2MoveEndPos = Vector2.zero;
    [SerializeField] float m_flMoveSpeed = 0f;
    [SerializeField] float m_flMoveWaitingTime = 0f;

    [Header("크기")]
    [SerializeField]
    Vector2 m_v2StartScale = Vector2.zero;
    [SerializeField] Vector2 m_v2EndScale = Vector2.zero;
    [SerializeField] float m_flSclaeSpeed = 0f;
    [SerializeField] float m_flScaleWaitingTime = 0f;

    [Header("애니메이션")]
    [SerializeField]
    Animator m_animator = null;

    public EnUIAnimationType UIType
    {
        set { m_enUIAnimationType = value; }
    }

    IEnumerator m_enumerator;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        if (m_rectTarget == null)
            m_rectTarget = transform.GetComponent<RectTransform>();

        switch (m_enUIAnimationType)
        {
            case EnUIAnimationType.MoveOn:
                m_rectTarget.anchoredPosition = m_v2MoveStartPos;
                break;

            case EnUIAnimationType.MoveOff:
                m_rectTarget.anchoredPosition = m_v2MoveEndPos;
                break;

            case EnUIAnimationType.Scale:
                m_rectTarget.localScale = m_v2StartScale;
                break;

            case EnUIAnimationType.Quest:
                m_rectTarget.anchoredPosition = m_v2MoveStartPos;
                break;

            case EnUIAnimationType.QuestClear:
                m_rectTarget.anchoredPosition = m_v2MoveStartPos;
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnDisable()
    {
        switch (m_enUIAnimationType)
        {
            case EnUIAnimationType.MoveOn:
             //   m_rectTarget.anchoredPosition = m_v2MoveStartPos;
                break;

            case EnUIAnimationType.MoveOff:
              //  m_rectTarget.anchoredPosition = m_v2MoveEndPos;
                break;

            case EnUIAnimationType.Scale:
                m_rectTarget.localScale = m_v2StartScale;
                break;

            case EnUIAnimationType.Quest:
                m_rectTarget.anchoredPosition = m_v2MoveStartPos;
                break;

            case EnUIAnimationType.QuestClear:
                m_rectTarget.anchoredPosition = m_v2MoveStartPos;
                m_animator.gameObject.SetActive(false);
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void StartAinmation()
    {
        if (m_enumerator != null)
        {
            StopCoroutine(m_enumerator);
            m_enumerator = null;
        }

        switch (m_enUIAnimationType)
        {
            case EnUIAnimationType.MoveOn:
                m_enumerator = StartMoveOn();
                break;

            case EnUIAnimationType.MoveOff:
                m_enumerator = StartMoveOff();
                break;

            case EnUIAnimationType.Scale:
                m_enumerator = StartSacle();
                break;

            case EnUIAnimationType.Quest:
                m_enumerator = StartQuest();
                break;

            case EnUIAnimationType.QuestClear:
                m_enumerator = StartQuestClear();
                break;
        }

        StartCoroutine(m_enumerator);
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator StartMoveOn()
    {
        Vector2 v2 = m_v2MoveStartPos;

        if (m_flMoveWaitingTime != 0f) yield return new WaitForSeconds(m_flMoveWaitingTime);

        while (Vector2.Distance(v2, m_v2MoveEndPos) >= 1f)
        {
            v2 = Vector2.Lerp(v2, m_v2MoveEndPos, Time.deltaTime * m_flMoveSpeed);
            m_rectTarget.anchoredPosition = v2;
            yield return null;
        }

        m_rectTarget.anchoredPosition = m_v2MoveEndPos;
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator StartMoveOff()
    {
        Vector2 v2 = m_v2MoveEndPos;

        while (Vector2.Distance(v2, m_v2MoveStartPos) >= 1f)
        {
            v2 = Vector2.Lerp(v2, m_v2MoveStartPos, Time.deltaTime * m_flMoveSpeed);
            m_rectTarget.anchoredPosition = v2;
            yield return null;
        }

        m_rectTarget.anchoredPosition = m_v2MoveStartPos;
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator StartSacle()
    {
        Vector2 v2 = m_v2StartScale;

        if (m_flScaleWaitingTime != 0f) yield return new WaitForSeconds(m_flScaleWaitingTime);

        while (Vector2.Distance(v2, m_v2EndScale) >= 1f)
        {
            v2 = Vector2.Lerp(v2, m_v2EndScale, Time.deltaTime * m_flSclaeSpeed);
            m_rectTarget.localScale = v2;
            yield return null;
        }

        m_rectTarget.anchoredPosition = m_v2EndScale;
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator StartQuest()
    {
        Vector2 v2 = m_v2MoveStartPos;

        if (m_animator != null)
        {
            m_animator.transform.localScale = Vector3.one;
            m_animator.enabled = false;
            m_animator.gameObject.SetActive(true);
            m_animator.transform.Find("FX_result").gameObject.SetActive(false);
        }

        while (Vector2.Distance(v2, m_v2MoveEndPos) >= 1f)
        {
            v2 = Vector2.Lerp(v2, m_v2MoveEndPos, Time.deltaTime * m_flMoveSpeed);
            m_rectTarget.anchoredPosition = v2;
            yield return null;
        }

        m_rectTarget.anchoredPosition = m_v2MoveEndPos;
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator StartQuestClear()
    {
        Vector2 v2 = m_v2MoveStartPos;

        if (m_animator != null)
        {
            m_animator.enabled = true;
            m_animator.gameObject.SetActive(false);
            m_animator.transform.Find("FX_result").gameObject.SetActive(false);
        }

        while (Vector2.Distance(v2, m_v2MoveEndPos) >= 1f)
        {
            v2 = Vector2.Lerp(v2, m_v2MoveEndPos, Time.deltaTime * m_flMoveSpeed);
            m_rectTarget.anchoredPosition = v2;
            yield return null;
        }

        m_rectTarget.anchoredPosition = m_v2MoveEndPos;

        if (m_animator != null)
        {
            m_animator.gameObject.SetActive(true);
        }

        yield return new WaitUntil(() => m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        m_animator.transform.Find("FX_result").gameObject.SetActive(true);
    }

}
