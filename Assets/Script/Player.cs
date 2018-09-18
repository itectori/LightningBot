using System;
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

    public class Player : MonoBehaviour
    {
        [SerializeField] private bool control;

        public Color Color;
        public float Speed;
        public Trail TrailPrefab;
        public Direction InitialDirection;

        private Trail trail;
        private Direction direction;


        private void Start()
        {
            direction = InitialDirection;
            TrailPrefab.Color = Color;
            TrailPrefab.Speed = Speed;
            trail = Instantiate(TrailPrefab, transform.position, Quaternion.Euler(0, 90 * (int) direction, 0));
        }


        private void Update()
        {
            var delta = Speed * Time.deltaTime;

            if (control)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                    Turn(Direction.Up);
                else if (Input.GetKeyDown(KeyCode.D))
                    Turn(Direction.Right);
                else if (Input.GetKeyDown(KeyCode.S))
                    Turn(Direction.Down);
                else if (Input.GetKeyDown(KeyCode.Q))
                    Turn(Direction.Left);
            }

            switch (direction)
            {
                case Direction.Up:
                    transform.position += Vector3.forward * delta;
                    break;
                case Direction.Down:
                    transform.position += Vector3.back * delta;
                    break;
                case Direction.Left:
                    transform.position += Vector3.left * delta;
                    break;
                case Direction.Right:
                    transform.position += Vector3.right * delta;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        public void Turn(Direction dir)
        {
            if (dir == direction)
                return;

            TrailPrefab.Color = Color;
            TrailPrefab.Speed = Speed;
            direction = dir;
            if (trail)
                trail.Stop();
            transform.position += new Vector3(
                transform.position.x > 0 ? 0.5f : -0.5f, 0,
                transform.position.z > 0 ? 0.5f : -0.5f);
            transform.position = new Vector3((int) transform.position.x, 0, (int) transform.position.z);
            trail = Instantiate(TrailPrefab, transform.position, Quaternion.Euler(0, 90 * (int) dir, 0));
        }


        private void Stop()
        {
            trail.Stop();
            enabled = false;
        }
    }
}