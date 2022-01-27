using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTarget : MonoBehaviour, ILockOn
{
    static int TargetCount;
    private int uniqueId;

    private void Start()
    {
        uniqueId = TargetCount++;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public int GetUniqueID()
    {
        return uniqueId;
    }

    public void LockedOff()
    {
    }

    public void LockedOn()
    {
    }
}
