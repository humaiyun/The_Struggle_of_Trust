﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowCoin : MonoBehaviour
{
     void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            
        Debug.Log("Went to here");
             FindObjectOfType<Currency>().UpdateBalance(3);
           // UnityEngine.Debug.Log("Health-- TRUE (Spikes)");
           Destroy(gameObject);
          
        }
    }
}