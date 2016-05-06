using UnityEngine;
using System.Collections;

public class Flame : Tile {


    public void Init(int i, int j, GameObject animationPrefab)
    {
        base.Init(i, j);
    }

    public void Update()
    {
        StartCoroutine(KillOnAnimationEnd());
    }

    public override void Explode()
    {
        // do nothing
    }

    private IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(0.6f);
        Remove();
    }
}
