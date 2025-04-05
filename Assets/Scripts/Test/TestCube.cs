using System;
using UnityEngine;
using TMPro;

public class TestCube : MonoBehaviour
{
    public TMP_Text text;

    private void Start()
    {
        text = GetComponentInChildren<TMP_Text>();
        text.text = GetComponent<Cube>().GetInteractions()[0].Name;
    }
}
