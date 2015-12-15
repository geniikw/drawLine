using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ListAnimation : ParentAnimation {

    public List<ParentAnimation> list = new List<ParentAnimation>();
    public bool isAtOnce = false;
    
    protected override IEnumerator PlayAnimation()
    {
        foreach (var item in list)
        {
            if (item == null)
                continue;

            if (!isAtOnce)
            {
                yield return StartCoroutine(item.Play());
            }
            else
            {
                StartCoroutine(item.Play());
            }
        }
    }
 

}
