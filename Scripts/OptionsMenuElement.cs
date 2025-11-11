using System;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;
using static TCS.Options.GeneralSettingsNames;
namespace TCS.Options {
    public enum OptionType { None, Choices, Audio, Video, }

    public static class GeneralSettingsNames {
        // Root Names
        public static readonly string OptionsName = "options-settings";
        public static readonly string HeaderContainerName = OptionsName + "__header-container";
        public static readonly string HeaderLabelName = OptionsName + "__header-label";
        public static readonly string SettingsContainerName = OptionsName + "__settings-container";
        public static readonly string BackButtonContainerName = OptionsName + "__back-button-container";
        public static readonly string BackButtonName = OptionsName + "__back-button";
        public static readonly string ElementContainerName = OptionsName + "__element-container";
        // Basic Setting Element Names
        public static readonly string SettingsLabelName = OptionsName + "__label";
        public static readonly string SettingsButtonName = OptionsName + "__button";
        public static readonly string SettingButtonName = OptionsName + "__setting-button";
        public static readonly string SettingsSliderName = OptionsName + "__slider";
        public static readonly string SettingsToggleName = OptionsName + "__toggle";
        public static readonly string SettingsDropdownName = OptionsName + "__dropdown";
    }

    [UxmlElement] public partial class OptionsMenuElement : VisualElement, IDisposable {
        public static readonly string ButtonContainerClass = OptionsName + "__button-container";

        readonly VisualElement m_buttonContainer;
        readonly Button m_audioButton;
        readonly Button m_videoButton;
        readonly Button m_backButton;

        OptionType m_currentOptionType = OptionType.None;

        readonly Button[] m_returnBackButtons;

        VisualElement m_elementContainer;

        public AudioSettingsElement AudioSettings { get; }
        public VideoSettingsElement VideoSettings { get; }

        [CreateProperty, UxmlAttribute( "current-option-type" )]
        public OptionType CurrentOptionType {
            get => m_currentOptionType;
            set {
                m_currentOptionType = value;
                NotifyPropertyChanged( nameof(CurrentOptionType) );
                HandleMenuSelection( m_currentOptionType );
            }
        }

        public OptionsMenuElement() {
            AddToClassList( OptionsName );

            // Button Container
            m_buttonContainer = new VisualElement {
                name = "ButtonContainer",
            };

            m_buttonContainer.AddToClassList( ButtonContainerClass );
            Add( m_buttonContainer );

            // Buttons
            m_audioButton = new Button { text = "Audio Settings" };
            m_audioButton.AddToClassList( SettingsButtonName );
            m_audioButton.AddToClassList( SettingButtonName );
            m_buttonContainer.Add( m_audioButton );
            m_videoButton = new Button { text = "Video Settings" };
            m_videoButton.AddToClassList( SettingsButtonName );
            m_videoButton.AddToClassList( SettingButtonName );
            m_buttonContainer.Add( m_videoButton );
            m_backButton = new Button { text = "Back" };
            m_backButton.AddToClassList( SettingsButtonName );
            m_backButton.AddToClassList( SettingButtonName );
            m_buttonContainer.Add( m_backButton );

            m_elementContainer = new VisualElement {
                name = "ElementContainer",
            };
            m_elementContainer.AddToClassList( ElementContainerName );
            Add( m_elementContainer );

            // Component Elements
            AudioSettings = new AudioSettingsElement();
            m_elementContainer.Add( AudioSettings );

            VideoSettings = new VideoSettingsElement();
            m_elementContainer.Add( VideoSettings );

            m_returnBackButtons = new[] {
                AudioSettings.BackButton,
                VideoSettings.BackButton,
            };
        }

        public void Init(Vector2 currentResolution) {
            CurrentOptionType = OptionType.None;

            foreach (var returnBackButton in m_returnBackButtons) {
                returnBackButton.clicked += HandleReturnBackPressed;
            }

            // Button Callbacks
            m_audioButton.clicked += HandleAudioPressed;
            m_videoButton.clicked += HandleVideoPressed;
            m_backButton.clicked += HandleBackPressed;
            
            VideoSettings.Init( OptionElementComponents.SupportedResolutions, currentResolution );
        }

        public void HandleReturnBackPressed() => CurrentOptionType = OptionType.Choices;
        void HandleBackPressed() => CurrentOptionType = OptionType.None;
        void HandleVideoPressed() => CurrentOptionType = OptionType.Video;
        void HandleAudioPressed() => CurrentOptionType = OptionType.Audio;

        public void Dispose() {
            foreach (var returnBackButton in m_returnBackButtons) {
                returnBackButton.clicked -= HandleReturnBackPressed;
            }

            m_audioButton.clicked -= HandleAudioPressed;
            m_videoButton.clicked -= HandleVideoPressed;
            m_backButton.clicked -= HandleBackPressed;
        }

        void HandleMenuSelection(OptionType settings) {
            switch (settings) {
                case OptionType.None:
                    ToggleElementVisibility( AudioSettings, false );
                    ToggleElementVisibility( VideoSettings, false );
                    ToggleElementVisibility( m_elementContainer, false );
                    ToggleElementVisibility( m_buttonContainer, false );
                    ToggleElementVisibility( this, false );
                    break;
                case OptionType.Audio:
                    ToggleElementVisibility( AudioSettings, true );
                    ToggleElementVisibility( VideoSettings, false );
                    ToggleElementVisibility( m_elementContainer, true );
                    ToggleElementVisibility( m_buttonContainer, false );
                    ToggleElementVisibility( this, true );
                    break;
                case OptionType.Video:
                    ToggleElementVisibility( AudioSettings, false );
                    ToggleElementVisibility( VideoSettings, true );
                    ToggleElementVisibility( m_elementContainer, true );
                    ToggleElementVisibility( m_buttonContainer, false );
                    ToggleElementVisibility( this, true );
                    break;
                case OptionType.Choices:
                    ToggleElementVisibility( AudioSettings, false );
                    ToggleElementVisibility( VideoSettings, false );
                    ToggleElementVisibility( m_elementContainer, false );
                    ToggleElementVisibility( m_buttonContainer, true );
                    ToggleElementVisibility( this, true );
                    break;
                default:
                    ToggleElementVisibility( AudioSettings, false );
                    ToggleElementVisibility( VideoSettings, false );
                    ToggleElementVisibility( m_elementContainer, false );
                    ToggleElementVisibility( m_buttonContainer, false );
                    ToggleElementVisibility( this, false );
                    break;
            }
        }

        static void ToggleElementVisibility(VisualElement element, bool isVisible)
            => element.style.display = isVisible ? DisplayStyle.Flex : DisplayStyle.None;
    }
}