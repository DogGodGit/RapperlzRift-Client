public class CsSceneArea2Alliance : CsSceneIngameContinent
{
	//---------------------------------------------------------------------------------------------------
	protected override void Awake()
	{
		base.Awake();
		m_csContinent = CsGameData.Instance.ContinentList.Find(a => a.SceneName == CsIngameData.Instance.Scene.name);
	}
}
