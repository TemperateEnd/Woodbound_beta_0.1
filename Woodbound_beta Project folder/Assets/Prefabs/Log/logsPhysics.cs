using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logsPhysics : MonoBehaviour
{

    public Sprite wetLog;
    public Sprite Log;
    public Sprite threequarter;
    public Sprite half;
    public Sprite onequarter;
    public Sprite inWater;
    public double health;
    public double healthFull;
    public bool isWet=false;
    // Start is called before the first frame update
    void Start()
    {
        health = healthFull;
    }

    // Update is called once per frame
    void Update()
    {
        //This is when the log is hit and junk
         if(health / healthFull >= 75&& isWet==false)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = Log;
        }
        else if (health / healthFull >= 50 && health/healthFull <= 75 &&isWet==false )
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = threequarter;
        }
        else if (health / healthFull >= 25 && health / healthFull <= 50 && isWet==false)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = half;
        }
        else if (health / healthFull >= 0 && health / healthFull <= 25 && isWet==false)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = onequarter;
        }
         else if (isWet == true)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = inWater;
        }
          
    }

    public void Damage()
    {
        if (health > 0) {
            health -= 25;
        }
        
    }
    public void Water()
    {
        isWet = !isWet;
    }

    
}
