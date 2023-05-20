using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class HoleMovement : MonoBehaviour
{
    public static HoleMovement Instance;
    
    public float speed;
    public Joystick joystick;
    public CinemachineVirtualCamera cam;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        float horizontalInput = joystick.Horizontal;
        float verticalInput = joystick.Vertical;

        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput) * speed * Time.deltaTime;
        
        transform.Translate(movement);
    }
    
    public void SetScale(float scaleValue)
    {
        transform.DOScale(
            new Vector3(transform.localScale.x + scaleValue, transform.localScale.y,
                transform.localScale.z + scaleValue), .2f);

        SetCameraPosition();
    }

    private Sequence camFollow;
    public void SetCameraPosition()
    {
        var transposer = cam.GetCinemachineComponent<CinemachineTransposer>();

        var camPos = transposer.m_FollowOffset;
        
        //if(camPos.y == 25f) return;
        
        camFollow?.Kill();
        camFollow = DOTween.Sequence();
        camFollow.Append(DOTween.To(() => transposer.m_FollowOffset, 
            x => transposer.m_FollowOffset = x, new Vector3(camPos.x,camPos.y + .5f,camPos.z - .5f),
            .5f));
    }
}
