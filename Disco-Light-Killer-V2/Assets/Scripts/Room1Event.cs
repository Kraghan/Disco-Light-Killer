using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room1Event : MonoBehaviour {
    private bool m_Started = false;
    private bool m_StartedSecond = false;
    [SerializeField] private Light m_lightSpot;
    [SerializeField] private Light m_lightSpot2;
    [SerializeField] private Light m_lightSpot3;
    [SerializeField] private float m_TimeBetweenFlash = 0.2f;
    [SerializeField] private float m_TimeElapsedSinceLastFlash = 0.0f;
    [SerializeField] private uint m_NumberOfFlash = 10;
    [SerializeField] private float m_TimeElapsedInTheDark = 0.0f;
    [SerializeField] private float m_TimeInTheDark = 0.1f;
    [SerializeField] private bool m_InTheDark = false;
    [SerializeField] private float m_LightIntensity = 5;
    private AudioSource m_Light1Audio;
    [SerializeField] private AudioClip m_AlterLightSound;


    public void Launch()
    {
        m_Started = true;
        m_Light1Audio.clip = m_AlterLightSound;
        m_Light1Audio.Play();
    }

    public void LaunchSecond()
    {
        m_StartedSecond = true;
        m_Light1Audio.clip = m_AlterLightSound;
        m_Light1Audio.Play();
    }

    void Start ()
    {
        m_Light1Audio = m_lightSpot.GetComponent<AudioSource>();
        m_lightSpot2.gameObject.SetActive(false);
        m_lightSpot3.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (m_Started)
            ProcessFirstEvents();

        if (m_StartedSecond)
            ProcessSecondEvents();
        
	}

    private void ProcessSecondEvents()
    {

    }

    private void ProcessFirstEvents()
    {
        if (m_InTheDark)
        {
            m_lightSpot.intensity = 0;
            m_TimeElapsedInTheDark += Time.deltaTime;
            if (m_TimeElapsedInTheDark >= m_TimeInTheDark)
            {
                m_TimeElapsedInTheDark = 0.0f;
                m_InTheDark = false;
                --m_NumberOfFlash;
            }
            else
                return;
        }
        else
            m_lightSpot.intensity = m_LightIntensity;

        if (m_NumberOfFlash != 0)
        {
            m_TimeElapsedSinceLastFlash += Time.deltaTime;
            if (m_TimeElapsedSinceLastFlash >= m_TimeBetweenFlash)
            {
                m_TimeElapsedSinceLastFlash = 0.0f;
                m_TimeBetweenFlash *= 9.0f / 10.0f;
                m_InTheDark = true;
                //m_LightIntensity = m_lightSpot.intensity;

            }
        }
        else
        {
            m_Started = false;
            m_Light1Audio.Stop();
            m_lightSpot2.gameObject.SetActive(true);
        }
    }
}
