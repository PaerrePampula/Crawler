using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatsDisplay : MonoBehaviour
{
    [SerializeField] GameObject textPrefab;
    private void OnEnable()
    {
        CreateElements();
    }
    private void OnDisable()
    {
        ClearElements();
    }
    void ClearElements()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
    void CreateElements()
    {
        Dictionary<string, RunStat> runStats = RunStatsController.Singleton.RunStats;
        foreach (RunStat runStat in runStats.Values)
        {
            GameObject go = Instantiate(textPrefab, transform);
            go.GetComponent<TMP_Text>().text = string.Format("{0}:{1}", runStat.Description, runStat.Value);
        }
    }
}
