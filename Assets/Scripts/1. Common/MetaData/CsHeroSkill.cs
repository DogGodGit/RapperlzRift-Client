using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-29)
//---------------------------------------------------------------------------------------------------

public class CsHeroSkill
{
	CsJobSkill m_csJobSkill;
	CsJobSkillLevel m_csJobSkillLevel;
	CsJobSkillMaster m_csJobSkillMaster;

	float m_flRemainCoolTime;
	bool m_bIsClicked;
	int m_nChainSkillSelectedIndex;

	//---------------------------------------------------------------------------------------------------
	public CsJobSkill JobSkill
	{
		get { return m_csJobSkill; }
	}

	public int SkillLevel
	{
		set { m_csJobSkillLevel = m_csJobSkill.GetJobSkillLevel(value);	}
	}

	public CsJobSkillLevel JobSkillLevel
	{
		get { return m_csJobSkillLevel; }
	}

	public CsJobSkillMaster JobSkillMaster
	{
		get { return m_csJobSkillMaster; }
	}

	public string Summary
	{
		get { return m_csJobSkillLevel.Summary; }
	}

	public bool IsLevelUp
	{
		get
		{
			if (CsMainQuestManager.Instance.MainQuest != null && CsMainQuestManager.Instance.MainQuest.MainQuestNo <= m_csJobSkillMaster.OpenRequiredMainQuestNo)
			{
				return false;
			}

			if (m_csJobSkillMaster.IsMaxLevel(m_csJobSkillLevel.Level))
			{
				return false;
			}
			else
			{
				CsJobSkillLevelMaster csJobSkillLevelMaster = m_csJobSkillMaster.GetJobSkillLevelMaster(m_csJobSkillLevel.Level);
				
				// 레벨체크
				if (csJobSkillLevelMaster.NextLevelUpRequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
				{
					// 골드체크
					if (csJobSkillLevelMaster.NextLevelUpGold <= CsGameData.Instance.MyHeroInfo.Gold)
					{
						// 아이템 체크
						if (csJobSkillLevelMaster.NextLevelUpItem != null)
						{
							int nCount = CsGameData.Instance.MyHeroInfo.GetItemCount(csJobSkillLevelMaster.NextLevelUpItem.ItemId);

							// 아이템 개수 체크
							if (nCount >= csJobSkillLevelMaster.NextLevelUpItemCount)
							{
								return true;
							}
							else
							{
								return false;
							}
						}
						else
						{
							return true;
						}
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}
		}
	}

	public float RemainCoolTime
	{
		get { return m_flRemainCoolTime; }
		set
		{
			m_flRemainCoolTime = value;
			if (m_flRemainCoolTime < 0)
				m_flRemainCoolTime = 0;
		}
	}

	public EnFormType FormType
	{
		get { return (EnFormType)m_csJobSkill.FormType; }
	}

	public bool IsClicked
	{
		get { return m_bIsClicked; }
		set { m_bIsClicked = value; }
	}

	public CsJobChainSkill CurrentJobChainSkill
	{
		get { return m_csJobSkill.JobChainSkillList[m_nChainSkillSelectedIndex]; }
	}

	public float CoolTimeProgress
	{
		get { return m_flRemainCoolTime / m_csJobSkill.CoolTime; }
	}

	public float ChainSkillCoolTimeProgress
	{
		get
		{
			if (CurrentJobChainSkill.CastConditionStartTime == 0)
			{
				return 0;
			}
			else
			{
				return m_flRemainCoolTime / CurrentJobChainSkill.CastConditionStartTime;
			}
		}
	}

	public int ChainSkillSelectedIndex
	{
		get { return m_nChainSkillSelectedIndex; }
		set { m_nChainSkillSelectedIndex = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroSkill(int nJobId, PDHeroSkill heroSkill)
	{
		m_csJobSkill = CsGameData.Instance.GetJobSkill(nJobId, heroSkill.skillId);
		m_csJobSkillMaster = CsGameData.Instance.GetJobSkillMaster(heroSkill.skillId);
		m_csJobSkillLevel = m_csJobSkill.GetJobSkillLevel(heroSkill.level);
	}

	//---------------------------------------------------------------------------------------------------
	public void Reset()
	{
		m_flRemainCoolTime = 0;
		m_bIsClicked = false;
		m_nChainSkillSelectedIndex = 0;
	}

	//---------------------------------------------------------------------------------------------------
	public void StartUseSkill(bool bChainSkill = false)
	{
		if (!bChainSkill)
		{
			m_flRemainCoolTime = m_csJobSkill.CoolTime;
		}
		else
		{
			m_flRemainCoolTime = CurrentJobChainSkill.CastConditionStartTime;
		}
	}
}
