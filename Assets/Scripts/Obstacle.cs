﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
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
