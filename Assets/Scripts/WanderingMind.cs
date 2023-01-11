using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingMind : MonoBehaviour
{
    enum State { WANDERING, WAITING, ROTATING, WALKING }

    private bool isWandering = false;

    private State state;

    [SerializeField]
    private float rotateSpeed = 15;
    [SerializeField]
    private float moveSpeed = 2;


    [SerializeField]
    private float minWandering = 1;
    [SerializeField]
    private float maxWandering = 3;
    // 타겟을 리턴한다.

    private void Update()
    {
        switch (state)
        {
            case State.WANDERING:
                StartCoroutine(Wander());
                break;
            case State.WAITING:
                break;
            case State.WALKING:
                transform.Translate(transform.forward * moveSpeed * Time.deltaTime);
                break;
            case State.ROTATING:
                transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
                break;
        }

    }

    private IEnumerator Wander()
    {
        int waitingTime = Random.Range(1, 3);

        int walkTime = Random.Range(1, 3);

        int rotateTime = Random.Range(1, 3);
        int rotateDir = Random.Range(1, 2);

        state = State.WAITING;

        yield return new WaitForSeconds(waitingTime);

        state = State.WALKING;

        yield return new WaitForSeconds(walkTime);

        state = State.ROTATING;

        yield return new WaitForSeconds(rotateTime);

        state = State.WANDERING;
    }


}
