using System.Collections.Generic;
using UnityEngine;
using System;

namespace Script
{
    public class Logs : ATimelineDependent
    {
        private TMPro.TextMeshProUGUI container;
        private int firstLog;
        private int lastLog;
        private const int NB_LOGS = 30;


        private static List<float> logTimeline;
        private static List<string> logs;
        private static string[] players;
        private static Color[] colors;

        private static readonly Func<string, string>[] table =
        {
            SendMove,
            Kill,
            InvalidMove,
            MoveTooLate
        };

        private void Start()
        {
            container = GetComponentInChildren<TMPro.TextMeshProUGUI>();
            container.text = "";
        }

        private static readonly string[] direction =
        {
            "right",
            "down",
            "left",
            "up"
        };

        public static void Init(string[] p, Color[] c)
        {
            players = p;
            colors = c;
            logTimeline = new List<float>();
            logs = new List<string>();
        }

        public static void AddLog(string log, float time)
        {
            logTimeline.Add(time);
            logs.Add(log);
        }

        public static string Decode(string log)
        {
            var action = int.Parse(log.Split()[1]);
            return table[action](log);
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

        public override void TimelineUpdate(float t)
        {
            while (lastLog < logTimeline.Count && logTimeline[lastLog] <= t)
                lastLog++;
            if (lastLog > logTimeline.Count)
                lastLog = logTimeline.Count;
            while (lastLog - 1 > 0 && logTimeline[lastLog - 1] > t)
                lastLog--;

            firstLog = lastLog - NB_LOGS;
            firstLog = firstLog < 0 ? 0 : firstLog;

            var text = "";
            for (var i = firstLog; i < lastLog - 1; i++)
                text += logs[i] + "\n";
            if (lastLog != 0)
                text += logs[lastLog - 1];
            container.text = text;
        }
    }
}