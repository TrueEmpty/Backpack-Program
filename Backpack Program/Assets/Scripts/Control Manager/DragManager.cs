using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragManager : MonoBehaviour
{
    public Transform target;

    bool dragging = false;
    Vector3 dragPoint = Vector3.zero;
    Vector3 startPoint = Vector3.zero;
    Vector2 baseScreen = new Vector2(1063, 686);
    Vector2 screenChange = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            DragWindow();
        }
    }

    void DragWindow()
    {
        if (!dragging)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                if (MouseClickInsideWindow())
                {
                    dragging = true;
                    dragPoint = Input.mousePosition;
                    startPoint = target.position;
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
                dragging = false;
            }
            else
            {
                screenChange = new Vector2((Screen.width / baseScreen.x) * .0145f, (Screen.height / baseScreen.y) * .0145f);

                Vector3 newPoint = new Vector3(Input.mousePosition.x - dragPoint.x, Input.mousePosition.y - dragPoint.y, 1);

                target.position = new Vector3(startPoint.x + (newPoint.x * screenChange.x), startPoint.y + (newPoint.y * screenChange.y), startPoint.z);
            }
        }
    }

    bool MouseClickInsideWindow()
    {
        bool result = false;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform == transform)
            {
                result = true;
            }
        }

        return result;
    }
}
