using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndOfGame : MonoBehaviour {
    [SerializeField]
    private Text m_messageBox;
    private float m_timeElapsed;
    private float m_timeBeforeLeave = 5;
    private Enemy[] m_enemies;

	// Use this for initialization
	void Start () {
        GameObject[] gameobjects = GameObject.FindGameObjectsWithTag("enemy");
        m_enemies = new Enemy[gameobjects.Length];
        for (int i = 0; i < gameobjects.Length; ++i)
            m_enemies[i] = gameobjects[i].GetComponent<Enemy>();

        m_timeElapsed = 0;

    }
	
	// Update is called once per frame
	void Update ()
    {
        int nbAlive = 0;
		for(int i = 0; i < m_enemies.Length; ++i)
        {
            if (!m_enemies[i].IsDead())
                nbAlive++;
        }

        if (nbAlive == 0)
        {
            m_messageBox.text = "Congratulation !\nYou've killed all the zombies.\nThanks for playing ! ";
            m_timeElapsed += Time.deltaTime;
            if (m_timeBeforeLeave <= m_timeElapsed)
                Application.Quit();
        }

        if (Input.GetKeyDown("escape"))
            Application.Quit();

    }
}
