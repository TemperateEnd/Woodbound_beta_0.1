using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombReaction : MonoBehaviour
{
    [SerializeField] GameObject bomb;
    public void Use()
    {
        PlayerMovement Player = FindObjectOfType<PlayerMovement>();
        Instantiate(bomb, Player.transform.Find("BombPlace").position, Quaternion.identity);
    }
    
}
