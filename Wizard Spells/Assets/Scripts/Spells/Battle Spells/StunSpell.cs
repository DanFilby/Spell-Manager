using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunSpell : BattleSpell
{
    [Header("Stun Stats")]
    public GameObject stunObject;
    public float stunDuration = 0.2f;
    public float spellDuration = 0.4f;
    public float stunSpeed = 1f;

    public override void QuickCast()
    {      
        GameObject spellObj =  Instantiate(stunObject, SpellCastPoint.position, SpellCastPoint.rotation);
        StartCoroutine(RemoveSpellObj(spellObj, spellDuration));
        spellObj.GetComponent<StunObj>().InitialiseStuner(stunSpeed, BattleSpellsManager.EnemyLayers , OnEnemyHit);
    }

    public override void Cast()
    {

    }

    public void OnEnemyHit(Transform t)
    {
        Debug.Log("Stun this twat");

    }

}
