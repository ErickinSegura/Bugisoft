using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personajes : MonoBehaviour
{
    private void Awake()
    {
        PlayerPrefs.SetInt("Personaje", 0);
    }

    public void SelectPersonaje(int id)
    {
        PlayerPrefs.SetInt("Personaje", id);
        Debug.Log(PlayerPrefs.GetInt("Personaje"));
        gameObject.SetActive(false);
    }
}
