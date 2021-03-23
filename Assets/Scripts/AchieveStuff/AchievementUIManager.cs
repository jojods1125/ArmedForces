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
    [Tooltip("Progress Bar Prefab")]
    public GameObject progressBarPrefab;

    private List<string> achievementDesc = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        // Go through all register Achievements
        foreach (Achievement a in AchievementManager.Instance().achievements)
        {
            // Create a GameObject based on achDisplay prefab
            GameObject curr = Instantiate(achievementPrefab, content.transform);

            // Get the Tiers child
            Transform tiers = curr.transform.Find("Tiers");
            // Get the Progress Bar child
            Transform progressBars = curr.transform.Find("Progress Bars");

            // Create a tier for every activation value
            for (int i = 0; i < a.activationValues.Length; i++)
            {
                Instantiate(tierPrefab, tiers);
                Instantiate(progressBarPrefab, progressBars);
            }

            // Set size of tier
            GridLayoutGroup tglg = tiers.GetComponent<GridLayoutGroup>();
            tglg.cellSize = new Vector2(tglg.cellSize.x / a.activationValues.Length, tglg.cellSize.y);
            // Set size of progress bars
            GridLayoutGroup pbglg = progressBars.GetComponent<GridLayoutGroup>();
            pbglg.cellSize = new Vector2(pbglg.cellSize.x / a.activationValues.Length, pbglg.cellSize.y);

            // Set progress bars
            int currentValue = a.currentValue;
            for (int i = 0; i < a.activationValues.Length; i++)
			{
                // Current working bar
                Transform bar = progressBars.GetChild( i );

                // This tiers max
                int max = a.activationValues[i];
                
                // Check if this bar needs scaled at all, break if not
                if (i > 0 && currentValue < a.activationValues[i - 1])
				{
                    break;
				}
                // Check if this bar has been met/exceeded
                else if (currentValue >= max)
				{
                    bar.localScale = new Vector3( 1f, bar.localScale.y, bar.localScale.z );
				}
                // Set the bar scale
				else
				{
                    bar.localScale = new Vector3( currentValue / max, bar.localScale.y, bar.localScale.z );
				}
			}

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
            /*Transform progressBar = curr.transform.Find("Progress");
            float maxValue = a.activationValues[a.activationValues.Length - 1];
            progressBar.localScale = new Vector3( a.currentValue / maxValue, progressBar.localScale.y, progressBar.localScale.z );*/
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < AchievementManager.Instance().achievements.Count; i++)
        {
            // Get Achievement
            Achievement a = AchievementManager.Instance().achievements[i];
            // Get Achievement Display
            Transform curr = content.transform.GetChild( i );
            // Get Progress Bars for Display
            Transform progressBars = curr.Find( "Progress Bars" );

            // Update progress bars
            int currentValue = a.currentValue;
            for (int j = 0; j < a.activationValues.Length; j++)
            {
                // Current working bar
                Transform bar = progressBars.GetChild( j );

                // This tiers max
                int max = a.activationValues[j];

                // Check if this bar needs scaled at all -> Zero the scale
                if (j > 0 && currentValue < a.activationValues[j - 1])
                {
                    bar.localScale = new Vector3(0f, bar.localScale.y, bar.localScale.z);
                }
                // Check if this bar has been met/exceeded -> Max the scale
                else if (currentValue >= max)
                {
                    bar.localScale = new Vector3(1f, bar.localScale.y, bar.localScale.z);
                }
                // Set the bar scale
                else
                {
					float percent = 0f;
					if (j > 0)
					{
						int delta = a.activationValues[j - 1];
						percent = (currentValue - delta) / ((float)max - delta);
					}
					else
					{
						percent = currentValue / (float)max;
					}
					/*float percent = currentValue / (float)max;*/
					bar.localScale = new Vector3(percent, bar.localScale.y, bar.localScale.z);
                }
            }

            // Set Progress Bar
            /*Transform progressBar = curr.Find("Progress");
            float maxValue = a.activationValues[a.activationValues.Length - 1];
            progressBar.localScale = new Vector3(a.currentValue / maxValue, progressBar.localScale.y, progressBar.localScale.z);*/
        }
    }
}