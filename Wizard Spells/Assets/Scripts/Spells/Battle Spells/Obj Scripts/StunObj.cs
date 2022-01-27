using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StunObj : MonoBehaviour
{
    private LayerMask DetectionLayers;
    private Vector3 movVec;
    private UnityAction<Transform> CallBack;

    void Update()
    {
        transform.Translate(movVec * Time.deltaTime, Space.Self);
    }

    public void InitialiseStuner(float _speed, LayerMask _detectionLayers ,UnityAction<Transform> _callBack)
    {
        movVec = Vector3.forward * _speed;
        CallBack = _callBack;
        DetectionLayers = _detectionLayers;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(DetectionLayers == (DetectionLayers | 1 << other.gameObject.layer))
        {
            CallBack.Invoke(other.transform);
        }
    }
}
