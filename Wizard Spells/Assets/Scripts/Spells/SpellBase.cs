using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBase : MonoBehaviour
{
    [Header("Base Stats")]
    public bool canQuickCast = false;
    public int spellPower = 1;
    public float castTime;

    public virtual void QuickCast() { }
    public virtual void Cast() { }

    public static Transform SpellCastPoint;

    /// <summary>
    /// Deletes an object after a delay. also checks if its still there
    /// </summary>
    public IEnumerator RemoveSpellObj(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        if(obj != null)
        {
            Destroy(obj);
        }
    }

}
/// <summary>
/// Holds the spell script itself and the UI components that go into the pullup spell menu
/// </summary>
[System.Serializable]
public struct SpellInfo
{
    public SpellBase script;
    public GameObject UIObj;
    //TODO: implement this

    public bool activated;
}
