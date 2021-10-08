
using System;
using System.Collections.Generic;
using UnityEngine;

class RunStatsController : MonoBehaviour
{
    Dictionary<string, RunStat> runStats = new Dictionary<string, RunStat>()
    {
        {
            "DamageTaken", new RunStat("DamageTaken", "Damage Taken")
        },
        {
            "DamageGiven", new RunStat("DamageGiven", "Damage Given")
        },
        {
            "HealthRegained", new RunStat("HealthRegained", "Health Regained")
        },
        {
            "MoneyTaken", new RunStat("MoneyTaken", "Money Taken")
        },
        {
            "TimeSpent", new RunStat("TimeSpent", "Time spent on this run")
        },
        {
            "PickupsFound", new RunStat("PickupsFound", "Pickups found")
        }
    };

    void AddToStats(string identifier, float value)
    {
        if (runStats.ContainsKey(identifier))
        {
            runStats[identifier].Value += value;
            if (Globals.DebugOn)
            {
                Debug.Log(string.Format("New stat change:{0}\nNew value:{1}", runStats[identifier].Description,  runStats[identifier].Value));
            }
        }
    }
    void Start()
    {
        Player.onCurrentHpChanged += parseHPChange;
    }

    private void parseHPChange(float newHP, float changeAmount)
    {
        string identifier = (changeAmount >= 0) ? "HealthRegained" : "DamageTaken";
        AddToStats(identifier, Math.Abs( changeAmount));
    }
}

class RunStat
{
    public RunStat(string ID, string Description, float Value = 0)
    {
        _description = Description;
        _ID = ID;
        _value = Value;
    }
    string _description;
    string _ID;
    float _value;


    public string Description { get => _description; set => _description = value; }
    public string ID { get => _ID; set => _ID = value; }
    public float Value { get => _value; set => _value = value; }
}
