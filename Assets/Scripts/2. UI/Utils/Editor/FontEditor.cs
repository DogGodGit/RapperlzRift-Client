using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

//-------------------------------------------------------------------------------------------------------
//작성: 김경훈 (2017-05-17) GUI v1.3
//-------------------------------------------------------------------------------------------------------

public class FontEditor : Editor
{

	// 유니티 Hierarchy에서 현재 선택된것 중 TEXT를 받아온 rgb값과 폰트사이즈로 변경시켜주는 함수(Bestfit도 적용됨)
    static public void rappelztext(byte r, byte g, byte b, int size)
    {
        if (Selection.gameObjects != null)
        {
            for (int i = 0; i < Selection.gameObjects.Length; i++)
            {
                if (Selection.gameObjects[i].GetComponent<Text>() != null)
                {
                    Text selectfont = Selection.gameObjects[i].GetComponent<Text>();
                    selectfont.color = new Color32(r, g, b, 255);
                    selectfont.fontSize = size;
					selectfont.resizeTextForBestFit = true;
					selectfont.resizeTextMaxSize = size;
					selectfont.resizeTextMinSize = 1;

					//Font font = (Font)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Resources/Fonts/NanumBarunGothic.ttf", typeof(Font));
					//selectfont.font = font;
                }
            }
        }
    }
	// 텍스트세팅(그라디언트사용)
	static public void RappelzGradient(byte startR, byte startG, byte startB, byte endR, byte endG, byte endB, int size)
	{
		if (Selection.gameObjects != null)
		{
			for (int i = 0; i < Selection.gameObjects.Length; i++)
			{
				if (Selection.gameObjects[i].GetComponent<Text>() != null)
				{
					Text selectfont = Selection.gameObjects[i].GetComponent<Text>();
					selectfont.fontSize = size;
					selectfont.resizeTextForBestFit = true;
					selectfont.resizeTextMaxSize = size;
					selectfont.resizeTextMinSize = 1;

					//Font font = (Font)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Resources/Fonts/NanumBarunGothic.ttf", typeof(Font));
					//selectfont.font = font;

					// Gradient
					if (Selection.gameObjects[i].GetComponent<CsGradient>() == null)
					{
						CsGradient selectGradient = Selection.gameObjects[i].AddComponent<CsGradient>();
						selectGradient.StartColor = new Color32(startR, startG, startB, 255);
						selectGradient.EndColor = new Color32(endR, endG, endB, 255);
					}
					else
					{
						CsGradient selectGradient = Selection.gameObjects[i].GetComponent<CsGradient>();
						selectGradient.StartColor = new Color32(startR, startG, startB, 255);
						selectGradient.EndColor = new Color32(endR, endG, endB, 255);
					}
				}
			}
		}
	}
	// OutLine
	static public void RappelzOutLine(byte r, byte g, byte b, int px)
	{
		if (Selection.gameObjects != null)
		{
			for (int i = 0; i < Selection.gameObjects.Length; i++)
			{
				if (Selection.gameObjects[i].GetComponent<Text>() != null)
				{
					if (Selection.gameObjects[i].GetComponent<Outline>() == null)
					{
						Outline selectOutline = Selection.gameObjects[i].AddComponent<Outline>();
						selectOutline.effectColor = new Color32(r, g, b, 255);
						selectOutline.effectDistance = new Vector2(px, px);
					}
					else
					{
						Outline selectOutline = Selection.gameObjects[i].GetComponent<Outline>();
						selectOutline.effectColor = new Color32(r, g, b, 255);
						selectOutline.effectDistance = new Vector2(px, px);
					}
				}
			}
		}
	}
	// Shadow
	static public void RappelzShadow(byte r, byte g, byte b, int x, int y)
	{
		if (Selection.gameObjects != null)
		{
			for (int i = 0; i < Selection.gameObjects.Length; i++)
			{
				if (Selection.gameObjects[i].GetComponent<Text>() != null)
				{
					if (Selection.gameObjects[i].GetComponent<Shadow>() == null)
					{
						Shadow selectShadow = Selection.gameObjects[i].AddComponent<Shadow>();
						selectShadow.effectColor = new Color32(r, g, b, 255);
						selectShadow.effectDistance = new Vector2(x, y);
					}
					else
					{
						Shadow selectShadow = Selection.gameObjects[i].GetComponent<Shadow>();
						selectShadow.effectColor = new Color32(r, g, b, 255);
						selectShadow.effectDistance = new Vector2(x, y);
					}
				}
			}
		}
	}
	

