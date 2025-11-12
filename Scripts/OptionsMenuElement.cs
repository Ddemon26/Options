using System;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;
using static TCS.Options.GeneralSettingsNames;
namespace TCS.Options {
    public enum OptionType { None, Choices, Audio, Video, InGame }

    public static class GeneralSettingsNames {
        // Root Names
        public static readonly string OptionsName = "options-settings";
        public static readonly string HeaderContainerName = OptionsName + "__header-container";
        public static readonly string HeaderLabelName = OptionsName + "__header-label";
        public static readonly string SettingsContainerName = OptionsName + "__settings-container";
        public static readonly string BackButtonContainerName = OptionsName + "__back-button-container";
        public static readonly string BackButtonName = OptionsName + "__back-button";
        public static readonly string ElementContainerName = OptionsName + "__element-container";
        public static readonly string SettingButtonName = OptionsName + "__setting-button";

        // Basic Setting Element Names
        public static readonly string SettingsLabelName = OptionsName + "__label";
        public static readonly string ButtonName = OptionsName + "__button";
        public static readonly string SettingsSliderName = OptionsName + "__slider";
        public static readonly string SettingsToggleName = OptionsName + "__toggle";
        public static readonly string SettingsDropdownName = OptionsName + "__dropdown";
    }

    [UxmlElement] public partial class OptionsMenuElement : VisualElement, IDisposable {
        // ReSharper disable once MemberCanBePrivate.Global
        public static readonly string ButtonContainerClass = OptionsName + "__button-container";

        readonly VisualElement m_optionsButtonContainer;
        readonly Button m_audioButton;
        readonly Button m_videoButton;
        readonly Button m_backButton;
        
        readonly VisualElement m_inGameButtonContainer;
        readonly Button m_resumeButton;
        readonly Button m_settingsButton;
        readonly Button m_quitButton;

        readonly VisualElement m_elementContainer;
        public AudioSettingsElement AudioSettings { get; }
        public VideoSettingsElement VideoSettings { get; }
        readonly Button[] m_returnBackButtons;
        OptionType m_currentOptionType = OptionType.None;

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
            m_optionsButtonContainer = new VisualElement {
                name = "ButtonContainer",
            };

            m_optionsButtonContainer.AddToClassList( ButtonContainerClass );
            Add( m_optionsButtonContainer );

            // Buttons
            m_audioButton = new Button { text = "Audio Settings" };
            m_audioButton.AddToClassList( ButtonName );
            m_audioButton.AddToClassList( SettingButtonName );
            m_optionsButtonContainer.Add( m_audioButton );
            m_videoButton = new Button { text = "Video Settings" };
            m_videoButton.AddToClassList( ButtonName );
            m_videoButton.AddToClassList( SettingButtonName );
            m_optionsButtonContainer.Add( m_videoButton );
            m_backButton = new Button { text = "Back" };
            m_backButton.AddToClassList( ButtonName );
            m_backButton.AddToClassList( SettingButtonName );
            m_optionsButtonContainer.Add( m_backButton );

            // In-Game Button Container
            m_inGameButtonContainer = new VisualElement {
                name = "InGameButtonContainer",
            };
            m_inGameButtonContainer.AddToClassList( ButtonContainerClass );
            ToggleElementVisibility( m_inGameButtonContainer, false );
            Add( m_inGameButtonContainer );

            // In-Game Buttons
            m_resumeButton = new Button { text = "Resume" };
            m_resumeButton.AddToClassList( ButtonName );
            m_resumeButton.AddToClassList( SettingButtonName );
            m_inGameButtonContainer.Add( m_resumeButton );
            m_settingsButton = new Button { text = "Settings" };
            m_settingsButton.AddToClassList( ButtonName );
            m_settingsButton.AddToClassList( SettingButtonName );
            m_inGameButtonContainer.Add( m_settingsButton );
            m_quitButton = new Button { text = "Quit" };
            m_quitButton.AddToClassList( ButtonName );
            m_quitButton.AddToClassList( SettingButtonName );
            m_inGameButtonContainer.Add( m_quitButton );

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

            // Option Choices Button Callbacks
            m_audioButton.clicked += HandleAudioPressed;
            m_videoButton.clicked += HandleVideoPressed;
            m_backButton.clicked += HandleBackPressed;

            // In-Game Button Callbacks
            m_resumeButton.clicked += HandleResumePressed;
            m_settingsButton.clicked += HandleInGamePressed;
            m_quitButton.clicked += HandleQuitPressed;

            VideoSettings.Init( OptionElementComponents.SupportedResolutions, currentResolution );
        }
        
        // Option Choices Handlers
        public void HandleReturnBackPressed() => CurrentOptionType = OptionType.Choices;
        void HandleBackPressed() => CurrentOptionType = OptionType.None;
        void HandleVideoPressed() => CurrentOptionType = OptionType.Video;
        void HandleAudioPressed() => CurrentOptionType = OptionType.Audio;
        // In-Game Handlers
        void HandleResumePressed() => CurrentOptionType = OptionType.None;
        public void HandleInGamePressed() => CurrentOptionType = OptionType.InGame;
        void HandleQuitPressed() => Debug.Log( "Quitting Application..." );

        public void Dispose() {
            foreach (var returnBackButton in m_returnBackButtons) {
                returnBackButton.clicked -= HandleReturnBackPressed;
            }

            m_audioButton.clicked -= HandleAudioPressed;
            m_videoButton.clicked -= HandleVideoPressed;
            m_backButton.clicked -= HandleBackPressed;

            m_resumeButton.clicked -= HandleResumePressed;
            m_settingsButton.clicked -= HandleReturnBackPressed;
            m_quitButton.clicked -= HandleQuitPressed;
        }

        void HandleMenuSelection(OptionType settings) {
            // Reset everything first
            ToggleElementVisibility( AudioSettings, false );
            ToggleElementVisibility( VideoSettings, false );
            ToggleElementVisibility( m_elementContainer, false );
            ToggleElementVisibility( m_optionsButtonContainer, false );
            ToggleElementVisibility( m_inGameButtonContainer, false );
            ToggleElementVisibility( this, false );

            switch (settings) {
                case OptionType.Audio:
                    ToggleElementVisibility( AudioSettings, true );
                    ToggleElementVisibility( m_elementContainer, true );
                    ToggleElementVisibility( this, true );
                    break;

                case OptionType.Video:
                    ToggleElementVisibility( VideoSettings, true );
                    ToggleElementVisibility( m_elementContainer, true );
                    ToggleElementVisibility( this, true );
                    break;

                case OptionType.Choices:
                    ToggleElementVisibility( m_optionsButtonContainer, true );
                    ToggleElementVisibility( this, true );
                    break;

                case OptionType.InGame:
                    ToggleElementVisibility( m_inGameButtonContainer, true );
                    ToggleElementVisibility( this, true );
                    break;

                case OptionType.None:
                default:
                    break;
            }
        }


        static void ToggleElementVisibility(VisualElement element, bool isVisible)
            => element.style.display = isVisible ? DisplayStyle.Flex : DisplayStyle.None;
    }
}