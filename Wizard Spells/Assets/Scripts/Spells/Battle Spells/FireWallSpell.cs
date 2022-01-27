using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWallSpell : BattleSpell
{
    [Header("Fire Ball Stats")]
    public GameObject FireBallObj;
    public GameObject FireObj;
    public float spellSpeed = 5f;
    public float spellDuration = 0.4f;

    //shoot fire ball that creates flames when lands, bit of an arc

    public override void QuickCast()
    {
        GameObject spellObj = Instantiate(FireBallObj, SpellCastPoint.position, SpellCastPoint.rotation);
        StartCoroutine(RemoveSpellObj(spellObj, spellDuration));
        spellObj.GetComponent<FireballObj1>().InitialiseFireBall(spellSpeed, spellDuration, BattleSpellsManager.EnemyLayers, OnFireHit);
    }

    public override void Cast()
    {


    }

    private void OnFireHit(Transform t, bool enemy)
    {
        Debug.Log($"Fire Hit {enemy}");

        if (enemy)
        {
            Instantiate(FireObj ,t.position, Quaternion.identity);
            //Set enemy on fire
        }
        else
        {
            Instantiate(FireObj, t.position, Quaternion.identity);

        }


    }

}
