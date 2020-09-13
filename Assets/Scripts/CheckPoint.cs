using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {

    private Animator anim;

    void Start() {
        anim = gameObject.GetComponent<Animator>();
        if (gameObject.tag.Equals("Blue")) {
            anim.SetTrigger("transforme-blue");            

        } else if (gameObject.tag.Equals("Green")) {
            anim.SetTrigger("transforme-green");
            

        } else if (gameObject.tag.Equals("Orange")) {
            anim.SetTrigger("transforme-orange");

        } else if (gameObject.tag.Equals("Pink")) {
            anim.SetTrigger("transforme-pink");

        } else {
            anim.SetTrigger("transforme-blue");
            gameObject.tag = "Blue";
        }
    }
}
