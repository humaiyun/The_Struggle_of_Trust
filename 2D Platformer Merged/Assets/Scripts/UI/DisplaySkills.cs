﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sss : MonoBehaviour
{
     Ray ray;
    RaycastHit2D hit;
    private void OnTriggerEnter2D(Collider2D other) {
    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
     RaycastHit2D hit = Physics2D.GetRayIntersection(ray,Mathf.Infinity);
     if (hit.collider == other)
     {
         Debug.Log("We got em boys");
     }    

    }
}