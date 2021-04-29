using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAI : MonoBehaviour
{

    public GameObject aiPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (MenuManager.Instance().training)
        {
            StartCoroutine(SpawnAIPlayer());
        }
    }


    public IEnumerator SpawnAIPlayer()
    {
        yield return new WaitForSeconds(3);

        Instantiate(aiPrefab, transform.localPosition, Quaternion.identity);
    }
}
