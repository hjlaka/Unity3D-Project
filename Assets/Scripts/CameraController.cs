using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : SingleTon<CameraController>
{

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float zoomSpeed;

    [SerializeField]
    private float padding;

    [SerializeField]
    private CinemachineVirtualCamera vCam;

    [SerializeField]
    private CinemachineFreeLook freeCam;

    [SerializeField]
    private CinemachineTargetGroup targetGroup;

    [SerializeField]
    private Transform freeCamInitTrans;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        AddToTargetGroup(freeCamInitTrans);
        RemoveFromTargetGroup(freeCamInitTrans);
    }

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

    public void ChangeFreeCamPriority(int priority)
    {
        freeCam.Priority = priority;
    }

    public void SetFreeCam()
    {
        ChangeFreeCamPriority(40);
    }

    public void SetCamToSelectedPiece()
    {
        //Debug.Log("카메라 기물 비추기");
        //SetFreeCam(PlaceManager.Instance.SelectedPiece.transform);
    }

    public void ChangeVCamPriority(int priority)
    {
        vCam.Priority = priority;
    }

    public void SetCamToTopDownView()
    {
        //Debug.Log("카메라 탑다운 뷰");
        ChangeVCamPriority(20);
        ChangeFreeCamPriority(10);
    }

    public void AddToTargetGroup(Transform trans)
    {
        targetGroup.AddMember(trans, 1, 0);
    }

    public void AddToTargetGroupAll(List<Transform> transList)
    {
        for(int i = 0; i < transList.Count; i++)
        {
            AddToTargetGroup(transList[i]);
        }
    }
    public void RemoveFromTargetGroup(Transform trans)
    {
        targetGroup.RemoveMember(trans);
    }

}
