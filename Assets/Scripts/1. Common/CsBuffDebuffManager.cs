using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientCommon;

// UI 표시 용도로만 사용(전투력미반영)
public class CsBuffDebuffManager : MonoBehaviour 
{
	const float m_flDefaultFactor = 1.0f;

	int m_nMoneyBuffId = -1;
	Dictionary<CsAttr, float> m_dicDungeonBuffDebuffAttrFactor;
	Dictionary<CsAttr, float> m_dicBuffDebuffAttrFactor;
	Dictionary<CsAttr, int> m_dicBuffDebuffAttrValue;
	Dictionary<long, CsAbnormalStateLevel> m_dicAbnormalState;

	//---------------------------------------------------------------------------------------------------
	public CsBuffDebuffManager()
	{
		CsRplzSession.Instance.EventEvtHeroAbnormalStateEffectStart += OnEventEvtHeroAbnormalStateEffectStart;
		CsRplzSession.Instance.EventEvtHeroAbnormalStateEffectFinished += OnEventEvtHeroAbnormalStateEffectFinished;
		CsGameEventToUI.Instance.EventHeroDead += OnEventHeroDead;

		m_dicDungeonBuffDebuffAttrFactor = new Dictionary<CsAttr, float>();
		m_dicBuffDebuffAttrFactor = new Dictionary<CsAttr, float>();
		m_dicBuffDebuffAttrValue = new Dictionary<CsAttr, int>();
		m_dicAbnormalState = new Dictionary<long,CsAbnormalStateLevel>();

		ResetAttrs();
	}

