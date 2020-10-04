using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selfDestructParticle : MonoBehaviour
{
    public ParticleSystem pS;
    // Start is called before the first frame update
    void Start()
    {
        if(pS == null)
            pS = GetComponent<ParticleSystem>();
        StartCoroutine(KillYourselfIn(pS.main.duration+ pS.main.startLifetime.constantMax));
    }

    IEnumerator KillYourselfIn(float timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(this.gameObject);
    }
}
