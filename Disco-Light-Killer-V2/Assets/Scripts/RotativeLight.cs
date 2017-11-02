using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotativeLight : MonoBehaviour {

    [SerializeField] float m_Speed;
    [SerializeField] Vector3 m_RotationAxes;

    // Update is called once per frame
    void Update ()
    {
        transform.Rotate(m_RotationAxes * m_Speed);
	}
}
