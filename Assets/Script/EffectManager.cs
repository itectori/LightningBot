using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour {
    [SerializeField]
    Camera cam;
    [SerializeField]
    ParticleSystem[] deathParticles;
    [SerializeField]
    int[] numberParticles = { 1, 1, 5 };
    // Use this for initialization
    IEnumerator Start()
    {
        var cols = Script.ColorMaker.DivideColors(10);
        while (true)
        {
            yield return new WaitForSeconds(1);
            DeathAnim(new Vector3(5,0,5), cols[Random.Range(0,9)]);
            DeathAnim(new Vector3(15, 0, 10), cols[Random.Range(0, 9)]);
        }
    }

	
	// Update is called once per frame
	void Update () {
		
	}
    public void DeathAnim(Vector3 worldPos, Color col)
    {
        var effect = cam.gameObject.AddComponent<RippleEffect>();
        for (int i = 0; i < deathParticles.Length;++i)
        {
            var main = deathParticles[i].main;
            main.startColor = col;
            deathParticles[i].transform.position = worldPos;
            deathParticles[i].Emit(numberParticles[i]);
        }
        worldPos = cam.WorldToScreenPoint(worldPos);
        print(worldPos);
        effect.reflectionColor = col;
        effect.Emit(new Vector3(worldPos.x/Screen.width, worldPos.y/Screen.height));
    }
}
