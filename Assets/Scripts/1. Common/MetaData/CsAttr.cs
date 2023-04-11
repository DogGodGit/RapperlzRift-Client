using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-12)
//---------------------------------------------------------------------------------------------------

/*
* 속성ID
1 : 최대HP
2 : 물리공격
3 : 마법공격
4 : 물리방어
5 : 마법방어
6 : 치명
7 : 치명저항
8 : 치명타피해(%)
9 : 치명타피해감면(%)
10 : 관통
11 : 막기
12 : 화염강화
13 : 화염보호
14 : 번개강화
15 : 번개보호
16 : 암흑강화
17 : 암흑보호
18 : 신성강화
19 : 신성보호
20 : 피해추가(%)
21 : 피해감면(%)
22 : 기절저항(%)
23 : 고정저항(%)
24 : 침묵저항(%)
25 : 기본 최대 HP 추가 (%)
26 : 기본 공격력 추가 (%)
27 : 기본 물리방어력 추가 (%)
28 : 기본 마법방어력 추가 (%)
29 : 공격(물리/방어)
*/

public enum EnAttr
{
	MaxHp = 1,
	PhysicalOffense = 2,
	MagicalOffense = 3,
	PhysicalDefense = 4,
	MagicalDefense = 5,
	EnchantFire = 12,
    ProtectFire = 13, 
	EnchantElectric = 14,
    ProtectElectric = 15, 
	EnchantDark = 16,
    ProtectDark = 17,
	EnchantLight = 18,
    ProtectLight = 19, 
	OffenseAddPer = 26,
	PhysicalDefenseAddPer = 27,
	MagicalDefenseAddPer = 28,
	Attack = 29,
}

public class CsAttr
{
	int m_nAttrId;							// 속성ID. 
	string m_strName;						// 이름
	int m_nBattlePowerFactor;				// 설명
	CsAttrCategory m_csAttrCategory;        // 속성카테고리ID
	int m_nSortNo;							// 정렬번호


	//---------------------------------------------------------------------------------------------------
	public int AttrId
	{
		get { return m_nAttrId; }
	}

	public EnAttr EnAttr
	{
		get { return (EnAttr)m_nAttrId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public int BattlePowerFactor
	{
		get { return m_nBattlePowerFactor; }
	}

	public CsAttrCategory AttrCategory
	{
		get { return m_csAttrCategory; }
	}

	public int SortNo
	{
		get { return m_nSortNo; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsAttr(WPDAttr attr)
	{
		m_nAttrId = attr.attrId;
		m_strName = CsConfiguration.Instance.GetString(attr.nameKey);
		m_nBattlePowerFactor = attr.battlePowerFactor;
		m_csAttrCategory = CsGameData.Instance.GetAttrCategory(attr.attrCategoryId);
		m_nSortNo = attr.sortNo;
	}

}
