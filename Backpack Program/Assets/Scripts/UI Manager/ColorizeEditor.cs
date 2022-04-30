using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

//[CustomEditor(typeof(Colorize))]
public class ColorizeEditor //: Editor
{
    /*bool showFollowers = false;
    bool followersColors = false;

    public override void OnInspectorGUI()
    {
        Colorize mS = (Colorize)target;

        #region Used Script Display

        GUI.enabled = false;
        MonoScript script = MonoScript.FromMonoBehaviour((MonoBehaviour)mS);
        script = EditorGUILayout.ObjectField("Script", script, typeof(MonoScript), false) as MonoScript;
        GUI.enabled = true;

        #endregion

        #region Create Followers List

        showFollowers = EditorGUILayout.Foldout(showFollowers, "Followers (" + mS.followers.Count + ")");

        if(showFollowers)
        {
            for (int i = 0; i <= mS.followers.Count; i++)
            {
                if (i < mS.followers.Count)
                {
                    //Grab Followers Colorize Script
                    Colorize co = mS.followers[i].GetComponent<Colorize>();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(mS.followers[i].name, GUILayout.MaxWidth(80));
                    Transform tr = EditorGUILayout.ObjectField("", mS.followers[i], typeof(Transform), true, GUILayout.MaxWidth(155)) as Transform;

                    //Check if it has a Colorize Script
                    if (tr != null)
                    {
                        if (tr.GetComponent<Colorize>() != null)
                        {
                            mS.followers[i] = tr;
                        }
                        else
                        {
                            mS.followers.RemoveAt(i);
                        }
                    }
                    else
                    {
                        mS.followers.RemoveAt(i);
                    }

                    if (GUILayout.Button("X",GUILayout.Height(15),GUILayout.MaxWidth(20)))
                    {
                        mS.followers.RemoveAt(i);
                    }

                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    if(mS.followers.Count > 0)
                    {
                        EditorGUILayout.Space();
                    }

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("New Follower", GUILayout.MaxWidth(80));
                    Transform ntr = EditorGUILayout.ObjectField("", null, typeof(Transform), true, GUILayout.MaxWidth(180)) as Transform;
                    EditorGUILayout.EndHorizontal();

                    //Check if it has a Colorize Script
                    if (ntr != null)
                    {
                        if (ntr.GetComponent<Colorize>() != null)
                        {
                            mS.followers.Add(ntr);
                        }
                    }
                }
            }
        }

        #endregion

        EditorGUILayout.Space();

        #region Colors

        mS.showColors = EditorGUILayout.Foldout(mS.showColors, "State Color");

        if(mS.showColors)
        {
            ShowColors(mS);
        }

        #endregion

        EditorGUILayout.Space();

        #region Show Colors for all the followers

        followersColors = EditorGUILayout.Foldout(followersColors,"Follower's State Color (" + mS.followers.Count + ")");

        if(followersColors)
        {
            if(mS.followers.Count > 0)
            {
                for (int i = 0; i < mS.followers.Count; i++)
                {
                    Colorize co = mS.followers[i].GetComponent<Colorize>();

                    co.showColors = EditorGUILayout.Foldout(co.showColors, mS.followers[i].name + "'s Colors");

                    if(co.showColors)
                    {
                        ShowColors(co);
                    }

                    ShowWhatColor(co);
                }
            }
        }

        #endregion

        #region Set Color Change

        ShowWhatColor(mS);

        #endregion

    }

    void ShowColors(Colorize tar)
    {
        Colorize mS = (Colorize)target;
        var style = new GUIStyle(GUI.skin.button);

        #region Normal
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Normal", GUILayout.MaxWidth(80));
        if (GUILayout.Button("Set", GUILayout.MaxWidth(32)))
        {
            if (tar.transform.GetComponent<Image>() != null)
            {
                tar.Normal = tar.transform.GetComponent<Image>().color;
            }
            else if (tar.transform.GetComponent<Text>() != null)
            {
                tar.Normal = tar.transform.GetComponent<Text>().color;
            }
        }

        if(mS.showWhatColor == "")
        {
            style.normal.textColor = Color.Lerp(Color.black,Color.green,.6f);
        }
        else
        {
            style.normal.textColor = Color.black;
        }

        if (GUILayout.Button("Show", style, GUILayout.MaxWidth(43)))
        {
            mS.showWhatColor = "";
        }
        tar.Normal = EditorGUILayout.ColorField(tar.Normal, GUILayout.MaxWidth(100));
        EditorGUILayout.EndHorizontal();

        #endregion

        #region Highlighted

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Highlighted", GUILayout.MaxWidth(80));
        if (GUILayout.Button("Set", GUILayout.MaxWidth(32)))
        {
            if (tar.transform.GetComponent<Image>() != null)
            {
                tar.Highlighted = tar.transform.GetComponent<Image>().color;
            }
            else if (tar.transform.GetComponent<Text>() != null)
            {
                tar.Highlighted = tar.transform.GetComponent<Text>().color;
            }
        }

        if (mS.showWhatColor == "Highlighted")
        {
            style.normal.textColor = Color.Lerp(Color.black, Color.green, .6f);
        }
        else
        {
            style.normal.textColor = Color.black;
        }

        if (GUILayout.Button("Show",style, GUILayout.MaxWidth(43)))
        {
            if (mS.showWhatColor == "")
            {
                if (tar.transform.GetComponent<Image>() != null)
                {
                    tar.Normal = tar.transform.GetComponent<Image>().color;
                }
                else if (tar.transform.GetComponent<Text>() != null)
                {
                    tar.Normal = tar.transform.GetComponent<Text>().color;
                }

                mS.showWhatColor = "Highlighted";
            }
            else if (mS.showWhatColor != "Highlighted")
            {
                mS.showWhatColor = "Highlighted";
            }
            else
            {
                mS.showWhatColor = "";
            }
        }
        tar.Highlighted = EditorGUILayout.ColorField(tar.Highlighted, GUILayout.MaxWidth(100));
        EditorGUILayout.EndHorizontal();

        #endregion

        #region Pressed

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Pressed", GUILayout.MaxWidth(80));
        if (GUILayout.Button("Set", GUILayout.MaxWidth(32)))
        {
            if (tar.transform.GetComponent<Image>() != null)
            {
                tar.Pressed = tar.transform.GetComponent<Image>().color;
            }
            else if (tar.transform.GetComponent<Text>() != null)
            {
                tar.Pressed = tar.transform.GetComponent<Text>().color;
            }
        }

        if (mS.showWhatColor == "Pressed")
        {
            style.normal.textColor = Color.Lerp(Color.black, Color.green, .6f);
        }
        else
        {
            style.normal.textColor = Color.black;
        }

        if (GUILayout.Button("Show",style, GUILayout.MaxWidth(43)))
        {
            if (mS.showWhatColor == "")
            {
                if (tar.transform.GetComponent<Image>() != null)
                {
                    tar.Normal = tar.transform.GetComponent<Image>().color;
                }
                else if (tar.transform.GetComponent<Text>() != null)
                {
                    tar.Normal = tar.transform.GetComponent<Text>().color;
                }

                mS.showWhatColor = "Pressed";
            }
            else if (mS.showWhatColor != "Pressed")
            {
                mS.showWhatColor = "Pressed";
            }
            else
            {
                mS.showWhatColor = "";
            }
        }
        tar.Pressed = EditorGUILayout.ColorField(tar.Pressed, GUILayout.MaxWidth(100));
        EditorGUILayout.EndHorizontal();

        #endregion

        #region Disabled

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Disabled", GUILayout.MaxWidth(80));
        if (GUILayout.Button("Set", GUILayout.MaxWidth(32)))
        {
            if (tar.transform.GetComponent<Image>() != null)
            {
                tar.Disabled = tar.transform.GetComponent<Image>().color;
            }
            else if (mS.transform.GetComponent<Text>() != null)
            {
                tar.Disabled = tar.transform.GetComponent<Text>().color;
            }
        }

        if (mS.showWhatColor == "Disabled")
        {
            style.normal.textColor = Color.Lerp(Color.black, Color.green, .6f);
        }
        else
        {
            style.normal.textColor = Color.black;
        }

        if (GUILayout.Button("Show",style, GUILayout.MaxWidth(43)))
        {
            if (mS.showWhatColor == "")
            {
                if (tar.transform.GetComponent<Image>() != null)
                {
                    tar.Normal = tar.transform.GetComponent<Image>().color;
                }
                else if (mS.transform.GetComponent<Text>() != null)
                {
                    tar.Normal = tar.transform.GetComponent<Text>().color;
                }

                mS.showWhatColor = "Disabled";
            }
            else if (mS.showWhatColor != "Disabled")
            {
                mS.showWhatColor = "Disabled";
            }
            else
            {
                mS.showWhatColor = "";
            }
        }
        tar.Disabled = EditorGUILayout.ColorField(tar.Disabled, GUILayout.MaxWidth(100));
        EditorGUILayout.EndHorizontal();

        #endregion
    }

    void ShowWhatColor(Colorize tar)
    {
        Colorize mS = (Colorize)target;

        //Change Color
        switch (mS.showWhatColor)
        {
            case "Highlighted":
                if (tar.GetComponent<Image>() != null)
                {
                    tar.GetComponent<Image>().color = tar.Highlighted;
                }
                else if (tar.GetComponent<Text>() != null)
                {
                    tar.GetComponent<Text>().color = tar.Highlighted;
                }
                break;
            case "Pressed":
                if (tar.GetComponent<Image>() != null)
                {
                    tar.GetComponent<Image>().color = tar.Pressed;
                }
                else if (mS.transform.GetComponent<Text>() != null)
                {
                    tar.GetComponent<Text>().color = tar.Pressed;
                }
                break;
            case "Disabled":
                if (tar.GetComponent<Image>() != null)
                {
                    tar.GetComponent<Image>().color = tar.Disabled;
                }
                else if (tar.GetComponent<Text>() != null)
                {
                    tar.GetComponent<Text>().color = tar.Disabled;
                }
                break;
            default: //Show normal Color
                if (tar.GetComponent<Image>() != null)
                {
                    tar.GetComponent<Image>().color = tar.Normal;
                }
                else if (tar.GetComponent<Text>() != null)
                {
                    tar.GetComponent<Text>().color = tar.Normal;
                }
                break;
        }
    }*/
}
