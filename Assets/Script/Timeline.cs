using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using UnityEngine;
using UnityEngine.EventSystems;
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
        private bool active = true;
        
        private void Start()
        {
            slider = GetComponent<Slider>();
            anim = GetComponent<Animation>();
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
            
            slider.value += Time.deltaTime / GameManager.TotalDuration * 20000;

            foreach (var d in dependences)
                d.TimelineUpdate(slider.value);
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