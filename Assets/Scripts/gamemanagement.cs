using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gamemanagement : MonoBehaviour {
    public int m_NumRoundsToWin = 5;
    public float m_StartDelay = 3f;
    public float m_EndDelay = 3f;
    public Text m_MessageText;
    public GameObject m_PlayerPrefab;
    public PlayerManager[] m_Players;
    public Transform deathPlane;

    int m_RoundNumber;
    WaitForSeconds m_StartWait;
    WaitForSeconds m_EndWait;
    PlayerManager m_RoundWinner;
    PlayerManager m_GameWinner;

    // Use this for initialization
    void Start () {
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        SpawnPlayers();

        StartCoroutine(GameLoop());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void SpawnPlayers()
    {
        for (int i = 0; i < m_Players.Length; i++)
        {
            m_Players[i].m_Instance =
                Instantiate(m_PlayerPrefab, m_Players[i].m_SpawnPoint.position, m_Players[i].m_SpawnPoint.rotation) as GameObject;
            m_Players[i].m_PlayerNumber = i + 1;
            m_Players[i].Setup();
        }
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        if (m_GameWinner != null)
        {
            SceneManager.LoadScene(0); 
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }

    private IEnumerator RoundStarting()
    {
        ResetAllPlayers();
        DisablePlayersControl();

        m_RoundNumber++;
        m_MessageText.text = "ROUND " + m_RoundNumber + " " + m_Players[1].m_Wins + " : " + m_Players[0].m_Wins;

        yield return m_StartWait;
    }


    private IEnumerator RoundPlaying()
    {
        EnablePlayersControl();

        m_MessageText.text = string.Empty;

        while (!OnePlayerLeft())
        {
            yield return null;
        }
    }


    private IEnumerator RoundEnding()
    {
        DisablePlayersControl();

        m_RoundWinner = null;

        m_RoundWinner = GetRoundWinner();

        if (m_RoundWinner != null)
            m_RoundWinner.m_Wins++;

        m_GameWinner = GetGameWinner();

        m_MessageText.text = EndMessage();

        yield return m_EndWait;
    }


    private bool OnePlayerLeft()
    {
        int numPlayersLeft = 0;

        for (int i = 0; i < m_Players.Length; i++)
        {
            //Debug.Log(Vector3.Dot(deathPlane.up, m_Players[i].m_Instance.GetComponent<Transform>().up));
            if (Vector3.Dot(deathPlane.up, m_Players[i].m_Instance.GetComponent<Rigidbody>().position - deathPlane.position) > 0.0f)
            {
                numPlayersLeft++;
            }
        }

        return numPlayersLeft <= 1;
    }


    private PlayerManager GetRoundWinner()
    {
        for (int i = 0; i < m_Players.Length; i++)
        {
            if (Vector3.Dot(deathPlane.up, m_Players[i].m_Instance.GetComponent<Rigidbody>().position - deathPlane.position) > 0.0f)
                return m_Players[i];
        }

        return null;
    }


    private PlayerManager GetGameWinner()
    {
        for (int i = 0; i < m_Players.Length; i++)
        {
            if (m_Players[i].m_Wins == m_NumRoundsToWin)
                return m_Players[i];
        }

        return null;
    }


    private string EndMessage()
    {
        string message = "DRAW!";

        if (m_RoundWinner != null)
            message = "Player " + m_RoundWinner.m_PlayerNumber + " WINS THE ROUND!";

        message += "\n\n\n\n";

        for (int i = 0; i < m_Players.Length; i++)
        {
            message += "Player " + i + ": " + m_Players[i].m_Wins + " WINS\n";
        }

        if (m_GameWinner != null)
            message = "Player " + m_GameWinner.m_PlayerNumber + " WINS THE GAME!";

        return message;
    }


    private void ResetAllPlayers()
    {
        for (int i = 0; i < m_Players.Length; i++)
        {
            m_Players[i].Reset();
        }
    }


    private void EnablePlayersControl()
    {
        for (int i = 0; i < m_Players.Length; i++)
        {
            m_Players[i].EnableControl();
        }
    }


    private void DisablePlayersControl()
    {
        for (int i = 0; i < m_Players.Length; i++)
        {
            m_Players[i].DisableControl();
        }
    }
}
