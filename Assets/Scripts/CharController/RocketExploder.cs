using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketExploder : MonoBehaviour
{


    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.contacts[0].ToString());
    }
}
