﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OldLadyTrigger : MonoBehaviour
{
    public GameObject canvasObject;
    private bool firstSentence = false;

    // Start is called before the first frame update
    void Start()
    {
        canvasObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
         if (Input.GetKeyDown(KeyCode.E) && firstSentence)
          {
             FindObjectOfType<DialogueManager>().DisplayNextSentence();
              if (FindObjectOfType<DialogueManager>().sentences.Count != 0){
                  if (canvasObject.activeSelf)
              FindObjectOfType<AudioManager>().Play("Render_Text"); // 06 June 2020
              }
            
             if (FindObjectOfType<DialogueManager>().sentences.Count == 0)
             {
                  canvasObject.SetActive(false);
                  FindObjectOfType<PauseMenu>().canPauseGame = true;
             }
          }

         
              
    }

  private void OnTriggerEnter2D(Collider2D collision) 
    {
      /*
         if (collision.CompareTag("Player"))
        {
                canvasObject.SetActive(true);
                FindObjectOfType<DialogueTrigger>().TriggerDialogue();
                 
        }
  */
    }

  
       private void OnTriggerStay2D(Collider2D collision) 
    {

        if (Input.GetKey(KeyCode.E) && !firstSentence)
        {
                FindObjectOfType<PauseMenu>().canPauseGame = false;
                canvasObject.SetActive(true);
                 firstSentence = true;
                FindObjectOfType<DialogueTrigger>().TriggerDialogue();
               
                 
        }

      

      
    }


     private void OnTriggerExit2D( Collider2D collision ) {
       canvasObject.SetActive(false);
       firstSentence = false;
       FindObjectOfType<PauseMenu>().canPauseGame = true;          
    }



}
