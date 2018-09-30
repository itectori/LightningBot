using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using UnityEditor;
using Color = UnityEngine.Color;

namespace Script
{
    public class GameManager : ATimelineDependent
    {
        public static readonly Color32 Clear = Color.black;

        private const int WIDTH = 1024;

        [SerializeField] private Texture2D texture;
        [SerializeField] private List<Color32> colors;
        [SerializeField] private bool local;
        [SerializeField] private string url;
        [SerializeField] private LightFlicker head;

        public static int TotalDuration;
        public static int TimeTurn;
        public static bool Ready;

        private static int nbPlayers;
        private static int sizeMap;
        public static float Unit;
        private static string[] lines;
        private static Player[] players;
        private static int nbTours;
        private static GameManager instance;

        private void Start()
        {
            instance = this;
            var fillColorArray = texture.GetPixels();
            for (var i = 0; i < fillColorArray.Length; ++i)
                fillColorArray[i] = Clear;
            texture.SetPixels(fillColorArray);
            texture.Apply();
            GetComponent<Renderer>().material.mainTexture = texture;

            if (local)
                ParseGameInfo(File.ReadAllText(url));
            else
                StartCoroutine(Tools.WebRequest(url, ParseGameInfo));
        }

        private void ParseGameInfo(string s)
        {
            lines = s.Split('\n');
            nbPlayers = lines[0].Split(' ').Length;
            sizeMap = int.Parse(lines[1]);
            TimeTurn = int.Parse(lines[2]);
            Unit = WIDTH / (float) sizeMap;
            players = new Player[nbPlayers];
            for (var i = 0; i < nbPlayers; i++)
            {
                var pos = lines[i + 3].Split(' ');
                players[i] = new Player(colors[i % colors.Count],
                    int.Parse(pos[0]),
                    int.Parse(pos[1]),
                    Instantiate(head));
            }

            nbTours = lines.Length - (nbPlayers + 3);
            if (lines[lines.Length - 1].Split(' ').Length != nbPlayers)
                nbTours--;
            TotalDuration = nbTours * TimeTurn;
            for (var i = 0; i < nbTours; i++)
            {
                var moves = lines[nbPlayers + 3 + i].Split(' ');
                for (var p = 0; p < nbPlayers; p++)
                {
                    players[p].AddTrail(int.Parse(moves[p]), (float) i / nbTours,
                        (float) (i + 1) / nbTours);
                }
            }

            Ready = true;
        }

        public override void TimelineUpdate(float t)
        {
            if (!Ready)
                return;
            foreach (var p in players)
            {
                p.TimelineUpdate(t);
            }

            texture.Apply();
        }


        public static int GridToImage(float pos)
        {
            return (int) (pos * Unit - Trail.WIDTH / 2);
        }


        public static void DrawRec(int x, int y, int width, int height, Color color)
        {
            for (var i = x; i < x + width; i++)
            {
                for (var j = y; j < y + height; j++)
                {
                    instance.texture.SetPixel(i, j, color);
                }
            }
        }

        public static float GridToWorld(float pos)
        {
            return -1; //(pos / Unit - Trail.WIDTH / (float) WIDTH) * 20;
        }
    }
}