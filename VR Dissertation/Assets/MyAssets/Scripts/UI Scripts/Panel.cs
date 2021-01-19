using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    // Reference canvas
    private Canvas canvas = null;

    // Reference MenuManager script
    private MenuManager menuManager = null;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    public void Setup(MenuManager menuManager)
    {
        // MenuManager of this instance will = the one we are passing in
        this.menuManager = menuManager;
        Hide();
    }

    public void Show()
    {
        canvas.enabled = true;
    }

    public void Hide()
    {
        canvas.enabled = false;
    }
}
