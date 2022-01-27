using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class BattleSpellInput : MonoBehaviour
{
    public GameObject gameControler;
    private MouseTracker mouseTracker;

    private Canvas canvas;

    //holds input details
    PlayerInput playerInput;

    //the action for toggling between ui and game 
    InputAction battleSpellUITog;

    //action for firing a quick spell
    InputAction quickPrimaryFire;

    //used to tell the spell manager script when and what to fire. they assign to here
    public UnityAction<InputAction.CallbackContext> QuickPrimaryFire;
    public UnityAction<InputAction.CallbackContext, UISkillLocation> SpellFire;

    //as bring up the menu and firing a quick spell has the same binding this is used to stop
    //firing a quick spell after closing the menu
    private bool spellMenu = false;

    void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
        canvas.enabled = false;
        InputSetup();
        mouseTracker = GetComponent<MouseTracker>();
    }

    private void InputSetup()
    {
        //find input maps. then the action battle spell UI toggle
        playerInput = gameControler.GetComponent<GameController>().GameInput;
        battleSpellUITog = playerInput.actions.FindAction("BattleSpellUIToggle", true);
        quickPrimaryFire = playerInput.actions.FindAction("BattleSpellPrimary", true);

        //as switching between input maps this has to always be active so is on a seperate map and enabled here
        battleSpellUITog.Enable();

        //its holding input so when been held its on then when let go its off
        battleSpellUITog.performed += BattleSpellUIOn;
        battleSpellUITog.canceled += BattleSpellUIOff;

        //the binding for when a quick spell is performed, it also needs to be enabled all the time
        quickPrimaryFire.performed += QuickPrimarySpellTrigger;
        quickPrimaryFire.Enable();
    }
    private void MouseLock(bool mouseLock)
    {
        Cursor.lockState = mouseLock ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !mouseLock;
    }

    /// <summary>
    /// brings up spell menu. and changes to UI input
    /// </summary>
    /// <param name="context"></param>
    private void BattleSpellUIOn(InputAction.CallbackContext context)
    {
        canvas.enabled = true;
        playerInput.SwitchCurrentActionMap("UI");
        spellMenu = true;
        MouseLock(false);
    }

    /// <summary>
    /// takes the UI menu down and then calls the function to determine if we need to shoot a spell
    /// </summary>
    /// <param name="context"></param>
    private void BattleSpellUIOff(InputAction.CallbackContext context)
    {
        canvas.enabled = false;
        playerInput.SwitchCurrentActionMap("Player");
        MouseLock(true);
        ShootSpell(context);
    }

    /// <summary>
    /// if the primary spell key binding is released before the threashold for bring up
    /// the menu. this is called to fire a quick spell
    /// </summary>
    /// <param name="context"></param>
    private void QuickPrimarySpellTrigger(InputAction.CallbackContext context)
    {
        if (spellMenu)
        {
            spellMenu = false;
            return;
        }
        QuickPrimaryFire.Invoke(context);
    }

    /// <summary>
    /// gets the current hovered over spell. then sends it to the spell manager
    /// </summary>
    /// <param name="context"></param>
    private void ShootSpell(InputAction.CallbackContext context)
    {
        UISkillLocation spell = mouseTracker.CurrentSelectedSkill;
        if(spell != UISkillLocation.None)
        {
            SpellFire(context, spell);
        }
    }

}
