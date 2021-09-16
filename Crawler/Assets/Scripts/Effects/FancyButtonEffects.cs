using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// Images with none as the source image makes the engine render the area of the
/// image as a blank square, this is used to make otherwise invisible-by-area
/// buttons have a cool effect whenever they are highlighted, so they will appear with no alpha when not highlighted, but
/// with max alpha when highlighted
/// </summary>
public class FancyButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Button button;
    Image image;
    [SerializeField] Color32 unselectedColor;
    [SerializeField] Color32 effectColor;
    // Start is called before the first frame update
    void OnEnable()
    {
        //The button component is required to check what state the button is in.
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        image.color = unselectedColor;

    }

    void displayEffectColor()
    {
        image.color = effectColor;
    }
    private void displayNormalColor()
    {
        image.color = unselectedColor;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        displayNormalColor();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        displayEffectColor();
    }
}
