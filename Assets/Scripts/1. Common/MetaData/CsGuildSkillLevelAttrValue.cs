using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-21)
//---------------------------------------------------------------------------------------------------

public class CsGuildSkillLevelAttrValue
{
	int m_nGuildSkillId;
	int m_nLevel;
	CsAttr m_csAttr;
	CsAttrValueInfo m_csAttrValue;

	//---------------------------------------------------------------------------------------------------
	public int GuildSkillId
	{
		get { return m_nGuildSkillId; }
	}

	public int Level
	{
		get { return m_nLevel; }
	}

	public CsAttr Attr
	{
		get { return m_csAttr; }
	}

	public CsAttrValueInfo AttrValue
	{
		get { return m_csAttrValue; }
	}

	public long BattlePower
	{
		get { return m_csAttrValue.Value * m_csAttr.BattlePowerFactor; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildSkillLevelAttrValue(WPDGuildSkillLevelAttrValue guildSkillLevelAttrValue)
	{
		m_nGuildSkillId = guildSkillLevelAttrValue.guildSkillId;
		m_nLevel = guildSkillLevelAttrValue.level;
		m_csAttr = CsGameData.Instance.GetAttr(guildSkillLevelAttrValue.attrId);
		m_csAttrValue = CsGameData.Instance.GetAttrValueInfo(guildSkillLevelAttrValue.attrValueId);
	}
}
