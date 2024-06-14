using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlendShapeChange : MonoBehaviour
{
    public SkinnedMeshRenderer SkinnedMeshRenderer;
    public List<Slider> Sliders = new List<Slider>();
    void Start()
    {
        SliderChange();
    }
    private void SliderChange()
    {
        for (int i = 0; i < Sliders.Count; i++)
        {
            int index = i;
            Sliders[index].onValueChanged.AddListener((value) =>
            {
                Change(index, value);
            });
        }
    }
    public void Change(int index, float value)
    {
        SkinnedMeshRenderer.SetBlendShapeWeight(index, value);//…Ë÷√BlendValue
    }
    // Update is called once per frame
    void Update()
    {

    }
}