	//---------------------------------------------------------------------------------------------------
	public static CsBuffDebuffManager Instance
	{
		get { return CsSingleton<CsBuffDebuffManager>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	public void Init()
	{
		
	}

	//---------------------------------------------------------------------------------------------------
	void ResetAttrs()
	{
		m_dicBuffDebuffAttrFactor.Clear();
		m_dicBuffDebuffAttrValue.Clear();

		foreach (CsAttr csAttr in CsGameData.Instance.AttrList)
		{
			m_dicBuffDebuffAttrFactor.Add(csAttr, m_flDefaultFactor);
			m_dicBuffDebuffAttrValue.Add(csAttr, 0);
		}
	}


	//---------------------------------------------------------------------------------------------------
	public void SetMoneyBuff(int nBuffId)
	{
		CsMoneyBuff csMoneyBuff = CsGameData.Instance.GetMoneyBuff(nBuffId);

		if (csMoneyBuff != null)
		{
			m_nMoneyBuffId = nBuffId;

			foreach (var moneyBuffAttr in csMoneyBuff.MoneyBuffAttrList)
			{
				AddDungeonBuffDebuffAttrFactor(moneyBuffAttr.Attr, moneyBuffAttr.AttrFactor);
			}
		}
	}

	////---------------------------------------------------------------------------------------------------
	//public void ResetMoneyBuff()
	//{
	//    CsMoneyBuff csMoneyBuff = CsGameData.Instance.GetMoneyBuff(m_nMoneyBuffId);

	//    if (csMoneyBuff != null)
	//    {
	//        m_nMoneyBuffId = -1;

	//        foreach (var moneyBuffAttr in csMoneyBuff.MoneyBuffAttrList)
	//        {
	//            RemoveDungeonBuffDebuffAttrFactor(moneyBuffAttr.Attr, moneyBuffAttr.AttrFactor);
	//        }
	//    }
	//}

	//---------------------------------------------------------------------------------------------------
	public void AddDungeonBuffDebuffAttrFactor(CsAttr csAttr, float flValue)
	{
		if (m_dicDungeonBuffDebuffAttrFactor.ContainsKey(csAttr))
		{
			m_dicDungeonBuffDebuffAttrFactor[csAttr] += flValue;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void ResetDungeonBuffDebuffAttrFactor()
	{
		foreach (var key in m_dicDungeonBuffDebuffAttrFactor.Keys)
		{
			m_dicDungeonBuffDebuffAttrFactor[key] = m_flDefaultFactor;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void RemoveDungeonBuffDebuffAttrFactor(CsAttr csAttr, float flValue)
	{
		if (m_dicDungeonBuffDebuffAttrFactor.ContainsKey(csAttr))
		{
			m_dicDungeonBuffDebuffAttrFactor[csAttr] -= flValue;

			if (m_dicDungeonBuffDebuffAttrFactor[csAttr] < 0f)
			{
				m_dicDungeonBuffDebuffAttrFactor[csAttr] = 0f;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void AddBuffDebuffAttrFactor(CsAttr csAttr, float flValue)
	{
		if (m_dicBuffDebuffAttrFactor.ContainsKey(csAttr))
		{
			m_dicBuffDebuffAttrFactor[csAttr] += (flValue / 10000.0f);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void RemoveBuffDebuffAttrFactor(CsAttr csAttr, float flValue)
	{
		if (m_dicBuffDebuffAttrFactor.ContainsKey(csAttr))
		{
			m_dicBuffDebuffAttrFactor[csAttr] -= (flValue / 10000.0f);

			if (m_dicBuffDebuffAttrFactor[csAttr] < 0f)
			{
				m_dicBuffDebuffAttrFactor[csAttr] = 0f;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void AddBuffDebuffAttrValue(CsAttr csAttr, int nValue)
	{
		if (m_dicBuffDebuffAttrValue.ContainsKey(csAttr))
		{
			m_dicBuffDebuffAttrValue[csAttr] += nValue;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void RemoveBuffDebuffAttrValue(CsAttr csAttr, int nValue)
	{
		if (m_dicBuffDebuffAttrValue.ContainsKey(csAttr))
		{
			m_dicBuffDebuffAttrValue[csAttr] -= nValue;

			if (m_dicBuffDebuffAttrValue[csAttr] < 0)
			{
				m_dicBuffDebuffAttrValue[csAttr] = 0;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public float GetBuffDebuffAttrFactor(CsAttr csAttr)
	{
		if (m_dicBuffDebuffAttrFactor.ContainsKey(csAttr))
		{
			return m_dicBuffDebuffAttrFactor[csAttr];
		}

		return m_flDefaultFactor;
	}

	//---------------------------------------------------------------------------------------------------
	public int GetBuffDebuffAttrValue(CsAttr csAttr)
	{
		if (m_dicBuffDebuffAttrValue.ContainsKey(csAttr))
		{
			return m_dicBuffDebuffAttrValue[csAttr];
		}

		return 0;
	}

	//---------------------------------------------------------------------------------------------------
	void SetAbnormalState(CsAbnormalStateLevel csAbnormalStateLevel)
	{
		switch (csAbnormalStateLevel.AbnormalStateId)
		{
			case 5:
				RemoveBuffDebuffAttrFactor(CsGameData.Instance.GetAttr(2), csAbnormalStateLevel.Value1);
				RemoveBuffDebuffAttrFactor(CsGameData.Instance.GetAttr(3), csAbnormalStateLevel.Value1);
				break;
			case 6:
				RemoveBuffDebuffAttrFactor(CsGameData.Instance.GetAttr(4), csAbnormalStateLevel.Value1);
				RemoveBuffDebuffAttrFactor(CsGameData.Instance.GetAttr(5), csAbnormalStateLevel.Value1);
				break;
			case 13:
				AddBuffDebuffAttrFactor(CsGameData.Instance.GetAttr(6), csAbnormalStateLevel.Value1);
				AddBuffDebuffAttrFactor(CsGameData.Instance.GetAttr(8), csAbnormalStateLevel.Value2);
				break;
			case 103:
			case 104:
			case 105:
			case 106:
			case 107:
				AddBuffDebuffAttrFactor(CsGameData.Instance.GetAttr(2), csAbnormalStateLevel.Value1);
				AddBuffDebuffAttrValue(CsGameData.Instance.GetAttr(2), csAbnormalStateLevel.Value2);
				break;
			case 108:
			case 109:
			case 110:
				AddBuffDebuffAttrFactor(CsGameData.Instance.GetAttr(3), csAbnormalStateLevel.Value1);
				AddBuffDebuffAttrValue(CsGameData.Instance.GetAttr(3), csAbnormalStateLevel.Value2);
				AddBuffDebuffAttrFactor(CsGameData.Instance.GetAttr(5), csAbnormalStateLevel.Value1);
				AddBuffDebuffAttrValue(CsGameData.Instance.GetAttr(5), csAbnormalStateLevel.Value2);
				break;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtHeroAbnormalStateEffectStart(SEBHeroAbnormalStateEffectStartEventBody csEvt)
	{
		if (CsGameData.Instance.MyHeroInfo.HeroId == csEvt.heroId)
		{
			CsAbnormalState csAbnormalState = CsGameData.Instance.GetAbnormalState(csEvt.abnormalStateId);

			if (csAbnormalState != null)
			{
				CsAbnormalStateLevel csAbnormalStateLevel = csAbnormalState.GetAbnormalStateLevel(csEvt.sourceJobId, csEvt.level);

				if (csAbnormalStateLevel != null)
				{
					if (!m_dicAbnormalState.ContainsKey(csEvt.abnormalStateEffectInstanceId))
					{
						m_dicAbnormalState.Add(csEvt.abnormalStateEffectInstanceId, csAbnormalStateLevel);

						SetAbnormalState(csAbnormalStateLevel);
					}
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtHeroAbnormalStateEffectFinished(SEBHeroAbnormalStateEffectFinishedEventBody csEvt)
	{
		if (CsGameData.Instance.MyHeroInfo.HeroId == csEvt.heroId)
		{
			if (m_dicAbnormalState.ContainsKey(csEvt.abnormalStateEffectInstanceId))
			{
				m_dicAbnormalState.Remove(csEvt.abnormalStateEffectInstanceId);

				ResetAttrs();

				foreach (var key in m_dicAbnormalState.Keys)
				{
					SetAbnormalState(m_dicAbnormalState[key]);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroDead(string strName)
	{
		if (CsUIData.Instance.DungeonInNow == EnDungeon.ProofOfValor ||
			CsUIData.Instance.DungeonInNow == EnDungeon.InfiniteWar)
		{
			// 용맹의증명, 무한대전 사망 시 상자로 획득한 버프 삭제
			CsBuffDebuffManager.Instance.ResetDungeonBuffDebuffAttrFactor();
		}
	}
}
