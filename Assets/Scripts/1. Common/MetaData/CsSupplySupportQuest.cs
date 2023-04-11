using System.Collections.Generic;
using WebCommon;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-13)
//---------------------------------------------------------------------------------------------------

public class CsSupplySupportQuest
{
	string m_strTargetTitle;
	string m_strTargetContent;
	int m_nRequiredHeroLevel;
	CsNpcInfo m_csNpcStart;
	CsNpcInfo m_csNpcCompletion;
	int m_nLimitCount;
	int m_nGuaranteeGold;
	int m_nLimitTime;
	string m_strStartDialogue;
	string m_strCompletionDialogue;
	string m_strCompletionText;
	string m_strFailGuideImageName;
	string m_strFailGuideTitle;
	string m_strFailGuideContent;


	List<CsSupplySupportQuestOrder> m_listCsSupplySupportQuestOrder;
	List<CsSupplySupportQuestWayPoint> m_listCsSupplySupportQuestWayPoint;
	List<CsSupplySupportQuestCart> m_listCsSupplySupportQuestCart;

	//---------------------------------------------------------------------------------------------------
	public string TargetTitle
	{
		get { return m_strTargetTitle; }
	}

	public string TargetContent
	{
		get { return m_strTargetContent; }
	}

	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
	}

	public CsNpcInfo StartNpc
	{
		get { return m_csNpcStart; }
	}

	public CsNpcInfo CompletionNpc
	{
		get { return m_csNpcCompletion; }
	}

	public int LimitCount
	{
		get { return m_nLimitCount; }
	}

	public int GuaranteeGold
	{
		get { return m_nGuaranteeGold; }
	}

	public int LimitTime
	{
		get { return m_nLimitTime; }
	}

	public string StartDialogue
	{
		get { return m_strStartDialogue; }
	}

	public string CompletionDialogue
	{
		get { return m_strCompletionDialogue; }
	}

	public string CompletionText
	{
		get { return m_strCompletionText; }
	}

	public string FailGuideImageName
	{
		get { return m_strFailGuideImageName; }
	}

	public string FailGuideTitle
	{
		get { return m_strFailGuideTitle; }
	}

	public string FailGuideContent
	{
		get { return m_strFailGuideContent; }
	}

	public List<CsSupplySupportQuestOrder> SupplySupportQuestOrderList
	{
		get { return m_listCsSupplySupportQuestOrder; }
	}

	public List<CsSupplySupportQuestWayPoint> SupplySupportQuestWayPointList
	{
		get { return m_listCsSupplySupportQuestWayPoint; }
	}

	public List<CsSupplySupportQuestCart> SupplySupportQuestCartList
	{
		get { return m_listCsSupplySupportQuestCart; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSupplySupportQuest(WPDSupplySupportQuest supplySupportQuest)
	{
		m_strTargetTitle = CsConfiguration.Instance.GetString(supplySupportQuest.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(supplySupportQuest.targetContentKey);
		m_nRequiredHeroLevel = supplySupportQuest.requiredHeroLevel;
		m_csNpcStart = CsGameData.Instance.GetNpcInfo(supplySupportQuest.startNpcId);
		m_csNpcCompletion = CsGameData.Instance.GetNpcInfo(supplySupportQuest.completionNpcId);
		m_nLimitCount = supplySupportQuest.limitCount;
		m_nGuaranteeGold = supplySupportQuest.guaranteeGold;
		m_nLimitTime = supplySupportQuest.limitTime;
		m_strStartDialogue = CsConfiguration.Instance.GetString(supplySupportQuest.startDialogueKey);
		m_strCompletionDialogue = CsConfiguration.Instance.GetString(supplySupportQuest.completionDialogueKey);
		m_strCompletionText = CsConfiguration.Instance.GetString(supplySupportQuest.completionTextKey);
		m_strFailGuideImageName = supplySupportQuest.failGuideImageName;
		m_strFailGuideTitle = CsConfiguration.Instance.GetString(supplySupportQuest.failGuideTitleKey);
		m_strFailGuideContent = CsConfiguration.Instance.GetString(supplySupportQuest.failGuideContentKey);

		m_listCsSupplySupportQuestOrder = new List<CsSupplySupportQuestOrder>();
		m_listCsSupplySupportQuestWayPoint = new List<CsSupplySupportQuestWayPoint>();
		m_listCsSupplySupportQuestCart = new List<CsSupplySupportQuestCart>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsSupplySupportQuestCart GetSupplySupportQuestCart(int nCartId)
	{
		for (int i = 0; i < m_listCsSupplySupportQuestCart.Count; i++)
		{
			if (m_listCsSupplySupportQuestCart[i].CartId == nCartId)
			{
				return m_listCsSupplySupportQuestCart[i];
			}
		}

		return null;
	}

    //---------------------------------------------------------------------------------------------------
    public CsSupplySupportQuestOrder GetSupplySupportQuestOrder(int nOrderId)
    {
        for (int i = 0; i < m_listCsSupplySupportQuestOrder.Count; i++)
        {
            if (m_listCsSupplySupportQuestOrder[i].OrderId == nOrderId)
            {
                return m_listCsSupplySupportQuestOrder[i];
            }
        }

        return null;
    }
	//---------------------------------------------------------------------------------------------------
	public CsSupplySupportQuestWayPoint GetSupplySupportQuestWayPoint(int nWayPojnt)
	{
		for (int i = 0; i < m_listCsSupplySupportQuestWayPoint.Count; i++)
		{
			if (m_listCsSupplySupportQuestWayPoint[i].WayPoint == nWayPojnt)
			{
				return m_listCsSupplySupportQuestWayPoint[i];
			}
		}

		return null;
	}
	
}
