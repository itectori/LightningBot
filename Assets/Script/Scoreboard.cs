using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviour {
    public string[] pTab;
    public Color[] cTab;

    private TMPro.TextMeshProUGUI Tmpro;

    // Use this for initialization
	IEnumerator Start () {
        Tmpro = GetComponent<TMPro.TextMeshProUGUI>();
        SetPlayers(new string []{"Itectori", "Dayblox", "Luka", "Jilowyn", "Retsila", "Findus", "Dahll", "Valou", "Nano", "Jaky"}, ColorMaker.DivideColors(10));
        yield return new WaitForSeconds(2);
        
        SetDead(2);
        yield return new WaitForSeconds(1);
        SetDead(1);

	}
    public void SetPlayers(string[] allPlayers, Color[] colorTab)
    {
        pTab = allPlayers;
        cTab = colorTab;
        DrawPlayers();
    }
    private Color changeColorToDead(Color c)
    {
        float H, S, V = 0f;
        Color.RGBToHSV(c,out H,out S,out V);
        return Color.HSVToRGB(H, S, 0.25f);
    }
    private void DrawPlayers()
    {
        Tmpro.text = "";
        for(var i = 0; i < pTab.Length; ++i)
            if (pTab[i] != "")
                Tmpro.text +=ColorMaker.GetColoredText(pTab[i], cTab[i]) + '\n';
    }
    public void SetDead(uint index)
    {
        cTab[index] = changeColorToDead(cTab[index]);
        DrawPlayers();
    }
}
