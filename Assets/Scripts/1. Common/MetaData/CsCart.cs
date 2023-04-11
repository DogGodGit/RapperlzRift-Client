using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-24)
//---------------------------------------------------------------------------------------------------

public class CsCart
{
	int m_nCartId;                  // 카트ID
	string m_strName;               // 이름
	string m_strPrefabName;         // 프리팹이름
	CsCartGrade m_csCartGrade;      // 등급
	float m_flRidableRange;         // 탑승가능거리 
	float m_flRadius;				// 반지름
	
	//---------------------------------------------------------------------------------------------------
	public int CartId
	{
		get { return m_nCartId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string PrefabName
	{
		get { return m_strPrefabName; }
	}

	public CsCartGrade CartGrade
	{
		get { return m_csCartGrade; }
	}

	public float RidableRange
	{
		get { return m_flRidableRange; }
	}

	public float Radius
	{
		get { return m_flRadius; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCart(WPDCart cart)
	{
		m_nCartId = cart.cartId;
		m_strName = CsConfiguration.Instance.GetString(cart.nameKey);
		m_strPrefabName = cart.prefabName;
		m_csCartGrade = CsGameData.Instance.GetCartGrade(cart.grade);
		m_flRidableRange = cart.ridableRange;
		m_flRadius = cart.radius;
	}
}