	[MenuItem("rappelz/OutLine/01")]
	static public void OutLine_01()
	{
		RappelzOutLine(0, 0, 0, 1);
	}

	[MenuItem("rappelz/Shadow/01")]
	static public void Shadow_01()
	{
		RappelzShadow(0, 0, 0, 2, -1);
	}
    


    #region 1280 720
    
    // Text_01
    [MenuItem("rappelz/text_01/01")]
    static public void text_01_01()
    {
        rappelztext(255, 255, 255, 18);
    }

    [MenuItem("rappelz/text_01/02")]
    static public void text_01_02()
    {
        rappelztext(255, 255, 255, 26);
    }

    [MenuItem("rappelz/text_01/03")]
    static public void text_01_03()
    {
        rappelztext(255, 255, 255, 29);
    }

    [MenuItem("rappelz/text_01/04")]
    static public void text_01_04()
    {
        rappelztext(255, 255, 255, 16);
    }
	
    [MenuItem("rappelz/text_01/05")]
    static public void text_01_05()
    {
        rappelztext(255, 255, 255, 34);
    }

    [MenuItem("rappelz/text_01/06")]
    static public void text_01_06()
    {
        rappelztext(255, 255, 255, 20);
    }

	[MenuItem("rappelz/text_01/07")]
	static public void text_01_07()
	{
		rappelztext(255, 255, 255, 23);
	}

	// Text_02
    [MenuItem("rappelz/text_02/01")]
    static public void text_02_01()
    {
        rappelztext(222, 222, 222, 18);
    }

    [MenuItem("rappelz/text_02/02")]
    static public void text_02_02()
    {
		rappelztext(222, 222, 222, 26);
    }

    [MenuItem("rappelz/text_02/03")]
    static public void text_02_03()
    {
		rappelztext(222, 222, 222, 29);
    }

    [MenuItem("rappelz/text_02/04")]
    static public void text_02_04()
    {
		rappelztext(222, 222, 222, 16);
    }

	[MenuItem("rappelz/text_02/05")]
	static public void text_02_05()
	{
		rappelztext(222, 222, 222, 34);
	}

	[MenuItem("rappelz/text_02/06")]
	static public void text_02_06()
	{
		rappelztext(222, 222, 222, 20);
	}

	[MenuItem("rappelz/text_02/07")]
	static public void text_02_07()
	{
		rappelztext(222, 222, 222, 23);
	}

	// Text_03
    [MenuItem("rappelz/text_03/01")]
    static public void text_03_01()
    {
        rappelztext(133, 141, 148, 18);
    }

    [MenuItem("rappelz/text_03/02")]
    static public void text_03_02()
    {
		rappelztext(133, 141, 148, 26);
    }

    [MenuItem("rappelz/text_03/03")]
    static public void text_03_03()
    {
		rappelztext(133, 141, 148, 29);
    }

    [MenuItem("rappelz/text_03/04")]
    static public void text_03_04()
    {
		rappelztext(133, 141, 148, 16);
    }

	[MenuItem("rappelz/text_03/05")]
	static public void text_03_05()
	{
		rappelztext(133, 141, 148, 34);
	}

	[MenuItem("rappelz/text_03/06")]
	static public void text_03_06()
	{
		rappelztext(133, 141, 148, 20);
	}

	[MenuItem("rappelz/text_03/07")]
	static public void text_03_07()
	{
		rappelztext(133, 141, 148, 23);
	}

