using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Repeatable Achievement", menuName = "Achievement/Repeatable")]
public class RepeatableAchievement : Achievement
{
    [Header("Repeat Value")]
    public int repeatValue;

    // Start is called before the first frame update
    void Start()
    {
        activationValues = new int[10];
        FillArray<int>( activationValues, repeatValue );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FillArray<T>( T[] array, T value)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = value;
        }
    }
}
