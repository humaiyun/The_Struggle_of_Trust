﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{

  [SerializeField]
    private Animator anim;
    public float timer = 0.45f;
    public bool noDamage = false;
    public GameObject top;
    public GameObject middle;
    public GameObject bot;

    public string name;
 

    public bool ApplyVol=false;
    void Start()
    {
      //  top.SetActive(true);
      //  middle.SetActive(false);
        bot.SetActive(false);
    }

    void Update()
    {
        if (noDamage)
        {
            top.SetActive(false);
             middle.SetActive(false);

        }
    }

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
           
            anim.SetBool(name,true);
           
            if (anim.GetBool(name))
    {
            top.SetActive(false);
            middle.SetActive(true);
         ApplyVol=true;
    }
           
             StartCoroutine(Test());

        }


   
        
    }




        IEnumerator Test()
    {
        
        yield return new WaitForSeconds(timer);
     
     
    if (anim.GetBool(name))
    {
         middle.SetActive(false);
        bot.SetActive(true);
        
      noDamage = true;
    }
       anim.SetBool(name,false);


    }


}