    // Text_04
    [MenuItem("rappelz/text_04/01")]
    static public void text_04_01()
    {
        rappelztext(40, 121, 255, 18);
    }

    [MenuItem("rappelz/text_04/02")]
    static public void text_04_02()
    {
		rappelztext(40, 121, 255, 26);
    }

    [MenuItem("rappelz/text_04/03")]
    static public void text_04_03()
    {
		rappelztext(40, 121, 255, 29);
    }

    [MenuItem("rappelz/text_04/04")]
    static public void text_04_04()
    {
		rappelztext(40, 121, 255, 16);
    }

	[MenuItem("rappelz/text_04/05")]
	static public void text_04_05()
	{
		rappelztext(40, 121, 255, 34);
	}

	[MenuItem("rappelz/text_04/06")]
	static public void text_04_06()
	{
		rappelztext(40, 121, 255, 20);
	}

	[MenuItem("rappelz/text_04/07")]
	static public void text_04_07()
	{
		rappelztext(40, 121, 255, 23);
	}

    // Text_05
    [MenuItem("rappelz/text_05/01")]
    static public void text_05_01()
    {
        rappelztext(255, 214, 80, 18);
    }

    [MenuItem("rappelz/text_05/02")]
    static public void text_05_02()
    {
		rappelztext(255, 214, 80, 26);
    }

    [MenuItem("rappelz/text_05/03")]
    static public void text_05_03()
    {
		rappelztext(255, 214, 80, 29);
    }

    [MenuItem("rappelz/text_05/04")]
    static public void text_05_04()
    {
		rappelztext(255, 214, 80, 16);
    }

	[MenuItem("rappelz/text_05/05")]
	static public void text_05_05()
	{
		rappelztext(255, 214, 80, 34);
	}

	[MenuItem("rappelz/text_05/06")]
	static public void text_05_06()
	{
		rappelztext(255, 214, 80, 20);
	}

	[MenuItem("rappelz/text_05/07")]
	static public void text_05_07()
	{
		rappelztext(255, 214, 80, 23);
	}

    // Text_06
    [MenuItem("rappelz/text_06/01")]
    static public void text_06_01()
    {
        rappelztext(57, 142, 61, 18);
    }

    [MenuItem("rappelz/text_06/02")]
    static public void text_06_02()
    {
		rappelztext(57, 142, 61, 26);
    }

    [MenuItem("rappelz/text_06/03")]
    static public void text_06_03()
    {
		rappelztext(57, 142, 61, 29);
    }

    [MenuItem("rappelz/text_06/04")]
    static public void text_06_04()
    {
		rappelztext(57, 142, 61, 16);
    }

	[MenuItem("rappelz/text_06/05")]
	static public void text_06_05()
	{
		rappelztext(57, 142, 61, 34);
	}

	[MenuItem("rappelz/text_06/06")]
	static public void text_06_06()
	{
		rappelztext(57, 142, 61, 20);
	}

	[MenuItem("rappelz/text_06/07")]
	static public void text_06_07()
	{
		rappelztext(57, 142, 61, 23);
	}

    // Text_07
    [MenuItem("rappelz/text_07/01")]
    static public void text_07_01()
    {
        rappelztext(126, 87, 194, 18);
    }

    [MenuItem("rappelz/text_07/02")]
    static public void text_07_02()
    {
		rappelztext(126, 87, 194, 26);
    }

    [MenuItem("rappelz/text_07/03")]
    static public void text_07_03()
    {
		rappelztext(126, 87, 194, 29);
    }

    [MenuItem("rappelz/text_07/04")]
    static public void text_07_04()
    {
		rappelztext(126, 87, 194, 16);
    }

	[MenuItem("rappelz/text_07/05")]
	static public void text_07_05()
	{
		rappelztext(126, 87, 194, 34);
	}

	[MenuItem("rappelz/text_07/06")]
	static public void text_07_06()
	{
		rappelztext(126, 87, 194, 20);
	}

	[MenuItem("rappelz/text_07/07")]
	static public void text_07_07()
	{
		rappelztext(126, 87, 194, 23);
	}

