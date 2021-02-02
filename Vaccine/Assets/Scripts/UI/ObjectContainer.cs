﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectContainer : MonoBehaviour
{
    public bool isFull;
    public GameManager gameManager;
    public Image backgroundImage;
    private Color32 cur = new Color32(0, 0, 0, 83);
    public Vector2 pos;
    public int x, y;

    IEnumerator Start()
    {
        gameManager = GameManager.instance;

        yield return new WaitForEndOfFrame();
        pos = gameObject.GetComponent<RectTransform>().anchoredPosition;
        x = (int) Mathf.Round((pos.x + 95) / 175);
        y = (int) Mathf.Round((pos.y - 40) / (-120));
    }

    //private void Awake()
    //{
        
        
    //}

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(gameManager.draggingObject != null && isFull == false)
        {
            gameManager.currentContainer = this.gameObject;
            backgroundImage.color = new Color32(56, 0, 0, 152);
            //backgroundImage.enabled = true;
        }   
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        gameManager.currentContainer = null;
        if(!isFull) backgroundImage.color = cur;

        //backgroundImage.enabled = false;
    }
}