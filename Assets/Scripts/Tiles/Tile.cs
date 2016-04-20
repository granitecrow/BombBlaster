﻿using UnityEngine;
using System.Collections;
using MapUtility;

public class Tile : MonoBehaviour {

    public int i;
    public int j;
    public bool isWalkable;
    public bool isExplodable;
    public bool isEmpty;
    
    public GameObject destroyPrefab;    // a prefab to instantiate when tile is destroyed
    protected Board board;
    protected bool isExploding = false;
    protected Animator animator;

    // Use this for initialization
    void Awake()
    {
        animator = this.gameObject.GetComponent<Animator>();
    }


    // basic constructor
    public Tile(int i = 0, int j = 0, bool isEmpty = false, bool isExplodable = false, bool isWalkable = true)
    {
        this.i = i;
        this.j = j;
        this.isEmpty = isEmpty;
        this.isExplodable = isExplodable;
        this.isWalkable = isWalkable;
    }


    public virtual void Init(int i, int j)
    {
        this.i = i;
        this.j = j;
        board = Board.instance;
    }

    public virtual void Explode()
    {
        if (isExploding) return;
        isExploding = true;
        gameObject.SetActive(false);

        board.AddTile(i, j, destroyPrefab).Init(i, j);
        //Instantiate(destroyPrefab, transform.position, Quaternion.identity);
        Remove();
    }

    public virtual void Remove()
    {
        board.SetTileToEmpty(this);
        Destroy(gameObject); 
    }

}