    // Text_08
    [MenuItem("rappelz/text_08/01")]
    static public void text_08_01()
    {
        rappelztext(127, 213, 246, 18);
    }
    
    [MenuItem("rappelz/text_08/02")]
    static public void text_08_02()
    {
		rappelztext(127, 213, 246, 26);
    }
    
    [MenuItem("rappelz/text_08/03")]
    static public void text_08_03()
    {
		rappelztext(127, 213, 246, 29);
    }
    
    [MenuItem("rappelz/text_08/04")]
    static public void text_08_04()
    {
		rappelztext(127, 213, 246, 16);
    }

	[MenuItem("rappelz/text_08/05")]
	static public void text_08_05()
	{
		rappelztext(127, 213, 246, 34);
	}

	[MenuItem("rappelz/text_08/06")]
	static public void text_08_06()
	{
		rappelztext(127, 213, 246, 20);
	}

	[MenuItem("rappelz/text_08/07")]
	static public void text_08_07()
	{
		rappelztext(127, 213, 246, 23);
	}

    // Text_09
    [MenuItem("rappelz/text_09/01")]
    static public void text_09_01()
    {
        rappelztext(229, 115, 115, 18);
    }

    [MenuItem("rappelz/text_09/02")]
    static public void text_09_02()
    {
		rappelztext(229, 115, 115, 26);
    }

    [MenuItem("rappelz/text_09/03")]
    static public void text_09_03()
    {
		rappelztext(229, 115, 115, 29);
    }

    [MenuItem("rappelz/text_09/04")]
    static public void text_09_04()
    {
		rappelztext(229, 115, 115, 16);
    }

	[MenuItem("rappelz/text_09/05")]
	static public void text_09_05()
	{
		rappelztext(229, 115, 115, 34);
	}

	[MenuItem("rappelz/text_09/06")]
	static public void text_09_06()
	{
		rappelztext(229, 115, 115, 20);
	}

	[MenuItem("rappelz/text_09/07")]
	static public void text_09_07()
	{
		rappelztext(229, 115, 115, 23);
	}

	// Text_10
	[MenuItem("rappelz/text_10/01")]
	static public void text_10_01()
	{
		rappelztext(175, 213, 122, 18);
	}

	[MenuItem("rappelz/text_10/02")]
	static public void text_10_02()
	{
		rappelztext(175, 213, 122, 26);
	}

	[MenuItem("rappelz/text_10/03")]
	static public void text_10_03()
	{
		rappelztext(175, 213, 122, 29);
	}

	[MenuItem("rappelz/text_10/04")]
	static public void text_10_04()
	{
		rappelztext(175, 213, 122, 16);
	}

	[MenuItem("rappelz/text_10/05")]
	static public void text_10_05()
	{
		rappelztext(175, 213, 122, 34);
	}

	[MenuItem("rappelz/text_10/06")]
	static public void text_10_06()
	{
		rappelztext(175, 213, 122, 20);
	}

	[MenuItem("rappelz/text_10/07")]
	static public void text_10_07()
	{
		rappelztext(175, 213, 122, 23);
	}

	// Text_11
	[MenuItem("rappelz/text_11/01")]
	static public void text_11_01()
	{
		rappelztext(187, 32, 27, 18);
	}

	[MenuItem("rappelz/text_11/02")]
	static public void text_11_02()
	{
		rappelztext(187, 32, 27, 26);
	}

	[MenuItem("rappelz/text_11/03")]
	static public void text_11_03()
	{
		rappelztext(187, 32, 27, 29);
	}

	[MenuItem("rappelz/text_11/04")]
	static public void text_11_04()
	{
		rappelztext(187, 32, 27, 16);
	}

	[MenuItem("rappelz/text_11/05")]
	static public void text_11_05()
	{
		rappelztext(187, 32, 27, 34);
	}

	[MenuItem("rappelz/text_11/06")]
	static public void text_11_06()
	{
		rappelztext(187, 32, 27, 20);
	}

