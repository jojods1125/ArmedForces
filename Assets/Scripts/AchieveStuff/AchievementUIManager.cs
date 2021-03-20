using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementUIManager : MonoBehaviour
{
    /*[Header("Client Achievement Manager")]
    [Tooltip("Achievement Manager")]
    public AchievementManager achievementManager;*/

    [Header("Content")]
    [Tooltip("Content of the ScrollView")]
    public GameObject content;
    [Tooltip("Display Prefab")]
    public GameObject achievementPrefab;
    [Tooltip("Tier Prefab")]
    public GameObject tierPrefab;

    private List<string> achievementDesc = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        // Go through all register Achievements
        foreach (Achievement a in AchievementManager.Instance().achievements)
        {
            /*// add correct toString
            if (a is TypedAchievement)
            {
                achievementDesc.Add(((TypedAchievement)a).ToString());
            }
            else
            {
                achievementDesc.Add(a.ToString());
            }*/

            // Create a GameObject based on achDisplay prefab
            GameObject curr = Instantiate(achievementPrefab, content.transform);

            // Get the Tiers child
            Transform tiers = curr.transform.Find("Tiers");
            
            // Create a tier for every activation value
            for (int i = 0; i < a.activationValues.Length; i++)
            {
                Instantiate(tierPrefab, tiers);
            }
            
            // Set size of tier
            GridLayoutGroup glg = tiers.GetComponent<GridLayoutGroup>();
            glg.cellSize = new Vector2(glg.cellSize.x / a.activationValues.Length, glg.cellSize.y);

            /** Set fields in display */
            // Name
            curr.transform.Find("Name").gameObject.GetComponent<TMP_Text>().text = a.achievementMessage;
            // Description
            curr.transform.Find("Description").gameObject.GetComponent<TMP_Text>().text = a.ToString();
            // Current Count
            curr.transform.Find("Current Count").gameObject.GetComponent<TMP_Text>().text = "Current Count: " + a.currentValue.ToString();
            // Next Count
            if (!a.IsComplete())
            {
                curr.transform.Find("Next Count").gameObject.GetComponent<TMP_Text>().text = "Next Count: " + a.activationValues[a.nextTier].ToString();
            }
            else
            {
                curr.transform.Find("Next Count").gameObject.GetComponent<TMP_Text>().text = "Achieved";
            }
            // Set Progress Bar
            Transform progressBar = curr.transform.Find("Progress");
            int maxValue = a.activationValues[a.activationValues.Length - 1];
            progressBar.localScale = new Vector3( a.currentValue / maxValue, progressBar.localScale.y, progressBar.localScale.z );
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
