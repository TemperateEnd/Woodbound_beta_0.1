using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{

    [SerializeField] GameObject chargeUp;
    [SerializeField] GameObject slashEffect;
    PlayerMovement player;
    Animator animator;
    bool isChargingUp = false;
    [SerializeField] int direction = -1;
    float starttime;

    void Start()
    {
        chargeUp.SetActive(false);
        player = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        if (isChargingUp)
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                direction = 0; //right
            }
            if (Input.GetAxis("Horizontal") < 0)
            {
                direction = 1; //left
            }
            if (Input.GetAxis("Vertical") > 0)
            {
                direction = 2; //up
            }
            if (Input.GetAxis("Vertical") < 0)
            {
                direction = 3; //down
            }  
        }
        if(Input.GetButtonDown("Fire1"))
        {
            starttime = Time.time; 
        }
        if (Input.GetButton("Fire1"))
        {
            if (Time.time - starttime >= 1f)
            {
                isChargingUp = true;
                chargeUp.SetActive(true);
                player.currentState = PlayerState.cast;
                animator.SetBool("isChargingUp", true);
            }
        }
        if(Input.GetButtonUp("Fire1")&&isChargingUp)
        {
          
            //&&(Input.GetButton("Horizontal")||Input.GetButton("Vertical")
            isChargingUp = false;
            chargeUp.SetActive(false);
            animator.SetBool("isChargingUp", false);

            if (direction == -1)
            {
                player.currentState = PlayerState.walk;
                return;
            }

            StartCoroutine(SlashAttack());
            StartCoroutine(ProduceWind()); 
            StartCoroutine(SlashPunish());
        }
    }

    private IEnumerator SlashAttack()
    { 
        animator.SetTrigger("Slash");
        SoundManager.instance.PlaySound("playerAttack");
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator ProduceWind()
    {
        yield return new WaitForSeconds(0.2f);
        slashEffect.transform.GetChild(direction).gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        slashEffect.transform.GetChild(direction).gameObject.SetActive(false);
        direction = -1;
    }

    private IEnumerator SlashPunish()
    {
        yield return new WaitForSeconds(1f);
        player.currentState = PlayerState.walk;
    }
}