	[MenuItem("rappelz/text_11/07")]
	static public void text_11_07()
	{
		rappelztext(187, 32, 27, 23);
	}

	// Text_12
	[MenuItem("rappelz/text_12/01")]
	static public void text_12_01()
	{
		RappelzGradient(222, 222, 222, 133, 141, 148, 18);
	}

	[MenuItem("rappelz/text_12/02")]
	static public void text_12_02()
	{
		RappelzGradient(222, 222, 222, 133, 141, 148, 26);
	}

	[MenuItem("rappelz/text_12/03")]
	static public void text_12_03()
	{
		RappelzGradient(222, 222, 222, 133, 141, 148, 29);
	}

	[MenuItem("rappelz/text_12/04")]
	static public void text_12_04()
	{
		RappelzGradient(222, 222, 222, 133, 141, 148, 16);
	}

	[MenuItem("rappelz/text_12/05")]
	static public void text_12_05()
	{
		RappelzGradient(222, 222, 222, 133, 141, 148, 34);
	}

	[MenuItem("rappelz/text_12/06")]
	static public void text_12_06()
	{
		RappelzGradient(222, 222, 222, 133, 141, 148, 20);
	}

	[MenuItem("rappelz/text_12/07")]
	static public void text_12_07()
	{
		RappelzGradient(222, 222, 222, 133, 141, 148, 23);
	}

	// Text_13
	[MenuItem("rappelz/text_13/01")]
	static public void text_13_01()
	{
		RappelzGradient(254, 148, 0, 160, 35, 29, 18);
	}

	[MenuItem("rappelz/text_13/02")]
	static public void text_13_02()
	{
		RappelzGradient(254, 148, 0, 160, 35, 29, 26);
	}

	[MenuItem("rappelz/text_13/03")]
	static public void text_13_03()
	{
		RappelzGradient(254, 148, 0, 160, 35, 29, 29);
	}

	[MenuItem("rappelz/text_13/04")]
	static public void text_13_04()
	{
		RappelzGradient(254, 148, 0, 160, 35, 29, 16);
	}

	[MenuItem("rappelz/text_13/05")]
	static public void text_13_05()
	{
		RappelzGradient(254, 148, 0, 160, 35, 29, 34);
	}

	[MenuItem("rappelz/text_13/06")]
	static public void text_13_06()
	{
		RappelzGradient(254, 148, 0, 160, 35, 29, 20);
	}

	[MenuItem("rappelz/text_13/07")]
	static public void text_13_07()
	{
		RappelzGradient(254, 148, 0, 160, 35, 29, 23);
	}

	// Text_14
	[MenuItem("rappelz/text_14/01")]
	static public void text_14_01()
	{
		rappelztext(40, 183, 197, 18);
	}

	[MenuItem("rappelz/text_14/02")]
	static public void text_14_02()
	{
		rappelztext(40, 183, 197, 26);
	}

	[MenuItem("rappelz/text_14/03")]
	static public void text_14_03()
	{
		rappelztext(40, 183, 197, 29);
	}

	[MenuItem("rappelz/text_14/04")]
	static public void text_14_04()
	{
		rappelztext(40, 183, 197, 16);
	}

	[MenuItem("rappelz/text_14/05")]
	static public void text_14_05()
	{
		rappelztext(40, 183, 197, 34);
	}

	[MenuItem("rappelz/text_14/06")]
	static public void text_14_06()
	{
		rappelztext(40, 183, 197, 20);
	}

	[MenuItem("rappelz/text_14/07")]
	static public void text_14_07()
	{
		rappelztext(40, 183, 197, 23);
	}

	// Text_15
	[MenuItem("rappelz/text_15/01")]
	static public void text_15_01()
	{
		rappelztext(254, 148, 0, 18);
	}

	[MenuItem("rappelz/text_15/02")]
	static public void text_15_02()
	{
		rappelztext(254, 148, 0, 26);
	}

	[MenuItem("rappelz/text_15/03")]
	static public void text_15_03()
	{
		rappelztext(254, 148, 0, 29);
	}

