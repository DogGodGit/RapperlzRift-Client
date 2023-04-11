using System.Collections.Generic;
using WebCommon;
//using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-23)
//---------------------------------------------------------------------------------------------------

public enum EnMainMenu
{
    Character = 1,
    Skill = 2,
    MainGear = 3,
    SubGear = 4,
    Mail = 5,
    Party = 6,
    Support = 7,
    QuestList = 8,
    Mount = 9,
    Wing = 10,
    Dungeon = 11,
    Vip = 12,
    Minimap = 13,
    TodayTask = 14,
    Class = 15,
    Ranking = 16,
    ViewOtherUsers = 17,
    Setting = 18,
    Guild = 19,
    Nation = 20,
    NationWar = 21,
    Achievement = 22,
    Collection = 23,
    DailyQuest = 24, 
    WeeklyQuest = 25, 
    DiaShop = 26, 
    LuckyShop = 27, 
    Friend = 28, 
	Creature = 29,
	Soul = 30,
	Constellation = 31,
}

public class CsMainMenu
{
    int m_nMenuId;                      // 메인메뉴ID
    string m_strName;                   // 이름
    string m_strPopupName;              // 팝업이름

    List<CsSubMenu> m_listCsSubMenu;    // 서브메뉴리스트

    //---------------------------------------------------------------------------------------------------
    public int MenuId
    {
        get { return m_nMenuId; }
    }

    public string Name
    {
        get { return m_strName; }
    }

    public string PopupName
    {
        get { return m_strPopupName; }
    }

    public List<CsSubMenu> SubMenuList
    {
        get { return m_listCsSubMenu; }
    }

    //---------------------------------------------------------------------------------------------------
    public CsMainMenu(WPDMainMenu mainMenu)
    {
        m_nMenuId = mainMenu.menuId;
        m_strName = CsConfiguration.Instance.GetString(mainMenu.nameKey);
        m_strPopupName = mainMenu.popupName;

        m_listCsSubMenu = new List<CsSubMenu>();
    }

    //---------------------------------------------------------------------------------------------------
    public CsSubMenu GetSubMenu(int nSubMenuId)
    {
        for (int i = 0; i < m_listCsSubMenu.Count; i++)
        {
            if (m_listCsSubMenu[i].SubMenuId == nSubMenuId)
            {
                return m_listCsSubMenu[i];
            }
        }

        return null;
    }

    //---------------------------------------------------------------------------------------------------
    public void SortSubMenuList()
    {
        m_listCsSubMenu.Sort();
    }
}
