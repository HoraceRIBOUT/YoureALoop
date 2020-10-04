using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Hero hero = collision.GetComponent<Hero>();
        if (hero != null)
        {
            LDManager.instance.LoadNextLevel();

            this.GetComponent<Collider2D>().enabled = false;
            //Launch animation of death
            ParticleSystem.MainModule main1 = GetComponentsInChildren<ParticleSystem>()[0].main;
            ParticleSystem.MainModule main2 = GetComponentsInChildren<ParticleSystem>()[1].main;
            main1.startSpeed = 5;
            main2.startSpeed = 5;
            Invoke("Kill", 3.0f);

            SoundManager.instance.GoToNextLevel();
        }
    }
    
    public ParticleSystem.ShapeModule shape1;
    public ParticleSystem.ShapeModule shape2;
    public float dead;

    public void Start()
    { 
        shape1 = GetComponentsInChildren<ParticleSystem>()[0].shape;
        shape2 = GetComponentsInChildren<ParticleSystem>()[1].shape;
    }

    public void Update()
    {
        shape1.scale = Vector3.one + Vector3.one * 0.4f * (Mathf.Sin(Time.time/15) + 1f) * 0.5f + dead * Vector3.one;
        shape2.scale = Vector3.one + Vector3.one * 0.4f * (Mathf.Cos(Time.time/15) + 1f) * 0.5f + dead * Vector3.one;


        //verify distance from Hero for sound
    }

    public void Kill()
    {
        GetComponentsInChildren<ParticleSystem>()[0].Stop();
        GetComponentsInChildren<ParticleSystem>()[1].Stop();
    }
}
