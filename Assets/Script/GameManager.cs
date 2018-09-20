using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Script
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private List<Color> colors;
        [SerializeField] private Player playerPrefab;
        [SerializeField] private string url;
        [SerializeField] private Camera mainCamera;

        private int nbPlayers;
        private int sizeMap;
        private string[] lines;
        private Player[] players;
        private int index;


        private bool ready;

        private IEnumerator Get()
        {
            using (var www = UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    lines = www.downloadHandler.text.Split('\n');
                    nbPlayers = lines[0].Split(' ').Length;
                    if (nbPlayers == 1)
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

                    mainCamera.orthographicSize = sizeMap / 2f;
                    ready = true;
                }
            }
        }


        private void Start()
        {
            StartCoroutine(Get());
        }


        private float cumul;

        private void Update()
        {
            if (!ready)
                return;

            cumul += Time.deltaTime;
            if (cumul < 1)
                return;
            cumul -= 1;

            if (index == lines.Length || lines[index] == "")
            {
                EndOfGame();
                return;
            }

            var dirs = lines[index].Split(' ');
            index++;
            for (var i = 0; i < nbPlayers; i++)
            {
                var dir = int.Parse(dirs[i]);
                if (dir >= 0)
                    players[i].Turn((Direction) dir);
                else
                    StopPlayer(i);
            }
        }

        private void StopPlayer(int i)
        {
            if (players[i] == null)
                return;
            players[i].Stop();
            players[i] = null;
        }

        private void EndOfGame()
        {
            for (var i = 0; i < nbPlayers; i++)
                StopPlayer(i);
            enabled = false;
        }
    }
}