using UnityEngine;

namespace Script
{
    public abstract class ATimelineDependent : MonoBehaviour
    {
        public abstract void TimelineUpdate(float t, bool manual);
    }

    public interface ITimelineDepend
    {
        void TimelineUpdate(float t, bool manual);
    }
}