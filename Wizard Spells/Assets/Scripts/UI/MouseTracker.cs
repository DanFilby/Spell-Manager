using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

//TODO: needs optimizing badly
/// <summary>
/// tracks mouse's position and loops over the UI polygons to check which spell is 
/// being selected. 
/// </summary>
public class MouseTracker : MonoBehaviour
{
    //holds all the UI areas
    public UISkillAreas UISkillAreas;
    public Dictionary<UIPolygon, UISkillLocation> UISkillMap;

    private Vector2 mousePos;
    private Canvas UICanvas;
    private (float, float) canvasSize;

    [Tooltip("The minumum the mouse needs to be away from the centre")]
    public float minimumPosRadius = 0.2f;

    public UISkillLocation CurrentSelectedSkill;
    public UnityAction HoveredSkillChange;

    void Awake()
    {
        UICanvas = GetComponentInChildren<Canvas>();
        Rect canvasRect = UICanvas.GetComponent<RectTransform>().rect;
        canvasSize = (canvasRect.width, canvasRect.height);

        UISkillMap = new Dictionary<UIPolygon, UISkillLocation>
        {
            { UISkillAreas.TopSkill, UISkillLocation.Top},
            { UISkillAreas.MidLeftSkill,UISkillLocation.MidLeft},
            { UISkillAreas.MidRightSkill, UISkillLocation.MidRight}, 
            { UISkillAreas.BotLeftSkill, UISkillLocation.BotLeft},
            { UISkillAreas.BotRightSkill,UISkillLocation.BotRight}
        };

        //UIPolgon yo = new UIPolgon(1, points);
        //yo.PrintPolygonInfo();
    }

    /// <summary>
    /// using new input system. called when the mouse pos is updated. but only when in spell
    /// select menu. this will then check if its within any UI area
    /// </summary>
    /// <param name="context"></param>
    public void GetMousePos(InputAction.CallbackContext context)
    {
        //changes the pos to 0-1
        mousePos = MousePosToGraph(context.ReadValue<Vector2>());
        if(Vector2.Distance(mousePos, new Vector2(0.5f,0.5f)) >= minimumPosRadius)
        {
            //finds the area of which the mouse is in. then if its changed, invokes event
            UISkillLocation skill = CheckUIInteraction(mousePos);
            if (skill != CurrentSelectedSkill && skill != UISkillLocation.None)
            {
                CurrentSelectedSkill = skill;
                HoveredSkillChange.Invoke();
            }
        }
        else
        {
            CurrentSelectedSkill = UISkillLocation.None;
            HoveredSkillChange.Invoke();
        }
        

    }

    /// <summary>
    /// changes a vector2 from pixels size to a range of 0-1
    /// </summary>
    private Vector2 MousePosToGraph(Vector2 mousePos)
    {
        mousePos.x /= canvasSize.Item1;
        mousePos.y /= canvasSize.Item2;

        return mousePos;
    }

    /// <summary>
    /// loops through UI areas checking which the mouse pos is in
    /// </summary>
    /// <param name="mousePos"></param>
    /// <returns></returns>
    private UISkillLocation CheckUIInteraction(Vector2 mousePos)
    {
        foreach(var pair in UISkillMap)
        {
            if (pair.Key.WithinPolygon(mousePos))
            {
                return pair.Value;
            }
        }
        return UISkillLocation.None;
    }

}

/// <summary>
/// Different areas in the Spell UI pullup menu
/// </summary>
public enum UISkillLocation {None = 0, Top = 1, MidLeft = 2, MidRight = 3, BotLeft = 4, BotRight = 5}

/// <summary>
/// Holds all the UI polygon for the skill menu areas
/// </summary>
[System.Serializable]
public struct UISkillAreas
{
    public UIPolygon TopSkill;
    public UIPolygon MidLeftSkill;
    public UIPolygon MidRightSkill;
    public UIPolygon BotLeftSkill;
    public UIPolygon BotRightSkill;
}

