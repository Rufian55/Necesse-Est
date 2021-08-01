using UnityEngine;
using UnityEngine.UI;
public class UIController : MonoBehaviour {

    public static UIController Instance = null;
    #pragma warning disable 0649
    [SerializeField] private Button toggleUI;
    [SerializeField] private Button leftSkybox;
    [SerializeField] private Button rightSkyBox;
    [SerializeField] private GameObject[] UIMembers;
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
        RemoveListeners();
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
        RenderSettings.skybox = ObjectAccessor.Instance.GetSkyBox(arg); // Will also set ObjectAccessor _skyboxIndex
        ObjectAccessor.Instance.SetBlackHoleProperties(10, Color.red);
        SwirlColorer.Instance.SetSwirlColors();                         // _skyboxIndex has already been set!
    }

    private void AddEventListeners() {
        toggleUI.onClick.AddListener(ToggleUI);
        leftSkybox.onClick.AddListener(delegate{ SwapSkyBox(false); });
        rightSkyBox.onClick.AddListener(delegate { SwapSkyBox(true); });
    }

    private void RemoveListeners() {
        toggleUI.onClick.RemoveListener(ToggleUI);
        leftSkybox.onClick.RemoveListener(delegate { SwapSkyBox(false); });
        rightSkyBox.onClick.RemoveListener(delegate { SwapSkyBox(true); });
    }

}
