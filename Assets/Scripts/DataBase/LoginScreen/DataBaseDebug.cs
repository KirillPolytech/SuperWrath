using UnityEngine;
using System;

public class DataBaseDebug : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            AccesToDataBase.SetRecord( UnityEngine.Random.Range(0, int.MaxValue) , Convert.ToString(DateTime.Now), 3);
            Debug.Log("Setted Record");
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            AccesToDataBase.DeleteDataBase();
            Debug.Log("records deleted");
        }
    }
}
