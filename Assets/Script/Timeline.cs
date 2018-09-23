using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script
{
    public class Timeline : MonoBehaviour
    {
        [SerializeField] private List<ATimelineDependent> dependences;

        private Slider slider;
        private float lastValue;
        private bool automatic;

        private void Start()
        {
            lastValue = 0;
            automatic = true;
            slider = GetComponent<Slider>();
        }

        private void Update()
        {
            foreach (var d in dependences)
            {
                d.TimelineUpdate(slider.value);
            }


            automatic = automatic && Math.Abs(lastValue - slider.value) < 0.01f;
            if (!automatic)
                return;
            slider.value = Time.time / GameManager.TotalDuration * 20000;

            lastValue = slider.value;
        }
    }
}