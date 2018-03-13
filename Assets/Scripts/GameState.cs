using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    // State stuff
    public int Coins { get; set; }
    public int Health { get; set; }

    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        this.Coins = 0;
        this.Health = 3;
    }
}
