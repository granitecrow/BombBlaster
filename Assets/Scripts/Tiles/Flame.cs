﻿using UnityEngine;
using System.Collections;

public class Flame : Tile {


    public void Init(int i, int j, GameObject animationPrefab)
    {
        base.Init(i, j);
    }

    public void Update()
    {
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            Remove();
    }

    public override void Explode()
    {
        // do nothing
    }

    IEnumerator PlayExplodeAdnim(float delayTime)
    {
        GetComponent<Animator>().SetBool("die", true);
        yield return new WaitForSeconds(delayTime);
        Destroy(gameObject);
    }
}
