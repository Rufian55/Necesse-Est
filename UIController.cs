/****************************************************************
 * UICOntroller.cs - manages UI objects and thier event handlers.
 * Attached to HUDisplay in hierarchy.
 ***************************************************************/
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIController : MonoBehaviour {

    public static UIController Instance = null;

    #pragma warning disable 0649
    [SerializeField] private Button toggleUI;
    [SerializeField] private Button leftSkybox;
    [SerializeField] private Button rightSkyBox;
    [SerializeField] private GameObject[] UIMembers;
    [SerializeField] private string[] galaxyNames;
    [SerializeField] private TMP_Text simName;
    #pragma warning restore 0649
    private bool pause;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
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

    private void ToggleUI() {
        for (int i = 0; i < UIMembers.Length; i++) {
            if (UIMembers[i].activeInHierarchy) {
                UIMembers[i].SetActive(false);
            }
            else {
                UIMembers[i].SetActive(true);
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

    private void AddEventListeners() {
        toggleUI.onClick.AddListener(ToggleUI);
        leftSkybox.onClick.AddListener(delegate{ SwapSkyBox(false); });
        rightSkyBox.onClick.AddListener(delegate { SwapSkyBox(true); });
    }

    private void RemoveEventListeners() {
        toggleUI.onClick.RemoveListener(ToggleUI);
        leftSkybox.onClick.RemoveListener(delegate { SwapSkyBox(false); });
        rightSkyBox.onClick.RemoveListener(delegate { SwapSkyBox(true); });
    }

}
