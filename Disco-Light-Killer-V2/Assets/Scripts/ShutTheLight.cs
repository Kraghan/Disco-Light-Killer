using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class ShutTheLight : MonoBehaviour {
    private Light m_light;

    void Start()
    {
        m_light = GetComponent<Light>();
    }

	// Update is called once per frame
	void Update ()
    {
        if(false && Input.GetButtonDown("LightsOut"))
        {
            if (m_light.intensity == 1)
                m_light.intensity = 0;
            else if (m_light.intensity == 0)
                m_light.intensity = 1;
        }
	}
}
