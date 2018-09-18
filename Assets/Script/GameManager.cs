using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Script
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private List<Color> colors;
        [SerializeField] private Player playerPrefab;
        [SerializeField] private string url;

        private int nbPlayers;
        private int sizeMap;
        private string[] lines;
        private Player[] players;
        private int index;

        private void Start()
        {
            //===== TEMP ======
            lines = File.ReadAllLines(url);
            //=================
            nbPlayers = int.Parse(lines[0]);
            sizeMap = int.Parse(lines[1]);
            players = new Player[nbPlayers];
            for (var i = 0; i < nbPlayers; i++)
            {
                var pos = lines[i + 2].Split(' ');
                playerPrefab.Color = colors[i % colors.Count];
                playerPrefab.InitialDirection = (Direction) int.Parse(pos[2]);
                players[i] = Instantiate(playerPrefab,
                    new Vector3(int.Parse(pos[0]) - 5, 0, int.Parse(pos[1]) - 5),
                    Quaternion.identity,
                    null);
            }

            index = nbPlayers + 2;
        }

        private float cumul;

        private void Update()
        {
            cumul += Time.deltaTime;
            if (cumul < 1)
                return;
            cumul -= 1;
            
            var dirs = lines[index].Split(' ');
            index++;
            for (var i = 0; i < nbPlayers; i++)
            {
                players[i].Turn((Direction) int.Parse(dirs[i]));
            }
        }
    }
}