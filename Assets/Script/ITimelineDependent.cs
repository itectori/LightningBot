using UnityEngine;

namespace Script
{
	public abstract class TimelineDependent : MonoBehaviour
	{
		public abstract void TimelineUpdate(float t);
	}
}
