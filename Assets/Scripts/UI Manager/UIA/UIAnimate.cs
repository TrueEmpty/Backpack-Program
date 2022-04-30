using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimate : MonoBehaviour
{
    public string baseKey = "";
    public Image image;
    public float baseSpeed = 1;

    [SerializeField]
    public List<UIAnimator> uIAList = new List<UIAnimator>();

    public string inputKey = "";
    public bool onExtra = false; //If set to *

    public Transform target;
    Transform tU;

    public RectTransform targetr;
    RectTransform tUr;


    // Start is called before the first frame update
    void Start()
    {
        //Set Target to move
        tUr = GetComponent<RectTransform>();

        if (targetr != null)
        {
            tUr = targetr;
        }

        if (tUr != null)
        {
             //Get base info
            if (image != null)
            {
                uIAList.Add(new UIAnimator(baseKey, tUr, image.color, baseSpeed));
            }
            else
            {
                uIAList.Add(new UIAnimator(baseKey, tUr, speed: baseSpeed));
            }
        }
        else
        {
            tU = transform;

            if (target != null)
            {
                tU = target;
            }

            //Get base info
            if (image != null)
            {
                uIAList.Add(new UIAnimator(baseKey, tU, image.color, baseSpeed));
            }
            else
            {
                uIAList.Add(new UIAnimator(baseKey, tU, speed: baseSpeed));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Will Check for the state
        if(uIAList.Count > 0)
        {
            //Grab uia
           UIAnimator uia = uIAList.Find(x => x.Key == inputKey);

            //If a uia is found
            if(uia != null)
            {
                if (tUr != null)
                {
                    UpdateUIARect(uia);
                }
                else
                {
                    UpdateUIA(uia);
                }
            }
            else
            {
                //Grab All Other uia (*) if avaliable
                uia = uIAList.Find(x => x.Key == "*");

                //If an all uia is found
                if (uia != null)
                {
                    if(tUr != null)
                    {
                        UpdateUIARect(uia);
                    }
                    else
                    {
                        UpdateUIA(uia);
                    }
                }
            }
        }
    }

    void UpdateUIA(UIAnimator uia)
    {
        //Update Position
        if (uia.updatePos)
        {
            tU.position = Vector3.Lerp(tU.position, uia.Position, uia.Speed);
        }

        //Update Rotation
        if (uia.updateRot)
        {
            tU.rotation = Quaternion.Lerp(tU.rotation, uia.Rotation, uia.Speed);
        }

        //Check if image is assigned
        if (image != null)
        {
            //Update Color
            if (uia.updateCol)
            {
                image.color = Color.Lerp(image.color, uia.Color, uia.Speed);
            }
        }

        //Update Size
        if (uia.updateSize)
        {
            tU.localScale = Vector3.Lerp(tU.localScale, uia.Size, uia.Speed);
        }
    }

    void UpdateUIARect(UIAnimator uia)
    {
        //Update Position
        if (uia.updatePos)
        {
            tUr.anchoredPosition3D = Vector2.Lerp(tUr.anchoredPosition3D, uia.Position, uia.Speed);
        }

        //Update Rotation
        if (uia.updateRot)
        {
            tUr.rotation = Quaternion.Lerp(tUr.rotation, uia.Rotation, uia.Speed);
        }

        //Check if image is assigned
        if (image != null)
        {
            //Update Color
            if (uia.updateCol)
            {
                image.color = Color.Lerp(image.color, uia.Color, uia.Speed);
            }
        }

        //Update Size
        if (uia.updateSize)
        {
            tUr.localScale = Vector3.Lerp(tUr.localScale, uia.Size, uia.Speed);
        }
    }
}
