using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// this GUI is responsible for selecting from 
/// RobotController _robots List
/// </summary>
class RobotSelectorUI : MonoBehaviour
{
    [SerializeField] RobotController _robotController;

    [InspectorName("UI-Properties")]
    public Color[] colors;
    public GameObject scrollbar;
    private float scroll_pos = 0;
    private float[] pos;
    private bool runIt = false;
    private float time;

    void Start()
    {
        if (_robotController == null)
            _robotController = FindObjectOfType<RobotController>();

        List<GameObject> robots = _robotController.GetRobotModels();

        for (int i = 0; i < robots.Count; i++)
        {
            //set UI Robot Panel names
            transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = robots[i].name;
        }
    }

    void Update()
    {
        UpdateSlideUI();
    }

    private void UpdateSlideUI()
    {
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);

        if (runIt)
        {
            time += Time.deltaTime;

            if (time > 1f)
            {
                time = 0;
                runIt = false;
            }
        }

        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }

        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }


        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
                transform.GetChild(i).GetComponent<Image>().color = colors[0];

                //set new robot when sliding over window
                _robotController.SetRobot(i);
                    
                for (int j = 0; j < pos.Length; j++)
                {
                    if (j != i)
                    {
                        transform.GetChild(j).localScale = Vector2.Lerp(transform.GetChild(j).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                        transform.GetChild(j).GetComponent<Image>().color = colors[1];
                    }
                }
            }
        }
    }
}
