using UnityEngine;

namespace Script
{
    public class Scoreboard : MonoBehaviour
    {
        private static string[] pTab;
        private static Color[] cTab;
        private static Color[] deadColors;
        private static bool[] alive;
        private static TMPro.TextMeshProUGUI tmpro;

        private void Start()
        {
            tmpro = GetComponent<TMPro.TextMeshProUGUI>();
            if (cTab != null)
                DrawPlayers();
        }

        public static void SetPlayers(string[] allPlayers, Color[] colorTab)
        {
            pTab = allPlayers;
            cTab = colorTab;
            alive = new bool[allPlayers.Length];
            deadColors = new Color[allPlayers.Length];
            for (var i = 0; i < allPlayers.Length; i++)
            {
                alive[i] = true;
                deadColors[i] = ColorMaker.ChangeColorToDead(cTab[i]);
            }

            if (tmpro)
                DrawPlayers();
        }

        private static void DrawPlayers()
        {
            tmpro.text = "";
            for (var i = 0; i < pTab.Length; ++i)
                tmpro.text += ColorMaker.GetColoredText(pTab[i],
                                  alive[i] ? cTab[i] : deadColors[i]
                              ) + '\n';
        }

        public static void SetDead(int index)
        {
            if (!alive[index])
                return;
            alive[index] = false;
            DrawPlayers();
        }

        public static void SetAlive(int index)
        {
            if (alive[index])
                return;
            alive[index] = true;
            DrawPlayers();
        }
    }
}