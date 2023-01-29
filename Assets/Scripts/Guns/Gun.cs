using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    [SerializeField] protected Vector3 _gunPosition;
    [SerializeField] protected int _ammo = 0;

    protected Color _initialColor = Color.white;
    protected Renderer _renderer;
    public int GetAmmo { get { return _ammo; } }
    public abstract  void Attack();

    public int GetCurrentAmmo
    {
        get {return _ammo; }
    }
    public abstract void BeingThrown(float throwingForce, Vector3 direction);

    public abstract void ChangeColor(Color color);
    public abstract void ReturnInitialColor();
    public abstract string GetName();
    public abstract void UnParent();
}
