using UnityEngine;

public abstract class FireArmWeapon : Gun
{
    [SerializeField] protected int _ammo = 0; // pistol = 7
    public int GetAmmo { get { return _ammo; } }
    public abstract override void BeingThrown(float throwingForce, Vector3 direction);
    public abstract override string GetName();
    public abstract override void UnParent();
}

//public abstract override void ChangeColor(Color color);

//public abstract override void ReturnInitialColor();