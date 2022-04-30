using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIAnimator
{
    public string Key; //String to activate this key

    public Vector3 Position = new Vector3(); //New Position
    public bool updatePos = false;
    public Quaternion Rotation = new Quaternion(); // New Rotation
    public bool updateRot = false;
    public Color Color = new Color(); //New Color
    public bool updateCol = false;
    public Vector3 Size = new Vector3(); //New Size
    public bool updateSize = false;

    public float Speed = 1; //Speed at which the Transition happens

    //For Editor
    public bool showUIAElements = false;

    public UIAnimator() { }

    public UIAnimator(string key,Vector3 position = new Vector3(), Quaternion rotation = new Quaternion(), Color color = new Color(), Vector3 size = new Vector3(), float speed = 1)
    {
        Key = key;

        if(position != new Vector3())
        {
            Position = position;
            updatePos = true;
        }

        if (rotation != new Quaternion())
        {
            Rotation = rotation;
            updateRot = true;
        }

        if (color != new Color())
        {
            Color = color;
            updateCol = true;
        }

        if (size != new Vector3())
        {
            Size = size;
            updateSize = true;
        }

        Speed = speed;
    }

    public UIAnimator(string key, Vector3 position = new Vector3(), Vector3 rotation = new Vector3(), Color color = new Color(), Vector3 size = new Vector3(), float speed = 1)
    {
        Key = key;

        if (position != new Vector3())
        {
            Position = position;
            updatePos = true;
        }

        if (rotation != new Vector3())
        {
            Rotation = Quaternion.Euler(rotation);
            updateRot = true;
        }

        if (color != new Color())
        {
            Color = color;
            updateCol = true;
        }

        if (size != new Vector3())
        {
            Size = size;
            updateSize = true;
        }

        Speed = speed;
    }

    public UIAnimator(string key,Transform target, Color color = new Color(),float speed = 1)
    {
        Key = key;

        Position = target.position;
        updatePos = true;
        Rotation = target.rotation;
        updateRot = true;

        if (color != new Color())
        {
            Color = color;
            updateCol = true;
        }

        Size = target.localScale;
        updateSize = true;

        Speed = speed;
    }

    public UIAnimator(string key, RectTransform target, Color color = new Color(), float speed = 1)
    {
        Key = key;

        Position = target.anchoredPosition3D;
        updatePos = true;
        Rotation = target.rotation;
        updateRot = true;

        if (color != new Color())
        {
            Color = color;
            updateCol = true;
        }

        Size = target.localScale;
        updateSize = true;

        Speed = speed;
    }
}
