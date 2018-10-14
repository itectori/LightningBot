using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Script
{
    public enum Direction
    {
        Right = 0,
        Down = 1,
        Left = 2,
        Up = 3
    }

    public class Player : ITimelineDepend, IDisposable
    {
        private readonly Color color;
        private readonly Transform head;
        private readonly List<Trail> trail = new List<Trail>();
        private readonly int indexScoreboard;
        private bool indestructible;
        private bool dead = false;

        public Player(Color color, int startX, int startY, GameObject head, int indexScoreboard)
        {
            this.color = color;
            this.indexScoreboard = indexScoreboard;
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
            head.GetComponentInChildren<ParticleSystem>().startColor = color;
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
                if (dir == -1)
                    indestructible = true;
                return;
            }

            var dirTmp = (Direction) dir;
            if (trail.Count == 0 || dirTmp != direction)
            {
                trail.Add(new Trail(xInstantiate, yInstantiate, start, end, dirTmp, color));
                direction = dirTmp;
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

        private int lastNotNull;

        public void TimelineUpdate(float t)
        {
            var prevIndex = (int) (previousTime * trail.Count);
            var index = (int) (t * trail.Count);
            prevIndex = prevIndex == trail.Count ? prevIndex - 1 : prevIndex;
            index = index == trail.Count ? index - 1 : index;
            var start = prevIndex > index ? index : prevIndex;
            var end = prevIndex > index ? prevIndex : index;
            for (var i = start; i <= end; i++)
                if (trail[i] != null)
                {
                    lastNotNull = i;
                    trail[i].TimelineUpdate(t);
                }

            previousTime = t;
            if (!dead)
                head.localPosition = trail[lastNotNull].GetPos();
            if (trail[index] != null || indestructible)
            {
                if (dead)
                    OnAlive();
            }
            else
            {
                if (!dead)
                    OnDead();
            }
        }

        private void OnAlive()
        {
            dead = false;
            Scoreboard.SetAlive(indexScoreboard);
            head.gameObject.SetActive(true);
        }

        private void OnDead()
        {
            dead = true;
            EffectManager.DeathAnim(head.position, color);
            Scoreboard.SetDead(indexScoreboard);
            head.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            if (head)
                Object.Destroy(head.gameObject);
        }
    }
}