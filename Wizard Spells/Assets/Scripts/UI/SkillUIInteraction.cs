using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIInteraction : MonoBehaviour
{
    //TODO: use the struct to make this changable. so players can pick which spell goes in each slot

    //maps the menu location to the UI objs there
    private Dictionary<UISkillLocation, GameObject> UISkillObjects;

    //choose the effect of the mouse hovering over the UI component
    public enum EffectOnUI { FadeInOut, DisableAndEnable}
    public EffectOnUI effectOnUI;

    //an effect for UI components. which two is used to cover hovering over and hovering away
    private delegate void UIEffect(GameObject SkillUI);
    UIEffect HoverOverUI;
    UIEffect HoverAwayUI;

    //other scripts
    private MouseTracker mouseTracker;
    private UISkillLocation activeSkill;

    void Start()
    {
        SetupUIEffect();

        //adds to the event which is inoked after the users hovers over another UI location
        mouseTracker = gameObject.GetComponent<MouseTracker>();
        mouseTracker.HoveredSkillChange += changeSkillUI;

        //finds all the UI components
        UISkillObjects = new Dictionary<UISkillLocation, GameObject>();
        foreach( var pair in mouseTracker.UISkillMap)
        {
            GameObject UIObject = pair.Key.gameObject;
            UISkillObjects.Add(pair.Value, UIObject);
            HoverAwayUI(UIObject);
        }

    }

    /// <summary>
    /// sets delegates
    /// </summary>
    private void SetupUIEffect()
    {
        if(effectOnUI == EffectOnUI.DisableAndEnable)
        {
            HoverOverUI = EnableUIElement;
            HoverAwayUI = DisableUIElement;
        }
        else if(effectOnUI == EffectOnUI.FadeInOut)
        {
            HoverOverUI = FadeInUIElement;
            HoverAwayUI = FadeOutUIElement;
        }
    }

    /// <summary>
    /// called when users hovers over another UI location. uses the delagates to affect
    /// the previous and now active UI location
    /// </summary>
    private void changeSkillUI()
    {
        if(activeSkill != UISkillLocation.None) HoverAwayUI(UISkillObjects[activeSkill]);

        activeSkill = mouseTracker.CurrentSelectedSkill;

        if(activeSkill != UISkillLocation.None) HoverOverUI(UISkillObjects[activeSkill]);

    }

    private void FadeOutUIElement(GameObject skillUI)
    {

    }

    private void FadeInUIElement(GameObject skillUI)
    {

    }

    private void DisableUIElement(GameObject skillUI)
    {
        skillUI.GetComponentInChildren<Text>().enabled = false;
    }

    private void EnableUIElement(GameObject skillUI)
    {
        skillUI.GetComponentInChildren<Text>().enabled = true;
    }

}

