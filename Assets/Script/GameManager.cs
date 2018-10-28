using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Color = UnityEngine.Color;

namespace Script
{
    public class GameManager : ATimelineDependent
    {
        private struct ColorPlayer : IComparable
        {
            public readonly string Name;
            public readonly Color Color;
            public readonly int Index;

            public ColorPlayer(string name, Color color, int index)
            {
                Name = name;
                Color = color;
                Index = index;
            }

            public int CompareTo(object obj)
            {
                return obj is ColorPlayer
                    ? string.Compare(Name, ((ColorPlayer) obj).Name, StringComparison.Ordinal)
                    : 0;
            }
        }

        public static readonly Color32 Clear = Color.black;

        private const int WIDTH = 1024;

        [SerializeField] private Texture2D texture;
        [SerializeField] private bool local;
        [SerializeField] private string url;
        [SerializeField] private GameObject head;

        public static int TotalDuration;
        public static int TimeTurn;
        private static int nbPlayers;
        private static int sizeMap;
        public static float Unit;
        private static Player[] players;
        private static int nbTours;
        private static GameManager instance;
        private static float scaleMap;
        private static bool readySave;
        private static bool readyLogs;

        public static bool Ready => readySave && readyLogs;

        private void Start()
        {
            instance = this;
            scaleMap = transform.localScale.x;
            GetComponent<Renderer>().material.mainTexture = texture;
            ClearGame();
            if (local)
            {
                ParseGameSave(File.ReadAllText(url));
                readyLogs = true;
            }
        }

        public static void NewGame(string game)
        {
            Timeline.ResetTime();
            GameName.SetGameName(game);
            if (players != null)
                foreach (var p in players)
                    p.Dispose();

            readySave = false;
            readyLogs = false;
            instance.StopAllCoroutines();
            instance.ClearGame();
            instance.StartCoroutine(Tools.WebRequest("https://lightningbot.tk/games/" + game,
                instance.ParseGameSave));
            instance.StartCoroutine(Tools.WebRequest("https://lightningbot.tk/logs/" + game,
                instance.ParseGameLogs));
        }

        private void ClearGame()
        {
            var fillColorArray = texture.GetPixels();
            for (var i = 0; i < fillColorArray.Length; ++i)
                fillColorArray[i] = Clear;
            texture.SetPixels(fillColorArray);
            texture.Apply();
        }

        private void ParseGameLogs(string s)
        {
            var lines = s.Split('\n');
            var totalMs = int.Parse(lines[0]);
            var pseudo = lines[1].Split(' ');
            Logs.Init(pseudo, ColorMaker.DivideColors((uint) pseudo.Length));

            for (var i = 2; i < lines.Length - 1; i++)
                Logs.AddLog(Logs.Decode(lines[i]), float.Parse(lines[i].Split(' ')[0]) / totalMs);

            readyLogs = true;
        }

        private void ParseGameSave(string s)
        {
            var lines = s.Split('\n');
            var playersName = lines[0].Split(' ').ToList();
            nbPlayers = playersName.Count;
            var colors = ColorMaker.DivideColors((uint) nbPlayers);

            var sortPlayers = new List<ColorPlayer>();
            for (var i = 0; i < nbPlayers; i++)
            {
                sortPlayers.Add(new ColorPlayer(playersName[i], colors[i], i));
            }

            sortPlayers.Sort();
            playersName.Clear();
            var sortedColors = new Color[nbPlayers];
            for (var i = 0; i < nbPlayers; i++)
            {
                playersName.Add(sortPlayers[i].Name);
                sortedColors[i] = sortPlayers[i].Color;
            }

            sizeMap = int.Parse(lines[1]);
            TimeTurn = int.Parse(lines[2]);
            Unit = WIDTH / (float) sizeMap;
            players = new Player[nbPlayers];
            Scoreboard.SetPlayers(playersName.ToArray(), sortedColors);
            for (var i = 0; i < nbPlayers; i++)
            {
                var pos = lines[i + 3].Split(' ');
                players[i] = new Player(colors[i],
                    int.Parse(pos[0]),
                    int.Parse(pos[1]),
                    Instantiate(head, transform.parent),
                    sortPlayers.FindIndex(c => c.Index == i));
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

            readySave = true;
        }

        public override void TimelineUpdate(float t, bool manual)
        {
            if (!readySave)
                return;
            foreach (var p in players)
            {
                p.TimelineUpdate(t, manual);
            }

            texture.Apply();
        }


        public static int GridToImage(float pos)
        {
            return (int) (pos * Unit) - Trail.WIDTH / 2 - (int) (Unit / 2);
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
            return ((pos / sizeMap - Unit / WIDTH / 2f) * scaleMap % scaleMap + scaleMap) % scaleMap;
        }
    }
}