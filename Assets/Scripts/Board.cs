using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Board : MonoBehaviour
{
    private Vector2Int size;
    public Vector2Int Size
    {
        get { return size; }
        set { size = value; }
    }

    private bool followRule;
    public bool FollowRule { get; set; }
    public Transform heatPointHUD;
    private List<TextMeshProUGUI> heatHUDList;
    public Place[,] places;
    private float cellSize;

    private void Awake()
    {
        heatHUDList = new List<TextMeshProUGUI>();
        
    }
    private void Start()
    {

        if (null == heatPointHUD)
            return;

        //Debug.Log(heatPointHUD.GetComponent<GridLayoutGroup>());
        //cellSize = heatPointHUD.GetComponent<GridLayout>().cellSize.x;
        CreateHUD();
        UpdateHeatHUD();
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            ResetHeat();
        }
    }
    public void ResetHeat()
    {
        if (null == heatPointHUD)
            return;


        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                places[i, j].HeatPoint = 0;
                heatHUDList[(i * size.x) + j].text = "0";
            }
        }
    }

    private void CreateHUD()
    {
        heatPointHUD.GetComponent<RectTransform>().sizeDelta = new Vector2(20 * size.x, 20 * size.y);

        for (int i = 0; i < size.x * size.y; i++)
        {
            GameObject textUI = Instantiate(new GameObject());
            textUI.transform.SetParent(heatPointHUD.transform, false);
            textUI.AddComponent<TextMeshProUGUI>();
            TextMeshProUGUI text = textUI.GetComponent<TextMeshProUGUI>();
            text.text = i.ToString();
            text.fontSize = 20f;
            //TextMeshProUGUI text = heatPointHUD.GetChild(i).GetComponent<TextMeshProUGUI>();
            heatHUDList.Add(text);
        }

        heatPointHUD.gameObject.SetActive(false);
    }
    public void UpdateHeatHUD()
    {
        if (null == heatPointHUD)
            return;

        for (int i = 0; i < size.x; i++)
        {
            for(int j = 0; j < size.y; j++)
            {
                heatHUDList[(i * size.x) + j].text = places[i, j].HeatPoint.ToString();
            }
        }
    }
}
