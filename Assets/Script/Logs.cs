using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Script
{
    public class Logs : MonoBehaviour {
        TMPro.TextMeshProUGUI[] textList;
        private int listIndex;
        Queue<string> queue;

        static string[] players;
        static Color[] colors;
        static Func<string, string>[] TABLE = {
            SendMove,
            Kill,
            InvalidMove,
            MoveTooLate
        };
        static string[] direction =
        {
            "right",
            "down",
            "left",
            "up"
        };

        // Use this for initialization
        void Start () {
            textList = GetComponentsInChildren<TMPro.TextMeshProUGUI>(true);
            print(textList.Length);

            queue = new Queue<string>();
            StartCoroutine(LogsCor());
            queue.Enqueue("PlayerX shot PlayerY");
            queue.Enqueue("PlayerX Disconnected");
            queue.Enqueue("PlayerX wants to go up");
            queue.Enqueue("<#ffff80>PlayerX</color> go up");
            queue.Enqueue("<#ffff80>PlayerX</color> killed <#4775FF>Player Y</color>");
            queue.Enqueue("<#ffff80>PlayerX</color> go up");
            queue.Enqueue("<#ffff80>PlayerX</color> go up");
            queue.Enqueue("<#ffff80>PlayerX</color> go up");
            queue.Enqueue("<#ffff80>PlayerX</color> go right");
            queue.Enqueue("<#ffff80>PlayerX</color> go right");
            queue.Enqueue("<#ffff80>PlayerX</color> go left");
            queue.Enqueue("<#ffff80>PlayerX</color> go up");
            queue.Enqueue("<#ffff80>PlayerX</color> go down");
            queue.Enqueue("<#ffff80>PlayerX</color> go up");
            queue.Enqueue("<#ffff80>PlayerX</color> go up");
            queue.Enqueue("<#ffff80>PlayerX</color> go Left");
            queue.Enqueue(ColorMaker.GetColoredText("PlayerX", Color.cyan)+" go up");

        }
	
        public void SendLog(string log)
        {
            queue.Enqueue(log);
        }


        private TMPro.TextMeshProUGUI getAText()
        {
            var toRet = textList[listIndex];
            listIndex = ++listIndex % 10;
            return toRet;
        }
        public void Init(string[] p, Color[]c)
        {
            players = p;
            colors = c;
        }
        public static string Decode(string log)
        {
            var action = int.Parse(log.Split()[1]);
            return TABLE[action](log);
        }
        private static string SendMove(string s)
        {
            var args = s.Split();
            int num = int.Parse(args[2]);
            var coloredPlayer = ColorMaker.GetColoredText(players[num], colors[num]);
            return $"{coloredPlayer} go {direction[int.Parse(args[3])]}";
        }
        private static string Kill(string s)
        {
            var args = s.Split();
            var a1 = int.Parse(args[2]);
            var a2 = int.Parse(args[3]);
            var p1 = ColorMaker.GetColoredText(players[a1], colors[a1]);
            var p2 = ColorMaker.GetColoredText(players[a2], colors[a2]);
            return $"{p1} killed {p2}";
        }
        private static string InvalidMove(string s)
        {
            var args = s.Split();
            var a1 = int.Parse(args[2]);
            var p1 = ColorMaker.GetColoredText(players[a1], colors[a1]);
            return $"{p1} sent invalid move";
        }
        private static string MoveTooLate(string s)
        {
            var args = s.Split();
            var a1 = int.Parse(args[2]);
            var p1 = ColorMaker.GetColoredText(players[a1], colors[a1]);
            return $"{p1} sent moved too late";
        }


        IEnumerator LogsCor()
        {
            string log = null;
            while(true)
            {
                if (queue.Count != 0)
                {
                    log = queue.Dequeue();
                    foreach (var t in textList)
                    {
                        t.rectTransform.localPosition = new Vector3(t.rectTransform.localPosition.x, t.rectTransform.localPosition.y + 15);
                    }
                
                    var container = getAText();
                    container.rectTransform.localPosition = new Vector3(container.rectTransform.localPosition.x, -95);
                    container.text = "";
                    container.gameObject.SetActive(true);
                    for (int i = 0; i < log.Length; ++i)
                    {
                        if (log[i] == '<')
                        {
                            string tag = "";
                            for (; log[i] != '>' && i < log.Length; ++i)
                            {
                                tag += log[i];
                            }
                            container.text += tag + log[i];
                        }
                        else
                            container.text += log[i];
                        yield return new WaitForSeconds(0.01f);
                    }
                }
                yield return null;
            }
        }
    }
}
