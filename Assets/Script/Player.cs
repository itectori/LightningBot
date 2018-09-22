using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    public enum Direction
    {
        Right = 0,
        Down = 1,
        Left = 2,
        Up = 3
    }

    public class Player : MonoBehaviour, ITimelineDependent
    {        
        public Color Color;
        [HideInInspector] public Color DarkColor;
        public Trail TrailPrefab;


        private Direction direction;
        private List<Trail> trails;
        private float previousTime;

        private void Awake()
        {
            var pc = GetComponentInChildren<ParticleSystem>();

            Color.RGBToHSV(Color, out DarkColor.r, out DarkColor.g, out DarkColor.b);
            DarkColor = Color.HSVToRGB(DarkColor.r, DarkColor.g, DarkColor.b / 2);
            DarkColor.a = 1;
            var main = pc.main;
            main.startColor = new ParticleSystem.MinMaxGradient(Color, DarkColor);
            trails = new List<Trail>();
            instancePos = transform.position;
        }


        private Vector3 instancePos;

        public void AddTrail(Direction dir, float start, float end)
        {
            if (trails.Count == 0 || dir != direction)
            {
                TrailPrefab.StartF = start;
                TrailPrefab.EndF = end;
                TrailPrefab.Size = GameManager.Unit;
                TrailPrefab.Color = Color;
                TrailPrefab.CornerAngle = ((int) dir + (int) direction) % 3 == 0 ? 90 : 0; 
                trails.Add(Instantiate(TrailPrefab, instancePos, Quaternion.Euler(0, 90 * (int) dir, 0)));
                direction = dir;
            }
            else
            {
                trails.Add(trails.Last());
                trails.Last().Size += GameManager.Unit;
                trails.Last().EndF = end;
            }

            switch (direction)
            {
                case Direction.Right:
                    instancePos += Vector3.right * GameManager.Unit;
                    break;
                case Direction.Down:
                    instancePos += Vector3.back * GameManager.Unit;
                    break;
                case Direction.Left:
                    instancePos += Vector3.left * GameManager.Unit;
                    break;
                case Direction.Up:
                    instancePos += Vector3.forward * GameManager.Unit;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void TimelineUpdate(float f)
        {
            if (Math.Abs(f - previousTime) < 0.01f)
                return;
            if (f >= 1)
                f = 0.99f;
            var prevIndex = (int) (previousTime * trails.Count);
            var index = (int) (f * trails.Count);
            var start = prevIndex > index ? index : prevIndex;
            var end = prevIndex > index ? prevIndex : index;
            for (var i = start; i <= end; i++)
                trails[i].TimelineUpdate(f);
            previousTime = f;
            transform.position = trails[index].GetPos();
        }
    }
}