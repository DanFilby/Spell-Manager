using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarryPotterSpells : MonoBehaviour
{
    [Header("Object Refrences")]
    public GameController gameController;
    private EffectMeshCreator meshCreator;
    private EnemyLockOn targetLockOn;

    [Header("Beam settings")]
    public MeshSettings beamSettings;
    public Material beamMaterial;
    private GameObject effectObj;
    private MeshFilter effectMeshFilter;

    public Transform startPoint;
    public float beamRadius = 0.05f;

    [Header("Debug")]
    public GameObject debugobj;

    void Start()
    {
        targetLockOn = gameController.enemyLockOn;
        meshCreator = new EffectMeshCreator(beamSettings);

        ResetEffectObj(ref effectObj);

    }

    public void ShootSpellInput()
    {
        StartCoroutine(ShootSpell());
    }

    private IEnumerator ShootSpell()
    {
        ILockOn target;
        if (targetLockOn.GetLockedOnTarget(out target))
        {
            effectMeshFilter.mesh = meshCreator.SimpleBeam(startPoint.position, target.GetTransform().position, beamRadius);

            yield return new WaitForSeconds(1f);
            ResetEffectObj(ref effectObj);
        }
    }


    private void ResetEffectObj(ref GameObject obj)
    {
        Destroy(obj);
        obj = new GameObject("Beam Effect");
        MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = beamMaterial;
        effectMeshFilter = obj.AddComponent<MeshFilter>();
    }


}