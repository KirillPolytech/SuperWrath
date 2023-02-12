using UnityEngine;

public abstract class Gun : MonoBehaviour, IChangeColor
{
    [SerializeField] protected Vector3 _gunPosition = new Vector3(0.22f, 1.32f, 0.42f);
    protected Color _initialColor;
    protected Renderer _renderer;
    public abstract void Attack();
    public abstract void BeingThrown(float throwingForce, Vector3 direction);
    public abstract string GetName();
    public abstract void UnParent();
    public virtual void ChangeColor(Color color)
    {
        Debug.Log("not overrided");
        //throw new System.NotImplementedException();
    }

    public virtual void ReturnColor()
    {
       // throw new System.NotImplementedException();
    }
}
//public abstract void ReturnInitialColor();
//public abstract void ChangeColor(Color color);