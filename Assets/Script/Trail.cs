using UnityEngine;

namespace Script
{
    public class Trail : MonoBehaviour
    {
        public float Speed = 1;
        public Color Color;
        public GameObject CornerStart;

        private float size;
        private Material material;

        private void Start()
        {
            material = new Material(Shader.Find("Standard"));
            transform.localScale = new Vector3(0, 1, 1);
            GetComponentInChildren<MeshRenderer>().material = material;
            CornerStart.GetComponent<MeshRenderer>().material = material;
            material.color = Color;
            Instantiate(CornerStart, transform.position, Quaternion.identity, null);
        }


        private void Update()
        {
            size += Time.deltaTime * Speed;
            transform.localScale = new Vector3(size, 1, 1);
        }


        public void Stop()
        {
            enabled = false;
        }
    }
}