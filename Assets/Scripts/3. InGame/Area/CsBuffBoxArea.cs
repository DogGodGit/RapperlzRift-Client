using System.Collections;
using UnityEngine;

public class CsBuffBoxArea : CsBaseArea
{
	long m_lInstanceId;
	int m_nBuffBoxId;

	CapsuleCollider m_capsuleCollider;
	CsProofOfValorBuffBox m_csProofOfValorBuffBox;
	CsInfiniteWarBuffBox m_csInfiniteWarBuffBox;
	CsDungeonManager m_csDungeonManager;

	public long InstanceId { get { return m_lInstanceId; } }
	public int BuffBoxId { get { return m_nBuffBoxId; } }

	//---------------------------------------------------------------------------------------------------
	public void Init(long lInstanceId, CsProofOfValorBuffBox csProofOfValorBuffBox, CsProofOfValorBuffBoxArrange csProofOfValorBuffBoxArrange)
	{
		Debug.Log("CsBuffBoxArea.Init(CsProofOfValorBuffBox)");
		BuffBoxArea();
		m_lInstanceId = lInstanceId;
		m_nBuffBoxId = csProofOfValorBuffBox.BuffBoxId;
		m_csProofOfValorBuffBox = csProofOfValorBuffBox;

		transform.position = new Vector3(csProofOfValorBuffBoxArrange.XPosition, csProofOfValorBuffBoxArrange.YPosition, csProofOfValorBuffBoxArrange.ZPosition);
		transform.eulerAngles = new Vector3(0, csProofOfValorBuffBoxArrange.YRotation, 0);
		m_capsuleCollider.radius = csProofOfValorBuffBoxArrange.AcquisitionRange - 0.5f;

		transform.Find(m_nBuffBoxId.ToString()).gameObject.SetActive(true);
		m_capsuleCollider.enabled = true;
		m_capsuleCollider.isTrigger = true;
	}

	//---------------------------------------------------------------------------------------------------
	public void Init(long lInstanceId, CsInfiniteWarBuffBox csInfiniteWarBuffBox)
	{
		Debug.Log("CsBuffBoxArea.Init(CsInfiniteWarBuffBox)");
		BuffBoxArea();
		m_lInstanceId = lInstanceId;
		m_nBuffBoxId = csInfiniteWarBuffBox.BuffBoxId;
		m_csInfiniteWarBuffBox = csInfiniteWarBuffBox;

		m_capsuleCollider.radius = CsDungeonManager.Instance.InfiniteWar.BuffBoxAcquisitionRange - 0.5f;

		transform.Find(m_nBuffBoxId.ToString()).gameObject.SetActive(true);
		m_capsuleCollider.enabled = true;
		m_capsuleCollider.isTrigger = true;
	}

	//---------------------------------------------------------------------------------------------------
	void BuffBoxArea()
	{
		m_csDungeonManager = CsDungeonManager.Instance;
		m_capsuleCollider = transform.GetComponent<CapsuleCollider>();
		gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
	}

	//---------------------------------------------------------------------------------------------------
	void OnDestroy()
	{
		m_capsuleCollider = null;
		m_csDungeonManager = null;
		m_csProofOfValorBuffBox = null;
		m_csInfiniteWarBuffBox = null;
	}

	//---------------------------------------------------------------------------------------------------
	public void Destroy()
	{
		m_capsuleCollider.enabled = false;
		m_capsuleCollider.isTrigger = false;
		transform.Find(m_nBuffBoxId.ToString()).gameObject.SetActive(false);
		transform.Find("Destroy").gameObject.SetActive(true);
		StartCoroutine(DelayDestroy());
	}

	//----------------------------------------------------------------------------------------------------
	IEnumerator DelayDestroy()
	{
		yield return new WaitForSeconds(3f);
		Destroy(gameObject);
	}

	//----------------------------------------------------------------------------------------------------
	public override void EnterAction()
	{
		if (m_csDungeonManager.DungeonPlay == EnDungeonPlay.ProofOfValor)
		{
			m_csDungeonManager.ProofOfValorBuffBoxAcquire(m_lInstanceId, m_csProofOfValorBuffBox);    // 유저 버프박스 진입 전달.
		}
		else if (m_csDungeonManager.DungeonPlay == EnDungeonPlay.InfiniteWar)
		{
			m_csDungeonManager.InfiniteWarBuffBoxAcquire(m_lInstanceId, m_csInfiniteWarBuffBox);    // 유저 버프박스 진입 전달.
		}
	}
}
