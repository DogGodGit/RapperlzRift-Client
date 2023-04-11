using System;
using UnityEngine;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using ClientCommon;
using SimpleDebugLog;


public class CsRplzSession : CsPhotonSession
{
	//----------------------------------------------------------------------------------------------------
	class CsRes 
	{
		public virtual void Execute(OperationResponse operationResponse){}
	}

	//----------------------------------------------------------------------------------------------------
	class CsEvt 
	{
		public virtual void Execute(EventData eventData){}
	}

	//----------------------------------------------------------------------------------------------------
	class CsRes<T> : CsRes  where T : Body
	{
		public event Delegate<int, T> Event;
		public override void Execute(OperationResponse operationResponse)
		{
			if (Event != null)
			{
				T t = Body.DeserializeRaw<T>((byte[])operationResponse.Parameters[(byte)CommandParameterCode.Body]);
				Event((int)operationResponse.ReturnCode, t);
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	class CsEvt<T> : CsEvt  where T : Body
	{
		public event Delegate<T> Event;
		public override void Execute(EventData eventData)
		{
			if (Event != null)
			{
				T t = Body.DeserializeRaw<T>((byte[])eventData.Parameters[(byte)EventParameterCode.Body]);
				Event(t);
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	private Dictionary<ClientCommandName, CsRes> m_dicRes = new Dictionary<ClientCommandName, CsRes>();
	private Dictionary<ServerEventName, CsEvt> m_dicEvt = new Dictionary<ServerEventName, CsEvt>();

	public event Delegate EventConnected;
	public event Delegate EventDisconnected;

	//----------------------------------------------------------------------------------------------------
	public void AddDelegate<T>(ClientCommandName en, Delegate<int, T> delegateFunc) where T : Body
	{
		if (!m_dicRes.ContainsKey(en))
		{
			m_dicRes.Add(en, new CsRes<T>());
		}
		((CsRes<T>)m_dicRes[en]).Event += delegateFunc;
	}

	//----------------------------------------------------------------------------------------------------
	public void DelDelegate<T>(ClientCommandName en, Delegate<int, T> delegateFunc) where T : Body
	{
		if (m_dicRes.ContainsKey(en))
		{
			((CsRes<T>)m_dicRes[en]).Event -= delegateFunc;
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void AddDelegate<T>(ServerEventName en, Delegate<T> delegateFunc) where T : Body
	{
		if (!m_dicEvt.ContainsKey(en))
		{
			m_dicEvt.Add(en, new CsEvt<T>());
		}
		((CsEvt<T>)m_dicEvt[en]).Event += delegateFunc;
	}

	//----------------------------------------------------------------------------------------------------
	public void DelDelegate<T>(ServerEventName en, Delegate<T> delegateFunc) where T : Body
	{
		if (m_dicEvt.ContainsKey(en))
		{
			((CsEvt<T>)m_dicEvt[en]).Event -= delegateFunc;
		}
	}

	//----------------------------------------------------------------------------------------------------
	#region Command
	// 서버 시간.
	public event Delegate<int, GetTimeResponseBody> EventResGetTime
	{
		add { AddDelegate<GetTimeResponseBody>(ClientCommandName.GetTime, value); }
		remove { DelDelegate<GetTimeResponseBody>(ClientCommandName.GetTime, value); }
	}

	#region Hero

	public event Delegate<int, LoginResponseBody> EventResLogin
	{
		add    {AddDelegate<LoginResponseBody>(ClientCommandName.Login, value);}
		remove {DelDelegate<LoginResponseBody>(ClientCommandName.Login, value);}
	}

	public event Delegate<int, LobbyInfoResponseBody> EventResLobbyInfo
	{
		add    {AddDelegate<LobbyInfoResponseBody>(ClientCommandName.LobbyInfo, value);}
		remove {DelDelegate<LobbyInfoResponseBody>(ClientCommandName.LobbyInfo, value);}
	}

	public event Delegate<int, HeroCreateResponseBody> EventResHeroCreate
	{
		add    {AddDelegate<HeroCreateResponseBody>(ClientCommandName.HeroCreate, value);}
		remove {DelDelegate<HeroCreateResponseBody>(ClientCommandName.HeroCreate, value);}
	}

	public event Delegate<int, HeroNamingTutorialCompleteResponseBody> EventResHeroNamingTutorialComplete
	{
		add    {AddDelegate<HeroNamingTutorialCompleteResponseBody>(ClientCommandName.HeroNamingTutorialComplete, value);}
		remove {DelDelegate<HeroNamingTutorialCompleteResponseBody>(ClientCommandName.HeroNamingTutorialComplete, value);}
	}

	public event Delegate<int, HeroNameSetResponseBody> EventResHeroNameSet
	{
		add    {AddDelegate<HeroNameSetResponseBody>(ClientCommandName.HeroNameSet, value);}
		remove {DelDelegate<HeroNameSetResponseBody>(ClientCommandName.HeroNameSet, value);}
	}

	public event Delegate<int, HeroDeleteResponseBody> EventResHeroDelete
	{
		add    {AddDelegate<HeroDeleteResponseBody>(ClientCommandName.HeroDelete, value);}
		remove {DelDelegate<HeroDeleteResponseBody>(ClientCommandName.HeroDelete, value);}
	}

	public event Delegate<int, HeroLoginResponseBody> EventResHeroLogin
	{
		add    {AddDelegate<HeroLoginResponseBody>(ClientCommandName.HeroLogin, value);}
		remove {DelDelegate<HeroLoginResponseBody>(ClientCommandName.HeroLogin, value);}
	}

	public event Delegate<int, HeroInitEnterResponseBody> EventResHeroInitEnter
	{
		add { AddDelegate<HeroInitEnterResponseBody>(ClientCommandName.HeroInitEnter, value); }
		remove { DelDelegate<HeroInitEnterResponseBody>(ClientCommandName.HeroInitEnter, value); }
	}

	public event Delegate<int, HeroLogoutResponseBody> EventResHeroLogout
	{
		add { AddDelegate<HeroLogoutResponseBody>(ClientCommandName.HeroLogout, value); }
		remove { DelDelegate<HeroLogoutResponseBody>(ClientCommandName.HeroLogout, value); }
	}

	public event Delegate<int, ImmediateReviveResponseBody> EventResImmediateRevive
	{
		add { AddDelegate<ImmediateReviveResponseBody>(ClientCommandName.ImmediateRevive, value); }
		remove { DelDelegate<ImmediateReviveResponseBody>(ClientCommandName.ImmediateRevive, value); }
	}

	public event Delegate<int, ContinentSaftyReviveResponseBody> EventResContinentSaftyRevive
	{
		add { AddDelegate<ContinentSaftyReviveResponseBody>(ClientCommandName.ContinentSaftyRevive, value); }
		remove { DelDelegate<ContinentSaftyReviveResponseBody>(ClientCommandName.ContinentSaftyRevive, value); }
	}

	public event Delegate<int, ContinentEnterForSaftyRevivalResponseBody> EventResContinentEnterForSaftyRevival
	{
		add { AddDelegate<ContinentEnterForSaftyRevivalResponseBody>(ClientCommandName.ContinentEnterForSaftyRevival, value); }
		remove { DelDelegate<ContinentEnterForSaftyRevivalResponseBody>(ClientCommandName.ContinentEnterForSaftyRevival, value); }
	}

    //영웅위치
    public event Delegate<int, HeroPositionResponseBody> EventResHeroPosition
    {
        add { AddDelegate<HeroPositionResponseBody>(ClientCommandName.HeroPosition, value); }
        remove { DelDelegate<HeroPositionResponseBody>(ClientCommandName.HeroPosition, value); }
    }

    //영웅정보
    public event Delegate<int, HeroInfoResponseBody> EventResHeroInfo
    {
        add { AddDelegate<HeroInfoResponseBody>(ClientCommandName.HeroInfo, value); }
        remove { DelDelegate<HeroInfoResponseBody>(ClientCommandName.HeroInfo, value); }
    }

    #endregion Hero

    #region MainGear

    // 메인장비 장착
    public event Delegate<int, MainGearEquipResponseBody> EventResMainGearEquip
	{
		add { AddDelegate<MainGearEquipResponseBody>(ClientCommandName.MainGearEquip, value); }
		remove { DelDelegate<MainGearEquipResponseBody>(ClientCommandName.MainGearEquip, value); }
	}

	// 메인장비 장착해제
	public event Delegate<int, MainGearUnequipResponseBody> EventResMainGearUnequip
	{
		add { AddDelegate<MainGearUnequipResponseBody>(ClientCommandName.MainGearUnequip, value); }
		remove { DelDelegate<MainGearUnequipResponseBody>(ClientCommandName.MainGearUnequip, value); }
	}

	// 메인장비강화
	public event Delegate<int, MainGearEnchantResponseBody> EventResMainGearEnchant
	{
		add { AddDelegate<MainGearEnchantResponseBody>(ClientCommandName.MainGearEnchant, value); }
		remove { DelDelegate<MainGearEnchantResponseBody>(ClientCommandName.MainGearEnchant, value); }
	}

	// 메인장비전이 
	public event Delegate<int, MainGearTransitResponseBody> EventResMainGearTransit
	{
		add { AddDelegate<MainGearTransitResponseBody>(ClientCommandName.MainGearTransit, value); }
		remove { DelDelegate<MainGearTransitResponseBody>(ClientCommandName.MainGearTransit, value); }
	}

	// 메인장비세련
	public event Delegate<int, MainGearRefineResponseBody> EventResMainGearRefine
	{
		add { AddDelegate<MainGearRefineResponseBody>(ClientCommandName.MainGearRefine, value); }
		remove { DelDelegate<MainGearRefineResponseBody>(ClientCommandName.MainGearRefine, value); }
	}

	// 메인장비세련적용
	public event Delegate<int, MainGearRefinementApplyResponseBody> EventResMainGearRefinementApply
	{
		add { AddDelegate<MainGearRefinementApplyResponseBody>(ClientCommandName.MainGearRefinementApply, value); }
		remove { DelDelegate<MainGearRefinementApplyResponseBody>(ClientCommandName.MainGearRefinementApply, value); }
	}

	// 메인장비분해
	public event Delegate<int, MainGearDisassembleResponseBody> EventResMainGearDisassemble
	{
		add { AddDelegate<MainGearDisassembleResponseBody>(ClientCommandName.MainGearDisassemble, value); }
		remove { DelDelegate<MainGearDisassembleResponseBody>(ClientCommandName.MainGearDisassemble, value); }
	}

    // 메인장비강화레벨세트활성
    public event Delegate<int, MainGearEnchantLevelSetActivateResponseBody> EventResMainGearEnchantLevelSetActivate
    {
        add { AddDelegate<MainGearEnchantLevelSetActivateResponseBody>(ClientCommandName.MainGearEnchantLevelSetActivate, value); }
        remove { DelDelegate<MainGearEnchantLevelSetActivateResponseBody>(ClientCommandName.MainGearEnchantLevelSetActivate, value); }
    }

    #endregion MainGear

    #region SubGear

    // 보조장비 장착
    public event Delegate<int, SubGearEquipResponseBody> EventResSubGearEquip
	{
		add { AddDelegate<SubGearEquipResponseBody>(ClientCommandName.SubGearEquip, value); }
		remove { DelDelegate<SubGearEquipResponseBody>(ClientCommandName.SubGearEquip, value); }
	}

	// 보조장비 장착해제
	public event Delegate<int, SubGearUnequipResponseBody> EventResSubGearUnequip
	{
		add { AddDelegate<SubGearUnequipResponseBody>(ClientCommandName.SubGearUnequip, value); }
		remove { DelDelegate<SubGearUnequipResponseBody>(ClientCommandName.SubGearUnequip, value); }
	}

	// 소울스톤소켓장착
	public event Delegate<int, SoulstoneSocketMountResponseBody> EventResSoulstoneSocketMount
	{
		add { AddDelegate<SoulstoneSocketMountResponseBody>(ClientCommandName.SoulstoneSocketMount, value); }
		remove { DelDelegate<SoulstoneSocketMountResponseBody>(ClientCommandName.SoulstoneSocketMount, value); }
	}

	// 소울스톤소켓장착해제
	public event Delegate<int, SoulstoneSocketUnmountResponseBody> EventResSoulstoneSocketUnmount
	{
		add { AddDelegate<SoulstoneSocketUnmountResponseBody>(ClientCommandName.SoulstoneSocketUnmount, value); }
		remove { DelDelegate<SoulstoneSocketUnmountResponseBody>(ClientCommandName.SoulstoneSocketUnmount, value); }
	}

	// 룬소켓장착
	public event Delegate<int, RuneSocketMountResponseBody> EventResRuneSocketMount
	{
		add { AddDelegate<RuneSocketMountResponseBody>(ClientCommandName.RuneSocketMount, value); }
		remove { DelDelegate<RuneSocketMountResponseBody>(ClientCommandName.RuneSocketMount, value); }
	}

	// 룬소켓장착해제
	public event Delegate<int, RuneSocketUnmountResponseBody> EventResRuneSocketUnmount
	{
		add { AddDelegate<RuneSocketUnmountResponseBody>(ClientCommandName.RuneSocketUnmount, value); }
		remove { DelDelegate<RuneSocketUnmountResponseBody>(ClientCommandName.RuneSocketUnmount, value); }
	}

	// 보조장비레벨업
	public event Delegate<int, SubGearLevelUpResponseBody> EventResSubGearLevelUp
	{
		add { AddDelegate<SubGearLevelUpResponseBody>(ClientCommandName.SubGearLevelUp, value); }
		remove { DelDelegate<SubGearLevelUpResponseBody>(ClientCommandName.SubGearLevelUp, value); }
	}

	// 보조장비전체레벨업
	public event Delegate<int, SubGearLevelUpTotallyResponseBody> EventResSubGearLevelUpTotally
	{
		add { AddDelegate<SubGearLevelUpTotallyResponseBody>(ClientCommandName.SubGearLevelUpTotally, value); }
		remove { DelDelegate<SubGearLevelUpTotallyResponseBody>(ClientCommandName.SubGearLevelUpTotally, value); }
	}

	// 보조장비등급업
	public event Delegate<int, SubGearGradeUpResponseBody> EventResSubGearGradeUp
	{
		add { AddDelegate<SubGearGradeUpResponseBody>(ClientCommandName.SubGearGradeUp, value); }
		remove { DelDelegate<SubGearGradeUpResponseBody>(ClientCommandName.SubGearGradeUp, value); }
	}

	// 보조장비품질업
	public event Delegate<int, SubGearQualityUpResponseBody> EventResSubGearQualityUp
	{
		add { AddDelegate<SubGearQualityUpResponseBody>(ClientCommandName.SubGearQualityUp, value); }
		remove { DelDelegate<SubGearQualityUpResponseBody>(ClientCommandName.SubGearQualityUp, value); }
	}

	// 장착된소울스톤합성
	public event Delegate<int, MountedSoulstoneComposeResponseBody> EventResMountedSoulstoneCompose
	{
		add { AddDelegate<MountedSoulstoneComposeResponseBody>(ClientCommandName.MountedSoulstoneCompose, value); }
		remove { DelDelegate<MountedSoulstoneComposeResponseBody>(ClientCommandName.MountedSoulstoneCompose, value); }
	}

    // 보조장비소울스톤레벨세트활성
    public event Delegate<int, SubGearSoulstoneLevelSetActivateResponseBody> EventResSubGearSoulstoneLevelSetActivate
    {
        add { AddDelegate<SubGearSoulstoneLevelSetActivateResponseBody>(ClientCommandName.SubGearSoulstoneLevelSetActivate, value); }
        remove { DelDelegate<SubGearSoulstoneLevelSetActivateResponseBody>(ClientCommandName.SubGearSoulstoneLevelSetActivate, value); }
    }

    #endregion SubGear

    #region Mail

    // 메일받기
    public event Delegate<int, MailReceiveResponseBody> EventResMailReceive
	{
		add { AddDelegate<MailReceiveResponseBody>(ClientCommandName.MailReceive, value); }
		remove { DelDelegate<MailReceiveResponseBody>(ClientCommandName.MailReceive, value); }
	}

	// 메일전체받기
	public event Delegate<int, MailReceiveAllResponseBody> EventResMailReceiveAll
	{
		add { AddDelegate<MailReceiveAllResponseBody>(ClientCommandName.MailReceiveAll, value); }
		remove { DelDelegate<MailReceiveAllResponseBody>(ClientCommandName.MailReceiveAll, value); }
	}

    // 메일삭제
    public event Delegate<int, MailDeleteResposneBody> EventResMailDelete
    {
        add { AddDelegate<MailDeleteResposneBody>(ClientCommandName.MailDelete, value); }
        remove { DelDelegate<MailDeleteResposneBody>(ClientCommandName.MailDelete, value); }
    }

    // 메일전체삭제
    public event Delegate<int, MailDeleteAllResponseBody> EventResMailDeleteAll
    {
        add { AddDelegate<MailDeleteAllResponseBody>(ClientCommandName.MailDeleteAll, value); }
        remove { DelDelegate<MailDeleteAllResponseBody>(ClientCommandName.MailDeleteAll, value); }
    }


    #endregion Mail

    #region Skill

    // 스킬레벨업
    public event Delegate<int, SkillLevelUpResponseBody> EventResSkillLevelUp
	{
		add { AddDelegate<SkillLevelUpResponseBody>(ClientCommandName.SkillLevelUp, value); }
		remove { DelDelegate<SkillLevelUpResponseBody>(ClientCommandName.SkillLevelUp, value); }
	}

	// 스킬전체레벨업
	public event Delegate<int, SkillLevelUpTotallyResponseBody> EventResSkillLevelUpTotally
	{
		add { AddDelegate<SkillLevelUpTotallyResponseBody>(ClientCommandName.SkillLevelUpTotally, value); }
		remove { DelDelegate<SkillLevelUpTotallyResponseBody>(ClientCommandName.SkillLevelUpTotally, value); }
	}

	#endregion Skill

	#region Continent, Portal

	// 포탈입장
	public event Delegate<int, PortalEnterResponseBody> EventResPortalEnter
	{
		add { AddDelegate<PortalEnterResponseBody>(ClientCommandName.PortalEnter, value); }
		remove { DelDelegate<PortalEnterResponseBody>(ClientCommandName.PortalEnter, value); }
	}

	// 포탈퇴장
	public event Delegate<int, PortalExitResponseBody> EventResPortalExit
	{
		add { AddDelegate<PortalExitResponseBody>(ClientCommandName.PortalExit, value); }
		remove { DelDelegate<PortalExitResponseBody>(ClientCommandName.PortalExit, value); }
	}

	// 이전대륙입장
	public event Delegate<int, PrevContinentEnterResponseBody> EventResPrevContinentEnter
	{
		add { AddDelegate<PrevContinentEnterResponseBody>(ClientCommandName.PrevContinentEnter, value); }
		remove { DelDelegate<PrevContinentEnterResponseBody>(ClientCommandName.PrevContinentEnter, value); }
	}

    // 대륙전송
    public event Delegate<int, ContinentTransmissionResponseBody> EventResContinentTransmission
    {
        add { AddDelegate<ContinentTransmissionResponseBody>(ClientCommandName.ContinentTransmission, value); }
        remove { DelDelegate<ContinentTransmissionResponseBody>(ClientCommandName.ContinentTransmission, value); }
    }

    // 대륙전송을위한대륙입장
    public event Delegate<int, ContinentEnterForContinentTransmissionResponseBody> EventResContinentEnterForContinentTransmission
    {
        add { AddDelegate<ContinentEnterForContinentTransmissionResponseBody>(ClientCommandName.ContinentEnterForContinentTransmission, value); }
        remove { DelDelegate<ContinentEnterForContinentTransmissionResponseBody>(ClientCommandName.ContinentEnterForContinentTransmission, value); }
    }

    // 대륙안전지역입장
    public event Delegate<int, ContinentSaftyAreaEnterResponseBody> EventResContinentSaftyAreaEnter
    {
        add { AddDelegate<ContinentSaftyAreaEnterResponseBody>(ClientCommandName.ContinentSaftyAreaEnter, value); }
        remove { DelDelegate<ContinentSaftyAreaEnterResponseBody>(ClientCommandName.ContinentSaftyAreaEnter, value); }
    }

    #endregion Continent, Portal

    #region Quest
    public event Delegate<int, MainQuestAcceptResponseBody> EventResMainQuestAccept
	{
		add { AddDelegate<MainQuestAcceptResponseBody>(ClientCommandName.MainQuestAccept, value); }
		remove { DelDelegate<MainQuestAcceptResponseBody>(ClientCommandName.MainQuestAccept, value); }
	}

	public event Delegate<int, MainQuestCompleteResponseBody> EventResMainQuestComplete
	{
		add { AddDelegate<MainQuestCompleteResponseBody>(ClientCommandName.MainQuestComplete, value); }
		remove { DelDelegate<MainQuestCompleteResponseBody>(ClientCommandName.MainQuestComplete, value); }
	}

	public event Delegate<int, ContinentObjectInteractionStartResponseBody> EventResContinentObjectInteractionStart
	{
		add { AddDelegate<ContinentObjectInteractionStartResponseBody>(ClientCommandName.ContinentObjectInteractionStart, value); }
		remove { DelDelegate<ContinentObjectInteractionStartResponseBody>(ClientCommandName.ContinentObjectInteractionStart, value); }
	}
	#endregion Quest

	#region SimpleShop

	// 간이상점구입
	public event Delegate<int, SimpleShopBuyResponseBody> EventResSimpleShopBuy
	{
		add { AddDelegate<SimpleShopBuyResponseBody>(ClientCommandName.SimpleShopBuy, value); }
		remove { DelDelegate<SimpleShopBuyResponseBody>(ClientCommandName.SimpleShopBuy, value); }
	}

	// 간이상점판매
	public event Delegate<int, SimpleShopSellResponseBody> EventResSimpleShopSell
	{
		add { AddDelegate<SimpleShopSellResponseBody>(ClientCommandName.SimpleShopSell, value); }
		remove { DelDelegate<SimpleShopSellResponseBody>(ClientCommandName.SimpleShopSell, value); }
	}

	#endregion SimpleShop

	#region Inventory

	// 인벤토리슬롯확장
	public event Delegate<int, InventorySlotExtendResponseBody> EventResInventorySlotExtend
	{
		add { AddDelegate<InventorySlotExtendResponseBody>(ClientCommandName.InventorySlotExtend, value); }
		remove { DelDelegate<InventorySlotExtendResponseBody>(ClientCommandName.InventorySlotExtend, value); }
	}

	#endregion Inventory

	#region Item

	// 아이템합성
	public event Delegate<int, ItemComposeResponseBody> EventResItemCompose
	{
		add { AddDelegate<ItemComposeResponseBody>(ClientCommandName.ItemCompose, value); }
		remove { DelDelegate<ItemComposeResponseBody>(ClientCommandName.ItemCompose, value); }
	}

	// 아이템전체합성
	public event Delegate<int, ItemComposeTotallyResponseBody> EventResItemComposeTotally
	{
		add { AddDelegate<ItemComposeTotallyResponseBody>(ClientCommandName.ItemComposeTotally, value); }
		remove { DelDelegate<ItemComposeTotallyResponseBody>(ClientCommandName.ItemComposeTotally, value); }
	}

	// 물약사용
	public event Delegate<int, HpPotionUseResponseBody> EventResHpPotionUse
	{
		add { AddDelegate<HpPotionUseResponseBody>(ClientCommandName.HpPotionUse, value); }
		remove { DelDelegate<HpPotionUseResponseBody>(ClientCommandName.HpPotionUse, value); }
	}

	// 귀환주문서사용
	public event Delegate<int, ReturnScrollUseResponseBody> EventResReturnScrollUse
	{
		add { AddDelegate<ReturnScrollUseResponseBody>(ClientCommandName.ReturnScrollUse, value); }
		remove { DelDelegate<ReturnScrollUseResponseBody>(ClientCommandName.ReturnScrollUse, value); }
	}

	// 귀환주문서사용에의한대륙입장
	public event Delegate<int, ContinentEnterForReturnScrollUseResponseBody> EventResContinentEnterForReturnScrollUse
	{
		add { AddDelegate<ContinentEnterForReturnScrollUseResponseBody>(ClientCommandName.ContinentEnterForReturnScrollUse, value); }
		remove { DelDelegate<ContinentEnterForReturnScrollUseResponseBody>(ClientCommandName.ContinentEnterForReturnScrollUse, value); }
	}

    // 뽑기상자사용
    public event Delegate<int, PickBoxUseResponseBody> EventResPickBoxUse
    {
        add { AddDelegate<PickBoxUseResponseBody>(ClientCommandName.PickBoxUse, value); }
        remove { DelDelegate<PickBoxUseResponseBody>(ClientCommandName.PickBoxUse, value); }
    }

    // 메인장비상자사용
    public event Delegate<int, MainGearBoxUseResponseBody> EventResMainGearBoxUse
    {
        add { AddDelegate<MainGearBoxUseResponseBody>(ClientCommandName.MainGearBoxUse, value); }
        remove { DelDelegate<MainGearBoxUseResponseBody>(ClientCommandName.MainGearBoxUse, value); }
    }

    // 경험치물약사용
    public event Delegate<int, ExpPotionUseResponseBody> EventResExpPotionUse
    {
        add { AddDelegate<ExpPotionUseResponseBody>(ClientCommandName.ExpPotionUse, value); }
        remove { DelDelegate<ExpPotionUseResponseBody>(ClientCommandName.ExpPotionUse, value); }
    }

    // 경험치주문서사용
    public event Delegate<int, ExpScrollUseResponseBody> EventResExpScrollUse
    {
        add { AddDelegate<ExpScrollUseResponseBody>(ClientCommandName.ExpScrollUse, value); }
        remove { DelDelegate<ExpScrollUseResponseBody>(ClientCommandName.ExpScrollUse, value); }
    }

    // 체력구매
    public event Delegate<int, StaminaBuyResponseBody> EventResStaminaBuy
    {
        add { AddDelegate<StaminaBuyResponseBody>(ClientCommandName.StaminaBuy, value); }
        remove { DelDelegate<StaminaBuyResponseBody>(ClientCommandName.StaminaBuy, value); }
    }

    // 골드아이템사용
    public event Delegate<int, GoldItemUseResponseBody> EventResGoldItemUse
    {
        add { AddDelegate<GoldItemUseResponseBody>(ClientCommandName.GoldItemUse, value); }
        remove { DelDelegate<GoldItemUseResponseBody>(ClientCommandName.GoldItemUse, value); }
    }

    // 귀속다이아아이템사용
    public event Delegate<int, OwnDiaItemUseResponseBody> EventResOwnDiaItemUse
    {
        add { AddDelegate<OwnDiaItemUseResponseBody>(ClientCommandName.OwnDiaItemUse, value); }
        remove { DelDelegate<OwnDiaItemUseResponseBody>(ClientCommandName.OwnDiaItemUse, value); }
    }

    // 명예포인트아이템사용
    public event Delegate<int, HonorPointItemUseResponseBody> EventResHonorPointItemUse
    {
        add { AddDelegate<HonorPointItemUseResponseBody>(ClientCommandName.HonorPointItemUse, value); }
        remove { DelDelegate<HonorPointItemUseResponseBody>(ClientCommandName.HonorPointItemUse, value); }
    }

    // 공적포인트아이템사용
    public event Delegate<int, ExploitPointItemUseResponseBody> EventResExploitPointItemUse
    {
        add { AddDelegate<ExploitPointItemUseResponseBody>(ClientCommandName.ExploitPointItemUse, value); }
        remove { DelDelegate<ExploitPointItemUseResponseBody>(ClientCommandName.ExploitPointItemUse, value); }
    }

    // 별의정수아이템사용
    public event Delegate<int, StarEssenseItemUseResponseBody> EventResStarEssenseItemUse
    {
        add { AddDelegate<StarEssenseItemUseResponseBody>(ClientCommandName.StarEssenseItemUse, value); }
        remove { DelDelegate<StarEssenseItemUseResponseBody>(ClientCommandName.StarEssenseItemUse, value); }
    }

    // 고급별의정수아이템사용
    public event Delegate<int, PremiumStarEssenseItemUseResponseBody> EventResPremiumStarEssenseItemUse
    {
        add { AddDelegate<PremiumStarEssenseItemUseResponseBody>(ClientCommandName.PremiumStarEssenseItemUse, value); }
        remove { DelDelegate<PremiumStarEssenseItemUseResponseBody>(ClientCommandName.PremiumStarEssenseItemUse, value); }
    }

    // 영혼석아이템사용
  

    #endregion Item

    #region Distortion

    // 왜곡주문서사용
    public event Delegate<int, DistortionScrollUseResponseBody> EventResDistortionScrollUse
    {
        add { AddDelegate<DistortionScrollUseResponseBody>(ClientCommandName.DistortionScrollUse, value); }
        remove { DelDelegate<DistortionScrollUseResponseBody>(ClientCommandName.DistortionScrollUse, value); }
    }

    // 왜곡취소
    public event Delegate<int, DistortionCancelResponseBody> EventResDistortionCancel
    {
        add { AddDelegate<DistortionCancelResponseBody>(ClientCommandName.DistortionCancel, value); }
        remove { DelDelegate<DistortionCancelResponseBody>(ClientCommandName.DistortionCancel, value); }
    }

    #endregion Distortion

    #region Rest

    // 무료휴식보상받기
    public event Delegate<int, RestRewardReceiveFreeResponseBody> EventResRestRewardReceiveFree
	{
		add { AddDelegate<RestRewardReceiveFreeResponseBody>(ClientCommandName.RestRewardReceiveFree, value); }
		remove { DelDelegate<RestRewardReceiveFreeResponseBody>(ClientCommandName.RestRewardReceiveFree, value); }
	}

	// 골드휴식보상받기
	public event Delegate<int, RestRewardReceiveGoldResponseBody> EventResRestRewardReceiveGold
	{
		add { AddDelegate<RestRewardReceiveGoldResponseBody>(ClientCommandName.RestRewardReceiveGold, value); }
		remove { DelDelegate<RestRewardReceiveGoldResponseBody>(ClientCommandName.RestRewardReceiveGold, value); }
	}

	// 다이아휴식보상받기
	public event Delegate<int, RestRewardReceiveDiaResponseBody> EventResRestRewardReceiveDia
	{
		add { AddDelegate<RestRewardReceiveDiaResponseBody>(ClientCommandName.RestRewardReceiveDia, value); }
		remove { DelDelegate<RestRewardReceiveDiaResponseBody>(ClientCommandName.RestRewardReceiveDia, value); }
	}

    #endregion Rest

    #region Chatting

    // 채팅메시지발송
    public event Delegate<int, ChattingMessageSendResponseBody> EventResChattingMessageSend
    {
        add { AddDelegate<ChattingMessageSendResponseBody>(ClientCommandName.ChattingMessageSend, value); }
        remove { DelDelegate<ChattingMessageSendResponseBody>(ClientCommandName.ChattingMessageSend, value); }
    }

    #endregion Chatting

    #region Party

    // 주변영웅목록
    public event Delegate<int, PartySurroundingHeroListResponseBody> EventResPartySurroundingHeroList
    {
        add { AddDelegate<PartySurroundingHeroListResponseBody>(ClientCommandName.PartySurroundingHeroList, value); }
        remove { DelDelegate<PartySurroundingHeroListResponseBody>(ClientCommandName.PartySurroundingHeroList, value); }
    }

    // 주변파티목록
    public event Delegate<int, PartySurroundingPartyListResponseBody> EventResPartySurroundingPartyList
    {
        add { AddDelegate<PartySurroundingPartyListResponseBody>(ClientCommandName.PartySurroundingPartyList, value); }
        remove { DelDelegate<PartySurroundingPartyListResponseBody>(ClientCommandName.PartySurroundingPartyList, value); }
    }

    // 파티생성
    public event Delegate<int, PartyCreateResponseBody> EventResPartyCreate
    {
        add { AddDelegate<PartyCreateResponseBody>(ClientCommandName.PartyCreate, value); }
        remove { DelDelegate<PartyCreateResponseBody>(ClientCommandName.PartyCreate, value); }
    }

    // 파티탈퇴
    public event Delegate<int, PartyExitResponseBody> EventResPartyExit
    {
        add { AddDelegate<PartyExitResponseBody>(ClientCommandName.PartyExit, value); }
        remove { DelDelegate<PartyExitResponseBody>(ClientCommandName.PartyExit, value); }
    }

    // 파티멤버강퇴
    public event Delegate<int, PartyMemberBanishResponseBody> EventResPartyMemberBanish
    {
        add { AddDelegate<PartyMemberBanishResponseBody>(ClientCommandName.PartyMemberBanish, value); }
        remove { DelDelegate<PartyMemberBanishResponseBody>(ClientCommandName.PartyMemberBanish, value); }
    }

    // 파티소집
    public event Delegate<int, PartyCallResponseBody> EventResPartyCall
    {
        add { AddDelegate<PartyCallResponseBody>(ClientCommandName.PartyCall, value); }
        remove { DelDelegate<PartyCallResponseBody>(ClientCommandName.PartyCall, value); }
    }

    // 파티해산
    public event Delegate<int, PartyDisbandResponseBody> EventResPartyDisband
    {
        add { AddDelegate<PartyDisbandResponseBody>(ClientCommandName.PartyDisband, value); }
        remove { DelDelegate<PartyDisbandResponseBody>(ClientCommandName.PartyDisband, value); }
    }

    // 파티신청
    public event Delegate<int, PartyApplyResponseBody> EventResPartyApply
    {
        add { AddDelegate<PartyApplyResponseBody>(ClientCommandName.PartyApply, value); }
        remove { DelDelegate<PartyApplyResponseBody>(ClientCommandName.PartyApply, value); }
    }

    // 파티신청수락
    public event Delegate<int, PartyApplicationAcceptResponseBody> EventResPartyApplicationAccept
    {
        add { AddDelegate<PartyApplicationAcceptResponseBody>(ClientCommandName.PartyApplicationAccept, value); }
        remove { DelDelegate<PartyApplicationAcceptResponseBody>(ClientCommandName.PartyApplicationAccept, value); }
    }

    // 파티신청거절
    public event Delegate<int, PartyApplicationRefuseResponseBody> EventResPartyApplicationRefuse
    {
        add { AddDelegate<PartyApplicationRefuseResponseBody>(ClientCommandName.PartyApplicationRefuse, value); }
        remove { DelDelegate<PartyApplicationRefuseResponseBody>(ClientCommandName.PartyApplicationRefuse, value); }
    }

    // 파티초대
    public event Delegate<int, PartyInviteResponseBody> EventResPartyInvite
    {
        add { AddDelegate<PartyInviteResponseBody>(ClientCommandName.PartyInvite, value); }
        remove { DelDelegate<PartyInviteResponseBody>(ClientCommandName.PartyInvite, value); }
    }

    // 파티초대수락
    public event Delegate<int, PartyInvitationAcceptResponseBody> EventResPartyInvitationAccept
    {
        add { AddDelegate<PartyInvitationAcceptResponseBody>(ClientCommandName.PartyInvitationAccept, value); }
        remove { DelDelegate<PartyInvitationAcceptResponseBody>(ClientCommandName.PartyInvitationAccept, value); }
    }

    // 파티초대거절
    public event Delegate<int, PartyInvitationRefuseResponseBody> EventResPartyInvitationRefuse
    {
        add { AddDelegate<PartyInvitationRefuseResponseBody>(ClientCommandName.PartyInvitationRefuse, value); }
        remove { DelDelegate<PartyInvitationRefuseResponseBody>(ClientCommandName.PartyInvitationRefuse, value); }
    }

    // 파티장변경
    public event Delegate<int, PartyMasterChangeResponseBody> EventResPartyMasterChange
    {
        add { AddDelegate<PartyMasterChangeResponseBody>(ClientCommandName.PartyMasterChange, value); }
        remove { DelDelegate<PartyMasterChangeResponseBody>(ClientCommandName.PartyMasterChange, value); }
    }

    #endregion Party

    #region MainQuestDungeon

    // 메인퀘스트던전입장을위한대륙퇴장
    public event Delegate<int, ContinentExitForMainQuestDungeonEnterResponseBody> EventResContinentExitForMainQuestDungeonEnter
    {
        add { AddDelegate<ContinentExitForMainQuestDungeonEnterResponseBody>(ClientCommandName.ContinentExitForMainQuestDungeonEnter, value); }
        remove { DelDelegate<ContinentExitForMainQuestDungeonEnterResponseBody>(ClientCommandName.ContinentExitForMainQuestDungeonEnter, value); }
    }

    // 메인퀘스트던전입장
    public event Delegate<int, MainQuestDungeonEnterResponseBody> EventResMainQuestDungeonEnter
    {
        add { AddDelegate<MainQuestDungeonEnterResponseBody>(ClientCommandName.MainQuestDungeonEnter, value); }
        remove { DelDelegate<MainQuestDungeonEnterResponseBody>(ClientCommandName.MainQuestDungeonEnter, value); }
    }

    // 메인퀘스트던전포기
    public event Delegate<int, MainQuestDungeonAbandonResponseBody> EventResMainQuestDungeonAbandon
    {
        add { AddDelegate<MainQuestDungeonAbandonResponseBody>(ClientCommandName.MainQuestDungeonAbandon, value); }
        remove { DelDelegate<MainQuestDungeonAbandonResponseBody>(ClientCommandName.MainQuestDungeonAbandon, value); }
    }

    // 메인퀘스트던전퇴장
    public event Delegate<int, MainQuestDungeonExitResponseBody> EventResMainQuestDungeonExit
    {
        add { AddDelegate<MainQuestDungeonExitResponseBody>(ClientCommandName.MainQuestDungeonExit, value); }
        remove { DelDelegate<MainQuestDungeonExitResponseBody>(ClientCommandName.MainQuestDungeonExit, value); }
    }

    // 메인퀘스트던전안전부활
    public event Delegate<int, MainQuestDungeonSaftyReviveResponseBody> EventResMainQuestDungeonSaftyRevive
    {
        add { AddDelegate<MainQuestDungeonSaftyReviveResponseBody>(ClientCommandName.MainQuestDungeonSaftyRevive, value); }
        remove { DelDelegate<MainQuestDungeonSaftyReviveResponseBody>(ClientCommandName.MainQuestDungeonSaftyRevive, value); }
    }

    #endregion MainQuestDungeon

    #region Reward

    // 레벨업보상받기
    public event Delegate<int, LevelUpRewardReceiveResponseBody> EventResLevelUpRewardReceive
    {
        add { AddDelegate<LevelUpRewardReceiveResponseBody>(ClientCommandName.LevelUpRewardReceive, value); }
        remove { DelDelegate<LevelUpRewardReceiveResponseBody>(ClientCommandName.LevelUpRewardReceive, value); }
    }

    // 일일접속시간보상받기
    public event Delegate<int, DailyAccessTimeRewardReceiveResponseBody> EventResDailyAccessTimeRewardReceive
    {
        add { AddDelegate<DailyAccessTimeRewardReceiveResponseBody>(ClientCommandName.DailyAccessTimeRewardReceive, value); }
        remove { DelDelegate<DailyAccessTimeRewardReceiveResponseBody>(ClientCommandName.DailyAccessTimeRewardReceive, value); }
    }

    // 출석보상받기
    public event Delegate<int, AttendRewardReceiveResponseBody> EventResAttendRewardReceive
    {
        add { AddDelegate<AttendRewardReceiveResponseBody>(ClientCommandName.AttendRewardReceive, value); }
        remove { DelDelegate<AttendRewardReceiveResponseBody>(ClientCommandName.AttendRewardReceive, value); }
    }

    // 연속미션보상받기
    public event Delegate<int, SeriesMissionRewardReceiveResponseBody> EventResSeriesMissionRewardReceive
    {
        add { AddDelegate<SeriesMissionRewardReceiveResponseBody>(ClientCommandName.SeriesMissionRewardReceive, value); }
        remove { DelDelegate<SeriesMissionRewardReceiveResponseBody>(ClientCommandName.SeriesMissionRewardReceive, value); }
    }

    // 오늘의미션보상받기
    public event Delegate<int, TodayMissionRewardReceiveResponseBody> EventResTodayMissionRewardReceive
    {
        add { AddDelegate<TodayMissionRewardReceiveResponseBody>(ClientCommandName.TodayMissionRewardReceive, value); }
        remove { DelDelegate<TodayMissionRewardReceiveResponseBody>(ClientCommandName.TodayMissionRewardReceive, value); }
    }

    // 신병선물받기
    public event Delegate<int, RookieGiftReceiveResponseBody> EventResRookieGiftReceive
    {
        add { AddDelegate<RookieGiftReceiveResponseBody>(ClientCommandName.RookieGiftReceive, value); }
        remove { DelDelegate<RookieGiftReceiveResponseBody>(ClientCommandName.RookieGiftReceive, value); }
    }

    // 오픈선물받기
    public event Delegate<int, OpenGiftReceiveResponseBody> EventResOpenGiftReceive
    {
        add { AddDelegate<OpenGiftReceiveResponseBody>(ClientCommandName.OpenGiftReceive, value); }
        remove { DelDelegate<OpenGiftReceiveResponseBody>(ClientCommandName.OpenGiftReceive, value); }
    }

    // 한정선물보상받기
    public event Delegate<int, LimitationGiftRewardReceiveResponseBody> EventResLimitationGiftRewardReceive
    {
        add { AddDelegate<LimitationGiftRewardReceiveResponseBody>(ClientCommandName.LimitationGiftRewardReceive, value); }
        remove { DelDelegate<LimitationGiftRewardReceiveResponseBody>(ClientCommandName.LimitationGiftRewardReceive, value); }
    }

    // 주말보상선택
    public event Delegate<int, WeekendRewardSelectResponseBody> EventResWeekendRewardSelect
    {
        add { AddDelegate<WeekendRewardSelectResponseBody>(ClientCommandName.WeekendRewardSelect, value); }
        remove { DelDelegate<WeekendRewardSelectResponseBody>(ClientCommandName.WeekendRewardSelect, value); }
    }

    // 주말보상받기
    public event Delegate<int, WeekendRewardReceiveResponseBody> EventResWeekendRewardReceive
    {
        add { AddDelegate<WeekendRewardReceiveResponseBody>(ClientCommandName.WeekendRewardReceive, value); }
        remove { DelDelegate<WeekendRewardReceiveResponseBody>(ClientCommandName.WeekendRewardReceive, value); }
    }

    #endregion Reward

    #region Mount

    // 탈것장착
    public event Delegate<int, MountEquipResponseBody> EventResMountEquip
    {
        add { AddDelegate<MountEquipResponseBody>(ClientCommandName.MountEquip, value); }
        remove { DelDelegate<MountEquipResponseBody>(ClientCommandName.MountEquip, value); }
    }

    // 탈것레벨업
    public event Delegate<int, MountLevelUpResponseBody> EventResMountLevelUp
    {
        add { AddDelegate<MountLevelUpResponseBody>(ClientCommandName.MountLevelUp, value); }
        remove { DelDelegate<MountLevelUpResponseBody>(ClientCommandName.MountLevelUp, value); }
    }

    // 탈것장비장착
    public event Delegate<int, MountGearEquipResponseBody> EventResMountGearEquip
    {
        add { AddDelegate<MountGearEquipResponseBody>(ClientCommandName.MountGearEquip, value); }
        remove { DelDelegate<MountGearEquipResponseBody>(ClientCommandName.MountGearEquip, value); }
    }

    // 탈것장비장착해제
    public event Delegate<int, MountGearUnequipResponseBody> EventResMountGearUnequip
    {
        add { AddDelegate<MountGearUnequipResponseBody>(ClientCommandName.MountGearUnequip, value); }
        remove { DelDelegate<MountGearUnequipResponseBody>(ClientCommandName.MountGearUnequip, value); }
    }

    // 탈것장비재강화
    public event Delegate<int, MountGearRefineResponseBody> EventResMountGearRefine
    {
        add { AddDelegate<MountGearRefineResponseBody>(ClientCommandName.MountGearRefine, value); }
        remove { DelDelegate<MountGearRefineResponseBody>(ClientCommandName.MountGearRefine, value); }
    }

    // 탈것장비뽑기상자제작
    public event Delegate<int, MountGearPickBoxMakeResponseBody> EventResMountGearPickBoxMake
    {
        add { AddDelegate<MountGearPickBoxMakeResponseBody>(ClientCommandName.MountGearPickBoxMake, value); }
        remove { DelDelegate<MountGearPickBoxMakeResponseBody>(ClientCommandName.MountGearPickBoxMake, value); }
    }

    // 탈것장비뽑기상자모두제작
    public event Delegate<int, MountGearPickBoxMakeTotallyResponseBody> EventResMountGearPickBoxMakeTotally
    {
        add { AddDelegate<MountGearPickBoxMakeTotallyResponseBody>(ClientCommandName.MountGearPickBoxMakeTotally, value); }
        remove { DelDelegate<MountGearPickBoxMakeTotallyResponseBody>(ClientCommandName.MountGearPickBoxMakeTotally, value); }
    }

    // 탈것타기
    public event Delegate<int, MountGetOnResponseBody> EventResMountGetOn
    {
        add { AddDelegate<MountGetOnResponseBody>(ClientCommandName.MountGetOn, value); }
        remove { DelDelegate<MountGetOnResponseBody>(ClientCommandName.MountGetOn, value); }
    }

    // 탈것각성레벨업
    public event Delegate<int, MountAwakeningLevelUpResponseBody> EventResMountAwakeningLevelUp
    {
        add { AddDelegate<MountAwakeningLevelUpResponseBody>(ClientCommandName.MountAwakeningLevelUp, value); }
        remove { DelDelegate<MountAwakeningLevelUpResponseBody>(ClientCommandName.MountAwakeningLevelUp, value); }
    }

    // 탈것속성물약사용
    public event Delegate<int, MountAttrPotionUseResponseBody> EventResMountAttrPotionUse
    {
        add { AddDelegate<MountAttrPotionUseResponseBody>(ClientCommandName.MountAttrPotionUse, value); }
        remove { DelDelegate<MountAttrPotionUseResponseBody>(ClientCommandName.MountAttrPotionUse, value); }
    }

    // 탈것아이템사용
    public event Delegate<int, MountItemUseResponseBody> EventResMountItemUse
    {
        add { AddDelegate<MountItemUseResponseBody>(ClientCommandName.MountItemUse, value); }
        remove { DelDelegate<MountItemUseResponseBody>(ClientCommandName.MountItemUse, value); }
    }

    #endregion Mount

    #region Wing

    // 날개장착
    public event Delegate<int, WingEquipResponseBody> EventResWingEquip
    {
        add { AddDelegate<WingEquipResponseBody>(ClientCommandName.WingEquip, value); }
        remove { DelDelegate<WingEquipResponseBody>(ClientCommandName.WingEquip, value); }
    }

    // 날개강화
    public event Delegate<int, WingEnchantResponseBody> EventResWingEnchant
    {
        add { AddDelegate<WingEnchantResponseBody>(ClientCommandName.WingEnchant, value); }
        remove { DelDelegate<WingEnchantResponseBody>(ClientCommandName.WingEnchant, value); }
    }

    // 날개전체강화
    public event Delegate<int, WingEnchantTotallyResponseBody> EventResWingEnchantTotally
    {
        add { AddDelegate<WingEnchantTotallyResponseBody>(ClientCommandName.WingEnchantTotally, value); }
        remove { DelDelegate<WingEnchantTotallyResponseBody>(ClientCommandName.WingEnchantTotally, value); }
    }

    // 날개기억조각장착
    public event Delegate<int, WingMemoryPieceInstallResponseBody> EventResWingMemoryPieceInstall
    {
        add { AddDelegate<WingMemoryPieceInstallResponseBody>(ClientCommandName.WingMemoryPieceInstall, value); }
        remove { DelDelegate<WingMemoryPieceInstallResponseBody>(ClientCommandName.WingMemoryPieceInstall, value); }
    }

    // 날개아이템사용
    public event Delegate<int, WingItemUseResponseBody> EventResWingItemUse
    {
        add { AddDelegate<WingItemUseResponseBody>(ClientCommandName.WingItemUse, value); }
        remove { DelDelegate<WingItemUseResponseBody>(ClientCommandName.WingItemUse, value); }
    }

    #endregion Wing

    #region StoryDungeon

    // 스토리던전입장을위한대륙퇴장
    public event Delegate<int, ContinentExitForStoryDungeonEnterResponseBody> EventResContinentExitForStoryDungeonEnter
    {
        add { AddDelegate<ContinentExitForStoryDungeonEnterResponseBody>(ClientCommandName.ContinentExitForStoryDungeonEnter, value); }
        remove { DelDelegate<ContinentExitForStoryDungeonEnterResponseBody>(ClientCommandName.ContinentExitForStoryDungeonEnter, value); }
    }

    // 스토리던전입장
    public event Delegate<int, StoryDungeonEnterResponseBody> EventResStoryDungeonEnter
    { 
        add { AddDelegate<StoryDungeonEnterResponseBody>(ClientCommandName.StoryDungeonEnter, value); }
        remove { DelDelegate<StoryDungeonEnterResponseBody>(ClientCommandName.StoryDungeonEnter, value); }
    }

    // 스토리던전포기
    public event Delegate<int, StoryDungeonAbandonResponseBody> EventResStoryDungeonAbandon
    {
        add { AddDelegate<StoryDungeonAbandonResponseBody>(ClientCommandName.StoryDungeonAbandon, value); }
        remove { DelDelegate<StoryDungeonAbandonResponseBody>(ClientCommandName.StoryDungeonAbandon, value); }
    }

    // 스토리던전퇴장
    public event Delegate<int, StoryDungeonExitResponseBody> EventResStoryDungeonExit
    {
        add { AddDelegate<StoryDungeonExitResponseBody>(ClientCommandName.StoryDungeonExit, value); }
        remove { DelDelegate<StoryDungeonExitResponseBody>(ClientCommandName.StoryDungeonExit, value); }
    }

    // 스토리던전부활
    public event Delegate<int, StoryDungeonReviveResponseBody> EventResStoryDungeonRevive
    {
        add { AddDelegate<StoryDungeonReviveResponseBody>(ClientCommandName.StoryDungeonRevive, value); }
        remove { DelDelegate<StoryDungeonReviveResponseBody>(ClientCommandName.StoryDungeonRevive, value); }
    }

    // 스토리던전소탕
    public event Delegate<int, StoryDungeonSweepResponseBody> EventResStoryDungeonSweep
    {
        add { AddDelegate<StoryDungeonSweepResponseBody>(ClientCommandName.StoryDungeonSweep, value); }
        remove { DelDelegate<StoryDungeonSweepResponseBody>(ClientCommandName.StoryDungeonSweep, value); }
    }

	// 스토리던전몬스터테이밍
	public event Delegate<int, StoryDungeonMonsterTameResponseBody> EventResStoryDungeonMonsterTame
	{
		add { AddDelegate<StoryDungeonMonsterTameResponseBody>(ClientCommandName.StoryDungeonMonsterTame, value); }
		remove { DelDelegate<StoryDungeonMonsterTameResponseBody>(ClientCommandName.StoryDungeonMonsterTame, value); }
	}

    #endregion StoryDungeon

    #region ExpDungeon

    // 경험치던전입장을위한대륙퇴장
    public event Delegate<int, ContinentExitForExpDungeonEnterResponseBody> EventResContinentExitForExpDungeonEnter
    {
        add { AddDelegate<ContinentExitForExpDungeonEnterResponseBody>(ClientCommandName.ContinentExitForExpDungeonEnter, value); }
        remove { DelDelegate<ContinentExitForExpDungeonEnterResponseBody>(ClientCommandName.ContinentExitForExpDungeonEnter, value); }
    }

    // 경험치던전입장
    public event Delegate<int, ExpDungeonEnterResponseBody> EventResExpDungeonEnter
    {
        add { AddDelegate<ExpDungeonEnterResponseBody>(ClientCommandName.ExpDungeonEnter, value); }
        remove { DelDelegate<ExpDungeonEnterResponseBody>(ClientCommandName.ExpDungeonEnter, value); }
    }

    // 경험치던전포기
    public event Delegate<int, ExpDungeonAbandonResponseBody> EventResExpDungeonAbandon
    {
        add { AddDelegate<ExpDungeonAbandonResponseBody>(ClientCommandName.ExpDungeonAbandon, value); }
        remove { DelDelegate<ExpDungeonAbandonResponseBody>(ClientCommandName.ExpDungeonAbandon, value); }
    }

    // 경험치던전퇴장
    public event Delegate<int, ExpDungeonExitResponseBody> EventResExpDungeonExit
    {
        add { AddDelegate<ExpDungeonExitResponseBody>(ClientCommandName.ExpDungeonExit, value); }
        remove { DelDelegate<ExpDungeonExitResponseBody>(ClientCommandName.ExpDungeonExit, value); }
    }

    // 경험치던전부활
    public event Delegate<int, ExpDungeonReviveResponseBody> EventResExpDungeonRevive
    {
        add { AddDelegate<ExpDungeonReviveResponseBody>(ClientCommandName.ExpDungeonRevive, value); }
        remove { DelDelegate<ExpDungeonReviveResponseBody>(ClientCommandName.ExpDungeonRevive, value); }
    }

    // 경험치던전소탕
    public event Delegate<int, ExpDungeonSweepResponseBody> EventResExpDungeonSweep
    {
        add { AddDelegate<ExpDungeonSweepResponseBody>(ClientCommandName.ExpDungeonSweep, value); }
        remove { DelDelegate<ExpDungeonSweepResponseBody>(ClientCommandName.ExpDungeonSweep, value); }
    }

    #endregion ExpDungeon

    #region GoldDungeon

    // 골드던전입장을위한대륙퇴장
    public event Delegate<int, ContinentExitForGoldDungeonEnterResponseBody> EventResContinentExitForGoldDungeonEnter
    {
        add { AddDelegate<ContinentExitForGoldDungeonEnterResponseBody>(ClientCommandName.ContinentExitForGoldDungeonEnter, value); }
        remove { DelDelegate<ContinentExitForGoldDungeonEnterResponseBody>(ClientCommandName.ContinentExitForGoldDungeonEnter, value); }
    }

    // 골드던전입장골드던전입장
    public event Delegate<int, GoldDungeonEnterResponseBody> EventResGoldDungeonEnter
    {
        add { AddDelegate<GoldDungeonEnterResponseBody>(ClientCommandName.GoldDungeonEnter, value); }
        remove { DelDelegate<GoldDungeonEnterResponseBody>(ClientCommandName.GoldDungeonEnter, value); }
    }

    // 골드던전포기
    public event Delegate<int, GoldDungeonAbandonResponseBody> EventResGoldDungeonAbandon
    {
        add { AddDelegate<GoldDungeonAbandonResponseBody>(ClientCommandName.GoldDungeonAbandon, value); }
        remove { DelDelegate<GoldDungeonAbandonResponseBody>(ClientCommandName.GoldDungeonAbandon, value); }
    }

    // 골드던전퇴장
    public event Delegate<int, GoldDungeonExitResponseBody> EventResGoldDungeonExit
    {
        add { AddDelegate<GoldDungeonExitResponseBody>(ClientCommandName.GoldDungeonExit, value); }
        remove { DelDelegate<GoldDungeonExitResponseBody>(ClientCommandName.GoldDungeonExit, value); }
    }

    // 골드던전부활
    public event Delegate<int, GoldDungeonReviveResponseBody> EventResGoldDungeonRevive
    {
        add { AddDelegate<GoldDungeonReviveResponseBody>(ClientCommandName.GoldDungeonRevive, value); }
        remove { DelDelegate<GoldDungeonReviveResponseBody>(ClientCommandName.GoldDungeonRevive, value); }
    }

    // 골드던전소탕
    public event Delegate<int, GoldDungeonSweepResponseBody> EventResGoldDungeonSweep
    {
        add { AddDelegate<GoldDungeonSweepResponseBody>(ClientCommandName.GoldDungeonSweep, value); }
        remove { DelDelegate<GoldDungeonSweepResponseBody>(ClientCommandName.GoldDungeonSweep, value); }
    }

    #endregion GoldDungeon

    #region TreatOfFarm

    // 농장의위협퀘스트수락
    public event Delegate<int, TreatOfFarmQuestAcceptResponseBody> EventResTreatOfFarmQuestAccept
    {
        add { AddDelegate<TreatOfFarmQuestAcceptResponseBody>(ClientCommandName.TreatOfFarmQuestAccept, value); }
        remove { DelDelegate<TreatOfFarmQuestAcceptResponseBody>(ClientCommandName.TreatOfFarmQuestAccept, value); }
    }

    // 농장의위협퀘스트완료
    public event Delegate<int, TreatOfFarmQuestCompleteResponseBody> EventResTreatOfFarmQuestComplete
    {
        add { AddDelegate<TreatOfFarmQuestCompleteResponseBody>(ClientCommandName.TreatOfFarmQuestComplete, value); }
        remove { DelDelegate<TreatOfFarmQuestCompleteResponseBody>(ClientCommandName.TreatOfFarmQuestComplete, value); }
    }

    // 농장의위협퀘스트미션수락
    public event Delegate<int, TreatOfFarmQuestMissionAcceptResponseBody> EventResTreatOfFarmQuestMissionAccept
    {
        add { AddDelegate<TreatOfFarmQuestMissionAcceptResponseBody>(ClientCommandName.TreatOfFarmQuestMissionAccept, value); }
        remove { DelDelegate<TreatOfFarmQuestMissionAcceptResponseBody>(ClientCommandName.TreatOfFarmQuestMissionAccept, value); }
    }

    // 농장의위협퀘스트미션포기
    public event Delegate<int, TreatOfFarmQuestMissionAbandonResponseBody> EventResTreatOfFarmQuestMissionAbandon
    {
        add { AddDelegate<TreatOfFarmQuestMissionAbandonResponseBody>(ClientCommandName.TreatOfFarmQuestMissionAbandon, value); }
        remove { DelDelegate<TreatOfFarmQuestMissionAbandonResponseBody>(ClientCommandName.TreatOfFarmQuestMissionAbandon, value); }
    }

    #endregion TreatOfFarm

    #region Nation

    // 국가전송
    public event Delegate<int, NationTransmissionResponseBody> EventResNationTransmission
    {
        add { AddDelegate<NationTransmissionResponseBody>(ClientCommandName.NationTransmission, value); }
        remove { DelDelegate<NationTransmissionResponseBody>(ClientCommandName.NationTransmission, value); }
    }

    // 국가전송을위한대륙입장
    public event Delegate<int, ContinentEnterForNationTransmissionResponseBody> EventResContinentEnterForNationTransmission
    {
        add { AddDelegate<ContinentEnterForNationTransmissionResponseBody>(ClientCommandName.ContinentEnterForNationTransmission, value); }
        remove { DelDelegate<ContinentEnterForNationTransmissionResponseBody>(ClientCommandName.ContinentEnterForNationTransmission, value); }
    }

    // 영웅검색
    public event Delegate<int, HeroSearchResponseBody> EventResHeroSearch
    {
        add { AddDelegate<HeroSearchResponseBody>(ClientCommandName.HeroSearch, value); }
        remove { DelDelegate<HeroSearchResponseBody>(ClientCommandName.HeroSearch, value); }
    }

    //// 국가기부
    public event Delegate<int, NationDonateResponseBody> EventResNationDonate
    {
        add { AddDelegate<NationDonateResponseBody>(ClientCommandName.NationDonate, value); }
        remove { DelDelegate<NationDonateResponseBody>(ClientCommandName.NationDonate, value); }
    }

    // 국가관직임명
    public event Delegate<int, NationNoblesseAppointResponseBody> EventResNationNoblesseAppoint
    {
        add { AddDelegate<NationNoblesseAppointResponseBody>(ClientCommandName.NationNoblesseAppoint, value); }
        remove { DelDelegate<NationNoblesseAppointResponseBody>(ClientCommandName.NationNoblesseAppoint, value); }
    }

    // 국가관직해임
    public event Delegate<int, NationNoblesseDismissResponseBody> EventResNationNoblesseDismiss
    {
        add { AddDelegate<NationNoblesseDismissResponseBody>(ClientCommandName.NationNoblesseDismiss, value); }
        remove { DelDelegate<NationNoblesseDismissResponseBody>(ClientCommandName.NationNoblesseDismiss, value); }
    }

    // 국가소집
    public event Delegate<int, NationCallResponseBody> EventResNationCall
    {
        add { AddDelegate<NationCallResponseBody>(ClientCommandName.NationCall, value); }
        remove { DelDelegate<NationCallResponseBody>(ClientCommandName.NationCall, value); }
    }

    // 국가소집전송
    public event Delegate<int, NationCallTransmissionResponseBody> EventResNationCallTransmission
    {
        add { AddDelegate<NationCallTransmissionResponseBody>(ClientCommandName.NationCallTransmission, value); }
        remove { DelDelegate<NationCallTransmissionResponseBody>(ClientCommandName.NationCallTransmission, value); }
    }

    // 국가소집전송에대한대륙입장
    public event Delegate<int, ContinentEnterForNationCallTransmissionResponseBody> EventResContinentEnterForNationCallTransmission
    {
        add { AddDelegate<ContinentEnterForNationCallTransmissionResponseBody>(ClientCommandName.ContinentEnterForNationCallTransmission, value); }
        remove { DelDelegate<ContinentEnterForNationCallTransmissionResponseBody>(ClientCommandName.ContinentEnterForNationCallTransmission, value); }
    }

    // 국가동맹신청
    public event Delegate<int, NationAllianceApplyResponseBody> EventResNationAllianceApply
    {
        add { AddDelegate<NationAllianceApplyResponseBody>(ClientCommandName.NationAllianceApply, value); }
        remove { DelDelegate<NationAllianceApplyResponseBody>(ClientCommandName.NationAllianceApply, value); }
    }

    // 국가동맹신청수락
    public event Delegate<int, NationAllianceApplicationAcceptResponseBody> EventResNationAllianceApplicationAccept
    {
        add { AddDelegate<NationAllianceApplicationAcceptResponseBody>(ClientCommandName.NationAllianceApplicationAccept, value); }
        remove { DelDelegate<NationAllianceApplicationAcceptResponseBody>(ClientCommandName.NationAllianceApplicationAccept, value); }
    }

    // 국가동맹신청취소
    public event Delegate<int, NationAllianceApplicationCancelResponseBody> EventResNationAllianceApplicationCancel
    {
        add { AddDelegate<NationAllianceApplicationCancelResponseBody>(ClientCommandName.NationAllianceApplicationCancel, value); }
        remove { DelDelegate<NationAllianceApplicationCancelResponseBody>(ClientCommandName.NationAllianceApplicationCancel, value); }
    }

    // 국가동맹신청거절
    public event Delegate<int, NationAllianceApplicationRejectResponseBody> EventResNationAllianceApplicationReject
    {
        add { AddDelegate<NationAllianceApplicationRejectResponseBody>(ClientCommandName.NationAllianceApplicationReject, value); }
        remove { DelDelegate<NationAllianceApplicationRejectResponseBody>(ClientCommandName.NationAllianceApplicationReject, value); }
    }

    // 국가동맹파기
    public event Delegate<int, NationAllianceBreakResponseBody> EventResNationAllianceBreak
    {
        add { AddDelegate<NationAllianceBreakResponseBody>(ClientCommandName.NationAllianceBreak, value); }
        remove { DelDelegate<NationAllianceBreakResponseBody>(ClientCommandName.NationAllianceBreak, value); }
    }

    #endregion Nation

    #region UndergroundMaze

    // 지하미로입장을위한대륙퇴장
    public event Delegate<int, ContinentExitForUndergroundMazeEnterResponseBody> EventResContinentExitForUndergroundMazeEnter
    {
        add { AddDelegate<ContinentExitForUndergroundMazeEnterResponseBody>(ClientCommandName.ContinentExitForUndergroundMazeEnter, value); }
        remove { DelDelegate<ContinentExitForUndergroundMazeEnterResponseBody>(ClientCommandName.ContinentExitForUndergroundMazeEnter, value); }
    }

    // 지하미로입장
    public event Delegate<int, UndergroundMazeEnterResponseBody> EventResUndergroundMazeEnter
    {
        add { AddDelegate<UndergroundMazeEnterResponseBody>(ClientCommandName.UndergroundMazeEnter, value); }
        remove { DelDelegate<UndergroundMazeEnterResponseBody>(ClientCommandName.UndergroundMazeEnter, value); }
    }

    // 지하미로퇴장
    public event Delegate<int, UndergroundMazeExitResponseBody> EventResUndergroundMazeExit
    {
        add { AddDelegate<UndergroundMazeExitResponseBody>(ClientCommandName.UndergroundMazeExit, value); }
        remove { DelDelegate<UndergroundMazeExitResponseBody>(ClientCommandName.UndergroundMazeExit, value); }
    }

    // 지하미로부활
    public event Delegate<int, UndergroundMazeReviveResponseBody> EventResUndergroundMazeRevive
    {
        add { AddDelegate<UndergroundMazeReviveResponseBody>(ClientCommandName.UndergroundMazeRevive, value); }
        remove { DelDelegate<UndergroundMazeReviveResponseBody>(ClientCommandName.UndergroundMazeRevive, value); }
    }

    // 부활에대한지하미로입장
    public event Delegate<int, UndergroundMazeEnterForUndergroundMazeReviveResponseBody> EventResUndergroundMazeEnterForUndergroundMazeRevive
    {
        add { AddDelegate<UndergroundMazeEnterForUndergroundMazeReviveResponseBody>(ClientCommandName.UndergroundMazeEnterForUndergroundMazeRevive, value); }
        remove { DelDelegate<UndergroundMazeEnterForUndergroundMazeReviveResponseBody>(ClientCommandName.UndergroundMazeEnterForUndergroundMazeRevive, value); }
    }

    // 지하미로포탈입장
    public event Delegate<int, UndergroundMazePortalEnterResponseBody> EventResUndergroundMazePortalEnter
    {
        add { AddDelegate<UndergroundMazePortalEnterResponseBody>(ClientCommandName.UndergroundMazePortalEnter, value); }
        remove { DelDelegate<UndergroundMazePortalEnterResponseBody>(ClientCommandName.UndergroundMazePortalEnter, value); }
    }

    // 지하미로포탈퇴장
    public event Delegate<int, UndergroundMazePortalExitResponseBody> EventResUndergroundMazePortalExit
    {
        add { AddDelegate<UndergroundMazePortalExitResponseBody>(ClientCommandName.UndergroundMazePortalExit, value); }
        remove { DelDelegate<UndergroundMazePortalExitResponseBody>(ClientCommandName.UndergroundMazePortalExit, value); }
    }

    // 지하미로전송
    public event Delegate<int, UndergroundMazeTransmissionResponseBody> EventResUndergroundMazeTransmission
    {
        add { AddDelegate<UndergroundMazeTransmissionResponseBody>(ClientCommandName.UndergroundMazeTransmission, value); }
        remove { DelDelegate<UndergroundMazeTransmissionResponseBody>(ClientCommandName.UndergroundMazeTransmission, value); }
    }

    // 전송에대한지하미로입장
    public event Delegate<int, UndergroundMazeEnterForTransmissionResponseBody> EventResUndergroundMazeEnterForTransmission
    {
        add { AddDelegate<UndergroundMazeEnterForTransmissionResponseBody>(ClientCommandName.UndergroundMazeEnterForTransmission, value); }
        remove { DelDelegate<UndergroundMazeEnterForTransmissionResponseBody>(ClientCommandName.UndergroundMazeEnterForTransmission, value); }
    }

    #endregion UndergroundMaze

    #region BountyHunterQuest

    // 현상금사냥꾼퀘스트주문서사용
    public event Delegate<int, BountyHunterQuestScrollUseResponseBody> EventResBountyHunterQuestScrollUse
    {
        add { AddDelegate<BountyHunterQuestScrollUseResponseBody>(ClientCommandName.BountyHunterQuestScrollUse, value); }
        remove { DelDelegate<BountyHunterQuestScrollUseResponseBody>(ClientCommandName.BountyHunterQuestScrollUse, value); }
    }

    // 현상금사냥꾼퀘스트완료
    public event Delegate<int, BountyHunterQuestCompleteResponseBody> EventResBountyHunterQuestComplete
    {
        add { AddDelegate<BountyHunterQuestCompleteResponseBody>(ClientCommandName.BountyHunterQuestComplete, value); }
        remove { DelDelegate<BountyHunterQuestCompleteResponseBody>(ClientCommandName.BountyHunterQuestComplete, value); }
    }

    // 현상금사냥꾼퀘스트포기
    public event Delegate<int, BountyHunterQuestAbandonResponseBody> EventResBountyHunterQuestAbandon
    {
        add { AddDelegate<BountyHunterQuestAbandonResponseBody>(ClientCommandName.BountyHunterQuestAbandon, value); }
        remove { DelDelegate<BountyHunterQuestAbandonResponseBody>(ClientCommandName.BountyHunterQuestAbandon, value); }
    }

    #endregion BountyHunterQuest

    #region Fishing

    // 낚시미끼사용
    public event Delegate<int, FishingBaitUseResponseBody> EventResFishingBaitUse
    {
        add { AddDelegate<FishingBaitUseResponseBody>(ClientCommandName.FishingBaitUse, value); }
        remove { DelDelegate<FishingBaitUseResponseBody>(ClientCommandName.FishingBaitUse, value); }
    }

    // 낚시시작
    public event Delegate<int, FishingStartResponseBody> EventResFishingStart
    {
        add { AddDelegate<FishingStartResponseBody>(ClientCommandName.FishingStart, value); }
        remove { DelDelegate<FishingStartResponseBody>(ClientCommandName.FishingStart, value); }
    }

    #endregion Fishing

    #region ArtifactRoom

    // 고대유물의방입장을위한대륙퇴장
    public event Delegate<int, ContinentExitForArtifactRoomEnterResponseBody> EventResContinentExitForArtifactRoomEnter
    {
        add { AddDelegate<ContinentExitForArtifactRoomEnterResponseBody>(ClientCommandName.ContinentExitForArtifactRoomEnter, value); }
        remove { DelDelegate<ContinentExitForArtifactRoomEnterResponseBody>(ClientCommandName.ContinentExitForArtifactRoomEnter, value); }
    }

    // 고대유물의방입장
    public event Delegate<int, ArtifactRoomEnterResponseBody> EventResArtifactRoomEnter
    {
        add { AddDelegate<ArtifactRoomEnterResponseBody>(ClientCommandName.ArtifactRoomEnter, value); }
        remove { DelDelegate<ArtifactRoomEnterResponseBody>(ClientCommandName.ArtifactRoomEnter, value); }
    }

    // 고대유물의방퇴장
    public event Delegate<int, ArtifactRoomExitResponseBody> EventResArtifactRoomExit
    {
        add { AddDelegate<ArtifactRoomExitResponseBody>(ClientCommandName.ArtifactRoomExit, value); }
        remove { DelDelegate<ArtifactRoomExitResponseBody>(ClientCommandName.ArtifactRoomExit, value); }
    }

    // 고대유물의방포기
    public event Delegate<int, ArtifactRoomAbandonResponseBody> EventResArtifactRoomAbandon
    {
        add { AddDelegate<ArtifactRoomAbandonResponseBody>(ClientCommandName.ArtifactRoomAbandon, value); }
        remove { DelDelegate<ArtifactRoomAbandonResponseBody>(ClientCommandName.ArtifactRoomAbandon, value); }
    }

    // 고대유물의방다음층도전
    public event Delegate<int, ArtifactRoomNextFloorChallengeResponseBody> EventResArtifactRoomNextFloorChallenge
    {
        add { AddDelegate<ArtifactRoomNextFloorChallengeResponseBody>(ClientCommandName.ArtifactRoomNextFloorChallenge, value); }
        remove { DelDelegate<ArtifactRoomNextFloorChallengeResponseBody>(ClientCommandName.ArtifactRoomNextFloorChallenge, value); }
    }

    // 고대유물의방초기화
    public event Delegate<int, ArtifactRoomInitResponseBody> EventResArtifactRoomInit
    {
        add { AddDelegate<ArtifactRoomInitResponseBody>(ClientCommandName.ArtifactRoomInit, value); }
        remove { DelDelegate<ArtifactRoomInitResponseBody>(ClientCommandName.ArtifactRoomInit, value); }
    }

    // 고대유물의방소탕
    public event Delegate<int, ArtifactRoomSweepResponseBody> EventResArtifactRoomSweep
    {
        add { AddDelegate<ArtifactRoomSweepResponseBody>(ClientCommandName.ArtifactRoomSweep, value); }
        remove { DelDelegate<ArtifactRoomSweepResponseBody>(ClientCommandName.ArtifactRoomSweep, value); }
    }

    // 고대유물의방소탕가속
    public event Delegate<int, ArtifactRoomSweepAccelerateResponseBody> EventResArtifactRoomSweepAccelerate
    {
        add { AddDelegate<ArtifactRoomSweepAccelerateResponseBody>(ClientCommandName.ArtifactRoomSweepAccelerate, value); }
        remove { DelDelegate<ArtifactRoomSweepAccelerateResponseBody>(ClientCommandName.ArtifactRoomSweepAccelerate, value); }
    }

    // 고대유물의방소탕완료
    public event Delegate<int, ArtifactRoomSweepCompleteResponseBody> EventResArtifactRoomSweepComplete
    {
        add { AddDelegate<ArtifactRoomSweepCompleteResponseBody>(ClientCommandName.ArtifactRoomSweepComplete, value); }
        remove { DelDelegate<ArtifactRoomSweepCompleteResponseBody>(ClientCommandName.ArtifactRoomSweepComplete, value); }
    }

    // 고대유물의방소탕중지
    public event Delegate<int, ArtifactRoomSweepStopResponseBody> EventResArtifactRoomSweepStop
    {
        add { AddDelegate<ArtifactRoomSweepStopResponseBody>(ClientCommandName.ArtifactRoomSweepStop, value); }
        remove { DelDelegate<ArtifactRoomSweepStopResponseBody>(ClientCommandName.ArtifactRoomSweepStop, value); }
    }

    #endregion ArtifactRoom

    #region MysteryBox

    // 의문의상자퀘스트수락
    public event Delegate<int, MysteryBoxQuestAcceptResponseBody> EventResMysteryBoxQuestAccept
    {
        add { AddDelegate<MysteryBoxQuestAcceptResponseBody>(ClientCommandName.MysteryBoxQuestAccept, value); }
        remove { DelDelegate<MysteryBoxQuestAcceptResponseBody>(ClientCommandName.MysteryBoxQuestAccept, value); }
    }

    // 의문의상자퀘스트완료
    public event Delegate<int, MysteryBoxQuestCompleteResponseBody> EventResMysteryBoxQuestComplete
    {
        add { AddDelegate<MysteryBoxQuestCompleteResponseBody>(ClientCommandName.MysteryBoxQuestComplete, value); }
        remove { DelDelegate<MysteryBoxQuestCompleteResponseBody>(ClientCommandName.MysteryBoxQuestComplete, value); }
    }

    // 의문의상자뽑기시작
    public event Delegate<int, MysteryBoxPickStartResponseBody> EventResMysteryBoxPickStart
    {
        add { AddDelegate<MysteryBoxPickStartResponseBody>(ClientCommandName.MysteryBoxPickStart, value); }
        remove { DelDelegate<MysteryBoxPickStartResponseBody>(ClientCommandName.MysteryBoxPickStart, value); }
    }

    #endregion MysteryBox

    #region SecretLetter

    // 밀서퀘스트수락
    public event Delegate<int, SecretLetterQuestAcceptResponseBody> EventResSecretLetterQuestAccept
    {
        add { AddDelegate<SecretLetterQuestAcceptResponseBody>(ClientCommandName.SecretLetterQuestAccept, value); }
        remove { DelDelegate<SecretLetterQuestAcceptResponseBody>(ClientCommandName.SecretLetterQuestAccept, value); }
    }

    // 밀서퀘스트완료
    public event Delegate<int, SecretLetterQuestCompleteResponseBody> EventResSecretLetterQuestComplete
    {
        add { AddDelegate<SecretLetterQuestCompleteResponseBody>(ClientCommandName.SecretLetterQuestComplete, value); }
        remove { DelDelegate<SecretLetterQuestCompleteResponseBody>(ClientCommandName.SecretLetterQuestComplete, value); }
    }

    // 밀서뽑기시작
    public event Delegate<int, SecretLetterPickStartResponseBody> EventResSecretLetterPickStart
    {
        add { AddDelegate<SecretLetterPickStartResponseBody>(ClientCommandName.SecretLetterPickStart, value); }
        remove { DelDelegate<SecretLetterPickStartResponseBody>(ClientCommandName.SecretLetterPickStart, value); }
    }

    #endregion SecretLetter

    #region DimensionRaidQuest

    // 차원습격퀘스트수락
    public event Delegate<int, DimensionRaidQuestAcceptResponseBody> EventResDimensionRaidQuestAccept
    {
        add { AddDelegate<DimensionRaidQuestAcceptResponseBody>(ClientCommandName.DimensionRaidQuestAccept, value); }
        remove { DelDelegate<DimensionRaidQuestAcceptResponseBody>(ClientCommandName.DimensionRaidQuestAccept, value); }
    }

    // 차원습격퀘스트완료
    public event Delegate<int, DimensionRaidQuestCompleteResponseBody> EventResDimensionRaidQuestComplete
    {
        add { AddDelegate<DimensionRaidQuestCompleteResponseBody>(ClientCommandName.DimensionRaidQuestComplete, value); }
        remove { DelDelegate<DimensionRaidQuestCompleteResponseBody>(ClientCommandName.DimensionRaidQuestComplete, value); }
    }

    // 차원습격상호작용시작
    public event Delegate<int, DimensionRaidInteractionStartResponseBody> EventResDimensionRaidInteractionStart
    {
        add { AddDelegate<DimensionRaidInteractionStartResponseBody>(ClientCommandName.DimensionRaidInteractionStart, value); }
        remove { DelDelegate<DimensionRaidInteractionStartResponseBody>(ClientCommandName.DimensionRaidInteractionStart, value); }
    }

    #endregion DimensionRaidQuest

    #region AncientRelic

    // 고대인의유적매칭시작
    public event Delegate<int, AncientRelicMatchingStartResponseBody> EventResAncientRelicMatchingStart
    {
        add { AddDelegate<AncientRelicMatchingStartResponseBody>(ClientCommandName.AncientRelicMatchingStart, value); }
        remove { DelDelegate<AncientRelicMatchingStartResponseBody>(ClientCommandName.AncientRelicMatchingStart, value); }
    }

    // 고대인의유적매칭취소
    public event Delegate<int, AncientRelicMatchingCancelResponseBody> EventResAncientRelicMatchingCancel
    {
        add { AddDelegate<AncientRelicMatchingCancelResponseBody>(ClientCommandName.AncientRelicMatchingCancel, value); }
        remove { DelDelegate<AncientRelicMatchingCancelResponseBody>(ClientCommandName.AncientRelicMatchingCancel, value); }
    }

    // 고대인의유적입장
    public event Delegate<int, AncientRelicEnterResponseBody> EventResAncientRelicEnter
    {
        add { AddDelegate<AncientRelicEnterResponseBody>(ClientCommandName.AncientRelicEnter, value); }
        remove { DelDelegate<AncientRelicEnterResponseBody>(ClientCommandName.AncientRelicEnter, value); }
    }

    // 고대인의유적퇴장
    public event Delegate<int, AncientRelicExitResponseBody> EventResAncientRelicExit
    {
        add { AddDelegate<AncientRelicExitResponseBody>(ClientCommandName.AncientRelicExit, value); }
        remove { DelDelegate<AncientRelicExitResponseBody>(ClientCommandName.AncientRelicExit, value); }
    }

    // 고대인의유적포기
    public event Delegate<int, AncientRelicAbandonResponseBody> EventResAncientRelicAbandon
    {
        add { AddDelegate<AncientRelicAbandonResponseBody>(ClientCommandName.AncientRelicAbandon, value); }
        remove { DelDelegate<AncientRelicAbandonResponseBody>(ClientCommandName.AncientRelicAbandon, value); }
    }

    // 고대인의유적부활
    public event Delegate<int, AncientRelicReviveResponseBody> EventResAncientRelicRevive
    {
        add { AddDelegate<AncientRelicReviveResponseBody>(ClientCommandName.AncientRelicRevive, value); }
        remove { DelDelegate<AncientRelicReviveResponseBody>(ClientCommandName.AncientRelicRevive, value); }
    }

    #endregion AncientRelic

    #region Today

    // 달성보상받기
    public event Delegate<int, AchievementRewardReceiveResponseBody> EventResAchievementRewardReceive
    {
        add { AddDelegate<AchievementRewardReceiveResponseBody>(ClientCommandName.AchievementRewardReceive, value); }
        remove { DelDelegate<AchievementRewardReceiveResponseBody>(ClientCommandName.AchievementRewardReceive, value); }
    }

    #endregion Today

    #region HolyWarQuest

    // 위대한성전퀘스트수락
    public event Delegate<int, HolyWarQuestAcceptResponseBody> EventResHolyWarQuestAccept
    {
        add { AddDelegate<HolyWarQuestAcceptResponseBody>(ClientCommandName.HolyWarQuestAccept, value); }
        remove { DelDelegate<HolyWarQuestAcceptResponseBody>(ClientCommandName.HolyWarQuestAccept, value); }
    }

    // 위대한성전퀘스트완료
    public event Delegate<int, HolyWarQuestCompleteResponseBody> EventResHolyWarQuestComplete
    {
        add { AddDelegate<HolyWarQuestCompleteResponseBody>(ClientCommandName.HolyWarQuestComplete, value); }
        remove { DelDelegate<HolyWarQuestCompleteResponseBody>(ClientCommandName.HolyWarQuestComplete, value); }
    }

    #endregion HolyWarQuest

    #region VIP

    // Vip레벨보상받기
    public event Delegate<int, VipLevelRewardReceiveResponseBody> EventResVipLevelRewardReceive
    {
        add { AddDelegate<VipLevelRewardReceiveResponseBody>(ClientCommandName.VipLevelRewardReceive, value); }
        remove { DelDelegate<VipLevelRewardReceiveResponseBody>(ClientCommandName.VipLevelRewardReceive, value); }
    }

    #endregion VIP

    #region Rank

    // 계급획득
    public event Delegate<int, RankAcquireResponseBody> EventResRankAcquire
    {
        add { AddDelegate<RankAcquireResponseBody>(ClientCommandName.RankAcquire, value); }
        remove { DelDelegate<RankAcquireResponseBody>(ClientCommandName.RankAcquire, value); }
    }

    // 계급보상받기
    public event Delegate<int, RankRewardReceiveResponseBody> EventResRankRewardReceive
    {
        add { AddDelegate<RankRewardReceiveResponseBody>(ClientCommandName.RankRewardReceive, value); }
        remove { DelDelegate<RankRewardReceiveResponseBody>(ClientCommandName.RankRewardReceive, value); }
    }

    // 계급액티브스킬레벨업
    public event Delegate<int, RankActiveSkillLevelUpResponseBody> EventResRankActiveSkillLevelUp
    {
        add { AddDelegate<RankActiveSkillLevelUpResponseBody>(ClientCommandName.RankActiveSkillLevelUp, value); }
        remove { DelDelegate<RankActiveSkillLevelUpResponseBody>(ClientCommandName.RankActiveSkillLevelUp, value); }
    }

    // 계급액티브스킬선택
    public event Delegate<int, RankActiveSkillSelectResponseBody> EventResRankActiveSkillSelect
    {
        add { AddDelegate<RankActiveSkillSelectResponseBody>(ClientCommandName.RankActiveSkillSelect, value); }
        remove { DelDelegate<RankActiveSkillSelectResponseBody>(ClientCommandName.RankActiveSkillSelect, value); }
    }

    // 계급패시브스킬레벨업
    public event Delegate<int, RankPassiveSkillLevelUpResponseBody> EventResRankPassiveSkillLevelUp
    {
        add { AddDelegate<RankPassiveSkillLevelUpResponseBody>(ClientCommandName.RankPassiveSkillLevelUp, value); }
        remove { DelDelegate<RankPassiveSkillLevelUpResponseBody>(ClientCommandName.RankPassiveSkillLevelUp, value); }
    }

    #endregion Rank

    #region HonorShop

    // 명예상점상품구매
    public event Delegate<int, HonorShopProductBuyResponseBody> EventResHonorShopProductBuy
    {
        add { AddDelegate<HonorShopProductBuyResponseBody>(ClientCommandName.HonorShopProductBuy, value); }
        remove { DelDelegate<HonorShopProductBuyResponseBody>(ClientCommandName.HonorShopProductBuy, value); }
    }

    #endregion HonorShop

    #region Ranking

    // 서버전투력랭킹
    public event Delegate<int, ServerBattlePowerRankingResponseBody> EventResServerBattlePowerRanking
    {
        add { AddDelegate<ServerBattlePowerRankingResponseBody>(ClientCommandName.ServerBattlePowerRanking, value); }
        remove { DelDelegate<ServerBattlePowerRankingResponseBody>(ClientCommandName.ServerBattlePowerRanking, value); }
    }

    // 서버직업전투력랭킹
    public event Delegate<int, ServerJobBattlePowerRankingResponseBody> EventResServerJobBattlePowerRanking
    {
        add { AddDelegate<ServerJobBattlePowerRankingResponseBody>(ClientCommandName.ServerJobBattlePowerRanking, value); }
        remove { DelDelegate<ServerJobBattlePowerRankingResponseBody>(ClientCommandName.ServerJobBattlePowerRanking, value); }
    }

    // 서버레벨랭킹
    public event Delegate<int, ServerLevelRankingResponseBody> EventResServerLevelRanking
    {
        add { AddDelegate<ServerLevelRankingResponseBody>(ClientCommandName.ServerLevelRanking, value); }
        remove { DelDelegate<ServerLevelRankingResponseBody>(ClientCommandName.ServerLevelRanking, value); }
    }

    // 서버레벨랭킹보상받기
    public event Delegate<int, ServerLevelRankingRewardReceiveResponseBody> EventResServerLevelRankingRewardReceive
    {
        add { AddDelegate<ServerLevelRankingRewardReceiveResponseBody>(ClientCommandName.ServerLevelRankingRewardReceive, value); }
        remove { DelDelegate<ServerLevelRankingRewardReceiveResponseBody>(ClientCommandName.ServerLevelRankingRewardReceive, value); }
    }

    // 본국전투력랭킹
    public event Delegate<int, NationBattlePowerRankingResponseBody> EventResNationBattlePowerRanking
    {
        add { AddDelegate<NationBattlePowerRankingResponseBody>(ClientCommandName.NationBattlePowerRanking, value); }
        remove { DelDelegate<NationBattlePowerRankingResponseBody>(ClientCommandName.NationBattlePowerRanking, value); }
    }

    // 본국공적포인트랭킹
    public event Delegate<int, NationExploitPointRankingResponseBody> EventResNationExploitPointRanking
    {
        add { AddDelegate<NationExploitPointRankingResponseBody>(ClientCommandName.NationExploitPointRanking, value); }
        remove { DelDelegate<NationExploitPointRankingResponseBody>(ClientCommandName.NationExploitPointRanking, value); }
    }

    // 서버크리쳐카드랭킹
    public event Delegate<int, ServerCreatureCardRankingResponseBody> EventResServerCreatureCardRanking
    {
        add { AddDelegate<ServerCreatureCardRankingResponseBody>(ClientCommandName.ServerCreatureCardRanking, value); }
        remove { DelDelegate<ServerCreatureCardRankingResponseBody>(ClientCommandName.ServerCreatureCardRanking, value); }
    }

    // 서버도감랭킹
    public event Delegate<int, ServerIllustratedBookRankingResponseBody> EventResServerIllustratedBookRanking
    {
        add { AddDelegate<ServerIllustratedBookRankingResponseBody>(ClientCommandName.ServerIllustratedBookRanking, value); }
        remove { DelDelegate<ServerIllustratedBookRankingResponseBody>(ClientCommandName.ServerIllustratedBookRanking, value); }
    }

    #endregion Ranking

    #region GuildRanking

    // 서버길드랭킹
    public event Delegate<int, ServerGuildRankingResponseBody> EventResServerGuildRanking
    {
        add { AddDelegate<ServerGuildRankingResponseBody>(ClientCommandName.ServerGuildRanking, value); }
        remove { DelDelegate<ServerGuildRankingResponseBody>(ClientCommandName.ServerGuildRanking, value); }
    }

    // 국가길드랭킹
    public event Delegate<int, NationGuildRankingResponseBody> EventResNationGuildRanking
    {
        add { AddDelegate<NationGuildRankingResponseBody>(ClientCommandName.NationGuildRanking, value); }
        remove { DelDelegate<NationGuildRankingResponseBody>(ClientCommandName.NationGuildRanking, value); }
    }


    #endregion GuildRanking

    #region Attainment

    // 도달보상받기
    public event Delegate<int, AttainmentRewardReceiveResponseBody> EventResAttainmentRewardReceive
    {
        add { AddDelegate<AttainmentRewardReceiveResponseBody>(ClientCommandName.AttainmentRewardReceive, value); }
        remove { DelDelegate<AttainmentRewardReceiveResponseBody>(ClientCommandName.AttainmentRewardReceive, value); }
    }

    #endregion Attainment

    #region Cart

    // 카트타기
    public event Delegate<int, CartGetOnResponseBody> EventResCartGetOn
    {
        add { AddDelegate<CartGetOnResponseBody>(ClientCommandName.CartGetOn, value); }
        remove { DelDelegate<CartGetOnResponseBody>(ClientCommandName.CartGetOn, value); }
    }

    // 카트내리기
    public event Delegate<int, CartGetOffResponseBody> EventResCartGetOff
    {
        add { AddDelegate<CartGetOffResponseBody>(ClientCommandName.CartGetOff, value); }
        remove { DelDelegate<CartGetOffResponseBody>(ClientCommandName.CartGetOff, value); }
    }

    // 카트가속
    public event Delegate<int, CartAccelerateResponseBody> EventResCartAccelerate
    {
        add { AddDelegate<CartAccelerateResponseBody>(ClientCommandName.CartAccelerate, value); }
        remove { DelDelegate<CartAccelerateResponseBody>(ClientCommandName.CartAccelerate, value); }
    }

    // 카트포탈입장
    public event Delegate<int, CartPortalEnterResponseBody> EventResCartPortalEnter
    {
        add { AddDelegate<CartPortalEnterResponseBody>(ClientCommandName.CartPortalEnter, value); }
        remove { DelDelegate<CartPortalEnterResponseBody>(ClientCommandName.CartPortalEnter, value); }
    }

    // 카트포탈퇴장
    public event Delegate<int, CartPortalExitResponseBody> EventResCartPortalExit
    {
        add { AddDelegate<CartPortalExitResponseBody>(ClientCommandName.CartPortalExit, value); }
        remove { DelDelegate<CartPortalExitResponseBody>(ClientCommandName.CartPortalExit, value); }
    }

    #endregion Cart

    #region FieldOfHonor

    // 결투장정보
    public event Delegate<int, FieldOfHonorInfoResponseBody> EventResFieldOfHonorInfo
    {
        add { AddDelegate<FieldOfHonorInfoResponseBody>(ClientCommandName.FieldOfHonorInfo, value); }
        remove { DelDelegate<FieldOfHonorInfoResponseBody>(ClientCommandName.FieldOfHonorInfo, value); }
    }

    // 결투장랭킹목록
    public event Delegate<int, FieldOfHonorTopRankingListResponseBody> EventResFieldOfHonorTopRankingList
    {
        add { AddDelegate<FieldOfHonorTopRankingListResponseBody>(ClientCommandName.FieldOfHonorTopRankingList, value); }
        remove { DelDelegate<FieldOfHonorTopRankingListResponseBody>(ClientCommandName.FieldOfHonorTopRankingList, value); }
    }

    // 결투장랭커정보
    public event Delegate<int, FieldOfHonorRankerInfoResponseBody> EventResFieldOfHonorRankerInfo
    {
        add { AddDelegate<FieldOfHonorRankerInfoResponseBody>(ClientCommandName.FieldOfHonorRankerInfo, value); }
        remove { DelDelegate<FieldOfHonorRankerInfoResponseBody>(ClientCommandName.FieldOfHonorRankerInfo, value); }
    }

    // 결투장도전을위한대륙퇴장
    public event Delegate<int, ContinentExitForFieldOfHonorChallengeResponseBody> EventResContinentExitForFieldOfHonorChallenge
    {
        add { AddDelegate<ContinentExitForFieldOfHonorChallengeResponseBody>(ClientCommandName.ContinentExitForFieldOfHonorChallenge, value); }
        remove { DelDelegate<ContinentExitForFieldOfHonorChallengeResponseBody>(ClientCommandName.ContinentExitForFieldOfHonorChallenge, value); }
    }

    // 결투장도전
    public event Delegate<int, FieldOfHonorChallengeResponseBody> EventResFieldOfHonorChallenge
    {
        add { AddDelegate<FieldOfHonorChallengeResponseBody>(ClientCommandName.FieldOfHonorChallenge, value); }
        remove { DelDelegate<FieldOfHonorChallengeResponseBody>(ClientCommandName.FieldOfHonorChallenge, value); }
    }

    // 결투장퇴장
    public event Delegate<int, FieldOfHonorExitResponseBody> EventResFieldOfHonorExit
    {
        add { AddDelegate<FieldOfHonorExitResponseBody>(ClientCommandName.FieldOfHonorExit, value); }
        remove { DelDelegate<FieldOfHonorExitResponseBody>(ClientCommandName.FieldOfHonorExit, value); }
    }

    // 결투장포기
    public event Delegate<int, FieldOfHonorAbandonResponseBody> EventResFieldOfHonorAbandon
    {
        add { AddDelegate<FieldOfHonorAbandonResponseBody>(ClientCommandName.FieldOfHonorAbandon, value); }
        remove { DelDelegate<FieldOfHonorAbandonResponseBody>(ClientCommandName.FieldOfHonorAbandon, value); }
    }

    // 결투장랭킹보상받기
    public event Delegate<int, FieldOfHonorRankingRewardReceiveResponseBody> EventResFieldOfHonorRankingRewardReceive
    {
        add { AddDelegate<FieldOfHonorRankingRewardReceiveResponseBody>(ClientCommandName.FieldOfHonorRankingRewardReceive, value); }
        remove { DelDelegate<FieldOfHonorRankingRewardReceiveResponseBody>(ClientCommandName.FieldOfHonorRankingRewardReceive, value); }
    }

    #endregion FieldOfHonor

    #region SupplySupport

    // 보급지원퀘스트수락
    public event Delegate<int, SupplySupportQuestAcceptResponseBody> EventResSupplySupportQuestAccept
    {
        add { AddDelegate<SupplySupportQuestAcceptResponseBody>(ClientCommandName.SupplySupportQuestAccept, value); }
        remove { DelDelegate<SupplySupportQuestAcceptResponseBody>(ClientCommandName.SupplySupportQuestAccept, value); }
    }

    // 보급지원퀘스트카트변경
    public event Delegate<int, SupplySupportQuestCartChangeResponseBody> EventResSupplySupportQuestCartChange
    {
        add { AddDelegate<SupplySupportQuestCartChangeResponseBody>(ClientCommandName.SupplySupportQuestCartChange, value); }
        remove { DelDelegate<SupplySupportQuestCartChangeResponseBody>(ClientCommandName.SupplySupportQuestCartChange, value); }
    }

    // 보급지원퀘스트완료
    public event Delegate<int, SupplySupportQuestCompleteResponseBody> EventResSupplySupportQuestComplete
    {
        add { AddDelegate<SupplySupportQuestCompleteResponseBody>(ClientCommandName.SupplySupportQuestComplete, value); }
        remove { DelDelegate<SupplySupportQuestCompleteResponseBody>(ClientCommandName.SupplySupportQuestComplete, value); }
    }

    #endregion SupplySupport

    #region SoulCoveter

    // 영혼을탐하는자매칭시작
    public event Delegate<int, SoulCoveterMatchingStartResponseBody> EventResSoulCoveterMatchingStart
    {
        add { AddDelegate<SoulCoveterMatchingStartResponseBody>(ClientCommandName.SoulCoveterMatchingStart, value); }
        remove { DelDelegate<SoulCoveterMatchingStartResponseBody>(ClientCommandName.SoulCoveterMatchingStart, value); }
    }

    // 영혼을탐하는자매칭취소
    public event Delegate<int, SoulCoveterMatchingCancelResponseBody> EventResSoulCoveterMatchingCancel
    {
        add { AddDelegate<SoulCoveterMatchingCancelResponseBody>(ClientCommandName.SoulCoveterMatchingCancel, value); }
        remove { DelDelegate<SoulCoveterMatchingCancelResponseBody>(ClientCommandName.SoulCoveterMatchingCancel, value); }
    }

    // 영혼을탐하는자입장
    public event Delegate<int, SoulCoveterEnterResponseBody> EventResSoulCoveterEnter
    {
        add { AddDelegate<SoulCoveterEnterResponseBody>(ClientCommandName.SoulCoveterEnter, value); }
        remove { DelDelegate<SoulCoveterEnterResponseBody>(ClientCommandName.SoulCoveterEnter, value); }
    }

    // 영혼을탐하는자퇴장
    public event Delegate<int, SoulCoveterExitResponseBody> EventResSoulCoveterExit
    {
        add { AddDelegate<SoulCoveterExitResponseBody>(ClientCommandName.SoulCoveterExit, value); }
        remove { DelDelegate<SoulCoveterExitResponseBody>(ClientCommandName.SoulCoveterExit, value); }
    }

    // 영혼을탐하는자포기
    public event Delegate<int, SoulCoveterAbandonResponseBody> EventResSoulCoveterAbandon
    {
        add { AddDelegate<SoulCoveterAbandonResponseBody>(ClientCommandName.SoulCoveterAbandon, value); }
        remove { DelDelegate<SoulCoveterAbandonResponseBody>(ClientCommandName.SoulCoveterAbandon, value); }
    }

    // 영혼을탐하는자부활
    public event Delegate<int, SoulCoveterReviveResponseBody> EventResSoulCoveterRevive
    {
        add { AddDelegate<SoulCoveterReviveResponseBody>(ClientCommandName.SoulCoveterRevive, value); }
        remove { DelDelegate<SoulCoveterReviveResponseBody>(ClientCommandName.SoulCoveterRevive, value); }
    }

    #endregion SoulCoveter

    #region Guild

    // 길드목록
    public event Delegate<int, GuildListResponseBody> EventResGuildList
    {
        add { AddDelegate<GuildListResponseBody>(ClientCommandName.GuildList, value); }
        remove { DelDelegate<GuildListResponseBody>(ClientCommandName.GuildList, value); }
    }

    // 길드생성
    public event Delegate<int, GuildCreateResponseBody> EventResGuildCreate
    {
        add { AddDelegate<GuildCreateResponseBody>(ClientCommandName.GuildCreate, value); }
        remove { DelDelegate<GuildCreateResponseBody>(ClientCommandName.GuildCreate, value); }
    }

    // 길드신청
    public event Delegate<int, GuildApplyResponseBody> EventResGuildApply
    {
        add { AddDelegate<GuildApplyResponseBody>(ClientCommandName.GuildApply, value); }
        remove { DelDelegate<GuildApplyResponseBody>(ClientCommandName.GuildApply, value); }
    }

    // 길드멤버탭정보
    public event Delegate<int, GuildMemberTabInfoResponseBody> EventResGuildMemberTabInfo
    {
        add { AddDelegate<GuildMemberTabInfoResponseBody>(ClientCommandName.GuildMemberTabInfo, value); }
        remove { DelDelegate<GuildMemberTabInfoResponseBody>(ClientCommandName.GuildMemberTabInfo, value); }
    }

    // 길드신청목록
    public event Delegate<int, GuildApplicationListResponseBody> EventResGuildApplicationList
    {
        add { AddDelegate<GuildApplicationListResponseBody>(ClientCommandName.GuildApplicationList, value); }
        remove { DelDelegate<GuildApplicationListResponseBody>(ClientCommandName.GuildApplicationList, value); }
    }

    // 길드신청수락
    public event Delegate<int, GuildApplicationAcceptResponseBody> EventResGuildApplicationAccept
    {
        add { AddDelegate<GuildApplicationAcceptResponseBody>(ClientCommandName.GuildApplicationAccept, value); }
        remove { DelDelegate<GuildApplicationAcceptResponseBody>(ClientCommandName.GuildApplicationAccept, value); }
    }

    // 길드신청거절
    public event Delegate<int, GuildApplicationRefuseResponseBody> EventResGuildApplicationRefuse
    {
        add { AddDelegate<GuildApplicationRefuseResponseBody>(ClientCommandName.GuildApplicationRefuse, value); }
        remove { DelDelegate<GuildApplicationRefuseResponseBody>(ClientCommandName.GuildApplicationRefuse, value); }
    }

    // 길드탈퇴
    public event Delegate<int, GuildExitResponseBody> EventResGuildExit
    {
        add { AddDelegate<GuildExitResponseBody>(ClientCommandName.GuildExit, value); }
        remove { DelDelegate<GuildExitResponseBody>(ClientCommandName.GuildExit, value); }
    }

    // 길드멤버추방
    public event Delegate<int, GuildMemberBanishResponseBody> EventResGuildMemberBanish
    {
        add { AddDelegate<GuildMemberBanishResponseBody>(ClientCommandName.GuildMemberBanish, value); }
        remove { DelDelegate<GuildMemberBanishResponseBody>(ClientCommandName.GuildMemberBanish, value); }
    }

    // 길드초대
    public event Delegate<int, GuildInviteResponseBody> EventResGuildInvite
    {
        add { AddDelegate<GuildInviteResponseBody>(ClientCommandName.GuildInvite, value); }
        remove { DelDelegate<GuildInviteResponseBody>(ClientCommandName.GuildInvite, value); }
    }

    // 길드초대수락
    public event Delegate<int, GuildInvitationAcceptResponseBody> EventResGuildInvitationAccept
    {
        add { AddDelegate<GuildInvitationAcceptResponseBody>(ClientCommandName.GuildInvitationAccept, value); }
        remove { DelDelegate<GuildInvitationAcceptResponseBody>(ClientCommandName.GuildInvitationAccept, value); }
    }

    // 길드초대거절
    public event Delegate<int, GuildInvitationRefuseResponseBody> EventResGuildInvitationRefuse
    {
        add { AddDelegate<GuildInvitationRefuseResponseBody>(ClientCommandName.GuildInvitationRefuse, value); }
        remove { DelDelegate<GuildInvitationRefuseResponseBody>(ClientCommandName.GuildInvitationRefuse, value); }
    }

    // 길드공지설정
    public event Delegate<int, GuildNoticeSetResponseBody> EventResGuildNoticeSet
    {
        add { AddDelegate<GuildNoticeSetResponseBody>(ClientCommandName.GuildNoticeSet, value); }
        remove { DelDelegate<GuildNoticeSetResponseBody>(ClientCommandName.GuildNoticeSet, value); }
    }

    // 길드임명
    public event Delegate<int, GuildAppointResponseBody> EventResGuildAppoint
    {
        add { AddDelegate<GuildAppointResponseBody>(ClientCommandName.GuildAppoint, value); }
        remove { DelDelegate<GuildAppointResponseBody>(ClientCommandName.GuildAppoint, value); }
    }

    // 길드장위임
    public event Delegate<int, GuildMasterTransferResponseBody> EventResGuildMasterTransfer
    {
        add { AddDelegate<GuildMasterTransferResponseBody>(ClientCommandName.GuildMasterTransfer, value); }
        remove { DelDelegate<GuildMasterTransferResponseBody>(ClientCommandName.GuildMasterTransfer, value); }
    }

    // 길드스킬레벨업
    public event Delegate<int, GuildSkillLevelUpResponseBody> EventResGuildSkillLevelUp
    {
        add { AddDelegate<GuildSkillLevelUpResponseBody>(ClientCommandName.GuildSkillLevelUp, value); }
        remove { DelDelegate<GuildSkillLevelUpResponseBody>(ClientCommandName.GuildSkillLevelUp, value); }
    }

    // 길드일일보상받기
    public event Delegate<int, GuildDailyRewardReceiveResponseBody> EventResGuildDailyRewardReceive
    {
        add { AddDelegate<GuildDailyRewardReceiveResponseBody>(ClientCommandName.GuildDailyRewardReceive, value); }
        remove { DelDelegate<GuildDailyRewardReceiveResponseBody>(ClientCommandName.GuildDailyRewardReceive, value); }
    }

    // 길드소집
    public event Delegate<int, GuildCallResponseBody> EventResGuildCall
    {
        add { AddDelegate<GuildCallResponseBody>(ClientCommandName.GuildCall, value); }
        remove { DelDelegate<GuildCallResponseBody>(ClientCommandName.GuildCall, value); }
    }

    // 길드소집전송
    public event Delegate<int, GuildCallTransmissionResponseBody> EventResGuildCallTransmission
    {
        add { AddDelegate<GuildCallTransmissionResponseBody>(ClientCommandName.GuildCallTransmission, value); }
        remove { DelDelegate<GuildCallTransmissionResponseBody>(ClientCommandName.GuildCallTransmission, value); }
    }

    // 길드소집전송에대한대륙입장
    public event Delegate<int, ContinentEnterForGuildCallTransmissionResponseBody> EventResContinentEnterForGuildCallTransmission
    {
        add { AddDelegate<ContinentEnterForGuildCallTransmissionResponseBody>(ClientCommandName.ContinentEnterForGuildCallTransmission, value); }
        remove { DelDelegate<ContinentEnterForGuildCallTransmissionResponseBody>(ClientCommandName.ContinentEnterForGuildCallTransmission, value); }
    }

    #endregion Guild

    #region GuildDonate

    // 길드기부
    public event Delegate<int, GuildDonateResponseBody> EventResGuildDonate
    {
        add { AddDelegate<GuildDonateResponseBody>(ClientCommandName.GuildDonate, value); }
        remove { DelDelegate<GuildDonateResponseBody>(ClientCommandName.GuildDonate, value); }
    }

    #endregion GuildDonate

    #region GuildBuilding

    // 길드건물레벨업
    public event Delegate<int, GuildBuildingLevelUpResponseBody> EventResGuildBuildingLevelUp
    {
        add { AddDelegate<GuildBuildingLevelUpResponseBody>(ClientCommandName.GuildBuildingLevelUp, value); }
        remove { DelDelegate<GuildBuildingLevelUpResponseBody>(ClientCommandName.GuildBuildingLevelUp, value); }
    }

    #endregion GuildBuilding

    #region GuildTerritory

    // 길드영지입장을위한대륙퇴장
    public event Delegate<int, ContinentExitForGuildTerritoryEnterResponseBody> EventResContinentExitForGuildTerritoryEnter
    {
        add { AddDelegate<ContinentExitForGuildTerritoryEnterResponseBody>(ClientCommandName.ContinentExitForGuildTerritoryEnter, value); }
        remove { DelDelegate<ContinentExitForGuildTerritoryEnterResponseBody>(ClientCommandName.ContinentExitForGuildTerritoryEnter, value); }
    }

    // 길드영지입장
    public event Delegate<int, GuildTerritoryEnterResponseBody> EventResGuildTerritoryEnter
    {
        add { AddDelegate<GuildTerritoryEnterResponseBody>(ClientCommandName.GuildTerritoryEnter, value); }
        remove { DelDelegate<GuildTerritoryEnterResponseBody>(ClientCommandName.GuildTerritoryEnter, value); }
    }

    // 길드영지퇴장
    public event Delegate<int, GuildTerritoryExitResponseBody> EventResGuildTerritoryExit
    {
        add { AddDelegate<GuildTerritoryExitResponseBody>(ClientCommandName.GuildTerritoryExit, value); }
        remove { DelDelegate<GuildTerritoryExitResponseBody>(ClientCommandName.GuildTerritoryExit, value); }
    }

    // 길드영지부활
    public event Delegate<int, GuildTerritoryReviveResponseBody> EventResGuildTerritoryRevive
    {
        add { AddDelegate<GuildTerritoryReviveResponseBody>(ClientCommandName.GuildTerritoryRevive, value); }
        remove { DelDelegate<GuildTerritoryReviveResponseBody>(ClientCommandName.GuildTerritoryRevive, value); }
    }

    // 길드영지부활에대한길드영지입장
    public event Delegate<int, GuildTerritoryEnterForGuildTerritoryRevivalResponseBody> EventResGuildTerritoryEnterForGuildTerritoryRevival
    {
        add { AddDelegate<GuildTerritoryEnterForGuildTerritoryRevivalResponseBody>(ClientCommandName.GuildTerritoryEnterForGuildTerritoryRevival, value); }
        remove { DelDelegate<GuildTerritoryEnterForGuildTerritoryRevivalResponseBody>(ClientCommandName.GuildTerritoryEnterForGuildTerritoryRevival, value); }
    }

    #endregion GuildTerritory

    #region GuildFarmQuest

    // 길드농장퀘스트수락
    public event Delegate<int, GuildFarmQuestAcceptResponseBody> EventResGuildFarmQuestAccept
    {
        add { AddDelegate<GuildFarmQuestAcceptResponseBody>(ClientCommandName.GuildFarmQuestAccept, value); }
        remove { DelDelegate<GuildFarmQuestAcceptResponseBody>(ClientCommandName.GuildFarmQuestAccept, value); }
    }

    // 길드농장퀘스트상호작용시작
    public event Delegate<int, GuildFarmQuestInteractionStartResponseBody> EventResGuildFarmQuestInteractionStart
    {
        add { AddDelegate<GuildFarmQuestInteractionStartResponseBody>(ClientCommandName.GuildFarmQuestInteractionStart, value); }
        remove { DelDelegate<GuildFarmQuestInteractionStartResponseBody>(ClientCommandName.GuildFarmQuestInteractionStart, value); }
    }

    // 길드농장퀘스트완료
    public event Delegate<int, GuildFarmQuestCompleteResponseBody> EventResGuildFarmQuestComplete
    {
        add { AddDelegate<GuildFarmQuestCompleteResponseBody>(ClientCommandName.GuildFarmQuestComplete, value); }
        remove { DelDelegate<GuildFarmQuestCompleteResponseBody>(ClientCommandName.GuildFarmQuestComplete, value); }
    }

    // 길드농장퀘스트포기
    public event Delegate<int, GuildFarmQuestAbandonResponseBody> EventResGuildFarmQuestAbandon
    {
        add { AddDelegate<GuildFarmQuestAbandonResponseBody>(ClientCommandName.GuildFarmQuestAbandon, value); }
        remove { DelDelegate<GuildFarmQuestAbandonResponseBody>(ClientCommandName.GuildFarmQuestAbandon, value); }
    }

    #endregion GuildFarmQuest

    #region GuildFoodWarehouse

    // 길드군량창고정보
    public event Delegate<int, GuildFoodWarehouseInfoResponseBody> EventResGuildFoodWarehouseInfo
    {
        add { AddDelegate<GuildFoodWarehouseInfoResponseBody>(ClientCommandName.GuildFoodWarehouseInfo, value); }
        remove { DelDelegate<GuildFoodWarehouseInfoResponseBody>(ClientCommandName.GuildFoodWarehouseInfo, value); }
    }

    // 길드군량창고납부
    public event Delegate<int, GuildFoodWarehouseStockResponseBody> EventResGuildFoodWarehouseStock
    {
        add { AddDelegate<GuildFoodWarehouseStockResponseBody>(ClientCommandName.GuildFoodWarehouseStock, value); }
        remove { DelDelegate<GuildFoodWarehouseStockResponseBody>(ClientCommandName.GuildFoodWarehouseStock, value); }
    }

    // 길드군량창고징수
    public event Delegate<int, GuildFoodWarehouseCollectResponseBody> EventResGuildFoodWarehouseCollect
    {
        add { AddDelegate<GuildFoodWarehouseCollectResponseBody>(ClientCommandName.GuildFoodWarehouseCollect, value); }
        remove { DelDelegate<GuildFoodWarehouseCollectResponseBody>(ClientCommandName.GuildFoodWarehouseCollect, value); }
    }

    // 길드군량창고보상받기
    public event Delegate<int, GuildFoodWarehouseRewardReceiveResponseBody> EventResGuildFoodWarehouseRewardReceive
    {
        add { AddDelegate<GuildFoodWarehouseRewardReceiveResponseBody>(ClientCommandName.GuildFoodWarehouseRewardReceive, value); }
        remove { DelDelegate<GuildFoodWarehouseRewardReceiveResponseBody>(ClientCommandName.GuildFoodWarehouseRewardReceive, value); }
    }

    #endregion GuildFoodWarehouse

    #region GuildAltar

    // 길드제단기부
    public event Delegate<int, GuildAltarDonateResponseBody> EventResGuildAltarDonate
    {
        add { AddDelegate<GuildAltarDonateResponseBody>(ClientCommandName.GuildAltarDonate, value); }
        remove { DelDelegate<GuildAltarDonateResponseBody>(ClientCommandName.GuildAltarDonate, value); }
    }

    // 길드제단마력주입미션시작
    public event Delegate<int, GuildAltarSpellInjectionMissionStartResponseBody> EventResGuildAltarSpellInjectionMissionStart
    {
        add { AddDelegate<GuildAltarSpellInjectionMissionStartResponseBody>(ClientCommandName.GuildAltarSpellInjectionMissionStart, value); }
        remove { DelDelegate<GuildAltarSpellInjectionMissionStartResponseBody>(ClientCommandName.GuildAltarSpellInjectionMissionStart, value); }
    }

    // 길드제단수비미션시작
    public event Delegate<int, GuildAltarDefenseMissionStartResponseBody> EventResGuildAltarDefenseMissionStart
    {
        add { AddDelegate<GuildAltarDefenseMissionStartResponseBody>(ClientCommandName.GuildAltarDefenseMissionStart, value); }
        remove { DelDelegate<GuildAltarDefenseMissionStartResponseBody>(ClientCommandName.GuildAltarDefenseMissionStart, value); }
    }

    // 길드제단보상받기
    public event Delegate<int, GuildAltarRewardReceiveResponseBody> EventResGuildAltarRewardReceive
    {
        add { AddDelegate<GuildAltarRewardReceiveResponseBody>(ClientCommandName.GuildAltarRewardReceive, value); }
        remove { DelDelegate<GuildAltarRewardReceiveResponseBody>(ClientCommandName.GuildAltarRewardReceive, value); }
    }

    #endregion GuildAltar

    #region GuildMission

    // 길드미션퀘스트수락
    public event Delegate<int, GuildMissionQuestAcceptResponseBody> EventResGuildMissionQuestAccept
    {
        add { AddDelegate<GuildMissionQuestAcceptResponseBody>(ClientCommandName.GuildMissionQuestAccept, value); }
        remove { DelDelegate<GuildMissionQuestAcceptResponseBody>(ClientCommandName.GuildMissionQuestAccept, value); }
    }

    // 길드미션수락
    public event Delegate<int, GuildMissionAcceptResponseBody> EventResGuildMissionAccept
    {
        add { AddDelegate<GuildMissionAcceptResponseBody>(ClientCommandName.GuildMissionAccept, value); }
        remove { DelDelegate<GuildMissionAcceptResponseBody>(ClientCommandName.GuildMissionAccept, value); }
    }

    // 길드미션포기
    public event Delegate<int, GuildMissionAbandonResponseBody> EventResGuildMissionAbandon
    {
        add { AddDelegate<GuildMissionAbandonResponseBody>(ClientCommandName.GuildMissionAbandon, value); }
        remove { DelDelegate<GuildMissionAbandonResponseBody>(ClientCommandName.GuildMissionAbandon, value); }
    }

    // 길드미션대상NPC상호작용
    public event Delegate<int, GuildMissionTargetNpcInteractResponseBody> EventResGuildMissionTargetNpcInteract
    {
        add { AddDelegate<GuildMissionTargetNpcInteractResponseBody>(ClientCommandName.GuildMissionTargetNpcInteract, value); }
        remove { DelDelegate<GuildMissionTargetNpcInteractResponseBody>(ClientCommandName.GuildMissionTargetNpcInteract, value); }
    }

    #endregion GuildMission

    #region GuildSupplySupport

    // 길드물자지원퀘스트수락
    public event Delegate<int, GuildSupplySupportQuestAcceptResponseBody> EventResGuildSupplySupportQuestAccept
    {
        add { AddDelegate<GuildSupplySupportQuestAcceptResponseBody>(ClientCommandName.GuildSupplySupportQuestAccept, value); }
        remove { DelDelegate<GuildSupplySupportQuestAcceptResponseBody>(ClientCommandName.GuildSupplySupportQuestAccept, value); }
    }

    // 길드물자지원퀘스트완료
    public event Delegate<int, GuildSupplySupportQuestCompleteResponseBody> EventResGuildSupplySupportQuestComplete
    {
        add { AddDelegate<GuildSupplySupportQuestCompleteResponseBody>(ClientCommandName.GuildSupplySupportQuestComplete, value); }
        remove { DelDelegate<GuildSupplySupportQuestCompleteResponseBody>(ClientCommandName.GuildSupplySupportQuestComplete, value); }
    }

    #endregion GuildSupplySupport

    #region GuildHunting

    // 길드헌팅퀘스트수락
    public event Delegate<int, GuildHuntingQuestAcceptResponseBody> EventResGuildHuntingQuestAccept
    {
        add { AddDelegate<GuildHuntingQuestAcceptResponseBody>(ClientCommandName.GuildHuntingQuestAccept, value); }
        remove { DelDelegate<GuildHuntingQuestAcceptResponseBody>(ClientCommandName.GuildHuntingQuestAccept, value); }
    }

    // 길드헌팅퀘스트포기
    public event Delegate<int, GuildHuntingQuestAbandonResponseBody> EventResGuildHuntingQuestAbandon
    {
        add { AddDelegate<GuildHuntingQuestAbandonResponseBody>(ClientCommandName.GuildHuntingQuestAbandon, value); }
        remove { DelDelegate<GuildHuntingQuestAbandonResponseBody>(ClientCommandName.GuildHuntingQuestAbandon, value); }
    }

    // 길드헌팅퀘스트완료
    public event Delegate<int, GuildHuntingQuestCompleteResponseBody> EventResGuildHuntingQuestComplete
    {
        add { AddDelegate<GuildHuntingQuestCompleteResponseBody>(ClientCommandName.GuildHuntingQuestComplete, value); }
        remove { DelDelegate<GuildHuntingQuestCompleteResponseBody>(ClientCommandName.GuildHuntingQuestComplete, value); }
    }

    // 길드헌팅기부
    public event Delegate<int, GuildHuntingDonateResponseBody> EventResGuildHuntingDonate
    {
        add { AddDelegate<GuildHuntingDonateResponseBody>(ClientCommandName.GuildHuntingDonate, value); }
        remove { DelDelegate<GuildHuntingDonateResponseBody>(ClientCommandName.GuildHuntingDonate, value); }
    }

    // 길드헌팅기부보상받기
    public event Delegate<int, GuildHuntingDonationRewardReceiveResponseBody> EventResGuildHuntingDonationRewardReceive
    {
        add { AddDelegate<GuildHuntingDonationRewardReceiveResponseBody>(ClientCommandName.GuildHuntingDonationRewardReceive, value); }
        remove { DelDelegate<GuildHuntingDonationRewardReceiveResponseBody>(ClientCommandName.GuildHuntingDonationRewardReceive, value); }
    }

    #endregion GuildHunting

    #region GuildDaily

    // 길드일일목표알림
    public event Delegate<int, GuildDailyObjectiveNoticeResponseBody> EventResGuildDailyObjectiveNotice
    {
        add { AddDelegate<GuildDailyObjectiveNoticeResponseBody>(ClientCommandName.GuildDailyObjectiveNotice, value); }
        remove { DelDelegate<GuildDailyObjectiveNoticeResponseBody>(ClientCommandName.GuildDailyObjectiveNotice, value); }
    }

    // 길드일일목표완료멤버목록
    public event Delegate<int, GuildDailyObjectiveCompletionMemberListResponseBody> EventResGuildDailyObjectiveCompletionMemberList
    {
        add { AddDelegate<GuildDailyObjectiveCompletionMemberListResponseBody>(ClientCommandName.GuildDailyObjectiveCompletionMemberList, value); }
        remove { DelDelegate<GuildDailyObjectiveCompletionMemberListResponseBody>(ClientCommandName.GuildDailyObjectiveCompletionMemberList, value); }
    }

    // 길드일일목표보상받기
    public event Delegate<int, GuildDailyObjectiveRewardReceiveResponseBody> EventResGuildDailyObjectiveRewardReceive
    {
        add { AddDelegate<GuildDailyObjectiveRewardReceiveResponseBody>(ClientCommandName.GuildDailyObjectiveRewardReceive, value); }
        remove { DelDelegate<GuildDailyObjectiveRewardReceiveResponseBody>(ClientCommandName.GuildDailyObjectiveRewardReceive, value); }
    }

    #endregion GuildDaily

    #region GuildWeekly

    // 길드주간목표설정
    public event Delegate<int, GuildWeeklyObjectiveSetResponseBody> EventResGuildWeeklyObjectiveSet
    {
        add { AddDelegate<GuildWeeklyObjectiveSetResponseBody>(ClientCommandName.GuildWeeklyObjectiveSet, value); }
        remove { DelDelegate<GuildWeeklyObjectiveSetResponseBody>(ClientCommandName.GuildWeeklyObjectiveSet, value); }
    }

    // 길드주간목표보상받기
    public event Delegate<int, GuildWeeklyObjectiveRewardReceiveResponseBody> EventResGuildWeeklyObjectiveRewardReceive
    {
        add { AddDelegate<GuildWeeklyObjectiveRewardReceiveResponseBody>(ClientCommandName.GuildWeeklyObjectiveRewardReceive, value); }
        remove { DelDelegate<GuildWeeklyObjectiveRewardReceiveResponseBody>(ClientCommandName.GuildWeeklyObjectiveRewardReceive, value); }
    }

    #endregion GuildWeekly

    #region NationWar

    // 국가전선포
    public event Delegate<int, NationWarDeclarationResponseBody> EventResNationWarDeclaration
    {
        add { AddDelegate<NationWarDeclarationResponseBody>(ClientCommandName.NationWarDeclaration, value); }
        remove { DelDelegate<NationWarDeclarationResponseBody>(ClientCommandName.NationWarDeclaration, value); }
    }

    // 국가전히스토리
    public event Delegate<int, NationWarHistoryResponseBody> EventResNationWarHistory
    {
        add { AddDelegate<NationWarHistoryResponseBody>(ClientCommandName.NationWarHistory, value); }
        remove { DelDelegate<NationWarHistoryResponseBody>(ClientCommandName.NationWarHistory, value); }
    }

    // 국가전참여
    public event Delegate<int, NationWarJoinResponseBody> EventResNationWarJoin
    {
        add { AddDelegate<NationWarJoinResponseBody>(ClientCommandName.NationWarJoin, value); }
        remove { DelDelegate<NationWarJoinResponseBody>(ClientCommandName.NationWarJoin, value); }
    }

    // 국가전참여에대한대륙입장
    public event Delegate<int, ContinentEnterForNationWarJoinResponseBody> EventResContinentEnterForNationWarJoin
    {
        add { AddDelegate<ContinentEnterForNationWarJoinResponseBody>(ClientCommandName.ContinentEnterForNationWarJoin, value); }
        remove { DelDelegate<ContinentEnterForNationWarJoinResponseBody>(ClientCommandName.ContinentEnterForNationWarJoin, value); }
    }

    // 국가전정보
    public event Delegate<int, NationWarInfoResponseBody> EventResNationWarInfo
    {
        add { AddDelegate<NationWarInfoResponseBody>(ClientCommandName.NationWarInfo, value); }
        remove { DelDelegate<NationWarInfoResponseBody>(ClientCommandName.NationWarInfo, value); }
    }

    // 국가전전송
    public event Delegate<int, NationWarTransmissionResponseBody> EventResNationWarTransmission
    {
        add { AddDelegate<NationWarTransmissionResponseBody>(ClientCommandName.NationWarTransmission, value); }
        remove { DelDelegate<NationWarTransmissionResponseBody>(ClientCommandName.NationWarTransmission, value); }
    }

    // 국가전전송에대한대륙입장
    public event Delegate<int, ContinentEnterForNationWarTransmissionResponseBody> EventResContinentEnterForNationWarTransmission
    {
        add { AddDelegate<ContinentEnterForNationWarTransmissionResponseBody>(ClientCommandName.ContinentEnterForNationWarTransmission, value); }
        remove { DelDelegate<ContinentEnterForNationWarTransmissionResponseBody>(ClientCommandName.ContinentEnterForNationWarTransmission, value); }
    }

    // 국가전NPC전송
    public event Delegate<int, NationWarNpcTransmissionResponseBody> EventResNationWarNpcTransmission
    {
        add { AddDelegate<NationWarNpcTransmissionResponseBody>(ClientCommandName.NationWarNpcTransmission, value); }
        remove { DelDelegate<NationWarNpcTransmissionResponseBody>(ClientCommandName.NationWarNpcTransmission, value); }
    }

    // 국가전NPC전송에대한대륙입장
    public event Delegate<int, ContinentEnterForNationWarNpcTransmissionResponseBody> EventResContinentEnterForNationWarNpcTransmission
    {
        add { AddDelegate<ContinentEnterForNationWarNpcTransmissionResponseBody>(ClientCommandName.ContinentEnterForNationWarNpcTransmission, value); }
        remove { DelDelegate<ContinentEnterForNationWarNpcTransmissionResponseBody>(ClientCommandName.ContinentEnterForNationWarNpcTransmission, value); }
    }

    // 국가전부활
    public event Delegate<int, NationWarReviveResponseBody> EventResNationWarRevive
    {
        add { AddDelegate<NationWarReviveResponseBody>(ClientCommandName.NationWarRevive, value); }
        remove { DelDelegate<NationWarReviveResponseBody>(ClientCommandName.NationWarRevive, value); }
    }

    // 국가전부활에대한대륙입장
    public event Delegate<int, ContinentEnterForNationWarReviveResponseBody> EventResContinentEnterForNationWarRevive
    {
        add { AddDelegate<ContinentEnterForNationWarReviveResponseBody>(ClientCommandName.ContinentEnterForNationWarRevive, value); }
        remove { DelDelegate<ContinentEnterForNationWarReviveResponseBody>(ClientCommandName.ContinentEnterForNationWarRevive, value); }
    }

    // 국가전소집
    public event Delegate<int, NationWarCallResponseBody> EventResNationWarCall
    {
        add { AddDelegate<NationWarCallResponseBody>(ClientCommandName.NationWarCall, value); }
        remove { DelDelegate<NationWarCallResponseBody>(ClientCommandName.NationWarCall, value); }
    }

    // 국가전소집전송
    public event Delegate<int, NationWarCallTransmissionResponseBody> EventResNationWarCallTransmission
    {
        add { AddDelegate<NationWarCallTransmissionResponseBody>(ClientCommandName.NationWarCallTransmission, value); }
        remove { DelDelegate<NationWarCallTransmissionResponseBody>(ClientCommandName.NationWarCallTransmission, value); }
    }

    // 국가전소집전송에대한대륙입장
    public event Delegate<int, ContinentEnterForNationWarCallTransmissionResponseBody> EventResContinentEnterForNationWarCallTransmission
    {
        add { AddDelegate<ContinentEnterForNationWarCallTransmissionResponseBody>(ClientCommandName.ContinentEnterForNationWarCallTransmission, value); }
        remove { DelDelegate<ContinentEnterForNationWarCallTransmissionResponseBody>(ClientCommandName.ContinentEnterForNationWarCallTransmission, value); }
    }

    // 국가전집중공격
    public event Delegate<int, NationWarConvergingAttackResponseBody> EventResNationWarConvergingAttack
    {
        add { AddDelegate<NationWarConvergingAttackResponseBody>(ClientCommandName.NationWarConvergingAttack, value); }
        remove { DelDelegate<NationWarConvergingAttackResponseBody>(ClientCommandName.NationWarConvergingAttack, value); }
    }

    // 국가전결과
    public event Delegate<int, NationWarResultResponseBody> EventResNationWarResult
    {
        add { AddDelegate<NationWarResultResponseBody>(ClientCommandName.NationWarResult, value); }
        remove { DelDelegate<NationWarResultResponseBody>(ClientCommandName.NationWarResult, value); }
    }

    #endregion NationWar

    #region IllustratedBook

    // 도감사용
    public event Delegate<int, IllustratedBookUseResponseBody> EventResIllustratedBookUse
    {
        add { AddDelegate<IllustratedBookUseResponseBody>(ClientCommandName.IllustratedBookUse, value); }
        remove { DelDelegate<IllustratedBookUseResponseBody>(ClientCommandName.IllustratedBookUse, value); }
    }

    // 도감탐험단계획득
    public event Delegate<int, IllustratedBookExplorationStepAcquireResponseBody> EventResIllustratedBookExplorationStepAcquire
    {
        add { AddDelegate<IllustratedBookExplorationStepAcquireResponseBody>(ClientCommandName.IllustratedBookExplorationStepAcquire, value); }
        remove { DelDelegate<IllustratedBookExplorationStepAcquireResponseBody>(ClientCommandName.IllustratedBookExplorationStepAcquire, value); }
    }

    // 도감탐험단계보상받기
    public event Delegate<int, IllustratedBookExplorationStepRewardReceiveResponseBody> EventResIllustratedBookExplorationStepRewardReceive
    {
        add { AddDelegate<IllustratedBookExplorationStepRewardReceiveResponseBody>(ClientCommandName.IllustratedBookExplorationStepRewardReceive, value); }
        remove { DelDelegate<IllustratedBookExplorationStepRewardReceiveResponseBody>(ClientCommandName.IllustratedBookExplorationStepRewardReceive, value); }
    }

    #endregion IllustratedBook

    #region Accomplishment

    // 업적보상받기
    public event Delegate<int, AccomplishmentRewardReceiveResponseBody> EventResAccomplishmentRewardReceive
    {
        add { AddDelegate<AccomplishmentRewardReceiveResponseBody>(ClientCommandName.AccomplishmentRewardReceive, value); }
        remove { DelDelegate<AccomplishmentRewardReceiveResponseBody>(ClientCommandName.AccomplishmentRewardReceive, value); }
    }

    // 업적보상전체받기
    public event Delegate<int, AccomplishmentRewardReceiveAllResponseBody> EventResAccomplishmentRewardReceiveAll
    {
        add { AddDelegate<AccomplishmentRewardReceiveAllResponseBody>(ClientCommandName.AccomplishmentRewardReceiveAll, value); }
        remove { DelDelegate<AccomplishmentRewardReceiveAllResponseBody>(ClientCommandName.AccomplishmentRewardReceiveAll, value); }
    }

    // 업적레벨업


    #endregion Accomplishment

    #region Title

    // 칭호아이템사용
    public event Delegate<int, TitleItemUseResponseBody> EventResTitleItemUse
    {
        add { AddDelegate<TitleItemUseResponseBody>(ClientCommandName.TitleItemUse, value); }
        remove { DelDelegate<TitleItemUseResponseBody>(ClientCommandName.TitleItemUse, value); }
    }

    // 활성칭호설정
    public event Delegate<int, ActivationTitleSetResponseBody> EventResActivationTitleSet
    {
        add { AddDelegate<ActivationTitleSetResponseBody>(ClientCommandName.ActivationTitleSet, value); }
        remove { DelDelegate<ActivationTitleSetResponseBody>(ClientCommandName.ActivationTitleSet, value); }
    }

    // 표시칭호설정
    public event Delegate<int, DisplayTitleSetResponseBody> EventResDisplayTitleSet
    {
        add { AddDelegate<DisplayTitleSetResponseBody>(ClientCommandName.DisplayTitleSet, value); }
        remove { DelDelegate<DisplayTitleSetResponseBody>(ClientCommandName.DisplayTitleSet, value); }
    }

    #endregion Title

    #region SceneryQuest

    // 풍광퀘스트시작
    public event Delegate<int, SceneryQuestStartResponseBody> EventResSceneryQuestStart
    {
        add { AddDelegate<SceneryQuestStartResponseBody>(ClientCommandName.SceneryQuestStart, value); }
        remove { DelDelegate<SceneryQuestStartResponseBody>(ClientCommandName.SceneryQuestStart, value); }
    }

    #endregion SceneryQuest

    #region CreatureCard

    // 크리처카드컬렉션활성
    public event Delegate<int, CreatureCardCollectionActivateResponseBody> EventResCreatureCardCollectionActivate
    {
        add { AddDelegate<CreatureCardCollectionActivateResponseBody>(ClientCommandName.CreatureCardCollectionActivate, value); }
        remove { DelDelegate<CreatureCardCollectionActivateResponseBody>(ClientCommandName.CreatureCardCollectionActivate, value); }
    }

    // 크리처카드합성
    public event Delegate<int, CreatureCardComposeResponseBody> EventResCreatureCardCompose
    {
        add { AddDelegate<CreatureCardComposeResponseBody>(ClientCommandName.CreatureCardCompose, value); }
        remove { DelDelegate<CreatureCardComposeResponseBody>(ClientCommandName.CreatureCardCompose, value); }
    }

    // 크리처카드분해
    public event Delegate<int, CreatureCardDisassembleResponseBody> EventResCreatureCardDisassemble
    {
        add { AddDelegate<CreatureCardDisassembleResponseBody>(ClientCommandName.CreatureCardDisassemble, value); }
        remove { DelDelegate<CreatureCardDisassembleResponseBody>(ClientCommandName.CreatureCardDisassemble, value); }
    }

    // 크리처카드전체분해
    public event Delegate<int, CreatureCardDisassembleAllResponseBody> EventResCreatureCardDisassembleAll
    {
        add { AddDelegate<CreatureCardDisassembleAllResponseBody>(ClientCommandName.CreatureCardDisassembleAll, value); }
        remove { DelDelegate<CreatureCardDisassembleAllResponseBody>(ClientCommandName.CreatureCardDisassembleAll, value); }
    }

    // 크리처카드상점유료갱신
    public event Delegate<int, CreatureCardShopPaidRefreshResponseBody> EventResCreatureCardShopPaidRefresh
    {
        add { AddDelegate<CreatureCardShopPaidRefreshResponseBody>(ClientCommandName.CreatureCardShopPaidRefresh, value); }
        remove { DelDelegate<CreatureCardShopPaidRefreshResponseBody>(ClientCommandName.CreatureCardShopPaidRefresh, value); }
    }

    // 크리처카드상점고정상품구매
    public event Delegate<int, CreatureCardShopFixedProductBuyResponseBody> EventResCreatureCardShopFixedProductBuy
    {
        add { AddDelegate<CreatureCardShopFixedProductBuyResponseBody>(ClientCommandName.CreatureCardShopFixedProductBuy, value); }
        remove { DelDelegate<CreatureCardShopFixedProductBuyResponseBody>(ClientCommandName.CreatureCardShopFixedProductBuy, value); }
    }

    // 크리처카드상점랜덤상품구매
    public event Delegate<int, CreatureCardShopRandomProductBuyResponseBody> EventResCreatureCardShopRandomProductBuy
    {
        add { AddDelegate<CreatureCardShopRandomProductBuyResponseBody>(ClientCommandName.CreatureCardShopRandomProductBuy, value); }
        remove { DelDelegate<CreatureCardShopRandomProductBuyResponseBody>(ClientCommandName.CreatureCardShopRandomProductBuy, value); }
    }

    #endregion CreatureCard

    #region EliteDungeon

    // 정예던전입장을위한대륙퇴장
    public event Delegate<int, ContinentExitForEliteDungeonEnterResponseBody> EventResContinentExitForEliteDungeonEnter
    {
        add { AddDelegate<ContinentExitForEliteDungeonEnterResponseBody>(ClientCommandName.ContinentExitForEliteDungeonEnter, value); }
        remove { DelDelegate<ContinentExitForEliteDungeonEnterResponseBody>(ClientCommandName.ContinentExitForEliteDungeonEnter, value); }
    }

    // 정예던전입장
    public event Delegate<int, EliteDungeonEnterResponseBody> EventResEliteDungeonEnter
    {
        add { AddDelegate<EliteDungeonEnterResponseBody>(ClientCommandName.EliteDungeonEnter, value); }
        remove { DelDelegate<EliteDungeonEnterResponseBody>(ClientCommandName.EliteDungeonEnter, value); }
    }

    // 정예던전퇴장
    public event Delegate<int, EliteDungeonExitResponseBody> EventResEliteDungeonExit
    {
        add { AddDelegate<EliteDungeonExitResponseBody>(ClientCommandName.EliteDungeonExit, value); }
        remove { DelDelegate<EliteDungeonExitResponseBody>(ClientCommandName.EliteDungeonExit, value); }
    }

    // 정예던전포기
    public event Delegate<int, EliteDungeonAbandonResponseBody> EventResEliteDungeonAbandon
    {
        add { AddDelegate<EliteDungeonAbandonResponseBody>(ClientCommandName.EliteDungeonAbandon, value); }
        remove { DelDelegate<EliteDungeonAbandonResponseBody>(ClientCommandName.EliteDungeonAbandon, value); }
    }

    // 정예던전부활
    public event Delegate<int, EliteDungeonReviveResponseBody> EventResEliteDungeonRevive
    {
        add { AddDelegate<EliteDungeonReviveResponseBody>(ClientCommandName.EliteDungeonRevive, value); }
        remove { DelDelegate<EliteDungeonReviveResponseBody>(ClientCommandName.EliteDungeonRevive, value); }
    }

    #endregion EliteDungeon

    #region Setting

    // 전투설정
    public event Delegate<int, BattleSettingSetResponseBody> EventResBattleSettingSet
    {
        add { AddDelegate<BattleSettingSetResponseBody>(ClientCommandName.BattleSettingSet, value); }
        remove { DelDelegate<BattleSettingSetResponseBody>(ClientCommandName.BattleSettingSet, value); }
    }

    #endregion Setting

    #region ProofOfValor

    // 용맹의증명입장을위한대륙퇴장
    public event Delegate<int, ContinentExitForProofOfValorEnterResponseBody> EventResContinentExitForProofOfValorEnter
    {
        add { AddDelegate<ContinentExitForProofOfValorEnterResponseBody>(ClientCommandName.ContinentExitForProofOfValorEnter, value); }
        remove { DelDelegate<ContinentExitForProofOfValorEnterResponseBody>(ClientCommandName.ContinentExitForProofOfValorEnter, value); }
    }

    // 용맹의증명입장
    public event Delegate<int, ProofOfValorEnterResponseBody> EventResProofOfValorEnter
    {
        add { AddDelegate<ProofOfValorEnterResponseBody>(ClientCommandName.ProofOfValorEnter, value); }
        remove { DelDelegate<ProofOfValorEnterResponseBody>(ClientCommandName.ProofOfValorEnter, value); }
    }

    // 용맹의증명퇴장
    public event Delegate<int, ProofOfValorExitResponseBody> EventResProofOfValorExit
    {
        add { AddDelegate<ProofOfValorExitResponseBody>(ClientCommandName.ProofOfValorExit, value); }
        remove { DelDelegate<ProofOfValorExitResponseBody>(ClientCommandName.ProofOfValorExit, value); }
    }

    // 용맹의증명포기
    public event Delegate<int, ProofOfValorAbandonResponseBody> EventResProofOfValorAbandon
    {
        add { AddDelegate<ProofOfValorAbandonResponseBody>(ClientCommandName.ProofOfValorAbandon, value); }
        remove { DelDelegate<ProofOfValorAbandonResponseBody>(ClientCommandName.ProofOfValorAbandon, value); }
    }

    // 용맹의증명소탕
    public event Delegate<int, ProofOfValorSweepResponseBody> EventResProofOfValorSweep
    {
        add { AddDelegate<ProofOfValorSweepResponseBody>(ClientCommandName.ProofOfValorSweep, value); }
        remove { DelDelegate<ProofOfValorSweepResponseBody>(ClientCommandName.ProofOfValorSweep, value); }
    }

    // 용맹의증명갱신
    public event Delegate<int, ProofOfValorRefreshResponseBody> EventResProofOfValorRefresh
    {
        add { AddDelegate<ProofOfValorRefreshResponseBody>(ClientCommandName.ProofOfValorRefresh, value); }
        remove { DelDelegate<ProofOfValorRefreshResponseBody>(ClientCommandName.ProofOfValorRefresh, value); }
    }

    // 용맹의증명버프상자획득
    public event Delegate<int, ProofOfValorBuffBoxAcquireResponseBody> EventResProofOfValorBuffBoxAcquire
    {
        add { AddDelegate<ProofOfValorBuffBoxAcquireResponseBody>(ClientCommandName.ProofOfValorBuffBoxAcquire, value); }
        remove { DelDelegate<ProofOfValorBuffBoxAcquireResponseBody>(ClientCommandName.ProofOfValorBuffBoxAcquire, value); }
    }

    #endregion ProofOfValor

    #region Tutorial

    // 오늘의미션튜토리얼시작
    public event Delegate<int, TodayMissionTutorialStartResponseBody> EventResTodayMissionTutorialStart
    {
        add { AddDelegate<TodayMissionTutorialStartResponseBody>(ClientCommandName.TodayMissionTutorialStart, value); }
        remove { DelDelegate<TodayMissionTutorialStartResponseBody>(ClientCommandName.TodayMissionTutorialStart, value); }
    }

    #endregion Tutrial

    #region GroggyMonster

    // 그로기몬스터아이템훔치기시작
    public event Delegate<int, GroggyMonsterItemStealStartResponseBody> EventResGroggyMonsterItemStealStart
    {
        add { AddDelegate<GroggyMonsterItemStealStartResponseBody>(ClientCommandName.GroggyMonsterItemStealStart, value); }
        remove { DelDelegate<GroggyMonsterItemStealStartResponseBody>(ClientCommandName.GroggyMonsterItemStealStart, value); }
    }

    #endregion GroggyMonster

    #region NpcShop
    
    // NPC상점상품구입
    public event Delegate<int, NpcShopProductBuyResponseBody> EventResNpcShopProductBuy
    {
        add { AddDelegate<NpcShopProductBuyResponseBody>(ClientCommandName.NpcShopProductBuy, value); }
        remove { DelDelegate<NpcShopProductBuyResponseBody>(ClientCommandName.NpcShopProductBuy, value); }
    }

    #endregion NpcShop

    #region DailyQuest

    // 일일퀘스트수락
    public event Delegate<int, DailyQuestAcceptResponseBody> EventResDailyQuestAccept
    {
        add { AddDelegate<DailyQuestAcceptResponseBody>(ClientCommandName.DailyQuestAccept, value); }
        remove { DelDelegate<DailyQuestAcceptResponseBody>(ClientCommandName.DailyQuestAccept, value); }
    }

    // 일일퀘스트완료
    public event Delegate<int, DailyQuestCompleteResponseBody> EventResDailyQuestComplete
    {
        add { AddDelegate<DailyQuestCompleteResponseBody>(ClientCommandName.DailyQuestComplete, value); }
        remove { DelDelegate<DailyQuestCompleteResponseBody>(ClientCommandName.DailyQuestComplete, value); }
    }

    // 일일퀘스트갱신
    public event Delegate<int, DailyQuestRefreshResponseBody> EventResDailyQuestRefresh
    {
        add { AddDelegate<DailyQuestRefreshResponseBody>(ClientCommandName.DailyQuestRefresh, value); }
        remove { DelDelegate<DailyQuestRefreshResponseBody>(ClientCommandName.DailyQuestRefresh, value); }
    }

    // 일일퀘스트미션즉시완료
    public event Delegate<int, DailyQuestMissionImmediatlyCompleteResponseBody> EventResDailyQuestMissionImmediatlyComplete
    {
        add { AddDelegate<DailyQuestMissionImmediatlyCompleteResponseBody>(ClientCommandName.DailyQuestMissionImmediatlyComplete, value); }
        remove { DelDelegate<DailyQuestMissionImmediatlyCompleteResponseBody>(ClientCommandName.DailyQuestMissionImmediatlyComplete, value); }
    }

    // 일일퀘스트포기
    public event Delegate<int, DailyQuestAbandonResponseBody> EventResDailyQuestAbandon
    {
        add { AddDelegate<DailyQuestAbandonResponseBody>(ClientCommandName.DailyQuestAbandon, value); }
        remove { DelDelegate<DailyQuestAbandonResponseBody>(ClientCommandName.DailyQuestAbandon, value); }
    }

    #endregion DailyQuest

    #region WeeklyQuest

    // 주간퀘스트라운드수락
    public event Delegate<int, WeeklyQuestRoundAcceptResponseBody> EventResWeeklyQuestRoundAccept
    {
        add { AddDelegate<WeeklyQuestRoundAcceptResponseBody>(ClientCommandName.WeeklyQuestRoundAccept, value); }
        remove { DelDelegate<WeeklyQuestRoundAcceptResponseBody>(ClientCommandName.WeeklyQuestRoundAccept, value); }
    }

    // 주간퀘스트라운드갱신
    public event Delegate<int, WeeklyQuestRoundRefreshResponseBody> EventResWeeklyQuestRoundRefresh
    {
        add { AddDelegate<WeeklyQuestRoundRefreshResponseBody>(ClientCommandName.WeeklyQuestRoundRefresh, value); }
        remove { DelDelegate<WeeklyQuestRoundRefreshResponseBody>(ClientCommandName.WeeklyQuestRoundRefresh, value); }
    }

    // 주간퀘스트라운드즉시완료
    public event Delegate<int, WeeklyQuestRoundImmediatlyCompleteResponseBody> EventResWeeklyQuestRoundImmediatlyComplete
    {
        add { AddDelegate<WeeklyQuestRoundImmediatlyCompleteResponseBody>(ClientCommandName.WeeklyQuestRoundImmediatlyComplete, value); }
        remove { DelDelegate<WeeklyQuestRoundImmediatlyCompleteResponseBody>(ClientCommandName.WeeklyQuestRoundImmediatlyComplete, value); }
    }

    // 주간퀘스트10라운드즉시완료
    public event Delegate<int, WeeklyQuestTenRoundImmediatlyCompleteResponseBody> EventResWeeklyQuestTenRoundImmediatlyComplete
    {
        add { AddDelegate<WeeklyQuestTenRoundImmediatlyCompleteResponseBody>(ClientCommandName.WeeklyQuestTenRoundImmediatlyComplete, value); }
        remove { DelDelegate<WeeklyQuestTenRoundImmediatlyCompleteResponseBody>(ClientCommandName.WeeklyQuestTenRoundImmediatlyComplete, value); }
    }

    // 주간퀘스트라운드이동미션완료
    public event Delegate<int, WeeklyQuestRoundMoveMissionCompleteResponseBody> EventResWeeklyQuestRoundMoveMissionComplete
    {
        add { AddDelegate<WeeklyQuestRoundMoveMissionCompleteResponseBody>(ClientCommandName.WeeklyQuestRoundMoveMissionComplete, value); }
        remove { DelDelegate<WeeklyQuestRoundMoveMissionCompleteResponseBody>(ClientCommandName.WeeklyQuestRoundMoveMissionComplete, value); }
    }

    #endregion WeeklyQuest

    #region WisdomTemple

    // 지혜의신전입장을위한대륙퇴장
    public event Delegate<int, ContinentExitForWisdomTempleEnterResponseBody> EventResContinentExitForWisdomTempleEnter
    {
        add { AddDelegate<ContinentExitForWisdomTempleEnterResponseBody>(ClientCommandName.ContinentExitForWisdomTempleEnter, value); }
        remove { DelDelegate<ContinentExitForWisdomTempleEnterResponseBody>(ClientCommandName.ContinentExitForWisdomTempleEnter, value); }
    }

    // 지혜의신전입장
    public event Delegate<int, WisdomTempleEnterResponseBody> EventResWisdomTempleEnter
    {
        add { AddDelegate<WisdomTempleEnterResponseBody>(ClientCommandName.WisdomTempleEnter, value); }
        remove { DelDelegate<WisdomTempleEnterResponseBody>(ClientCommandName.WisdomTempleEnter, value); }
    }

    // 지혜의신전퇴장
    public event Delegate<int, WisdomTempleExitResponseBody> EventResWisdomTempleExit
    {
        add { AddDelegate<WisdomTempleExitResponseBody>(ClientCommandName.WisdomTempleExit, value); }
        remove { DelDelegate<WisdomTempleExitResponseBody>(ClientCommandName.WisdomTempleExit, value); }
    }

    // 지혜의신전포기
    public event Delegate<int, WisdomTempleAbandonResponseBody> EventResWisdomTempleAbandon
    {
        add { AddDelegate<WisdomTempleAbandonResponseBody>(ClientCommandName.WisdomTempleAbandon, value); }
        remove { DelDelegate<WisdomTempleAbandonResponseBody>(ClientCommandName.WisdomTempleAbandon, value); }
    }

    // 지혜의신전색맞추기오브젝트상호작용시작
    public event Delegate<int, WisdomTempleColorMatchingObjectInteractionStartResponseBody> EventResWisdomTempleColorMatchingObjectInteractionStart
    {
        add { AddDelegate<WisdomTempleColorMatchingObjectInteractionStartResponseBody>(ClientCommandName.WisdomTempleColorMatchingObjectInteractionStart, value); }
        remove { DelDelegate<WisdomTempleColorMatchingObjectInteractionStartResponseBody>(ClientCommandName.WisdomTempleColorMatchingObjectInteractionStart, value); }
    }

    // 지혜의신전색맞추기오브젝트검사
    public event Delegate<int, WisdomTempleColorMatchingObjectCheckResponseBody> EventResWisdomTempleColorMatchingObjectCheck
    {
        add { AddDelegate<WisdomTempleColorMatchingObjectCheckResponseBody>(ClientCommandName.WisdomTempleColorMatchingObjectCheck, value); }
        remove { DelDelegate<WisdomTempleColorMatchingObjectCheckResponseBody>(ClientCommandName.WisdomTempleColorMatchingObjectCheck, value); }
    }

    // 지혜의신전퍼즐보상오브젝트상호작용시작
    public event Delegate<int, WisdomTemplePuzzleRewardObjectInteractionStartResponseBody> EventResWisdomTemplePuzzleRewardObjectInteractionStart
    {
        add { AddDelegate<WisdomTemplePuzzleRewardObjectInteractionStartResponseBody>(ClientCommandName.WisdomTemplePuzzleRewardObjectInteractionStart, value); }
        remove { DelDelegate<WisdomTemplePuzzleRewardObjectInteractionStartResponseBody>(ClientCommandName.WisdomTemplePuzzleRewardObjectInteractionStart, value); }
    }

    // 지혜의신전소탕
    public event Delegate<int, WisdomTempleSweepResponseBody> EventResWisdomTempleSweep
    {
        add { AddDelegate<WisdomTempleSweepResponseBody>(ClientCommandName.WisdomTempleSweep, value); }
        remove { DelDelegate<WisdomTempleSweepResponseBody>(ClientCommandName.WisdomTempleSweep, value); }
    }

    #endregion WisdomTemple

    #region Open7Day

    // 오픈7일이벤트미션보상받기
    public event Delegate<int, Open7DayEventMissionRewardReceiveResponseBody> EventResOpen7DayEventMissionRewardReceive
    {
        add { AddDelegate<Open7DayEventMissionRewardReceiveResponseBody>(ClientCommandName.Open7DayEventMissionRewardReceive, value); }
        remove { DelDelegate<Open7DayEventMissionRewardReceiveResponseBody>(ClientCommandName.Open7DayEventMissionRewardReceive, value); }
    }

    // 오픈7일이벤트상품구매
    public event Delegate<int, Open7DayEventProductBuyResponseBody> EventResOpen7DayEventProductBuy
    {
        add { AddDelegate<Open7DayEventProductBuyResponseBody>(ClientCommandName.Open7DayEventProductBuy, value); }
        remove { DelDelegate<Open7DayEventProductBuyResponseBody>(ClientCommandName.Open7DayEventProductBuy, value); }
    }

    // 오픈7일이벤트보상받기
    public event Delegate<int, Open7DayEventRewardReceiveResponseBody> EventResOpen7DayEventRewardReceive
    {
        add { AddDelegate<Open7DayEventRewardReceiveResponseBody>(ClientCommandName.Open7DayEventRewardReceive, value); }
        remove { DelDelegate<Open7DayEventRewardReceiveResponseBody>(ClientCommandName.Open7DayEventRewardReceive, value); }
    }

    #endregion Open7Day

    #region Retrieve

    // 골드회수
    public event Delegate<int, RetrieveGoldResponseBody> EventResRetrieveGold
    {
        add { AddDelegate<RetrieveGoldResponseBody>(ClientCommandName.RetrieveGold, value); }
        remove { DelDelegate<RetrieveGoldResponseBody>(ClientCommandName.RetrieveGold, value); }
    }

    // 골드모두회수
    public event Delegate<int, RetrieveGoldAllResponseBody> EventResRetrieveGoldAll
    {
        add { AddDelegate<RetrieveGoldAllResponseBody>(ClientCommandName.RetrieveGoldAll, value); }
        remove { DelDelegate<RetrieveGoldAllResponseBody>(ClientCommandName.RetrieveGoldAll, value); }
    }

    // 다이아회수
    public event Delegate<int, RetrieveDiaResponseBody> EventResRetrieveDia
    {
        add { AddDelegate<RetrieveDiaResponseBody>(ClientCommandName.RetrieveDia, value); }
        remove { DelDelegate<RetrieveDiaResponseBody>(ClientCommandName.RetrieveDia, value); }
    }

    // 다이아모두회수
    public event Delegate<int, RetrieveDiaAllResponseBody> EventResRetrieveDiaAll
    {
        add { AddDelegate<RetrieveDiaAllResponseBody>(ClientCommandName.RetrieveDiaAll, value); }
        remove { DelDelegate<RetrieveDiaAllResponseBody>(ClientCommandName.RetrieveDiaAll, value); }
    }

    #endregion Retrieve

    #region RuinsReclaim

    // 유적탈환매칭시작
    public event Delegate<int, RuinsReclaimMatchingStartResponseBody> EventResRuinsReclaimMatchingStart
    {
        add { AddDelegate<RuinsReclaimMatchingStartResponseBody>(ClientCommandName.RuinsReclaimMatchingStart, value); }
        remove { DelDelegate<RuinsReclaimMatchingStartResponseBody>(ClientCommandName.RuinsReclaimMatchingStart, value); }
    }

    // 유적탈환매칭취소
    public event Delegate<int, RuinsReclaimMatchingCancelResponseBody> EventResRuinsReclaimMatchingCancel
    {
        add { AddDelegate<RuinsReclaimMatchingCancelResponseBody>(ClientCommandName.RuinsReclaimMatchingCancel, value); }
        remove { DelDelegate<RuinsReclaimMatchingCancelResponseBody>(ClientCommandName.RuinsReclaimMatchingCancel, value); }
    }

    // 유적탈환입장
    public event Delegate<int, RuinsReclaimEnterResponseBody> EventResRuinsReclaimEnter
    {
        add { AddDelegate<RuinsReclaimEnterResponseBody>(ClientCommandName.RuinsReclaimEnter, value); }
        remove { DelDelegate<RuinsReclaimEnterResponseBody>(ClientCommandName.RuinsReclaimEnter, value); }
    }

    // 유적탈환퇴장
    public event Delegate<int, RuinsReclaimExitResponseBody> EventResRuinsReclaimExit
    {
        add { AddDelegate<RuinsReclaimExitResponseBody>(ClientCommandName.RuinsReclaimExit, value); }
        remove { DelDelegate<RuinsReclaimExitResponseBody>(ClientCommandName.RuinsReclaimExit, value); }
    }

    // 유적탈환포기
    public event Delegate<int, RuinsReclaimAbandonResponseBody> EventResRuinsReclaimAbandon
    {
        add { AddDelegate<RuinsReclaimAbandonResponseBody>(ClientCommandName.RuinsReclaimAbandon, value); }
        remove { DelDelegate<RuinsReclaimAbandonResponseBody>(ClientCommandName.RuinsReclaimAbandon, value); }
    }

    // 유적탈환포탈입장
    public event Delegate<int, RuinsReclaimPortalEnterResponseBody> EventResRuinsReclaimPortalEnter
    {
        add { AddDelegate<RuinsReclaimPortalEnterResponseBody>(ClientCommandName.RuinsReclaimPortalEnter, value); }
        remove { DelDelegate<RuinsReclaimPortalEnterResponseBody>(ClientCommandName.RuinsReclaimPortalEnter, value); }
    }

    // 유적탈환부활
    public event Delegate<int, RuinsReclaimReviveResponseBody> EventResRuinsReclaimRevive
    {
        add { AddDelegate<RuinsReclaimReviveResponseBody>(ClientCommandName.RuinsReclaimRevive, value); }
        remove { DelDelegate<RuinsReclaimReviveResponseBody>(ClientCommandName.RuinsReclaimRevive, value); }
    }

    // 유적탈환몬스터변신취소오브젝트상호작용시작
    public event Delegate<int, RuinsReclaimMonsterTransformationCancelObjectInteractionStartResponseBody> EventResRuinsReclaimMonsterTransformationCancelObjectInteractionStart
    {
        add { AddDelegate<RuinsReclaimMonsterTransformationCancelObjectInteractionStartResponseBody>(ClientCommandName.RuinsReclaimMonsterTransformationCancelObjectInteractionStart, value); }
        remove { DelDelegate<RuinsReclaimMonsterTransformationCancelObjectInteractionStartResponseBody>(ClientCommandName.RuinsReclaimMonsterTransformationCancelObjectInteractionStart, value); }
    }

    // 유적탈환보상오브젝트상호작용시작
    public event Delegate<int, RuinsReclaimRewardObjectInteractionStartResponseBody> EventResRuinsReclaimRewardObjectInteractionStart
    {
        add { AddDelegate<RuinsReclaimRewardObjectInteractionStartResponseBody>(ClientCommandName.RuinsReclaimRewardObjectInteractionStart, value); }
        remove { DelDelegate<RuinsReclaimRewardObjectInteractionStartResponseBody>(ClientCommandName.RuinsReclaimRewardObjectInteractionStart, value); }
    }

    #endregion RuinsReclaim

    #region TaskConsignment

    // 할일위탁시작
    public event Delegate<int, TaskConsignmentStartResponseBody> EventResTaskConsignmentStart
    {
        add { AddDelegate<TaskConsignmentStartResponseBody>(ClientCommandName.TaskConsignmentStart, value); }
        remove { DelDelegate<TaskConsignmentStartResponseBody>(ClientCommandName.TaskConsignmentStart, value); }
    }

    // 할일위탁완료
    public event Delegate<int, TaskConsignmentCompleteResponseBody> EventResTaskConsignmentComplete
    {
        add { AddDelegate<TaskConsignmentCompleteResponseBody>(ClientCommandName.TaskConsignmentComplete, value); }
        remove { DelDelegate<TaskConsignmentCompleteResponseBody>(ClientCommandName.TaskConsignmentComplete, value); }
    }

    // 할일위탁즉시완료
    public event Delegate<int, TaskConsignmentImmediatelyCompleteResponseBody> EventResTaskConsignmentImmediatelyComplete
    {
        add { AddDelegate<TaskConsignmentImmediatelyCompleteResponseBody>(ClientCommandName.TaskConsignmentImmediatelyComplete, value); }
        remove { DelDelegate<TaskConsignmentImmediatelyCompleteResponseBody>(ClientCommandName.TaskConsignmentImmediatelyComplete, value); }
    }

    #endregion TaskConsignment

    #region HeroTrueHeroQuest

    // 진정한영웅퀘스트수락
    public event Delegate<int, TrueHeroQuestAcceptResponseBody> EventResTrueHeroQuestAccept
    {
        add { AddDelegate<TrueHeroQuestAcceptResponseBody>(ClientCommandName.TrueHeroQuestAccept, value); }
        remove { DelDelegate<TrueHeroQuestAcceptResponseBody>(ClientCommandName.TrueHeroQuestAccept, value); }
    }

    // 진정한영웅퀘스트완료
    public event Delegate<int, TrueHeroQuestCompleteResponseBody> EventResTrueHeroQuestComplete
    {
        add { AddDelegate<TrueHeroQuestCompleteResponseBody>(ClientCommandName.TrueHeroQuestComplete, value); }
        remove { DelDelegate<TrueHeroQuestCompleteResponseBody>(ClientCommandName.TrueHeroQuestComplete, value); }
    }

    // 진정한영웅퀘스트단계상호작용시작
    public event Delegate<int, TrueHeroQuestStepInteractionStartResponseBody> EventResTrueHeroQuestStepInteractionStart
    {
        add { AddDelegate<TrueHeroQuestStepInteractionStartResponseBody>(ClientCommandName.TrueHeroQuestStepInteractionStart, value); }
        remove { DelDelegate<TrueHeroQuestStepInteractionStartResponseBody>(ClientCommandName.TrueHeroQuestStepInteractionStart, value); }
    }

    #endregion HeroTrueHeroQuest

    #region InfiniteWar

    // 무한대전매칭시작
    public event Delegate<int, InfiniteWarMatchingStartResponseBody> EventResInfiniteWarMatchingStart
    {
        add { AddDelegate<InfiniteWarMatchingStartResponseBody>(ClientCommandName.InfiniteWarMatchingStart, value); }
        remove { DelDelegate<InfiniteWarMatchingStartResponseBody>(ClientCommandName.InfiniteWarMatchingStart, value); }
    }

    // 무한대전매칭취소
    public event Delegate<int, InfiniteWarMatchingCancelResponseBody> EventResInfiniteWarMatchingCancel
    {
        add { AddDelegate<InfiniteWarMatchingCancelResponseBody>(ClientCommandName.InfiniteWarMatchingCancel, value); }
        remove { DelDelegate<InfiniteWarMatchingCancelResponseBody>(ClientCommandName.InfiniteWarMatchingCancel, value); }
    }

    // 무한대전입장
    public event Delegate<int, InfiniteWarEnterResponseBody> EventResInfiniteWarEnter
    {
        add { AddDelegate<InfiniteWarEnterResponseBody>(ClientCommandName.InfiniteWarEnter, value); }
        remove { DelDelegate<InfiniteWarEnterResponseBody>(ClientCommandName.InfiniteWarEnter, value); }
    }

    // 무한대전퇴장
    public event Delegate<int, InfiniteWarExitResponseBody> EventResInfiniteWarExit
    {
        add { AddDelegate<InfiniteWarExitResponseBody>(ClientCommandName.InfiniteWarExit, value); }
        remove { DelDelegate<InfiniteWarExitResponseBody>(ClientCommandName.InfiniteWarExit, value); }
    }

    // 무한대전포기
    public event Delegate<int, InfiniteWarAbandonResponseBody> EventResInfiniteWarAbandon
    {
        add { AddDelegate<InfiniteWarAbandonResponseBody>(ClientCommandName.InfiniteWarAbandon, value); }
        remove { DelDelegate<InfiniteWarAbandonResponseBody>(ClientCommandName.InfiniteWarAbandon, value); }
    }

    // 무한대전부활
    public event Delegate<int, InfiniteWarReviveResponseBody> EventResInfiniteWarRevive
    {
        add { AddDelegate<InfiniteWarReviveResponseBody>(ClientCommandName.InfiniteWarRevive, value); }
        remove { DelDelegate<InfiniteWarReviveResponseBody>(ClientCommandName.InfiniteWarRevive, value); }
    }

    // 무한대전버프상자획득
    public event Delegate<int, InfiniteWarBuffBoxAcquireResponseBody> EventResInfiniteWarBuffBoxAcquire
    {
        add { AddDelegate<InfiniteWarBuffBoxAcquireResponseBody>(ClientCommandName.InfiniteWarBuffBoxAcquire, value); }
        remove { DelDelegate<InfiniteWarBuffBoxAcquireResponseBody>(ClientCommandName.InfiniteWarBuffBoxAcquire, value); }
    }

    #endregion InfiniteWar

    #region Warehouse

    // 창고입고
    public event Delegate<int, WarehouseDepositResponseBody> EventResWarehouseDeposit
    {
        add { AddDelegate<WarehouseDepositResponseBody>(ClientCommandName.WarehouseDeposit, value); }
        remove { DelDelegate<WarehouseDepositResponseBody>(ClientCommandName.WarehouseDeposit, value); }
    }

    // 창고출고
    public event Delegate<int, WarehouseWithdrawResponseBody> EventResWarehouseWithdraw
    {
        add { AddDelegate<WarehouseWithdrawResponseBody>(ClientCommandName.WarehouseWithdraw, value); }
        remove { DelDelegate<WarehouseWithdrawResponseBody>(ClientCommandName.WarehouseWithdraw, value); }
    }

    // 창고슬롯확장
    public event Delegate<int, WarehouseSlotExtendResponseBody> EventResWarehouseSlotExtend
    {
        add { AddDelegate<WarehouseSlotExtendResponseBody>(ClientCommandName.WarehouseSlotExtend, value); }
        remove { DelDelegate<WarehouseSlotExtendResponseBody>(ClientCommandName.WarehouseSlotExtend, value); }
    }

    #endregion Warehouse

    #region FearAltar

    // 공포의제단매칭시작
    public event Delegate<int, FearAltarMatchingStartResponseBody> EventResFearAltarMatchingStart
    {
        add { AddDelegate<FearAltarMatchingStartResponseBody>(ClientCommandName.FearAltarMatchingStart, value); }
        remove { DelDelegate<FearAltarMatchingStartResponseBody>(ClientCommandName.FearAltarMatchingStart, value); }
    }

    // 공포의제단매칭취소
    public event Delegate<int, FearAltarMatchingCancelResponseBody> EventResFearAltarMatchingCancel
    {
        add { AddDelegate<FearAltarMatchingCancelResponseBody>(ClientCommandName.FearAltarMatchingCancel, value); }
        remove { DelDelegate<FearAltarMatchingCancelResponseBody>(ClientCommandName.FearAltarMatchingCancel, value); }
    }

    // 공포의제단입장
    public event Delegate<int, FearAltarEnterResponseBody> EventResFearAltarEnter
    {
        add { AddDelegate<FearAltarEnterResponseBody>(ClientCommandName.FearAltarEnter, value); }
        remove { DelDelegate<FearAltarEnterResponseBody>(ClientCommandName.FearAltarEnter, value); }
    }

    // 공포의제단퇴장
    public event Delegate<int, FearAltarExitResponseBody> EventResFearAltarExit
    {
        add { AddDelegate<FearAltarExitResponseBody>(ClientCommandName.FearAltarExit, value); }
        remove { DelDelegate<FearAltarExitResponseBody>(ClientCommandName.FearAltarExit, value); }
    }

    // 공포의제단포기
    public event Delegate<int, FearAltarAbandonResponseBody> EventResFearAltarAbandon
    {
        add { AddDelegate<FearAltarAbandonResponseBody>(ClientCommandName.FearAltarAbandon, value); }
        remove { DelDelegate<FearAltarAbandonResponseBody>(ClientCommandName.FearAltarAbandon, value); }
    }

    // 공포의제단부활
    public event Delegate<int, FearAltarReviveResponseBody> EventResFearAltarRevive
    {
        add { AddDelegate<FearAltarReviveResponseBody>(ClientCommandName.FearAltarRevive, value); }
        remove { DelDelegate<FearAltarReviveResponseBody>(ClientCommandName.FearAltarRevive, value); }
    }

    // 공포의제단성물원소보상받기
    public event Delegate<int, FearAltarHalidomElementalRewardReceiveResponseBody> EventResFearAltarHalidomElementalRewardReceive
    {
        add { AddDelegate<FearAltarHalidomElementalRewardReceiveResponseBody>(ClientCommandName.FearAltarHalidomElementalRewardReceive, value); }
        remove { DelDelegate<FearAltarHalidomElementalRewardReceiveResponseBody>(ClientCommandName.FearAltarHalidomElementalRewardReceive, value); }
    }

    // 공포의제단성물수집보상받기
    public event Delegate<int, FearAltarHalidomCollectionRewardReceiveResponseBody> EventResFearAltarHalidomCollectionRewardReceive
    {
        add { AddDelegate<FearAltarHalidomCollectionRewardReceiveResponseBody>(ClientCommandName.FearAltarHalidomCollectionRewardReceive, value); }
        remove { DelDelegate<FearAltarHalidomCollectionRewardReceiveResponseBody>(ClientCommandName.FearAltarHalidomCollectionRewardReceive, value); }
    }

    #endregion FearAltar

    #region DiaShop

    // 다이아상점상품구매
    public event Delegate<int, DiaShopProductBuyResponseBody> EventResDiaShopProductBuy
    {
        add { AddDelegate<DiaShopProductBuyResponseBody>(ClientCommandName.DiaShopProductBuy, value); }
        remove { DelDelegate<DiaShopProductBuyResponseBody>(ClientCommandName.DiaShopProductBuy, value); }
    }

    #endregion DiaShop

    #region SubQuest

    // 서브퀘스트수락
    public event Delegate<int, SubQuestAcceptResponseBody> EventResSubQuestAccept
    {
        add { AddDelegate<SubQuestAcceptResponseBody>(ClientCommandName.SubQuestAccept, value); }
        remove { DelDelegate<SubQuestAcceptResponseBody>(ClientCommandName.SubQuestAccept, value); }
    }

    // 서브퀘스트완료
    public event Delegate<int, SubQuestCompleteResponseBody> EventResSubQuestComplete
    {
        add { AddDelegate<SubQuestCompleteResponseBody>(ClientCommandName.SubQuestComplete, value); }
        remove { DelDelegate<SubQuestCompleteResponseBody>(ClientCommandName.SubQuestComplete, value); }
    }

    // 서브퀘스트포기
    public event Delegate<int, SubQuestAbandonResponseBody> EventResSubQuestAbandon
    {
        add { AddDelegate<SubQuestAbandonResponseBody>(ClientCommandName.SubQuestAbandon, value); }
        remove { DelDelegate<SubQuestAbandonResponseBody>(ClientCommandName.SubQuestAbandon, value); }
    }

    #endregion SubQuest

    #region WarMemory

    // 전쟁의기억매칭시작
    public event Delegate<int, WarMemoryMatchingStartResponseBody> EventResWarMemoryMatchingStart
    {
        add { AddDelegate<WarMemoryMatchingStartResponseBody>(ClientCommandName.WarMemoryMatchingStart, value); }
        remove { DelDelegate<WarMemoryMatchingStartResponseBody>(ClientCommandName.WarMemoryMatchingStart, value); }
    }

    // 전쟁의기억매칭취소
    public event Delegate<int, WarMemoryMatchingCancelResponseBody> EventResWarMemoryMatchingCancel
    {
        add { AddDelegate<WarMemoryMatchingCancelResponseBody>(ClientCommandName.WarMemoryMatchingCancel, value); }
        remove { DelDelegate<WarMemoryMatchingCancelResponseBody>(ClientCommandName.WarMemoryMatchingCancel, value); }
    }

    // 전쟁의기억입장
    public event Delegate<int, WarMemoryEnterResponseBody> EventResWarMemoryEnter
    {
        add { AddDelegate<WarMemoryEnterResponseBody>(ClientCommandName.WarMemoryEnter, value); }
        remove { DelDelegate<WarMemoryEnterResponseBody>(ClientCommandName.WarMemoryEnter, value); }
    }

    // 전쟁의기억퇴장
    public event Delegate<int, WarMemoryExitResponseBody> EventResWarMemoryExit
    {
        add { AddDelegate<WarMemoryExitResponseBody>(ClientCommandName.WarMemoryExit, value); }
        remove { DelDelegate<WarMemoryExitResponseBody>(ClientCommandName.WarMemoryExit, value); }
    }

    // 전쟁의기억포기
    public event Delegate<int, WarMemoryAbandonResponseBody> EventResWarMemoryAbandon
    {
        add { AddDelegate<WarMemoryAbandonResponseBody>(ClientCommandName.WarMemoryAbandon, value); }
        remove { DelDelegate<WarMemoryAbandonResponseBody>(ClientCommandName.WarMemoryAbandon, value); }
    }

    // 전쟁의기억부활
    public event Delegate<int, WarMemoryReviveResponseBody> EventResWarMemoryRevive
    {
        add { AddDelegate<WarMemoryReviveResponseBody>(ClientCommandName.WarMemoryRevive, value); }
        remove { DelDelegate<WarMemoryReviveResponseBody>(ClientCommandName.WarMemoryRevive, value); }
    }

    // 전쟁의기억변신오브젝트상호작용시작
    public event Delegate<int, WarMemoryTransformationObjectInteractionStartResponseBody> EventResWarMemoryTransformationObjectInteractionStart
    {
        add { AddDelegate<WarMemoryTransformationObjectInteractionStartResponseBody>(ClientCommandName.WarMemoryTransformationObjectInteractionStart, value); }
        remove { DelDelegate<WarMemoryTransformationObjectInteractionStartResponseBody>(ClientCommandName.WarMemoryTransformationObjectInteractionStart, value); }
    }

    #endregion WarMemory

    #region OrdealQuest

    // 시련퀘스트슬롯완료
    public event Delegate<int, OrdealQuestSlotCompleteResponseBody> EventResOrdealQuestSlotComplete
    {
        add { AddDelegate<OrdealQuestSlotCompleteResponseBody>(ClientCommandName.OrdealQuestSlotComplete, value); }
        remove { DelDelegate<OrdealQuestSlotCompleteResponseBody>(ClientCommandName.OrdealQuestSlotComplete, value); }
    }

    // 시련퀘스트완료
    public event Delegate<int, OrdealQuestCompleteResponseBody> EventResOrdealQuestComplete
    {
        add { AddDelegate<OrdealQuestCompleteResponseBody>(ClientCommandName.OrdealQuestComplete, value); }
        remove { DelDelegate<OrdealQuestCompleteResponseBody>(ClientCommandName.OrdealQuestComplete, value); }
    }

    #endregion OrdealQuest

    #region OsirisRoom

    // 오시리스의방입장에대한대륙퇴장
    public event Delegate<int, ContinentExitForOsirisRoomEnterResponseBody> EventResContinentExitForOsirisRoomEnter
    {
        add { AddDelegate<ContinentExitForOsirisRoomEnterResponseBody>(ClientCommandName.ContinentExitForOsirisRoomEnter, value); }
        remove { DelDelegate<ContinentExitForOsirisRoomEnterResponseBody>(ClientCommandName.ContinentExitForOsirisRoomEnter, value); }
    }

    // 오시리스의방입장
    public event Delegate<int, OsirisRoomEnterResponseBody> EventResOsirisRoomEnter
    {
        add { AddDelegate<OsirisRoomEnterResponseBody>(ClientCommandName.OsirisRoomEnter, value); }
        remove { DelDelegate<OsirisRoomEnterResponseBody>(ClientCommandName.OsirisRoomEnter, value); }
    }

    // 오시리스의방퇴장
    public event Delegate<int, OsirisRoomExitResponseBody> EventResOsirisRoomExit
    {
        add { AddDelegate<OsirisRoomExitResponseBody>(ClientCommandName.OsirisRoomExit, value); }
        remove { DelDelegate<OsirisRoomExitResponseBody>(ClientCommandName.OsirisRoomExit, value); }
    }

    // 오시리스의방포기
    public event Delegate<int, OsirisRoomAbandonResponseBody> EventResOsirisRoomAbandon
    {
        add { AddDelegate<OsirisRoomAbandonResponseBody>(ClientCommandName.OsirisRoomAbandon, value); }
        remove { DelDelegate<OsirisRoomAbandonResponseBody>(ClientCommandName.OsirisRoomAbandon, value); }
    }

    // 오시리스의방재화버프활성화
    public event Delegate<int, OsirisRoomMoneyBuffActivateResponseBody> EventResOsirisRoomMoneyBuffActivate
    {
        add { AddDelegate<OsirisRoomMoneyBuffActivateResponseBody>(ClientCommandName.OsirisRoomMoneyBuffActivate, value); }
        remove { DelDelegate<OsirisRoomMoneyBuffActivateResponseBody>(ClientCommandName.OsirisRoomMoneyBuffActivate, value); }
    }

    #endregion OsirisRoom

    #region Biography

    // 전기시작
    public event Delegate<int, BiographyStartResponseBody> EventResBiographyStart
    {
        add { AddDelegate<BiographyStartResponseBody>(ClientCommandName.BiographyStart, value); }
        remove { DelDelegate<BiographyStartResponseBody>(ClientCommandName.BiographyStart, value); }
    }

    // 전기완료
    public event Delegate<int, BiographyCompleteResponseBody> EventResBiographyComplete
    {
        add { AddDelegate<BiographyCompleteResponseBody>(ClientCommandName.BiographyComplete, value); }
        remove { DelDelegate<BiographyCompleteResponseBody>(ClientCommandName.BiographyComplete, value); }
    }

    // 전기퀘스트수락
    public event Delegate<int, BiographyQuestAcceptResponseBody> EventResBiographyQuestAccept
    {
        add { AddDelegate<BiographyQuestAcceptResponseBody>(ClientCommandName.BiographyQuestAccept, value); }
        remove { DelDelegate<BiographyQuestAcceptResponseBody>(ClientCommandName.BiographyQuestAccept, value); }
    }

    // 전기퀘스트완료
    public event Delegate<int, BiographyQuestCompleteResponseBody> EventResBiographyQuestComplete
    {
        add { AddDelegate<BiographyQuestCompleteResponseBody>(ClientCommandName.BiographyQuestComplete, value); }
        remove { DelDelegate<BiographyQuestCompleteResponseBody>(ClientCommandName.BiographyQuestComplete, value); }
    }

    // 전기퀘스트이동목표완료
    public event Delegate<int, BiographyQuestMoveObjectiveCompleteResponseBody> EventResBiographyQuestMoveObjectiveComplete
    {
        add { AddDelegate<BiographyQuestMoveObjectiveCompleteResponseBody>(ClientCommandName.BiographyQuestMoveObjectiveComplete, value); }
        remove { DelDelegate<BiographyQuestMoveObjectiveCompleteResponseBody>(ClientCommandName.BiographyQuestMoveObjectiveComplete, value); }
    }

    // 전기퀘스트NPC대화완료
    public event Delegate<int, BiographyQuestNpcConversationCompleteResponseBody> EventResBiographyQuestNpcConversationComplete
    {
        add { AddDelegate<BiographyQuestNpcConversationCompleteResponseBody>(ClientCommandName.BiographyQuestNpcConversationComplete, value); }
        remove { DelDelegate<BiographyQuestNpcConversationCompleteResponseBody>(ClientCommandName.BiographyQuestNpcConversationComplete, value); }
    }

    #endregion Biography

    #region BiographyQuestDungeon

    // 전기퀘스트던전입장에대한대륙퇴장
    public event Delegate<int, ContinentExitForBiographyQuestDungeonEnterResponseBody> EventResContinentExitForBiographyQuestDungeonEnter
    {
        add { AddDelegate<ContinentExitForBiographyQuestDungeonEnterResponseBody>(ClientCommandName.ContinentExitForBiographyQuestDungeonEnter, value); }
        remove { DelDelegate<ContinentExitForBiographyQuestDungeonEnterResponseBody>(ClientCommandName.ContinentExitForBiographyQuestDungeonEnter, value); }
    }

    // 전기퀘스트던전입장
    public event Delegate<int, BiographyQuestDungeonEnterResponseBody> EventResBiographyQuestDungeonEnter
    {
        add { AddDelegate<BiographyQuestDungeonEnterResponseBody>(ClientCommandName.BiographyQuestDungeonEnter, value); }
        remove { DelDelegate<BiographyQuestDungeonEnterResponseBody>(ClientCommandName.BiographyQuestDungeonEnter, value); }
    }

    // 전기퀘스트던전퇴장
    public event Delegate<int, BiographyQuestDungeonExitResponseBody> EventResBiographyQuestDungeonExit
    {
        add { AddDelegate<BiographyQuestDungeonExitResponseBody>(ClientCommandName.BiographyQuestDungeonExit, value); }
        remove { DelDelegate<BiographyQuestDungeonExitResponseBody>(ClientCommandName.BiographyQuestDungeonExit, value); }
    }

    // 전기퀘스트던전포기
    public event Delegate<int, BiographyQuestDungeonAbandonResponseBody> EventResBiographyQuestDungeonAbandon
    {
        add { AddDelegate<BiographyQuestDungeonAbandonResponseBody>(ClientCommandName.BiographyQuestDungeonAbandon, value); }
        remove { DelDelegate<BiographyQuestDungeonAbandonResponseBody>(ClientCommandName.BiographyQuestDungeonAbandon, value); }
    }

    // 전기퀘스트던전부활
    public event Delegate<int, BiographyQuestDungeonReviveResponseBody> EventResBiographyQuestDungeonRevive
    {
        add { AddDelegate<BiographyQuestDungeonReviveResponseBody>(ClientCommandName.BiographyQuestDungeonRevive, value); }
        remove { DelDelegate<BiographyQuestDungeonReviveResponseBody>(ClientCommandName.BiographyQuestDungeonRevive, value); }
    }

    #endregion BiographyQuestDungeon

    #region Friend

    // 친구목록
    public event Delegate<int, FriendListResponseBody> EventResFriendList
    {
        add { AddDelegate<FriendListResponseBody>(ClientCommandName.FriendList, value); }
        remove { DelDelegate<FriendListResponseBody>(ClientCommandName.FriendList, value); }
    }

    // 친구삭제
    public event Delegate<int, FriendDeleteResponseBody> EventResFriendDelete
    {
        add { AddDelegate<FriendDeleteResponseBody>(ClientCommandName.FriendDelete, value); }
        remove { DelDelegate<FriendDeleteResponseBody>(ClientCommandName.FriendDelete, value); }
    }

    // 친구신청
    public event Delegate<int, FriendApplyResponseBody> EventResFriendApply
    {
        add { AddDelegate<FriendApplyResponseBody>(ClientCommandName.FriendApply, value); }
        remove { DelDelegate<FriendApplyResponseBody>(ClientCommandName.FriendApply, value); }
    }

    // 친구신청수락
    public event Delegate<int, FriendApplicationAcceptResponseBody> EventResFriendApplicationAccept
    {
        add { AddDelegate<FriendApplicationAcceptResponseBody>(ClientCommandName.FriendApplicationAccept, value); }
        remove { DelDelegate<FriendApplicationAcceptResponseBody>(ClientCommandName.FriendApplicationAccept, value); }
    }

    // 친구신청거절
    public event Delegate<int, FriendApplicationRefuseResponseBody> EventResFriendApplicationRefuse
    {
        add { AddDelegate<FriendApplicationRefuseResponseBody>(ClientCommandName.FriendApplicationRefuse, value); }
        remove { DelDelegate<FriendApplicationRefuseResponseBody>(ClientCommandName.FriendApplicationRefuse, value); }
    }

    // 친구신청을위한영웅검색
    public event Delegate<int, HeroSearchForFriendApplicationResponseBody> EventResHeroSearchForFriendApplication
    {
        add { AddDelegate<HeroSearchForFriendApplicationResponseBody>(ClientCommandName.HeroSearchForFriendApplication, value); }
        remove { DelDelegate<HeroSearchForFriendApplicationResponseBody>(ClientCommandName.HeroSearchForFriendApplication, value); }
    }

    #endregion Friend

    #region Blacklist

    // 블랙리스트항목추가
    public event Delegate<int, BlacklistEntryAddResponseBody> EventResBlacklistEntryAdd
    {
        add { AddDelegate<BlacklistEntryAddResponseBody>(ClientCommandName.BlacklistEntryAdd, value); }
        remove { DelDelegate<BlacklistEntryAddResponseBody>(ClientCommandName.BlacklistEntryAdd, value); }
    }

    // 블랙리스트항목삭제
    public event Delegate<int, BlacklistEntryDeleteResponseBody> EventResBlacklistEntryDelete
    {
        add { AddDelegate<BlacklistEntryDeleteResponseBody>(ClientCommandName.BlacklistEntryDelete, value); }
        remove { DelDelegate<BlacklistEntryDeleteResponseBody>(ClientCommandName.BlacklistEntryDelete, value); }
    }

    #endregion Blacklist

    #region ItemLuckyShop

    // 아이템행운상점무료뽑기
    public event Delegate<int, ItemLuckyShopFreePickResponseBody> EventResItemLuckyShopFreePick
    {
        add { AddDelegate<ItemLuckyShopFreePickResponseBody>(ClientCommandName.ItemLuckyShopFreePick, value); }
        remove { DelDelegate<ItemLuckyShopFreePickResponseBody>(ClientCommandName.ItemLuckyShopFreePick, value); }
    }

    // 아이템행운상점1회뽑기
    public event Delegate<int, ItemLuckyShop1TimePickResponseBody> EventResItemLuckyShop1TimePick
    {
        add { AddDelegate<ItemLuckyShop1TimePickResponseBody>(ClientCommandName.ItemLuckyShop1TimePick, value); }
        remove { DelDelegate<ItemLuckyShop1TimePickResponseBody>(ClientCommandName.ItemLuckyShop1TimePick, value); }
    }

    // 아이템행운상점5회뽑기
    public event Delegate<int, ItemLuckyShop5TimePickResponseBody> EventResItemLuckyShop5TimePick
    {
        add { AddDelegate<ItemLuckyShop5TimePickResponseBody>(ClientCommandName.ItemLuckyShop5TimePick, value); }
        remove { DelDelegate<ItemLuckyShop5TimePickResponseBody>(ClientCommandName.ItemLuckyShop5TimePick, value); }
    }

    #endregion ItemLuckyShop

    #region CreatureCardLuckyShop

    // 크리처카드상점행운상점무료뽑기
    public event Delegate<int, CreatureCardLuckyShopFreePickResponseBody> EventResCreatureCardLuckyShopFreePick
    {
        add { AddDelegate<CreatureCardLuckyShopFreePickResponseBody>(ClientCommandName.CreatureCardLuckyShopFreePick, value); }
        remove { DelDelegate<CreatureCardLuckyShopFreePickResponseBody>(ClientCommandName.CreatureCardLuckyShopFreePick, value); }
    }

    // 크리처카드상점행운상점1회뽑기
    public event Delegate<int, CreatureCardLuckyShop1TimePickResponseBody> EventResCreatureCardLuckyShop1TimePick
    {
        add { AddDelegate<CreatureCardLuckyShop1TimePickResponseBody>(ClientCommandName.CreatureCardLuckyShop1TimePick, value); }
        remove { DelDelegate<CreatureCardLuckyShop1TimePickResponseBody>(ClientCommandName.CreatureCardLuckyShop1TimePick, value); }
    }

    // 크리처카드상점행운상점5회뽑기
    public event Delegate<int, CreatureCardLuckyShop5TimePickResponseBody> EventResCreatureCardLuckyShop5TimePick
    {
        add { AddDelegate<CreatureCardLuckyShop5TimePickResponseBody>(ClientCommandName.CreatureCardLuckyShop5TimePick, value); }
        remove { DelDelegate<CreatureCardLuckyShop5TimePickResponseBody>(ClientCommandName.CreatureCardLuckyShop5TimePick, value); }
    }

    #endregion CreatureCardLuckyShop

    #region BlessingQuest

    // 축복퀘스트축복발송
    public event Delegate<int, BlessingQuestBlessingSendResponseBody> EventResBlessingQuestBlessingSend
    {
        add { AddDelegate<BlessingQuestBlessingSendResponseBody>(ClientCommandName.BlessingQuestBlessingSend, value); }
        remove { DelDelegate<BlessingQuestBlessingSendResponseBody>(ClientCommandName.BlessingQuestBlessingSend, value); }
    }

    // 축복퀘스트모두삭제
    public event Delegate<int, BlessingQuestDeleteAllResponseBody> EventResBlessingQuestDeleteAll
    {
        add { AddDelegate<BlessingQuestDeleteAllResponseBody>(ClientCommandName.BlessingQuestDeleteAll, value); }
        remove { DelDelegate<BlessingQuestDeleteAllResponseBody>(ClientCommandName.BlessingQuestDeleteAll, value); }
    }

    #endregion BlessingQuest

    #region Blessing

    // 축복보상받기
    public event Delegate<int, BlessingRewardReceiveResponseBody> EventResBlessingRewardReceive
    {
        add { AddDelegate<BlessingRewardReceiveResponseBody>(ClientCommandName.BlessingRewardReceive, value); }
        remove { DelDelegate<BlessingRewardReceiveResponseBody>(ClientCommandName.BlessingRewardReceive, value); }
    }

    // 축복모두삭제
    public event Delegate<int, BlessingDeleteAllResponseBody> EventResBlessingDeleteAll
    {
        add { AddDelegate<BlessingDeleteAllResponseBody>(ClientCommandName.BlessingDeleteAll, value); }
        remove { DelDelegate<BlessingDeleteAllResponseBody>(ClientCommandName.BlessingDeleteAll, value); }
    }

    #endregion Blessing

    #region OwnerProspectQuest

    // 소유유망자퀘스트보상받기
    public event Delegate<int, OwnerProspectQuestRewardReceiveResponseBody> EventResOwnerProspectQuestRewardReceive
    {
        add { AddDelegate<OwnerProspectQuestRewardReceiveResponseBody>(ClientCommandName.OwnerProspectQuestRewardReceive, value); }
        remove { DelDelegate<OwnerProspectQuestRewardReceiveResponseBody>(ClientCommandName.OwnerProspectQuestRewardReceive, value); }
    }

    // 소유유망자퀘스트보상모두받기
    public event Delegate<int, OwnerProspectQuestRewardReceiveAllResponseBody> EventResOwnerProspectQuestRewardReceiveAll
    {
        add { AddDelegate<OwnerProspectQuestRewardReceiveAllResponseBody>(ClientCommandName.OwnerProspectQuestRewardReceiveAll, value); }
        remove { DelDelegate<OwnerProspectQuestRewardReceiveAllResponseBody>(ClientCommandName.OwnerProspectQuestRewardReceiveAll, value); }
    }

    // 대상유망자퀘스트보상받기
    public event Delegate<int, TargetProspectQuestRewardReceiveResponseBody> EventResTargetProspectQuestRewardReceive
    {
        add { AddDelegate<TargetProspectQuestRewardReceiveResponseBody>(ClientCommandName.TargetProspectQuestRewardReceive, value); }
        remove { DelDelegate<TargetProspectQuestRewardReceiveResponseBody>(ClientCommandName.TargetProspectQuestRewardReceive, value); }
    }

    // 대상유망자퀘스트보상모두받기
    public event Delegate<int, TargetProspectQuestRewardReceiveAllResponseBody> EventResTargetProspectQuestRewardReceiveAll
    {
        add { AddDelegate<TargetProspectQuestRewardReceiveAllResponseBody>(ClientCommandName.TargetProspectQuestRewardReceiveAll, value); }
        remove { DelDelegate<TargetProspectQuestRewardReceiveAllResponseBody>(ClientCommandName.TargetProspectQuestRewardReceiveAll, value); }
    }

    #endregion OwnerProspectQuest

    #region DragonNest

    // 용의둥지매칭시작
    public event Delegate<int, DragonNestMatchingStartResponseBody> EventResDragonNestMatchingStart
    {
        add { AddDelegate<DragonNestMatchingStartResponseBody>(ClientCommandName.DragonNestMatchingStart, value); }
        remove { DelDelegate<DragonNestMatchingStartResponseBody>(ClientCommandName.DragonNestMatchingStart, value); }
    }

    // 용의둥지매칭취소
    public event Delegate<int, DragonNestMatchingCancelResponseBody> EventResDragonNestMatchingCancel
    {
        add { AddDelegate<DragonNestMatchingCancelResponseBody>(ClientCommandName.DragonNestMatchingCancel, value); }
        remove { DelDelegate<DragonNestMatchingCancelResponseBody>(ClientCommandName.DragonNestMatchingCancel, value); }
    }

    // 용의둥지입장
    public event Delegate<int, DragonNestEnterResponseBody> EventResDragonNestEnter
    {
        add { AddDelegate<DragonNestEnterResponseBody>(ClientCommandName.DragonNestEnter, value); }
        remove { DelDelegate<DragonNestEnterResponseBody>(ClientCommandName.DragonNestEnter, value); }
    }

    // 용의둥지퇴장
    public event Delegate<int, DragonNestExitResponseBody> EventResDragonNestExit
    {
        add { AddDelegate<DragonNestExitResponseBody>(ClientCommandName.DragonNestExit, value); }
        remove { DelDelegate<DragonNestExitResponseBody>(ClientCommandName.DragonNestExit, value); }
    }

    // 용의둥지포기
    public event Delegate<int, DragonNestAbandonResponseBody> EventResDragonNestAbandon
    {
        add { AddDelegate<DragonNestAbandonResponseBody>(ClientCommandName.DragonNestAbandon, value); }
        remove { DelDelegate<DragonNestAbandonResponseBody>(ClientCommandName.DragonNestAbandon, value); }
    }

    // 용의둥지부활
    public event Delegate<int, DragonNestReviveResponseBody> EventResDragonNestRevive
    {
        add { AddDelegate<DragonNestReviveResponseBody>(ClientCommandName.DragonNestRevive, value); }
        remove { DelDelegate<DragonNestReviveResponseBody>(ClientCommandName.DragonNestRevive, value); }
    }

    #endregion DragonNest

    #region Creature

    // 크리처출전
    public event Delegate<int, CreatureParticipateResponseBody> EventResCreatureParticipate
    {
        add { AddDelegate<CreatureParticipateResponseBody>(ClientCommandName.CreatureParticipate, value); }
        remove { DelDelegate<CreatureParticipateResponseBody>(ClientCommandName.CreatureParticipate, value); }
    }

    // 크리처출전취소
    public event Delegate<int, CreatureParticipationCancelResponseBody> EventResCreatureParticipationCancel
    {
        add { AddDelegate<CreatureParticipationCancelResponseBody>(ClientCommandName.CreatureParticipationCancel, value); }
        remove { DelDelegate<CreatureParticipationCancelResponseBody>(ClientCommandName.CreatureParticipationCancel, value); }
    }

    // 크리처응원
    public event Delegate<int, CreatureCheerResponseBody> EventResCreatureCheer
    {
        add { AddDelegate<CreatureCheerResponseBody>(ClientCommandName.CreatureCheer, value); }
        remove { DelDelegate<CreatureCheerResponseBody>(ClientCommandName.CreatureCheer, value); }
    }

    // 크리처응원취소
    public event Delegate<int, CreatureCheerCancelResponseBody> EventResCreatureCheerCancel
    {
        add { AddDelegate<CreatureCheerCancelResponseBody>(ClientCommandName.CreatureCheerCancel, value); }
        remove { DelDelegate<CreatureCheerCancelResponseBody>(ClientCommandName.CreatureCheerCancel, value); }
    }

    // 크리처양육
    public event Delegate<int, CreatureRearResponseBody> EventResCreatureRear
    {
        add { AddDelegate<CreatureRearResponseBody>(ClientCommandName.CreatureRear, value); }
        remove { DelDelegate<CreatureRearResponseBody>(ClientCommandName.CreatureRear, value); }
    }

    // 크리처방생
    public event Delegate<int, CreatureReleaseResponseBody> EventResCreatureRelease
    {
        add { AddDelegate<CreatureReleaseResponseBody>(ClientCommandName.CreatureRelease, value); }
        remove { DelDelegate<CreatureReleaseResponseBody>(ClientCommandName.CreatureRelease, value); }
    }

    // 크리처주입
    public event Delegate<int, CreatureInjectResponseBody> EventResCreatureInject
    {
        add { AddDelegate<CreatureInjectResponseBody>(ClientCommandName.CreatureInject, value); }
        remove { DelDelegate<CreatureInjectResponseBody>(ClientCommandName.CreatureInject, value); }
    }

    // 크리처주입회수
    public event Delegate<int, CreatureInjectionRetrievalResponseBody> EventResCreatureInjectionRetrieval
    {
        add { AddDelegate<CreatureInjectionRetrievalResponseBody>(ClientCommandName.CreatureInjectionRetrieval, value); }
        remove { DelDelegate<CreatureInjectionRetrievalResponseBody>(ClientCommandName.CreatureInjectionRetrieval, value); }
    }

    // 크리처변이
    public event Delegate<int, CreatureVaryResponseBody> EventResCreatureVary
    {
        add { AddDelegate<CreatureVaryResponseBody>(ClientCommandName.CreatureVary, value); }
        remove { DelDelegate<CreatureVaryResponseBody>(ClientCommandName.CreatureVary, value); }
    }

    // 크리처추가속성변환
    public event Delegate<int, CreatureAdditionalAttrSwitchResponseBody> EventResCreatureAdditionalAttrSwitch
    {
        add { AddDelegate<CreatureAdditionalAttrSwitchResponseBody>(ClientCommandName.CreatureAdditionalAttrSwitch, value); }
        remove { DelDelegate<CreatureAdditionalAttrSwitchResponseBody>(ClientCommandName.CreatureAdditionalAttrSwitch, value); }
    }

    // 크리처스킬슬롯개방
    public event Delegate<int, CreatureSkillSlotOpenResponseBody> EventResCreatureSkillSlotOpen
    {
        add { AddDelegate<CreatureSkillSlotOpenResponseBody>(ClientCommandName.CreatureSkillSlotOpen, value); }
        remove { DelDelegate<CreatureSkillSlotOpenResponseBody>(ClientCommandName.CreatureSkillSlotOpen, value); }
    }

    // 크리처합성
    public event Delegate<int, CreatureComposeResponseBody> EventResCreatureCompose
    {
        add { AddDelegate<CreatureComposeResponseBody>(ClientCommandName.CreatureCompose, value); }
        remove { DelDelegate<CreatureComposeResponseBody>(ClientCommandName.CreatureCompose, value); }
    }

    // 크리처알사용
    public event Delegate<int, CreatureEggUseResponseBody> EventResCreatureEggUse
    {
        add { AddDelegate<CreatureEggUseResponseBody>(ClientCommandName.CreatureEggUse, value); }
        remove { DelDelegate<CreatureEggUseResponseBody>(ClientCommandName.CreatureEggUse, value); }
    }

    #endregion Creature

    #region Costume

    // 코스튬아이템사용
    public event Delegate<int, CostumeItemUseResponseBody> EventResCostumeItemUse
    {
        add { AddDelegate<CostumeItemUseResponseBody>(ClientCommandName.CostumeItemUse, value); }
        remove { DelDelegate<CostumeItemUseResponseBody>(ClientCommandName.CostumeItemUse, value); }
    }

    // 코스튬장착
    public event Delegate<int, CostumeEquipResponseBody> EventResCostumeEquip
    {
        add { AddDelegate<CostumeEquipResponseBody>(ClientCommandName.CostumeEquip, value); }
        remove { DelDelegate<CostumeEquipResponseBody>(ClientCommandName.CostumeEquip, value); }
    }

    // 코스튬장착해제
    public event Delegate<int, CostumeUnequipResponseBody> EventResCostumeUnequip
    {
        add { AddDelegate<CostumeUnequipResponseBody>(ClientCommandName.CostumeUnequip, value); }
        remove { DelDelegate<CostumeUnequipResponseBody>(ClientCommandName.CostumeUnequip, value); }
    }

    // 효스튬효과적용
    public event Delegate<int, CostumeEffectApplyResponseBody> EventResCostumeEffectApply
    {
        add { AddDelegate<CostumeEffectApplyResponseBody>(ClientCommandName.CostumeEffectApply, value); }
        remove { DelDelegate<CostumeEffectApplyResponseBody>(ClientCommandName.CostumeEffectApply, value); }
    }

    // 코스튬강화
    public event Delegate<int, CostumeEnchantResponseBody> EventResCostumeEnchant
    {
        add { AddDelegate<CostumeEnchantResponseBody>(ClientCommandName.CostumeEnchant, value); }
        remove { DelDelegate<CostumeEnchantResponseBody>(ClientCommandName.CostumeEnchant, value); }
    }

    // 코스튬콜렉션셔플
    public event Delegate<int, CostumeCollectionShuffleResponseBody> EventResCostumeCollectionShuffle
    {
        add { AddDelegate<CostumeCollectionShuffleResponseBody>(ClientCommandName.CostumeCollectionShuffle, value); }
        remove { DelDelegate<CostumeCollectionShuffleResponseBody>(ClientCommandName.CostumeCollectionShuffle, value); }
    }

    // 코스튬콜렉션활성화
    public event Delegate<int, CostumeCollectionActivateResponseBody> EventResCostumeCollectionActivate
    {
        add { AddDelegate<CostumeCollectionActivateResponseBody>(ClientCommandName.CostumeCollectionActivate, value); }
        remove { DelDelegate<CostumeCollectionActivateResponseBody>(ClientCommandName.CostumeCollectionActivate, value); }
    }

    #endregion Costume
    
    #region Present

    // 선물발송
    public event Delegate<int, PresentSendResponseBody> EventResPresentSend
    {
        add { AddDelegate<PresentSendResponseBody>(ClientCommandName.PresentSend, value); }
        remove { DelDelegate<PresentSendResponseBody>(ClientCommandName.PresentSend, value); }
    }

    // 선물답장
    public event Delegate<int, PresentReplyResponseBody> EventResPresentReply
    {
        add { AddDelegate<PresentReplyResponseBody>(ClientCommandName.PresentReply, value); }
        remove { DelDelegate<PresentReplyResponseBody>(ClientCommandName.PresentReply, value); }
    }
    
    #endregion Present

    #region Present Ranking

    // 서버선물인기점수랭킹
    public event Delegate<int, ServerPresentPopularityPointRankingResponseBody> EventResServerPresentPopularityPointRanking
    {
        add { AddDelegate<ServerPresentPopularityPointRankingResponseBody>(ClientCommandName.ServerPresentPopularityPointRanking, value); }
        remove { DelDelegate<ServerPresentPopularityPointRankingResponseBody>(ClientCommandName.ServerPresentPopularityPointRanking, value); }
    }

    // 국가주간선물인기점수랭킹
    public event Delegate<int, NationWeeklyPresentPopularityPointRankingResponseBody> EventResNationWeeklyPresentPopularityPointRanking
    {
        add { AddDelegate<NationWeeklyPresentPopularityPointRankingResponseBody>(ClientCommandName.NationWeeklyPresentPopularityPointRanking, value); }
        remove { DelDelegate<NationWeeklyPresentPopularityPointRankingResponseBody>(ClientCommandName.NationWeeklyPresentPopularityPointRanking, value); }
    }

    // 국가주간선물인기점수랭킹보상받기
    public event Delegate<int, NationWeeklyPresentPopularityPointRankingRewardReceiveResponseBody> EventResNationWeeklyPresentPopularityPointRankingRewardReceive
    {
        add { AddDelegate<NationWeeklyPresentPopularityPointRankingRewardReceiveResponseBody>(ClientCommandName.NationWeeklyPresentPopularityPointRankingRewardReceive, value); }
        remove { DelDelegate<NationWeeklyPresentPopularityPointRankingRewardReceiveResponseBody>(ClientCommandName.NationWeeklyPresentPopularityPointRankingRewardReceive, value); }
    }

    // 서버선물공헌점수랭킹
    public event Delegate<int, ServerPresentContributionPointRankingResponseBody> EventResServerPresentContributionPointRanking
    {
        add { AddDelegate<ServerPresentContributionPointRankingResponseBody>(ClientCommandName.ServerPresentContributionPointRanking, value); }
        remove { DelDelegate<ServerPresentContributionPointRankingResponseBody>(ClientCommandName.ServerPresentContributionPointRanking, value); }
    }

    // 국가주간선물공헌점수랭킹
    public event Delegate<int, NationWeeklyPresentContributionPointRankingResponseBody> EventResNationWeeklyPresentContributionPointRanking
    {
        add { AddDelegate<NationWeeklyPresentContributionPointRankingResponseBody>(ClientCommandName.NationWeeklyPresentContributionPointRanking, value); }
        remove { DelDelegate<NationWeeklyPresentContributionPointRankingResponseBody>(ClientCommandName.NationWeeklyPresentContributionPointRanking, value); }
    }

    // 국가주간선물공헌점수랭킹보상받기
    public event Delegate<int, NationWeeklyPresentContributionPointRankingRewardReceiveResponseBody> EventResNationWeeklyPresentContributionPointRankingRewardReceive
    {
        add { AddDelegate<NationWeeklyPresentContributionPointRankingRewardReceiveResponseBody>(ClientCommandName.NationWeeklyPresentContributionPointRankingRewardReceive, value); }
        remove { DelDelegate<NationWeeklyPresentContributionPointRankingRewardReceiveResponseBody>(ClientCommandName.NationWeeklyPresentContributionPointRankingRewardReceive, value); }
    }

    #endregion Present Ranking

    #region CreatureFarmQuest

    // 크리처농장퀘스트수락
    public event Delegate<int, CreatureFarmQuestAcceptResponseBody> EventResCreatureFarmQuestAccept
    {
        add { AddDelegate<CreatureFarmQuestAcceptResponseBody>(ClientCommandName.CreatureFarmQuestAccept, value); }
        remove { DelDelegate<CreatureFarmQuestAcceptResponseBody>(ClientCommandName.CreatureFarmQuestAccept, value); }
    }

    // 크리처농장퀘스트완료
    public event Delegate<int, CreatureFarmQuestCompleteResponseBody> EventResCreatureFarmQuestComplete
    {
        add { AddDelegate<CreatureFarmQuestCompleteResponseBody>(ClientCommandName.CreatureFarmQuestComplete, value); }
        remove { DelDelegate<CreatureFarmQuestCompleteResponseBody>(ClientCommandName.CreatureFarmQuestComplete, value); }
    }

    // 크리처농장퀘스트미션이동목표완료
    public event Delegate<int, CreatureFarmQuestMissionMoveObjectiveCompleteResponseBody> EventResCreatureFarmQuestMissionMoveObjectiveComplete
    {
        add { AddDelegate<CreatureFarmQuestMissionMoveObjectiveCompleteResponseBody>(ClientCommandName.CreatureFarmQuestMissionMoveObjectiveComplete, value); }
        remove { DelDelegate<CreatureFarmQuestMissionMoveObjectiveCompleteResponseBody>(ClientCommandName.CreatureFarmQuestMissionMoveObjectiveComplete, value); }
    }

    #endregion CreatureFarmQuest

    #region GuildBlessing

    // 길드축복시작
    public event Delegate<int, GuildBlessingBuffStartResponseBody> EventResGuildBlessingBuffStart
    {
        add { AddDelegate<GuildBlessingBuffStartResponseBody>(ClientCommandName.GuildBlessingBuffStart, value); }
        remove { DelDelegate<GuildBlessingBuffStartResponseBody>(ClientCommandName.GuildBlessingBuffStart, value); }
    }

    #endregion GuildBlessing

    #region JobChangeQuest

    // 전직퀘스트수락
    public event Delegate<int, JobChangeQuestAcceptResponseBody> EventResJobChangeQuestAccept
    {
        add { AddDelegate<JobChangeQuestAcceptResponseBody>(ClientCommandName.JobChangeQuestAccept, value); }
        remove { DelDelegate<JobChangeQuestAcceptResponseBody>(ClientCommandName.JobChangeQuestAccept, value); }
    }

    // 전직퀘스트완료
    public event Delegate<int, JobChangeQuestCompleteResponseBody> EventResJobChangeQuestComplete
    {
        add { AddDelegate<JobChangeQuestCompleteResponseBody>(ClientCommandName.JobChangeQuestComplete, value); }
        remove { DelDelegate<JobChangeQuestCompleteResponseBody>(ClientCommandName.JobChangeQuestComplete, value); }
    }

    #endregion JobChangeQuest

    #region HeroJobChange

    // 영웅전직
    public event Delegate<int, HeroJobChangeResponseBody> EventResHeroJobChange
    {
        add { AddDelegate<HeroJobChangeResponseBody>(ClientCommandName.HeroJobChange, value); }
        remove { DelDelegate<HeroJobChangeResponseBody>(ClientCommandName.HeroJobChange, value); }
    }

    #endregion HeroJobChange

    #region HeroAttrPotion

    // 영웅속성물약사용
    public event Delegate<int, HeroAttrPotionUseResponseBody> EventResHeroAttrPotionUse
    {
        add { AddDelegate<HeroAttrPotionUseResponseBody>(ClientCommandName.HeroAttrPotionUse, value); }
        remove { DelDelegate<HeroAttrPotionUseResponseBody>(ClientCommandName.HeroAttrPotionUse, value); }
    }

    // 영웅속성물약전체사용
    public event Delegate<int, HeroAttrPotionUseAllResponseBody> EventResHeroAttrPotionUseAll
    {
        add { AddDelegate<HeroAttrPotionUseAllResponseBody>(ClientCommandName.HeroAttrPotionUseAll, value); }
        remove { DelDelegate<HeroAttrPotionUseAllResponseBody>(ClientCommandName.HeroAttrPotionUseAll, value); }
    }

    #endregion HeroAttrPotion

    #region CashProductPurchase

    // 캐쉬상품구매시작
    public event Delegate<int, CashProductPurchaseStartResponseBody> EventResCashProductPurchaseStart
    {
        add { AddDelegate<CashProductPurchaseStartResponseBody>(ClientCommandName.CashProductPurchaseStart, value); }
        remove { DelDelegate<CashProductPurchaseStartResponseBody>(ClientCommandName.CashProductPurchaseStart, value); }
    }

    // 캐쉬상품구매취소
    public event Delegate<int, CashProductPurchaseCancelResponseBody> EventResCashProductPurchaseCancel
    {
        add { AddDelegate<CashProductPurchaseCancelResponseBody>(ClientCommandName.CashProductPurchaseCancel, value); }
        remove { DelDelegate<CashProductPurchaseCancelResponseBody>(ClientCommandName.CashProductPurchaseCancel, value); }
    }

    // 캐쉬상품구매실패
    public event Delegate<int, CashProductPurchaseFailResponseBody> EventResCashProductPurchaseFail
    {
        add { AddDelegate<CashProductPurchaseFailResponseBody>(ClientCommandName.CashProductPurchaseFail, value); }
        remove { DelDelegate<CashProductPurchaseFailResponseBody>(ClientCommandName.CashProductPurchaseFail, value); }
    }

    // 캐쉬상품구매완료
    public event Delegate<int, CashProductPurchaseCompleteResponseBody> EventResCashProductPurchaseComplete
    {
        add { AddDelegate<CashProductPurchaseCompleteResponseBody>(ClientCommandName.CashProductPurchaseComplete, value); }
        remove { DelDelegate<CashProductPurchaseCompleteResponseBody>(ClientCommandName.CashProductPurchaseComplete, value); }
    }

    #endregion CashProductPurchase

    #region FirstChargeEvent

    // 첫충전이벤트보상받기
    public event Delegate<int, FirstChargeEventRewardReceiveResponseBody> EventResFirstChargeEventRewardReceive
    {
        add { AddDelegate<FirstChargeEventRewardReceiveResponseBody>(ClientCommandName.FirstChargeEventRewardReceive, value); }
        remove { DelDelegate<FirstChargeEventRewardReceiveResponseBody>(ClientCommandName.FirstChargeEventRewardReceive, value); }
    }

    // 재충전이벤트보상받기
    public event Delegate<int, RechargeEventRewardReceiveResponseBody> EventResRechargeEventRewardReceive
    {
        add { AddDelegate<RechargeEventRewardReceiveResponseBody>(ClientCommandName.RechargeEventRewardReceive, value); }
        remove { DelDelegate<RechargeEventRewardReceiveResponseBody>(ClientCommandName.RechargeEventRewardReceive, value); }
    }

    // 충전이벤트미션보상받기
    public event Delegate<int, ChargeEventMissionRewardReceiveResponseBody> EventResChargeEventMissionRewardReceive
    {
        add { AddDelegate<ChargeEventMissionRewardReceiveResponseBody>(ClientCommandName.ChargeEventMissionRewardReceive, value); }
        remove { DelDelegate<ChargeEventMissionRewardReceiveResponseBody>(ClientCommandName.ChargeEventMissionRewardReceive, value); }
    }

    // 매일충전이벤트미션보상받기
    public event Delegate<int, DailyChargeEventMissionRewardReceiveResponseBody> EventResDailyChargeEventMissionRewardReceive
    {
        add { AddDelegate<DailyChargeEventMissionRewardReceiveResponseBody>(ClientCommandName.DailyChargeEventMissionRewardReceive, value); }
        remove { DelDelegate<DailyChargeEventMissionRewardReceiveResponseBody>(ClientCommandName.DailyChargeEventMissionRewardReceive, value); }
    }

    #endregion FirstChargeEvent

    #region ConsumeEvent

    // 소비이벤트미션보상받기
    public event Delegate<int, ConsumeEventMissionRewardReceiveResponseBody> EventResConsumeEventMissionRewardReceive
    {
        add { AddDelegate<ConsumeEventMissionRewardReceiveResponseBody>(ClientCommandName.ConsumeEventMissionRewardReceive, value); }
        remove { DelDelegate<ConsumeEventMissionRewardReceiveResponseBody>(ClientCommandName.ConsumeEventMissionRewardReceive, value); }
    }

    // 매일소비이벤트미션보상받기
    public event Delegate<int, DailyConsumeEventMissionRewardReceiveResponseBody> EventResDailyConsumeEventMissionRewardReceive
    {
        add { AddDelegate<DailyConsumeEventMissionRewardReceiveResponseBody>(ClientCommandName.DailyConsumeEventMissionRewardReceive, value); }
        remove { DelDelegate<DailyConsumeEventMissionRewardReceiveResponseBody>(ClientCommandName.DailyConsumeEventMissionRewardReceive, value); }
    }

    #endregion ConsumeEvent

    #region AnkouTomb

    // 안쿠의무덤매칭시작
    public event Delegate<int, AnkouTombMatchingStartResponseBody> EventResAnkouTombMatchingStart
    {
        add { AddDelegate<AnkouTombMatchingStartResponseBody>(ClientCommandName.AnkouTombMatchingStart, value); }
        remove { DelDelegate<AnkouTombMatchingStartResponseBody>(ClientCommandName.AnkouTombMatchingStart, value); }
    }

    // 안쿠의무덤매칭취소
    public event Delegate<int, AnkouTombMatchingCancelResponseBody> EventResAnkouTombMatchingCancel
    {
        add { AddDelegate<AnkouTombMatchingCancelResponseBody>(ClientCommandName.AnkouTombMatchingCancel, value); }
        remove { DelDelegate<AnkouTombMatchingCancelResponseBody>(ClientCommandName.AnkouTombMatchingCancel, value); }
    }

    // 안쿠의무덤입장
    public event Delegate<int, AnkouTombEnterResponseBody> EventResAnkouTombEnter
    {
        add { AddDelegate<AnkouTombEnterResponseBody>(ClientCommandName.AnkouTombEnter, value); }
        remove { DelDelegate<AnkouTombEnterResponseBody>(ClientCommandName.AnkouTombEnter, value); }
    }

    // 안쿠의무덤퇴장
    public event Delegate<int, AnkouTombExitResponseBody> EventResAnkouTombExit
    {
        add { AddDelegate<AnkouTombExitResponseBody>(ClientCommandName.AnkouTombExit, value); }
        remove { DelDelegate<AnkouTombExitResponseBody>(ClientCommandName.AnkouTombExit, value); }
    }

    // 안쿠의무덤포기
    public event Delegate<int, AnkouTombAbandonResponseBody> EventResAnkouTombAbandon
    {
        add { AddDelegate<AnkouTombAbandonResponseBody>(ClientCommandName.AnkouTombAbandon, value); }
        remove { DelDelegate<AnkouTombAbandonResponseBody>(ClientCommandName.AnkouTombAbandon, value); }
    }

    // 안쿠의무덤부활
    public event Delegate<int, AnkouTombReviveResponseBody> EventResAnkouTombRevive
    {
        add { AddDelegate<AnkouTombReviveResponseBody>(ClientCommandName.AnkouTombRevive, value); }
        remove { DelDelegate<AnkouTombReviveResponseBody>(ClientCommandName.AnkouTombRevive, value); }
    }

    // 안쿠의무덤재화버프활성화
    public event Delegate<int, AnkouTombMoneyBuffActivateResponseBody> EventResAnkouTombMoneyBuffActivate
    {
        add { AddDelegate<AnkouTombMoneyBuffActivateResponseBody>(ClientCommandName.AnkouTombMoneyBuffActivate, value); }
        remove { DelDelegate<AnkouTombMoneyBuffActivateResponseBody>(ClientCommandName.AnkouTombMoneyBuffActivate, value); }
    }

    // 안쿠의무덤추가보상경험치받기
    public event Delegate<int, AnkouTombAdditionalRewardExpReceiveResponseBody> EventResAnkouTombAdditionalRewardExpReceive
    {
        add { AddDelegate<AnkouTombAdditionalRewardExpReceiveResponseBody>(ClientCommandName.AnkouTombAdditionalRewardExpReceive, value); }
        remove { DelDelegate<AnkouTombAdditionalRewardExpReceiveResponseBody>(ClientCommandName.AnkouTombAdditionalRewardExpReceive, value); }
    }

    #endregion AnkouTomb

    #region Constellation

    // 별자리항목활성화
    public event Delegate<int, ConstellationEntryActivateResponseBody> EventResConstellationEntryActivate
    {
        add { AddDelegate<ConstellationEntryActivateResponseBody>(ClientCommandName.ConstellationEntryActivate, value); }
        remove { DelDelegate<ConstellationEntryActivateResponseBody>(ClientCommandName.ConstellationEntryActivate, value); }
    }

    // 별자리단계개방
    public event Delegate<int, ConstellationStepOpenResponseBody> EventResConstellationStepOpen
    {
        add { AddDelegate<ConstellationStepOpenResponseBody>(ClientCommandName.ConstellationStepOpen, value); }
        remove { DelDelegate<ConstellationStepOpenResponseBody>(ClientCommandName.ConstellationStepOpen, value); }
    }

    #endregion Constellation

    #region Artifact

    // 아티팩트레벨업
    public event Delegate<int, ArtifactLevelUpResponseBody> EventResArtifactLevelUp
    {
        add { AddDelegate<ArtifactLevelUpResponseBody>(ClientCommandName.ArtifactLevelUp, value); }
        remove { DelDelegate<ArtifactLevelUpResponseBody>(ClientCommandName.ArtifactLevelUp, value); }
    }

    // 아티팩트장착
    public event Delegate<int, ArtifactEquipResponseBody> EventResArtifactEquip
    {
        add { AddDelegate<ArtifactEquipResponseBody>(ClientCommandName.ArtifactEquip, value); }
        remove { DelDelegate<ArtifactEquipResponseBody>(ClientCommandName.ArtifactEquip, value); }
    }

    #endregion Artifact

    #region TradeShip

    // 무역선탈환매칭시작
    public event Delegate<int, TradeShipMatchingStartResponseBody> EventResTradeShipMatchingStart
    {
        add { AddDelegate<TradeShipMatchingStartResponseBody>(ClientCommandName.TradeShipMatchingStart, value); }
        remove { DelDelegate<TradeShipMatchingStartResponseBody>(ClientCommandName.TradeShipMatchingStart, value); }
    }

    // 무역선탈환매칭취소
    public event Delegate<int, TradeShipMatchingCancelResponseBody> EventResTradeShipMatchingCancel
    {
        add { AddDelegate<TradeShipMatchingCancelResponseBody>(ClientCommandName.TradeShipMatchingCancel, value); }
        remove { DelDelegate<TradeShipMatchingCancelResponseBody>(ClientCommandName.TradeShipMatchingCancel, value); }
    }

    // 무역선탈환입장
    public event Delegate<int, TradeShipEnterResponseBody> EventResTradeShipEnter
    {
        add { AddDelegate<TradeShipEnterResponseBody>(ClientCommandName.TradeShipEnter, value); }
        remove { DelDelegate<TradeShipEnterResponseBody>(ClientCommandName.TradeShipEnter, value); }
    }

    // 무역선탈환퇴장
    public event Delegate<int, TradeShipExitResponseBody> EventResTradeShipExit
    {
        add { AddDelegate<TradeShipExitResponseBody>(ClientCommandName.TradeShipExit, value); }
        remove { DelDelegate<TradeShipExitResponseBody>(ClientCommandName.TradeShipExit, value); }
    }

    // 무역선탈환포기
    public event Delegate<int, TradeShipAbandonResponseBody> EventResTradeShipAbandon
    {
        add { AddDelegate<TradeShipAbandonResponseBody>(ClientCommandName.TradeShipAbandon, value); }
        remove { DelDelegate<TradeShipAbandonResponseBody>(ClientCommandName.TradeShipAbandon, value); }
    }

    // 무역선탈환부활
    public event Delegate<int, TradeShipReviveResponseBody> EventResTradeShipRevive
    {
        add { AddDelegate<TradeShipReviveResponseBody>(ClientCommandName.TradeShipRevive, value); }
        remove { DelDelegate<TradeShipReviveResponseBody>(ClientCommandName.TradeShipRevive, value); }
    }

    // 무역선탈환재화버프활성화
    public event Delegate<int, TradeShipMoneyBuffActivateResponseBody> EventResTradeShipMoneyBuffActivate
    {
        add { AddDelegate<TradeShipMoneyBuffActivateResponseBody>(ClientCommandName.TradeShipMoneyBuffActivate, value); }
        remove { DelDelegate<TradeShipMoneyBuffActivateResponseBody>(ClientCommandName.TradeShipMoneyBuffActivate, value); }
    }

    // 무역선탈환추가보상경험치받기
    public event Delegate<int, TradeShipAdditionalRewardExpReceiveResponseBody> EventResTradeShipAdditionalRewardExpReceive
    {
        add { AddDelegate<TradeShipAdditionalRewardExpReceiveResponseBody>(ClientCommandName.TradeShipAdditionalRewardExpReceive, value); }
        remove { DelDelegate<TradeShipAdditionalRewardExpReceiveResponseBody>(ClientCommandName.TradeShipAdditionalRewardExpReceive, value); }
    }

    #endregion TradeShip

    #region TeamBattlefield

    // 팀전장정보


    // 팀전장입장을위한대륙퇴장


    // 팀전장입장


    // 팀전장퇴장


    // 팀전장포기


    // 팀전장부활


    #endregion TeamBattlefield

    #endregion Command

    #region Server Event

    // 계정중복로그인
    public event Delegate<SEBAccountLoginDuplicatedEventBody> EventEvtAccountLoginDuplicated
    {
        add { AddDelegate<SEBAccountLoginDuplicatedEventBody>(ServerEventName.AccountLoginDuplicated, value); }
        remove { DelDelegate<SEBAccountLoginDuplicatedEventBody>(ServerEventName.AccountLoginDuplicated, value); }
    }

    // 신규메일
    public event Delegate<SEBNewMailEventBody> EventEvtNewMail
	{
		add { AddDelegate<SEBNewMailEventBody>(ServerEventName.NewMail, value); }
		remove { DelDelegate<SEBNewMailEventBody>(ServerEventName.NewMail, value); }
	}

	// 라크획득
	public event Delegate<SEBLakAcquisitionEventBody> EventEvtLakAcquisition
	{
		add { AddDelegate<SEBLakAcquisitionEventBody>(ServerEventName.LakAcquisition, value); }
		remove { DelDelegate<SEBLakAcquisitionEventBody>(ServerEventName.LakAcquisition, value); }
	}

    // 날짜변경
    public event Delegate<SEBDateChangedEventBody> EventEvtDateChanged
    {
        add { AddDelegate<SEBDateChangedEventBody>(ServerEventName.DateChanged, value); }
        remove { DelDelegate<SEBDateChangedEventBody>(ServerEventName.DateChanged, value); }
    }

    // 서버최고레벨갱신
    public event Delegate<SEBServerMaxLevelUpdatedEventBody> EventEvtServerMaxLevelUpdated
    {
        add { AddDelegate<SEBServerMaxLevelUpdatedEventBody>(ServerEventName.ServerMaxLevelUpdated, value); }
        remove { DelDelegate<SEBServerMaxLevelUpdatedEventBody>(ServerEventName.ServerMaxLevelUpdated, value); }
    }

    #region Event Nation

    // 밀서퀘스트대상국가변경
    public event Delegate<SEBSecretLetterQuestTargetNationChangedEventBody> EventEvtSecretLetterQuestTargetNationChanged
    {
        add { AddDelegate<SEBSecretLetterQuestTargetNationChangedEventBody>(ServerEventName.SecretLetterQuestTargetNationChanged, value); }
        remove { DelDelegate<SEBSecretLetterQuestTargetNationChangedEventBody>(ServerEventName.SecretLetterQuestTargetNationChanged, value); }
    }

    // 국가관직임명
    public event Delegate<SEBNationNoblesseAppointmentEventBody> EventEvtNationNoblesseAppointment
    {
        add { AddDelegate<SEBNationNoblesseAppointmentEventBody>(ServerEventName.NationNoblesseAppointment, value); }
        remove { DelDelegate<SEBNationNoblesseAppointmentEventBody>(ServerEventName.NationNoblesseAppointment, value); }
    }

    // 국가관직해임
    public event Delegate<SEBNationNoblesseDismissalEventBody> EventEvtNationNoblesseDismissal
    {
        add { AddDelegate<SEBNationNoblesseDismissalEventBody>(ServerEventName.NationNoblesseDismissal, value); }
        remove { DelDelegate<SEBNationNoblesseDismissalEventBody>(ServerEventName.NationNoblesseDismissal, value); }
    }

    // 국가자금변경
    public event Delegate<SEBNationFundChangedEventBody> EventEvtNationFundChanged
    {
        add { AddDelegate<SEBNationFundChangedEventBody>(ServerEventName.NationFundChanged, value); }
        remove { DelDelegate<SEBNationFundChangedEventBody>(ServerEventName.NationFundChanged, value); }
    }

    // 국가소집
    public event Delegate<SEBNationCallEventBody> EventEvtNationCall
    {
        add { AddDelegate<SEBNationCallEventBody>(ServerEventName.NationCall, value); }
        remove { DelDelegate<SEBNationCallEventBody>(ServerEventName.NationCall, value); }
    }

    #endregion Event Nation

    #region Event ReturnScroll

    // 귀환주문서사용완료
    public event Delegate<SEBReturnScrollUseFinishedEventBody> EventEvtReturnScrollUseFinished
	{
		add { AddDelegate<SEBReturnScrollUseFinishedEventBody>(ServerEventName.ReturnScrollUseFinished, value); }
		remove { DelDelegate<SEBReturnScrollUseFinishedEventBody>(ServerEventName.ReturnScrollUseFinished, value); }
	}

	// 귀환주문서사용취소
	public event Delegate<SEBReturnScrollUseCancelEventBody> EventEvtReturnScrollUseCancel
	{
		add { AddDelegate<SEBReturnScrollUseCancelEventBody>(ServerEventName.ReturnScrollUseCancel, value); }
		remove { DelDelegate<SEBReturnScrollUseCancelEventBody>(ServerEventName.ReturnScrollUseCancel, value); }
	}

	public event Delegate<SEBHeroReturnScrollUseStartEventBody> EventEvtHeroReturnScrollUseStart
	{
		add { AddDelegate<SEBHeroReturnScrollUseStartEventBody>(ServerEventName.HeroReturnScrollUseStart, value); }
		remove { DelDelegate<SEBHeroReturnScrollUseStartEventBody>(ServerEventName.HeroReturnScrollUseStart, value); }
	}

	public event Delegate<SEBHeroReturnScrollUseFinishedEventBody> EventEvtHeroReturnScrollUseFinished
	{
		add { AddDelegate<SEBHeroReturnScrollUseFinishedEventBody>(ServerEventName.HeroReturnScrollUseFinished, value); }
		remove { DelDelegate<SEBHeroReturnScrollUseFinishedEventBody>(ServerEventName.HeroReturnScrollUseFinished, value); }
	}

	public event Delegate<SEBHeroReturnScrollUseCancelEventBody> EventEvtHeroReturnScrollUseCancel
	{
		add { AddDelegate<SEBHeroReturnScrollUseCancelEventBody>(ServerEventName.HeroReturnScrollUseCancel, value); }
		remove { DelDelegate<SEBHeroReturnScrollUseCancelEventBody>(ServerEventName.HeroReturnScrollUseCancel, value); }
	}

	#endregion Event ReturnScroll

	#region Event Gear

	// 메인장비장착
	public event Delegate<SEBHeroMainGearEquipEventBody> EventEvtHeroMainGearEquip
	{
		add { AddDelegate<SEBHeroMainGearEquipEventBody>(ServerEventName.HeroMainGearEquip, value); }
		remove { DelDelegate<SEBHeroMainGearEquipEventBody>(ServerEventName.HeroMainGearEquip, value); }
	}

	// 메인장비장착해제
	public event Delegate<SEBHeroMainGearUnequipEventBody> EventEvtHeroMainGearUnequip
	{
		add { AddDelegate<SEBHeroMainGearUnequipEventBody>(ServerEventName.HeroMainGearUnequip, value); }
		remove { DelDelegate<SEBHeroMainGearUnequipEventBody>(ServerEventName.HeroMainGearUnequip, value); }
	}

    #endregion Event Gear

    #region Event Hero

    // 영웅입장
    public event Delegate<SEBHeroEnterEventBody> EventEvtHeroEnter 
	{
		add { AddDelegate<SEBHeroEnterEventBody>(ServerEventName.HeroEnter, value); }
		remove { DelDelegate<SEBHeroEnterEventBody>(ServerEventName.HeroEnter, value); }
	}

    // 영웅퇴장
    public event Delegate<SEBHeroExitEventBody> EventEvtHeroExit 
	{
		add { AddDelegate<SEBHeroExitEventBody>(ServerEventName.HeroExit, value); }
		remove { DelDelegate<SEBHeroExitEventBody>(ServerEventName.HeroExit, value); }
	}

    // 영웅관심영역입장
    public event Delegate<SEBHeroInterestAreaEnterEventBody> EventEvtHeroInterestAreaEnter
	{
		add { AddDelegate<SEBHeroInterestAreaEnterEventBody>(ServerEventName.HeroInterestAreaEnter, value); }
		remove { DelDelegate<SEBHeroInterestAreaEnterEventBody>(ServerEventName.HeroInterestAreaEnter, value); }
	}

    // 영웅관심영역퇴장
    public event Delegate<SEBHeroInterestAreaExitEventBody> EventEvtHeroInterestAreaExit
	{
		add { AddDelegate<SEBHeroInterestAreaExitEventBody>(ServerEventName.HeroInterestAreaExit, value); }
		remove { DelDelegate<SEBHeroInterestAreaExitEventBody>(ServerEventName.HeroInterestAreaExit, value); }
	}

    // 관심대상변경
    public event Delegate<SEBInterestTargetChangeEventBody> EventEvtHeroInterestTargetChange 
	{
		add { AddDelegate<SEBInterestTargetChangeEventBody>(ServerEventName.InterestTargetChange, value); }
		remove { DelDelegate<SEBInterestTargetChangeEventBody>(ServerEventName.InterestTargetChange, value); }
	}

    // 영웅이동
    public event Delegate<SEBHeroMoveEventBody> EventEvtHeroMove
	{
		add { AddDelegate<SEBHeroMoveEventBody>(ServerEventName.HeroMove, value); }
		remove { DelDelegate<SEBHeroMoveEventBody>(ServerEventName.HeroMove, value); }
	}

    // 영웅이동모드변경
    public event Delegate<SEBHeroMoveModeChangedEventBody> EventEvtHeroMoveModeChanged
    {
        add { AddDelegate<SEBHeroMoveModeChangedEventBody>(ServerEventName.HeroMoveModeChanged, value); }
        remove { DelDelegate<SEBHeroMoveModeChangedEventBody>(ServerEventName.HeroMoveModeChanged, value); }
    }

    //영웅스킬시전
	public event Delegate<SEBHeroSkillCastEventBody> EventEvtHeroSkillCast
	{
		add { AddDelegate<SEBHeroSkillCastEventBody>(ServerEventName.HeroSkillCast, value); }
		remove { DelDelegate<SEBHeroSkillCastEventBody>(ServerEventName.HeroSkillCast, value); }
	}

    //스킬시전결과
	public event Delegate<SEBSkillCastResultEventBody> EventEvtSkillCastResult
	{
		add { AddDelegate<SEBSkillCastResultEventBody>(ServerEventName.SkillCastResult, value); }
		remove { DelDelegate<SEBSkillCastResultEventBody>(ServerEventName.SkillCastResult, value); }
	}

    //영웅적중
	public event Delegate<SEBHeroHitEventBody> EventEvtHeroHit
	{
		add { AddDelegate<SEBHeroHitEventBody>(ServerEventName.HeroHit, value); }
		remove { DelDelegate<SEBHeroHitEventBody>(ServerEventName.HeroHit, value); }
	}

    //전투모드시작
	public event Delegate<SEBBattleModeStartEventBody> EventEvtBattleModeStart
	{
		add { AddDelegate<SEBBattleModeStartEventBody>(ServerEventName.BattleModeStart, value); }
		remove { DelDelegate<SEBBattleModeStartEventBody>(ServerEventName.BattleModeStart, value); }
	}

    //전투모드종료
	public event Delegate<SEBBattleModeEndEventBody> EventEvtBattleModeEnd
	{
		add { AddDelegate<SEBBattleModeEndEventBody>(ServerEventName.BattleModeEnd, value); }
		remove { DelDelegate<SEBBattleModeEndEventBody>(ServerEventName.BattleModeEnd, value); }
	}

    //영웅전투모드시작
    public event Delegate<SEBHeroBattleModeStartEventBody> EventEvtHeroBattleModeStart
    {
        add { AddDelegate<SEBHeroBattleModeStartEventBody>(ServerEventName.HeroBattleModeStart, value); }
        remove { DelDelegate<SEBHeroBattleModeStartEventBody>(ServerEventName.HeroBattleModeStart, value); }
    }

    //영웅전투모드종료
    public event Delegate<SEBHeroBattleModeEndEventBody> EventEvtHeroBattleModeEnd
    {
        add { AddDelegate<SEBHeroBattleModeEndEventBody>(ServerEventName.HeroBattleModeEnd, value); }
        remove { DelDelegate<SEBHeroBattleModeEndEventBody>(ServerEventName.HeroBattleModeEnd, value); }
    }

    //경험치획득
    public event Delegate<SEBExpAcquisitionEventBody> EventEvtExpAcquisition
	{
		add { AddDelegate<SEBExpAcquisitionEventBody>(ServerEventName.ExpAcquisition, value); }
		remove { DelDelegate<SEBExpAcquisitionEventBody>(ServerEventName.ExpAcquisition, value); }
	}

    //영웅레벨업
	public event Delegate<SEBHeroLevelUpEventBody> EventEvtHeroLevelUp
	{
		add { AddDelegate<SEBHeroLevelUpEventBody>(ServerEventName.HeroLevelUp, value); }
		remove { DelDelegate<SEBHeroLevelUpEventBody>(ServerEventName.HeroLevelUp, value); }
	}

    //영웅부활
	public event Delegate<SEBHeroRevivedEventBody> EventEvtHeroRevived
	{
		add { AddDelegate<SEBHeroRevivedEventBody>(ServerEventName.HeroRevived, value); }
		remove { DelDelegate<SEBHeroRevivedEventBody>(ServerEventName.HeroRevived, value); }
	}

    //부활무적취소
    public event Delegate<SEBRevivalInvincibilityCanceledEventBody> EventEvtRevivalInvincibilityCanceled
    {
        add { AddDelegate<SEBRevivalInvincibilityCanceledEventBody>(ServerEventName.HeroRevived, value); }
        remove { DelDelegate<SEBRevivalInvincibilityCanceledEventBody>(ServerEventName.HeroRevived, value); }
    }

    //영웅부활무적취소


    //영웅상태이상효과시작
    public event Delegate<SEBHeroAbnormalStateEffectStartEventBody> EventEvtHeroAbnormalStateEffectStart
	{
		add { AddDelegate<SEBHeroAbnormalStateEffectStartEventBody>(ServerEventName.HeroAbnormalStateEffectStart, value); }
		remove { DelDelegate<SEBHeroAbnormalStateEffectStartEventBody>(ServerEventName.HeroAbnormalStateEffectStart, value); }
	}

    //영웅상태이상효과종료
    public event Delegate<SEBHeroAbnormalStateEffectFinishedEventBody> EventEvtHeroAbnormalStateEffectFinished
    {
        add { AddDelegate<SEBHeroAbnormalStateEffectFinishedEventBody>(ServerEventName.HeroAbnormalStateEffectFinished, value); }
        remove { DelDelegate<SEBHeroAbnormalStateEffectFinishedEventBody>(ServerEventName.HeroAbnormalStateEffectFinished, value); }
    }

    //영웅상태이상효과적중
    public event Delegate<SEBHeroAbnormalStateEffectHitEventBody> EventEvtHeroAbnormalStateEffectHit
	{
		add { AddDelegate<SEBHeroAbnormalStateEffectHitEventBody>(ServerEventName.HeroAbnormalStateEffectHit, value); }
		remove { DelDelegate<SEBHeroAbnormalStateEffectHitEventBody>(ServerEventName.HeroAbnormalStateEffectHit, value); }
	}

    //드랍오브젝트루팅
    public event Delegate<SEBDropObjectLootedEventBody> EventEvtDropObjectLooted
    {
        add { AddDelegate<SEBDropObjectLootedEventBody>(ServerEventName.DropObjectLooted, value); }
        remove { DelDelegate<SEBDropObjectLootedEventBody>(ServerEventName.DropObjectLooted, value); }
    }

    //영웅VIP레벨변경
    public event Delegate<SEBHeroVipLevelChangedEventBody> EventEvtHeroVipLevelChanged
    {
        add { AddDelegate<SEBHeroVipLevelChangedEventBody>(ServerEventName.HeroVipLevelChanged, value); }
        remove { DelDelegate<SEBHeroVipLevelChangedEventBody>(ServerEventName.HeroVipLevelChanged, value); }
    }

    //영웅최대HP변경
    public event Delegate<SEBHeroMaxHpChangedEventBody> EventEvtHeroMaxHpChanged
    {
        add { AddDelegate<SEBHeroMaxHpChangedEventBody>(ServerEventName.HeroMaxHpChanged, value); }
        remove { DelDelegate<SEBHeroMaxHpChangedEventBody>(ServerEventName.HeroMaxHpChanged, value); }
    }

    //최대HP변경
    public event Delegate<SEBMaxHpChangedEventBody> EventEvtMaxHpChanged
    {
        add { AddDelegate<SEBMaxHpChangedEventBody>(ServerEventName.MaxHpChanged, value); }
        remove { DelDelegate<SEBMaxHpChangedEventBody>(ServerEventName.MaxHpChanged, value); }
    }

    //영웅날개변경
    public event Delegate<SEBHeroEquippedWingChangedEventBody> EventEvtHeroEquippedWingChanged
    {
        add { AddDelegate<SEBHeroEquippedWingChangedEventBody>(ServerEventName.HeroEquippedWingChanged, value); }
        remove { DelDelegate<SEBHeroEquippedWingChangedEventBody>(ServerEventName.HeroEquippedWingChanged, value); }
    }

    //스태미나자동회복
    public event Delegate<SEBStaminaAutoRecoveryEventBody> EventEvtStaminaAutoRecovery
    {
        add { AddDelegate<SEBStaminaAutoRecoveryEventBody>(ServerEventName.StaminaAutoRecovery, value); }
        remove { DelDelegate<SEBStaminaAutoRecoveryEventBody>(ServerEventName.StaminaAutoRecovery, value); }
    }

    //스태미나스케쥴회복
    public event Delegate<SEBStaminaScheduleRecoveryEventBody> EventEvtStaminaScheduleRecovery
    {
        add { AddDelegate<SEBStaminaScheduleRecoveryEventBody>(ServerEventName.StaminaScheduleRecovery, value); }
        remove { DelDelegate<SEBStaminaScheduleRecoveryEventBody>(ServerEventName.StaminaScheduleRecovery, value); }
    }

    //영웅HP회복
    public event Delegate<SEBHeroHpRestoredEventBody> EventEvtHeroHpRestored
    {
        add { AddDelegate<SEBHeroHpRestoredEventBody>(ServerEventName.HeroHpRestored, value); }
        remove { DelDelegate<SEBHeroHpRestoredEventBody>(ServerEventName.HeroHpRestored, value); }
    }

    //날개획득
    public event Delegate<SEBWingAcquisitionEventBody> EventEvtWingAcquisition
    {
        add { AddDelegate<SEBWingAcquisitionEventBody>(ServerEventName.WingAcquisition, value); }
        remove { DelDelegate<SEBWingAcquisitionEventBody>(ServerEventName.WingAcquisition, value); }
    }

    //영웅직업공통스킬시전
    public event Delegate<SEBHeroJobCommonSkillCastEventBody> EventEvtHeroJobCommonSkillCast
    {
        add { AddDelegate<SEBHeroJobCommonSkillCastEventBody>(ServerEventName.HeroJobCommonSkillCast, value); }
        remove { DelDelegate<SEBHeroJobCommonSkillCastEventBody>(ServerEventName.HeroJobCommonSkillCast, value); }
    }

    //가속시작
    public event Delegate<SEBAccelerationStartedEventBody> EventEvtAccelerationStarted
    {
        add { AddDelegate<SEBAccelerationStartedEventBody>(ServerEventName.AccelerationStarted, value); }
        remove { DelDelegate<SEBAccelerationStartedEventBody>(ServerEventName.AccelerationStarted, value); }
    }

    //영웅가속시작
    public event Delegate<SEBHeroAccelerationStartedEventBody> EventEvtHeroAccelerationStarted
    {
        add { AddDelegate<SEBHeroAccelerationStartedEventBody>(ServerEventName.HeroAccelerationStarted, value); }
        remove { DelDelegate<SEBHeroAccelerationStartedEventBody>(ServerEventName.HeroAccelerationStarted, value); }
    }

    //영웅가속종료
    public event Delegate<SEBHeroAccelerationEndedEventBody> EventEvtHeroAccelerationEnded
    {
        add { AddDelegate<SEBHeroAccelerationEndedEventBody>(ServerEventName.HeroAccelerationEnded, value); }
        remove { DelDelegate<SEBHeroAccelerationEndedEventBody>(ServerEventName.HeroAccelerationEnded, value); }
    }

    #endregion Event Hero

    #region Event Monster

    public event Delegate<SEBMonsterInterestAreaEnterEventBody> EventEvtMonsterInterestAreaEnter
	{
		add { AddDelegate<SEBMonsterInterestAreaEnterEventBody>(ServerEventName.MonsterInterestAreaEnter, value); }
		remove { DelDelegate<SEBMonsterInterestAreaEnterEventBody>(ServerEventName.MonsterInterestAreaEnter, value); }
	}

	public event Delegate<SEBMonsterInterestAreaExitEventBody> EventEvtMonsterInterestAreaExit
	{
		add { AddDelegate<SEBMonsterInterestAreaExitEventBody>(ServerEventName.MonsterInterestAreaExit, value); }
		remove { DelDelegate<SEBMonsterInterestAreaExitEventBody>(ServerEventName.MonsterInterestAreaExit, value); }
	}

	public event Delegate<SEBMonsterMoveEventBody> EventEvtMonsterMove
	{
		add { AddDelegate<SEBMonsterMoveEventBody>(ServerEventName.MonsterMove, value); }
		remove { DelDelegate<SEBMonsterMoveEventBody>(ServerEventName.MonsterMove, value); }
	}

	public event Delegate<SEBMonsterOwnershipChangeEventBody> EventEvtMonsterOwnershipChange
	{
		add { AddDelegate<SEBMonsterOwnershipChangeEventBody>(ServerEventName.MonsterOwnershipChange, value); }
		remove { DelDelegate<SEBMonsterOwnershipChangeEventBody>(ServerEventName.MonsterOwnershipChange, value); }
	}

	public event Delegate<SEBMonsterSpawnEventBody> EventEvtMonsterSpawn
	{
		add { AddDelegate<SEBMonsterSpawnEventBody>(ServerEventName.MonsterSpawn, value); }
		remove { DelDelegate<SEBMonsterSpawnEventBody>(ServerEventName.MonsterSpawn, value); }
	}

	public event Delegate<SEBMonsterHitEventBody> EventEvtMonsterHit
	{
		add { AddDelegate<SEBMonsterHitEventBody>(ServerEventName.MonsterHit, value); }
		remove { DelDelegate<SEBMonsterHitEventBody>(ServerEventName.MonsterHit, value); }
	}

	public event Delegate<SEBMonsterMentalHitEventBody> EventEvtMonsterMentalHit
	{
		add { AddDelegate<SEBMonsterMentalHitEventBody>(ServerEventName.MonsterMentalHit, value); }
		remove { DelDelegate<SEBMonsterMentalHitEventBody>(ServerEventName.MonsterMentalHit, value); }
	}

	public event Delegate<SEBMonsterSkillCastEventBody> EventEvtMonsterSkillCast
	{
		add { AddDelegate<SEBMonsterSkillCastEventBody>(ServerEventName.MonsterSkillCast, value); }
		remove { DelDelegate<SEBMonsterSkillCastEventBody>(ServerEventName.MonsterSkillCast, value); }
	}

	public event Delegate<SEBMonsterAbnormalStateEffectStartEventBody> EventEvtMonsterAbnormalStateEffectStart
	{
		add { AddDelegate<SEBMonsterAbnormalStateEffectStartEventBody>(ServerEventName.MonsterAbnormalStateEffectStart, value); }
		remove { DelDelegate<SEBMonsterAbnormalStateEffectStartEventBody>(ServerEventName.MonsterAbnormalStateEffectStart, value); }
	}

	public event Delegate<SEBMonsterAbnormalStateEffectHitEventBody> EventEvtMonsterAbnormalStateEffectHit
	{
		add { AddDelegate<SEBMonsterAbnormalStateEffectHitEventBody>(ServerEventName.MonsterAbnormalStateEffectHit, value); }
		remove { DelDelegate<SEBMonsterAbnormalStateEffectHitEventBody>(ServerEventName.MonsterAbnormalStateEffectHit, value); }
	}

	public event Delegate<SEBMonsterAbnormalStateEffectFinishedEventBody> EventEvtMonsterAbnormalStateEffectFinished
	{
		add { AddDelegate<SEBMonsterAbnormalStateEffectFinishedEventBody>(ServerEventName.MonsterAbnormalStateEffectFinished, value); }
		remove { DelDelegate<SEBMonsterAbnormalStateEffectFinishedEventBody>(ServerEventName.MonsterAbnormalStateEffectFinished, value); }
	}

    public event Delegate<SEBMonsterReturnModeChangedEventBody> EventEvtMonsterReturnModeChanged
    {
        add { AddDelegate<SEBMonsterReturnModeChangedEventBody>(ServerEventName.MonsterReturnModeChanged, value); }
        remove { DelDelegate<SEBMonsterReturnModeChangedEventBody>(ServerEventName.MonsterReturnModeChanged, value); }
    }

    #endregion Event Monster

	#region Event Quest

    // 메인퀘스트갱신
	public event Delegate<SEBMainQuestUpdatedEventBody> EventEvtMainQuestUpdated
	{
		add { AddDelegate<SEBMainQuestUpdatedEventBody>(ServerEventName.MainQuestUpdated, value); }
		remove { DelDelegate<SEBMainQuestUpdatedEventBody>(ServerEventName.MainQuestUpdated, value); }
	}

	public event Delegate<SEBHeroContinentObjectInteractionStartEventBody> EventEvtHeroContinentObjectInteractionStart
	{
		add { AddDelegate<SEBHeroContinentObjectInteractionStartEventBody>(ServerEventName.HeroContinentObjectInteractionStart, value); }
		remove { DelDelegate<SEBHeroContinentObjectInteractionStartEventBody>(ServerEventName.HeroContinentObjectInteractionStart, value); }
	}

	public event Delegate<SEBHeroContinentObjectInteractionFinishedEventBody> EventEvtHeroContinentObjectInteractionFinished
	{
		add { AddDelegate<SEBHeroContinentObjectInteractionFinishedEventBody>(ServerEventName.HeroContinentObjectInteractionFinished, value); }
		remove { DelDelegate<SEBHeroContinentObjectInteractionFinishedEventBody>(ServerEventName.HeroContinentObjectInteractionFinished, value); }
	}

	public event Delegate<SEBHeroContinentObjectInteractionCancelEventBody> EventEvtHeroContinentObjectInteractionCancel
	{
		add { AddDelegate<SEBHeroContinentObjectInteractionCancelEventBody>(ServerEventName.HeroContinentObjectInteractionCancel, value); }
		remove { DelDelegate<SEBHeroContinentObjectInteractionCancelEventBody>(ServerEventName.HeroContinentObjectInteractionCancel, value); }
	}

	public event Delegate<SEBContinentObjectCreatedEventBody> EventEvtContinentObjectCreated
	{
		add { AddDelegate<SEBContinentObjectCreatedEventBody>(ServerEventName.ContinentObjectCreated, value); }
		remove { DelDelegate<SEBContinentObjectCreatedEventBody>(ServerEventName.ContinentObjectCreated, value); }
	}

    public event Delegate<SEBContinentBanishedEventBody> EventEvtContinentBanished
    {
        add { AddDelegate<SEBContinentBanishedEventBody>(ServerEventName.ContinentBanished, value); }
        remove { DelDelegate<SEBContinentBanishedEventBody>(ServerEventName.ContinentBanished, value); }
    }

    public event Delegate<SEBMonsterRemovedEventBody> EventEvtMonsterRemoved
    {
        add { AddDelegate<SEBMonsterRemovedEventBody>(ServerEventName.MonsterRemoved, value); }
        remove { DelDelegate<SEBMonsterRemovedEventBody>(ServerEventName.MonsterRemoved, value); }
    }

    // 메인퀘스트몬스터변신취소
    public event Delegate<SEBMainQuestMonsterTransformationCanceledEventBody> EventEvtMainQuestMonsterTransformationCanceled
    {
        add { AddDelegate<SEBMainQuestMonsterTransformationCanceledEventBody>(ServerEventName.MainQuestMonsterTransformationCanceled, value); }
        remove { DelDelegate<SEBMainQuestMonsterTransformationCanceledEventBody>(ServerEventName.MainQuestMonsterTransformationCanceled, value); }
    }

    // 메인퀘스트몬스터변신종료
    public event Delegate<SEBMainQuestMonsterTransformationFinishedEventBody> EventEvtMainQuestMonsterTransformationFinished
    {
        add { AddDelegate<SEBMainQuestMonsterTransformationFinishedEventBody>(ServerEventName.MainQuestMonsterTransformationFinished, value); }
        remove { DelDelegate<SEBMainQuestMonsterTransformationFinishedEventBody>(ServerEventName.MainQuestMonsterTransformationFinished, value); }
    }

    // 영웅메인퀘스트몬스터변신시작
    public event Delegate<SEBHeroMainQuestMonsterTransformationStartedEventBody> EventEvtHeroMainQuestMonsterTransformationStarted
    {
        add { AddDelegate<SEBHeroMainQuestMonsterTransformationStartedEventBody>(ServerEventName.HeroMainQuestMonsterTransformationStarted, value); }
        remove { DelDelegate<SEBHeroMainQuestMonsterTransformationStartedEventBody>(ServerEventName.HeroMainQuestMonsterTransformationStarted, value); }
    }

    // 영웅메인퀘스트몬스터변신취소
    public event Delegate<SEBHeroMainQuestMonsterTransformationCanceledEventBody> EventEvtHeroMainQuestMonsterTransformationCanceled
    {
        add { AddDelegate<SEBHeroMainQuestMonsterTransformationCanceledEventBody>(ServerEventName.HeroMainQuestMonsterTransformationCanceled, value); }
        remove { DelDelegate<SEBHeroMainQuestMonsterTransformationCanceledEventBody>(ServerEventName.HeroMainQuestMonsterTransformationCanceled, value); }
    }

    // 영웅메인퀘스트몬스터변신종료
    public event Delegate<SEBHeroMainQuestMonsterTransformationFinishedEventBody> EventEvtHeroMainQuestMonsterTransformationFinished
    {
        add { AddDelegate<SEBHeroMainQuestMonsterTransformationFinishedEventBody>(ServerEventName.HeroMainQuestMonsterTransformationFinished, value); }
        remove { DelDelegate<SEBHeroMainQuestMonsterTransformationFinishedEventBody>(ServerEventName.HeroMainQuestMonsterTransformationFinished, value); }
    }

    // 영웅메인퀘스트변신몬스터스킬시전
    public event Delegate<SEBHeroMainQuestTransformationMonsterSkillCastEventBody> EventEvtHeroMainQuestTransformationMonsterSkillCast
    {
        add { AddDelegate<SEBHeroMainQuestTransformationMonsterSkillCastEventBody>(ServerEventName.HeroMainQuestTransformationMonsterSkillCast, value); }
        remove { DelDelegate<SEBHeroMainQuestTransformationMonsterSkillCastEventBody>(ServerEventName.HeroMainQuestTransformationMonsterSkillCast, value); }
    }

    #endregion Event Quest

    #region Event Party

    // 파티신청도착
    public event Delegate<SEBPartyApplicationArrivedEventBody> EventEvtPartyApplicationArrived
	{
		add { AddDelegate<SEBPartyApplicationArrivedEventBody>(ServerEventName.PartyApplicationArrived, value); }
		remove { DelDelegate<SEBPartyApplicationArrivedEventBody>(ServerEventName.PartyApplicationArrived, value); }
	}

	// 파티신청취소
	public event Delegate<SEBPartyApplicationCanceledEventBody> EventEvtPartyApplicationCanceled
	{
		add { AddDelegate<SEBPartyApplicationCanceledEventBody>(ServerEventName.PartyApplicationCanceled, value); }
		remove { DelDelegate<SEBPartyApplicationCanceledEventBody>(ServerEventName.PartyApplicationCanceled, value); }
	}

	// 파티신청수락
	public event Delegate<SEBPartyApplicationAcceptedEventBody> EventEvtPartyApplicationAccepted
	{
		add { AddDelegate<SEBPartyApplicationAcceptedEventBody>(ServerEventName.PartyApplicationAccepted, value); }
		remove { DelDelegate<SEBPartyApplicationAcceptedEventBody>(ServerEventName.PartyApplicationAccepted, value); }
	}

	// 파티신청거절
	public event Delegate<SEBPartyApplicationRefusedEventBody> EventEvtPartyApplicationRefused
	{
		add { AddDelegate<SEBPartyApplicationRefusedEventBody>(ServerEventName.PartyApplicationRefused, value); }
		remove { DelDelegate<SEBPartyApplicationRefusedEventBody>(ServerEventName.PartyApplicationRefused, value); }
	}

	// 파티신청수명종료
	public event Delegate<SEBPartyApplicationLifetimeEndedEventBody> EventEvtPartyApplicationLifetimeEnded
	{
		add { AddDelegate<SEBPartyApplicationLifetimeEndedEventBody>(ServerEventName.PartyApplicationLifetimeEnded, value); }
		remove { DelDelegate<SEBPartyApplicationLifetimeEndedEventBody>(ServerEventName.PartyApplicationLifetimeEnded, value); }
	}



	// 파티초대도착
	public event Delegate<SEBPartyInvitationArrivedEventBody> EventEvtPartyInvitationArrived
	{
		add { AddDelegate<SEBPartyInvitationArrivedEventBody>(ServerEventName.PartyInvitationArrived, value); }
		remove { DelDelegate<SEBPartyInvitationArrivedEventBody>(ServerEventName.PartyInvitationArrived, value); }
	}

	// 파티초대취소
	public event Delegate<SEBPartyInvitationCanceledEventBody> EventEvtPartyInvitationCanceled
	{
		add { AddDelegate<SEBPartyInvitationCanceledEventBody>(ServerEventName.PartyInvitationCanceled, value); }
		remove { DelDelegate<SEBPartyInvitationCanceledEventBody>(ServerEventName.PartyInvitationCanceled, value); }
	}

	// 파티초대수락
	public event Delegate<SEBPartyInvitationAcceptedEventBody> EventEvtPartyInvitationAccepted
	{
		add { AddDelegate<SEBPartyInvitationAcceptedEventBody>(ServerEventName.PartyInvitationAccepted, value); }
		remove { DelDelegate<SEBPartyInvitationAcceptedEventBody>(ServerEventName.PartyInvitationAccepted, value); }
	}

	// 파티초대거절
	public event Delegate<SEBPartyInvitationRefusedEventBody> EventEvtPartyInvitationRefused
	{
		add { AddDelegate<SEBPartyInvitationRefusedEventBody>(ServerEventName.PartyInvitationRefused, value); }
		remove { DelDelegate<SEBPartyInvitationRefusedEventBody>(ServerEventName.PartyInvitationRefused, value); }
	}

	// 파티초대수명종료
	public event Delegate<SEBPartyInvitationLifetimeEndedEventBody> EventEvtPartyInvitationLifetimeEnded
	{
		add { AddDelegate<SEBPartyInvitationLifetimeEndedEventBody>(ServerEventName.PartyInvitationLifetimeEnded, value); }
		remove { DelDelegate<SEBPartyInvitationLifetimeEndedEventBody>(ServerEventName.PartyInvitationLifetimeEnded, value); }
	}

	// 파티멤버입장
	public event Delegate<SEBPartyMemberEnterEventBody> EventEvtPartyMemberEnter
	{
		add { AddDelegate<SEBPartyMemberEnterEventBody>(ServerEventName.PartyMemberEnter, value); }
		remove { DelDelegate<SEBPartyMemberEnterEventBody>(ServerEventName.PartyMemberEnter, value); }
	}

	// 파티멤버퇴장
	public event Delegate<SEBPartyMemberExitEventBody> EventEvtPartyMemberExit
	{
		add { AddDelegate<SEBPartyMemberExitEventBody>(ServerEventName.PartyMemberExit, value); }
		remove { DelDelegate<SEBPartyMemberExitEventBody>(ServerEventName.PartyMemberExit, value); }
	}

	// 파티강퇴
	public event Delegate<SEBPartyBanishedEventBody> EventEvtPartyBanished
	{
		add { AddDelegate<SEBPartyBanishedEventBody>(ServerEventName.PartyBanished, value); }
		remove { DelDelegate<SEBPartyBanishedEventBody>(ServerEventName.PartyBanished, value); }
	}

	// 파티장변경
	public event Delegate<SEBPartyMasterChangedEventBody> EventEvtPartyMasterChanged
	{
		add { AddDelegate<SEBPartyMasterChangedEventBody>(ServerEventName.PartyMasterChanged, value); }
		remove { DelDelegate<SEBPartyMasterChangedEventBody>(ServerEventName.PartyMasterChanged, value); }
	}

	// 파티소집
	public event Delegate<SEBPartyCallEventBody> EventEvtPartyCall
	{
		add { AddDelegate<SEBPartyCallEventBody>(ServerEventName.PartyCall, value); }
		remove { DelDelegate<SEBPartyCallEventBody>(ServerEventName.PartyCall, value); }
	}

	// 파티해산
	public event Delegate<SEBPartyDisbandedEventBody> EventEvtPartyDisbanded
	{
		add { AddDelegate<SEBPartyDisbandedEventBody>(ServerEventName.PartyDisbanded, value); }
		remove { DelDelegate<SEBPartyDisbandedEventBody>(ServerEventName.PartyDisbanded, value); }
	}

	// 파티멤버갱신
	public event Delegate<SEBPartyMembersUpdatedEventBody> EventEvtPartyMembersUpdated
	{
		add { AddDelegate<SEBPartyMembersUpdatedEventBody>(ServerEventName.PartyMembersUpdated, value); }
		remove { DelDelegate<SEBPartyMembersUpdatedEventBody>(ServerEventName.PartyMembersUpdated, value); }
	}

    #endregion Event Party

    #region Event Chatting

    // 채팅메시지수신
    public event Delegate<SEBChattingMessageReceivedEventBody> EventEvtChattingMessageReceived
    {
        add { AddDelegate<SEBChattingMessageReceivedEventBody>(ServerEventName.ChattingMessageReceived, value); }
        remove { DelDelegate<SEBChattingMessageReceivedEventBody>(ServerEventName.ChattingMessageReceived, value); }
    }

    #endregion

    #region Event Mount

    // 영웅탈것타기
    public event Delegate<SEBHeroMountGetOnEventBody> EventEvtHeroMountGetOn
    {
        add { AddDelegate<SEBHeroMountGetOnEventBody>(ServerEventName.HeroMountGetOn, value); }
        remove { DelDelegate<SEBHeroMountGetOnEventBody>(ServerEventName.HeroMountGetOn, value); }
    }

    // 영웅탈것내리기
    public event Delegate<SEBHeroMountGetOffEventBody> EventEvtHeroMountGetOff
    {
        add { AddDelegate<SEBHeroMountGetOffEventBody>(ServerEventName.HeroMountGetOff, value); }
        remove { DelDelegate<SEBHeroMountGetOffEventBody>(ServerEventName.HeroMountGetOff, value); }
    }

    // 영웅탈것레벨업
    public event Delegate<SEBHeroMountLevelUpEventBody> EventEvtHeroMountLevelUp
    {
        add { AddDelegate<SEBHeroMountLevelUpEventBody>(ServerEventName.HeroMountLevelUp, value); }
        remove { DelDelegate<SEBHeroMountLevelUpEventBody>(ServerEventName.HeroMountLevelUp, value); }
    }


    #endregion Event Mount

    #region Event MainQuestDungeon

    // 메인퀘스트던전단계시작
    public event Delegate<SEBMainQuestDungeonStepStartEventBody> EventEvtMainQuestDungeonStepStart
    {
        add { AddDelegate<SEBMainQuestDungeonStepStartEventBody>(ServerEventName.MainQuestDungeonStepStart, value); }
        remove { DelDelegate<SEBMainQuestDungeonStepStartEventBody>(ServerEventName.MainQuestDungeonStepStart, value); }
    }

    // 메인퀘스트던전단계완료
    public event Delegate<SEBMainQuestDungeonStepCompletedEventBody> EventEvtMainQuestDungeonStepCompleted
    {
        add { AddDelegate<SEBMainQuestDungeonStepCompletedEventBody>(ServerEventName.MainQuestDungeonStepCompleted, value); }
        remove { DelDelegate<SEBMainQuestDungeonStepCompletedEventBody>(ServerEventName.MainQuestDungeonStepCompleted, value); }
    }

    // 메인퀘스트던전실패
    public event Delegate<SEBMainQuestDungeonFailEventBody> EventEvtMainQuestDungeonFail
    {
        add { AddDelegate<SEBMainQuestDungeonFailEventBody>(ServerEventName.MainQuestDungeonFail, value); }
        remove { DelDelegate<SEBMainQuestDungeonFailEventBody>(ServerEventName.MainQuestDungeonFail, value); }
    }

    // 메인퀘스트던전클리어
    public event Delegate<SEBMainQuestDungeonClearEventBody> EventEvtMainQuestDungeonClear
    {
        add { AddDelegate<SEBMainQuestDungeonClearEventBody>(ServerEventName.MainQuestDungeonClear, value); }
        remove { DelDelegate<SEBMainQuestDungeonClearEventBody>(ServerEventName.MainQuestDungeonClear, value); }
    }

    // 메인퀘스트던전강퇴
    public event Delegate<SEBMainQuestDungeonBanishedEventBody> EventEvtMainQuestDungeonBanished
	{
        add { AddDelegate<SEBMainQuestDungeonBanishedEventBody>(ServerEventName.MainQuestDungeonBanished, value); }
        remove { DelDelegate<SEBMainQuestDungeonBanishedEventBody>(ServerEventName.MainQuestDungeonBanished, value); }
    }

    // 메인퀘스트던전몬스터소환
    public event Delegate<SEBMainQuestDungeonMonsterSummonEventBody> EventEvtMainQuestDungeonMonsterSummon
    {
        add { AddDelegate<SEBMainQuestDungeonMonsterSummonEventBody>(ServerEventName.MainQuestDungeonMonsterSummon, value); }
        remove { DelDelegate<SEBMainQuestDungeonMonsterSummonEventBody>(ServerEventName.MainQuestDungeonMonsterSummon, value); }
    }

    #endregion Event MainQuestDungeon

    #region Event StoryDungeon

    //스토리던전단계시작
    public event Delegate<SEBStoryDungeonStepStartEventBody> EventEvtStoryDungeonStepStart
    {
        add { AddDelegate<SEBStoryDungeonStepStartEventBody>(ServerEventName.StoryDungeonStepStart, value); }
        remove { DelDelegate<SEBStoryDungeonStepStartEventBody>(ServerEventName.StoryDungeonStepStart, value); }
    }

    //스토리던전클리어
    public event Delegate<SEBStoryDungeonClearEventBody> EventEvtStoryDungeonClear
    {
        add { AddDelegate<SEBStoryDungeonClearEventBody>(ServerEventName.StoryDungeonClear, value); }
        remove { DelDelegate<SEBStoryDungeonClearEventBody>(ServerEventName.StoryDungeonClear, value); }
    }

    //스토리던전실패
    public event Delegate<SEBStoryDungeonFailEventBody> EventEvtStoryDungeonFail
    {
        add { AddDelegate<SEBStoryDungeonFailEventBody>(ServerEventName.StoryDungeonFail, value); }
        remove { DelDelegate<SEBStoryDungeonFailEventBody>(ServerEventName.StoryDungeonFail, value); }
    }

    //스토리던전강퇴
    public event Delegate<SEBStoryDungeonBanishedEventBody> EventEvtStoryDungeonBanished
    {
        add { AddDelegate<SEBStoryDungeonBanishedEventBody>(ServerEventName.StoryDungeonBanished, value); }
        remove { DelDelegate<SEBStoryDungeonBanishedEventBody>(ServerEventName.StoryDungeonBanished, value); }
    }

    //스토리던전함정시전
    public event Delegate<SEBStoryDungeonTrapCastEventBody> EventEvtStoryDungeonTrapCast
    {
		add { AddDelegate<SEBStoryDungeonTrapCastEventBody>(ServerEventName.StoryDungeonTrapCast, value); }
		remove { DelDelegate<SEBStoryDungeonTrapCastEventBody>(ServerEventName.StoryDungeonTrapCast, value); }
	}

    //스토리던전함정적중
    public event Delegate<SEBStoryDungeonTrapHitEventBody> EventEvtStoryDungeonTrapHit
    {
        add { AddDelegate<SEBStoryDungeonTrapHitEventBody>(ServerEventName.StoryDungeonTrapHit, value); }
        remove { DelDelegate<SEBStoryDungeonTrapHitEventBody>(ServerEventName.StoryDungeonTrapHit, value); }
    }

    #endregion Event StoryDungeon

    #region Event ExpDungeon

    //경험치던전웨이브시작
    public event Delegate<SEBExpDungeonWaveStartEventBody> EventEvtExpDungeonWaveStart
    {
        add { AddDelegate<SEBExpDungeonWaveStartEventBody>(ServerEventName.ExpDungeonWaveStart, value); }
        remove { DelDelegate<SEBExpDungeonWaveStartEventBody>(ServerEventName.ExpDungeonWaveStart, value); }
    }

    //경험치던전웨이브완료
    public event Delegate<SEBExpDungeonWaveCompletedEventBody> EventEvtExpDungeonWaveCompleted
    {
        add { AddDelegate<SEBExpDungeonWaveCompletedEventBody>(ServerEventName.ExpDungeonWaveCompleted, value); }
        remove { DelDelegate<SEBExpDungeonWaveCompletedEventBody>(ServerEventName.ExpDungeonWaveCompleted, value); }
    }

    //경험치던전웨이브타임아웃
    public event Delegate<SEBExpDungeonWaveTimeoutEventBody> EventEvtExpDungeonWaveTimeout
    {
        add { AddDelegate<SEBExpDungeonWaveTimeoutEventBody>(ServerEventName.ExpDungeonWaveTimeout, value); }
        remove { DelDelegate<SEBExpDungeonWaveTimeoutEventBody>(ServerEventName.ExpDungeonWaveTimeout, value); }
    }

    //경험치던전클리어
    public event Delegate<SEBExpDungeonClearEventBody> EventEvtExpDungeonClear
    {
        add { AddDelegate<SEBExpDungeonClearEventBody>(ServerEventName.ExpDungeonClear, value); }
        remove { DelDelegate<SEBExpDungeonClearEventBody>(ServerEventName.ExpDungeonClear, value); }
    }

    //경험치던전강퇴
    public event Delegate<SEBExpDungeonBanishedEventBody> EventEvtExpDungeonBanished
    {
        add { AddDelegate<SEBExpDungeonBanishedEventBody>(ServerEventName.ExpDungeonBanished, value); }
        remove { DelDelegate<SEBExpDungeonBanishedEventBody>(ServerEventName.ExpDungeonBanished, value); }
    }

    #endregion Event ExpDungeon

    #region Event GoldDungeon

    //골드던전스텝시작
    public event Delegate<SEBGoldDungeonStepStartEventBody> EventEvtGoldDungeonStepStart
    {
        add { AddDelegate<SEBGoldDungeonStepStartEventBody>(ServerEventName.GoldDungeonStepStart, value); }
        remove { DelDelegate<SEBGoldDungeonStepStartEventBody>(ServerEventName.GoldDungeonStepStart, value); }
    }

    //골드던전스텝완료
    public event Delegate<SEBGoldDungeonStepCompletedEventBody> EventEvtGoldDungeonStepCompleted
    {
        add { AddDelegate<SEBGoldDungeonStepCompletedEventBody>(ServerEventName.GoldDungeonStepCompleted, value); }
        remove { DelDelegate<SEBGoldDungeonStepCompletedEventBody>(ServerEventName.GoldDungeonStepCompleted, value); }
    }

    //골드던전웨이브시작
    public event Delegate<SEBGoldDungeonWaveStartEventBody> EventEvtGoldDungeonWaveStart
    {
        add { AddDelegate<SEBGoldDungeonWaveStartEventBody>(ServerEventName.GoldDungeonWaveStart, value); }
        remove { DelDelegate<SEBGoldDungeonWaveStartEventBody>(ServerEventName.GoldDungeonWaveStart, value); }
    }

    //골드던전웨이브완료
    public event Delegate<SEBGoldDungeonWaveCompletedEventBody> EventEvtGoldDungeonWaveCompleted
    {
        add { AddDelegate<SEBGoldDungeonWaveCompletedEventBody>(ServerEventName.GoldDungeonWaveCompleted, value); }
        remove { DelDelegate<SEBGoldDungeonWaveCompletedEventBody>(ServerEventName.GoldDungeonWaveCompleted, value); }
    }

    //골드던전웨이브타임아웃
    public event Delegate<SEBGoldDungeonWaveTimeoutEventBody> EventEvtGoldDungeonWaveTimeout
    {
        add { AddDelegate<SEBGoldDungeonWaveTimeoutEventBody>(ServerEventName.GoldDungeonWaveTimeout, value); }
        remove { DelDelegate<SEBGoldDungeonWaveTimeoutEventBody>(ServerEventName.GoldDungeonWaveTimeout, value); }
    }

    //골드던전클리어
    public event Delegate<SEBGoldDungeonClearEventBody> EventEvtGoldDungeonClear
    {
        add { AddDelegate<SEBGoldDungeonClearEventBody>(ServerEventName.GoldDungeonClear, value); }
        remove { DelDelegate<SEBGoldDungeonClearEventBody>(ServerEventName.GoldDungeonClear, value); }
    }

    //골드던전실패
    public event Delegate<SEBGoldDungeonFailEventBody> EventEvtGoldDungeonFail
    {
        add { AddDelegate<SEBGoldDungeonFailEventBody>(ServerEventName.GoldDungeonFail, value); }
        remove { DelDelegate<SEBGoldDungeonFailEventBody>(ServerEventName.GoldDungeonFail, value); }
    }

    //골드던전강퇴
    public event Delegate<SEBGoldDungeonBanishedEventBody> EventEvtGoldDungeonBanished
    {
        add { AddDelegate<SEBGoldDungeonBanishedEventBody>(ServerEventName.GoldDungeonBanished, value); }
        remove { DelDelegate<SEBGoldDungeonBanishedEventBody>(ServerEventName.GoldDungeonBanished, value); }
    }

    #endregion Event GoldDungeon

    #region Event TreatOfFarm

    //농장의위협퀘스트미션완료
    public event Delegate<SEBTreatOfFarmQuestMissionCompleteEventBody> EventEvtTreatOfFarmQuestMissionComplete
    {
        add { AddDelegate<SEBTreatOfFarmQuestMissionCompleteEventBody>(ServerEventName.TreatOfFarmQuestMissionComplete, value); }
        remove { DelDelegate<SEBTreatOfFarmQuestMissionCompleteEventBody>(ServerEventName.TreatOfFarmQuestMissionComplete, value); }
    }

    //농장의위협퀘스트미션실패
    public event Delegate<SEBTreatOfFarmQuestMissionFailEventBody> EventEvtTreatOfFarmQuestMissionFail
    {
        add { AddDelegate<SEBTreatOfFarmQuestMissionFailEventBody>(ServerEventName.TreatOfFarmQuestMissionFail, value); }
        remove { DelDelegate<SEBTreatOfFarmQuestMissionFailEventBody>(ServerEventName.TreatOfFarmQuestMissionFail, value); }
    }

    //농장의위협퀘스트미션몬스터스폰
    public event Delegate<SEBTreatOfFarmQuestMissionMonsterSpawnedEventBody> EventEvtTreatOfFarmQuestMissionMonsterSpawned
    {
        add { AddDelegate<SEBTreatOfFarmQuestMissionMonsterSpawnedEventBody>(ServerEventName.TreatOfFarmQuestMissionMonsterSpawned, value); }
        remove { DelDelegate<SEBTreatOfFarmQuestMissionMonsterSpawnedEventBody>(ServerEventName.TreatOfFarmQuestMissionMonsterSpawned, value); }
    }

    #endregion Event TreatOfFarm

    #region Event UndergroundMaze

    //지하미로강퇴
    public event Delegate<SEBUndergroundMazeBanishedEventBody> EventEvtUndergroundMazeBanished
    {
        add { AddDelegate<SEBUndergroundMazeBanishedEventBody>(ServerEventName.UndergroundMazeBanished, value); }
        remove { DelDelegate<SEBUndergroundMazeBanishedEventBody>(ServerEventName.UndergroundMazeBanished, value); }
    }

    #endregion Event UndergroundMaze

    #region Event BountyHunterQuest

    //현상금사냥꾼퀘스트갱신
    public event Delegate<SEBBountyHunterQuestUpdatedEventBody> EventEvtBountyHunterQuestUpdated
    {
        add { AddDelegate<SEBBountyHunterQuestUpdatedEventBody>(ServerEventName.BountyHunterQuestUpdated, value); }
        remove { DelDelegate<SEBBountyHunterQuestUpdatedEventBody>(ServerEventName.BountyHunterQuestUpdated, value); }
    }

    #endregion Event BountyHunterQuest

    #region Event Fishing

    //낚시캐스팅완료
    public event Delegate<SEBFishingCastingCompletedEventBody> EventEvtFishingCastingCompleted
    {
        add { AddDelegate<SEBFishingCastingCompletedEventBody>(ServerEventName.FishingCastingCompleted, value); }
        remove { DelDelegate<SEBFishingCastingCompletedEventBody>(ServerEventName.FishingCastingCompleted, value); }
    }

    //낚시취소
    public event Delegate<SEBFishingCanceledEventBody> EventEvtFishingCanceled
    {
        add { AddDelegate<SEBFishingCanceledEventBody>(ServerEventName.FishingCanceled, value); }
        remove { DelDelegate<SEBFishingCanceledEventBody>(ServerEventName.FishingCanceled, value); }
    }

    //영웅낚시시작
    public event Delegate<SEBHeroFishingStartedEventBody> EventEvtHeroFishingStarted
    {
        add { AddDelegate<SEBHeroFishingStartedEventBody>(ServerEventName.HeroFishingStarted, value); }
        remove { DelDelegate<SEBHeroFishingStartedEventBody>(ServerEventName.HeroFishingStarted, value); }
    }

    //영웅낚시완료
    public event Delegate<SEBHeroFishingCompletedEventBody> EventEvtHeroFishingCompleted
    {
        add { AddDelegate<SEBHeroFishingCompletedEventBody>(ServerEventName.HeroFishingCompleted, value); }
        remove { DelDelegate<SEBHeroFishingCompletedEventBody>(ServerEventName.HeroFishingCompleted, value); }
    }

    //영웅낚시취소
    public event Delegate<SEBHeroFishingCanceledEventBody> EventEvtHeroFishingCanceled
    {
        add { AddDelegate<SEBHeroFishingCanceledEventBody>(ServerEventName.HeroFishingCanceled, value); }
        remove { DelDelegate<SEBHeroFishingCanceledEventBody>(ServerEventName.HeroFishingCanceled, value); }
    }

    #endregion Event Fishing

    #region Event ArtifactRoom

    //고대유물의방시작
    public event Delegate<SEBArtifactRoomStartEventBody> EventEvtArtifactRoomStart
    {
        add { AddDelegate<SEBArtifactRoomStartEventBody>(ServerEventName.ArtifactRoomStart, value); }
        remove { DelDelegate<SEBArtifactRoomStartEventBody>(ServerEventName.ArtifactRoomStart, value); }
    }

    //고대유물의방완료
    public event Delegate<SEBArtifactRoomClearEventBody> EventEvtArtifactRoomClear
    {
        add { AddDelegate<SEBArtifactRoomClearEventBody>(ServerEventName.ArtifactRoomClear, value); }
        remove { DelDelegate<SEBArtifactRoomClearEventBody>(ServerEventName.ArtifactRoomClear, value); }
    }

    //고대유물의방실패
    public event Delegate<SEBArtifactRoomFailEventBody> EventEvtArtifactRoomFail
    {
        add { AddDelegate<SEBArtifactRoomFailEventBody>(ServerEventName.ArtifactRoomFail, value); }
        remove { DelDelegate<SEBArtifactRoomFailEventBody>(ServerEventName.ArtifactRoomFail, value); }
    }

    //고대유물의방강퇴
    public event Delegate<SEBArtifactRoomBanishedEventBody> EventEvtArtifactRoomBanished
    {
        add { AddDelegate<SEBArtifactRoomBanishedEventBody>(ServerEventName.ArtifactRoomBanished, value); }
        remove { DelDelegate<SEBArtifactRoomBanishedEventBody>(ServerEventName.ArtifactRoomBanished, value); }
    }

    //다음층도전에대한고대유물의방강퇴
    public event Delegate<SEBArtifactRoomBanishedForNextFloorChallengeEventBody> EventEvtArtifactRoomBanishedForNextFloorChallenge
    {
        add { AddDelegate<SEBArtifactRoomBanishedForNextFloorChallengeEventBody>(ServerEventName.ArtifactRoomBanishedForNextFloorChallenge, value); }
        remove { DelDelegate<SEBArtifactRoomBanishedForNextFloorChallengeEventBody>(ServerEventName.ArtifactRoomBanishedForNextFloorChallenge, value); }
    }

    //고대유물의방소탕다음층소탕시작
    public event Delegate<SEBArtifactRoomSweepNextFloorStartEventBody> EventEvtArtifactRoomSweepNextFloorStart
    {
        add { AddDelegate<SEBArtifactRoomSweepNextFloorStartEventBody>(ServerEventName.ArtifactRoomSweepNextFloorStart, value); }
        remove { DelDelegate<SEBArtifactRoomSweepNextFloorStartEventBody>(ServerEventName.ArtifactRoomSweepNextFloorStart, value); }
    }

    //고대유물의방소탕완료
    public event Delegate<SEBArtifactRoomSweepCompletedEventBody> EventEvtArtifactRoomSweepCompleted
    {
        add { AddDelegate<SEBArtifactRoomSweepCompletedEventBody>(ServerEventName.ArtifactRoomSweepCompleted, value); }
        remove { DelDelegate<SEBArtifactRoomSweepCompletedEventBody>(ServerEventName.ArtifactRoomSweepCompleted, value); }
    }

    #endregion Event ArtifactRoom

    #region Event MysteryBox

    //의문의상자뽑기완료
    public event Delegate<SEBMysteryBoxPickCompletedEventBody> EventEvtMysteryBoxPickCompleted
    {
        add { AddDelegate<SEBMysteryBoxPickCompletedEventBody>(ServerEventName.MysteryBoxPickCompleted, value); }
        remove { DelDelegate<SEBMysteryBoxPickCompletedEventBody>(ServerEventName.MysteryBoxPickCompleted, value); }
    }

    //의문의상자뽑기취소
    public event Delegate<SEBMysteryBoxPickCanceledEventBody> EventEvtMysteryBoxPickCanceled
    {
        add { AddDelegate<SEBMysteryBoxPickCanceledEventBody>(ServerEventName.MysteryBoxPickCanceled, value); }
        remove { DelDelegate<SEBMysteryBoxPickCanceledEventBody>(ServerEventName.MysteryBoxPickCanceled, value); }
    }

    //영웅의문의상자뽑기시작
    public event Delegate<SEBHeroMysteryBoxPickStartedEventBody> EventEvtHeroMysteryBoxPickStarted
    {
        add { AddDelegate<SEBHeroMysteryBoxPickStartedEventBody>(ServerEventName.HeroMysteryBoxPickStarted, value); }
        remove { DelDelegate<SEBHeroMysteryBoxPickStartedEventBody>(ServerEventName.HeroMysteryBoxPickStarted, value); }
    }

    //영웅의문의상자뽑기완료
    public event Delegate<SEBHeroMysteryBoxPickCompletedEventBody> EventEvtHeroMysteryBoxPickCompleted
    {
        add { AddDelegate<SEBHeroMysteryBoxPickCompletedEventBody>(ServerEventName.HeroMysteryBoxPickCompleted, value); }
        remove { DelDelegate<SEBHeroMysteryBoxPickCompletedEventBody>(ServerEventName.HeroMysteryBoxPickCompleted, value); }
    }

    //영웅의문의상자뽑기취소
    public event Delegate<SEBHeroMysteryBoxPickCanceledEventBody> EventEvtHeroMysteryBoxPickCanceled
    {
        add { AddDelegate<SEBHeroMysteryBoxPickCanceledEventBody>(ServerEventName.HeroMysteryBoxPickCanceled, value); }
        remove { DelDelegate<SEBHeroMysteryBoxPickCanceledEventBody>(ServerEventName.HeroMysteryBoxPickCanceled, value); }
    }

    //영웅의문의상자퀘스트완료
    public event Delegate<SEBHeroMysteryBoxQuestCompletedEventBody> EventEvtHeroMysteryBoxQuestCompleted
    {
        add { AddDelegate<SEBHeroMysteryBoxQuestCompletedEventBody>(ServerEventName.HeroMysteryBoxQuestCompleted, value); }
        remove { DelDelegate<SEBHeroMysteryBoxQuestCompletedEventBody>(ServerEventName.HeroMysteryBoxQuestCompleted, value); }
    }

    #endregion Event MysteryBox

    #region Event SecretLetter

    //밀서뽑기완료
    public event Delegate<SEBSecretLetterPickCompletedEventBody> EventEvtSecretLetterPickCompleted
    {
        add { AddDelegate<SEBSecretLetterPickCompletedEventBody>(ServerEventName.SecretLetterPickCompleted, value); }
        remove { DelDelegate<SEBSecretLetterPickCompletedEventBody>(ServerEventName.SecretLetterPickCompleted, value); }
    }

    //밀서뽑기취소
    public event Delegate<SEBSecretLetterPickCanceledEventBody> EventEvtSecretLetterPickCanceled
    {
        add { AddDelegate<SEBSecretLetterPickCanceledEventBody>(ServerEventName.SecretLetterPickCanceled, value); }
        remove { DelDelegate<SEBSecretLetterPickCanceledEventBody>(ServerEventName.SecretLetterPickCanceled, value); }
    }

    //영웅밀서뽑기시작
    public event Delegate<SEBHeroSecretLetterPickStartedEventBody> EventEvtHeroSecretLetterPickStarted
    {
        add { AddDelegate<SEBHeroSecretLetterPickStartedEventBody>(ServerEventName.HeroSecretLetterPickStarted, value); }
        remove { DelDelegate<SEBHeroSecretLetterPickStartedEventBody>(ServerEventName.HeroSecretLetterPickStarted, value); }
    }

    //영웅밀서뽑기완료
    public event Delegate<SEBHeroSecretLetterPickCompletedEventBody> EventEvtHeroSecretLetterPickCompleted
    {
        add { AddDelegate<SEBHeroSecretLetterPickCompletedEventBody>(ServerEventName.HeroSecretLetterPickCompleted, value); }
        remove { DelDelegate<SEBHeroSecretLetterPickCompletedEventBody>(ServerEventName.HeroSecretLetterPickCompleted, value); }
    }

    //영웅밀서뽑기취소
    public event Delegate<SEBHeroSecretLetterPickCanceledEventBody> EventEvtHeroSecretLetterPickCanceled
    {
        add { AddDelegate<SEBHeroSecretLetterPickCanceledEventBody>(ServerEventName.HeroSecretLetterPickCanceled, value); }
        remove { DelDelegate<SEBHeroSecretLetterPickCanceledEventBody>(ServerEventName.HeroSecretLetterPickCanceled, value); }
    }

    //영웅밀서퀘스트완료
    public event Delegate<SEBHeroSecretLetterQuestCompletedEventBody> EventEvtHeroSecretLetterQuestCompleted
    {
        add { AddDelegate<SEBHeroSecretLetterQuestCompletedEventBody>(ServerEventName.HeroSecretLetterQuestCompleted, value); }
        remove { DelDelegate<SEBHeroSecretLetterQuestCompletedEventBody>(ServerEventName.HeroSecretLetterQuestCompleted, value); }
    }

    #endregion Event SecretLetter

    #region Event DimensionRaidQuest

    //차원습격상호작용완료
    public event Delegate<SEBDimensionRaidInteractionCompletedEventBody> EventEvtDimensionRaidInteractionCompleted
    {
        add { AddDelegate<SEBDimensionRaidInteractionCompletedEventBody>(ServerEventName.DimensionRaidInteractionCompleted, value); }
        remove { DelDelegate<SEBDimensionRaidInteractionCompletedEventBody>(ServerEventName.DimensionRaidInteractionCompleted, value); }
    }

    //차원습격상호작용취소
    public event Delegate<SEBDimensionRaidInteractionCanceledEventBody> EventEvtDimensionRaidInteractionCanceled
    {
        add { AddDelegate<SEBDimensionRaidInteractionCanceledEventBody>(ServerEventName.DimensionRaidInteractionCanceled, value); }
        remove { DelDelegate<SEBDimensionRaidInteractionCanceledEventBody>(ServerEventName.DimensionRaidInteractionCanceled, value); }
    }

    //차원습격상호작용시작
    public event Delegate<SEBHeroDimensionRaidInteractionStartedEventBody> EventEvtHeroDimensionRaidInteractionStarted
    {
        add { AddDelegate<SEBHeroDimensionRaidInteractionStartedEventBody>(ServerEventName.HeroDimensionRaidInteractionStarted, value); }
        remove { DelDelegate<SEBHeroDimensionRaidInteractionStartedEventBody>(ServerEventName.HeroDimensionRaidInteractionStarted, value); }
    }

    //차원습격상호작용완료
    public event Delegate<SEBHeroDimensionRaidInteractionCompletedEventBody> EventEvtHeroDimensionRaidInteractionCompleted
    {
        add { AddDelegate<SEBHeroDimensionRaidInteractionCompletedEventBody>(ServerEventName.HeroDimensionRaidInteractionCompleted, value); }
        remove { DelDelegate<SEBHeroDimensionRaidInteractionCompletedEventBody>(ServerEventName.HeroDimensionRaidInteractionCompleted, value); }
    }

    //차원습격상호작용취소
    public event Delegate<SEBHeroDimensionRaidInteractionCanceledEventBody> EventEvtHeroDimensionRaidInteractionCanceled
    {
        add { AddDelegate<SEBHeroDimensionRaidInteractionCanceledEventBody>(ServerEventName.HeroDimensionRaidInteractionCanceled, value); }
        remove { DelDelegate<SEBHeroDimensionRaidInteractionCanceledEventBody>(ServerEventName.HeroDimensionRaidInteractionCanceled, value); }
    }

    #endregion Event DimensionRaidQuest

    #region Event Reward

    //연속미션갱신
    public event Delegate<SEBSeriesMissionUpdatedEventBody> EventEvtSeriesMissionUpdated
    {
        add { AddDelegate<SEBSeriesMissionUpdatedEventBody>(ServerEventName.SeriesMissionUpdated, value); }
        remove { DelDelegate<SEBSeriesMissionUpdatedEventBody>(ServerEventName.SeriesMissionUpdated, value); }
    }

    //오늘의미션목록변경
    public event Delegate<SEBTodayMissionListChangedEventBody> EventEvtTodayMissionListChanged
    {
        add { AddDelegate<SEBTodayMissionListChangedEventBody>(ServerEventName.TodayMissionListChanged, value); }
        remove { DelDelegate<SEBTodayMissionListChangedEventBody>(ServerEventName.TodayMissionListChanged, value); }
    }

    //오늘의미션갱신
    public event Delegate<SEBTodayMissionUpdatedEventBody> EventEvtTodayMissionUpdated
    {
        add { AddDelegate<SEBTodayMissionUpdatedEventBody>(ServerEventName.TodayMissionUpdated, value); }
        remove { DelDelegate<SEBTodayMissionUpdatedEventBody>(ServerEventName.TodayMissionUpdated, value); }
    }

    #endregion Event Reward

    #region Event AncientRelic

    //고대인유적매칭상태변경
    public event Delegate<SEBAncientRelicMatchingStatusChangedEventBody> EventEvtAncientRelicMatchingStatusChanged
    {
        add { AddDelegate<SEBAncientRelicMatchingStatusChangedEventBody>(ServerEventName.AncientRelicMatchingStatusChanged, value); }
        remove { DelDelegate<SEBAncientRelicMatchingStatusChangedEventBody>(ServerEventName.AncientRelicMatchingStatusChanged, value); }
    }

    //고대인유적매칭방강퇴
    public event Delegate<SEBAncientRelicMatchingRoomBanishedEventBody> EventEvtAncientRelicMatchingRoomBanished
    {
        add { AddDelegate<SEBAncientRelicMatchingRoomBanishedEventBody>(ServerEventName.AncientRelicMatchingRoomBanished, value); }
        remove { DelDelegate<SEBAncientRelicMatchingRoomBanishedEventBody>(ServerEventName.AncientRelicMatchingRoomBanished, value); }
    }

    //고대인유적매칭방파티입장
    public event Delegate<SEBAncientRelicMatchingRoomPartyEnterEventBody> EventEvtAncientRelicMatchingRoomPartyEnter
    {
        add { AddDelegate<SEBAncientRelicMatchingRoomPartyEnterEventBody>(ServerEventName.AncientRelicMatchingRoomPartyEnter, value); }
        remove { DelDelegate<SEBAncientRelicMatchingRoomPartyEnterEventBody>(ServerEventName.AncientRelicMatchingRoomPartyEnter, value); }
    }

    //고대인유적입장에대한대륙퇴장
    public event Delegate<SEBContinentExitForAncientRelicEnterEventBody> EventEvtContinentExitForAncientRelicEnter
    {
        add { AddDelegate<SEBContinentExitForAncientRelicEnterEventBody>(ServerEventName.ContinentExitForAncientRelicEnter, value); }
        remove { DelDelegate<SEBContinentExitForAncientRelicEnterEventBody>(ServerEventName.ContinentExitForAncientRelicEnter, value); }
    }

    //고대인유적단계시작
    public event Delegate<SEBAncientRelicStepStartEventBody> EventEvtAncientRelicStepStart
    {
        add { AddDelegate<SEBAncientRelicStepStartEventBody>(ServerEventName.AncientRelicStepStart, value); }
        remove { DelDelegate<SEBAncientRelicStepStartEventBody>(ServerEventName.AncientRelicStepStart, value); }
    }

    //고대인유적단계완료
    public event Delegate<SEBAncientRelicStepCompletedEventBody> EventEvtAncientRelicStepCompleted
    {
        add { AddDelegate<SEBAncientRelicStepCompletedEventBody>(ServerEventName.AncientRelicStepCompleted, value); }
        remove { DelDelegate<SEBAncientRelicStepCompletedEventBody>(ServerEventName.AncientRelicStepCompleted, value); }
    }

    //고대인유적웨이브시작
    public event Delegate<SEBAncientRelicWaveStartEventBody> EventEvtAncientRelicWaveStart
    {
        add { AddDelegate<SEBAncientRelicWaveStartEventBody>(ServerEventName.AncientRelicWaveStart, value); }
        remove { DelDelegate<SEBAncientRelicWaveStartEventBody>(ServerEventName.AncientRelicWaveStart, value); }
    }

    //고대인유적완료
    public event Delegate<SEBAncientRelicClearEventBody> EventEvtAncientRelicClear
    {
        add { AddDelegate<SEBAncientRelicClearEventBody>(ServerEventName.AncientRelicClear, value); }
        remove { DelDelegate<SEBAncientRelicClearEventBody>(ServerEventName.AncientRelicClear, value); }
    }

    //고대인유적실패
    public event Delegate<SEBAncientRelicFailEventBody> EventEvtAncientRelicFail
    {
        add { AddDelegate<SEBAncientRelicFailEventBody>(ServerEventName.AncientRelicFail, value); }
        remove { DelDelegate<SEBAncientRelicFailEventBody>(ServerEventName.AncientRelicFail, value); }
    }

    //고대인유적강퇴
    public event Delegate<SEBAncientRelicBanishedEventBody> EventEvtAncientRelicBanished
    {
        add { AddDelegate<SEBAncientRelicBanishedEventBody>(ServerEventName.AncientRelicBanished, value); }
        remove { DelDelegate<SEBAncientRelicBanishedEventBody>(ServerEventName.AncientRelicBanished, value); }
    }

    //고대인유적포인트갱신
    public event Delegate<SEBAncientRelicPointUpdatedEventBody> EventEvtAncientRelicPointUpdated
    {
        add { AddDelegate<SEBAncientRelicPointUpdatedEventBody>(ServerEventName.AncientRelicPointUpdated, value); }
        remove { DelDelegate<SEBAncientRelicPointUpdatedEventBody>(ServerEventName.AncientRelicPointUpdated, value); }
    }

    //고대인의유적함정활성
    public event Delegate<SEBAncientRelicTrapActivatedEventBody> EventEvtAncientRelicTrapActivated
    {
        add { AddDelegate<SEBAncientRelicTrapActivatedEventBody>(ServerEventName.AncientRelicTrapActivated, value); }
        remove { DelDelegate<SEBAncientRelicTrapActivatedEventBody>(ServerEventName.AncientRelicTrapActivated, value); }
    }

    //고대인유적함정적중
    public event Delegate<SEBAncientRelicTrapHitEventBody> EventEvtAncientRelicTrapHit
    {
        add { AddDelegate<SEBAncientRelicTrapHitEventBody>(ServerEventName.AncientRelicTrapHit, value); }
        remove { DelDelegate<SEBAncientRelicTrapHitEventBody>(ServerEventName.AncientRelicTrapHit, value); }
    }

    //고대인유적함정효과종료
    public event Delegate<SEBAncientRelicTrapEffectFinishedEventBody> EventEvtAncientRelicTrapEffectFinished
    {
        add { AddDelegate<SEBAncientRelicTrapEffectFinishedEventBody>(ServerEventName.AncientRelicTrapEffectFinished, value); }
        remove { DelDelegate<SEBAncientRelicTrapEffectFinishedEventBody>(ServerEventName.AncientRelicTrapEffectFinished, value); }
    }

    #endregion Event AncientRelic

    #region Event Distortion

    //왜곡취소
    public event Delegate<SEBDistortionCanceledEventBody> EventEvtDistortionCanceled
    {
        add { AddDelegate<SEBDistortionCanceledEventBody>(ServerEventName.DistortionCanceled, value); }
        remove { DelDelegate<SEBDistortionCanceledEventBody>(ServerEventName.DistortionCanceled, value); }
    }

    //영웅왜곡시작
    public event Delegate<SEBHeroDistortionStartedEventBody> EventEvtHeroDistortionStarted
    {
        add { AddDelegate<SEBHeroDistortionStartedEventBody>(ServerEventName.HeroDistortionStarted, value); }
        remove { DelDelegate<SEBHeroDistortionStartedEventBody>(ServerEventName.HeroDistortionStarted, value); }
    }

    //영웅왜곡종료
    public event Delegate<SEBHeroDistortionFinishedEventBody> EventEvtHeroDistortionFinished
    {
        add { AddDelegate<SEBHeroDistortionFinishedEventBody>(ServerEventName.HeroDistortionFinished, value); }
        remove { DelDelegate<SEBHeroDistortionFinishedEventBody>(ServerEventName.HeroDistortionFinished, value); }
    }

    //영웅왜곡취소
    public event Delegate<SEBHeroDistortionCanceledEventBody> EventEvtHeroDistortionCanceled
    {
        add { AddDelegate<SEBHeroDistortionCanceledEventBody>(ServerEventName.HeroDistortionCanceled, value); }
        remove { DelDelegate<SEBHeroDistortionCanceledEventBody>(ServerEventName.HeroDistortionCanceled, value); }
    }

    #endregion Event Distortio

    #region Event Today

    //오늘의미션갱신
    public event Delegate<SEBTodayTaskUpdatedEventBody> EventEvtTodayTaskUpdated
    {
        add { AddDelegate<SEBTodayTaskUpdatedEventBody>(ServerEventName.TodayTaskUpdated, value); }
        remove { DelDelegate<SEBTodayTaskUpdatedEventBody>(ServerEventName.TodayTaskUpdated, value); }
    }

    #endregion Event Today

    #region Event HolyWarQuest

    //위대한성전퀘스트갱신
    public event Delegate<SEBHolyWarQuestUpdatedEventBody> EventEvtHolyWarQuestUpdated
    {
        add { AddDelegate<SEBHolyWarQuestUpdatedEventBody>(ServerEventName.HolyWarQuestUpdated, value); }
        remove { DelDelegate<SEBHolyWarQuestUpdatedEventBody>(ServerEventName.HolyWarQuestUpdated, value); }
    }

    #endregion Event HolyWarQuest

    #region Event PVP

    //Pvp킬
    public event Delegate<SEBPvpKillEventBody> EventEvtPvpKill
    {
        add { AddDelegate<SEBPvpKillEventBody>(ServerEventName.PvpKill, value); }
        remove { DelDelegate<SEBPvpKillEventBody>(ServerEventName.PvpKill, value); }
    }

    //Pvp도움
    public event Delegate<SEBPvpAssistEventBody> EventEvtPvpAssist
    {
        add { AddDelegate<SEBPvpAssistEventBody>(ServerEventName.PvpAssist, value); }
        remove { DelDelegate<SEBPvpAssistEventBody>(ServerEventName.PvpAssist, value); }
    }

    #endregion Event PVP

    #region Event Rank

    //영웅계급획득
    public event Delegate<SEBHeroRankAcquiredEventBody> EventEvtHeroRankAcquired
    {
        add { AddDelegate<SEBHeroRankAcquiredEventBody>(ServerEventName.HeroRankAcquired, value); }
        remove { DelDelegate<SEBHeroRankAcquiredEventBody>(ServerEventName.HeroRankAcquired, value); }
    }

    //영웅계급액티브스킬시전
    public event Delegate<SEBHeroRankActiveSkillCastEventBody> EventEvtHeroRankActiveSkillCast
    {
        add { AddDelegate<SEBHeroRankActiveSkillCastEventBody>(ServerEventName.HeroRankActiveSkillCast, value); }
        remove { DelDelegate<SEBHeroRankActiveSkillCastEventBody>(ServerEventName.HeroRankActiveSkillCast, value); }
    }

    #endregion Event Rank

    #region Event Cart

    //카트입장
    public event Delegate<SEBCartEnterEventBody> EventEvtCartEnter
    {
        add { AddDelegate<SEBCartEnterEventBody>(ServerEventName.CartEnter, value); }
        remove { DelDelegate<SEBCartEnterEventBody>(ServerEventName.CartEnter, value); }
    }

    //카트퇴장
    public event Delegate<SEBCartExitEventBody> EventEvtCartExit
    {
        add { AddDelegate<SEBCartExitEventBody>(ServerEventName.CartExit, value); }
        remove { DelDelegate<SEBCartExitEventBody>(ServerEventName.CartExit, value); }
    }

    //카트타기
    public event Delegate<SEBCartGetOnEventBody> EventEvtCartGetOn
    {
        add { AddDelegate<SEBCartGetOnEventBody>(ServerEventName.CartGetOn, value); }
        remove { DelDelegate<SEBCartGetOnEventBody>(ServerEventName.CartGetOn, value); }
    }

    //카트내리기
    public event Delegate<SEBCartGetOffEventBody> EventEvtCartGetOff
    {
        add { AddDelegate<SEBCartGetOffEventBody>(ServerEventName.CartGetOff, value); }
        remove { DelDelegate<SEBCartGetOffEventBody>(ServerEventName.CartGetOff, value); }
    }

    //카트고속주행시작
    public event Delegate<SEBCartHighSpeedStartEventBody> EventEvtCartHighSpeedStart
    {
        add { AddDelegate<SEBCartHighSpeedStartEventBody>(ServerEventName.CartHighSpeedStart, value); }
        remove { DelDelegate<SEBCartHighSpeedStartEventBody>(ServerEventName.CartHighSpeedStart, value); }
    }

    //카트고속주행종료
    public event Delegate<SEBCartHighSpeedEndEventBody> EventEvtCartHighSpeedEnd
    {
        add { AddDelegate<SEBCartHighSpeedEndEventBody>(ServerEventName.CartHighSpeedEnd, value); }
        remove { DelDelegate<SEBCartHighSpeedEndEventBody>(ServerEventName.CartHighSpeedEnd, value); }
    }

    //내카트고속주행종료
    public event Delegate<SEBMyCartHighSpeedEndEventBody> EventEvtMyCartHighSpeedEnd
    {
        add { AddDelegate<SEBMyCartHighSpeedEndEventBody>(ServerEventName.MyCartHighSpeedEnd, value); }
        remove { DelDelegate<SEBMyCartHighSpeedEndEventBody>(ServerEventName.MyCartHighSpeedEnd, value); }
    }

    //카트관심영역입장
    public event Delegate<SEBCartInterestAreaEnterEventBody> EventEvtCartInterestAreaEnter
    {
        add { AddDelegate<SEBCartInterestAreaEnterEventBody>(ServerEventName.CartInterestAreaEnter, value); }
        remove { DelDelegate<SEBCartInterestAreaEnterEventBody>(ServerEventName.CartInterestAreaEnter, value); }
    }

    //카트관심영역퇴장
    public event Delegate<SEBCartInterestAreaExitEventBody> EventEvtCartInterestAreaExit
    {
        add { AddDelegate<SEBCartInterestAreaExitEventBody>(ServerEventName.CartInterestAreaExit, value); }
        remove { DelDelegate<SEBCartInterestAreaExitEventBody>(ServerEventName.CartInterestAreaExit, value); }
    }

    //카트이동
    public event Delegate<SEBCartMoveEventBody> EventEvtCartMove
    {
        add { AddDelegate<SEBCartMoveEventBody>(ServerEventName.CartMove, value); }
        remove { DelDelegate<SEBCartMoveEventBody>(ServerEventName.CartMove, value); }
    }

    //카트변경
    public event Delegate<SEBCartChangedEventBody> EventEvtCartChanged
    {
        add { AddDelegate<SEBCartChangedEventBody>(ServerEventName.CartChanged, value); }
        remove { DelDelegate<SEBCartChangedEventBody>(ServerEventName.CartChanged, value); }
    }

    //카트적중
    public event Delegate<SEBCartHitEventBody> EventEvtCartHit
    {
        add { AddDelegate<SEBCartHitEventBody>(ServerEventName.CartHit, value); }
        remove { DelDelegate<SEBCartHitEventBody>(ServerEventName.CartHit, value); }
    }

    //카트상태이상효과시작
    public event Delegate<SEBCartAbnormalStateEffectStartEventBody> EventEvtCartAbnormalStateEffectStart
    {
        add { AddDelegate<SEBCartAbnormalStateEffectStartEventBody>(ServerEventName.CartAbnormalStateEffectStart, value); }
        remove { DelDelegate<SEBCartAbnormalStateEffectStartEventBody>(ServerEventName.CartAbnormalStateEffectStart, value); }
    }

    //카트상태이상효과종료
    public event Delegate<SEBCartAbnormalStateEffectFinishedEventBody> EventEvtCartAbnormalStateEffectFinished
    {
        add { AddDelegate<SEBCartAbnormalStateEffectFinishedEventBody>(ServerEventName.CartAbnormalStateEffectFinished, value); }
        remove { DelDelegate<SEBCartAbnormalStateEffectFinishedEventBody>(ServerEventName.CartAbnormalStateEffectFinished, value); }
    }

    //카트상태이상효과적중
    public event Delegate<SEBCartAbnormalStateEffectHitEventBody> EventEvtCartAbnormalStateEffectHit
    {
        add { AddDelegate<SEBCartAbnormalStateEffectHitEventBody>(ServerEventName.CartAbnormalStateEffectHit, value); }
        remove { DelDelegate<SEBCartAbnormalStateEffectHitEventBody>(ServerEventName.CartAbnormalStateEffectHit, value); }
    }

    #endregion Event Cart

    #region Event FieldOfHonor

    //결투장시작
    public event Delegate<SEBFieldOfHonorStartEventBody> EventEvtFieldOfHonorStart
    {
        add { AddDelegate<SEBFieldOfHonorStartEventBody>(ServerEventName.FieldOfHonorStart, value); }
        remove { DelDelegate<SEBFieldOfHonorStartEventBody>(ServerEventName.FieldOfHonorStart, value); }
    }

    //결투장승리
    public event Delegate<SEBFieldOfHonorClearEventBody> EventEvtFieldOfHonorClear
    {
        add { AddDelegate<SEBFieldOfHonorClearEventBody>(ServerEventName.FieldOfHonorClear, value); }
        remove { DelDelegate<SEBFieldOfHonorClearEventBody>(ServerEventName.FieldOfHonorClear, value); }
    }

    //결투장패배
    public event Delegate<SEBFieldOfHonorFailEventBody> EventEvtFieldOfHonorFail
    {
        add { AddDelegate<SEBFieldOfHonorFailEventBody>(ServerEventName.FieldOfHonorFail, value); }
        remove { DelDelegate<SEBFieldOfHonorFailEventBody>(ServerEventName.FieldOfHonorFail, value); }
    }

    //결투장강퇴
    public event Delegate<SEBFieldOfHonorBanishedEventBody> EventEvtFieldOfHonorBanished
    {
        add { AddDelegate<SEBFieldOfHonorBanishedEventBody>(ServerEventName.FieldOfHonorBanished, value); }
        remove { DelDelegate<SEBFieldOfHonorBanishedEventBody>(ServerEventName.FieldOfHonorBanished, value); }
    }

    //결투장일일랭킹갱신
    public event Delegate<SEBFieldOfHonorDailyRankingUpdatedEventBody> EventEvtFieldOfHonorDailyRankingUpdated
    {
        add { AddDelegate<SEBFieldOfHonorDailyRankingUpdatedEventBody>(ServerEventName.FieldOfHonorDailyRankingUpdated, value); }
        remove { DelDelegate<SEBFieldOfHonorDailyRankingUpdatedEventBody>(ServerEventName.FieldOfHonorDailyRankingUpdated, value); }
    }

    #endregion Event FieldOfHonor

    #region Event Ranking

    //일일서버레벨랭킹갱신
    public event Delegate<SEBDailyServerLevelRankingUpdatedEventBody> EventEvtDailyServerLevelRankingUpdated
    {
        add { AddDelegate<SEBDailyServerLevelRankingUpdatedEventBody>(ServerEventName.DailyServerLevelRankingUpdated, value); }
        remove { DelDelegate<SEBDailyServerLevelRankingUpdatedEventBody>(ServerEventName.DailyServerLevelRankingUpdated, value); }
    }

    #endregion Event Ranking

    #region Event SupplySupport

    //보급지원퀘스트실패
    public event Delegate<SEBSupplySupportQuestFailEventBody> EventEvtSupplySupportQuestFail
    {
        add { AddDelegate<SEBSupplySupportQuestFailEventBody>(ServerEventName.SupplySupportQuestFail, value); }
        remove { DelDelegate<SEBSupplySupportQuestFailEventBody>(ServerEventName.SupplySupportQuestFail, value); }
    }

    //보급지원퀘스트카트파괴보상
    public event Delegate<SEBSupplySupportQuestCartDestructionRewardEventBody> EventEvtSupplySupportQuestCartDestructionReward
    {
        add { AddDelegate<SEBSupplySupportQuestCartDestructionRewardEventBody>(ServerEventName.SupplySupportQuestCartDestructionReward, value); }
        remove { DelDelegate<SEBSupplySupportQuestCartDestructionRewardEventBody>(ServerEventName.SupplySupportQuestCartDestructionReward, value); }
    }

    #endregion Event SupplySupport

    #region Event SoulCoveter

    //영혼을탐하는자매칭상태변경
    public event Delegate<SEBSoulCoveterMatchingStatusChangedEventBody> EventEvtSoulCoveterMatchingStatusChanged
    {
        add { AddDelegate<SEBSoulCoveterMatchingStatusChangedEventBody>(ServerEventName.SoulCoveterMatchingStatusChanged, value); }
        remove { DelDelegate<SEBSoulCoveterMatchingStatusChangedEventBody>(ServerEventName.SoulCoveterMatchingStatusChanged, value); }
    }

    //영혼을탐하는자매칭방강퇴
    public event Delegate<SEBSoulCoveterMatchingRoomBanishedEventBody> EventEvtSoulCoveterMatchingRoomBanished
    {
        add { AddDelegate<SEBSoulCoveterMatchingRoomBanishedEventBody>(ServerEventName.SoulCoveterMatchingRoomBanished, value); }
        remove { DelDelegate<SEBSoulCoveterMatchingRoomBanishedEventBody>(ServerEventName.SoulCoveterMatchingRoomBanished, value); }
    }

    //영혼을탐하는자매칭방파티입장
    public event Delegate<SEBSoulCoveterMatchingRoomPartyEnterEventBody> EventEvtSoulCoveterMatchingRoomPartyEnter
    {
        add { AddDelegate<SEBSoulCoveterMatchingRoomPartyEnterEventBody>(ServerEventName.SoulCoveterMatchingRoomPartyEnter, value); }
        remove { DelDelegate<SEBSoulCoveterMatchingRoomPartyEnterEventBody>(ServerEventName.SoulCoveterMatchingRoomPartyEnter, value); }
    }

    //영혼을탐하는자입장에대한대륙퇴장
    public event Delegate<SEBContinentExitForSoulCoveterEnterEventBody> EventEvtContinentExitForSoulCoveterEnter
    {
        add { AddDelegate<SEBContinentExitForSoulCoveterEnterEventBody>(ServerEventName.ContinentExitForSoulCoveterEnter, value); }
        remove { DelDelegate<SEBContinentExitForSoulCoveterEnterEventBody>(ServerEventName.ContinentExitForSoulCoveterEnter, value); }
    }

    //영혼을탐하는자웨이브시작
    public event Delegate<SEBSoulCoveterWaveStartEventBody> EventEvtSoulCoveterWaveStart
    {
        add { AddDelegate<SEBSoulCoveterWaveStartEventBody>(ServerEventName.SoulCoveterWaveStart, value); }
        remove { DelDelegate<SEBSoulCoveterWaveStartEventBody>(ServerEventName.SoulCoveterWaveStart, value); }
    }

    //영혼을탐하는자완료
    public event Delegate<SEBSoulCoveterClearEventBody> EventEvtSoulCoveterClear
    {
        add { AddDelegate<SEBSoulCoveterClearEventBody>(ServerEventName.SoulCoveterClear, value); }
        remove { DelDelegate<SEBSoulCoveterClearEventBody>(ServerEventName.SoulCoveterClear, value); }
    }

    //영혼을탐하는자실패
    public event Delegate<SEBSoulCoveterFailEventBody> EventEvtSoulCoveterFail
    {
        add { AddDelegate<SEBSoulCoveterFailEventBody>(ServerEventName.SoulCoveterFail, value); }
        remove { DelDelegate<SEBSoulCoveterFailEventBody>(ServerEventName.SoulCoveterFail, value); }
    }

    //영혼을탐하는자강퇴
    public event Delegate<SEBSoulCoveterBanishedEventBody> EventEvtSoulCoveterBanished
    {
        add { AddDelegate<SEBSoulCoveterBanishedEventBody>(ServerEventName.SoulCoveterBanished, value); }
        remove { DelDelegate<SEBSoulCoveterBanishedEventBody>(ServerEventName.SoulCoveterBanished, value); }
    }

    #endregion Event SoulCoveter

    #region Event Guild

    //길드신청수락
    public event Delegate<SEBGuildApplicationAcceptedEventBody> EventEvtGuildApplicationAccepted
    {
        add { AddDelegate<SEBGuildApplicationAcceptedEventBody>(ServerEventName.GuildApplicationAccepted, value); }
        remove { DelDelegate<SEBGuildApplicationAcceptedEventBody>(ServerEventName.GuildApplicationAccepted, value); }
    }

    //길드신청거절
    public event Delegate<SEBGuildApplicationRefusedEventBody> EventEvtGuildApplicationRefused
    {
        add { AddDelegate<SEBGuildApplicationRefusedEventBody>(ServerEventName.GuildApplicationRefused, value); }
        remove { DelDelegate<SEBGuildApplicationRefusedEventBody>(ServerEventName.GuildApplicationRefused, value); }
    }

    //길드신청수갱신
    public event Delegate<SEBGuildApplicationCountUpdatedEventBody> EventEvtGuildApplicationCountUpdated
    {
        add { AddDelegate<SEBGuildApplicationCountUpdatedEventBody>(ServerEventName.GuildApplicationCountUpdated, value); }
        remove { DelDelegate<SEBGuildApplicationCountUpdatedEventBody>(ServerEventName.GuildApplicationCountUpdated, value); }
    }

    //길드멤버가입
    public event Delegate<SEBGuildMemberEnterEventBody> EventEvtGuildMemberEnter
    {
        add { AddDelegate<SEBGuildMemberEnterEventBody>(ServerEventName.GuildMemberEnter, value); }
        remove { DelDelegate<SEBGuildMemberEnterEventBody>(ServerEventName.GuildMemberEnter, value); }
    }

    //길드멤버탈퇴
    public event Delegate<SEBGuildMemberExitEventBody> EventEvtGuildMemberExit
    {
        add { AddDelegate<SEBGuildMemberExitEventBody>(ServerEventName.GuildMemberExit, value); }
        remove { DelDelegate<SEBGuildMemberExitEventBody>(ServerEventName.GuildMemberExit, value); }
    }

    //길드강퇴
    public event Delegate<SEBGuildBanishedEventBody> EventEvtGuildBanished
    {
        add { AddDelegate<SEBGuildBanishedEventBody>(ServerEventName.GuildBanished, value); }
        remove { DelDelegate<SEBGuildBanishedEventBody>(ServerEventName.GuildBanished, value); }
    }

    //영웅길드정보갱신
    public event Delegate<SEBHeroGuildInfoUpdatedEventBody> EventEvtHeroGuildInfoUpdated
    {
        add { AddDelegate<SEBHeroGuildInfoUpdatedEventBody>(ServerEventName.HeroGuildInfoUpdated, value); }
        remove { DelDelegate<SEBHeroGuildInfoUpdatedEventBody>(ServerEventName.HeroGuildInfoUpdated, value); }
    }

    //길드초대도착
    public event Delegate<SEBGuildInvitationArrivedEventBody> EventEvtGuildInvitationArrived
    {
        add { AddDelegate<SEBGuildInvitationArrivedEventBody>(ServerEventName.GuildInvitationArrived, value); }
        remove { DelDelegate<SEBGuildInvitationArrivedEventBody>(ServerEventName.GuildInvitationArrived, value); }
    }

    //길드초대거절
    public event Delegate<SEBGuildInvitationRefusedEventBody> EventEvtGuildInvitationRefused
    {
        add { AddDelegate<SEBGuildInvitationRefusedEventBody>(ServerEventName.GuildInvitationRefused, value); }
        remove { DelDelegate<SEBGuildInvitationRefusedEventBody>(ServerEventName.GuildInvitationRefused, value); }
    }

    //길드초대수명종료
    public event Delegate<SEBGuildInvitationLifetimeEndedEventBody> EventEvtGuildInvitationLifetimeEnded
    {
        add { AddDelegate<SEBGuildInvitationLifetimeEndedEventBody>(ServerEventName.GuildInvitationLifetimeEnded, value); }
        remove { DelDelegate<SEBGuildInvitationLifetimeEndedEventBody>(ServerEventName.GuildInvitationLifetimeEnded, value); }
    }

    //길드임명
    public event Delegate<SEBGuildAppointedEventBody> EventEvtGuildAppointed
    {
        add { AddDelegate<SEBGuildAppointedEventBody>(ServerEventName.GuildAppointed, value); }
        remove { DelDelegate<SEBGuildAppointedEventBody>(ServerEventName.GuildAppointed, value); }
    }

    //길드장위임
    public event Delegate<SEBGuildMasterTransferredEventBody> EventEvtGuildMasterTransferred
    {
        add { AddDelegate<SEBGuildMasterTransferredEventBody>(ServerEventName.GuildMasterTransferred, value); }
        remove { DelDelegate<SEBGuildMasterTransferredEventBody>(ServerEventName.GuildMasterTransferred, value); }
    }

    //길드공지변경
    public event Delegate<SEBGuildNoticeChangedEventBody> EventEvtGuildNoticeChanged
    {
        add { AddDelegate<SEBGuildNoticeChangedEventBody>(ServerEventName.GuildNoticeChanged, value); }
        remove { DelDelegate<SEBGuildNoticeChangedEventBody>(ServerEventName.GuildNoticeChanged, value); }
    }

    //길드자금변경
    public event Delegate<SEBGuildFundChangedEventBody> EventEvtGuildFundChanged
    {
        add { AddDelegate<SEBGuildFundChangedEventBody>(ServerEventName.GuildFundChanged, value); }
        remove { DelDelegate<SEBGuildFundChangedEventBody>(ServerEventName.GuildFundChanged, value); }
    }

    //길드모럴포인트변경
    public event Delegate<SEBGuildMoralPointChangedEventBody> EventEvtGuildMoralPointChanged
    {
        add { AddDelegate<SEBGuildMoralPointChangedEventBody>(ServerEventName.GuildMoralPointChanged, value); }
        remove { DelDelegate<SEBGuildMoralPointChangedEventBody>(ServerEventName.GuildMoralPointChanged, value); }
    }

    //길드소집
    public event Delegate<SEBGuildCallEventBody> EventEvtGuildCall
    {
        add { AddDelegate<SEBGuildCallEventBody>(ServerEventName.GuildCall, value); }
        remove { DelDelegate<SEBGuildCallEventBody>(ServerEventName.GuildCall, value); }
    }

    #endregion Event Guild

    #region Event GuildFarmQuest

    //길드농장퀘스트상호작용완료
    public event Delegate<SEBGuildFarmQuestInteractionCompletedEventBody> EventEvtGuildFarmQuestInteractionCompleted
    {
        add { AddDelegate<SEBGuildFarmQuestInteractionCompletedEventBody>(ServerEventName.GuildFarmQuestInteractionCompleted, value); }
        remove { DelDelegate<SEBGuildFarmQuestInteractionCompletedEventBody>(ServerEventName.GuildFarmQuestInteractionCompleted, value); }
    }

    //길드농장퀘스트상호작용취소
    public event Delegate<SEBGuildFarmQuestInteractionCanceledEventBody> EventEvtGuildFarmQuestInteractionCanceled
    {
        add { AddDelegate<SEBGuildFarmQuestInteractionCanceledEventBody>(ServerEventName.GuildFarmQuestInteractionCanceled, value); }
        remove { DelDelegate<SEBGuildFarmQuestInteractionCanceledEventBody>(ServerEventName.GuildFarmQuestInteractionCanceled, value); }
    }

    //영웅길드농장퀘스트상호작용시작
    public event Delegate<SEBHeroGuildFarmQuestInteractionStartedEventBody> EventEvtHeroGuildFarmQuestInteractionStarted
    {
        add { AddDelegate<SEBHeroGuildFarmQuestInteractionStartedEventBody>(ServerEventName.HeroGuildFarmQuestInteractionStarted, value); }
        remove { DelDelegate<SEBHeroGuildFarmQuestInteractionStartedEventBody>(ServerEventName.HeroGuildFarmQuestInteractionStarted, value); }
    }

    //영웅길드농장퀘스트상호작용완료
    public event Delegate<SEBHeroGuildFarmQuestInteractionCompletedEventBody> EventEvtHeroGuildFarmQuestInteractionCompleted
    {
        add { AddDelegate<SEBHeroGuildFarmQuestInteractionCompletedEventBody>(ServerEventName.HeroGuildFarmQuestInteractionCompleted, value); }
        remove { DelDelegate<SEBHeroGuildFarmQuestInteractionCompletedEventBody>(ServerEventName.HeroGuildFarmQuestInteractionCompleted, value); }
    }

    //영웅길드농장퀘스트상호작용취소
    public event Delegate<SEBHeroGuildFarmQuestInteractionCanceledEventBody> EventEvtHeroGuildFarmQuestInteractionCanceled
    {
        add { AddDelegate<SEBHeroGuildFarmQuestInteractionCanceledEventBody>(ServerEventName.HeroGuildFarmQuestInteractionCanceled, value); }
        remove { DelDelegate<SEBHeroGuildFarmQuestInteractionCanceledEventBody>(ServerEventName.HeroGuildFarmQuestInteractionCanceled, value); }
    }

    #endregion Event GuildFarmQuest

    #region Event GuildBuilding

    //길드건물레벨업
    public event Delegate<SEBGuildBuildingLevelUpEventBody> EventEvtGuildBuildingLevelUp
    {
        add { AddDelegate<SEBGuildBuildingLevelUpEventBody>(ServerEventName.GuildBuildingLevelUp, value); }
        remove { DelDelegate<SEBGuildBuildingLevelUpEventBody>(ServerEventName.GuildBuildingLevelUp, value); }
    }

    #endregion Event GuildBuilding

    #region Event GuildFoodWarehouse

    //길드군량창고징수
    public event Delegate<SEBGuildFoodWarehouseCollectedEventBody> EventEvtGuildFoodWarehouseCollected
    {
        add { AddDelegate<SEBGuildFoodWarehouseCollectedEventBody>(ServerEventName.GuildFoodWarehouseCollected, value); }
        remove { DelDelegate<SEBGuildFoodWarehouseCollectedEventBody>(ServerEventName.GuildFoodWarehouseCollected, value); }
    }

    #endregion Event GuildFoodWarehouse

    #region Event GuildAltar

    //길드제단마력주입미션완료
    public event Delegate<SEBGuildAltarSpellInjectionMissionCompletedEventBody> EventEvtGuildAltarSpellInjectionMissionCompleted
    {
        add { AddDelegate<SEBGuildAltarSpellInjectionMissionCompletedEventBody>(ServerEventName.GuildAltarSpellInjectionMissionCompleted, value); }
        remove { DelDelegate<SEBGuildAltarSpellInjectionMissionCompletedEventBody>(ServerEventName.GuildAltarSpellInjectionMissionCompleted, value); }
    }

    //길드제단마력주입미션취소
    public event Delegate<SEBGuildAltarSpellInjectionMissionCanceledEventBody> EventEvtGuildAltarSpellInjectionMissionCanceled
    {
        add { AddDelegate<SEBGuildAltarSpellInjectionMissionCanceledEventBody>(ServerEventName.GuildAltarSpellInjectionMissionCanceled, value); }
        remove { DelDelegate<SEBGuildAltarSpellInjectionMissionCanceledEventBody>(ServerEventName.GuildAltarSpellInjectionMissionCanceled, value); }
    }

    //영웅길드제단마력주입미션시작
    public event Delegate<SEBHeroGuildAltarSpellInjectionMissionStartedEventBody> EventEvtHeroGuildAltarSpellInjectionMissionStarted
    {
        add { AddDelegate<SEBHeroGuildAltarSpellInjectionMissionStartedEventBody>(ServerEventName.HeroGuildAltarSpellInjectionMissionStarted, value); }
        remove { DelDelegate<SEBHeroGuildAltarSpellInjectionMissionStartedEventBody>(ServerEventName.HeroGuildAltarSpellInjectionMissionStarted, value); }
    }

    //영웅길드제단마력주입미션완료
    public event Delegate<SEBHeroGuildAltarSpellInjectionMissionCompletedEventBody> EventEvtHeroGuildAltarSpellInjectionMissionCompleted
    {
        add { AddDelegate<SEBHeroGuildAltarSpellInjectionMissionCompletedEventBody>(ServerEventName.HeroGuildAltarSpellInjectionMissionCompleted, value); }
        remove { DelDelegate<SEBHeroGuildAltarSpellInjectionMissionCompletedEventBody>(ServerEventName.HeroGuildAltarSpellInjectionMissionCompleted, value); }
    }

    //영웅길드제단마력주입미션취소
    public event Delegate<SEBHeroGuildAltarSpellInjectionMissionCanceledEventBody> EventEvtHeroGuildAltarSpellInjectionMissionCanceled
    {
        add { AddDelegate<SEBHeroGuildAltarSpellInjectionMissionCanceledEventBody>(ServerEventName.HeroGuildAltarSpellInjectionMissionCanceled, value); }
        remove { DelDelegate<SEBHeroGuildAltarSpellInjectionMissionCanceledEventBody>(ServerEventName.HeroGuildAltarSpellInjectionMissionCanceled, value); }
    }

    //길드제단수비미션완료
    public event Delegate<SEBGuildAltarDefenseMissionCompletedEventBody> EventEvtGuildAltarDefenseMissionCompleted
    {
        add { AddDelegate<SEBGuildAltarDefenseMissionCompletedEventBody>(ServerEventName.GuildAltarDefenseMissionCompleted, value); }
        remove { DelDelegate<SEBGuildAltarDefenseMissionCompletedEventBody>(ServerEventName.GuildAltarDefenseMissionCompleted, value); }
    }

    //길드제단수비미션실패
    public event Delegate<SEBGuildAltarDefenseMissionFailedEventBody> EventEvtGuildAltarDefenseMissionFailed
    {
        add { AddDelegate<SEBGuildAltarDefenseMissionFailedEventBody>(ServerEventName.GuildAltarDefenseMissionFailed, value); }
        remove { DelDelegate<SEBGuildAltarDefenseMissionFailedEventBody>(ServerEventName.GuildAltarDefenseMissionFailed, value); }
    }

    //길드제단완료
    public event Delegate<SEBGuildAltarCompletedEventBody> EventEvtGuildAltarCompleted
    {
        add { AddDelegate<SEBGuildAltarCompletedEventBody>(ServerEventName.GuildAltarCompleted, value); }
        remove { DelDelegate<SEBGuildAltarCompletedEventBody>(ServerEventName.GuildAltarCompleted, value); }
    }

    #endregion Event GuildAltar

    #region Event GuildMission

    //길드미션완료
    public event Delegate<SEBGuildMissionCompletedEventBody> EventEvtGuildMissionCompleted
    {
        add { AddDelegate<SEBGuildMissionCompletedEventBody>(ServerEventName.GuildMissionCompleted, value); }
        remove { DelDelegate<SEBGuildMissionCompletedEventBody>(ServerEventName.GuildMissionCompleted, value); }
    }

    //길드미션실패
    public event Delegate<SEBGuildMissionFailedEventBody> EventEvtGuildMissionFailed
    {
        add { AddDelegate<SEBGuildMissionFailedEventBody>(ServerEventName.GuildMissionFailed, value); }
        remove { DelDelegate<SEBGuildMissionFailedEventBody>(ServerEventName.GuildMissionFailed, value); }
    }

	// 길드미션완료
	public event Delegate<SEBGuildMissionCompletedEventBody > EventEvtGuildMissionComplete
	{
		add { AddDelegate<SEBGuildMissionCompletedEventBody>(ServerEventName.GuildMissionCompleted, value); }
		remove { DelDelegate<SEBGuildMissionCompletedEventBody>(ServerEventName.GuildMissionCompleted, value); }
	}

    //길드미션갱신
    public event Delegate<SEBGuildMissionUpdatedEventBody> EventEvtGuildMissionUpdated
    {
        add { AddDelegate<SEBGuildMissionUpdatedEventBody>(ServerEventName.GuildMissionUpdated, value); }
        remove { DelDelegate<SEBGuildMissionUpdatedEventBody>(ServerEventName.GuildMissionUpdated, value); }
    }

    //길드정신알림
    public event Delegate<SEBGuildSpiritAnnouncedEventBody> EventEvtGuildSpiritAnnounced
    {
        add { AddDelegate<SEBGuildSpiritAnnouncedEventBody>(ServerEventName.GuildSpiritAnnounced, value); }
        remove { DelDelegate<SEBGuildSpiritAnnouncedEventBody>(ServerEventName.GuildSpiritAnnounced, value); }
    }

    #endregion Event GuildMission

    #region Event GuildSupplySupport

    //길드물자지원퀘스트시작
    public event Delegate<SEBGuildSupplySupportQuestStartedEventBody> EventEvtGuildSupplySupportQuestStarted
    {
        add { AddDelegate<SEBGuildSupplySupportQuestStartedEventBody>(ServerEventName.GuildSupplySupportQuestStarted, value); }
        remove { DelDelegate<SEBGuildSupplySupportQuestStartedEventBody>(ServerEventName.GuildSupplySupportQuestStarted, value); }
    }

    //길드물자지원퀘스트완료
    public event Delegate<SEBGuildSupplySupportQuestCompletedEventBody> EventEvtGuildSupplySupportQuestCompleted
    {
        add { AddDelegate<SEBGuildSupplySupportQuestCompletedEventBody>(ServerEventName.GuildSupplySupportQuestCompleted, value); }
        remove { DelDelegate<SEBGuildSupplySupportQuestCompletedEventBody>(ServerEventName.GuildSupplySupportQuestCompleted, value); }
    }

    //길드물자지원퀘스트실패
    public event Delegate<SEBGuildSupplySupportQuestFailEventBody> EventEvtGuildSupplySupportQuestFail
    {
        add { AddDelegate<SEBGuildSupplySupportQuestFailEventBody>(ServerEventName.GuildSupplySupportQuestFail, value); }
        remove { DelDelegate<SEBGuildSupplySupportQuestFailEventBody>(ServerEventName.GuildSupplySupportQuestFail, value); }
    }

    #endregion Event GuildSupplySupport

    #region Event GuildHunting

    //길드헌팅퀘스트갱신
    public event Delegate<SEBGuildHuntingQuestUpdatedEventBody> EventEvtGuildHuntingQuestUpdated
    {
        add { AddDelegate<SEBGuildHuntingQuestUpdatedEventBody>(ServerEventName.GuildHuntingQuestUpdated, value); }
        remove { DelDelegate<SEBGuildHuntingQuestUpdatedEventBody>(ServerEventName.GuildHuntingQuestUpdated, value); }
    }

    //길드헌팅기부횟수갱신
    public event Delegate<SEBGuildHuntingDonationCountUpdatedEventBody> EventEvtGuildHuntingDonationCountUpdated
    {
        add { AddDelegate<SEBGuildHuntingDonationCountUpdatedEventBody>(ServerEventName.GuildHuntingDonationCountUpdated, value); }
        remove { DelDelegate<SEBGuildHuntingDonationCountUpdatedEventBody>(ServerEventName.GuildHuntingDonationCountUpdated, value); }
    }

    #endregion Event GuildHunting

    #region Event GuildDaily

    //길드일일목표알림
    public event Delegate<SEBGuildDailyObjectiveNoticeEventBody> EventEvtGuildDailyObjectiveNotice
    {
        add { AddDelegate<SEBGuildDailyObjectiveNoticeEventBody>(ServerEventName.GuildDailyObjectiveNotice, value); }
        remove { DelDelegate<SEBGuildDailyObjectiveNoticeEventBody>(ServerEventName.GuildDailyObjectiveNotice, value); }
    }

    //길드일일목표설정
    public event Delegate<SEBGuildDailyObjectiveSetEventBody> EventEvtGuildDailyObjectiveSet
    {
        add { AddDelegate<SEBGuildDailyObjectiveSetEventBody>(ServerEventName.GuildDailyObjectiveSet, value); }
        remove { DelDelegate<SEBGuildDailyObjectiveSetEventBody>(ServerEventName.GuildDailyObjectiveSet, value); }
    }

    //길드일일목표완료멤버수갱신
    public event Delegate<SEBGuildDailyObjectiveCompletionMemberCountUpdatedEventBody> EventEvtGuildDailyObjectiveCompletionMemberCountUpdated
    {
        add { AddDelegate<SEBGuildDailyObjectiveCompletionMemberCountUpdatedEventBody>(ServerEventName.GuildDailyObjectiveCompletionMemberCountUpdated, value); }
        remove { DelDelegate<SEBGuildDailyObjectiveCompletionMemberCountUpdatedEventBody>(ServerEventName.GuildDailyObjectiveCompletionMemberCountUpdated, value); }
    }

    #endregion Event GuildDaily

    #region Event GuildWeekly

    //길드주간목표설정
    public event Delegate<SEBGuildWeeklyObjectiveSetEventBody> EventEvtGuildWeeklyObjectiveSet
    {
        add { AddDelegate<SEBGuildWeeklyObjectiveSetEventBody>(ServerEventName.GuildWeeklyObjectiveSet, value); }
        remove { DelDelegate<SEBGuildWeeklyObjectiveSetEventBody>(ServerEventName.GuildWeeklyObjectiveSet, value); }
    }

    //길드일일목표완료멤버수갱신
    public event Delegate<SEBGuildWeeklyObjectiveCompletionMemberCountUpdatedEventBody> EventEvtGuildWeeklyObjectiveCompletionMemberCountUpdated
    {
        add { AddDelegate<SEBGuildWeeklyObjectiveCompletionMemberCountUpdatedEventBody>(ServerEventName.GuildWeeklyObjectiveCompletionMemberCountUpdated, value); }
        remove { DelDelegate<SEBGuildWeeklyObjectiveCompletionMemberCountUpdatedEventBody>(ServerEventName.GuildWeeklyObjectiveCompletionMemberCountUpdated, value); }
    }

    #endregion Event GuildWeekly

    #region Event NationWar

    //국가전선포
    public event Delegate<SEBNationWarDeclarationEventBody> EventEvtNationWarDeclaration
    {
        add { AddDelegate<SEBNationWarDeclarationEventBody>(ServerEventName.NationWarDeclaration, value); }
        remove { DelDelegate<SEBNationWarDeclarationEventBody>(ServerEventName.NationWarDeclaration, value); }
    }

    //국가전시작
    public event Delegate<SEBNationWarStartEventBody> EventEvtNationWarStart
    {
        add { AddDelegate<SEBNationWarStartEventBody>(ServerEventName.NationWarStart, value); }
        remove { DelDelegate<SEBNationWarStartEventBody>(ServerEventName.NationWarStart, value); }
    }

    //국가전종료
    public event Delegate<SEBNationWarFinishedEventBody> EventEvtNationWarFinished
    {
        add { AddDelegate<SEBNationWarFinishedEventBody>(ServerEventName.NationWarFinished, value); }
        remove { DelDelegate<SEBNationWarFinishedEventBody>(ServerEventName.NationWarFinished, value); }
    }

    //국가전승리
    public event Delegate<SEBNationWarWinEventBody> EventEvtNationWarWin
    {
        add { AddDelegate<SEBNationWarWinEventBody>(ServerEventName.NationWarWin, value); }
        remove { DelDelegate<SEBNationWarWinEventBody>(ServerEventName.NationWarWin, value); }
    }

    //국가전패배
    public event Delegate<SEBNationWarLoseEventBody> EventEvtNationWarLose
    {
        add { AddDelegate<SEBNationWarLoseEventBody>(ServerEventName.NationWarLose, value); }
        remove { DelDelegate<SEBNationWarLoseEventBody>(ServerEventName.NationWarLose, value); }
    }

    //국가전몬스터전투모드시작
    public event Delegate<SEBNationWarMonsterBattleModeStartEventBody> EventEvtNationWarMonsterBattleModeStart
    {
        add { AddDelegate<SEBNationWarMonsterBattleModeStartEventBody>(ServerEventName.NationWarMonsterBattleModeStart, value); }
        remove { DelDelegate<SEBNationWarMonsterBattleModeStartEventBody>(ServerEventName.NationWarMonsterBattleModeStart, value); }
    }

    //국가전몬스터전투모드종료
    public event Delegate<SEBNationWarMonsterBattleModeEndEventBody> EventEvtNationWarMonsterBattleModeEnd
    {
        add { AddDelegate<SEBNationWarMonsterBattleModeEndEventBody>(ServerEventName.NationWarMonsterBattleModeEnd, value); }
        remove { DelDelegate<SEBNationWarMonsterBattleModeEndEventBody>(ServerEventName.NationWarMonsterBattleModeEnd, value); }
    }

    //국가전몬스터사망
    public event Delegate<SEBNationWarMonsterDeadEventBody> EventEvtNationWarMonsterDead
    {
        add { AddDelegate<SEBNationWarMonsterDeadEventBody>(ServerEventName.NationWarMonsterDead, value); }
        remove { DelDelegate<SEBNationWarMonsterDeadEventBody>(ServerEventName.NationWarMonsterDead, value); }
    }

    //국가전몬스터긴급상황
    public event Delegate<SEBNationWarMonsterEmergencyEventBody> EventEvtNationWarMonsterEmergency
    {
        add { AddDelegate<SEBNationWarMonsterEmergencyEventBody>(ServerEventName.NationWarMonsterEmergency, value); }
        remove { DelDelegate<SEBNationWarMonsterEmergencyEventBody>(ServerEventName.NationWarMonsterEmergency, value); }
    }

    //국가전몬스터소환
    public event Delegate<SEBNationWarMonsterSpawnEventBody> EventEvtNationWarMonsterSpawn
    {
        add { AddDelegate<SEBNationWarMonsterSpawnEventBody>(ServerEventName.NationWarMonsterSpawn, value); }
        remove { DelDelegate<SEBNationWarMonsterSpawnEventBody>(ServerEventName.NationWarMonsterSpawn, value); }
    }

    //국가전집중공격
    public event Delegate<SEBNationWarConvergingAttackEventBody> EventEvtNationWarConvergingAttack
    {
        add { AddDelegate<SEBNationWarConvergingAttackEventBody>(ServerEventName.NationWarConvergingAttack, value); }
        remove { DelDelegate<SEBNationWarConvergingAttackEventBody>(ServerEventName.NationWarConvergingAttack, value); }
    }

    //국가전집중공격종료
    public event Delegate<SEBNationWarConvergingAttackFinishedEventBody> EventEvtNationWarConvergingAttackFinished
    {
        add { AddDelegate<SEBNationWarConvergingAttackFinishedEventBody>(ServerEventName.NationWarConvergingAttackFinished, value); }
        remove { DelDelegate<SEBNationWarConvergingAttackFinishedEventBody>(ServerEventName.NationWarConvergingAttackFinished, value); }
    }

    //국가전소집
    public event Delegate<SEBNationWarCallEventBody> EventEvtNationWarCall
    {
        add { AddDelegate<SEBNationWarCallEventBody>(ServerEventName.NationWarCall, value); }
        remove { DelDelegate<SEBNationWarCallEventBody>(ServerEventName.NationWarCall, value); }
    }

    //국가전킬횟수갱신
    public event Delegate<SEBNationWarKillCountUpdatedEventBody> EventEvtNationWarKillCountUpdated
    {
        add { AddDelegate<SEBNationWarKillCountUpdatedEventBody>(ServerEventName.NationWarKillCountUpdated, value); }
        remove { DelDelegate<SEBNationWarKillCountUpdatedEventBody>(ServerEventName.NationWarKillCountUpdated, value); }
    }

    //국가전도움횟수갱신
    public event Delegate<SEBNationWarAssistCountUpdatedEventBody> EventEvtNationWarAssistCountUpdated
    {
        add { AddDelegate<SEBNationWarAssistCountUpdatedEventBody>(ServerEventName.NationWarAssistCountUpdated, value); }
        remove { DelDelegate<SEBNationWarAssistCountUpdatedEventBody>(ServerEventName.NationWarAssistCountUpdated, value); }
    }

    //국가전사망횟수갱신
    public event Delegate<SEBNationWarDeadCountUpdatedEventBody> EventEvtNationWarDeadCountUpdated
    {
        add { AddDelegate<SEBNationWarDeadCountUpdatedEventBody>(ServerEventName.NationWarDeadCountUpdated, value); }
        remove { DelDelegate<SEBNationWarDeadCountUpdatedEventBody>(ServerEventName.NationWarDeadCountUpdated, value); }
    }

    //국가전즉시부활횟수갱신
    public event Delegate<SEBNationWarImmediateRevivalCountUpdatedEventBody> EventEvtNationWarImmediateRevivalCountUpdated
    {
        add { AddDelegate<SEBNationWarImmediateRevivalCountUpdatedEventBody>(ServerEventName.NationWarImmediateRevivalCountUpdated, value); }
        remove { DelDelegate<SEBNationWarImmediateRevivalCountUpdatedEventBody>(ServerEventName.NationWarImmediateRevivalCountUpdated, value); }
    }

    //국가전멀티킬
    public event Delegate<SEBNationWarMultiKillEventBody> EventEvtNationWarMultiKill
    {
        add { AddDelegate<SEBNationWarMultiKillEventBody>(ServerEventName.NationWarMultiKill, value); }
        remove { DelDelegate<SEBNationWarMultiKillEventBody>(ServerEventName.NationWarMultiKill, value); }
    }

    //국가전관직자킬
    public event Delegate<SEBNationWarNoblesseKillEventBody> EventEvtNationWarNoblesseKill
    {
        add { AddDelegate<SEBNationWarNoblesseKillEventBody>(ServerEventName.NationWarNoblesseKill, value); }
        remove { DelDelegate<SEBNationWarNoblesseKillEventBody>(ServerEventName.NationWarNoblesseKill, value); }
    }

    #endregion Event NationWar

    #region Event NationPowerRanking

    // 국력랭킹
    public event Delegate<SEBDailyServerNationPowerRankingUpdatedEventBody> EventEvtDailyServerNationPowerRankingUpdated
    {
        add { AddDelegate<SEBDailyServerNationPowerRankingUpdatedEventBody>(ServerEventName.DailyServerNationPowerRankingUpdated, value); }
        remove { DelDelegate<SEBDailyServerNationPowerRankingUpdatedEventBody>(ServerEventName.DailyServerNationPowerRankingUpdated, value); }
    }

    #endregion Event NationPowerRanking

    #region Event NationAlliance

    // 국가동맹신청
    public event Delegate<SEBNationAllianceAppliedEventBody> EventEvtNationAllianceApplied
    {
        add { AddDelegate<SEBNationAllianceAppliedEventBody>(ServerEventName.NationAllianceApplied, value); }
        remove { DelDelegate<SEBNationAllianceAppliedEventBody>(ServerEventName.NationAllianceApplied, value); }
    }

    // 국가동맹체결
    public event Delegate<SEBNationAllianceConcludedEventBody> EventEvtNationAllianceConcluded
    {
        add { AddDelegate<SEBNationAllianceConcludedEventBody>(ServerEventName.NationAllianceConcluded, value); }
        remove { DelDelegate<SEBNationAllianceConcludedEventBody>(ServerEventName.NationAllianceConcluded, value); }
    }

    // 국가동맹신청수락
    public event Delegate<SEBNationAllianceApplicationAcceptedEventBody> EventEvtNationAllianceApplicationAccepted
    {
        add { AddDelegate<SEBNationAllianceApplicationAcceptedEventBody>(ServerEventName.NationAllianceApplicationAccepted, value); }
        remove { DelDelegate<SEBNationAllianceApplicationAcceptedEventBody>(ServerEventName.NationAllianceApplicationAccepted, value); }
    }

    // 국가동맹신청취소
    public event Delegate<SEBNationAllianceApplicationCanceledEventBody> EventEvtNationAllianceApplicationCanceled
    {
        add { AddDelegate<SEBNationAllianceApplicationCanceledEventBody>(ServerEventName.NationAllianceApplicationCanceled, value); }
        remove { DelDelegate<SEBNationAllianceApplicationCanceledEventBody>(ServerEventName.NationAllianceApplicationCanceled, value); }
    }

    // 국가동맹신청거절
    public event Delegate<SEBNationAllianceApplicationRejectedEventBody> EventEvtNationAllianceApplicationRejected
    {
        add { AddDelegate<SEBNationAllianceApplicationRejectedEventBody>(ServerEventName.NationAllianceApplicationRejected, value); }
        remove { DelDelegate<SEBNationAllianceApplicationRejectedEventBody>(ServerEventName.NationAllianceApplicationRejected, value); }
    }

    // 국가동맹파기
    public event Delegate<SEBNationAllianceBrokenEventBody> EventEvtNationAllianceBroken
    {
        add { AddDelegate<SEBNationAllianceBrokenEventBody>(ServerEventName.NationAllianceBroken, value); }
        remove { DelDelegate<SEBNationAllianceBrokenEventBody>(ServerEventName.NationAllianceBroken, value); }
    }
    
    #endregion Event NationAlliance

    #region Event SceneryQuest

    //풍광퀘스트취소
    public event Delegate<SEBSceneryQuestCanceledEventBody> EventEvtSceneryQuestCanceled
    {
        add { AddDelegate<SEBSceneryQuestCanceledEventBody>(ServerEventName.SceneryQuestCanceled, value); }
        remove { DelDelegate<SEBSceneryQuestCanceledEventBody>(ServerEventName.SceneryQuestCanceled, value); }
    }

    //풍광퀘스트완료
    public event Delegate<SEBSceneryQuestCompletedEventBody> EventEvtSceneryQuestCompleted
    {
        add { AddDelegate<SEBSceneryQuestCompletedEventBody>(ServerEventName.SceneryQuestCompleted, value); }
        remove { DelDelegate<SEBSceneryQuestCompletedEventBody>(ServerEventName.SceneryQuestCompleted, value); }
    }

    #endregion Event SceneryQuest

    #region Event Accomplishment

    //누적몬스터킬횟수갱신
    public event Delegate<SEBAccMonsterKillCountUpdatedEventBody> EventEvtAccMonsterKillCountUpdated
    {
        add { AddDelegate<SEBAccMonsterKillCountUpdatedEventBody>(ServerEventName.AccMonsterKillCountUpdated, value); }
        remove { DelDelegate<SEBAccMonsterKillCountUpdatedEventBody>(ServerEventName.AccMonsterKillCountUpdated, value); }
    }

    //누적국가전총사령관킬횟수갱신
    public event Delegate<SEBAccNationWarCommanderKillCountUpdatedEventBody> EventEvtAccNationWarCommanderKillCountUpdated
    {
        add { AddDelegate<SEBAccNationWarCommanderKillCountUpdatedEventBody>(ServerEventName.AccNationWarCommanderKillCountUpdated, value); }
        remove { DelDelegate<SEBAccNationWarCommanderKillCountUpdatedEventBody>(ServerEventName.AccNationWarCommanderKillCountUpdated, value); }
    }

    #endregion Event Accomplishment

    #region Event Title

    //칭호수명종료
    public event Delegate<SEBTitleLifetimeEndedEventBody> EventEvtTitleLifetimeEnded
    {
        add { AddDelegate<SEBTitleLifetimeEndedEventBody>(ServerEventName.TitleLifetimeEnded, value); }
        remove { DelDelegate<SEBTitleLifetimeEndedEventBody>(ServerEventName.TitleLifetimeEnded, value); }
    }

    //영웅표시칭호변경
    public event Delegate<SEBHeroDisplayTitleChangedEventBody> EventEvtHeroDisplayTitleChanged
    {
        add { AddDelegate<SEBHeroDisplayTitleChangedEventBody>(ServerEventName.HeroDisplayTitleChanged, value); }
        remove { DelDelegate<SEBHeroDisplayTitleChangedEventBody>(ServerEventName.HeroDisplayTitleChanged, value); }
    }

    #endregion Event Title

    #region Event EliteMonster

    //정예몬스터스폰
    public event Delegate<SEBEliteMonsterSpawnEventBody> EventEvtEliteMonsterSpawn
    {
        add { AddDelegate<SEBEliteMonsterSpawnEventBody>(ServerEventName.EliteMonsterSpawn, value); }
        remove { DelDelegate<SEBEliteMonsterSpawnEventBody>(ServerEventName.EliteMonsterSpawn, value); }
    }

    //정예몬스터삭제
    public event Delegate<SEBEliteMonsterRemovedEventBody> EventEvtEliteMonsterRemoved
    {
        add { AddDelegate<SEBEliteMonsterRemovedEventBody>(ServerEventName.EliteMonsterRemoved, value); }
        remove { DelDelegate<SEBEliteMonsterRemovedEventBody>(ServerEventName.EliteMonsterRemoved, value); }
    }

    //정예몬스터킬횟수갱신
    public event Delegate<SEBEliteMonsterKillCountUpdatedEventBody> EventEvtEliteMonsterKillCountUpdated
    {
        add { AddDelegate<SEBEliteMonsterKillCountUpdatedEventBody>(ServerEventName.EliteMonsterKillCountUpdated, value); }
        remove { DelDelegate<SEBEliteMonsterKillCountUpdatedEventBody>(ServerEventName.EliteMonsterKillCountUpdated, value); }
    }

    #endregion Event EliteMonster

    #region Event EliteDungeon

    //정예던전시작
    public event Delegate<SEBEliteDungeonStartEventBody> EventEvtEliteDungeonStart
    {
        add { AddDelegate<SEBEliteDungeonStartEventBody>(ServerEventName.EliteDungeonStart, value); }
        remove { DelDelegate<SEBEliteDungeonStartEventBody>(ServerEventName.EliteDungeonStart, value); }
    }

    //정예던전클리어
    public event Delegate<SEBEliteDungeonClearEventBody> EventEvtEliteDungeonClear
    {
        add { AddDelegate<SEBEliteDungeonClearEventBody>(ServerEventName.EliteDungeonClear, value); }
        remove { DelDelegate<SEBEliteDungeonClearEventBody>(ServerEventName.EliteDungeonClear, value); }
    }

    //정예던전실패
    public event Delegate<SEBEliteDungeonFailEventBody> EventEvtEliteDungeonFail
    {
        add { AddDelegate<SEBEliteDungeonFailEventBody>(ServerEventName.EliteDungeonFail, value); }
        remove { DelDelegate<SEBEliteDungeonFailEventBody>(ServerEventName.EliteDungeonFail, value); }
    }

    //정예던전강퇴
    public event Delegate<SEBEliteDungeonBanishedEventBody> EventEvtEliteDungeonBanished
    {
        add { AddDelegate<SEBEliteDungeonBanishedEventBody>(ServerEventName.EliteDungeonBanished, value); }
        remove { DelDelegate<SEBEliteDungeonBanishedEventBody>(ServerEventName.EliteDungeonBanished, value); }
    }

    #endregion Event EliteDungeon

    #region Event CreatureCard

    //크리쳐카드상점갱신
    public event Delegate<SEBCreatureCardShopRefreshedEventBody> EventEvtCreatureCardShopRefreshed
    {
        add { AddDelegate<SEBCreatureCardShopRefreshedEventBody>(ServerEventName.CreatureCardShopRefreshed, value); }
        remove { DelDelegate<SEBCreatureCardShopRefreshedEventBody>(ServerEventName.CreatureCardShopRefreshed, value); }
    }

    #endregion Event CreatureCard

    #region Event ProofOfValor

    //용맹의증명시작
    public event Delegate<SEBProofOfValorStartEventBody> EventEvtProofOfValorStart
    {
        add { AddDelegate<SEBProofOfValorStartEventBody>(ServerEventName.ProofOfValorStart, value); }
        remove { DelDelegate<SEBProofOfValorStartEventBody>(ServerEventName.ProofOfValorStart, value); }
    }

    //용맹의증명클리어
    public event Delegate<SEBProofOfValorClearEventBody> EventEvtProofOfValorClear
    {
        add { AddDelegate<SEBProofOfValorClearEventBody>(ServerEventName.ProofOfValorClear, value); }
        remove { DelDelegate<SEBProofOfValorClearEventBody>(ServerEventName.ProofOfValorClear, value); }
    }

    //용맹의증명실패
    public event Delegate<SEBProofOfValorFailEventBody> EventEvtProofOfValorFail
    {
        add { AddDelegate<SEBProofOfValorFailEventBody>(ServerEventName.ProofOfValorFail, value); }
        remove { DelDelegate<SEBProofOfValorFailEventBody>(ServerEventName.ProofOfValorFail, value); }
    }

    //용맹의증명강퇴
    public event Delegate<SEBProofOfValorBanishedEventBody> EventEvtProofOfValorBanished
    {
        add { AddDelegate<SEBProofOfValorBanishedEventBody>(ServerEventName.ProofOfValorBanished, value); }
        remove { DelDelegate<SEBProofOfValorBanishedEventBody>(ServerEventName.ProofOfValorBanished, value); }
    }

    //용맹의증명버프상자생성
    public event Delegate<SEBProofOfValorBuffBoxCreatedEventBody> EventEvtProofOfValorBuffBoxCreated
    {
        add { AddDelegate<SEBProofOfValorBuffBoxCreatedEventBody>(ServerEventName.ProofOfValorBuffBoxCreated, value); }
        remove { DelDelegate<SEBProofOfValorBuffBoxCreatedEventBody>(ServerEventName.ProofOfValorBuffBoxCreated, value); }
    }

    //용맹의증명버프상자생명주기종료
    public event Delegate<SEBProofOfValorBuffBoxLifetimeEndedEventBody> EventEvtProofOfValorBuffBoxLifetimeEnded
    {
        add { AddDelegate<SEBProofOfValorBuffBoxLifetimeEndedEventBody>(ServerEventName.ProofOfValorBuffBoxLifetimeEnded, value); }
        remove { DelDelegate<SEBProofOfValorBuffBoxLifetimeEndedEventBody>(ServerEventName.ProofOfValorBuffBoxLifetimeEnded, value); }
    }

    //용맹의증명버프종료
    public event Delegate<SEBProofOfValorBuffFinishedEventBody> EventEvtProofOfValorBuffFinished
    {
        add { AddDelegate<SEBProofOfValorBuffFinishedEventBody>(ServerEventName.ProofOfValorBuffFinished, value); }
        remove { DelDelegate<SEBProofOfValorBuffFinishedEventBody>(ServerEventName.ProofOfValorBuffFinished, value); }
    }

    //크리쳐카드상점갱신
    public event Delegate<SEBProofOfValorRefreshedEventBody> EventEvtProofOfValorRefreshed
    {
        add { AddDelegate<SEBProofOfValorRefreshedEventBody>(ServerEventName.ProofOfValorRefreshed, value); }
        remove { DelDelegate<SEBProofOfValorRefreshedEventBody>(ServerEventName.ProofOfValorRefreshed, value); }
    }

    #endregion Event ProofOfValor

    #region Event Notice

    //공지
    public event Delegate<SEBNoticeEventBody> EventEvtNotice
    {
        add { AddDelegate<SEBNoticeEventBody>(ServerEventName.Notice, value); }
        remove { DelDelegate<SEBNoticeEventBody>(ServerEventName.Notice, value); }
    }

    #endregion Event Notice

    #region Event GroggyMonster

    //그로기몬스터아이템훔치기종료
    public event Delegate<SEBGroggyMonsterItemStealFinishedEventBody> EventEvtGroggyMonsterItemStealFinished
    {
        add { AddDelegate<SEBGroggyMonsterItemStealFinishedEventBody>(ServerEventName.GroggyMonsterItemStealFinished, value); }
        remove { DelDelegate<SEBGroggyMonsterItemStealFinishedEventBody>(ServerEventName.GroggyMonsterItemStealFinished, value); }
    }

    //그로기몬스터아이템훔치기취소
    public event Delegate<SEBGroggyMonsterItemStealCancelEventBody> EventEvtGroggyMonsterItemStealCancel
    {
        add { AddDelegate<SEBGroggyMonsterItemStealCancelEventBody>(ServerEventName.GroggyMonsterItemStealCancel, value); }
        remove { DelDelegate<SEBGroggyMonsterItemStealCancelEventBody>(ServerEventName.GroggyMonsterItemStealCancel, value); }
    }

    //영웅그로기몬스터아이템훔치기시작
    public event Delegate<SEBHeroGroggyMonsterItemStealStartEventBody> EventEvtHeroGroggyMonsterItemStealStart
    {
        add { AddDelegate<SEBHeroGroggyMonsterItemStealStartEventBody>(ServerEventName.HeroGroggyMonsterItemStealStart, value); }
        remove { DelDelegate<SEBHeroGroggyMonsterItemStealStartEventBody>(ServerEventName.HeroGroggyMonsterItemStealStart, value); }
    }

    //영웅그로기몬스터아이템훔치기종료
    public event Delegate<SEBHeroGroggyMonsterItemStealFinishedEventBody> EventEvtHeroGroggyMonsterItemStealFinished
    {
        add { AddDelegate<SEBHeroGroggyMonsterItemStealFinishedEventBody>(ServerEventName.HeroGroggyMonsterItemStealFinished, value); }
        remove { DelDelegate<SEBHeroGroggyMonsterItemStealFinishedEventBody>(ServerEventName.HeroGroggyMonsterItemStealFinished, value); }
    }

    //영웅그로기몬스터아이템훔치기취소
    public event Delegate<SEBHeroGroggyMonsterItemStealCancelEventBody> EventEvtHeroGroggyMonsterItemStealCancel
    {
        add { AddDelegate<SEBHeroGroggyMonsterItemStealCancelEventBody>(ServerEventName.HeroGroggyMonsterItemStealCancel, value); }
        remove { DelDelegate<SEBHeroGroggyMonsterItemStealCancelEventBody>(ServerEventName.HeroGroggyMonsterItemStealCancel, value); }
    }

    #endregion Event GroggyMonster

    #region Event DailyQuest

    //일일퀘스트진행카운트갱신
    public event Delegate<SEBHeroDailyQuestProgressCountUpdatedEventBody> EventEvtHeroDailyQuestProgressCountUpdated
    {
        add { AddDelegate<SEBHeroDailyQuestProgressCountUpdatedEventBody>(ServerEventName.HeroDailyQuestProgressCountUpdated, value); }
        remove { DelDelegate<SEBHeroDailyQuestProgressCountUpdatedEventBody>(ServerEventName.HeroDailyQuestProgressCountUpdated, value); }
    }

    //일일퀘스트생성
    public event Delegate<SEBHeroDailyQuestCreatedEventBody> EventEvtHeroDailyQuestCreated
    {
        add { AddDelegate<SEBHeroDailyQuestCreatedEventBody>(ServerEventName.HeroDailyQuestCreated, value); }
        remove { DelDelegate<SEBHeroDailyQuestCreatedEventBody>(ServerEventName.HeroDailyQuestCreated, value); }
    }

    #endregion Event DailyQuest

    #region Event WeeklyQuest

    //주간퀘스트생성
    public event Delegate<SEBWeeklyQuestCreatedEventBody> EventEvtWeeklyQuestCreated
    {
        add { AddDelegate<SEBWeeklyQuestCreatedEventBody>(ServerEventName.WeeklyQuestCreated, value); }
        remove { DelDelegate<SEBWeeklyQuestCreatedEventBody>(ServerEventName.WeeklyQuestCreated, value); }
    }

    //주간퀘스트라운드진행카운트갱신
    public event Delegate<SEBWeeklyQuestRoundProgressCountUpdatedEventBody> EventEvtWeeklyQuestRoundProgressCountUpdated
    {
        add { AddDelegate<SEBWeeklyQuestRoundProgressCountUpdatedEventBody>(ServerEventName.WeeklyQuestRoundProgressCountUpdated, value); }
        remove { DelDelegate<SEBWeeklyQuestRoundProgressCountUpdatedEventBody>(ServerEventName.WeeklyQuestRoundProgressCountUpdated, value); }
    }

    //주간퀘스트라운드완료
    public event Delegate<SEBWeeklyQuestRoundCompletedEventBody> EventEvtWeeklyQuestRoundCompleted
    {
        add { AddDelegate<SEBWeeklyQuestRoundCompletedEventBody>(ServerEventName.WeeklyQuestRoundCompleted, value); }
        remove { DelDelegate<SEBWeeklyQuestRoundCompletedEventBody>(ServerEventName.WeeklyQuestRoundCompleted, value); }
    }

    #endregion Event WeeklyQuest

    #region Event WisdomTemple

    //지혜의신전단계시작
    public event Delegate<SEBWisdomTempleStepStartEventBody> EventEvtWisdomTempleStepStart
    {
        add { AddDelegate<SEBWisdomTempleStepStartEventBody>(ServerEventName.WisdomTempleStepStart, value); }
        remove { DelDelegate<SEBWisdomTempleStepStartEventBody>(ServerEventName.WisdomTempleStepStart, value); }
    }

    //지혜의신전단계완료
    public event Delegate<SEBWisdomTempleStepCompletedEventBody> EventEvtWisdomTempleStepCompleted
    {
        add { AddDelegate<SEBWisdomTempleStepCompletedEventBody>(ServerEventName.WisdomTempleStepCompleted, value); }
        remove { DelDelegate<SEBWisdomTempleStepCompletedEventBody>(ServerEventName.WisdomTempleStepCompleted, value); }
    }

    //지혜의신전색맞추기오브젝트상호작용종료
    public event Delegate<SEBWisdomTempleColorMatchingObjectInteractionFinishedEventBody> EventEvtWisdomTempleColorMatchingObjectInteractionFinished
    {
        add { AddDelegate<SEBWisdomTempleColorMatchingObjectInteractionFinishedEventBody>(ServerEventName.WisdomTempleColorMatchingObjectInteractionFinished, value); }
        remove { DelDelegate<SEBWisdomTempleColorMatchingObjectInteractionFinishedEventBody>(ServerEventName.WisdomTempleColorMatchingObjectInteractionFinished, value); }
    }

    //지혜의신전색맞추기오브젝트상호작용취소
    public event Delegate<SEBWisdomTempleColorMatchingObjectInteractionCancelEventBody> EventEvtWisdomTempleColorMatchingObjectInteractionCancel
    {
        add { AddDelegate<SEBWisdomTempleColorMatchingObjectInteractionCancelEventBody>(ServerEventName.WisdomTempleColorMatchingObjectInteractionCancel, value); }
        remove { DelDelegate<SEBWisdomTempleColorMatchingObjectInteractionCancelEventBody>(ServerEventName.WisdomTempleColorMatchingObjectInteractionCancel, value); }
    }

    //지혜의신전색맞추기몬스터생성
    public event Delegate<SEBWisdomTempleColorMatchingMonsterCreatedEventBody> EventEvtWisdomTempleColorMatchingMonsterCreated
    {
        add { AddDelegate<SEBWisdomTempleColorMatchingMonsterCreatedEventBody>(ServerEventName.WisdomTempleColorMatchingMonsterCreated, value); }
        remove { DelDelegate<SEBWisdomTempleColorMatchingMonsterCreatedEventBody>(ServerEventName.WisdomTempleColorMatchingMonsterCreated, value); }
    }

    //지혜의신전색맞추기몬스터킬
    public event Delegate<SEBWisdomTempleColorMatchingMonsterKillEventBody> EventEvtWisdomTempleColorMatchingMonsterKill
    {
        add { AddDelegate<SEBWisdomTempleColorMatchingMonsterKillEventBody>(ServerEventName.WisdomTempleColorMatchingMonsterKill, value); }
        remove { DelDelegate<SEBWisdomTempleColorMatchingMonsterKillEventBody>(ServerEventName.WisdomTempleColorMatchingMonsterKill, value); }
    }

    //지혜의신전가짜보물상자킬
    public event Delegate<SEBWisdomTempleFakeTreasureBoxKillEventBody> EventEvtWisdomTempleFakeTreasureBoxKill
    {
        add { AddDelegate<SEBWisdomTempleFakeTreasureBoxKillEventBody>(ServerEventName.WisdomTempleFakeTreasureBoxKill, value); }
        remove { DelDelegate<SEBWisdomTempleFakeTreasureBoxKillEventBody>(ServerEventName.WisdomTempleFakeTreasureBoxKill, value); }
    }

    //지혜의신전퍼즐완료
    public event Delegate<SEBWisdomTemplePuzzleCompletedEventBody> EventEvtWisdomTemplePuzzleCompleted
    {
        add { AddDelegate<SEBWisdomTemplePuzzleCompletedEventBody>(ServerEventName.WisdomTemplePuzzleCompleted, value); }
        remove { DelDelegate<SEBWisdomTemplePuzzleCompletedEventBody>(ServerEventName.WisdomTemplePuzzleCompleted, value); }
    }

    //지혜의신전퍼즐보상오브젝트상호작용종료
    public event Delegate<SEBWisdomTemplePuzzleRewardObjectInteractionFinishedEventBody> EventEvtWisdomTemplePuzzleRewardObjectInteractionFinished
    {
        add { AddDelegate<SEBWisdomTemplePuzzleRewardObjectInteractionFinishedEventBody>(ServerEventName.WisdomTemplePuzzleRewardObjectInteractionFinished, value); }
        remove { DelDelegate<SEBWisdomTemplePuzzleRewardObjectInteractionFinishedEventBody>(ServerEventName.WisdomTemplePuzzleRewardObjectInteractionFinished, value); }
    }

    //지혜의신전퍼즐보상오브젝트상호작용취소
    public event Delegate<SEBWisdomTemplePuzzleRewardObjectInteractionCancelEventBody> EventEvtWisdomTemplePuzzleRewardObjectInteractionCancel
    {
        add { AddDelegate<SEBWisdomTemplePuzzleRewardObjectInteractionCancelEventBody>(ServerEventName.WisdomTemplePuzzleRewardObjectInteractionCancel, value); }
        remove { DelDelegate<SEBWisdomTemplePuzzleRewardObjectInteractionCancelEventBody>(ServerEventName.WisdomTemplePuzzleRewardObjectInteractionCancel, value); }
    }

    //지혜의신전퀴즈실패
    public event Delegate<SEBWisdomTempleQuizFailEventBody> EventEvtWisdomTempleQuizFail
    {
        add { AddDelegate<SEBWisdomTempleQuizFailEventBody>(ServerEventName.WisdomTempleQuizFail, value); }
        remove { DelDelegate<SEBWisdomTempleQuizFailEventBody>(ServerEventName.WisdomTempleQuizFail, value); }
    }

    //지혜의신전보스몬스터생성
    public event Delegate<SEBWisdomTempleBossMonsterCreatedEventBody> EventEvtWisdomTempleBossMonsterCreated
    {
        add { AddDelegate<SEBWisdomTempleBossMonsterCreatedEventBody>(ServerEventName.WisdomTempleBossMonsterCreated, value); }
        remove { DelDelegate<SEBWisdomTempleBossMonsterCreatedEventBody>(ServerEventName.WisdomTempleBossMonsterCreated, value); }
    }

    //지혜의신전보스몬스터킬
    public event Delegate<SEBWisdomTempleBossMonsterKillEventBody> EventEvtWisdomTempleBossMonsterKill
    {
        add { AddDelegate<SEBWisdomTempleBossMonsterKillEventBody>(ServerEventName.WisdomTempleBossMonsterKill, value); }
        remove { DelDelegate<SEBWisdomTempleBossMonsterKillEventBody>(ServerEventName.WisdomTempleBossMonsterKill, value); }
    }

    //지혜의신전클리어
    public event Delegate<SEBWisdomTempleClearEventBody> EventEvtWisdomTempleClear
    {
        add { AddDelegate<SEBWisdomTempleClearEventBody>(ServerEventName.WisdomTempleClear, value); }
        remove { DelDelegate<SEBWisdomTempleClearEventBody>(ServerEventName.WisdomTempleClear, value); }
    }

    //지혜의신전실패
    public event Delegate<SEBWisdomTempleFailEventBody> EventEvtWisdomTempleFail
    {
        add { AddDelegate<SEBWisdomTempleFailEventBody>(ServerEventName.WisdomTempleFail, value); }
        remove { DelDelegate<SEBWisdomTempleFailEventBody>(ServerEventName.WisdomTempleFail, value); }
    }

    //지혜의신전강퇴
    public event Delegate<SEBWisdomTempleBanishedEventBody> EventEvtWisdomTempleBanished
    {
        add { AddDelegate<SEBWisdomTempleBanishedEventBody>(ServerEventName.WisdomTempleBanished, value); }
        remove { DelDelegate<SEBWisdomTempleBanishedEventBody>(ServerEventName.WisdomTempleBanished, value); }
    }

    #endregion Event WisdomTemple

    #region Event Open7Day

    // 오픈7일이벤트진행카운트갱신
    public event Delegate<SEBOpen7DayEventProgressCountUpdatedEventBody> EventEvtOpen7DayEventProgressCountUpdated
    {
        add { AddDelegate<SEBOpen7DayEventProgressCountUpdatedEventBody>(ServerEventName.Open7DayEventProgressCountUpdated, value); }
        remove { DelDelegate<SEBOpen7DayEventProgressCountUpdatedEventBody>(ServerEventName.Open7DayEventProgressCountUpdated, value); }
    }

    #endregion Event Open7Day

    #region Event Retrieve

    // 회수진행카운트갱신
    public event Delegate<SEBRetrievalProgressCountUpdatedEventBody> EventEvtRetrievalProgressCountUpdated
    {
        add { AddDelegate<SEBRetrievalProgressCountUpdatedEventBody>(ServerEventName.RetrievalProgressCountUpdated, value); }
        remove { DelDelegate<SEBRetrievalProgressCountUpdatedEventBody>(ServerEventName.RetrievalProgressCountUpdated, value); }
    }

    #endregion Event Retrieve

    #region Event RuinsReclaim

    // 유적탈환입장에대한대륙퇴장
    public event Delegate<SEBContinentExitForRuinsReclaimEnterEventBody> EventEvtContinentExitForRuinsReclaimEnter
    {
        add { AddDelegate<SEBContinentExitForRuinsReclaimEnterEventBody>(ServerEventName.ContinentExitForRuinsReclaimEnter, value); }
        remove { DelDelegate<SEBContinentExitForRuinsReclaimEnterEventBody>(ServerEventName.ContinentExitForRuinsReclaimEnter, value); }
    }

    // 유적탈환매칭방파티입장
    public event Delegate<SEBRuinsReclaimMatchingRoomPartyEnterEventBody> EventEvtRuinsReclaimMatchingRoomPartyEnter
    {
        add { AddDelegate<SEBRuinsReclaimMatchingRoomPartyEnterEventBody>(ServerEventName.RuinsReclaimMatchingRoomPartyEnter, value); }
        remove { DelDelegate<SEBRuinsReclaimMatchingRoomPartyEnterEventBody>(ServerEventName.RuinsReclaimMatchingRoomPartyEnter, value); }
    }

    // 유적탈환매칭상태변경
    public event Delegate<SEBRuinsReclaimMatchingStatusChangedEventBody> EventEvtRuinsReclaimMatchingStatusChanged
    {
        add { AddDelegate<SEBRuinsReclaimMatchingStatusChangedEventBody>(ServerEventName.RuinsReclaimMatchingStatusChanged, value); }
        remove { DelDelegate<SEBRuinsReclaimMatchingStatusChangedEventBody>(ServerEventName.RuinsReclaimMatchingStatusChanged, value); }
    }

    // 유적탈환매칭방강퇴
    public event Delegate<SEBRuinsReclaimMatchingRoomBanishedEventBody> EventEvtRuinsReclaimMatchingRoomBanished
    {
        add { AddDelegate<SEBRuinsReclaimMatchingRoomBanishedEventBody>(ServerEventName.RuinsReclaimMatchingRoomBanished, value); }
        remove { DelDelegate<SEBRuinsReclaimMatchingRoomBanishedEventBody>(ServerEventName.RuinsReclaimMatchingRoomBanished, value); }
    }

    // 유적탈환단계시작
    public event Delegate<SEBRuinsReclaimStepStartEventBody> EventEvtRuinsReclaimStepStart
    {
        add { AddDelegate<SEBRuinsReclaimStepStartEventBody>(ServerEventName.RuinsReclaimStepStart, value); }
        remove { DelDelegate<SEBRuinsReclaimStepStartEventBody>(ServerEventName.RuinsReclaimStepStart, value); }
    }

    // 유적탈환단계완료
    public event Delegate<SEBRuinsReclaimStepCompletedEventBody> EventEvtRuinsReclaimStepCompleted
    {
        add { AddDelegate<SEBRuinsReclaimStepCompletedEventBody>(ServerEventName.RuinsReclaimStepCompleted, value); }
        remove { DelDelegate<SEBRuinsReclaimStepCompletedEventBody>(ServerEventName.RuinsReclaimStepCompleted, value); }
    }

    // 유적탈환보상오브젝트상호작용취소
    public event Delegate<SEBRuinsReclaimRewardObjectInteractionCancelEventBody> EventEvtRuinsReclaimRewardObjectInteractionCancel
    {
        add { AddDelegate<SEBRuinsReclaimRewardObjectInteractionCancelEventBody>(ServerEventName.RuinsReclaimRewardObjectInteractionCancel, value); }
        remove { DelDelegate<SEBRuinsReclaimRewardObjectInteractionCancelEventBody>(ServerEventName.RuinsReclaimRewardObjectInteractionCancel, value); }
    }

    // 유적탈환보상오브젝트상호작용종료
    public event Delegate<SEBRuinsReclaimRewardObjectInteractionFinishedEventBody> EventEvtRuinsReclaimRewardObjectInteractionFinished
    {
        add { AddDelegate<SEBRuinsReclaimRewardObjectInteractionFinishedEventBody>(ServerEventName.RuinsReclaimRewardObjectInteractionFinished, value); }
        remove { DelDelegate<SEBRuinsReclaimRewardObjectInteractionFinishedEventBody>(ServerEventName.RuinsReclaimRewardObjectInteractionFinished, value); }
    }

    // 영웅유적탈환보상오브젝트상호작용시작
    public event Delegate<SEBHeroRuinsReclaimRewardObjectInteractionStartEventBody> EventEvtHeroRuinsReclaimRewardObjectInteractionStart
    {
        add { AddDelegate<SEBHeroRuinsReclaimRewardObjectInteractionStartEventBody>(ServerEventName.HeroRuinsReclaimRewardObjectInteractionStart, value); }
        remove { DelDelegate<SEBHeroRuinsReclaimRewardObjectInteractionStartEventBody>(ServerEventName.HeroRuinsReclaimRewardObjectInteractionStart, value); }
    }

    // 영웅유적탈환보상오브젝트상호작용취소
    public event Delegate<SEBHeroRuinsReclaimRewardObjectInteractionCancelEventBody> EventEvtHeroHeroRuinsReclaimRewardObjectInteractionCancel
    {
        add { AddDelegate<SEBHeroRuinsReclaimRewardObjectInteractionCancelEventBody>(ServerEventName.HeroRuinsReclaimRewardObjectInteractionCancel, value); }
        remove { DelDelegate<SEBHeroRuinsReclaimRewardObjectInteractionCancelEventBody>(ServerEventName.HeroRuinsReclaimRewardObjectInteractionCancel, value); }
    }

    // 영웅유적탈환보상오브젝트상호작용종료
    public event Delegate<SEBHeroRuinsReclaimRewardObjectInteractionFinishedEventBody> EventEvtHeroRuinsReclaimRewardObjectInteractionFinished
    {
        add { AddDelegate<SEBHeroRuinsReclaimRewardObjectInteractionFinishedEventBody>(ServerEventName.HeroRuinsReclaimRewardObjectInteractionFinished, value); }
        remove { DelDelegate<SEBHeroRuinsReclaimRewardObjectInteractionFinishedEventBody>(ServerEventName.HeroRuinsReclaimRewardObjectInteractionFinished, value); }
    }

    // 유적탈환웨이브시작
    public event Delegate<SEBRuinsReclaimWaveStartEventBody> EventEvtRuinsReclaimWaveStart
    {
        add { AddDelegate<SEBRuinsReclaimWaveStartEventBody>(ServerEventName.RuinsReclaimWaveStart, value); }
        remove { DelDelegate<SEBRuinsReclaimWaveStartEventBody>(ServerEventName.RuinsReclaimWaveStart, value); }
    }

    // 유적탈환웨이브완료
    public event Delegate<SEBRuinsReclaimWaveCompletedEventBody> EventEvtRuinsReclaimWaveCompleted
    {
        add { AddDelegate<SEBRuinsReclaimWaveCompletedEventBody>(ServerEventName.RuinsReclaimWaveCompleted, value); }
        remove { DelDelegate<SEBRuinsReclaimWaveCompletedEventBody>(ServerEventName.RuinsReclaimWaveCompleted, value); }
    }

    // 유적탈환단계웨이브스킬시전
    public event Delegate<SEBRuinsReclaimStepWaveSkillCastEventBody> EventEvtRuinsReclaimStepWaveSkillCast
    {
        add { AddDelegate<SEBRuinsReclaimStepWaveSkillCastEventBody>(ServerEventName.RuinsReclaimStepWaveSkillCast, value); }
        remove { DelDelegate<SEBRuinsReclaimStepWaveSkillCastEventBody>(ServerEventName.RuinsReclaimStepWaveSkillCast, value); }
    }

    // 유적탈환몬스터변신시작
    public event Delegate<SEBRuinsReclaimMonsterTransformationStartEventBody> EventEvtRuinsReclaimMonsterTransformationStart
    {
        add { AddDelegate<SEBRuinsReclaimMonsterTransformationStartEventBody>(ServerEventName.RuinsReclaimMonsterTransformationStart, value); }
        remove { DelDelegate<SEBRuinsReclaimMonsterTransformationStartEventBody>(ServerEventName.RuinsReclaimMonsterTransformationStart, value); }
    }

    // 유적탈환몬스터변신종료
    public event Delegate<SEBRuinsReclaimMonsterTransformationFinishedEventBody> EventEvtRuinsReclaimMonsterTransformationFinished
    {
        add { AddDelegate<SEBRuinsReclaimMonsterTransformationFinishedEventBody>(ServerEventName.RuinsReclaimMonsterTransformationFinished, value); }
        remove { DelDelegate<SEBRuinsReclaimMonsterTransformationFinishedEventBody>(ServerEventName.RuinsReclaimMonsterTransformationFinished, value); }
    }

	// 유적탈환몬스터변신취소오브젝트수명종료
	public event Delegate<SEBRuinsReclaimMonsterTransformationCancelObjectLifetimeEndedEventBody> EventEvtRuinsReclaimMonsterTransformationCancelObjectLifetimeEnded
	{
		add { AddDelegate<SEBRuinsReclaimMonsterTransformationCancelObjectLifetimeEndedEventBody>(ServerEventName.RuinsReclaimMonsterTransformationCancelObjectLifetimeEnded, value); }
		remove { DelDelegate<SEBRuinsReclaimMonsterTransformationCancelObjectLifetimeEndedEventBody>(ServerEventName.RuinsReclaimMonsterTransformationCancelObjectLifetimeEnded, value); }
	}

	// 영웅유적탈환몬스터변신시작
	public event Delegate<SEBHeroRuinsReclaimMonsterTransformationStartEventBody> EventEvtHeroRuinsReclaimMonsterTransformationStart
    {
        add { AddDelegate<SEBHeroRuinsReclaimMonsterTransformationStartEventBody>(ServerEventName.HeroRuinsReclaimMonsterTransformationStart, value); }
        remove { DelDelegate<SEBHeroRuinsReclaimMonsterTransformationStartEventBody>(ServerEventName.HeroRuinsReclaimMonsterTransformationStart, value); }
    }

    // 영웅유적탈환몬스터변신종료
    public event Delegate<SEBHeroRuinsReclaimMonsterTransformationFinishedEventBody> EventEvtHeroRuinsReclaimMonsterTransformationFinished
    {
        add { AddDelegate<SEBHeroRuinsReclaimMonsterTransformationFinishedEventBody>(ServerEventName.HeroRuinsReclaimMonsterTransformationFinished, value); }
        remove { DelDelegate<SEBHeroRuinsReclaimMonsterTransformationFinishedEventBody>(ServerEventName.HeroRuinsReclaimMonsterTransformationFinished, value); }
    }

	// 유적탈환몬스터변신취소오브젝트상호작용취소
	public event Delegate<SEBRuinsReclaimMonsterTransformationCancelObjectInteractionCancelEventBody> EventEvtRuinsReclaimMonsterTransformationCancelObjectInteractionCancel
    {
        add { AddDelegate<SEBRuinsReclaimMonsterTransformationCancelObjectInteractionCancelEventBody>(ServerEventName.RuinsReclaimMonsterTransformationCancelObjectInteractionCancel, value); }
        remove { DelDelegate<SEBRuinsReclaimMonsterTransformationCancelObjectInteractionCancelEventBody>(ServerEventName.RuinsReclaimMonsterTransformationCancelObjectInteractionCancel, value); }
    }

    // 유적탈환몬스터변신취소오브젝트상호작용종료
    public event Delegate<SEBRuinsReclaimMonsterTransformationCancelObjectInteractionFinishedEventBody> EventEvtRuinsReclaimMonsterTransformationCancelObjectInteractionFinished
    {
        add { AddDelegate<SEBRuinsReclaimMonsterTransformationCancelObjectInteractionFinishedEventBody>(ServerEventName.RuinsReclaimMonsterTransformationCancelObjectInteractionFinished, value); }
        remove { DelDelegate<SEBRuinsReclaimMonsterTransformationCancelObjectInteractionFinishedEventBody>(ServerEventName.RuinsReclaimMonsterTransformationCancelObjectInteractionFinished, value); }
    }

    // 영웅유적탈환몬스터변신취소오브젝트상호작용시작
    public event Delegate<SEBHeroRuinsReclaimMonsterTransformationCancelObjectInteractionStartEventBody> EventEvtHeroRuinsReclaimMonsterTransformationCancelObjectInteractionStart
    {
        add { AddDelegate<SEBHeroRuinsReclaimMonsterTransformationCancelObjectInteractionStartEventBody>(ServerEventName.HeroRuinsReclaimMonsterTransformationCancelObjectInteractionStart, value); }
        remove { DelDelegate<SEBHeroRuinsReclaimMonsterTransformationCancelObjectInteractionStartEventBody>(ServerEventName.HeroRuinsReclaimMonsterTransformationCancelObjectInteractionStart, value); }
    }

    // 영웅유적탈환몬스터변신취소오브젝트상호작용취소
    public event Delegate<SEBHeroRuinsReclaimMonsterTransformationCancelObjectInteractionCancelEventBody> EventEvtHeroRuinsReclaimMonsterTransformationCancelObjectInteractionCancel
    {
        add { AddDelegate<SEBHeroRuinsReclaimMonsterTransformationCancelObjectInteractionCancelEventBody>(ServerEventName.HeroRuinsReclaimMonsterTransformationCancelObjectInteractionCancel, value); }
        remove { DelDelegate<SEBHeroRuinsReclaimMonsterTransformationCancelObjectInteractionCancelEventBody>(ServerEventName.HeroRuinsReclaimMonsterTransformationCancelObjectInteractionCancel, value); }
    }

    // 영웅유적탈환몬스터변신취소오브젝트상호작용종료
    public event Delegate<SEBHeroRuinsReclaimMonsterTransformationCancelObjectInteractionFinishedEventBody> EventEvtHeroRuinsReclaimMonsterTransformationCancelObjectInteractionFinished
    {
        add { AddDelegate<SEBHeroRuinsReclaimMonsterTransformationCancelObjectInteractionFinishedEventBody>(ServerEventName.HeroRuinsReclaimMonsterTransformationCancelObjectInteractionFinished, value); }
        remove { DelDelegate<SEBHeroRuinsReclaimMonsterTransformationCancelObjectInteractionFinishedEventBody>(ServerEventName.HeroRuinsReclaimMonsterTransformationCancelObjectInteractionFinished, value); }
    }

    // 유적탈환몬스터소환
    public event Delegate<SEBRuinsReclaimMonsterSummonEventBody> EventEvtRuinsReclaimMonsterSummon
    {
        add { AddDelegate<SEBRuinsReclaimMonsterSummonEventBody>(ServerEventName.RuinsReclaimMonsterSummon, value); }
        remove { DelDelegate<SEBRuinsReclaimMonsterSummonEventBody>(ServerEventName.RuinsReclaimMonsterSummon, value); }
    }

    // 유적탈환함정적중
    public event Delegate<SEBRuinsReclaimTrapHitEventBody> EventEvtRuinsReclaimTrapHit
    {
        add { AddDelegate<SEBRuinsReclaimTrapHitEventBody>(ServerEventName.RuinsReclaimTrapHit, value); }
        remove { DelDelegate<SEBRuinsReclaimTrapHitEventBody>(ServerEventName.RuinsReclaimTrapHit, value); }
    }

    // 유적탈환디버프효과시작
    public event Delegate<SEBRuinsReclaimDebuffEffectStartEventBody> EventEvtRuinsReclaimDebuffEffectStart
    {
        add { AddDelegate<SEBRuinsReclaimDebuffEffectStartEventBody>(ServerEventName.RuinsReclaimDebuffEffectStart, value); }
        remove { DelDelegate<SEBRuinsReclaimDebuffEffectStartEventBody>(ServerEventName.RuinsReclaimDebuffEffectStart, value); }
    }

    // 유적탈환디버프효과중지
    public event Delegate<SEBRuinsReclaimDebuffEffectStopEventBody> EventEvtRuinsReclaimDebuffEffectStop
    {
        add { AddDelegate<SEBRuinsReclaimDebuffEffectStopEventBody>(ServerEventName.RuinsReclaimDebuffEffectStop, value); }
        remove { DelDelegate<SEBRuinsReclaimDebuffEffectStopEventBody>(ServerEventName.RuinsReclaimDebuffEffectStop, value); }
    }

    // 영웅유적탈환포탈입장
    public event Delegate<SEBHeroRuinsReclaimPortalEnterEventBody> EventEvtHeroRuinsReclaimPortalEnter
    {
        add { AddDelegate<SEBHeroRuinsReclaimPortalEnterEventBody>(ServerEventName.HeroRuinsReclaimPortalEnter, value); }
        remove { DelDelegate<SEBHeroRuinsReclaimPortalEnterEventBody>(ServerEventName.HeroRuinsReclaimPortalEnter, value); }
    }

    // 유적탈환클리어
    public event Delegate<SEBRuinsReclaimClearEventBody> EventEvtRuinsReclaimClear
    {
        add { AddDelegate<SEBRuinsReclaimClearEventBody>(ServerEventName.RuinsReclaimClear, value); }
        remove { DelDelegate<SEBRuinsReclaimClearEventBody>(ServerEventName.RuinsReclaimClear, value); }
    }

    // 유적탈환실패
    public event Delegate<SEBRuinsReclaimFailEventBody> EventEvtRuinsReclaimFail
    {
        add { AddDelegate<SEBRuinsReclaimFailEventBody>(ServerEventName.RuinsReclaimFail, value); }
        remove { DelDelegate<SEBRuinsReclaimFailEventBody>(ServerEventName.RuinsReclaimFail, value); }
    }

    // 유적탈환강퇴
    public event Delegate<SEBRuinsReclaimBanishedEventBody> EventEvtRuinsReclaimBanished
    {
        add { AddDelegate<SEBRuinsReclaimBanishedEventBody>(ServerEventName.RuinsReclaimBanished, value); }
        remove { DelDelegate<SEBRuinsReclaimBanishedEventBody>(ServerEventName.RuinsReclaimBanished, value); }
    }

    #endregion Event RuinsReclaim

    #region Event HeroTrueHeroQuest

    // 영웅진정한영웅퀘스트단계상호작용시작
    public event Delegate<SEBHeroTrueHeroQuestStepInteractionStartedEventBody> EventEvtHeroTrueHeroQuestStepInteractionStarted
    {
        add { AddDelegate<SEBHeroTrueHeroQuestStepInteractionStartedEventBody>(ServerEventName.HeroTrueHeroQuestStepInteractionStarted, value); }
        remove { DelDelegate<SEBHeroTrueHeroQuestStepInteractionStartedEventBody>(ServerEventName.HeroTrueHeroQuestStepInteractionStarted, value); }
    }

    // 영웅진정한영웅퀘스트단계상호작용취소
    public event Delegate<SEBHeroTrueHeroQuestStepInteractionCanceledEventBody> EventEvtHeroTrueHeroQuestStepInteractionCanceled
    {
        add { AddDelegate<SEBHeroTrueHeroQuestStepInteractionCanceledEventBody>(ServerEventName.HeroTrueHeroQuestStepInteractionCanceled, value); }
        remove { DelDelegate<SEBHeroTrueHeroQuestStepInteractionCanceledEventBody>(ServerEventName.HeroTrueHeroQuestStepInteractionCanceled, value); }
    }

    // 영웅진정한영웅퀘스트단계상호작용완료
    public event Delegate<SEBHeroTrueHeroQuestStepInteractionFinishedEventBody> EventEvtHeroTrueHeroQuestStepInteractionFinished
    {
        add { AddDelegate<SEBHeroTrueHeroQuestStepInteractionFinishedEventBody>(ServerEventName.HeroTrueHeroQuestStepInteractionFinished, value); }
        remove { DelDelegate<SEBHeroTrueHeroQuestStepInteractionFinishedEventBody>(ServerEventName.HeroTrueHeroQuestStepInteractionFinished, value); }
    }

    // 진정한영웅도발
    public event Delegate<SEBTrueHeroQuestTauntedEventBody> EventEvtTrueHeroQuestTaunted
    {
        add { AddDelegate<SEBTrueHeroQuestTauntedEventBody>(ServerEventName.TrueHeroQuestTaunted, value); }
        remove { DelDelegate<SEBTrueHeroQuestTauntedEventBody>(ServerEventName.TrueHeroQuestTaunted, value); }
    }

    // 진정한영웅단계상호작용취소
    public event Delegate<SEBTrueHeroQuestStepInteractionCanceledEventBody> EventEvtTrueHeroQuestStepInteractionCanceled
    {
        add { AddDelegate<SEBTrueHeroQuestStepInteractionCanceledEventBody>(ServerEventName.TrueHeroQuestStepInteractionCanceled, value); }
        remove { DelDelegate<SEBTrueHeroQuestStepInteractionCanceledEventBody>(ServerEventName.TrueHeroQuestStepInteractionCanceled, value); }
    }

    // 진정한영웅단계상호작용완료
    public event Delegate<SEBTrueHeroQuestStepInteractionFinishedEventBody> EventEvtTrueHeroQuestStepInteractionFinished
    {
        add { AddDelegate<SEBTrueHeroQuestStepInteractionFinishedEventBody>(ServerEventName.TrueHeroQuestStepInteractionFinished, value); }
        remove { DelDelegate<SEBTrueHeroQuestStepInteractionFinishedEventBody>(ServerEventName.TrueHeroQuestStepInteractionFinished, value); }
    }

    // 진정한영웅단계대기취소
    public event Delegate<SEBTrueHeroQuestStepWaitingCanceledEventBody> EventEvtTrueHeroQuestStepWaitingCanceled
    {
        add { AddDelegate<SEBTrueHeroQuestStepWaitingCanceledEventBody>(ServerEventName.TrueHeroQuestStepWaitingCanceled, value); }
        remove { DelDelegate<SEBTrueHeroQuestStepWaitingCanceledEventBody>(ServerEventName.TrueHeroQuestStepWaitingCanceled, value); }
    }

    // 진정한영웅단계완료
    public event Delegate<SEBTrueHeroQuestStepCompletedEventBody> EventEvtTrueHeroQuestStepCompleted
    {
        add { AddDelegate<SEBTrueHeroQuestStepCompletedEventBody>(ServerEventName.TrueHeroQuestStepCompleted, value); }
        remove { DelDelegate<SEBTrueHeroQuestStepCompletedEventBody>(ServerEventName.TrueHeroQuestStepCompleted, value); }
    }

    #endregion Event HeroTrueHeroQuest

    #region Event InfiniteWar

    // 무한대전입장에대한대륙퇴장
    public event Delegate<SEBContinentExitForInfiniteWarEnterEventBody> EventEvtContinentExitForInfiniteWarEnter
    {
        add { AddDelegate<SEBContinentExitForInfiniteWarEnterEventBody>(ServerEventName.ContinentExitForInfiniteWarEnter, value); }
        remove { DelDelegate<SEBContinentExitForInfiniteWarEnterEventBody>(ServerEventName.ContinentExitForInfiniteWarEnter, value); }
    }

    // 무한대전매칭상태변경
    public event Delegate<SEBInfiniteWarMatchingStatusChangedEventBody> EventEvtInfiniteWarMatchingStatusChanged
    {
        add { AddDelegate<SEBInfiniteWarMatchingStatusChangedEventBody>(ServerEventName.InfiniteWarMatchingStatusChanged, value); }
        remove { DelDelegate<SEBInfiniteWarMatchingStatusChangedEventBody>(ServerEventName.InfiniteWarMatchingStatusChanged, value); }
    }

    // 무한대전매칭방강퇴
    public event Delegate<SEBInfiniteWarMatchingRoomBanishedEventBody> EventEvtInfiniteWarMatchingRoomBanished
    {
        add { AddDelegate<SEBInfiniteWarMatchingRoomBanishedEventBody>(ServerEventName.InfiniteWarMatchingRoomBanished, value); }
        remove { DelDelegate<SEBInfiniteWarMatchingRoomBanishedEventBody>(ServerEventName.InfiniteWarMatchingRoomBanished, value); }
    }

    // 무한대전시작
    public event Delegate<SEBInfiniteWarStartEventBody> EventEvtInfiniteWarStart
    {
        add { AddDelegate<SEBInfiniteWarStartEventBody>(ServerEventName.InfiniteWarStart, value); }
        remove { DelDelegate<SEBInfiniteWarStartEventBody>(ServerEventName.InfiniteWarStart, value); }
    }

    // 무한대전몬스터스폰
    public event Delegate<SEBInfiniteWarMonsterSpawnEventBody> EventEvtInfiniteWarMonsterSpawn
    {
        add { AddDelegate<SEBInfiniteWarMonsterSpawnEventBody>(ServerEventName.InfiniteWarMonsterSpawn, value); }
        remove { DelDelegate<SEBInfiniteWarMonsterSpawnEventBody>(ServerEventName.InfiniteWarMonsterSpawn, value); }
    }

    // 무한대전버프상자생성
    public event Delegate<SEBInfiniteWarBuffBoxCreatedEventBody> EventEvtInfiniteWarBuffBoxCreated
    {
        add { AddDelegate<SEBInfiniteWarBuffBoxCreatedEventBody>(ServerEventName.InfiniteWarBuffBoxCreated, value); }
        remove { DelDelegate<SEBInfiniteWarBuffBoxCreatedEventBody>(ServerEventName.InfiniteWarBuffBoxCreated, value); }
    }

    // 무한대전버프상자수명종료
    public event Delegate<SEBInfiniteWarBuffBoxLifetimeEndedEventBody> EventEvtInfiniteWarBuffBoxLifetimeEnded
    {
        add { AddDelegate<SEBInfiniteWarBuffBoxLifetimeEndedEventBody>(ServerEventName.InfiniteWarBuffBoxLifetimeEnded, value); }
        remove { DelDelegate<SEBInfiniteWarBuffBoxLifetimeEndedEventBody>(ServerEventName.InfiniteWarBuffBoxLifetimeEnded, value); }
    }

    // 영웅무한대전버프상자획득
    public event Delegate<SEBHeroInfiniteWarBuffBoxAcquisitionEventBody> EventEvtHeroInfiniteWarBuffBoxAcquisition
    {
        add { AddDelegate<SEBHeroInfiniteWarBuffBoxAcquisitionEventBody>(ServerEventName.HeroInfiniteWarBuffBoxAcquisition, value); }
        remove { DelDelegate<SEBHeroInfiniteWarBuffBoxAcquisitionEventBody>(ServerEventName.HeroInfiniteWarBuffBoxAcquisition, value); }
    }

    // 무한대전버프종료
    public event Delegate<SEBInfiniteWarBuffFinishedEventBody> EventEvtInfiniteWarBuffFinished
    {
        add { AddDelegate<SEBInfiniteWarBuffFinishedEventBody>(ServerEventName.InfiniteWarBuffFinished, value); }
        remove { DelDelegate<SEBInfiniteWarBuffFinishedEventBody>(ServerEventName.InfiniteWarBuffFinished, value); }
    }

    // 무한대전점수획득
    public event Delegate<SEBInfiniteWarPointAcquisitionEventBody> EventEvtInfiniteWarPointAcquisition
    {
        add { AddDelegate<SEBInfiniteWarPointAcquisitionEventBody>(ServerEventName.InfiniteWarPointAcquisition, value); }
        remove { DelDelegate<SEBInfiniteWarPointAcquisitionEventBody>(ServerEventName.InfiniteWarPointAcquisition, value); }
    }

    // 영웅무한대전점수획득
    public event Delegate<SEBHeroInfiniteWarPointAcquisitionEventBody> EventEvtHeroInfiniteWarPointAcquisition
    {
        add { AddDelegate<SEBHeroInfiniteWarPointAcquisitionEventBody>(ServerEventName.HeroInfiniteWarPointAcquisition, value); }
        remove { DelDelegate<SEBHeroInfiniteWarPointAcquisitionEventBody>(ServerEventName.HeroInfiniteWarPointAcquisition, value); }
    }

    // 무한대전클리어
    public event Delegate<SEBInfiniteWarClearEventBody> EventEvtInfiniteWarClear
    {
        add { AddDelegate<SEBInfiniteWarClearEventBody>(ServerEventName.InfiniteWarClear, value); }
        remove { DelDelegate<SEBInfiniteWarClearEventBody>(ServerEventName.InfiniteWarClear, value); }
    }

    // 무한대전강퇴
    public event Delegate<SEBInfiniteWarBanishedEventBody> EventEvtInfiniteWarBanished
    {
        add { AddDelegate<SEBInfiniteWarBanishedEventBody>(ServerEventName.InfiniteWarBanished, value); }
        remove { DelDelegate<SEBInfiniteWarBanishedEventBody>(ServerEventName.InfiniteWarBanished, value); }
    }

    #endregion Event InfiniteWar

    #region Event FieldBoss

    // 필드보스이벤트시작
    public event Delegate<SEBFieldBossEventStartedEventBody> EventEvtFieldBossEventStarted
    {
        add { AddDelegate<SEBFieldBossEventStartedEventBody>(ServerEventName.FieldBossEventStarted, value); }
        remove { DelDelegate<SEBFieldBossEventStartedEventBody>(ServerEventName.FieldBossEventStarted, value); }
    }

    // 필드보스이벤트종료
    public event Delegate<SEBFieldBossEventEndedEventBody> EventEvtFieldBossEventEnded
    {
        add { AddDelegate<SEBFieldBossEventEndedEventBody>(ServerEventName.FieldBossEventEnded, value); }
        remove { DelDelegate<SEBFieldBossEventEndedEventBody>(ServerEventName.FieldBossEventEnded, value); }
    }

    // 필드보스사망
    public event Delegate<SEBFieldBossDeadEventBody> EventEvtFieldBossDead
    {
        add { AddDelegate<SEBFieldBossDeadEventBody>(ServerEventName.FieldBossDead, value); }
        remove { DelDelegate<SEBFieldBossDeadEventBody>(ServerEventName.FieldBossDead, value); }
    }

    // 필드보스보상루팅
    public event Delegate<SEBFieldBossRewardLootedEventBody> EventEvtFieldBossRewardLooted
    {
        add { AddDelegate<SEBFieldBossRewardLootedEventBody>(ServerEventName.FieldBossRewardLooted, value); }
        remove { DelDelegate<SEBFieldBossRewardLootedEventBody>(ServerEventName.FieldBossRewardLooted, value); }
    }

    #endregion Event FieldBoss

    #region Event FearAltar

    // 공포의제단입장에대한대륙퇴장
    public event Delegate<SEBContinentExitForFearAltarEnterEventBody> EventEvtContinentExitForFearAltarEnter
    {
        add { AddDelegate<SEBContinentExitForFearAltarEnterEventBody>(ServerEventName.ContinentExitForFearAltarEnter, value); }
        remove { DelDelegate<SEBContinentExitForFearAltarEnterEventBody>(ServerEventName.ContinentExitForFearAltarEnter, value); }
    }

    // 공포의제단매칭방파티입장
    public event Delegate<SEBFearAltarMatchingRoomPartyEnterEventBody> EventEvtFearAltarMatchingRoomPartyEnter
    {
        add { AddDelegate<SEBFearAltarMatchingRoomPartyEnterEventBody>(ServerEventName.FearAltarMatchingRoomPartyEnter, value); }
        remove { DelDelegate<SEBFearAltarMatchingRoomPartyEnterEventBody>(ServerEventName.FearAltarMatchingRoomPartyEnter, value); }
    }

    // 공포의제단매칭방강퇴
    public event Delegate<SEBFearAltarMatchingRoomBanishedEventBody> EventEvtFearAltarMatchingRoomBanished
    {
        add { AddDelegate<SEBFearAltarMatchingRoomBanishedEventBody>(ServerEventName.FearAltarMatchingRoomBanished, value); }
        remove { DelDelegate<SEBFearAltarMatchingRoomBanishedEventBody>(ServerEventName.FearAltarMatchingRoomBanished, value); }
    }

    // 공포의제단매칭상태변경
    public event Delegate<SEBFearAltarMatchingStatusChangedEventBody> EventEvtFearAltarMatchingStatusChanged
    {
        add { AddDelegate<SEBFearAltarMatchingStatusChangedEventBody>(ServerEventName.FearAltarMatchingStatusChanged, value); }
        remove { DelDelegate<SEBFearAltarMatchingStatusChangedEventBody>(ServerEventName.FearAltarMatchingStatusChanged, value); }
    }

    // 공포의제단웨이브시작
    public event Delegate<SEBFearAltarWaveStartEventBody> EventEvtFearAltarWaveStart
    {
        add { AddDelegate<SEBFearAltarWaveStartEventBody>(ServerEventName.FearAltarWaveStart, value); }
        remove { DelDelegate<SEBFearAltarWaveStartEventBody>(ServerEventName.FearAltarWaveStart, value); }
    }

    // 공포의제단성물획득
    public event Delegate<SEBFearAltarHalidomAcquisitionEventBody> EventEvtFearAltarHalidomAcquisition
    {
        add { AddDelegate<SEBFearAltarHalidomAcquisitionEventBody>(ServerEventName.FearAltarHalidomAcquisition, value); }
        remove { DelDelegate<SEBFearAltarHalidomAcquisitionEventBody>(ServerEventName.FearAltarHalidomAcquisition, value); }
    }

    // 공포의제단클리어
    public event Delegate<SEBFearAltarClearEventBody> EventEvtFearAltarClear
    {
        add { AddDelegate<SEBFearAltarClearEventBody>(ServerEventName.FearAltarClear, value); }
        remove { DelDelegate<SEBFearAltarClearEventBody>(ServerEventName.FearAltarClear, value); }
    }

    // 공포의제단실패
    public event Delegate<SEBFearAltarFailEventBody> EventEvtFearAltarFail
    {
        add { AddDelegate<SEBFearAltarFailEventBody>(ServerEventName.FearAltarFail, value); }
        remove { DelDelegate<SEBFearAltarFailEventBody>(ServerEventName.FearAltarFail, value); }
    }

    // 공포의제단강퇴
    public event Delegate<SEBFearAltarBanishedEventBody> EventEvtFearAltarBanished
    {
        add { AddDelegate<SEBFearAltarBanishedEventBody>(ServerEventName.FearAltarBanished, value); }
        remove { DelDelegate<SEBFearAltarBanishedEventBody>(ServerEventName.FearAltarBanished, value); }
    }

    #endregion Event FearAltar

    #region Event SubQuest

    // 서브퀘스트수락
    public event Delegate<SEBSubQuestsAcceptedEventBody> EventEvtSubQuestsAccepted
    {
        add { AddDelegate<SEBSubQuestsAcceptedEventBody>(ServerEventName.SubQuestsAccepted, value); }
        remove { DelDelegate<SEBSubQuestsAcceptedEventBody>(ServerEventName.SubQuestsAccepted, value); }
    }

    // 서브퀘스트진행카운트갱신
    public event Delegate<SEBSubQuestProgressCountsUpdatedEventBody> EventEvtSubQuestProgressCountsUpdated
    {
        add { AddDelegate<SEBSubQuestProgressCountsUpdatedEventBody>(ServerEventName.SubQuestProgressCountsUpdated, value); }
        remove { DelDelegate<SEBSubQuestProgressCountsUpdatedEventBody>(ServerEventName.SubQuestProgressCountsUpdated, value); }
    }

    #endregion Event SubQuest

    #region Event WarMemory

    // 전쟁의기억입장에대한대륙퇴장
    public event Delegate<SEBContinentExitForWarMemoryEnterEventBody> EventEvtContinentExitForWarMemoryEnter
    {
        add { AddDelegate<SEBContinentExitForWarMemoryEnterEventBody>(ServerEventName.ContinentExitForWarMemoryEnter, value); }
        remove { DelDelegate<SEBContinentExitForWarMemoryEnterEventBody>(ServerEventName.ContinentExitForWarMemoryEnter, value); }
    }

    // 전쟁의기억매칭방파티입장
    public event Delegate<SEBWarMemoryMatchingRoomPartyEnterEventBody> EventEvtWarMemoryMatchingRoomPartyEnter
    {
        add { AddDelegate<SEBWarMemoryMatchingRoomPartyEnterEventBody>(ServerEventName.WarMemoryMatchingRoomPartyEnter, value); }
        remove { DelDelegate<SEBWarMemoryMatchingRoomPartyEnterEventBody>(ServerEventName.WarMemoryMatchingRoomPartyEnter, value); }
    }

    // 전쟁의기억매칭방강퇴
    public event Delegate<SEBWarMemoryMatchingRoomBanishedEventBody> EventEvtWarMemoryMatchingRoomBanished
    {
        add { AddDelegate<SEBWarMemoryMatchingRoomBanishedEventBody>(ServerEventName.WarMemoryMatchingRoomBanished, value); }
        remove { DelDelegate<SEBWarMemoryMatchingRoomBanishedEventBody>(ServerEventName.WarMemoryMatchingRoomBanished, value); }
    }

    // 전쟁의기억매칭상태변경
    public event Delegate<SEBWarMemoryMatchingStatusChangedEventBody> EventEvtWarMemoryMatchingStatusChanged
    {
        add { AddDelegate<SEBWarMemoryMatchingStatusChangedEventBody>(ServerEventName.WarMemoryMatchingStatusChanged, value); }
        remove { DelDelegate<SEBWarMemoryMatchingStatusChangedEventBody>(ServerEventName.WarMemoryMatchingStatusChanged, value); }
    }

    // 전쟁의기억웨이브시작
    public event Delegate<SEBWarMemoryWaveStartEventBody> EventEvtWarMemoryWaveStart
    {
        add { AddDelegate<SEBWarMemoryWaveStartEventBody>(ServerEventName.WarMemoryWaveStart, value); }
        remove { DelDelegate<SEBWarMemoryWaveStartEventBody>(ServerEventName.WarMemoryWaveStart, value); }
    }

    // 전쟁의기억웨이브완료
    public event Delegate<SEBWarMemoryWaveCompletedEventBody> EventEvtWarMemoryWaveCompleted
    {
        add { AddDelegate<SEBWarMemoryWaveCompletedEventBody>(ServerEventName.WarMemoryWaveCompleted, value); }
        remove { DelDelegate<SEBWarMemoryWaveCompletedEventBody>(ServerEventName.WarMemoryWaveCompleted, value); }
    }

    // 전쟁의기억변신오브젝트수명종료
    public event Delegate<SEBWarMemoryTransformationObjectLifetimeEndedEventBody> EventEvtWarMemoryTransformationObjectLifetimeEnded
    {
        add { AddDelegate<SEBWarMemoryTransformationObjectLifetimeEndedEventBody>(ServerEventName.WarMemoryTransformationObjectLifetimeEnded, value); }
        remove { DelDelegate<SEBWarMemoryTransformationObjectLifetimeEndedEventBody>(ServerEventName.WarMemoryTransformationObjectLifetimeEnded, value); }
    }

    // 전쟁의기억변신오브젝트상호작용취소
    public event Delegate<SEBWarMemoryTransformationObjectInteractionCancelEventBody> EventEvtWarMemoryTransformationObjectInteractionCancel
    {
        add { AddDelegate<SEBWarMemoryTransformationObjectInteractionCancelEventBody>(ServerEventName.WarMemoryTransformationObjectInteractionCancel, value); }
        remove { DelDelegate<SEBWarMemoryTransformationObjectInteractionCancelEventBody>(ServerEventName.WarMemoryTransformationObjectInteractionCancel, value); }
    }

    // 전쟁의기억변신오브젝트상호작용종료
    public event Delegate<SEBWarMemoryTransformationObjectInteractionFinishedEventBody> EventEvtWarMemoryTransformationObjectInteractionFinished
    {
        add { AddDelegate<SEBWarMemoryTransformationObjectInteractionFinishedEventBody>(ServerEventName.WarMemoryTransformationObjectInteractionFinished, value); }
        remove { DelDelegate<SEBWarMemoryTransformationObjectInteractionFinishedEventBody>(ServerEventName.WarMemoryTransformationObjectInteractionFinished, value); }
    }

    // 영웅전쟁의기억변신오브젝트상호작용시작
    public event Delegate<SEBHeroWarMemoryTransformationObjectInteractionStartEventBody> EventEvtHeroWarMemoryTransformationObjectInteractionStart
    {
        add { AddDelegate<SEBHeroWarMemoryTransformationObjectInteractionStartEventBody>(ServerEventName.HeroWarMemoryTransformationObjectInteractionStart, value); }
        remove { DelDelegate<SEBHeroWarMemoryTransformationObjectInteractionStartEventBody>(ServerEventName.HeroWarMemoryTransformationObjectInteractionStart, value); }
    }

    // 영웅전쟁의기억변신오브젝트상호작용취소
    public event Delegate<SEBHeroWarMemoryTransformationObjectInteractionCancelEventBody> EventEvtHeroWarMemoryTransformationObjectInteractionCancel
    {
        add { AddDelegate<SEBHeroWarMemoryTransformationObjectInteractionCancelEventBody>(ServerEventName.HeroWarMemoryTransformationObjectInteractionCancel, value); }
        remove { DelDelegate<SEBHeroWarMemoryTransformationObjectInteractionCancelEventBody>(ServerEventName.HeroWarMemoryTransformationObjectInteractionCancel, value); }
    }

    // 영웅전쟁의기억변신오브젝트상호작용종료
    public event Delegate<SEBHeroWarMemoryTransformationObjectInteractionFinishedEventBody> EventEvtHeroWarMemoryTransformationObjectInteractionFinished
    {
        add { AddDelegate<SEBHeroWarMemoryTransformationObjectInteractionFinishedEventBody>(ServerEventName.HeroWarMemoryTransformationObjectInteractionFinished, value); }
        remove { DelDelegate<SEBHeroWarMemoryTransformationObjectInteractionFinishedEventBody>(ServerEventName.HeroWarMemoryTransformationObjectInteractionFinished, value); }
    }

    // 전쟁의기억몬스터변신취소
    public event Delegate<SEBWarMemoryMonsterTransformationCancelEventBody> EventEvtWarMemoryMonsterTransformationCancel
    {
        add { AddDelegate<SEBWarMemoryMonsterTransformationCancelEventBody>(ServerEventName.WarMemoryMonsterTransformationCancel, value); }
        remove { DelDelegate<SEBWarMemoryMonsterTransformationCancelEventBody>(ServerEventName.WarMemoryMonsterTransformationCancel, value); }
    }

    // 전쟁의기억몬스터변신종료
    public event Delegate<SEBWarMemoryMonsterTransformationFinishedEventBody> EventEvtWarMemoryMonsterTransformationFinished
    {
        add { AddDelegate<SEBWarMemoryMonsterTransformationFinishedEventBody>(ServerEventName.WarMemoryMonsterTransformationFinished, value); }
        remove { DelDelegate<SEBWarMemoryMonsterTransformationFinishedEventBody>(ServerEventName.WarMemoryMonsterTransformationFinished, value); }
    }

    // 영웅전쟁의기억몬스터변신취소
    public event Delegate<SEBHeroWarMemoryMonsterTransformationCancelEventBody> EventEvtHeroWarMemoryMonsterTransformationCancel
    {
        add { AddDelegate<SEBHeroWarMemoryMonsterTransformationCancelEventBody>(ServerEventName.HeroWarMemoryMonsterTransformationCancel, value); }
        remove { DelDelegate<SEBHeroWarMemoryMonsterTransformationCancelEventBody>(ServerEventName.HeroWarMemoryMonsterTransformationCancel, value); }
    }

    // 영웅전쟁의기억몬스터변신종료
    public event Delegate<SEBHeroWarMemoryMonsterTransformationFinishedEventBody> EventEvtHeroWarMemoryMonsterTransformationFinished
    {
        add { AddDelegate<SEBHeroWarMemoryMonsterTransformationFinishedEventBody>(ServerEventName.HeroWarMemoryMonsterTransformationFinished, value); }
        remove { DelDelegate<SEBHeroWarMemoryMonsterTransformationFinishedEventBody>(ServerEventName.HeroWarMemoryMonsterTransformationFinished, value); }
    }

    // 전쟁의기억몬스터소환
    public event Delegate<SEBWarMemoryMonsterSummonEventBody> EventEvtWarMemoryMonsterSummon
    {
        add { AddDelegate<SEBWarMemoryMonsterSummonEventBody>(ServerEventName.WarMemoryMonsterSummon, value); }
        remove { DelDelegate<SEBWarMemoryMonsterSummonEventBody>(ServerEventName.WarMemoryMonsterSummon, value); }
    }

    // 전쟁의기억점수획득
    public event Delegate<SEBWarMemoryPointAcquisitionEventBody> EventEvtWarMemoryPointAcquisition
    {
        add { AddDelegate<SEBWarMemoryPointAcquisitionEventBody>(ServerEventName.WarMemoryPointAcquisition, value); }
        remove { DelDelegate<SEBWarMemoryPointAcquisitionEventBody>(ServerEventName.WarMemoryPointAcquisition, value); }
    }

    // 영웅전쟁의기억점수획득
    public event Delegate<SEBHeroWarMemoryPointAcquisitionEventBody> EventEvtHeroWarMemoryPointAcquisition
    {
        add { AddDelegate<SEBHeroWarMemoryPointAcquisitionEventBody>(ServerEventName.HeroWarMemoryPointAcquisition, value); }
        remove { DelDelegate<SEBHeroWarMemoryPointAcquisitionEventBody>(ServerEventName.HeroWarMemoryPointAcquisition, value); }
    }

    // 영웅전쟁의기억변신몬스터스킬시전
    public event Delegate<SEBHeroWarMemoryTransformationMonsterSkillCastEventBody> EventEvtHeroWarMemoryTransformationMonsterSkillCast
    {
        add { AddDelegate<SEBHeroWarMemoryTransformationMonsterSkillCastEventBody>(ServerEventName.HeroWarMemoryTransformationMonsterSkillCast, value); }
        remove { DelDelegate<SEBHeroWarMemoryTransformationMonsterSkillCastEventBody>(ServerEventName.HeroWarMemoryTransformationMonsterSkillCast, value); }
    }

    // 전쟁의기억클리어
    public event Delegate<SEBWarMemoryClearEventBody> EventEvtWarMemoryClear
    {
        add { AddDelegate<SEBWarMemoryClearEventBody>(ServerEventName.WarMemoryClear, value); }
        remove { DelDelegate<SEBWarMemoryClearEventBody>(ServerEventName.WarMemoryClear, value); }
    }

    // 전쟁의기억실패
    public event Delegate<SEBWarMemoryFailEventBody> EventEvtWarMemoryFail
    {
        add { AddDelegate<SEBWarMemoryFailEventBody>(ServerEventName.WarMemoryFail, value); }
        remove { DelDelegate<SEBWarMemoryFailEventBody>(ServerEventName.WarMemoryFail, value); }
    }

    // 전쟁의기억강퇴
    public event Delegate<SEBWarMemoryBanishedEventBody> EventEvtWarMemoryBanished
    {
        add { AddDelegate<SEBWarMemoryBanishedEventBody>(ServerEventName.WarMemoryBanished, value); }
        remove { DelDelegate<SEBWarMemoryBanishedEventBody>(ServerEventName.WarMemoryBanished, value); }
    }

    #endregion Event WarMemory

    #region Event OrdealQuest

    // 시련퀘스트수락
    public event Delegate<SEBOrdealQuestAcceptedEventBody> EventEvtOrdealQuestAccepted
    {
        add { AddDelegate<SEBOrdealQuestAcceptedEventBody>(ServerEventName.OrdealQuestAccepted, value); }
        remove { DelDelegate<SEBOrdealQuestAcceptedEventBody>(ServerEventName.OrdealQuestAccepted, value); }
    }

    // 시련퀘스트진행카운트갱신
    public event Delegate<SEBOrdealQuestSlotProgressCountsUpdatedEventBody> EventEvtOrdealQuestSlotProgressCountsUpdated
    {
        add { AddDelegate<SEBOrdealQuestSlotProgressCountsUpdatedEventBody>(ServerEventName.OrdealQuestSlotProgressCountsUpdated, value); }
        remove { DelDelegate<SEBOrdealQuestSlotProgressCountsUpdatedEventBody>(ServerEventName.OrdealQuestSlotProgressCountsUpdated, value); }
    }

    #endregion Event OrdealQuest

    #region Event OsirisRoom

    // 오시리스의방웨이브시작
    public event Delegate<SEBOsirisRoomWaveStartEventBody> EventEvtOsirisRoomWaveStart
    {
        add { AddDelegate<SEBOsirisRoomWaveStartEventBody>(ServerEventName.OsirisRoomWaveStart, value); }
        remove { DelDelegate<SEBOsirisRoomWaveStartEventBody>(ServerEventName.OsirisRoomWaveStart, value); }
    }

    // 오시리스의방몬스터스폰
    public event Delegate<SEBOsirisRoomMonsterSpawnEventBody> EventEvtOsirisRoomMonsterSpawn
    {
        add { AddDelegate<SEBOsirisRoomMonsterSpawnEventBody>(ServerEventName.OsirisRoomMonsterSpawn, value); }
        remove { DelDelegate<SEBOsirisRoomMonsterSpawnEventBody>(ServerEventName.OsirisRoomMonsterSpawn, value); }
    }

    // 오시리스의방보상골드획득
    public event Delegate<SEBOsirisRoomRewardGoldAcquisitionEventBody> EventEvtOsirisRoomRewardGoldAcquisition
    {
        add { AddDelegate<SEBOsirisRoomRewardGoldAcquisitionEventBody>(ServerEventName.OsirisRoomRewardGoldAcquisition, value); }
        remove { DelDelegate<SEBOsirisRoomRewardGoldAcquisitionEventBody>(ServerEventName.OsirisRoomRewardGoldAcquisition, value); }
    }

    // 오시리스의방재화버프종료
    public event Delegate<SEBOsirisRoomMoneyBuffFinishedEventBody> EventEvtOsirisRoomMoneyBuffFinished
    {
        add { AddDelegate<SEBOsirisRoomMoneyBuffFinishedEventBody>(ServerEventName.OsirisRoomMoneyBuffFinished, value); }
        remove { DelDelegate<SEBOsirisRoomMoneyBuffFinishedEventBody>(ServerEventName.OsirisRoomMoneyBuffFinished, value); }
    }

    // 오시리스의방재화버프취소
    public event Delegate<SEBOsirisRoomMoneyBuffCancelEventBody> EventEvtOsirisRoomMoneyBuffCancel
    {
        add { AddDelegate<SEBOsirisRoomMoneyBuffCancelEventBody>(ServerEventName.OsirisRoomMoneyBuffCancel, value); }
        remove { DelDelegate<SEBOsirisRoomMoneyBuffCancelEventBody>(ServerEventName.OsirisRoomMoneyBuffCancel, value); }
    }

    // 오시리스의방클리어
    public event Delegate<SEBOsirisRoomClearEventBody> EventEvtOsirisRoomClear
    {
        add { AddDelegate<SEBOsirisRoomClearEventBody>(ServerEventName.OsirisRoomClear, value); }
        remove { DelDelegate<SEBOsirisRoomClearEventBody>(ServerEventName.OsirisRoomClear, value); }
    }

    // 오시리스의방실패
    public event Delegate<SEBOsirisRoomFailEventBody> EventEvtOsirisRoomFail
    {
        add { AddDelegate<SEBOsirisRoomFailEventBody>(ServerEventName.OsirisRoomFail, value); }
        remove { DelDelegate<SEBOsirisRoomFailEventBody>(ServerEventName.OsirisRoomFail, value); }
    }

    // 오시리스의방강퇴
    public event Delegate<SEBOsirisRoomBanishedEventBody> EventEvtOsirisRoomBanished
    {
        add { AddDelegate<SEBOsirisRoomBanishedEventBody>(ServerEventName.OsirisRoomBanished, value); }
        remove { DelDelegate<SEBOsirisRoomBanishedEventBody>(ServerEventName.OsirisRoomBanished, value); }
    }

    #endregion Event OsirisRoom

    #region Event BiographyQuest

    // 전기퀘스트진행카운트갱신
    public event Delegate<SEBBiographyQuestProgressCountsUpdatedEventBody> EventEvtBiographyQuestProgressCountsUpdated
    {
        add { AddDelegate<SEBBiographyQuestProgressCountsUpdatedEventBody>(ServerEventName.BiographyQuestProgressCountsUpdated, value); }
        remove { DelDelegate<SEBBiographyQuestProgressCountsUpdatedEventBody>(ServerEventName.BiographyQuestProgressCountsUpdated, value); }
    }

    #endregion Event BiographyQuest

    #region Event BiographyQuestDungeon

    // 전기퀘스트던전웨이브시작
    public event Delegate<SEBBiographyQuestDungeonWaveStartEventBody> EventEvtBiographyQuestDungeonWaveStart
    {
        add { AddDelegate<SEBBiographyQuestDungeonWaveStartEventBody>(ServerEventName.BiographyQuestDungeonWaveStart, value); }
        remove { DelDelegate<SEBBiographyQuestDungeonWaveStartEventBody>(ServerEventName.BiographyQuestDungeonWaveStart, value); }
    }

    // 전기퀘스트던전웨이브완료
    public event Delegate<SEBBiographyQuestDungeonWaveCompletedEventBody> EventEvtBiographyQuestDungeonWaveCompleted
    {
        add { AddDelegate<SEBBiographyQuestDungeonWaveCompletedEventBody>(ServerEventName.BiographyQuestDungeonWaveCompleted, value); }
        remove { DelDelegate<SEBBiographyQuestDungeonWaveCompletedEventBody>(ServerEventName.BiographyQuestDungeonWaveCompleted, value); }
    }

    // 전기퀘스트던전실패
    public event Delegate<SEBBiographyQuestDungeonFailEventBody> EventEvtBiographyQuestDungeonFail
    {
        add { AddDelegate<SEBBiographyQuestDungeonFailEventBody>(ServerEventName.BiographyQuestDungeonFail, value); }
        remove { DelDelegate<SEBBiographyQuestDungeonFailEventBody>(ServerEventName.BiographyQuestDungeonFail, value); }
    }

    // 전기퀘스트던전클리어
    public event Delegate<SEBBiographyQuestDungeonClearEventBody> EventEvtBiographyQuestDungeonClear
    {
        add { AddDelegate<SEBBiographyQuestDungeonClearEventBody>(ServerEventName.BiographyQuestDungeonClear, value); }
        remove { DelDelegate<SEBBiographyQuestDungeonClearEventBody>(ServerEventName.BiographyQuestDungeonClear, value); }
    }

    // 전기퀘스트던전강퇴
    public event Delegate<SEBBiographyQuestDungeonBanishedEventBody> EventEvtBiographyQuestDungeonBanished
    {
        add { AddDelegate<SEBBiographyQuestDungeonBanishedEventBody>(ServerEventName.BiographyQuestDungeonBanished, value); }
        remove { DelDelegate<SEBBiographyQuestDungeonBanishedEventBody>(ServerEventName.BiographyQuestDungeonBanished, value); }
    }

    #endregion Event BiographyQuestDungeon

    #region Event Friend

    // 친구신청받음
    public event Delegate<SEBFriendApplicationReceivedEventBody> EventEvtFriendApplicationReceived
    {
        add { AddDelegate<SEBFriendApplicationReceivedEventBody>(ServerEventName.FriendApplicationReceived, value); }
        remove { DelDelegate<SEBFriendApplicationReceivedEventBody>(ServerEventName.FriendApplicationReceived, value); }
    }

    // 친구신청취소
    public event Delegate<SEBFriendApplicationCanceledEventBody> EventEvtFriendApplicationCanceled
    {
        add { AddDelegate<SEBFriendApplicationCanceledEventBody>(ServerEventName.FriendApplicationCanceled, value); }
        remove { DelDelegate<SEBFriendApplicationCanceledEventBody>(ServerEventName.FriendApplicationCanceled, value); }
    }

    // 친구신청수락
    public event Delegate<SEBFriendApplicationAcceptedEventBody> EventEvtFriendApplicationAccepted
    {
        add { AddDelegate<SEBFriendApplicationAcceptedEventBody>(ServerEventName.FriendApplicationAccepted, value); }
        remove { DelDelegate<SEBFriendApplicationAcceptedEventBody>(ServerEventName.FriendApplicationAccepted, value); }
    }

    // 친구신청거절
    public event Delegate<SEBFriendApplicationRefusedEventBody> EventEvtFriendApplicationRefused
    {
        add { AddDelegate<SEBFriendApplicationRefusedEventBody>(ServerEventName.FriendApplicationRefused, value); }
        remove { DelDelegate<SEBFriendApplicationRefusedEventBody>(ServerEventName.FriendApplicationRefused, value); }
    }

    #endregion Event Friend

    #region Event TempFriend

    // 임시친구추가
    public event Delegate<SEBTempFriendAddedEventBody> EventEvtTempFriendAdded
    {
        add { AddDelegate<SEBTempFriendAddedEventBody>(ServerEventName.TempFriendAdded, value); }
        remove { DelDelegate<SEBTempFriendAddedEventBody>(ServerEventName.TempFriendAdded, value); }
    }

    #endregion Event TempFriend

    #region Event DeadRecord

    // 사망기록추가
    public event Delegate<SEBDeadRecordAddedEventBody> EventEvtDeadRecordAdded
    {
        add { AddDelegate<SEBDeadRecordAddedEventBody>(ServerEventName.DeadRecordAdded, value); }
        remove { DelDelegate<SEBDeadRecordAddedEventBody>(ServerEventName.DeadRecordAdded, value); }
    }

    #endregion Event DeadRecord

    #region Event BlessingQuest

    // 축복퀘스트시작
    public event Delegate<SEBBlessingQuestStartedEventBody> EventEvtBlessingQuestStarted
    {
        add { AddDelegate<SEBBlessingQuestStartedEventBody>(ServerEventName.BlessingQuestStarted, value); }
        remove { DelDelegate<SEBBlessingQuestStartedEventBody>(ServerEventName.BlessingQuestStarted, value); }
    }

    #endregion Event BlessingQuest

    #region Event Blessing

    // 축복받음
    public event Delegate<SEBBlessingReceivedEventBody> EventEvtBlessingReceived
    {
        add { AddDelegate<SEBBlessingReceivedEventBody>(ServerEventName.BlessingReceived, value); }
        remove { DelDelegate<SEBBlessingReceivedEventBody>(ServerEventName.BlessingReceived, value); }
    }

    // 축복감사메시지수신
    public event Delegate<SEBBlessingThanksMessageReceivedEventBody> EventEvtBlessingThanksMessageReceived
    {
        add { AddDelegate<SEBBlessingThanksMessageReceivedEventBody>(ServerEventName.BlessingThanksMessageReceived, value); }
        remove { DelDelegate<SEBBlessingThanksMessageReceivedEventBody>(ServerEventName.BlessingThanksMessageReceived, value); }
    }

    #endregion Event Blessing

    #region Event OwnerProspectQuest

    // 소유유망자퀘스트완료
    public event Delegate<SEBOwnerProspectQuestCompletedEventBody> EventEvtOwnerProspectQuestCompleted
    {
        add { AddDelegate<SEBOwnerProspectQuestCompletedEventBody>(ServerEventName.OwnerProspectQuestCompleted, value); }
        remove { DelDelegate<SEBOwnerProspectQuestCompletedEventBody>(ServerEventName.OwnerProspectQuestCompleted, value); }
    }

    // 소유유망자퀘스트실패
    public event Delegate<SEBOwnerProspectQuestFailedEventBody> EventEvtOwnerProspectQuestFailed
    {
        add { AddDelegate<SEBOwnerProspectQuestFailedEventBody>(ServerEventName.OwnerProspectQuestFailed, value); }
        remove { DelDelegate<SEBOwnerProspectQuestFailedEventBody>(ServerEventName.OwnerProspectQuestFailed, value); }
    }

    // 소유유망자퀘스트대상레벨갱신
    public event Delegate<SEBOwnerProspectQuestTargetLevelUpdatedEventBody> EventEvtOwnerProspectQuestTargetLevelUpdated
    {
        add { AddDelegate<SEBOwnerProspectQuestTargetLevelUpdatedEventBody>(ServerEventName.OwnerProspectQuestTargetLevelUpdated, value); }
        remove { DelDelegate<SEBOwnerProspectQuestTargetLevelUpdatedEventBody>(ServerEventName.OwnerProspectQuestTargetLevelUpdated, value); }
    }

    // 대상유망자퀘스트시작
    public event Delegate<SEBTargetProspectQuestStartedEventBody> EventEvtTargetProspectQuestStarted
    {
        add { AddDelegate<SEBTargetProspectQuestStartedEventBody>(ServerEventName.TargetProspectQuestStarted, value); }
        remove { DelDelegate<SEBTargetProspectQuestStartedEventBody>(ServerEventName.TargetProspectQuestStarted, value); }
    }

    // 대상유망자퀘스트완료
    public event Delegate<SEBTargetProspectQuestCompletedEventBody> EventEvtTargetProspectQuestCompleted
    {
        add { AddDelegate<SEBTargetProspectQuestCompletedEventBody>(ServerEventName.TargetProspectQuestCompleted, value); }
        remove { DelDelegate<SEBTargetProspectQuestCompletedEventBody>(ServerEventName.TargetProspectQuestCompleted, value); }
    }

    // 대상유망자퀘스트실패
    public event Delegate<SEBTargetProspectQuestFailedEventBody> EventEvtTargetProspectQuestFailed
    {
        add { AddDelegate<SEBTargetProspectQuestFailedEventBody>(ServerEventName.TargetProspectQuestFailed, value); }
        remove { DelDelegate<SEBTargetProspectQuestFailedEventBody>(ServerEventName.TargetProspectQuestFailed, value); }
    }

    #endregion Event OwnerProspectQuest

    #region Event DragonNest

    // 용의둥지입장에대한대륙퇴장
    public event Delegate<SEBContinentExitForDragonNestEnterEventBody> EventEvtContinentExitForDragonNestEnter
    {
        add { AddDelegate<SEBContinentExitForDragonNestEnterEventBody>(ServerEventName.ContinentExitForDragonNestEnter, value); }
        remove { DelDelegate<SEBContinentExitForDragonNestEnterEventBody>(ServerEventName.ContinentExitForDragonNestEnter, value); }
    }

    // 용의둥지매칭방파티입장
    public event Delegate<SEBDragonNestMatchingRoomPartyEnterEventBody> EventEvtDragonNestMatchingRoomPartyEnter
    {
        add { AddDelegate<SEBDragonNestMatchingRoomPartyEnterEventBody>(ServerEventName.DragonNestMatchingRoomPartyEnter, value); }
        remove { DelDelegate<SEBDragonNestMatchingRoomPartyEnterEventBody>(ServerEventName.DragonNestMatchingRoomPartyEnter, value); }
    }

    // 용의둥지매칭방강퇴
    public event Delegate<SEBDragonNestMatchingRoomBanishedEventBody> EventEvtDragonNestMatchingRoomBanished
    {
        add { AddDelegate<SEBDragonNestMatchingRoomBanishedEventBody>(ServerEventName.DragonNestMatchingRoomBanished, value); }
        remove { DelDelegate<SEBDragonNestMatchingRoomBanishedEventBody>(ServerEventName.DragonNestMatchingRoomBanished, value); }
    }

    // 용의둥지매칭상태변경
    public event Delegate<SEBDragonNestMatchingStatusChangedEventBody> EventEvtDragonNestMatchingStatusChanged
    {
        add { AddDelegate<SEBDragonNestMatchingStatusChangedEventBody>(ServerEventName.DragonNestMatchingStatusChanged, value); }
        remove { DelDelegate<SEBDragonNestMatchingStatusChangedEventBody>(ServerEventName.DragonNestMatchingStatusChanged, value); }
    }

    // 용의둥지단계시작
    public event Delegate<SEBDragonNestStepStartEventBody> EventEvtDragonNestStepStart
    {
        add { AddDelegate<SEBDragonNestStepStartEventBody>(ServerEventName.DragonNestStepStart, value); }
        remove { DelDelegate<SEBDragonNestStepStartEventBody>(ServerEventName.DragonNestStepStart, value); }
    }

    // 용의둥지단계완료
    public event Delegate<SEBDragonNestStepCompletedEventBody> EventEvtDragonNestStepCompleted
    {
        add { AddDelegate<SEBDragonNestStepCompletedEventBody>(ServerEventName.DragonNestStepCompleted, value); }
        remove { DelDelegate<SEBDragonNestStepCompletedEventBody>(ServerEventName.DragonNestStepCompleted, value); }
    }

    // 영웅용의둥지함정적중
    public event Delegate<SEBHeroDragonNestTrapHitEventBody> EventEvtHeroDragonNestTrapHit
    {
        add { AddDelegate<SEBHeroDragonNestTrapHitEventBody>(ServerEventName.HeroDragonNestTrapHit, value); }
        remove { DelDelegate<SEBHeroDragonNestTrapHitEventBody>(ServerEventName.HeroDragonNestTrapHit, value); }
    }

    // 용의둥지함정효과종료
    public event Delegate<SEBDragonNestTrapEffectFinishedEventBody> EventEvtDragonNestTrapEffectFinished
    {
        add { AddDelegate<SEBDragonNestTrapEffectFinishedEventBody>(ServerEventName.DragonNestTrapEffectFinished, value); }
        remove { DelDelegate<SEBDragonNestTrapEffectFinishedEventBody>(ServerEventName.DragonNestTrapEffectFinished, value); }
    }

    // 영웅용의둥지함정효과종료
    public event Delegate<SEBHeroDragonNestTrapEffectFinishedEventBody> EventEvtHeroDragonNestTrapEffectFinished
    {
        add { AddDelegate<SEBHeroDragonNestTrapEffectFinishedEventBody>(ServerEventName.HeroDragonNestTrapEffectFinished, value); }
        remove { DelDelegate<SEBHeroDragonNestTrapEffectFinishedEventBody>(ServerEventName.HeroDragonNestTrapEffectFinished, value); }
    }

    // 용의둥지클리어
    public event Delegate<SEBDragonNestClearEventBody> EventEvtDragonNestClear
    {
        add { AddDelegate<SEBDragonNestClearEventBody>(ServerEventName.DragonNestClear, value); }
        remove { DelDelegate<SEBDragonNestClearEventBody>(ServerEventName.DragonNestClear, value); }
    }

    // 용의둥지실패
    public event Delegate<SEBDragonNestFailEventBody> EventEvtDragonNestFail
    {
        add { AddDelegate<SEBDragonNestFailEventBody>(ServerEventName.DragonNestFail, value); }
        remove { DelDelegate<SEBDragonNestFailEventBody>(ServerEventName.DragonNestFail, value); }
    }

    // 용의둥지강퇴
    public event Delegate<SEBDragonNestBanishedEventBody> EventEvtDragonNestBanished
    {
        add { AddDelegate<SEBDragonNestBanishedEventBody>(ServerEventName.DragonNestBanished, value); }
        remove { DelDelegate<SEBDragonNestBanishedEventBody>(ServerEventName.DragonNestBanished, value); }
    }

    #endregion Event DragonNest

    #region Event Creature

    // 크리처출전
    public event Delegate<SEBHeroCreatureParticipatedEventBody> EventEvtHeroCreatureParticipated
    {
        add { AddDelegate<SEBHeroCreatureParticipatedEventBody>(ServerEventName.HeroCreatureParticipated, value); }
        remove { DelDelegate<SEBHeroCreatureParticipatedEventBody>(ServerEventName.HeroCreatureParticipated, value); }
    }

    // 크리처출전취소
    public event Delegate<SEBHeroCreatureParticipationCanceledEventBody> EventEvtHeroCreatureParticipationCanceled
    {
        add { AddDelegate<SEBHeroCreatureParticipationCanceledEventBody>(ServerEventName.HeroCreatureParticipationCanceled, value); }
        remove { DelDelegate<SEBHeroCreatureParticipationCanceledEventBody>(ServerEventName.HeroCreatureParticipationCanceled, value); }
    }

    #endregion Event Creature

    #region Event Costume

    // 코스튬기간만료
    public event Delegate<SEBCostumePeriodExpiredEventBody> EventEvtCostumePeriodExpired
    {
        add { AddDelegate<SEBCostumePeriodExpiredEventBody>(ServerEventName.CostumePeriodExpired, value); }
        remove { DelDelegate<SEBCostumePeriodExpiredEventBody>(ServerEventName.CostumePeriodExpired, value); }
    }

    // 영웅코스튬장착
    public event Delegate<SEBHeroCostumeEquippedEventBody> EventEvtHeroCostumeEquipped
    {
        add { AddDelegate<SEBHeroCostumeEquippedEventBody>(ServerEventName.HeroCostumeEquipped, value); }
        remove { DelDelegate<SEBHeroCostumeEquippedEventBody>(ServerEventName.HeroCostumeEquipped, value); }
    }

    // 영웅코스튬장착해제
    public event Delegate<SEBHeroCostumeUnequippedEventBody> EventEvtHeroCostumeUnequipped
    {
        add { AddDelegate<SEBHeroCostumeUnequippedEventBody>(ServerEventName.HeroCostumeUnequipped, value); }
        remove { DelDelegate<SEBHeroCostumeUnequippedEventBody>(ServerEventName.HeroCostumeUnequipped, value); }
    }

    // 영웅코스튬효과적용
    public event Delegate<SEBHeroCostumeEffectAppliedEventBody> EventEvtHeroCostumeEffectApplied
    {
        add { AddDelegate<SEBHeroCostumeEffectAppliedEventBody>(ServerEventName.HeroCostumeEffectApplied, value); }
        remove { DelDelegate<SEBHeroCostumeEffectAppliedEventBody>(ServerEventName.HeroCostumeEffectApplied, value); }
    }

    #endregion Event Costume

    #region Event Present

    // 선물수신
    public event Delegate<SEBPresentReceivedEventBody> EventEvtPresentReceived
    {
        add { AddDelegate<SEBPresentReceivedEventBody>(ServerEventName.PresentReceived, value); }
        remove { DelDelegate<SEBPresentReceivedEventBody>(ServerEventName.PresentReceived, value); }
    }

    // 선물답장수신
    public event Delegate<SEBPresentReplyReceivedEventBody> EventEvtPresentReplyReceived
    {
        add { AddDelegate<SEBPresentReplyReceivedEventBody>(ServerEventName.PresentReplyReceived, value); }
        remove { DelDelegate<SEBPresentReplyReceivedEventBody>(ServerEventName.PresentReplyReceived, value); }
    }

    // 영웅선물
    public event Delegate<SEBHeroPresentEventBody> EventEvtHeroPresent
    {
        add { AddDelegate<SEBHeroPresentEventBody>(ServerEventName.HeroPresent, value); }
        remove { DelDelegate<SEBHeroPresentEventBody>(ServerEventName.HeroPresent, value); }
    }

    #endregion Event Present

    #region Event Present Ranking

    // 국가주간선물인기점수랭킹갱신
    public event Delegate<SEBNationWeeklyPresentPopularityPointRankingUpdatedEventBody> EventEvtNationWeeklyPresentPopularityPointRankingUpdated
    {
        add { AddDelegate<SEBNationWeeklyPresentPopularityPointRankingUpdatedEventBody>(ServerEventName.NationWeeklyPresentPopularityPointRankingUpdated, value); }
        remove { DelDelegate<SEBNationWeeklyPresentPopularityPointRankingUpdatedEventBody>(ServerEventName.NationWeeklyPresentPopularityPointRankingUpdated, value); }
    }

    // 국가주간선물공헌점수랭킹갱신
    public event Delegate<SEBNationWeeklyPresentContributionPointRankingUpdatedEventBody> EventEvtNationWeeklyPresentContributionPointRankingUpdated
    {
        add { AddDelegate<SEBNationWeeklyPresentContributionPointRankingUpdatedEventBody>(ServerEventName.NationWeeklyPresentContributionPointRankingUpdated, value); }
        remove { DelDelegate<SEBNationWeeklyPresentContributionPointRankingUpdatedEventBody>(ServerEventName.NationWeeklyPresentContributionPointRankingUpdated, value); }
    }

    #endregion Event Present Ranking

    #region Event CreatureFarmQeust

    // 크리처농장퀘스트미션진행카운트갱신
    public event Delegate<SEBCreatureFarmQuestMissionProgressCountUpdatedEventBody> EventEvtCreatureFarmQuestMissionProgressCountUpdated
    {
        add { AddDelegate<SEBCreatureFarmQuestMissionProgressCountUpdatedEventBody>(ServerEventName.CreatureFarmQuestMissionProgressCountUpdated, value); }
        remove { DelDelegate<SEBCreatureFarmQuestMissionProgressCountUpdatedEventBody>(ServerEventName.CreatureFarmQuestMissionProgressCountUpdated, value); }
    }

    // 크리처농장퀘스트미션완료
    public event Delegate<SEBCreatureFarmQuestMissionCompletedEventBody> EventEvtCreatureFarmQuestMissionCompleted
    {
        add { AddDelegate<SEBCreatureFarmQuestMissionCompletedEventBody>(ServerEventName.CreatureFarmQuestMissionCompleted, value); }
        remove { DelDelegate<SEBCreatureFarmQuestMissionCompletedEventBody>(ServerEventName.CreatureFarmQuestMissionCompleted, value); }
    }

    // 크리처농장퀘스트미션몬스터스폰
    public event Delegate<SEBCreatureFarmQuestMissionMonsterSpawnedEventBody> EventEvtCreatureFarmQuestMissionMonsterSpawned
    {
        add { AddDelegate<SEBCreatureFarmQuestMissionMonsterSpawnedEventBody>(ServerEventName.CreatureFarmQuestMissionMonsterSpawned, value); }
        remove { DelDelegate<SEBCreatureFarmQuestMissionMonsterSpawnedEventBody>(ServerEventName.CreatureFarmQuestMissionMonsterSpawned, value); }
    }

    #endregion Event CreatureFarmQuest

    #region Event SafeMode

    // 안전모드시작
    public event Delegate<SEBSafeModeStartedEventBody> EventEvtSafeModeStarted
    {
        add { AddDelegate<SEBSafeModeStartedEventBody>(ServerEventName.SafeModeStarted, value); }
        remove { DelDelegate<SEBSafeModeStartedEventBody>(ServerEventName.SafeModeStarted, value); }
    }

    // 안전모드종료
    public event Delegate<SEBSafeModeEndedEventBody> EventEvtSafeModeEnded
    {
        add { AddDelegate<SEBSafeModeEndedEventBody>(ServerEventName.SafeModeEnded, value); }
        remove { DelDelegate<SEBSafeModeEndedEventBody>(ServerEventName.SafeModeEnded, value); }
    }

    // 영웅안전모드시작
    public event Delegate<SEBHeroSafeModeStartedEventBody> EventEvtHeroSafeModeStarted
    {
        add { AddDelegate<SEBHeroSafeModeStartedEventBody>(ServerEventName.HeroSafeModeStarted, value); }
        remove { DelDelegate<SEBHeroSafeModeStartedEventBody>(ServerEventName.HeroSafeModeStarted, value); }
    }

    // 영웅안전모드종료
    public event Delegate<SEBHeroSafeModeEndedEventBody> EventEvtHeroSafeModeEnded
    {
        add { AddDelegate<SEBHeroSafeModeEndedEventBody>(ServerEventName.HeroSafeModeEnded, value); }
        remove { DelDelegate<SEBHeroSafeModeEndedEventBody>(ServerEventName.HeroSafeModeEnded, value); }
    }

    #endregion Event SafeMode

    #region Event GuildBlessing

    // 길드축복시작
    public event Delegate<SEBGuildBlessingBuffStartedEventBody> EventEvtGuildBlessingBuffStarted
    {
        add { AddDelegate<SEBGuildBlessingBuffStartedEventBody>(ServerEventName.GuildBlessingBuffStarted, value); }
        remove { DelDelegate<SEBGuildBlessingBuffStartedEventBody>(ServerEventName.GuildBlessingBuffStarted, value); }
    }

    // 길드축복종료
    public event Delegate<SEBGuildBlessingBuffEndedEventBody> EventEvtGuildBlessingBuffEnded
    {
        add { AddDelegate<SEBGuildBlessingBuffEndedEventBody>(ServerEventName.GuildBlessingBuffEnded, value); }
        remove { DelDelegate<SEBGuildBlessingBuffEndedEventBody>(ServerEventName.GuildBlessingBuffEnded, value); }
    }

    #endregion Event GuildBlessing

    #region Event JobChangeQuest

    // 전직퀘스트진행카운트갱신
    public event Delegate<SEBJobChangeQuestProgressCountUpdatedEventBody> EventEvtJobChangeQuestProgressCountUpdated
    {
        add { AddDelegate<SEBJobChangeQuestProgressCountUpdatedEventBody>(ServerEventName.JobChangeQuestProgressCountUpdated, value); }
        remove { DelDelegate<SEBJobChangeQuestProgressCountUpdatedEventBody>(ServerEventName.JobChangeQuestProgressCountUpdated, value); }
    }

    // 전직퀘스트몬스터스폰
    public event Delegate<SEBJobChangeQuestMonsterSpawnedEventBody> EventEvtJobChangeQuestMonsterSpawned
    {
        add { AddDelegate<SEBJobChangeQuestMonsterSpawnedEventBody>(ServerEventName.JobChangeQuestMonsterSpawned, value); }
        remove { DelDelegate<SEBJobChangeQuestMonsterSpawnedEventBody>(ServerEventName.JobChangeQuestMonsterSpawned, value); }
    }

    // 전직퀘스트실패
    public event Delegate<SEBJobChangeQuestFailedEventBody> EventEvtJobChangeQuestFailed
    {
        add { AddDelegate<SEBJobChangeQuestFailedEventBody>(ServerEventName.JobChangeQuestFailed, value); }
        remove { DelDelegate<SEBJobChangeQuestFailedEventBody>(ServerEventName.JobChangeQuestFailed, value); }
    }

    #endregion Event JobChangeQuest

    #region Event HeroJobChange

    // 영웅전직
    public event Delegate<SEBHeroJobChangedEventBody> EventEvtHeroJobChanged
    {
        add { AddDelegate<SEBHeroJobChangedEventBody>(ServerEventName.HeroJobChanged, value); }
        remove { DelDelegate<SEBHeroJobChangedEventBody>(ServerEventName.HeroJobChanged, value); }
    }

    #endregion Event HeroJobChange

    #region Event ChargeEvent

    // 첫충전이벤트목표완료
    public event Delegate<SEBFirstChargeEventObjectiveCompletedEventBody> EventEvtFirstChargeEventObjectiveCompleted
    {
        add { AddDelegate<SEBFirstChargeEventObjectiveCompletedEventBody>(ServerEventName.FirstChargeEventObjectiveCompleted, value); }
        remove { DelDelegate<SEBFirstChargeEventObjectiveCompletedEventBody>(ServerEventName.FirstChargeEventObjectiveCompleted, value); }
    }

    // 재충전이벤트진행
    public event Delegate<SEBRechargeEventProgressEventBody> EventEvtRechargeEventProgress
    {
        add { AddDelegate<SEBRechargeEventProgressEventBody>(ServerEventName.RechargeEventProgress, value); }
        remove { DelDelegate<SEBRechargeEventProgressEventBody>(ServerEventName.RechargeEventProgress, value); }
    }

    // 충전이벤트진행
    public event Delegate<SEBChargeEventProgressEventBody> EventEvtChargeEventProgress
    {
        add { AddDelegate<SEBChargeEventProgressEventBody>(ServerEventName.ChargeEventProgress, value); }
        remove { DelDelegate<SEBChargeEventProgressEventBody>(ServerEventName.ChargeEventProgress, value); }
    }

    // 매일충전이벤트진행
    public event Delegate<SEBDailyChargeEventProgressEventBody> EventEvtDailyChargeEventProgress
    {
        add { AddDelegate<SEBDailyChargeEventProgressEventBody>(ServerEventName.DailyChargeEventProgress, value); }
        remove { DelDelegate<SEBDailyChargeEventProgressEventBody>(ServerEventName.DailyChargeEventProgress, value); }
    }

    #endregion Event ChargeEvent

    #region Event ConsumeEvent

    // 소비이벤트진행
    public event Delegate<SEBConsumeEventProgressEventBody> EventEvtConsumeEventProgress
    {
        add { AddDelegate<SEBConsumeEventProgressEventBody>(ServerEventName.ConsumeEventProgress, value); }
        remove { DelDelegate<SEBConsumeEventProgressEventBody>(ServerEventName.ConsumeEventProgress, value); }
    }

    // 매일소비이벤트진행
    public event Delegate<SEBDailyConsumeEventProgressEventBody> EventEvtDailyConsumeEventProgress
    {
        add { AddDelegate<SEBDailyConsumeEventProgressEventBody>(ServerEventName.DailyConsumeEventProgress, value); }
        remove { DelDelegate<SEBDailyConsumeEventProgressEventBody>(ServerEventName.DailyConsumeEventProgress, value); }
    }

    #endregion Event ConsumeEvent

    #region Event AnkouTomb

    // 안쿠의무덤입장에대한대륙퇴장
    public event Delegate<SEBContinentExitForAnkouTombEnterEventBody> EventEvtContinentExitForAnkouTombEnter
    {
        add { AddDelegate<SEBContinentExitForAnkouTombEnterEventBody>(ServerEventName.ContinentExitForAnkouTombEnter, value); }
        remove { DelDelegate<SEBContinentExitForAnkouTombEnterEventBody>(ServerEventName.ContinentExitForAnkouTombEnter, value); }
    }

    // 안쿠의무덤매칭방파티입장
    public event Delegate<SEBAnkouTombMatchingRoomPartyEnterEventBody> EventEvtAnkouTombMatchingRoomPartyEnter
    {
        add { AddDelegate<SEBAnkouTombMatchingRoomPartyEnterEventBody>(ServerEventName.AnkouTombMatchingRoomPartyEnter, value); }
        remove { DelDelegate<SEBAnkouTombMatchingRoomPartyEnterEventBody>(ServerEventName.AnkouTombMatchingRoomPartyEnter, value); }
    }

    // 안쿠의무덤매칭방강퇴
    public event Delegate<SEBAnkouTombMatchingRoomBanishedEventBody> EventEvtAnkouTombMatchingRoomBanished
    {
        add { AddDelegate<SEBAnkouTombMatchingRoomBanishedEventBody>(ServerEventName.AnkouTombMatchingRoomBanished, value); }
        remove { DelDelegate<SEBAnkouTombMatchingRoomBanishedEventBody>(ServerEventName.AnkouTombMatchingRoomBanished, value); }
    }

    // 안쿠의무덤매칭상태변경
    public event Delegate<SEBAnkouTombMatchingStatusChangedEventBody> EventEvtAnkouTombMatchingStatusChanged
    {
        add { AddDelegate<SEBAnkouTombMatchingStatusChangedEventBody>(ServerEventName.AnkouTombMatchingStatusChanged, value); }
        remove { DelDelegate<SEBAnkouTombMatchingStatusChangedEventBody>(ServerEventName.AnkouTombMatchingStatusChanged, value); }
    }

    // 안쿠의무덤웨이브시작
    public event Delegate<SEBAnkouTombWaveStartEventBody> EventEvtAnkouTombWaveStart
    {
        add { AddDelegate<SEBAnkouTombWaveStartEventBody>(ServerEventName.AnkouTombWaveStart, value); }
        remove { DelDelegate<SEBAnkouTombWaveStartEventBody>(ServerEventName.AnkouTombWaveStart, value); }
    }

    // 안쿠의무덤점수획득
    public event Delegate<SEBAnkouTombPointAcquisitionEventBody> EventEvtAnkouTombPointAcquisition
    {
        add { AddDelegate<SEBAnkouTombPointAcquisitionEventBody>(ServerEventName.AnkouTombPointAcquisition, value); }
        remove { DelDelegate<SEBAnkouTombPointAcquisitionEventBody>(ServerEventName.AnkouTombPointAcquisition, value); }
    }

    // 안쿠의무덤재화버프종료
    public event Delegate<SEBAnkouTombMoneyBuffFinishedEventBody> EventEvtAnkouTombMoneyBuffFinished
    {
        add { AddDelegate<SEBAnkouTombMoneyBuffFinishedEventBody>(ServerEventName.AnkouTombMoneyBuffFinished, value); }
        remove { DelDelegate<SEBAnkouTombMoneyBuffFinishedEventBody>(ServerEventName.AnkouTombMoneyBuffFinished, value); }
    }

    // 안쿠의무덤재화버프취소
    public event Delegate<SEBAnkouTombMoneyBuffCancelEventBody> EventEvtAnkouTombMoneyBuffCancel
    {
        add { AddDelegate<SEBAnkouTombMoneyBuffCancelEventBody>(ServerEventName.AnkouTombMoneyBuffCancel, value); }
        remove { DelDelegate<SEBAnkouTombMoneyBuffCancelEventBody>(ServerEventName.AnkouTombMoneyBuffCancel, value); }
    }

    // 안쿠의무덤클리어
    public event Delegate<SEBAnkouTombClearEventBody> EventEvtAnkouTombClear
    {
        add { AddDelegate<SEBAnkouTombClearEventBody>(ServerEventName.AnkouTombClear, value); }
        remove { DelDelegate<SEBAnkouTombClearEventBody>(ServerEventName.AnkouTombClear, value); }
    }

    // 안쿠의무덤실패
    public event Delegate<SEBAnkouTombFailEventBody> EventEvtAnkouTombFail
    {
        add { AddDelegate<SEBAnkouTombFailEventBody>(ServerEventName.AnkouTombFail, value); }
        remove { DelDelegate<SEBAnkouTombFailEventBody>(ServerEventName.AnkouTombFail, value); }
    }

    // 안쿠의무덤강퇴
    public event Delegate<SEBAnkouTombBanishedEventBody> EventEvtAnkouTombBanished
    {
        add { AddDelegate<SEBAnkouTombBanishedEventBody>(ServerEventName.AnkouTombBanished, value); }
        remove { DelDelegate<SEBAnkouTombBanishedEventBody>(ServerEventName.AnkouTombBanished, value); }
    }

    // 안쿠의무덤서버최고기록갱신
    public event Delegate<SEBAnkouTombServerBestRecordUpdatedEventBody> EventEvtAnkouTombServerBestRecordUpdated
    {
        add { AddDelegate<SEBAnkouTombServerBestRecordUpdatedEventBody>(ServerEventName.AnkouTombServerBestRecordUpdated, value); }
        remove { DelDelegate<SEBAnkouTombServerBestRecordUpdatedEventBody>(ServerEventName.AnkouTombServerBestRecordUpdated, value); }
    }

    #endregion Event AnkouTomb

    #region Event Constellation

    // 별자리개방
    public event Delegate<SEBConstellationOpenedEventBody> EventEvtConstellationOpened
    {
        add { AddDelegate<SEBConstellationOpenedEventBody>(ServerEventName.ConstellationOpened, value); }
        remove { DelDelegate<SEBConstellationOpenedEventBody>(ServerEventName.ConstellationOpened, value); }
    }

    #endregion Event Constellation

    #region Event Artifact

    // 아티팩트개방
    public event Delegate<SEBArtifactOpenedEventBody> EventEvtArtifactOpened
    {
        add { AddDelegate<SEBArtifactOpenedEventBody>(ServerEventName.ArtifactOpened, value); }
        remove { DelDelegate<SEBArtifactOpenedEventBody>(ServerEventName.ArtifactOpened, value); }
    }

    // 영웅장작아티팩트변경
    public event Delegate<SEBHeroEquippedArtifactChangedEventBody> EventEvtHeroEquippedArtifactChanged
    {
        add { AddDelegate<SEBHeroEquippedArtifactChangedEventBody>(ServerEventName.HeroEquippedArtifactChanged, value); }
        remove { DelDelegate<SEBHeroEquippedArtifactChangedEventBody>(ServerEventName.HeroEquippedArtifactChanged, value); }
    }

    #endregion Event Artifact

    #region Event TradeShip

    // 무역선탈환입장에대한대륙퇴장
    public event Delegate<SEBContinentExitForTradeShipEnterEventBody> EventEvtContinentExitForTradeShipEnter
    {
        add { AddDelegate<SEBContinentExitForTradeShipEnterEventBody>(ServerEventName.ContinentExitForTradeShipEnter, value); }
        remove { DelDelegate<SEBContinentExitForTradeShipEnterEventBody>(ServerEventName.ContinentExitForTradeShipEnter, value); }
    }

    // 무역선탈환매칭방파티입장
    public event Delegate<SEBTradeShipMatchingRoomPartyEnterEventBody> EventEvtTradeShipMatchingRoomPartyEnter
    {
        add { AddDelegate<SEBTradeShipMatchingRoomPartyEnterEventBody>(ServerEventName.TradeShipMatchingRoomPartyEnter, value); }
        remove { DelDelegate<SEBTradeShipMatchingRoomPartyEnterEventBody>(ServerEventName.TradeShipMatchingRoomPartyEnter, value); }
    }

    // 무역선탈환매칭방강퇴
    public event Delegate<SEBTradeShipMatchingRoomBanishedEventBody> EventEvtTradeShipMatchingRoomBanished
    {
        add { AddDelegate<SEBTradeShipMatchingRoomBanishedEventBody>(ServerEventName.TradeShipMatchingRoomBanished, value); }
        remove { DelDelegate<SEBTradeShipMatchingRoomBanishedEventBody>(ServerEventName.TradeShipMatchingRoomBanished, value); }
    }

    // 무역선탈환매칭상태변경
    public event Delegate<SEBTradeShipMatchingStatusChangedEventBody> EventEvtTradeShipMatchingStatusChanged
    {
        add { AddDelegate<SEBTradeShipMatchingStatusChangedEventBody>(ServerEventName.TradeShipMatchingStatusChanged, value); }
        remove { DelDelegate<SEBTradeShipMatchingStatusChangedEventBody>(ServerEventName.TradeShipMatchingStatusChanged, value); }
    }

    // 무역선탈환단계시작
    public event Delegate<SEBTradeShipStepStartEventBody> EventEvtTradeShipStepStart
    {
        add { AddDelegate<SEBTradeShipStepStartEventBody>(ServerEventName.TradeShipStepStart, value); }
        remove { DelDelegate<SEBTradeShipStepStartEventBody>(ServerEventName.TradeShipStepStart, value); }
    }

    // 무역선탈환점수획득
    public event Delegate<SEBTradeShipPointAcquisitionEventBody> EventEvtTradeShipPointAcquisition
    {
        add { AddDelegate<SEBTradeShipPointAcquisitionEventBody>(ServerEventName.TradeShipPointAcquisition, value); }
        remove { DelDelegate<SEBTradeShipPointAcquisitionEventBody>(ServerEventName.TradeShipPointAcquisition, value); }
    }

    // 무역선탈환오브젝트파괴보상
    public event Delegate<SEBTradeShipObjectDestructionRewardEventBody> EventEvtTradeShipObjectDestructionReward
    {
        add { AddDelegate<SEBTradeShipObjectDestructionRewardEventBody>(ServerEventName.TradeShipObjectDestructionReward, value); }
        remove { DelDelegate<SEBTradeShipObjectDestructionRewardEventBody>(ServerEventName.TradeShipObjectDestructionReward, value); }
    }

    // 무역선탈환재화버프종료
    public event Delegate<SEBTradeShipMoneyBuffFinishedEventBody> EventEvtTradeShipMoneyBuffFinished
    {
        add { AddDelegate<SEBTradeShipMoneyBuffFinishedEventBody>(ServerEventName.TradeShipMoneyBuffFinished, value); }
        remove { DelDelegate<SEBTradeShipMoneyBuffFinishedEventBody>(ServerEventName.TradeShipMoneyBuffFinished, value); }
    }

    // 무역선탈환재화버프취소
    public event Delegate<SEBTradeShipMoneyBuffCancelEventBody> EventEvtTradeShipMoneyBuffCancel
    {
        add { AddDelegate<SEBTradeShipMoneyBuffCancelEventBody>(ServerEventName.TradeShipMoneyBuffCancel, value); }
        remove { DelDelegate<SEBTradeShipMoneyBuffCancelEventBody>(ServerEventName.TradeShipMoneyBuffCancel, value); }
    }

    // 무역선탈환클리어
    public event Delegate<SEBTradeShipClearEventBody> EventEvtTradeShipClear
    {
        add { AddDelegate<SEBTradeShipClearEventBody>(ServerEventName.TradeShipClear, value); }
        remove { DelDelegate<SEBTradeShipClearEventBody>(ServerEventName.TradeShipClear, value); }
    }

    // 무역선탈환실패
    public event Delegate<SEBTradeShipFailEventBody> EventEvtTradeShipFail
    {
        add { AddDelegate<SEBTradeShipFailEventBody>(ServerEventName.TradeShipFail, value); }
        remove { DelDelegate<SEBTradeShipFailEventBody>(ServerEventName.TradeShipFail, value); }
    }

    // 무역선탈환강퇴
    public event Delegate<SEBTradeShipBanishedEventBody> EventEvtTradeShipBanished
    {
        add { AddDelegate<SEBTradeShipBanishedEventBody>(ServerEventName.TradeShipBanished, value); }
        remove { DelDelegate<SEBTradeShipBanishedEventBody>(ServerEventName.TradeShipBanished, value); }
    }

    // 무역선탈환서버최고기록갱신
    public event Delegate<SEBTradeShipServerBestRecordUpdatedEventBody> EventEvtTradeShipServerBestRecordUpdated
    {
        add { AddDelegate<SEBTradeShipServerBestRecordUpdatedEventBody>(ServerEventName.TradeShipServerBestRecordUpdated, value); }
        remove { DelDelegate<SEBTradeShipServerBestRecordUpdatedEventBody>(ServerEventName.TradeShipServerBestRecordUpdated, value); }
    }

    #endregion Event TradeShip

    #region Event SystemMessage

    // 시스템메시지
    public event Delegate<SEBSystemMessageEventBody> EventEvtSystemMessage
    {
        add { AddDelegate<SEBSystemMessageEventBody>(ServerEventName.SystemMessage, value); }
        remove { DelDelegate<SEBSystemMessageEventBody>(ServerEventName.SystemMessage, value); }
    }

    #endregion Event SystemMessage

    #region Event TeamBattlefield

    // 팀전장플레이대기시작


    // 팀전장시작


    // 팀전장클리어


    #endregion Event TeamBattlefield

    #endregion Server Event

    //----------------------------------------------------------------------------------------------------
    public static CsRplzSession Instance
	{
		get {return CsSingleton<CsRplzSession>.GetInstance();}
	}

	//----------------------------------------------------------------------------------------------------
	public CsRplzSession()
	{

	}

	//----------------------------------------------------------------------------------------------------
	public void Send(ClientCommandName enClientCommandName, Body body, bool bReliable = true, bool bEncrypt = false) // Command.
	{
		//Debug.Log("CsRplzSession.Send enClientCommandName" + enClientCommandName);
		var Param = new Dictionary<byte, object>()
		{
			{(byte)CommandParameterCode.PacketId, (long)0},
			{(byte)CommandParameterCode.CommandName, (short)enClientCommandName},
			{(byte)CommandParameterCode.Body, Body.SerializeRaw(body)}
		};

//		dd.d("CsRplzSession.Send.Command", enClientCommandName);

		Send(new OperationRequest() { OperationCode = 0, Parameters = Param }, bReliable, 0, bEncrypt);
	}

	//----------------------------------------------------------------------------------------------------
	public void Send(ClientEventName enClientEventName, Body body, bool bReliable = true, bool bEncrypt = false) // Event.
	{
		//Debug.Log("CsRplzSession.Send enClientEventName" + enClientEventName);
		var Param = new Dictionary<byte, object>()
		{
			{(byte)EventParameterCode.EventName, (short)enClientEventName},
			{(byte)EventParameterCode.Body, Body.SerializeRaw(body)}
		};

		//if (enClientEventName != ClientEventName.MonsterMove && enClientEventName != ClientEventName.Move)
		//{
		//	dd.d("CsRplzSession.Send.Event", enClientEventName);
		//}
		Send(new OperationRequest() { OperationCode = 1, Parameters = Param }, bReliable, 0, bEncrypt);
	}

	//----------------------------------------------------------------------------------------------------
	protected override void OnConnected()
	{
		if (EventConnected != null)
		{
			EventConnected();
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected override void OnDisconnected(StatusCode enStatusCode)
	{
		Debug.Log(enStatusCode);
		if (EventDisconnected != null)
		{
			EventDisconnected();
		}
	}

//	//----------------------------------------------------------------------------------------------------
//	void CallEventFunc<T>(Delegate<T> delegateFunc, EventData eventData) where T : Body
//	{
//		T t = Body.DeserializeRaw<T>((byte[])eventData.Parameters[(byte)EventParameterCode.Body]);
//
//		if (delegateFunc != null)
//		{
//			delegateFunc(t);
//		}
//	}
//
//	//----------------------------------------------------------------------------------------------------
//	void CallEventFunc<T>(Delegate<int, T> delegateFunc, OperationResponse operationResponse) where T : Body
//	{
//		T t = Body.DeserializeRaw<T>((byte[])operationResponse.Parameters[(byte)CommandParameterCode.Body]);
//		if (delegateFunc != null)
//		{
//			delegateFunc((int)operationResponse.ReturnCode, t);
//		}
//	}

	//----------------------------------------------------------------------------------------------------
	public override void OnOperationResponse(OperationResponse operationResponse)
	{
		Dictionary<byte, object> dicParams = operationResponse.Parameters;

		if (!dicParams.ContainsKey((byte)ClientCommon.CommandParameterCode.CommandName) ||
			!dicParams.ContainsKey((byte)ClientCommon.CommandParameterCode.PacketId) ||
			!dicParams.ContainsKey((byte)ClientCommon.CommandParameterCode.Body))
		{
			Debug.Log("CsRplzSession.OnOperationResponse 1 error");
			return;
		}

		short shCommandName = (short)dicParams[(byte)ClientCommon.CommandParameterCode.CommandName];
		long lPacketId = (long)dicParams[(byte)ClientCommon.CommandParameterCode.PacketId];
		byte[] abyData = (byte[])dicParams[(byte)ClientCommon.CommandParameterCode.Body];

		if (operationResponse.OperationCode != 0)
		{
			Debug.Log("CsRplzSession.OnOperationResponse 2 error " + operationResponse.OperationCode.ToString() + " " + operationResponse.ReturnCode.ToString());
			Debug.Log(operationResponse.DebugMessage);
			CsCommandEventManager.Instance.SendErrorLogging("CsRplzSession.OnOperationResponse1 Error) : " + operationResponse.OperationCode.ToString() + " " + operationResponse.ReturnCode.ToString() + " " + operationResponse.DebugMessage);
			return;
		}

		ClientCommandName enCommand = (ClientCommandName)shCommandName;

		if (m_dicRes.ContainsKey(enCommand))
		{
			m_dicRes[enCommand].Execute(operationResponse);
			if (operationResponse.DebugMessage != null)
			{
				Debug.Log(enCommand + " : " + operationResponse.DebugMessage);
				CsCommandEventManager.Instance.SendErrorLogging("CsRplzSession.OnOperationResponse2 Error) : " + enCommand + " " + operationResponse.ReturnCode.ToString() + " " + operationResponse.DebugMessage);
			}
		}
//		Debug.Log("CsRplzSession.OnOperationResponse2"); I need to translate theat error message.. okk
	}

	//----------------------------------------------------------------------------------------------------
	public override void OnEvent(EventData eventData) // Event 수신.
	{
		Dictionary<byte, object> dicParams = eventData.Parameters;

		if (!dicParams.ContainsKey((byte)ClientCommon.EventParameterCode.EventName) ||
			!dicParams.ContainsKey((byte)ClientCommon.EventParameterCode.Body))
		{
			Debug.Log("CsRplzSession.OnEvent 1 error");
			return;
		}

		short shEventName = (short)dicParams[(byte)ClientCommon.EventParameterCode.EventName];
		byte[] abyData = (byte[])dicParams[(byte)ClientCommon.EventParameterCode.Body];

		if (eventData.Code != 0)
		{
			Debug.Log("CsRplzSession.OnEvent 2 error ");
			return;
		}

		ServerEventName enEvent = (ServerEventName)shEventName;

		//dd.d("CsRplzSession.OnEvent", enEvent);

		//Debug.Log("CsRplzSession.OnEvent1 " + enEvent);

		if (m_dicEvt.ContainsKey(enEvent))
		{
			m_dicEvt[enEvent].Execute(eventData);
		}
		//Debug.Log("CsRplzSession.OnEvent2");
	}

	//----------------------------------------------------------------------------------------------------
	public static Vector3 Translate(PDVector3 pdvtPos)
	{
		return new Vector3(pdvtPos.x,pdvtPos.y,pdvtPos.z);
	}

	//----------------------------------------------------------------------------------------------------
	public static PDVector3 Translate(Vector3 vtPos)
	{
		return new PDVector3(vtPos.x,vtPos.y,vtPos.z);
	}
}
