using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    private HeroControllerWithAnimations __player;
    private GameObject __interface;

    [SerializeField] private TextMeshProUGUI __ammo;
    [SerializeField] private TextMeshProUGUI __FPS;
    [SerializeField] private GameObject _winGameObject;
    [SerializeField] private RawImage __blackScreen;

    [SerializeField] private GameObject _basicCursor;
    [SerializeField] private GameObject _fistCursor;

    private float __deltaTime, __fps;
    private float _alpha = 0f;

    private HeroCamera __heroCamera;
    private void Awake()
    {
        __heroCamera = FindObjectOfType<HeroCamera>();
        __player = FindObjectOfType<HeroControllerWithAnimations>();           

        __interface = GameObject.FindGameObjectWithTag("UI");

        __ammo = __interface.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        __FPS = __interface.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _winGameObject = __interface.transform.GetChild(2).gameObject;
        __blackScreen = __interface.transform.GetChild(3).gameObject.GetComponent<RawImage>();
        _basicCursor = __interface.transform.GetChild(4).gameObject;
        _fistCursor = __interface.transform.GetChild(5).gameObject;
    }
    private void FixedUpdate()
    {        
        __ammo.text = "Ammo: " + __player.GetWeapon().GetComponent<Gun>().GetAmmo;

        if (__player.IsPlayerWon)
        {
            _winGameObject.transform.GetChild(0).gameObject.SetActive(true);
            _winGameObject.transform.GetChild(1).gameObject.SetActive(true);
            __player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            __player.ThrowWeapon();
            TimeManager.SetTimeScale(0.5f);
        }
        if (__player.IsPlayerDead)
        {
            _alpha += Time.fixedDeltaTime;
            __blackScreen.color = new Color(0, 0, 0, _alpha);
            //Debug.Log(__player.IsPlayerDead);
        }

        ChangeCursor();
    }
    private void ChangeCursor()
    {
        if (__heroCamera.GetHittedGameObjectOn3Meters() && __heroCamera.GetHittedGameObjectOn3Meters().CompareTag("Enemy") && __player.GetWeapon().GetName() == "Fists")
        {
            _basicCursor.SetActive(false);
            _fistCursor.SetActive(true);
        }
        else
        {
            _basicCursor.SetActive(true);
            _fistCursor.SetActive(false);
        }
    }
    public Color GetBlackScreenColor()
    {
        return __blackScreen.color;
    }
    public void SetBlackScreenColor( Color color)
    {
        __blackScreen.color = color;
    }

    private void Update()
    {
        ShowFPS();
    }
    private void ShowFPS()
    {
        __deltaTime += (Time.deltaTime - __deltaTime) * 0.1f;
        __fps = 1.0f / __deltaTime;
        __FPS.text = Mathf.Ceil(__fps).ToString();
    }
}
