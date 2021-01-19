using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighscoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;

    private void Awake()
    {
        entryContainer = transform.Find("highscoreEntryContainer");    // Grab reference to the highscore entry container        
        entryTemplate = entryContainer.Find("highscoreEntryTemplate"); // Grab reference to the highscore entry template
                
        entryTemplate.gameObject.SetActive(false);                  // Hide default template

        string jsonString = PlayerPrefs.GetString("highscoreTable");            // Grab string for JsonString from highscoreTable
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);   // Use FromJson to convert from Highscores obj type to jsonString & return the Highscores object

        // Sort the entries by Score
        highscores.highscoreEntryList.Sort((x, y) => y.score.CompareTo(x.score));

        // The below code is deprecated as the above line does the same thing, but has been kept just in case it is needed
        //for (int i = 0; i < highscores.highscoreEntryList.Count; i++)                      // Go thru entire list, for every element...
        //{
        //    for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++)              // ...cycle thru all the elements after that one...
        //    {
        //        if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score)  // ... then test to see which one has the higher score...
        //        {
        //            // ...if needed, we swap their positions
        //            HighscoreEntry tmp = highscores.highscoreEntryList[i];
        //            highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
        //            highscores.highscoreEntryList[j] = tmp;
        //        }
        //    }
        //}

        highscoreEntryTransformList = new List<Transform>();
        // cycle thru list and add entries using the CreateHighscoreEntry function
        foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
        {
            CreateHighscoreEntry(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
    }

    // Function to add a new entry to the table using highscoreEntry object, transform for the container, and list of transforms to add instantiated transform onto
    private void CreateHighscoreEntry(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 60f;                                 // float for template height

        Transform entryTransform = Instantiate(entryTemplate, container);                   // Instantiate entryTransform based on the template
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();    // Grab RectTransform
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);          // Position RectTransform height based on the float * transformList.Count
        entryTransform.gameObject.SetActive(true);                                          // Set entryTransform object to true

        int rank = transformList.Count + 1;   // Position rank using +1 so the first rank is not as rank 0
        // Convert rank int into suitable strings for top 3. Default is for positions 4 - 10.
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "TH"; break;

            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }

        entryTransform.Find("PositionText").GetComponent<TMP_Text>().text = rankString;     // Update Position Text Field

        //int score = ScoreScript.scoreValue;                                                 // score = scoreValue from ScoreScript
        int score = highscoreEntry.score;                                                   // Get score from highscoreEntry
        entryTransform.Find("ScoreText").GetComponent<TMP_Text>().text = score.ToString();  // Update Score Text Field

        string name = highscoreEntry.name;                                                  // Set name from highscoreEntry
        entryTransform.Find("NameText").GetComponent<TMP_Text>().text = name;               // Update Name Text Field

        entryTransform.Find("Background").gameObject.SetActive(rank % 2 == 1);              // Find "Background", make it visible behind odds and not evens

        if (rank == 1)
        {
            // Highlight 1st
            entryTransform.Find("PositionText").GetComponent<TMP_Text>().color = Color.green;
            entryTransform.Find("ScoreText").GetComponent<TMP_Text>().color = Color.green;
            entryTransform.Find("NameText").GetComponent<TMP_Text>().color = Color.green;
        }

        // Add to the transformList
        transformList.Add(entryTransform);
    }

    // Function to add a new entry
    private void AddHighScoreEntry(int score, string name)
    {
        // Create HighscoreEntry Object
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name };

        // Load Saved Highscores from PlayerPrefs
        string jsonString = PlayerPrefs.GetString("highscoreTable");            // Grab string for JsonString from highscoreTable
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);   // Use FromJson to convert from Highscores obj type to jsonString & return the Highscores object

        // Add entry to the Highscores
        highscores.highscoreEntryList.Add(highscoreEntry);

        // Save Updated Highscores
        string json = JsonUtility.ToJson(highscores);   // Use Json utility to convert object (highscoreEntryList) into json, which returns a string for the Json string
        PlayerPrefs.SetString("highscoreTable", json);  // Use PlayerPrefs to save and load persistent data
        PlayerPrefs.Save();
    }

    // Class containing highscoreEntryList
    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }

    // Class to represent a single high score entry
    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
        public string name;
    }
}
