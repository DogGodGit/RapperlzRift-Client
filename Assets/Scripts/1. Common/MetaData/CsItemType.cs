using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-12)
//---------------------------------------------------------------------------------------------------

/*
1 : HP물약
2 : 마을귀환서
3 : 메인장비상자
4 : 뽑기상자
5 : 확성기 아이템
6 : 경험치물약
7 : 탈것레벨업 아이템
8 : 탈 것 재강화 아이템
9 : 탈 것 제작 아이템
10 : 소탕권
11 : 경험치스크롤
12 : 날개강화(깃털)
13 : 현상수배서
14 : 낚시미끼
15 : 룬조각
16 : 왜곡주문서(휴전)
17 : 보급지령서
18 : 골드
19 : 귀속다이아
20 : 명예점수
21 : 공적점수
22 : 군량미아이템
23 : 길드소집아이템
24 : 국가소집아이템
25 : 칭호
26 : 도감
27 : 프리시페조각
28 : 탈것
29 : 날개
30 : NPC상점교환아이템
31 : 영혼석
32 : NPC상점교환아이템조각
33 : 힘의결정
34 : 힘의결정조각
35 : 오픈7일교환권
36 : 영석
37 : 크리처먹이
38 : 크리처정수
39 : 크리처알
40 : 크리처알조각
41 : 던전입장권
42 : 전기아이템
43 : 코스튬
44 : 코스튬효과
45 : 속성물약
46 : 별자리
47 : 고급별자리
48 : 탈것속성물약
49 : 코스튬강화아이템
50 : 코스튬활성화아이템
51 : 코스튬셔플티켓
52 : 탈것각성아이템
53 : 크리처스킬잠금아이템
54 : 엘릭서아이템
55 : 업적점수아이템

101 : 생명의 돌(소울스톤)
102 : 용기의 돌(소울스톤)
103 : 수호의 돌(소울스톤)
104 : 저항의 돌(소울스톤)
105 : 활력의 룬
106 : 맹공의 룬
107 : 인내의 룬
108 : 저항의 룬
109 : 화염의 룬
110 : 번개의 룬
111 : 신성의 룬
112 : 어둠의 룬
113 : 화염보호의 룬
114 : 번개보호의 룬
115 : 신성보호의 룬
116 : 어둠보호의 룬
*/

public enum EnItemType
{
	HpPotion = 1,
    ReturnScroll = 2,
	MainGearBox = 3,
	PickBox = 4,
	Speaker = 5,
    ExpPotion = 6,
	MountLevelUp = 7,
	MountGearRefine = 8,
	MountGearPickBoxMake = 9,
    ExpScroll = 11,
	WingEnchant = 12,
	BountyHunter = 13,
	FishingBait = 14,
	RunePiece = 15,
    DistortionScroll = 16,
	SupplySupport = 17,
	Gold = 18,
	OwnDia = 19,
	HonorPoint = 20,
	ExploitPoint = 21,
	GuildCall = 23,
    NationCall = 24, 
    Title = 25,
    IllustratedBook = 26,
	Mount = 28,
	Wing = 29,
	SpiritStone = 31,
	CreatureFood = 37,
	CreatureEssence = 38,
	CreatureEgg = 39,
	BiographyItem = 42,
    Costume = 43,
    CostumeEffect = 44,
    PotionAttr = 45, 
	StarEssense = 46,
	PremiumStarEssense = 47, 
    MountPotionAttr = 48, 
    CostumeEnchant = 49, 
    MountAwakening = 52, 
	AccomplishmentPoint = 55,
}

public class CsItemType
{
	int m_nItemType;									// 아이템타입
	int m_nMaxCountPerInventorySlot;					// 인벤토리슬롯당최대캐수
	CsItemMainCategory m_csItemMainCategory;            // 메인카테고리
	CsItemSubCategory m_csItemSubCategory;				// 서브카테고리

	//---------------------------------------------------------------------------------------------------
	public int ItemType
	{
		get { return m_nItemType; }
	}

	public EnItemType EnItemType
	{
		get { return (EnItemType)m_nItemType; }
	}

	public int MaxCountPerInventorySlot
	{
		get { return m_nMaxCountPerInventorySlot; }
	}

	public CsItemMainCategory MainCategory
	{
		get { return m_csItemMainCategory; }
	}

	public CsItemSubCategory SubCategory
	{
		get { return m_csItemSubCategory; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsItemType(WPDItemType itemType)
	{
		m_nItemType = itemType.itemType;
		m_nMaxCountPerInventorySlot = itemType.maxCountPerInventorySlot;
		m_csItemMainCategory = CsGameData.Instance.GetItemMainCategory(itemType.mainCategoryId);

		if (m_csItemMainCategory != null)
		{
			m_csItemSubCategory = m_csItemMainCategory.GetItemSubCategory(itemType.subCategoryId);
		}
	}
}
