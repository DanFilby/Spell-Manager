using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLockOn : MonoBehaviour
{
    [Header("Object References")]
    public Camera cam;
    public LayerMask targetLayer;

    [Header("Lock on settings")]
    public bool lockOntoTargets = true;
    [Range(0, 100), Tooltip("Lock on goes from centre, so this will control how big the radius will be")]
    public int lockOnRadius = 20;
    public float lockOnRange = 5f;
    public float lockOnRefreshRate = 0.1f;
    [Range(0.01f, 0.1f)] public float rayFrequency = 0.05f;

    private Dictionary<int, float> targetsHitIDs;
    private SortedList<float, ILockOn> targetsSorted;
    public ILockOn currentLockOn;
    private ILockOn prevTraget;
    private (int, int) targetIds;   //stores current target id and previous target id in that order
    private WaitForSeconds delay;

    private void Start()
    {
        targetsHitIDs = new Dictionary<int, float>();
        targetsSorted = new SortedList<float, ILockOn>();
        delay = new WaitForSeconds(lockOnRefreshRate);
        StartCoroutine(LockOn());
    }

    public bool GetLockedOnTarget(out ILockOn target)
    {
        target = null;
        if (currentLockOn == null) return false;

        targetIds = (0, 0);
        currentLockOn.LockedOff();
        target = currentLockOn;
        currentLockOn = null;
        prevTraget = null;
        return true;
    }

    //using co routine to allow for a variable time interval. this uses all the functions 
    //shoot rays to get the list of targets then finds the closest to the centre with another
    //func and check enemy will control what to do to locked on targets
    IEnumerator LockOn()
    {
        while (lockOntoTargets)
        {
            ShootRays();
            currentLockOn = FindClosestTarget(targetsSorted);

            //if (currentLockOn != null) Debug.Log(currentLockOn.GetUniqueID());
            CheckForEnemy();

            yield return delay;
        }

    }

    /// <summary>
    /// checks if the current target needs to call locked on, or the prev needs to be locked off
    /// does this by checking the ids of the current and prev.
    /// </summary>
    private void CheckForEnemy()
    {
        if (targetIds.Item1 != targetIds.Item2 && targetIds.Item1 + targetIds.Item2 != 0)
        {
            if (prevTraget != null)
            {
                prevTraget.LockedOff();
            }
            if (currentLockOn != null)
            {
                currentLockOn.LockedOn();
            }

            prevTraget = currentLockOn;
            targetIds.Item2 = targetIds.Item1;
        }

    }

    /// <summary>
    /// Shoots ray from the cam, pointing towards the centre of the screen
    /// they go in a rectangle coveraging a given percentage of the middle of the 
    /// screen. using a dict to store id and dist, and a sorted list to store the 
    /// closest dist for that id
    /// </summary>
    private void ShootRays()
    {
        targetsHitIDs.Clear();
        targetsSorted.Clear();

        float radius = (lockOnRadius / 100.0f) / 2.0f;

        float x = 0.5f - radius;
        while (x <= 0.5f + radius)
        {
            float y = 0.5f - radius;
            while (y <= 0.5f + radius)
            {
                Ray ray = cam.ViewportPointToRay(new Vector3(x, y, 0));
                if (Physics.Raycast(ray, out RaycastHit hit, lockOnRange, targetLayer))
                {
                    ILockOn targetScr = hit.transform.GetComponent<ILockOn>();
                    float distFromCntr = Mathf.Sqrt(Mathf.Pow(0.5f - Mathf.Abs(x), 2) + Mathf.Pow(0.5f - Mathf.Abs(y), 2));
                    distFromCntr = (float)Mathf.Round(distFromCntr * 100f) / 100f;  //rounds for edge cases

                    //if it hasn't been hit before and the distance isnt the same as one already in the sorted list
                    if (!targetsHitIDs.ContainsKey(targetScr.GetUniqueID()))
                    {
                        //HACK: if two targets have the same dist from centre they wont both be added
                        if (!targetsSorted.ContainsKey(distFromCntr))
                        {
                            //adds to the sorted and also to targets hit
                            targetsSorted.Add(distFromCntr, targetScr);
                            targetsHitIDs.Add(targetScr.GetUniqueID(), distFromCntr);
                        }
                    }   //if already been hit and the distance is less. removes the current dist for that target and re adds it, also updating the other list
                    else if (targetsHitIDs[targetScr.GetUniqueID()] - distFromCntr > 0.1)
                    {
                        float currentDist = targetsHitIDs[targetScr.GetUniqueID()];
                        targetsSorted.Remove(currentDist);
                        //HACK: checks if another enemy is the same dist 
                        if (!targetsSorted.ContainsKey(distFromCntr))
                        {
                            targetsSorted.Add(distFromCntr, targetScr);
                            targetsHitIDs[targetScr.GetUniqueID()] = distFromCntr;
                        }
                    }
                }
                y += rayFrequency;
            }
            x += rayFrequency;
        }
    }
    /// <summary>
    /// finds the target closests to the centre of screen 
    /// </summry>
    /// <param name="targets"> copy of targets sorted by dist to centre </param>
    /// <returns> the target closest to centre or null </returns>
    private ILockOn FindClosestTarget(SortedList<float, ILockOn> targets)
    {
        //if no targets in range set the current target id to 0, so to potentially call the locked off func
        //off the prev target. then return null
        if (targets.Count == 0)
        {
            targetIds.Item1 = 0;
            return null;
        }

        //find the closest as already sorted, then update the current target id so to call locked on
        ILockOn target = targets.Values[0];
        targetIds.Item1 = target.GetUniqueID();

        return target;
    }
}


public interface ILockOn
{
    void LockedOn();

    void LockedOff();

    int GetUniqueID();

    Transform GetTransform();
}
