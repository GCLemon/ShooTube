using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    void Start()
    {
        //0.5秒後に消滅
        Destroy(gameObject, 0.5f);
    }
}
