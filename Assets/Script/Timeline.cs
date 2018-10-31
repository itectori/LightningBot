using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    public class Timeline : MonoBehaviour
    {
        [SerializeField] private List<ATimelineDependent> dependences;

        private Slider slider;

        private Animation anim;
        private const float TIME_TO_WAIT = 1.5f;
        private float lastTime;
        private float lastValue;
        private bool active = true;
        private bool changing = false;
        private float multiplier = 1f;

        private static Timeline instance;

        private void Start()
        {
            instance = this;
            slider = GetComponent<Slider>();
            anim = GetComponent<Animation>();
            slider.value = 1f;
        }


        private Vector3 mousePos;

        private void Update()
        {
            var pos = Input.mousePosition;
            if (pos != mousePos)
            {
                mousePos = pos;
                lastTime = Time.time;
                Activate();
            }
            else
            {
                if (Time.time - lastTime > TIME_TO_WAIT)
                {
                    Hide();
                }
            }

            if (!GameManager.Ready)
                return;

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (lastValue != slider.value)
                changing = true;
            if (!changing)
                slider.value += multiplier * Time.deltaTime / Math.Min(GameManager.TotalDuration / 7000f, 15f);
            else if (Input.GetMouseButtonUp(0))
                changing = false;
            foreach (var d in dependences)
                d.TimelineUpdate(slider.value, changing);
            lastValue = slider.value;
        }

        public void ClickPlus()
        {
            if (multiplier > 3)
                return;
            multiplier *= 2;
        }

        public void ClickMinus()
        {
            if (multiplier < 0.05f)
                return;
            multiplier /= 2;
        }

        public static void ResetTime()
        {
            instance.slider.interactable = true;
            instance.slider.value = 0;
            instance.lastValue = 0;
        }

        private void Activate()
        {
            if (active)
                return;
            active = true;
            anim.Play("Appear");
        }

        private void Hide()
        {
            if (!active)
                return;
            active = false;
            anim.Play("Hide");
        }
    }
}