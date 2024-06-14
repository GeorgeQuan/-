using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;

public class MouseDraw : MonoBehaviour
{
    private Vector3 startMousePosition;//鼠标拖动开始坐标
    private Vector3 endMousePosition;//鼠标拖动结束坐标
    private bool isDragging = false;//是否在拖动

    private List<GameObject> selectedUnits = new List<GameObject>();//检测到的范围内的对象
    public LayerMask unitLayerMask; // 用于检测单位的层

    void Update()
    {
        
        // 鼠标按下时记录起始位置
        if (Input.GetMouseButtonDown(0))
        {
            startMousePosition = Input.mousePosition;//记录开始位置
            isDragging = true;
        }

        // 鼠标抬起时停止拖动并处理选中单位
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            SelectUnits();
        }

        // 更新结束位置
        if (isDragging)
        {
            endMousePosition = Input.mousePosition;
        }
    }

    void OnGUI()
    {
      
        // 绘制选择框
        if (isDragging)
        {
            Rect rect = GetScreenRect(startMousePosition, endMousePosition);
            DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }

    // 获取屏幕矩形
    Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
    {
        screenPosition1.y = Screen.height - screenPosition1.y;//从unity 屏幕坐标系转换到GUi坐标系
        screenPosition2.y = Screen.height - screenPosition2.y;
        Debug.Log("new" + screenPosition2.y);
        Vector3 topLeft = Vector3.Min(screenPosition1, screenPosition2);
        Vector3 bottomRight = Vector3.Max(screenPosition1, screenPosition2);
        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }

    // 绘制矩形
    void DrawScreenRect(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, Texture2D.whiteTexture);//指定区域绘制纹理
        GUI.color = Color.white;
    }

    // 绘制矩形边框
    /// <summary>
    /// 四个矩阵就像四个边
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="thickness">厚度</param>
    /// <param name="color"></param>
    void DrawScreenRectBorder(Rect rect, float thickness, Color color)
    {
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
    }

    // 选择框内的单位
    void SelectUnits()
    {
        // 清空当前选中单位
        selectedUnits.Clear();

        // 获取选择框的世界坐标范围
        Bounds selectionBounds = GetViewportBounds(Camera.main, startMousePosition, endMousePosition);
        Debug.Log("lod" + endMousePosition.y);
        try
        {
            // 查找所有单位并判断是否在选择框内
            foreach (var unit in GameObject.FindGameObjectsWithTag("Unit"))
            {
                if (selectionBounds.Contains(Camera.main.WorldToViewportPoint(unit.transform.position)))
                {
                    selectedUnits.Add(unit);
                    // 处理选中逻辑，例如改变颜色或添加标记
                    unit.GetComponent<Renderer>().material.color = Color.red;
                }
                else
                {
                    // 处理取消选中逻辑，例如恢复原来的颜色
                    unit.GetComponent<Renderer>().material.color = Color.white;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log("场景内还没有对象");
        }


    }

    // 获取视口坐标的包围盒
    Bounds GetViewportBounds(Camera camera, Vector3 screenPosition1, Vector3 screenPosition2)
    {
        Vector3 v1 = camera.ScreenToViewportPoint(screenPosition1);
        Vector3 v2 = camera.ScreenToViewportPoint(screenPosition2);
        Vector3 min = Vector3.Min(v1, v2);
        Vector3 max = Vector3.Max(v1, v2);
        min.z = camera.nearClipPlane;//视锥最近位置
        max.z = camera.farClipPlane;//视锥最远位置

        Bounds bounds = new Bounds();
        bounds.SetMinMax(min, max);//底层会根据这两个点生成其余的点
        return bounds;
    }
}
