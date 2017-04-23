using System;
using UnityEngine;

[Serializable]
public class PlayerManager
{
    public Transform m_SpawnPoint;
    [HideInInspector]
    public int m_PlayerNumber;
    [HideInInspector]
    public GameObject m_Instance;
    [HideInInspector]
    public int m_Wins;

    private Camera m_Camera;
    private move m_Movement;
    private collision m_Collision;

    public void Setup()
    {
        m_Movement = m_Instance.GetComponent<move>();
        m_Collision = m_Instance.GetComponent<collision>();
        m_Camera = m_Instance.GetComponentInChildren<Camera>();

        m_Movement.m_PlayerNumber = m_PlayerNumber;

        if (m_PlayerNumber == 2)
            m_Camera.rect = new Rect(0, 0, 0.5f, 1);
    }


    public void DisableControl()
    {
        m_Movement.enabled = false;
        m_Collision.enabled = false;
    }


    public void EnableControl()
    {
        m_Movement.enabled = true;
        m_Collision.enabled = true;
    }


    public void Reset()
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }
}
