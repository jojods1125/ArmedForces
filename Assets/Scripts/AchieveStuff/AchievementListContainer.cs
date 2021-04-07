using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AchievementListContainer
{
    public List<A_Tiered> tiered;
    public List<A_Typed> typed;
    public List<A_Repeatable> repeatable;

    /// <summary>
    /// Creates new Lists and then adds achievements into the corresponding list
    /// </summary>
    /// <param name="list"> List of base Achievements </param>
    public void CreateSeperateLists( List<Achievement> list )
    {
        typed = new List<A_Typed>();
        tiered = new List<A_Tiered>();
        repeatable = new List<A_Repeatable>();

        foreach (Achievement a in list)
        {
            if (a is A_Typed)
            {
                typed.Add((A_Typed)a);
            }
            else if (a is A_Tiered)
            {
                tiered.Add((A_Tiered)a);
            }
            else if (a is A_Repeatable)
            {
                repeatable.Add((A_Repeatable)a);
            }
            else
            {
                Debug.LogError("Non listed achievement inheritance");
            }
        }
    }

    /// <summary>
    /// Creates the base list for the manager to handle
    /// </summary>
    /// <returns> basic list of Achievements </returns>
    public List<Achievement> CreateBaseList()
    {
        List<Achievement> list = new List<Achievement>();

        foreach (Achievement a in tiered)
        {
            list.Add(a);
        }
        foreach (Achievement a in typed)
        {
            list.Add(a);
        }
        foreach (Achievement a in repeatable)
        {
            list.Add(a);
        }

        return list;
    }

    public void SetLists( List<A_Tiered> tieredList, List<A_Typed> typedList, List<A_Repeatable> repeatableList)
    {
        tiered = tieredList;
        typed = typedList;
        repeatable = repeatableList;
    }
}
