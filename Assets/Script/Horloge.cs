using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Horloge : MonoBehaviour {
    [SerializeField]
    private TMPro.TextMeshProUGUI minutes;
    [SerializeField]
    private TMPro.TextMeshProUGUI seconds;

    private void Update()
    {
        UpdateTime(Time.time * 1000);
    }

    void UpdateTime(float t)
    {
        TimeSpan ts = TimeSpan.FromMilliseconds(t);
        var m = string.Format("{0:00}", ts.Minutes);
        var s = string.Format("{0:00}:{1:00}", ts.Seconds, ts.Milliseconds / 10);
        minutes.text = m;
        seconds.text = s;
    }
}
