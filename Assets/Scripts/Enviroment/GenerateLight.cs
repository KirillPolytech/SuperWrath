using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class GenerateLight : MonoBehaviour
{
    [SerializeField] private bool _bool = false;
    GameObject[] _props;
    private void Update()
    {
        if (_bool)
            return;
        _props = GameObject.FindGameObjectsWithTag("Prop");
        foreach (var prop in _props)
        {
            //GameObjectUtility.SetStaticEditorFlags(prop.gameObject, StaticEditorFlags.ContributeGI);
        }
        _bool = true;
        Debug.Log("Done");
    }
}