using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

namespace Script
{
    public class LightFlicker : MonoBehaviour
    {
        private Light headLight;

        private void Start()
        {
            headLight = GetComponentInChildren<Light>();
            StartCoroutine(Flicker());
        }

        public void SetColor(Color color)
        {
            headLight = GetComponentInChildren<Light>();
            headLight.color = color;
        }

        private IEnumerator Flicker()
        {
            while (true)
            {
                headLight.intensity = Random.Range(2f, 3f);
                headLight.range = Random.Range(1f, 2f);
                yield return new WaitForSeconds(Random.Range(0.01f, 0.1f));
            }

            // ReSharper disable once IteratorNeverReturns
        }
    }
}