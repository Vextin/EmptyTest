using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    //The Transform we wish to copy the position of
    [SerializeField] private Transform _target;

    [SerializeField] private bool _followPosition;
    [SerializeField] private bool _followYRotation;

    // Update is called once per frame
    void Update()
    {
        //If we forgot to set the target, don't error out - just do nothing.
        if(_target is null) return;

        if(_followPosition) 
        { 
            transform.position = _target.position;
        }
        if(_followYRotation)
        {
            //Keep this object's own rotation, except for the Y-rotation, which is copied from the target.
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, _target.eulerAngles.y, transform.localEulerAngles.z);
        }
    }
}