	[MenuItem("rappelz/text_15/04")]
	static public void text_15_04()
	{
		rappelztext(254, 148, 0, 16);
	}

	[MenuItem("rappelz/text_15/05")]
	static public void text_15_05()
	{
		rappelztext(254, 148, 0, 34);
	}

	[MenuItem("rappelz/text_15/06")]
	static public void text_15_06()
	{
		rappelztext(254, 148, 0, 20);
	}

	[MenuItem("rappelz/text_15/07")]
	static public void text_15_07()
	{
		rappelztext(254, 148, 0, 23);
	}

	// Text_16
	[MenuItem("rappelz/text_16/01")]
	static public void text_16_01()
	{
		rappelztext(51, 153, 0, 18);
	}

	[MenuItem("rappelz/text_16/02")]
	static public void text_16_02()
	{
		rappelztext(51, 153, 0, 26);
	}

	[MenuItem("rappelz/text_16/03")]
	static public void text_16_03()
	{
		rappelztext(51, 153, 0, 29);
	}

	[MenuItem("rappelz/text_16/04")]
	static public void text_16_04()
	{
		rappelztext(51, 153, 0, 16);
	}

	[MenuItem("rappelz/text_16/05")]
	static public void text_16_05()
	{
		rappelztext(51, 153, 0, 34);
	}

	[MenuItem("rappelz/text_16/06")]
	static public void text_16_06()
	{
		rappelztext(51, 153, 0, 20);
	}

	[MenuItem("rappelz/text_16/07")]
	static public void text_16_07()
	{
		rappelztext(51, 153, 0, 23);
	}

	// Text_17
	[MenuItem("rappelz/text_17/01")]
	static public void text_17_01()
	{
		rappelztext(204, 51, 204, 18);
	}

	[MenuItem("rappelz/text_17/02")]
	static public void text_17_02()
	{
		rappelztext(204, 51, 204, 26);
	}

	[MenuItem("rappelz/text_17/03")]
	static public void text_17_03()
	{
		rappelztext(204, 51, 204, 29);
	}

	[MenuItem("rappelz/text_17/04")]
	static public void text_17_04()
	{
		rappelztext(204, 51, 204, 16);
	}

	[MenuItem("rappelz/text_17/05")]
	static public void text_17_05()
	{
		rappelztext(204, 51, 204, 34);
	}

	[MenuItem("rappelz/text_17/06")]
	static public void text_17_06()
	{
		rappelztext(204, 51, 204, 20);
	}

	[MenuItem("rappelz/text_17/07")]
	static public void text_17_07()
	{
		rappelztext(204, 51, 204, 23);
	}

	// Text_18
	[MenuItem("rappelz/text_18/01")]
	static public void text_18_01()
	{
		rappelztext(255, 102, 204, 18);
	}

	[MenuItem("rappelz/text_18/02")]
	static public void text_18_02()
	{
		rappelztext(255, 102, 204, 26);
	}

	[MenuItem("rappelz/text_18/03")]
	static public void text_18_03()
	{
		rappelztext(255, 102, 204, 29);
	}

	[MenuItem("rappelz/text_18/04")]
	static public void text_18_04()
	{
		rappelztext(255, 102, 204, 16);
	}

	[MenuItem("rappelz/text_18/05")]
	static public void text_18_05()
	{
		rappelztext(255, 102, 204, 34);
	}

	[MenuItem("rappelz/text_18/06")]
	static public void text_18_06()
	{
		rappelztext(255, 102, 204, 20);
	}

	[MenuItem("rappelz/text_18/07")]
	static public void text_18_07()
	{
		rappelztext(255, 102, 204, 23);
	}


	// Text_19
	[MenuItem("rappelz/text_19/01")]
	static public void text_19_01()
	{
		rappelztext(171, 130, 255, 18);
	}

	[MenuItem("rappelz/text_19/02")]
	static public void text_19_02()
	{
		rappelztext(171, 130, 255, 26);
	}

