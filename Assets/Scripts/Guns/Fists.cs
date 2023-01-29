using UnityEngine;

public class Fists : Gun
{
    [SerializeField] private float _distanceToAttack = 4f;
    private HeroCamera _heroCamera;
    private void Start()
    {
        _heroCamera = Camera.main.GetComponent<HeroCamera>();
    }
    public override void Attack()
    {
        if (_heroCamera.GetHittedGameObjectOn3Meters() && _heroCamera.GetHittedGameObjectOn3Meters().CompareTag("Enemy"))
        {
            _heroCamera.GetHittedGameObjectOn3Meters().GetComponent<EnemyTemplate>().GetDamage(100);
            Debug.Log("Deal Damage");
        }
    }
    public override string GetName()
    {
        return GetType().ToString();
    }

    public override void BeingThrown(float throwingForce, Vector3 direction)
    {

    }

    public override void ChangeColor(Color color)
    {

    }

    public override void ReturnInitialColor()
    {

    }

    public override void UnParent()
    {
        
    }
}
