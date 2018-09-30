using UnityEngine;

namespace Script
{
    public static class ColorMaker {

        public static Color[] DivideColors(uint nbPlayers)
        {
            var d = 360f / nbPlayers;
            var tab = new Color[nbPlayers];
            for(uint i = 0; i < nbPlayers; ++i)
                tab[i] = Color.HSVToRGB(d * i/360f , 69f/100f, 1);
        
            return tab;
        }
        
        public static string GetColoredText(string text, Color color)
        {
            var colString = ColorUtility.ToHtmlStringRGB(color);
            return "<#" + colString + ">" + text + "</color>";
        }
    
        public static Color ChangeColorToDead(Color c)
        {
            float h, s, v;
            Color.RGBToHSV(c, out h, out s, out v);
            return Color.HSVToRGB(h, s, 0.25f);
        }
    }
}