	[MenuItem("rappelz/text_19/03")]
	static public void text_19_03()
	{
		rappelztext(171, 130, 255, 29);
	}

	[MenuItem("rappelz/text_19/04")]
	static public void text_19_04()
	{
		rappelztext(171, 130, 255, 16);
	}

	[MenuItem("rappelz/text_19/05")]
	static public void text_19_05()
	{
		rappelztext(171, 130, 255, 34);
	}

	[MenuItem("rappelz/text_19/06")]
	static public void text_19_06()
	{
		rappelztext(171, 130, 255, 20);
	}

	[MenuItem("rappelz/text_19/07")]
	static public void text_19_07()
	{
		rappelztext(171, 130, 255, 23);
	}

    // Text_20
    [MenuItem("rappelz/text_20/01")]
    static public void text_20_01()
    {
        rappelztext(206, 170, 139, 18);
    }

    [MenuItem("rappelz/text_20/02")]
    static public void text_20_02()
    {
        rappelztext(206, 170, 139, 26);
    }

    [MenuItem("rappelz/text_20/03")]
    static public void text_20_03()
    {
        rappelztext(206, 170, 139, 29);
    }

    [MenuItem("rappelz/text_20/04")]
    static public void text_20_04()
    {
        rappelztext(206, 170, 139, 16);
    }

    [MenuItem("rappelz/text_20/05")]
    static public void text_20_05()
    {
        rappelztext(206, 170, 139, 34);
    }

    [MenuItem("rappelz/text_20/06")]
    static public void text_20_06()
    {
        rappelztext(206, 170, 139, 20);
    }

	[MenuItem("rappelz/text_20/07")]
	static public void text_20_07()
	{
		rappelztext(206, 170, 139, 23);
	}

    [MenuItem("rappelz/text_20/08")]
    static public void text_20_08()
    {
        rappelztext(206, 170, 139, 40);
    }

    // Text_21
    [MenuItem("rappelz/text_21/01")]
    static public void text_21_01()
    {
        rappelztext(35, 216, 128, 18);
    }

    [MenuItem("rappelz/text_21/02")]
    static public void text_21_02()
    {
        rappelztext(35, 216, 128, 26);
    }

    [MenuItem("rappelz/text_21/03")]
    static public void text_21_03()
    {
        rappelztext(35, 216, 128, 29);
    }

    [MenuItem("rappelz/text_21/04")]
    static public void text_21_04()
    {
        rappelztext(35, 216, 128, 16);
    }

    [MenuItem("rappelz/text_21/05")]
    static public void text_21_05()
    {
        rappelztext(35, 216, 128, 34);
    }

    [MenuItem("rappelz/text_21/06")]
    static public void text_21_06()
    {
        rappelztext(35, 216, 128, 20);
    }

	[MenuItem("rappelz/text_21/07")]
	static public void text_21_07()
	{
		rappelztext(35, 216, 128, 23);
	}

    // Text_22
    [MenuItem("rappelz/text_22/01")]
    static public void text_22_01()
    {
        rappelztext(144, 167, 242, 18);
    }

    [MenuItem("rappelz/text_22/02")]
    static public void text_22_02()
    {
        rappelztext(144, 167, 242, 26);
    }

    [MenuItem("rappelz/text_22/03")]
    static public void text_22_03()
    {
        rappelztext(144, 167, 242, 29);
    }

    [MenuItem("rappelz/text_22/04")]
    static public void text_22_04()
    {
        rappelztext(144, 167, 242, 16);
    }

    [MenuItem("rappelz/text_22/05")]
    static public void text_22_05()
    {
        rappelztext(144, 167, 242, 34);
    }

    [MenuItem("rappelz/text_22/06")]
    static public void text_22_06()
    {
        rappelztext(144, 167, 242, 20);
    }

	[MenuItem("rappelz/text_22/07")]
	static public void text_22_07()
	{
		rappelztext(144, 167, 242, 23);
	}

    // Text_23
    [MenuItem("rappelz/text_23/01")]
    static public void text_23_01()
    {
        rappelztext(246, 231, 180, 18);
    }

