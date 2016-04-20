using UnityEngine;
using System.Collections;
using MapUtility;

public class PowerUp : Tile {

    public PowerupCode code;

    public void Init(int i, int j, PowerupCode code)
    {
        base.Init(i, j);

        this.code = code;

    }

    // Update is called once per frame
    void Update()
    {
        if (isExploding) return;
    }
}
