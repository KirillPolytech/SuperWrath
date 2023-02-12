using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    private HeroController __player;
    private GameObject __interface;

    [SerializeField] private TextMeshProUGUI __ammo;
    [SerializeField] private TextMeshProUGUI __FPS;
    [SerializeField] private GameObject _winGameObject;
    [SerializeField] private RawImage __blackScreen;
    [SerializeField] private RawImage __redScreen;
    [SerializeField] private TextMeshProUGUI __currentTimeScale;
    [SerializeField] private GameObject __inGameMenu;

    [SerializeField] private GameObject _basicCursor;
    [SerializeField] private GameObject _fistCursor;

    private float __deltaTime, __fps;

    private HeroCamera __heroCamera;
    private HeroGunsController __gunsController;
    private void Awake()
    {
        __heroCamera = FindObjectOfType<HeroCamera>();
        __player = FindObjectOfType<HeroController>();
        __gunsController = FindObjectOfType<HeroGunsController>();           

        __interface = GameObject.FindGameObjectWithTag("UI");

        __ammo = __interface.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        __FPS = __interface.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _winGameObject = __interface.transform.GetChild(2).gameObject;
        __blackScreen = __interface.transform.GetChild(3).gameObject.GetComponent<RawImage>();
        __redScreen = __interface.transform.GetChild(4).gameObject.GetComponent<RawImage>();
        _basicCursor = __interface.transform.GetChild(5).gameObject;
        _fistCursor = __interface.transform.GetChild(6).gameObject;
        __currentTimeScale = __interface.transform.GetChild(7).gameObject.GetComponent<TextMeshProUGUI>();
        __inGameMenu = __interface.transform.GetChild(8).gameObject;
    }
    private void FixedUpdate()
    {        
        if(__gunsController.GetFireArmWeapon)
            __ammo.text = "Ammo: " + __gunsController.GetFireArmWeapon.GetAmmo;
        else       
            __ammo.text = "Melee";

        __currentTimeScale.text = "TimeScale: " + TimeManager.GetTimeScale().ToString("f2");

        if (__player.IsPlayerWon)
        {
            _winGameObject.transform.GetChild(0).gameObject.SetActive(true);
            _winGameObject.transform.GetChild(1).gameObject.SetActive(true);
            //__player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            __gunsController.ThrowWeapon();
            TimeManager.SetTimeScale(0.25f);
        }
        ChangeCursor();
    }
    private void ChangeCursor()
    {
        if (__heroCamera.GetHittedGameObject(3) && __heroCamera.GetHittedGameObject(3).CompareTag("Enemy") && __gunsController.GetMeleeWeapon && __gunsController.GetMeleeWeapon.GetName() == "Fists")
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
    public void SetRedScreenColor(Color color)
    {
        __redScreen.color = color;
    }
    public RawImage GetRedScreenImage()
    {
        return __redScreen;
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
