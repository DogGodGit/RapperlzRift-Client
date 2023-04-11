using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-23)
//---------------------------------------------------------------------------------------------------

public class CsCreatureCardCollectionAttr
{
	int m_nCollectionId;
	CsAttr m_csAttr;
	CsAttrValueInfo m_csAttrValue;

	//---------------------------------------------------------------------------------------------------
	public int CollectionId
	{
		get { return m_nCollectionId; }
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
		get { return m_csAttr.BattlePowerFactor * m_csAttrValue.Value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureCardCollectionAttr(WPDCreatureCardCollectionAttr creatureCardCollectionAttr)
	{
		m_nCollectionId = creatureCardCollectionAttr.collectionId;
		m_csAttr = CsGameData.Instance.GetAttr(creatureCardCollectionAttr.attrId);
		m_csAttrValue = CsGameData.Instance.GetAttrValueInfo(creatureCardCollectionAttr.attrValueId);
	}
}
