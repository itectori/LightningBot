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
    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            DeathAnim();
        }
    }

	
	// Update is called once per frame
	void Update () {
		
	}
    public void DeathAnim()
    {
        
        for(int i = 0; i < deathParticles.Length;++i)
        {

            deathParticles[i].Emit(numberParticles[i]);
        }
        ripple.Emit(new Vector2(0.5f, 0.5f));
    }
}
