using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticItemDrop : MonoBehaviour
{
    [SerializeField] List<ItemScriptable> possibleDrops = new List<ItemScriptable>();
    // Start is called before the first frame update
    void Start()
    {
        int dropIndex = Random.Range(0, possibleDrops.Count);
        GetComponent<Pickup>().InitializePickUp(possibleDrops[dropIndex]);
    }

}
