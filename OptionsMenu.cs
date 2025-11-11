using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
namespace TCS.Options {
    [DefaultExecutionOrder( -999 )]
    public class OptionsMenu : MonoBehaviour {
        [SerializeField] UIDocument m_uiDocument;
        [SerializeField] AudioMixer m_audioMixer;
        [SerializeField] VolumeProfile m_volumeProfile;
        public static OptionsMenu Instance { get; private set; }
        public OptionsMenuElement OptionsMenuElement { get; private set; }
        public OptionElementComponents ElementComponents { get; private set; }
        public OptionSettings OptionsSettings { get; private set; }
        public SettingValues SettingValues { get; private set; }

        void Awake() {
            if ( Instance != null && Instance != this ) {
                Destroy( gameObject );
                return;
            }

            Instance = this;

            transform.parent = null;
            DontDestroyOnLoad( this );

            if ( m_uiDocument == null ) {
                m_uiDocument = GetComponent<UIDocument>();
                if ( m_uiDocument == null ) {
                    Debug.LogError( "UIDocument is not assigned in OptionsMenu." );
                    enabled = false;
                    return;
                }
            }

            var root = m_uiDocument.rootVisualElement;
            OptionsMenuElement = root.Q<OptionsMenuElement>();

            ElementComponents = new OptionElementComponents {
                MasterVolumeSlider = OptionsMenuElement.AudioSettings.MasterVolumeSlider,
                MusicVolumeSlider = OptionsMenuElement.AudioSettings.MusicVolumeSlider,
                SfxVolumeSlider = OptionsMenuElement.AudioSettings.sfxVolumeSlider,
                AudioMixer = m_audioMixer,
                ResolutionDropdown = OptionsMenuElement.VideoSettings.ResolutionDropdown,
                FullscreenToggle = OptionsMenuElement.VideoSettings.FullscreenToggle,
                VSyncToggle = OptionsMenuElement.VideoSettings.VSyncToggle,
                DepthOfFieldToggle = OptionsMenuElement.VideoSettings.DepthOfFieldToggle,
                AcceptChangesButton = OptionsMenuElement.VideoSettings.AcceptChangesButton,
            };
            
            if ( m_volumeProfile != null ) {
                ElementComponents.VolumeProfile = m_volumeProfile;
            }else {
                Debug.LogError( "VolumeProfile is not assigned in OptionsMenu." );
            }

            SettingValues = new SettingValues();
            OptionsSettings = new OptionSettings( ElementComponents, SettingValues );
        }

        void Start() {
            OptionsSettings.Init();
            OptionsMenuElement.Init(SettingValues.videoValues.GetResolutionVector2());
        }

        void OnDestroy() {
            OptionsMenuElement.Dispose();
            OptionsSettings.Dispose();
        }

        public void OpenOptionMenu() => OptionsMenuElement.HandleReturnBackPressed();
    }
}