using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script
{
    public class LightFlicker : MonoBehaviour {
        private Light light;
        // Use this for initialization
        void Start() {
            light = GetComponentInChildren<Light>();
            light.color = GetComponentInParent<Player>().Color;
            StartCoroutine(Flicker());
    }

        // Update is called once per frame
        void Update() {

        }
        IEnumerator Flicker()
        {
            while(true)
            {
                light.intensity = Random.Range(2f, 3f);
                light.range = Random.Range(1f, 2f);
                yield return new WaitForSeconds(Random.Range(0.01f, 0.1f));
            }
        }
    }
}
