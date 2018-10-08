using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour {
    [SerializeField]
    Camera cam;
    [SerializeField]
    RippleEffect ripple;
    [SerializeField]
    ParticleSystem[] deathParticles;
    [SerializeField]
    int[] numberParticles = { 1, 1, 5 };
    // Use this for initialization

	
	// Update is called once per frame
	void Update () {
		
	}
    public void DeathAnim(Vector3 worldPos, Color col)
    {
        
        for (int i = 0; i < deathParticles.Length;++i)
        {
            var main = deathParticles[i].main;
            main.startColor = col;
            deathParticles[i].transform.position = worldPos;
            deathParticles[i].Emit(numberParticles[i]);
        }
        worldPos = cam.WorldToScreenPoint(worldPos);
        print(worldPos);
        ripple.reflectionColor = col;
        ripple.Emit(new Vector3(worldPos.x/Screen.width, worldPos.y/Screen.height));
    }
}
