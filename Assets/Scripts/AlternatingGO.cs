using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternatingGO : MonoBehaviour
{

    public GameObject leftGO;
    public GameObject rightGO;

    public void SwitchSide(bool left)
    {
        leftGO.SetActive(left);
        rightGO.SetActive(!left);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Hero hero = collision.gameObject.GetComponent<Hero>();
        if (hero != null)
        {
            hero.Impact(collision);
        }
    }

}
