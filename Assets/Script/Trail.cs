using UnityEngine;

namespace Script
{
    public class Trail : MonoBehaviour
    {
        [SerializeField] private Material standardMat;
        
        public float Speed = 1;
        public Color Color;
        public GameObject CornerStart;

        private float size;
        private Material material;

        private void Start()
        {
            material = new Material(standardMat);
            transform.localScale = new Vector3(0, 1, 1);
            transform.GetChild(0).GetComponent<MeshRenderer>().material = material;
            transform.GetChild(1).GetComponent<MeshRenderer>().material.color = Color;
            CornerStart.transform.GetChild(0).GetComponent<MeshRenderer>().material = material;
            material.color = Color;
            Instantiate(CornerStart, transform.position, Quaternion.identity, null)
                .transform.GetChild(1).GetComponent<MeshRenderer>().material.color = Color;
        }


        private void Update()
        {
            size += Time.deltaTime * Speed;
            transform.localScale = new Vector3(size, 1, 1);
        }


        public void Stop()
        {
            transform.localScale = new Vector3((int)(transform.localScale.x + 0.5f), 1, 1);
            enabled = false;
        }
    }
}