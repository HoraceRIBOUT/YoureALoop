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

    public void OnTriggerEnter2D(Collider2D collision)
    {

        Hero hero = collision.GetComponent<Hero>();
        if (hero != null)
        {
            hero.bounceBack = !hero.bounceBack;
            hero.leftDirection = !hero.leftDirection;
        }
    }

}
