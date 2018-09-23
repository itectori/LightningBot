using System;
using UnityEngine;

namespace Script
{
    public class Trail : ATimelineDependent
    {
        [SerializeField] private Material standardMat;
        [SerializeField] private GameObject cornerStart;

        public Color Color;
        //public float CornerAngle;

        public float StartF;
        public float EndF;
        public float Size;

        private Material material;
        private GameObject cornerInstance;

        private void Start()
        {
            material = new Material(standardMat);
            transform.localScale = new Vector3(0, 1, 1);
            transform.GetChild(0).GetComponent<MeshRenderer>().material = material;
            //transform.GetChild(1).GetComponent<MeshRenderer>().material.color = Color;
            cornerStart.transform.GetChild(0).GetComponent<MeshRenderer>().material = material;
            material.color = Color;
            cornerInstance = Instantiate(cornerStart, transform.position, Quaternion.identity, transform.parent);
            //cornerInstance.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = Color;
            cornerInstance.SetActive(false);
            gameObject.SetActive(false);
        }

        public override void TimelineUpdate(float t)
        {
            gameObject.SetActive(true);
            if (t < StartF)
            {
                cornerInstance.SetActive(false);
                gameObject.SetActive(false);
            }
            else if (t > EndF)
            {
                cornerInstance.SetActive(true);
                gameObject.SetActive(true);
                transform.localScale = new Vector3(Size, 1, 1);
            }
            else
            {
                cornerInstance.SetActive(true);
                gameObject.SetActive(true);
                transform.localScale = new Vector3(Size * (t - StartF) / (EndF - StartF), 1, 1);
            }
        }

        public Vector3 GetPos()
        {
            var dir = (Direction) (transform.localEulerAngles.y / 90);
            switch (dir)
            {
                case Direction.Right:
                    return transform.position + Vector3.right * transform.localScale.x;
                case Direction.Down:
                    return transform.position + Vector3.back * transform.localScale.x;
                case Direction.Left:
                    return transform.position + Vector3.left * transform.localScale.x;
                case Direction.Up:
                    return transform.position + Vector3.forward * transform.localScale.x;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}