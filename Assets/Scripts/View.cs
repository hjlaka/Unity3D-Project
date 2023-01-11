using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class View : MonoBehaviour
{
    [SerializeField]
    private float viewRange;
    [SerializeField, Range(0f, 360f)]
    private float viewAngle;
    [SerializeField]
    private LayerMask targetMask;

    public UnityEvent<GameObject> OnDetect;

    public void FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, viewRange);

        for(int i = 0; i < colliders.Length; i++)
        {
            Vector3 dirToTarget = (colliders[i].transform.position - transform.position).normalized;
            Vector3 rightDir = AngleToDir(transform.eulerAngles.y + viewAngle * 0.5f);

            if (Vector3.Dot(transform.forward, dirToTarget) < Vector3.Dot(transform.forward, rightDir))
                continue;

            float disToTarget = Vector3.Distance(transform.position, colliders[i].transform.position);
            if (Physics.Raycast(transform.position, dirToTarget, disToTarget))
                continue;

            Debug.DrawRay(transform.position, dirToTarget * disToTarget, Color.red);

            OnDetect?.Invoke(colliders[i].gameObject);

        }
    }

    private Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewRange);

        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + viewAngle * 0.5f);
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - viewAngle * 0.5f);

        Debug.DrawRay(transform.position, rightDir * viewRange, Color.green);
        Debug.DrawRay(transform.position, leftDir * viewRange, Color.green);
    }
}
