using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    List<Transform> movePoints = new List<Transform>();
    int currentTargetPointIndex = 0;
    [SerializeField] bool returnInReverse = false;
    bool returning = false;
    [SerializeField] Transform movePointParent;
    [SerializeField] Transform platform;
    Vector3 movedir;
    [SerializeField] float platformMoveSpeed = 2f;
    [SerializeField] float platformReachEndWaitTime = 1f;
    bool cycling = false;
    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < movePointParent.childCount; i++)
        {
            movePoints.Add(movePointParent.GetChild(i));
        }
    }
    private void OnEnable()
    {
        currentTargetPointIndex = 0;
        platform.position = movePoints[currentTargetPointIndex].position;
        StopAllCoroutines();
        cycling = false;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(platform.position, movePoints[currentTargetPointIndex].position) <= 0.05f)
        {

            if (currentTargetPointIndex == 0 || currentTargetPointIndex == movePoints.Count-1)
            {
                if (!cycling)
                {
                    cycling = true;
                    movedir = Vector3.zero;
                    StartCoroutine(ActionDelayer.actionWait(() => CycleTarget(), Time.time + platformReachEndWaitTime));
                }

            }
            else
            {
                CycleTarget();
            }


        }
        platform.position += movedir * Time.deltaTime;
    }


    private void CycleTarget()
    {
        if (returnInReverse)
        {
            if (returning)
            {
                currentTargetPointIndex = (currentTargetPointIndex - 1 > 0) ? currentTargetPointIndex - 1 : 0;
            }
            else
            {
                currentTargetPointIndex = (currentTargetPointIndex >= movePoints.Count - 1) ? 0 : currentTargetPointIndex + 1;
            }
            if (currentTargetPointIndex == movePoints.Count - 1) returning = true;
            else if (currentTargetPointIndex == 0) returning = false;
        }
        else
        {
            currentTargetPointIndex = (currentTargetPointIndex >= movePoints.Count - 1) ? 0 : currentTargetPointIndex + 1;
        }



        movedir = (movePoints[currentTargetPointIndex].position - platform.position).normalized;
        
        movedir = movedir * platformMoveSpeed;
        cycling = false;
    }
}
