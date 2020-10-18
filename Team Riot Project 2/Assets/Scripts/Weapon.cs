﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //weapon's enchantments
    public List<PlayerTool> enchantments;

    //sprite renderer
    private SpriteRenderer spriteRenderer;

    //effects type of weapon spawn
    bool tutorial;

    // Start is called before the first frame update
    void Start()
    {
        //save sprite renderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        //set tutorial value to player manager's tutorial value
        tutorial = PlayerManager.instance.tutorial;

        //give this weapon randomized enchantments
        int randEnchantment = Random.Range(0, 3);
        //tutorial checks. Adding each minigame to Tool list
        if(tutorial == false)
        {
            enchantments.Add(PlayerTool.Lockpick);
            enchantments.Add(PlayerTool.Loupe);
            enchantments.Add(PlayerTool.Eyepiece);
        }
        else //normal case
        {
            switch (randEnchantment)
            {
                case 0:
                    enchantments.Add(PlayerTool.Lockpick);
                    spriteRenderer.color = Color.red;
                    break;
                case 1:
                    enchantments.Add(PlayerTool.Loupe);
                    spriteRenderer.color = new Color32(124, 55, 189, 255);
                    break;
                case 2:
                    enchantments.Add(PlayerTool.Eyepiece);
                    spriteRenderer.color = Color.blue;
                    break;
            }
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {

        Debug.Log("Clicking on Weapon");
        //tutorial check
        if(tutorial == false)
        {
            if (PlayerManager.instance.currentMinigame == null && PlayerManager.instance.currentTool != PlayerTool.Unequipped)
            {
                //updating tutorial value from manager
                tutorial = PlayerManager.instance.tutorial;
                PlayerTool minigame = PlayerManager.instance.currentTool;
                PlayerManager.instance.StartMinigame(minigame);
                switch (minigame)
                {
                    case PlayerTool.Lockpick:
                        spriteRenderer.color = Color.red;
                        break;
                    case PlayerTool.Loupe:
                        spriteRenderer.color = new Color32(124, 55, 189, 255);
                        break;
                    case PlayerTool.Eyepiece:
                        spriteRenderer.color = Color.blue;
                        break;
                }
            }
        }
        else //normal case
        {
            //alternative
            if (enchantments.Contains(PlayerManager.instance.currentTool) && PlayerManager.instance.currentMinigame == null)
            {
                tutorial = PlayerManager.instance.tutorial;
                PlayerManager.instance.StartMinigame(PlayerManager.instance.currentTool);
            }
            /*if (PlayerManager.instance.currentTool == enchantments[0] && PlayerManager.instance.currentMinigame == null)
            {
                tutorial = PlayerManager.instance.tutorial;
                PlayerManager.instance.StartMinigame(PlayerManager.instance.currentTool);
            }*/
        }

        
    }
}
