using UnityEngine;
using System.Collections;
using System.Linq;

//[ExecuteInEditMode]
public class CsClipObject : MonoBehaviour
{
	float m_flDownTime = 0.0f;
	float m_flDefaultSizeY; // 컬링 시작 높이
	float m_flClipHeightY; // 숨길 위치
	Material sharedMaterial;

	Vector3 plane1Position = Vector3.zero;
	Vector3 plane1Rotation = new Vector3(180, 0, 0);

	public void Start() 
	{
		plane1Position = transform.position;
		sharedMaterial = GetComponent<MeshRenderer>().material;
		sharedMaterial.DisableKeyword("CLIP_TWO"); // 클립영역 2와 3은 사용하지 않습니다.
		sharedMaterial.DisableKeyword("CLIP_THREE");

		plane1Rotation = new Vector3(180f, 0, 0); // 컬링 영역 상하 반대로 설정함. 뒤집으면 아래에서 위로 컬링이 세팅됨.
	}


	public bool isEnable = false;

	public void Update()
	{
		//Material materialPlayer = CsGameData.Instance.MyHeroTransform.GetComponentInChildren<MeshRenderer>().material;
		//if (materialPlayer != null)
		//{
		//    if (materialPlayer.renderQueue > sharedMaterial.renderQueue)
		//    {
		//        isEnable = true;
		//    }
		//    else
		//    {
		//        isEnable = false;
		//    }
		//}
		//else
		//{
		//    isEnable = false;
		//}
		//sharedMaterial.renderQueue
		//Debug.Log(sharedMaterial.GetInt("unity_GUIZTestMode"));
		//if (sharedMaterial.GetInt("unity_GUIZTestMode") == 2)
		if(isEnable)
		{
			sharedMaterial.EnableKeyword("CLIP_ONE"); // 클립영역 1을 실행 (쉐이더)

			if (CsGameData.Instance.MyHeroTransform != null)
			{
				m_flDefaultSizeY = CsGameData.Instance.MyHeroTransform.position.y + GetComponent<MeshRenderer>().bounds.size.y; // 오브젝트에 최고점+ 플레이어의 높이 에서 부터
				m_flClipHeightY = CsGameData.Instance.MyHeroTransform.position.y + 1; //플레이어의 높이+ 지상 1미터 까지
				//sharedMaterial.SetVector("_planePos", CsGameData.Instance.MyHeroTransform.position + Vector3.up);
				//sharedMaterial.SetVector("_planePos", new Vector3(0, m_flClipHeightY,0));
				sharedMaterial.SetVector("_planePos", new Vector3(0, Mathf.Lerp(m_flDefaultSizeY, m_flClipHeightY, m_flDownTime),0));

				m_flDownTime += Time.deltaTime;
			}

			sharedMaterial.SetVector("_planeNorm", Quaternion.Euler(plane1Rotation) * Vector3.up);
		}
		else
		{
			sharedMaterial.DisableKeyword("CLIP_ONE");
			m_flDownTime = 0.0f;
		}
	}
}