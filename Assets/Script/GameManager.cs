using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Script
{
    public class GameManager : MonoBehaviour, ITimelineDependent
    {
        [SerializeField] private List<Color> colors;
        [SerializeField] private Player playerPrefab;
        [SerializeField] private string url;

        // Timeline tmp
        [SerializeField, Range(0, 1)] private float advancement;

        private int nbPlayers;
        private int sizeMap;
        private string[] lines;
        private Player[] players;
        private int nbTours;
        private bool ready;


        private void Start()
        {
            StartCoroutine(Tools.WebRequest(url, ParseGameInfo));
        }

        private void ParseGameInfo(string s)
        {
            lines = s.Split('\n');
            nbPlayers = lines[0].Split(' ').Length;
            if (nbPlayers == 1)
                nbPlayers = int.Parse(lines[0]);
            sizeMap = int.Parse(lines[1]);
            players = new Player[nbPlayers];
            for (var i = 0; i < nbPlayers; i++)
            {
                var pos = lines[i + 2].Split(' ');
                playerPrefab.Color = colors[i % colors.Count];
                players[i] = Instantiate(playerPrefab,
                    new Vector3(int.Parse(pos[0]) - 5, 0, int.Parse(pos[1]) - 5),
                    Quaternion.identity,
                    null);
            }
            nbTours = lines.Length - (nbPlayers + 2);
            if (lines[lines.Length - 1].Split(' ').Length != nbPlayers)
                nbTours--;
            for (var i = 0; i < nbTours; i++)
            {
                var moves = lines[nbPlayers + 2 + i].Split(' ');
                for (var p = 0; p < nbPlayers; p++)
                {
                    players[p].AddTrail((Direction) int.Parse(moves[p]), (float) i / nbTours, (float)(i + 1) / nbTours);
                }
            }
            ready = true;
        }


        private float cumul;


        //Timeline tmp
        private void Update()
        {
            if (!ready)
                return;
            TimelineUpdate(advancement);
        }

        public void TimelineUpdate(float f)
        {
            foreach (var p in players)
            {
                p.TimelineUpdate(f);
            }
        }
    }
}