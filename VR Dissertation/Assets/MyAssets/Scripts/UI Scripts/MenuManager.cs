using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public Panel currentPanel = null;

    // Create list of panels
    private List<Panel> panelHistory = new List<Panel>();

    private void Start()
    {
        SetupPanels();
    }

    private void SetupPanels()
    {
        // Start with array of panels and get all the panels that are children
        Panel[] panels = GetComponentsInChildren<Panel>();

        // go thru each panel in list of panels
        foreach (Panel panel in panels)
            panel.Setup(this);  // reference this instance of the panel manager

        currentPanel.Show();    // show current panel
    }

    private void Update()
    {
        // when chosen button is pressed, run GoToPrevious function
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
            GoToPrevious();
    }

    public void GoToPrevious()
    {
        // check to see if we have any panels in the history
        if (panelHistory.Count == 0)
        {
            // if user is in first menu and presses a back button, check if user wants to quit the app or not
            OVRManager.PlatformUIConfirmQuit();
            return;
        }

        // access most recent element of our history
        int lastIndex = panelHistory.Count - 1;

        // get that recent panel and set it as our current one
        SetCurrent(panelHistory[lastIndex]);

        // remove the recent element from the list
        panelHistory.RemoveAt(lastIndex);
    }

    public void SetCurrentWithHistory(Panel newPanel)
    {
        // when we go to a new panel, add it to the history
        panelHistory.Add(currentPanel);

        // Call SetCurrent to pass in the new panel
        SetCurrent(newPanel);
    }

    private void SetCurrent(Panel newPanel)
    {
        // Hide the currently active panel
        currentPanel.Hide();

        // Setup with the panel we are passing in and show it
        currentPanel = newPanel;
        currentPanel.Show();
    }
}
