using UnityEngine;
using System.Collections.Generic;

public class ObjectsManager : MonoBehaviour
{
    [SerializeField] private int _amountOfObjects = 0;
    [SerializeField] private int _amountOfBullets = 0;
    private void Awake()
    {
        GameObject[] _props = GameObject.FindGameObjectsWithTag("Prop");
        foreach (var prop in _props)
        {
            if (!prop.GetComponent<Prop>())
                prop.gameObject.AddComponent<Prop>();
            if (!prop.GetComponent<BoxCollider>())
                prop.gameObject.AddComponent<BoxCollider>();
        }

        ObjectTimeScale[] _tempObjectTimeScales = FindObjectsOfType<ObjectTimeScale>();
        foreach (var obj in _tempObjectTimeScales)
        {
            AddObjectToList(obj.GetComponent<ObjectTimeScale>());
        }
    }

    private LinkedList<ObjectProperties> _objects = new LinkedList<ObjectProperties>();
    public void AddObjectToList( ObjectTimeScale objectTimeScale)
    {
        _objects.AddLast(new ObjectProperties(_amountOfObjects++, objectTimeScale));
    }

    private LinkedList<BulletProperies> _bulletStats = new LinkedList<BulletProperies>();
    public void AddBulletToList(GameObject bullet)
    {
        _bulletStats.AddLast(new BulletProperies(_amountOfBullets++, bullet.GetComponent<Bullet>(), bullet));
    }

    public void ModifyTimeScale()
    {
        foreach (var bullet in _bulletStats)
        {
            if (bullet.GetBullet)
            {
                bullet.GetBullet.SetTimeScale();
            }
        }

        foreach (var obj in _objects)
        {
            obj.GetObjectTimeScale.SetTimeScale();
        }
    }

    private float _tempTimeScale = 0f;
    private void FixedUpdate()
    {
        if (TimeManager.GetTimeScale() > 0)
        {
            ModifyTimeScale();
            _tempTimeScale = 1;
            //Debug.Log("Modify1 " + TimeManager.GetTimeScale());
        }
        else if (_tempTimeScale == 1)
        {
            ModifyTimeScale();
            _tempTimeScale = 0;
            //Debug.Log("Modify2 " + TimeManager.GetTimeScale());
        }
    }
};
public struct BulletProperies
{
    private int _index;
    private Bullet _bullet;
    public int GetIndex {get { return _index; }}
    public Bullet GetBullet    {        get { return _bullet; }    }
    public BulletProperies(int index, Bullet bullet, GameObject bulletGameObject)
    {
        _index = index;
        _bullet = bullet;
    }
};

public struct ObjectProperties
{
    private int _index;
    private ObjectTimeScale _objectTimeScale;
    public ObjectProperties(int index, ObjectTimeScale objectTimeScale)
    {
        _index = index;
        _objectTimeScale = objectTimeScale;
    }
    public ObjectTimeScale GetObjectTimeScale { get { return _objectTimeScale; } } 
};

//     private float _timeScaleValue = 1;
/*
int i = 0, j = 0;
foreach (var bullet in _bullets)
{
    foreach (var velocity in _velocities)
    {
        if (i == j)
        {
            bullet.GetComponent<Rigidbody>().velocity = velocity * TimeManager.GetTimeScale();
        }
        j++;
    }
    i++;
}
*/

/*
 *     //private LinkedList<Vector3> _velocities = new LinkedList<Vector3>();
    //private LinkedList<Bullet> _bulletsts = new LinkedList<Bullet>();
foreach (var bullet in _bulletsts)
{
    bullet.SetTimeScale();
}
*/

/*
if ( Mathf.Abs(TimeManager.GetTimeScale() - _timeScaleValue) > 0.02f)
{
    ModifyTimeScale();
    _timeScaleValue = TimeManager.GetTimeScale();
}
*/

/*
private GameObject _object;
private Vector3 _velocityOfObject;
private Vector3 _angularVelocityOfObject;

public int GetIndex
{
    get { return _index; }
}
public Vector3 GetOrSetObjectVelocity
{
    get { return _velocityOfObject; }
    set { _velocityOfObject = value; }
}
public Vector3 GetOrSetObjectAngularVelocity
{
    get { return _angularVelocityOfObject; }
    set { _angularVelocityOfObject = value; }
}
public ObjectProperties(int index, GameObject currentObject, Vector3 velocity, Vector3 angularVelocity, Prop prop)
{
    _velocityOfObject = velocity;
    _angularVelocityOfObject = angularVelocity;
    _index = index;
    _object = currentObject;
    _prop = prop;
}
*/

/*
GameObject[] _tempProps = GameObject.FindGameObjectsWithTag("Prop");
GameObject[] _tempGuns = GameObject.FindGameObjectsWithTag("Gun");
foreach (var obj in _tempProps)
    if (obj.GetComponent<Rigidbody>())
        AddObjectToList(obj.GetComponent<Prop>());

foreach (var gun in _tempGuns)
    if (!gun.GetComponent<Gun>().enabled)
        AddObjectToList(gun.GetComponent<Prop>());
*/