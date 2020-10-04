using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sendTrigger : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        AlternatingGO alternating = GetComponentInParent<AlternatingGO>();

        alternating.OnCollisionEnter2D(collision);
    }
}
