using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

class StatBuffUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI statBuffPercentageText;
    float currentPercentage = 0f;
    public void ChangePercentage(float toAddPercentage)
    {
        currentPercentage += toAddPercentage;
        UpdatePercentageToText();
    }
    public void Initialize(Sprite sprite)
    {
        GetComponent<Image>().sprite = sprite;
    }
    void UpdatePercentageToText()
    {
        statBuffPercentageText.text = string.Format("{0:.##}%", currentPercentage * 100f);
    }

}

