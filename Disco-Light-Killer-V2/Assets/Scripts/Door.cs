using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    [SerializeField] private bool m_IsOpen;
    [SerializeField] private Animation m_Animation;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Open()
    {
        m_IsOpen = true;
        m_Animation.Play("open");
    }

    public void Close()
    {
        m_IsOpen = false;
        m_Animation.Play("close");
    }
}
