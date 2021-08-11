/************************************************************************************************************************
 * UICOntroller.cs - manages UI objects and their event handlers. See AudioManager for additional UI controls.
 * Attached to HUDisplay in hierarchy.
 * NOTE: Due to recent regression (by Unity Technologies) to v2020.1.3, et. al., dynamic parameter passing from the
 * Inspector is no longer working, thus we resort to the bool 'arg' & 'showTimeControlsIsOn' work around method.  See:
 * h ttps://forum.unity.com/threads/unityevent-with-dynamic-parameters-not-showing-anymore-in-the-unity-inspector.746354/
 ************************************************************************************************************************/
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public static UIController Instance = null;

    #pragma warning disable 0649
    [SerializeField] private TMP_Text simName;
    [SerializeField] private Button leftSkybox;
    [SerializeField] private Button rightSkyBox;
    [SerializeField] private Button autoLeft;
    [SerializeField] private Button autoRight;
    [SerializeField] private TMP_Text autoTimeText;
    [SerializeField] private Slider time2NextIndex;
    [SerializeField] private TMP_Text lightSeconds;
    [SerializeField] private Button toggleUI;
    [SerializeField] private GameObject[] UIMembers;
    [SerializeField] private GameObject[] autoUIMembers;    // Workaround method - see ToggleUI()
    [SerializeField] private string[] galaxyNames;
    #pragma warning restore 0649

    private Coroutine autoIndexSimulation;
    private float _time2NextGalaxy;
    private bool pause;
    private bool autoIsRunning;
    private bool arg, showTimeControlsIsOn;                 // Workaround method - see ToggleUI()


    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    void Start() {
        _time2NextGalaxy = time2NextIndex.value;
        lightSeconds.text = "";
        ShowTimeControls(false);
    }

    void OnEnable() {
        AddEventListeners();
    }

    void OnDestroy() {
        RemoveEventListeners();
    }

    void Update() {
        // Quit <ESC>.
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
        // Pause <P>.
        if (Input.GetKeyDown(KeyCode.P)) {
            pause = !pause;
            if (pause) {
                Time.timeScale = 0;
            }
            else {
                Time.timeScale = 1f;
            }
        }
    }

    public void SetGalaxyNameOnStart(int arg) {
        simName.text = galaxyNames[arg];
    }

    // This would normally be called dynamically from Inspector. See Note in header comment.
    private void ToggleUI() {
        arg = !arg;
        for (int i = 0; i < UIMembers.Length; i++) {
            if (arg) {
                UIMembers[i].SetActive(false);
            }
            else {
                UIMembers[i].SetActive(true);
            }
        }
        // Activate/Deactivate Time Control UI objects.
        for (int i = 0; i < autoUIMembers.Length; i++) {
            if (!arg && showTimeControlsIsOn) {
                autoUIMembers[i].SetActive(true);
            }
            else {
                autoUIMembers[i].SetActive(false);
            }
        }
    }

    private void SwapSkyBox(bool direction) {
        int arg = ObjectAccessor.Instance.SkyBoxIndex;
        if (direction) {
            arg++;
        }
        else {
            arg--;
        }
        RenderSettings.skybox = ObjectAccessor.Instance.GetSkyBox(arg);     // Will also set ObjectAccessor _skyboxIndex
        ObjectAccessor.Instance.SetBlackHoleProperties();                   // Parameters handled at ObjectAccessor.
        SwirlColorer.Instance.SetSwirlColors();                             // _skyboxIndex has already been set!
        simName.text = galaxyNames[ObjectAccessor.Instance.SkyBoxIndex];
    }

    private void InitAutoIndexer(bool direction) {
        if (!autoIsRunning) {
            autoIndexSimulation = StartCoroutine(AutoIndexer(direction));
            ShowTimeControls(true);
            lightSeconds.gameObject.SetActive(true);
            lightSeconds.text = _time2NextGalaxy.ToString("N0");
        }
    }

    private void StopAutoIndexer() {
        if (autoIndexSimulation != null) {
            StopCoroutine(autoIndexSimulation);
            autoIsRunning = false;
            ShowTimeControls(false);           
        }
    }

    private IEnumerator AutoIndexer(bool direction) {
        autoIsRunning = true;
        while (true) {
            SwapSkyBox(direction);
            yield return new WaitForSeconds(_time2NextGalaxy);
        }
    }

    private void Time2NextIndexChange() {
        _time2NextGalaxy = time2NextIndex.value;
        lightSeconds.text = time2NextIndex.value.ToString("N0");
    }

    private void ShowTimeControls(bool showSliderAndTime) {
        if (showSliderAndTime) {
            time2NextIndex.gameObject.SetActive(true);
            lightSeconds.text = time2NextIndex.value.ToString("N0");
            autoTimeText.gameObject.SetActive(true);
            showTimeControlsIsOn = true;
        }
        else {
            time2NextIndex.gameObject.SetActive(false);
            lightSeconds.text = "";
            autoTimeText.gameObject.SetActive(false);
            showTimeControlsIsOn = false;
        }
    }

    private void AddEventListeners() {
        toggleUI.onClick.AddListener(ToggleUI);
        leftSkybox.onClick.AddListener(delegate { StopAutoIndexer(); SwapSkyBox(false); });
        rightSkyBox.onClick.AddListener(delegate { StopAutoIndexer(); SwapSkyBox(true); });
        autoLeft.onClick.AddListener(delegate { StopAutoIndexer(); InitAutoIndexer(false); });
        autoRight.onClick.AddListener(delegate { StopAutoIndexer(); InitAutoIndexer(true); });
        time2NextIndex.onValueChanged.AddListener(delegate { Time2NextIndexChange(); });
    }

    private void RemoveEventListeners() {
        toggleUI.onClick.RemoveListener(ToggleUI);
        leftSkybox.onClick.RemoveListener(delegate { StopAutoIndexer(); SwapSkyBox(false); });
        rightSkyBox.onClick.RemoveListener(delegate { StopAutoIndexer(); SwapSkyBox(true); });
        autoLeft.onClick.RemoveListener(delegate { StopAutoIndexer(); InitAutoIndexer(false); });
        autoRight.onClick.RemoveListener(delegate { StopAutoIndexer(); InitAutoIndexer(true); });
        time2NextIndex.onValueChanged.RemoveListener(delegate { Time2NextIndexChange(); });
    }

}
