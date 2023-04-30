using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class AnimationRandomStart : MonoBehaviour
{
    Animation anim;
    void Start()
    {
        anim = GetComponent<Animation>();
        float waitTime = Random.Range(0f, 2f);
        StartCoroutine(StartAnimAfter(waitTime));
    }

    IEnumerator StartAnimAfter(float time)
    {
        yield return new WaitForSeconds(time);
        anim.Play();
    }
}