    [MenuItem("rappelz/text_23/02")]
    static public void text_23_02()
    {
        rappelztext(246, 231, 180, 26);
    }

    [MenuItem("rappelz/text_23/03")]
    static public void text_23_03()
    {
        rappelztext(246, 231, 180, 29);
    }

    [MenuItem("rappelz/text_23/04")]
    static public void text_23_04()
    {
        rappelztext(246, 231, 180, 16);
    }

    [MenuItem("rappelz/text_23/05")]
    static public void text_23_05()
    {
        rappelztext(246, 231, 180, 34);
    }

    [MenuItem("rappelz/text_23/06")]
    static public void text_23_06()
    {
        rappelztext(246, 231, 180, 20);
    }

	[MenuItem("rappelz/text_23/07")]
	static public void text_23_07()
	{
		rappelztext(246, 231, 180, 23);
	}

    // Text_24
    [MenuItem("rappelz/text_24/01")]
    static public void text_24_01()
    {
        rappelztext(252, 199, 81, 18);
    }

    [MenuItem("rappelz/text_24/02")]
    static public void text_24_02()
    {
        rappelztext(252, 199, 81, 26);
    }

    [MenuItem("rappelz/text_24/03")]
    static public void text_24_03()
    {
        rappelztext(252, 199, 81, 29);
    }

    [MenuItem("rappelz/text_24/04")]
    static public void text_24_04()
    {
        rappelztext(252, 199, 81, 16);
    }

    [MenuItem("rappelz/text_24/05")]
    static public void text_24_05()
    {
        rappelztext(252, 199, 81, 34);
    }

    [MenuItem("rappelz/text_24/06")]
    static public void text_24_06()
    {
        rappelztext(252, 199, 81, 20);
    }

	[MenuItem("rappelz/text_24/07")]
	static public void text_24_07()
	{
		rappelztext(252, 199, 81, 23);
	}

    // Text_25
    [MenuItem("rappelz/text_25/01")]
    static public void text_25_01()
    {
        rappelztext(236, 75, 38, 18);
    }

    [MenuItem("rappelz/text_25/02")]
    static public void text_25_02()
    {
        rappelztext(236, 75, 38, 26);
    }

    [MenuItem("rappelz/text_25/03")]
    static public void text_25_03()
    {
        rappelztext(236, 75, 38, 29);
    }

    [MenuItem("rappelz/text_25/04")]
    static public void text_25_04()
    {
        rappelztext(236, 75, 38, 16);
    }

    [MenuItem("rappelz/text_25/05")]
    static public void text_25_05()
    {
        rappelztext(236, 75, 38, 34);
    }

    [MenuItem("rappelz/text_25/06")]
    static public void text_25_06()
    {
        rappelztext(236, 75, 38, 20);
    }

	[MenuItem("rappelz/text_25/07")]
	static public void text_25_07()
	{
		rappelztext(236, 75, 38, 23);
	}

    // Text_26
    [MenuItem("rappelz/text_26/01")]
    static public void text_26_01()
    {
        rappelztext(247, 120, 246, 18);
    }

    [MenuItem("rappelz/text_26/02")]
    static public void text_26_02()
    {
        rappelztext(247, 120, 246, 26);
    }

    [MenuItem("rappelz/text_26/03")]
    static public void text_26_03()
    {
        rappelztext(247, 120, 246, 29);
    }

    [MenuItem("rappelz/text_26/04")]
    static public void text_26_04()
    {
        rappelztext(247, 120, 246, 16);
    }

    [MenuItem("rappelz/text_26/05")]
    static public void text_26_05()
    {
        rappelztext(247, 120, 246, 34);
    }

    [MenuItem("rappelz/text_26/06")]
    static public void text_26_06()
    {
        rappelztext(247, 120, 246, 20);
    }

	[MenuItem("rappelz/text_26/07")]
	static public void text_26_07()
	{
		rappelztext(247, 120, 246, 23);
	}

    #endregion 1280 720
}
