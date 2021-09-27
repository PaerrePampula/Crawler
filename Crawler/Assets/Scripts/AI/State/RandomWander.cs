using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class RandomWander : IState
{
    CharacterController _characterController;
    BaseMook _baseMook;
    Vector3 wanderLocation;
    int wanderCount;
    int currentWanders = 0;
    bool _wanderDone;
    public RandomWander(CharacterController characterController, BaseMook baseMook)
    {
        _baseMook = baseMook;
        _characterController = characterController;
    }
    public void OnStateEnter()
    {
        _wanderDone = false;
        //One or two wanders
        wanderCount = UnityEngine.Random.Range(1, 3);
        CreateWander();
    }

    private void CreateWander()
    {
        if (currentWanders < wanderCount)
        {
            //Add some random coordinate to the current pos, set that as a random wander target
            wanderLocation = new Vector3(UnityEngine.Random.Range(-3f, 3f), 0, UnityEngine.Random.Range(-3f, 3f));
            float wanderTime = UnityEngine.Random.Range(0.2f, 0.8f);
            _baseMook.StartCoroutine(waitForWander(wanderTime));
        }
        else
        {
            _wanderDone = true;
        }
    }

    IEnumerator waitForWander(float timeForWander)
    {
        float timer = 0;
        while (timer <= timeForWander)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        currentWanders++;
        CreateWander();
    }
    public void OnStateExit()
    {
        wanderLocation = Vector3.zero;
        currentWanders = 0;
    }

    public bool StateReadyToTransistion()
    {
        return true;
    }

    public void Tick()
    {
        _characterController.Move(wanderLocation * Time.deltaTime);
    }
    public bool WanderIsDone()
    {
        return _wanderDone;
    }
}

