using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartBagPlane : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Plane;
    public GameObject Data, Green;
    public Button DataPlane;
    public Button GreenPlane, OpenBagButton;
    public Button CloseButtonOne, CloseButtonTwo;
    public AnimationClip Open, Close;
    public Animation Animation;

    public Vector3 StartPos;
    public Vector3 EndPos;
    void Start()
    {
        if(OpenBagButton==null)
        {
            OpenBagButton = GameObject.Find("OpenBag").GetComponent<Button>();
        }

        EndPos = transform.localPosition;
        StartPos = new Vector3(0, 0, 0);
        DataPlane.onClick.AddListener(() =>
        {
            Data.SetActive(false);
            Green.SetActive(true);

        });
        GreenPlane.onClick.AddListener(() =>
        {
            Data.SetActive(true);
            Green.SetActive(false);

        });
        OpenBagButton.onClick.AddListener(() =>
        {
           // StartCoroutine(OpenBag(StartPos));
            Animation.clip = Open;
            Animation.Play();
           
        });
        CloseButtonOne.onClick.AddListener(() =>
        {
            //StartCoroutine(OpenBag(EndPos));
            Animation.clip = Close;
            Animation.Play();

        });
        CloseButtonTwo.onClick.AddListener(() =>
        {
           // StartCoroutine(OpenBag(EndPos));
            Animation.clip = Close;
            Animation.Play();

        });


    }
    IEnumerator OpenBag(Vector3 endPos)
    {
        Vector3 localEndPos = endPos;
        float timer = 0;
        while (Vector3.Distance(transform.localPosition, localEndPos) > 1f)
        {
            timer += Time.deltaTime / 10;
            transform.localPosition = Vector3.Lerp(transform.localPosition, localEndPos, timer);
            yield return null;
        }
        transform.localPosition = localEndPos;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
