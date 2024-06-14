using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;

public class MouseDraw : MonoBehaviour
{
    private Vector3 startMousePosition;//����϶���ʼ����
    private Vector3 endMousePosition;//����϶���������
    private bool isDragging = false;//�Ƿ����϶�

    private List<GameObject> selectedUnits = new List<GameObject>();//��⵽�ķ�Χ�ڵĶ���
    public LayerMask unitLayerMask; // ���ڼ�ⵥλ�Ĳ�

    void Update()
    {
        
        // ��갴��ʱ��¼��ʼλ��
        if (Input.GetMouseButtonDown(0))
        {
            startMousePosition = Input.mousePosition;//��¼��ʼλ��
            isDragging = true;
        }

        // ���̧��ʱֹͣ�϶�������ѡ�е�λ
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            SelectUnits();
        }

        // ���½���λ��
        if (isDragging)
        {
            endMousePosition = Input.mousePosition;
        }
    }

    void OnGUI()
    {
      
        // ����ѡ���
        if (isDragging)
        {
            Rect rect = GetScreenRect(startMousePosition, endMousePosition);
            DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }

    // ��ȡ��Ļ����
    Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
    {
        screenPosition1.y = Screen.height - screenPosition1.y;//��unity ��Ļ����ϵת����GUi����ϵ
        screenPosition2.y = Screen.height - screenPosition2.y;
        Debug.Log("new" + screenPosition2.y);
        Vector3 topLeft = Vector3.Min(screenPosition1, screenPosition2);
        Vector3 bottomRight = Vector3.Max(screenPosition1, screenPosition2);
        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }

    // ���ƾ���
    void DrawScreenRect(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, Texture2D.whiteTexture);//ָ�������������
        GUI.color = Color.white;
    }

    // ���ƾ��α߿�
    /// <summary>
    /// �ĸ���������ĸ���
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="thickness">���</param>
    /// <param name="color"></param>
    void DrawScreenRectBorder(Rect rect, float thickness, Color color)
    {
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
    }

    // ѡ����ڵĵ�λ
    void SelectUnits()
    {
        // ��յ�ǰѡ�е�λ
        selectedUnits.Clear();

        // ��ȡѡ�����������귶Χ
        Bounds selectionBounds = GetViewportBounds(Camera.main, startMousePosition, endMousePosition);
        Debug.Log("lod" + endMousePosition.y);
        try
        {
            // �������е�λ���ж��Ƿ���ѡ�����
            foreach (var unit in GameObject.FindGameObjectsWithTag("Unit"))
            {
                if (selectionBounds.Contains(Camera.main.WorldToViewportPoint(unit.transform.position)))
                {
                    selectedUnits.Add(unit);
                    // ����ѡ���߼�������ı���ɫ����ӱ��
                    unit.GetComponent<Renderer>().material.color = Color.red;
                }
                else
                {
                    // ����ȡ��ѡ���߼�������ָ�ԭ������ɫ
                    unit.GetComponent<Renderer>().material.color = Color.white;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log("�����ڻ�û�ж���");
        }


    }

    // ��ȡ�ӿ�����İ�Χ��
    Bounds GetViewportBounds(Camera camera, Vector3 screenPosition1, Vector3 screenPosition2)
    {
        Vector3 v1 = camera.ScreenToViewportPoint(screenPosition1);
        Vector3 v2 = camera.ScreenToViewportPoint(screenPosition2);
        Vector3 min = Vector3.Min(v1, v2);
        Vector3 max = Vector3.Max(v1, v2);
        min.z = camera.nearClipPlane;//��׶���λ��
        max.z = camera.farClipPlane;//��׶��Զλ��

        Bounds bounds = new Bounds();
        bounds.SetMinMax(min, max);//�ײ���������������������ĵ�
        return bounds;
    }
}
