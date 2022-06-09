using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("初期X位置(-5/5)")]
    public int initPos_x = 0; //初期x位置
    [Header("初期Z位置(-5/5)")]
    public int initPos_z = 0; //初期z位置

    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = new Vector3(initPos_x, 1.0f, initPos_z);
        transform.position = pos;
    }
}
