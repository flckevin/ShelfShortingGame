#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(HorizontalLine))]

public class HorizontalLineDrawer : DecoratorDrawer
{
    public override float GetHeight()
    {
        HorizontalLine attr = attribute as HorizontalLine;
        return Mathf.Max(attr.padding,attr.thickness);
    }

    public override void OnGUI(Rect position)
    {
        HorizontalLine attr = attribute as HorizontalLine;

        position.height = attr.thickness;
        position.y += attr.padding * 0.5f;

        EditorGUI.DrawRect(position, EditorGUIUtility.isProSkin ? new Color(0.3f, 0.3f, 0.3f, 1) : new Color(0.7f, 0.7f, 0.7f, 1f));
    }
}
#endif
