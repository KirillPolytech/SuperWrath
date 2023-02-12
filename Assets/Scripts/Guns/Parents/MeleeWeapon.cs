
using UnityEngine;

public abstract class MeleeWeapon : Gun
{
    [Range(0,100)]
    [SerializeField] protected int _damage = 100;
    [SerializeField] protected GameObject _parentGameObject;
    public abstract override void BeingThrown(float throwingForce, Vector3 direction);
    public abstract override string GetName();
    public abstract override void UnParent();
    public abstract void SetParent(GameObject parent);
}
//public abstract override void ReturnInitialColor();
//public abstract override void ChangeColor(Color color);
