using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("����X�ʒu(-5/5)")]
    public int initPos_x = 0; //����x�ʒu
    [Header("����Z�ʒu(-5/5)")]
    public int initPos_z = 0; //����z�ʒu

    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = new Vector3(initPos_x, 1.0f, initPos_z);
        transform.position = pos;
    }
}
