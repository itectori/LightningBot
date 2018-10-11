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

    [SerializeField]
    Shader ripple;

    private static EffectManager instance;

    static List<RippleEffect> shockwaveList;

    void Start()
    {
        shockwaveList = new List<RippleEffect>();
        instance = this;

    }


    static RippleEffect getARipple()
    {
        foreach (var e in shockwaveList)
        {
            if (e.available)
                return e;
        }
        var effect = instance.cam.gameObject.AddComponent<RippleEffect>();
        shockwaveList.Add(effect);
        effect.shader = instance.ripple;
        effect.ManualAwake();
        effect.StartChrono();
        return effect;
    }


    public static void DeathAnim(Vector3 worldPos, Color col)
    {
        var effect = getARipple();
        for (int i = 0; i < instance.deathParticles.Length;++i)
        {
            var main = instance.deathParticles[i].main;
            main.startColor = col;
            instance.deathParticles[i].transform.position = worldPos;
            instance.deathParticles[i].Emit(instance.numberParticles[i]);
        }
        worldPos = instance.cam.WorldToScreenPoint(worldPos);
        effect.reflectionColor = col;
        effect.shader = instance.ripple;
        effect.UpdateShaderParameters();
        effect.Emit(new Vector3(worldPos.x/Screen.width, worldPos.y/Screen.height));
    }
}
