using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;

public class HoleManager : MonoBehaviour
{
    public static HoleManager Instance;
    
    public int score;
    
    // public GameObject gravityPoint;
    // public float gravityPower = 1f; // gravity power
    // public float gravityRange = 1f; // range of gravity
    //
    // public LayerMask layerMask; // determines which layer should be affected by gravity

    public Image progressBar;
    public float totalColorValue;
    public float maxProgressBarValue;
    
    public float distance;
    public GameObject center;

    public Collider[] colors;
    private Collider[] colorsInsideZone;
    private Collider[] colorsOutsideZone;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameObject[] colorObjects = GameObject.FindGameObjectsWithTag("Color");

        colors = new Collider[colorObjects.Length];
        for (int i = 0; i < colorObjects.Length; i++)
        {
            colors[i] = colorObjects[i].GetComponent<Collider>();
        }
    }

    void Update()
    {
        //ColorInsideZone();

        //Gravity(gravityPoint.transform.position,gravityRange,layerMask);


        colorsInsideZone = Physics.OverlapSphere(center.transform.position, distance);
        
        foreach (var hits in colorsInsideZone)
        {
            if (hits==null)
            {
                continue;
            }
            if (hits.gameObject.CompareTag("Color"))
            {
                hits.gameObject.layer = 14;
            }
        }
        
        colorsOutsideZone = colors.Except(colorsInsideZone).ToArray();
        
        foreach (var hits in colorsOutsideZone)
        {
            if (hits==null)
            {
                continue;
            }
            if (hits.gameObject.CompareTag("Color"))
            {
                hits.gameObject.layer = 13;
            }
        }
    }

    // public void ColorInsideZone()
    // {
    //     GameObject[] colorObjects = GameObject.FindGameObjectsWithTag("Color");
    //     
    //     foreach (var obj in colorObjects)
    //     {
    //         float distanceX = Mathf.Abs(center.transform.position.x - obj.transform.position.x);
    //         float distanceZ = Mathf.Abs(center.transform.position.z - obj.transform.position.z);
    //         
    //         if (distanceX <= distance && distanceZ <= distance)
    //         {
    //             if (obj.gameObject.CompareTag("Color"))
    //             {
    //                 colorsInsideZone.Add(obj.GetComponent<Collider>());   
    //             }
    //         }
    //         if (distanceX > distance || distanceZ > distance)
    //         {
    //             if (obj.gameObject.CompareTag("Color"))
    //             {
    //                 colorsOutsideZone.Add(obj.GetComponent<Collider>());
    //             }
    //         }
    //     }
    //
    //     for (int i = 0; i < colorsInsideZone.Count; i++)
    //     {
    //         colorsInsideZone[i].gameObject.layer = 14;
    //     }
    //     for (int i = 0; i < colorsOutsideZone.Count; i++)
    //     {
    //         colorsOutsideZone[i].gameObject.layer = 13;
    //     }
    // }

    public void LoadProgressbar(float colorValue, float colorScaleValue)
    {
        totalColorValue += colorValue;

        if (totalColorValue >= maxProgressBarValue)
        {
            //progressBar.fillAmount = 0;
            SetFillImage(0);
            totalColorValue = 0;
            HoleMovement.Instance.SetScale(colorScaleValue);
        }
        else
        {
            SetFillImage(totalColorValue / maxProgressBarValue);
            //progressBar.fillAmount = totalColorValue / maxProgressBarValue;
        }
    }
    public void SetFillImage(float value, float duration = .25f, Ease ease = Ease.OutBack)
    {
        DOTween.Kill(progressBar);
        progressBar.DOFillAmount(value, 1).SetEase(ease).SetId(progressBar);
    }
    // private void Gravity(Vector3 gravitySource, float range, LayerMask layerMask)
    // {
    //     Collider[] objs = Physics.OverlapSphere(gravitySource, range, layerMask);
    //
    //     for (int i = 0; i < objs.Length; i++)
    //     {
    //         Rigidbody rbs = objs[i].GetComponent<Rigidbody>();
    //
    //         Vector3 forceDirection = gravitySource - objs[i].transform.position;
    //
    //         rbs.AddForceAtPosition(gravityPower * forceDirection.normalized, gravitySource);
    //     }
    // }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(center.transform.position, distance);
    }

    public void AddScore(int scoreValue)
    {
        score += scoreValue;
    }
}
