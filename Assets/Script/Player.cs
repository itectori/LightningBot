﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Script
{
    public enum Direction
    {
        Right = 0,
        Down = 1,
        Left = 2,
        Up = 3
    }

    public class Player : ITimelineDepend
    {
        private readonly Color color;
        private readonly int startX;
        private readonly int startY;
        private readonly Transform head;
        private readonly List<Trail> trail = new List<Trail>();

        public Player(Color color, int startX, int startY, LightFlicker head)
        {
            this.color = color;
            this.startX = startX;
            this.startY = startY;
            this.head = head.transform;
            xInstantiate = startX;
            yInstantiate = startY;
            GameManager.DrawRec(
                GameManager.GridToImage(startX),
                GameManager.GridToImage(startY),
                Trail.WIDTH, Trail.WIDTH, color);
            head.transform.position = new Vector3(
                GameManager.GridToWorld(xInstantiate),
                1,
                GameManager.GridToWorld(yInstantiate));
            head.SetColor(color);
        }


        private Direction direction;
        private float previousTime;
        private int xInstantiate;
        private int yInstantiate;

        public void AddTrail(int dir, float start, float end)
        {
            if (dir < 0)
            {
                trail.Add(null);
                return;
            }

            var dir_ = (Direction) dir;
            if (trail.Count == 0 || dir_ != direction)
            {
                trail.Add(new Trail(xInstantiate, yInstantiate, start, end, dir_, color));
                direction = dir_;
            }
            else
            {
                trail.Add(trail.Last());
                trail.Last().IncrSize();
                trail.Last().SetEndF(end);
            }

            switch (direction)
            {
                case Direction.Right:
                    xInstantiate++;
                    break;
                case Direction.Down:
                    yInstantiate--;
                    break;
                case Direction.Left:
                    xInstantiate--;
                    break;
                case Direction.Up:
                    yInstantiate++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void TimelineUpdate(float t)
        {
            var prevIndex = (int) (previousTime * trail.Count);
            var index = (int) (t * trail.Count);
            prevIndex = prevIndex == trail.Count ? prevIndex - 1 : prevIndex;
            index = index == trail.Count ? index - 1 : index;
            var start = prevIndex > index ? index : prevIndex;
            var end = prevIndex > index ? prevIndex : index;
            for (var i = start; i <= end; i++)
                trail[i]?.TimelineUpdate(t);
            previousTime = t;
            if (trail[index] != null)
                head.position = trail[index].GetPos();
        }
    }
}