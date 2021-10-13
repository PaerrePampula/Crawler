
using System;
using System.Collections.Generic;
using UnityEngine;

class RunStatsController : MonoBehaviour
{
    static RunStatsController _singleton;
    public static RunStatsController Singleton
    {
        get
        {
            if (_singleton == null)
            {
                _singleton = FindObjectOfType<RunStatsController>();
            }
            return _singleton;
        }
    }

    internal Dictionary<string, RunStat> RunStats { get => runStats; set => runStats = value; }

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
        if (RunStats.ContainsKey(identifier))
        {
            RunStats[identifier].Value += value;
            if (Globals.DebugOn)
            {
                Debug.Log(string.Format("New stat change:{0}\nNew value:{1}", RunStats[identifier].Description,  RunStats[identifier].Value));
            }
        }
    }
    void Start()
    {
        Player.onCurrentHpChanged += parseHPChange;
        Player.onPlayerReceivedItem += parseItemReceive;
        BaseMook.onMookDamaged += parseMookHPChange;
    }

    private void parseItemReceive(Item item)
    {

        if (item.ItemScriptable.ItemType == ItemBehaviourType.Money)
        {
            MoneyItemScriptable moneyItemScriptable = (MoneyItemScriptable)item.ItemScriptable;
            AddToStats("MoneyTaken", moneyItemScriptable.moneyAmount);
        }
        AddToStats("PickupsFound", 1);

    }

    private void OnDisable()
    {
        Player.onCurrentHpChanged -= parseHPChange;
        BaseMook.onMookDamaged -= parseMookHPChange;
        Player.onPlayerReceivedItem -= parseItemReceive;
    }

    private void parseMookHPChange(float amount, Vector3 location, bool wasCritical = false)
    {
        AddToStats("DamageGiven", Math.Abs(amount));
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
