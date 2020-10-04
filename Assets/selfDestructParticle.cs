using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selfDestructParticle : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem pS = GetComponent<ParticleSystem>();
        StartCoroutine(KillYourselfIn(pS.main.duration+ pS.main.startLifetime.constantMax));
    }

    IEnumerator KillYourselfIn(float timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(this.gameObject);
    }
}
