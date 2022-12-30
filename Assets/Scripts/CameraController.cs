using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float zoomSpeed;

    [SerializeField]
    private float padding;

    private void Update()
    {
        Move();   
        Zoom();
    }

    private void Move()
    {
        Vector2 pos = Input.mousePosition;

        if (0 <= pos.x && pos.x <= padding)
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.World);
        else if (Screen.width - padding <= pos.x && pos.x <= Screen.width)
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime, Space.World);
        else if (0 <= pos.y && pos.y <= padding)
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);
        else if (Screen.height - padding <= pos.y && pos.y <= Screen.height)
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.World);
    }

    private void Zoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        transform.Translate(transform.forward * scroll * zoomSpeed * Time.deltaTime);       // 원하는대로 동작하지 않음
    }
}
