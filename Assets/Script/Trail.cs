using System;
using UnityEngine;

namespace Script
{
    public class Trail : ITimelineDepend
    {
        private readonly int x;
        private readonly int y;
        private readonly float startF;
        private float endF;
        private readonly Direction dir;
        private readonly Color color;
        private int size;

        public const int WIDTH = 6;

        public Trail(int x, int y, float startF, float endF, Direction dir, Color color)
        {
            this.x = x;
            this.y = y;
            this.startF = startF;
            this.endF = endF;
            this.dir = dir;
            this.color = color;
            size = 1;
        }

        public void SetEndF(float val)
        {
            endF = val;
        }

        public void IncrSize()
        {
            size++;
        }

        private float lastLenght;
        public void TimelineUpdate(float t, bool manual)
        {
            if (t >= endF)
            {
                DrawTrail(1, color);
                lastLenght = 1;
            }
            else if (t < startF)
            {
                DrawTrail(1, GameManager.Clear);
                lastLenght = 0;
            }
            else
            {
                DrawTrail(lastLenght, GameManager.Clear);
                lastLenght = (t - startF) / (endF - startF);
                DrawTrail(lastLenght, color);
            }
        }

        private void DrawTrail(float length, Color drawColor)
        {
            var sizePx = (int) (length * size * GameManager.Unit);
            switch (dir)
            {
                case Direction.Right:
                    GameManager.DrawRec(
                        GameManager.GridToImage(x) + WIDTH,
                        GameManager.GridToImage(y),
                        sizePx,
                        WIDTH,
                        drawColor);
                    break;
                case Direction.Down:
                    GameManager.DrawRec(
                        GameManager.GridToImage(x),
                        GameManager.GridToImage(y) - sizePx,
                        WIDTH,
                        sizePx,
                        drawColor);
                    break;
                case Direction.Left:
                    GameManager.DrawRec(
                        GameManager.GridToImage(x) - sizePx,
                        GameManager.GridToImage(y),
                        sizePx,
                        WIDTH,
                        drawColor);
                    break;
                case Direction.Up:
                    GameManager.DrawRec(
                        GameManager.GridToImage(x),
                        GameManager.GridToImage(y) + WIDTH,
                        WIDTH,
                        sizePx,
                        drawColor);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public Vector3 GetPos()
        {
            switch (dir)
            {
                case Direction.Right:
                    return new Vector3(GameManager.GridToWorld(x + lastLenght * size), 0 , GameManager.GridToWorld(y));
                case Direction.Down:
                    return new Vector3(GameManager.GridToWorld(x), 0 , GameManager.GridToWorld(y - lastLenght * size));
                case Direction.Left:
                    return new Vector3(GameManager.GridToWorld(x - lastLenght * size), 0 , GameManager.GridToWorld(y));
                case Direction.Up:
                    return new Vector3(GameManager.GridToWorld(x), 0 , GameManager.GridToWorld(y + lastLenght * size));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}