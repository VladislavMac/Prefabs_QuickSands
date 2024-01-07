using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePalmObject : MonoBehaviour
{
    public string TypePalmObject;

    public int AmountWoodLog;
    public int AmountAxeHit;

    public Transform ParentWoodLog;

    public GameObject WoodLog;

    public bool IsPalmFell = false;
}
