using UnityEngine;
using System.Collections;

public class csJumperAnim : MonoBehaviour
{
    public Animator Jumper_Anim;

    public bool Basic_Jump = false;
    public bool Big_Jump_Ready_Anim = false;
    public bool Big_Jump_Anim = false;
    public bool Run_Anim = false;

    void Start()
    {
        Jumper_Anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(Run_Anim == true)
        {
            Jumper_Anim.SetBool("Basic_Jump", false);
            Jumper_Anim.SetBool("Big_Jump_Anim", false);
            Jumper_Anim.SetBool("Big_Jump_Ready_Anim", false);
        }

        if (Basic_Jump == true)
        {
            Jumper_Anim.SetBool("Basic_Jump", true);
        }

        if (Big_Jump_Ready_Anim == true)
        {
            Jumper_Anim.SetBool("Big_Jump_Ready_Anim", true);
        }

        if (Big_Jump_Anim == true)
        {
            Jumper_Anim.SetBool("Big_Jump_Ready_Anim", false);
            Jumper_Anim.SetBool("Big_Jump_Anim", true);
        }
    }
}
