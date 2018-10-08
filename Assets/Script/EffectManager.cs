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

    private static EffectManager instance;

    private void Start()
    { 
        instance = this;
    }
    
    public static void DeathAnim(Vector3 worldPos, Color col)
    {
        var effect = instance.cam.gameObject.AddComponent<RippleEffect>();
        for (int i = 0; i < instance.deathParticles.Length;++i)
        {
            var main = instance.deathParticles[i].main;
            main.startColor = col;
            instance.deathParticles[i].transform.position = worldPos;
            instance.deathParticles[i].Emit(instance.numberParticles[i]);
        }
        worldPos = instance.cam.WorldToScreenPoint(worldPos);
        print(worldPos);
        effect.reflectionColor = col;
        effect.Emit(new Vector3(worldPos.x/Screen.width, worldPos.y/Screen.height));
    }
}
