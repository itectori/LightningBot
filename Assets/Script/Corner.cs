using UnityEngine;

namespace Script
{
    public class Corner : MonoBehaviour
    {
        public Color Color;

        private void Start()
        {
            var mesh = GetComponent<MeshRenderer>();
            mesh.material = new Material(Shader.Find("Standard")) {color = Color};
        }
    }
}