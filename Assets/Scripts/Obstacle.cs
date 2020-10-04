using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        Hero hero = collision.gameObject.GetComponent<Hero>();
        if (hero != null)
        {
            hero.Impact(collision);
        }
    }
}
