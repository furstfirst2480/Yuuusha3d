using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    private Animator anim;
    private void Awake()
    
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            anim.SetTrigger("Attack");
            ThePlayer PlayerScript = GetComponent<ThePlayer>();
            PlayerScript.playerSpeed =0;
        }
    }
}



