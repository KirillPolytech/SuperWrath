using UnityEngine;

public class InGameTimer : MonoBehaviour
{
    private float __timer = 0f;
    private void Update()
    {
        __timer += Time.deltaTime;
    }
}
