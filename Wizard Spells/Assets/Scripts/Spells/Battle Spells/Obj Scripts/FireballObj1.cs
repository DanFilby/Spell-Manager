using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FireballObj1 : MonoBehaviour
{
    private UnityAction<Transform, bool> CallBack;
    private LayerMask detectionLayers;
    private Vector3 movVec; //doesnt work when private ??garbage collection??
    public Vector3 gravity;


    void Update()
    {
        transform.Translate(movVec * Time.deltaTime, Space.Self);
        movVec += gravity * Time.deltaTime;
    }

    public void InitialiseFireBall(float _speed, float _duration, LayerMask _detectionLayers, UnityAction<Transform, bool> _callBack)
    {
        movVec = Vector3.forward * _speed;
        gravity = new Vector3(0, -9.81f, 0) /(_duration);
        detectionLayers = _detectionLayers;
        CallBack = _callBack;
    }

    private void OnTriggerEnter(Collider other)
    {        
        if (detectionLayers == (detectionLayers | 1 << other.gameObject.layer))
        {
            //enemy transform
            CallBack.Invoke(other.transform, true);
        }
        else
        {
            //fireball transform
            CallBack.Invoke(transform, false);
        }
    }
}
