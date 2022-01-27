using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleSpellsManager : MonoBehaviour
{
    //classes an objs to get vars etc
    public GameObject gameControllerObj;
    private GameController gameController;
    private BattleSpellInput battleSpellInputManager;

    public Transform spellCastPoint;

    //set spells in inspector with ui components. using the spellinfo struct
    public List<SpellInfo> battleSpells;
    private BattleSpell primarySpell;

    //layers that the spell will interact with
    public static LayerMask EnemyLayers;

    void Start()
    {
        SpellBase.SpellCastPoint = spellCastPoint;

        primarySpell = battleSpells[1].script as BattleSpell;

        gameController = gameControllerObj.GetComponent<GameController>();
        EnemyLayers = gameController.EnemyLayers;
        battleSpellInputManager = gameController.SpellUIObj.GetComponent<BattleSpellInput>();

        AssignToInputManager();
    }

    private void AssignToInputManager()
    {
        //spell input manager will invoke these when nessesary
        battleSpellInputManager.QuickPrimaryFire += QuickPrimarySpell;
        battleSpellInputManager.SpellFire += Spell;

    }

    /// <summary>
    /// shoot a quick primary spell
    /// </summary>
    /// <param name="context"></param>
    private void QuickPrimarySpell(InputAction.CallbackContext context)
    {       
        primarySpell.QuickCast();
        Debug.Log("Primary attack");
    }

    /// <summary>
    /// shoot the selected spell 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="spell"></param>
    private void Spell(InputAction.CallbackContext context, UISkillLocation spell)
    {
        Debug.Log(spell);
    }


}

