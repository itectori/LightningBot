using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorMaker : MonoBehaviour {

	public static Color[] DivideColors(uint nbPlayers)
    {
        float d = 360 / nbPlayers;
        Color[] tab = new Color[nbPlayers];
        for(uint i = 0; i < nbPlayers; ++i)
            tab[i] = Color.HSVToRGB(d * i/360f , 69f/100f, 1);
        
        return tab;
    }
    public static string GetColoredText(string text, Color color)
    {
        string colString = ColorUtility.ToHtmlStringRGB(color);
        return "<#" + colString + ">" + text + "</color>";
    }
}
