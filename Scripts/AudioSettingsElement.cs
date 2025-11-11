using UnityEngine;
using UnityEngine.UIElements;
using static TCS.Options.GeneralSettingsNames;

namespace TCS.Options {
    #region Base Class
    public abstract class SettingsSectionElement : VisualElement {
        protected readonly VisualElement HeaderContainer;
        protected readonly Label HeaderLabel;
        protected readonly VisualElement SettingsContainer;

        protected VisualElement BackButtonContainer;
        public Button BackButton;

        protected SettingsSectionElement(string sectionTitle, string ussRootClass) {
            AddToClassList( ussRootClass );

            HeaderContainer = new VisualElement();
            HeaderContainer.AddToClassList( HeaderContainerName );
            Add( HeaderContainer );

            HeaderLabel = new Label( sectionTitle );
            HeaderLabel.AddToClassList( HeaderLabelName );
            HeaderContainer.Add( HeaderLabel );

            SettingsContainer = new VisualElement();
            SettingsContainer.AddToClassList( SettingsContainerName );
            Add( SettingsContainer );
        }

        protected void AddBackButton() {
            BackButtonContainer = new VisualElement {
                name = "BackButtonContainer",
            };

            BackButtonContainer.AddToClassList( BackButtonContainerName );
            Add( BackButtonContainer );

            BackButton = new Button { text = "Back" };
            BackButton.AddToClassList( BackButtonName );
            BackButtonContainer.Add( BackButton );
        }

        protected void AddSetting<T>(T element, string className) where T : VisualElement {
            element.AddToClassList( className );
            SettingsContainer.Add( element );
        }
    }
    #endregion

    [UxmlElement] public partial class VideoSettingsElement : SettingsSectionElement {
        // USS class names specific to this element
        public static readonly string USSClassName = "video-settings";
        public static readonly string AcceptChangesButtonName = "accept-changes-button";

        public DropdownField ResolutionDropdown { get; } = new("Resolution");
        public Toggle FullscreenToggle { get; } = new("Fullscreen");
        public Toggle VSyncToggle { get; } = new("V-Sync");
        public Toggle DepthOfFieldToggle { get; } = new("Depth of Field");
        public Button AcceptChangesButton { get; }

        // Controls
        public VideoSettingsElement()
            : base( "Video Settings", USSClassName ) {
            AddSetting( ResolutionDropdown, SettingsDropdownName );
            AddSetting( FullscreenToggle, SettingsToggleName );
            AddSetting( VSyncToggle, SettingsToggleName );
            AddSetting( DepthOfFieldToggle, SettingsToggleName );

            AcceptChangesButton = new Button {
                text = "Accept Changes",
                tooltip = "Apply the changes made to video settings.",
            };
            AcceptChangesButton.AddToClassList( AcceptChangesButtonName );

            AddSetting( AcceptChangesButton, ButtonName );

            foreach (var labels in this.Query<Label>().ToList()) {
                labels.AddToClassList( SettingsLabelName );
            }
            
            ResolutionDropdown.value = "1920 x 1080";

            AddBackButton();
        }

        public void Init(Vector2[] resolutions, Vector2 currentResolution) {
            // Populate resolution dropdown
            foreach (var res in resolutions) {
                ResolutionDropdown.choices.Add( $"{(int)res.x} x {(int)res.y}" );
            }
            
            // Set current resolution
            var currentResString = $"{(int)currentResolution.x} x {(int)currentResolution.y}";
            if ( ResolutionDropdown.choices.Contains( currentResString ) ) {
                ResolutionDropdown.value = currentResString;
            }
        }
    }

    [UxmlElement] public partial class AudioSettingsElement : SettingsSectionElement {
        // USS class names specific to this element
        public static readonly string USSClassName = "audio-settings";

        const float VOLUME_MIN = 0f;
        const float VOLUME_MAX = 100f;
        const float DEFAULT_MASTER = 75f;
        const float DEFAULT_MUSIC = 60f;
        const float DEFAULT_SFX = 80f;

        // Controls
        public Slider MasterVolumeSlider { get; } = new("Master Volume", VOLUME_MIN, VOLUME_MAX);
        public Slider MusicVolumeSlider { get; } = new("Music Volume", VOLUME_MIN, VOLUME_MAX);
        public Slider sfxVolumeSlider { get; } = new("SFX Volume", VOLUME_MIN, VOLUME_MAX);

        public AudioSettingsElement()
            : base( "Audio Settings", USSClassName ) {
            ConfigureVolumeSlider( MasterVolumeSlider, DEFAULT_MASTER );
            ConfigureVolumeSlider( MusicVolumeSlider, DEFAULT_MUSIC );
            ConfigureVolumeSlider( sfxVolumeSlider, DEFAULT_SFX );

            AddSetting( MasterVolumeSlider, SettingsSliderName );
            AddSetting( MusicVolumeSlider, SettingsSliderName );
            AddSetting( sfxVolumeSlider, SettingsSliderName );

            foreach (var labels in this.Query<Label>().ToList()) {
                labels.AddToClassList( SettingsLabelName );
            }

            AddBackButton();
        }

        static void ConfigureVolumeSlider(Slider slider, float defaultValue) {
            slider.showInputField = true;
            slider.value = defaultValue;
        }
    }
}