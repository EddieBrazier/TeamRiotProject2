﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PlayerTool
{ 
    Lockpick,
    Loupe,
    Eyepiece,
    Unequipped
}


public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public PlayerTool currentTool = PlayerTool.Unequipped;

    //list of prefabs for minigames
    public List<GameObject> minigames;

    //list of maze prefabs
    public List<GameObject> mazes;

    //list of UI elements
    public List<GameObject> toolUI;

    //use to track the current minigame being played
    public GameObject currentMinigame;

    public bool tutorial;
    private bool finishedTut=false;

    private int clipIndex;
    public List<AudioClip> finishClips;

    List<PlayerTool> toolCount = new List<PlayerTool>();
    // Start is called before the first frame update
    void Start()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        tutorial = false;

    }

    // Update is called once per frame
    void Update()
    {
        //unequip current tool if the player right-clicks
        if(Input.GetKeyDown(KeyCode.Mouse1) && currentTool != PlayerTool.Unequipped)
        {
            UnequipTool();
        }

    }

    public void StartMinigame(PlayerTool tool)
    {
        //hide tools in UI
        ToggleUI(false);

        //deactivate active weapon
        LevelManager.instance.activeWeapon.GetComponent<SpriteRenderer>().enabled = false;
        LevelManager.instance.activeWeapon.GetComponent<BoxCollider2D>().enabled = false;

        //start new minigame
        GameObject newMinigame = null;
        
        switch (tool)
        {
            case PlayerTool.Lockpick:
                newMinigame = minigames[1];
                clipIndex = 1;
                toolCount.Add(PlayerTool.Lockpick);
                currentMinigame = Instantiate(newMinigame);
                break;
            case PlayerTool.Loupe:
                clipIndex = 0;
                //randomize which maze is spawned
                int randMaze = Random.Range(0, 3);
                switch(randMaze)
                {
                    case 0:
                        newMinigame = mazes[0];
                        break;
                    case 1:
                        newMinigame = mazes[1];
                        break;
                    case 2:
                        newMinigame = mazes[2];
                        break;
                }
                toolCount.Add(PlayerTool.Loupe);
                currentMinigame = Instantiate(newMinigame);
                break;
            case PlayerTool.Eyepiece:
                newMinigame = minigames[2];
                clipIndex = 2;
                toolCount.Add(PlayerTool.Eyepiece);
                currentMinigame = Instantiate(newMinigame);
                break;
        }

        

        //set current tool to unequipped
        UnequipTool();
    }

    public void EndMinigame()
    {
        //reactivate active weapon
        LevelManager.instance.activeWeapon.GetComponent<SpriteRenderer>().enabled = true;
        LevelManager.instance.activeWeapon.GetComponent<BoxCollider2D>().enabled = true;

        Debug.Log("ENDING MIN GAME");
        //(1, true) //(3, false) for normal run
        //tutorial = TutorialCheck(1, true); //true will complete the tutorial check regardless of the game
        if(tutorial == false)
        {
            tutorial = TutorialCheck(3, false);
        }
        //start moving weapon offscreen //When tutorial is done 
        if (tutorial == true&!finishedTut)
        {
            StartCoroutine(LevelManager.instance.MoveWeapon(LevelManager.instance.activeWeapon, LevelManager.instance.weaponLocList[2]));
            LevelManager.instance.weaponsCompleted++;
            LevelManager.instance.scoreUI.text = "Weapons Completed: " + LevelManager.instance.weaponsCompleted;
            finishedTut = true;
        }
        else //normal case 
        {
            if (LevelManager.instance.activeWeapon.GetComponent<Weapon>().enchantments.Count == 0)
            {
                StartCoroutine(LevelManager.instance.MoveWeapon(LevelManager.instance.activeWeapon, LevelManager.instance.weaponLocList[2]));
                LevelManager.instance.weaponsCompleted++;
                LevelManager.instance.scoreUI.text = "Weapons Completed: " + LevelManager.instance.weaponsCompleted;
            }
        }
        //increase weapons completed and close current minigame
        //TODO: support weapons with multiple enchantments
        PlayFinishClip();
        Destroy(currentMinigame);

        //show tools in UI
        ToggleUI(true);
    }

    private bool TutorialCheck(int _num, bool pass)
    {
        if ( pass == true && toolCount.Count == _num &&
                (toolCount.Contains(PlayerTool.Eyepiece) ||
                toolCount.Contains(PlayerTool.Lockpick) ||
                toolCount.Contains(PlayerTool.Loupe)))
        {
            return true;
        }

        if (tutorial == false && pass == false)
        {
            if(toolCount.Count == _num && 
                (toolCount.Contains(PlayerTool.Eyepiece) && 
                toolCount.Contains(PlayerTool.Lockpick) &&
                toolCount.Contains(PlayerTool.Loupe)))
            {
                return true;
            } 
        }

        return false;
    }

    public void UnequipTool()
    {
        currentTool = PlayerTool.Unequipped;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public void ToggleUI(bool toggle)
    {
        foreach(GameObject tool in toolUI)
        {
            tool.SetActive(toggle);
        }
    }

    public void PlayFinishClip()
    {
        GetComponent<AudioSource>().clip = finishClips[clipIndex];
        GetComponent<AudioSource>().Play();
    }
}
