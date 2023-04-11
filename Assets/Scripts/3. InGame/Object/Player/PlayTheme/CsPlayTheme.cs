
public interface IAutoPlay
{
	void Start();
	void Stop(bool bNotify);
	void Update(bool bMoveState);
	int GetTypeKey();

	void ArrivalMoveToPos();

	EnAutoMode GetType();
	int GetTypeSub();
	bool IsSavable();
	bool IsRequiredLoakAtTarget();
	float GetTargetRange();
	void StateChangedToIdle();
	void EnterPortal(int nPortalId);
}

//---------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------
public class CsPlayTheme : IAutoPlay
{
	protected bool m_bUpdateOnMove = false;
	protected int m_nDifficulty = 1; // Exp 던전 난의도
	protected CsMyPlayer m_csPlayer;
	protected CsMyHeroInfo m_csMyHeroInfo;
	protected CsSimpleTimer m_timer = new CsSimpleTimer();

	//---------------------------------------------------------------------------------------------------
	protected CsMyPlayer Player
	{
		get { return m_csPlayer; }
	}

	//---------------------------------------------------------------------------------------------------
	public virtual void Init(CsMyPlayer csPlayer)
	{
		m_csPlayer = csPlayer;
		m_csMyHeroInfo = CsGameData.Instance.MyHeroInfo;
	}

	//---------------------------------------------------------------------------------------------------
	public virtual void Uninit()
	{
		m_csPlayer = null;
		m_csMyHeroInfo = null;
	}

	//---------------------------------------------------------------------------------------------------
	protected virtual void StartAutoPlay() { m_timer.ResetTimer(); }
	protected virtual void StopAutoPlay(bool bNotify) { }
	protected virtual void UpdateAutoPlay() { }

	//---------------------------------------------------------------------------------------------------
	public static int MakeTypeKey(EnAutoMode enType, int nSubType)
	{
		return ((int)enType) * 1000 + nSubType;
	}

	//---------------------------------------------------------------------------------------------------
	protected bool IsThisAutoPlaying()
	{
		return m_csPlayer.AutoPlay == this;
	}

	#region IAutoPlay
	//---------------------------------------------------------------------------------------------------
	public void Start()
	{
		StartAutoPlay();
	}

	//---------------------------------------------------------------------------------------------------
	public void Stop(bool bNotify)
	{
		StopAutoPlay(bNotify);
	}

	//---------------------------------------------------------------------------------------------------
	public void Update(bool bMoveState)
	{
		if (Player == null || Player.Dead || CsIngameData.Instance.ActiveScene == false) return;

		if (bMoveState)
		{
			if (!m_bUpdateOnMove) return;
		}

		if (m_timer.CheckSetTimer())
		{
			UpdateAutoPlay();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public virtual EnAutoMode GetType() { return EnAutoMode.None; }
	public virtual int GetTypeSub() { return 0; }
	public int GetTypeKey() { return MakeTypeKey(GetType(), GetTypeSub()); }
	public virtual bool IsSavable() { return false; }
	public virtual bool IsRequiredLoakAtTarget() { return false; }
	public virtual float GetTargetRange() { return 20.0f; }

	//---------------------------------------------------------------------------------------------------
	public virtual void ArrivalMoveToPos() { }
	//---------------------------------------------------------------------------------------------------
	public virtual void StateChangedToIdle() {}
	//---------------------------------------------------------------------------------------------------
	public virtual void EnterPortal(int nPortalId) { }
	#endregion IAutoPlay
}

