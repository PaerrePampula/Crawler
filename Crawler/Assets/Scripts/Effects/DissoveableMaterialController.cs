using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Controls the cool dissolve effect on e.g door locks
/// </summary>
public class DissoveableMaterialController : MonoBehaviour
{
    [SerializeField] Transform objectWithDissolvableMaterial;
    Material dissolveableMaterial;
    // Start is called before the first frame update
    void Start()
    {
        dissolveableMaterial = objectWithDissolvableMaterial.GetComponent<MeshRenderer>().material;
    }
    public void dissolveInSeconds(float seconds)
    {
        StartCoroutine(dissolveMaterial(seconds));
    }
    IEnumerator dissolveMaterial(float time)
    {
        float timer = 0;
        while (timer <= time)
        {
            float dissolveAmount = Mathf.Lerp(0f, 1f, timer / time);
            dissolveableMaterial.SetFloat("DissolveAmount", dissolveAmount);
            timer += Time.deltaTime;
            yield return null;
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
