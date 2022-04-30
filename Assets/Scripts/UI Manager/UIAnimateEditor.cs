using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

//[CustomEditor(typeof(UIAnimate))]
public class UIAnimateEditor //: Editor
{/*
    #region Variables

    Transform tr;
    Image im;
    string tar;

    UIAnimator nUIA = new UIAnimator();

    #endregion

    public override void OnInspectorGUI()
    {
        #region Pre Variables
        string pad = "      ";

        GUIStyle style = new GUIStyle();
        style.fontStyle = FontStyle.Bold;

        GUIStyle topAllign = new GUIStyle();
        topAllign.alignment = TextAnchor.UpperLeft;

        var togButt = new GUIStyle(GUI.skin.button);
        togButt.normal.textColor = Color.black;

        UIAnimate mS = (UIAnimate)target;

        #endregion

        #region Used Script Display

        GUI.enabled = false;
            MonoScript script = MonoScript.FromMonoBehaviour((MonoBehaviour)mS);
            script = EditorGUILayout.ObjectField("Script", script, typeof(MonoScript), false) as MonoScript;
        GUI.enabled = true;

        #endregion

        #region Base Info

        EditorGUILayout.LabelField("Base Variables", style);

        mS.baseKey = EditorGUILayout.TextField("Base Key",mS.baseKey, GUILayout.MaxWidth(305));

        mS.baseSpeed = EditorGUILayout.FloatField("Base Speed", mS.baseSpeed, GUILayout.MaxWidth(305));

        im = EditorGUILayout.ObjectField("Image", im, typeof(Image), true, GUILayout.MaxWidth(305)) as Image;

        //Check if oject contains an image script
        if(im != null)
        {
            if (im.GetComponent<Image>() == null)
            {
                im = null;
            }
        }

        tr = EditorGUILayout.ObjectField("Target", tr, typeof(Transform), true, GUILayout.MaxWidth(305)) as Transform;

        //Check if oject contains a Rect Transform or Transform script
        if (tr != null)
        {
            if (tr.GetComponent<RectTransform>() == null)
            {
                if (tr.GetComponent<Transform>() == null)
                {
                    tr = null;
                }
                else
                {
                    tar = "TT";
                }
            }
            else
            {
                tar = "TR";
            }
        }
        else
        {
            if (mS.GetComponent<RectTransform>() == null)
            {
                if (mS.GetComponent<Transform>() != null)
                {
                    tar = "ST";
                }
                else
                {
                    tar = "";
                }
            }
            else
            {
                tar = "SR";
            }
        }

        mS.onExtra = EditorGUILayout.Toggle("On Extra", mS.onExtra);

        #endregion

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        #region UIAList

        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("UIAList Elements ", style,GUILayout.Width(110));
            EditorGUILayout.LabelField("(" + mS.uIAList.Count + ")", topAllign);
        EditorGUILayout.EndHorizontal();

        for(int i = 0; i <= mS.uIAList.Count; i++)
        {
            if(i < mS.uIAList.Count)
            {
                UIAnimator an = mS.uIAList[i];


                an.showUIAElements = EditorGUILayout.Foldout(an.showUIAElements, an.Key);

                if (an.showUIAElements)
                {
                    #region UIA List

                    an.Key = EditorGUILayout.TextField(pad + "Key", an.Key, GUILayout.MaxWidth(305));

                    #region Position

                    EditorGUILayout.BeginHorizontal();

                        EditorGUILayout.LabelField(pad, GUILayout.MaxWidth(22));

                        togButt.normal.textColor = Color.black;

                        if (an.updatePos)
                        {
                            togButt.normal.textColor = Color.Lerp(Color.black, Color.blue, .6f);
                        }

                        if (GUILayout.Button("Position", togButt, GUILayout.MaxWidth(75), GUILayout.MaxHeight(15)))
                        {
                            an.updatePos = !an.updatePos;
                        }

                        EditorGUILayout.LabelField(pad, GUILayout.MaxWidth(10));

                        EditorGUILayout.LabelField("X", GUILayout.MaxWidth(11));

                        an.Position.x = EditorGUILayout.FloatField(an.Position.x, GUILayout.Width(36));

                        EditorGUILayout.LabelField("Y", GUILayout.MaxWidth(11));

                        an.Position.y = EditorGUILayout.FloatField(an.Position.y, GUILayout.Width(36));

                        EditorGUILayout.LabelField("Z", GUILayout.MaxWidth(11));

                        an.Position.z = EditorGUILayout.FloatField(an.Position.z, GUILayout.Width(36));

                        if(GUILayout.Button("C", GUILayout.MaxWidth(21),GUILayout.MaxHeight(15)))
                        {
                            switch(tar)
                            {
                                case "TT":
                                    an.Position = tr.transform.position;
                                break;
                                case "TR":
                                    an.Position = tr.GetComponent<RectTransform>().anchoredPosition3D;
                                break;
                                case "ST":
                                    an.Position = mS.transform.position;
                                break;
                                case "SR":
                                    an.Position = mS.GetComponent<RectTransform>().anchoredPosition3D;
                                break;
                            }
                        }

                    EditorGUILayout.EndHorizontal();

                    #endregion

                    #region Rotation

                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField(pad, GUILayout.MaxWidth(22));

                    togButt.normal.textColor = Color.black;

                    if (an.updateRot)
                    {
                        togButt.normal.textColor = Color.Lerp(Color.black, Color.blue, .6f);
                    }

                    if (GUILayout.Button("Rotation", togButt, GUILayout.MaxWidth(75), GUILayout.MaxHeight(15)))
                    {
                        an.updateRot = !an.updateRot;
                    }

                    EditorGUILayout.LabelField(pad, GUILayout.MaxWidth(10));

                    EditorGUILayout.LabelField("X", GUILayout.MaxWidth(11));

                    float x = EditorGUILayout.FloatField(an.Rotation.eulerAngles.x, GUILayout.Width(36));

                    EditorGUILayout.LabelField("Y", GUILayout.MaxWidth(11));

                    float y = EditorGUILayout.FloatField(an.Rotation.eulerAngles.y, GUILayout.Width(36));

                    EditorGUILayout.LabelField("Z", GUILayout.MaxWidth(11));

                    float z = EditorGUILayout.FloatField(an.Rotation.eulerAngles.z, GUILayout.Width(36));

                    an.Rotation = Quaternion.Euler(x, y, z);

                    if (GUILayout.Button("C", GUILayout.MaxWidth(21), GUILayout.MaxHeight(15)))
                    {
                        switch (tar)
                        {
                            case "TT":
                                an.Rotation = Quaternion.Euler(tr.transform.rotation.eulerAngles);
                                break;
                            case "TR":
                                an.Rotation = Quaternion.Euler(tr.transform.rotation.eulerAngles);
                                break;
                            case "ST":
                                an.Rotation = Quaternion.Euler(mS.transform.rotation.eulerAngles);
                                break;
                            case "SR":
                                an.Rotation = Quaternion.Euler(mS.transform.rotation.eulerAngles);
                                break;
                        }
                    }

                    EditorGUILayout.EndHorizontal();

                    #endregion

                    #region Color

                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField(pad, GUILayout.MaxWidth(22));

                    togButt.normal.textColor = Color.black;

                    if (an.updateCol)
                    {
                        togButt.normal.textColor = Color.Lerp(Color.black, Color.blue, .6f);
                    }

                    if (GUILayout.Button("Color", togButt, GUILayout.MaxWidth(75), GUILayout.MaxHeight(15)))
                    {
                        an.updateCol = !an.updateCol;
                    }

                    EditorGUILayout.LabelField(pad, GUILayout.MaxWidth(24));

                    an.Color = EditorGUILayout.ColorField(an.Color, GUILayout.MaxWidth(147));

                    if (GUILayout.Button("C", GUILayout.MaxWidth(21), GUILayout.MaxHeight(15)))
                    {
                        switch (tar)
                        {
                            case "TT":

                                Image imtt = tr.GetComponent<Image>();
                                if (imtt != null)
                                {
                                    an.Color = tr.GetComponent<Image>().color;
                                }

                                break;
                            case "TR":

                                Image imtr = tr.GetComponent<Image>();
                                if (imtr != null)
                                {
                                    an.Color = tr.GetComponent<Image>().color;
                                }

                                break;
                            case "ST":

                                Image imst = mS.GetComponent<Image>();
                                if (im != null)
                                {
                                    an.Color = mS.GetComponent<Image>().color;
                                }

                                break;
                            case "SR":

                                Image imsr = mS.GetComponent<Image>();

                                if (imsr != null)
                                {
                                    an.Color = mS.GetComponent<Image>().color;
                                }

                                break;
                        }
                    }

                    EditorGUILayout.EndHorizontal();

                    #endregion

                    #region Size

                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField(pad, GUILayout.MaxWidth(22));

                    togButt.normal.textColor = Color.black;

                    if (an.updateSize)
                    {
                        togButt.normal.textColor = Color.Lerp(Color.black, Color.blue, .6f);
                    }

                    if (GUILayout.Button("Size", togButt, GUILayout.MaxWidth(75), GUILayout.MaxHeight(15)))
                    {
                        an.updateSize = !an.updateSize;
                    }

                    EditorGUILayout.LabelField(pad, GUILayout.MaxWidth(10));

                    EditorGUILayout.LabelField("X", GUILayout.MaxWidth(11));

                    an.Size.x = EditorGUILayout.FloatField(an.Size.x, GUILayout.Width(36));

                    EditorGUILayout.LabelField("Y", GUILayout.MaxWidth(11));

                    an.Size.y = EditorGUILayout.FloatField(an.Size.y, GUILayout.Width(36));

                    EditorGUILayout.LabelField("Z", GUILayout.MaxWidth(11));

                    an.Size.z = EditorGUILayout.FloatField(an.Size.z, GUILayout.Width(36));

                    if (GUILayout.Button("C", GUILayout.MaxWidth(21), GUILayout.MaxHeight(15)))
                    {
                        switch (tar)
                        {
                            case "TT":
                                an.Size = tr.transform.localScale;
                                break;
                            case "TR":
                                an.Size = tr.GetComponent<RectTransform>().localScale;
                                break;
                            case "ST":
                                an.Size = mS.transform.localScale;
                                break;
                            case "SR":
                                an.Size = mS.GetComponent<RectTransform>().localScale;
                                break;
                        }
                    }

                    EditorGUILayout.EndHorizontal();

                    #endregion

                    an.Speed = EditorGUILayout.FloatField(pad + "Speed", an.Speed, GUILayout.MaxWidth(305));

                    #region Remove Element
                    EditorGUILayout.BeginHorizontal();

                        EditorGUILayout.LabelField(pad, GUILayout.MaxWidth(22));

                        togButt.normal.textColor = Color.red;

                        if (GUILayout.Button("Remove Element", togButt, GUILayout.MaxWidth(288), GUILayout.MaxHeight(15)))
                        {
                            mS.uIAList.RemoveAt(i);
                        }

                    EditorGUILayout.EndHorizontal();
                    #endregion

                    #endregion
                }
            }
            else //Last is the list which will be the add new field
            {
                #region New UIA

                EditorGUILayout.Space();

                if (mS.uIAList.Count > 0)
                {
                    EditorGUILayout.Space();
                }

                EditorGUILayout.LabelField("New UIA", style);

                nUIA.Key = EditorGUILayout.TextField(pad + "Key", nUIA.Key, GUILayout.MaxWidth(305));

                #region Position

                EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField(pad, GUILayout.MaxWidth(22));

                    togButt.normal.textColor = Color.black;

                    if (nUIA.updatePos)
                    {
                        togButt.normal.textColor = Color.Lerp(Color.black, Color.blue, .6f);
                    }

                    if (GUILayout.Button("Position", togButt, GUILayout.MaxWidth(75), GUILayout.MaxHeight(15)))
                    {
                        nUIA.updatePos = !nUIA.updatePos;
                    }

                    EditorGUILayout.LabelField(pad, GUILayout.MaxWidth(10));

                    EditorGUILayout.LabelField("X", GUILayout.MaxWidth(11));

                    nUIA.Position.x = EditorGUILayout.FloatField(nUIA.Position.x, GUILayout.Width(36));

                    EditorGUILayout.LabelField("Y", GUILayout.MaxWidth(11));

                    nUIA.Position.y = EditorGUILayout.FloatField(nUIA.Position.y, GUILayout.Width(36));

                    EditorGUILayout.LabelField("Z", GUILayout.MaxWidth(11));

                    nUIA.Position.z = EditorGUILayout.FloatField(nUIA.Position.z, GUILayout.Width(36));

                    if (GUILayout.Button("C", GUILayout.MaxWidth(21), GUILayout.MaxHeight(15)))
                    {
                        switch (tar)
                        {
                            case "TT":
                                nUIA.Position = tr.transform.position;
                                break;
                            case "TR":
                                nUIA.Position = tr.GetComponent<RectTransform>().anchoredPosition3D;
                                break;
                            case "ST":
                                nUIA.Position = mS.transform.position;
                                break;
                            case "SR":
                                nUIA.Position = mS.GetComponent<RectTransform>().anchoredPosition3D;
                                break;
                        }
                    }

                EditorGUILayout.EndHorizontal();

                #endregion

                #region Rotation

                EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField(pad, GUILayout.MaxWidth(22));

                    togButt.normal.textColor = Color.black;

                    if (nUIA.updateRot)
                    {
                        togButt.normal.textColor = Color.Lerp(Color.black, Color.blue, .6f);
                    }

                    if (GUILayout.Button("Rotation", togButt, GUILayout.MaxWidth(75), GUILayout.MaxHeight(15)))
                    {
                        nUIA.updateRot = !nUIA.updateRot;
                    }

                    EditorGUILayout.LabelField(pad, GUILayout.MaxWidth(10));

                    EditorGUILayout.LabelField("X", GUILayout.MaxWidth(11));

                    float nx = EditorGUILayout.FloatField(nUIA.Rotation.eulerAngles.x, GUILayout.Width(36));

                    EditorGUILayout.LabelField("Y", GUILayout.MaxWidth(11));

                    float ny = EditorGUILayout.FloatField(nUIA.Rotation.eulerAngles.y, GUILayout.Width(36));

                    EditorGUILayout.LabelField("Z", GUILayout.MaxWidth(11));

                    float nz = EditorGUILayout.FloatField(nUIA.Rotation.eulerAngles.z, GUILayout.Width(36));

                    nUIA.Rotation = Quaternion.Euler(nx, ny, nz);

                    if (GUILayout.Button("C", GUILayout.MaxWidth(21), GUILayout.MaxHeight(15)))
                    {
                        switch (tar)
                        {
                            case "TT":
                                nUIA.Rotation = Quaternion.Euler(tr.transform.rotation.eulerAngles);
                                break;
                            case "TR":
                                nUIA.Rotation = Quaternion.Euler(tr.transform.rotation.eulerAngles);
                                break;
                            case "ST":
                                nUIA.Rotation = Quaternion.Euler(mS.transform.rotation.eulerAngles);
                                break;
                            case "SR":
                                nUIA.Rotation = Quaternion.Euler(mS.transform.rotation.eulerAngles);
                                break;
                        }
                    }

                EditorGUILayout.EndHorizontal();

                #endregion

                #region Color

                EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField(pad, GUILayout.MaxWidth(22));

                    togButt.normal.textColor = Color.black;

                    if (nUIA.updateCol)
                    {
                        togButt.normal.textColor = Color.Lerp(Color.black, Color.blue, .6f);
                    }

                    if (GUILayout.Button("Color", togButt, GUILayout.MaxWidth(75), GUILayout.MaxHeight(15)))
                    {
                        nUIA.updateCol = !nUIA.updateCol;
                    }

                    EditorGUILayout.LabelField(pad, GUILayout.MaxWidth(24));

                    nUIA.Color = EditorGUILayout.ColorField(nUIA.Color, GUILayout.MaxWidth(147));

                    if (GUILayout.Button("C", GUILayout.MaxWidth(21), GUILayout.MaxHeight(15)))
                    {                       
                        switch (tar)
                        {
                            case "TT":

                                Image imtt = tr.GetComponent<Image>();
                                if (imtt != null)
                                {
                                    nUIA.Color = tr.GetComponent<Image>().color;
                                }

                            break;
                            case "TR":

                                Image imtr = tr.GetComponent<Image>();
                                if (imtr != null)
                                {
                                    nUIA.Color = tr.GetComponent<Image>().color;
                                }

                            break;
                            case "ST":

                                Image imst = mS.GetComponent<Image>();
                                if (im != null)
                                {
                                    nUIA.Color = mS.GetComponent<Image>().color;
                                }

                            break;
                            case "SR":

                                Image imsr = mS.GetComponent<Image>();

                                if (imsr != null)
                                {
                                    nUIA.Color = mS.GetComponent<Image>().color;
                                }

                            break;
                        }
                    }

                EditorGUILayout.EndHorizontal();

                #endregion

                #region Size

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(pad, GUILayout.MaxWidth(22));

                togButt.normal.textColor = Color.black;

                if (nUIA.updateSize)
                {
                    togButt.normal.textColor = Color.Lerp(Color.black, Color.blue, .6f);
                }

                if (GUILayout.Button("Size", togButt, GUILayout.MaxWidth(75), GUILayout.MaxHeight(15)))
                {
                    nUIA.updateSize = !nUIA.updateSize;
                }

                EditorGUILayout.LabelField(pad, GUILayout.MaxWidth(10));

                EditorGUILayout.LabelField("X", GUILayout.MaxWidth(11));

                nUIA.Size.x = EditorGUILayout.FloatField(nUIA.Size.x, GUILayout.Width(36));

                EditorGUILayout.LabelField("Y", GUILayout.MaxWidth(11));

                nUIA.Size.y = EditorGUILayout.FloatField(nUIA.Size.y, GUILayout.Width(36));

                EditorGUILayout.LabelField("Z", GUILayout.MaxWidth(11));

                nUIA.Size.z = EditorGUILayout.FloatField(nUIA.Size.z, GUILayout.Width(36));

                if (GUILayout.Button("C", GUILayout.MaxWidth(21), GUILayout.MaxHeight(15)))
                {
                    switch (tar)
                    {
                        case "TT":
                            nUIA.Size = tr.transform.localScale;
                            break;
                        case "TR":
                            nUIA.Size = tr.GetComponent<RectTransform>().localScale;
                            break;
                        case "ST":
                            nUIA.Size = mS.transform.localScale;
                            break;
                        case "SR":
                            nUIA.Size = mS.GetComponent<RectTransform>().localScale;
                            break;
                    }
                }

                EditorGUILayout.EndHorizontal();

                #endregion

                nUIA.Speed = EditorGUILayout.FloatField(pad + "Speed", nUIA.Speed, GUILayout.MaxWidth(305));

                EditorGUILayout.Space();

                #region Add Button
                EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField(pad, GUILayout.MaxWidth(22));

                    if (GUILayout.Button("Add UIA", GUILayout.MaxWidth(100), GUILayout.MaxHeight(15)))
                    {
                        //Add all elements to UIA
                        mS.uIAList.Add(new UIAnimator());

                        UIAnimator luial = mS.uIAList[mS.uIAList.Count - 1];

                        luial.Key = nUIA.Key;
                        luial.Position = nUIA.Position;
                        luial.updatePos = nUIA.updatePos;
                        luial.Rotation = nUIA.Rotation;
                        luial.updateRot = nUIA.updateRot;
                        luial.Color = nUIA.Color;
                        luial.updateCol = nUIA.updateCol;
                        luial.Size = nUIA.Size;
                        luial.updateSize = nUIA.updateSize;
                        luial.Speed = nUIA.Speed;

                        //Clear Current UIA
                        nUIA = new UIAnimator();
                    }

                    EditorGUILayout.LabelField(pad, GUILayout.MaxWidth(70));

                    if (GUILayout.Button("Clear UIA", GUILayout.MaxWidth(100), GUILayout.MaxHeight(15)))
                    {
                        nUIA = new UIAnimator();
                    }

                EditorGUILayout.EndHorizontal();
                #endregion

                #endregion
            }
        }

        #endregion
    }*/
}
