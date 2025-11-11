using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
namespace TCS.Options {
    #region Option Settings
    public class OptionSettings {
        readonly AudioSettings m_audioSettings;
        readonly VideoSettings m_videoSettings;
        readonly SaveSettings m_saveSettings = new();
        readonly SettingValues m_settingValues;

        public OptionSettings(OptionElementComponents elementComponents, SettingValues settingValues) {
            m_settingValues = settingValues;
            // Load saved settings
            m_settingValues = m_saveSettings.Load();
            m_audioSettings = new AudioSettings( elementComponents, m_settingValues.audioValues );
            m_videoSettings = new VideoSettings( elementComponents, m_settingValues.videoValues );
        }

        public void Init() {
            // Apply audio settings
            m_audioSettings.Init();

            // Apply video settings
            m_videoSettings.VideoValues.Resolution = new ResolutionData {
                Width = m_settingValues.videoValues.Resolution.Width,
                Height = m_settingValues.videoValues.Resolution.Height,
            };
            m_videoSettings.VideoValues.VSync = m_settingValues.videoValues.VSync;
            m_videoSettings.VideoValues.DepthOfField = m_settingValues.videoValues.DepthOfField;
        }

        public void Dispose() {
            SaveOptionSettings();
            m_audioSettings.Dispose();
            m_videoSettings.Dispose();
        }

        void SaveOptionSettings() {
            var audioValues = m_audioSettings.GetCurrentDecibelLevels();
            var videoValues = m_videoSettings.GetCurrentVideoValues();

            var values = new SettingValues {
                audioValues = new AudioValues {
                    MasterVolume = audioValues.MasterVolume,
                    MusicVolume = audioValues.MusicVolume,
                    SfxVolume = audioValues.SfxVolume,
                },

                videoValues = new VideoValues {
                    Resolution = new ResolutionData {
                        Width = videoValues.Resolution.Width,
                        Height = videoValues.Resolution.Height,
                    },
                    VSync = videoValues.VSync,
                    DepthOfField = videoValues.DepthOfField,
                }
            };

            m_saveSettings.Save( values );
        }
    }
    #endregion

    #region Audio Settings
    public sealed class AudioSettings : IDisposable {
        const string MASTER_PARAM = "MASTER";
        const string MUSIC_PARAM = "MUSIC";
        const string SFX_PARAM = "SFX";

        const float MIN_DB = -80f;
        const float MAX_PERCENT = 100f;
        const float MIN_PERCENT_NON_ZERO = 0.0001f;
        readonly Slider m_masterVolumeSlider;

        readonly AudioMixer m_mixer;
        readonly Slider m_musicVolumeSlider;
        readonly Slider m_sfxVolumeSlider;

        bool m_initialized;

        public AudioValues AudioValues { get; }

        EventCallback<ChangeEvent<float>> m_masterCallback;
        EventCallback<ChangeEvent<float>> m_musicCallback;
        EventCallback<ChangeEvent<float>> m_sfxCallback;

        public AudioSettings(OptionElementComponents elementComponents, AudioValues audioValues) {
            AudioValues = audioValues;
            m_mixer = elementComponents.AudioMixer;
            m_masterVolumeSlider = elementComponents.MasterVolumeSlider;
            m_musicVolumeSlider = elementComponents.MusicVolumeSlider;
            m_sfxVolumeSlider = elementComponents.SfxVolumeSlider;
        }

        public void Init() {
            if ( m_initialized ) return;
            if ( m_mixer == null ) return;
            if ( AudioValues == null ) return;

            // Set initial slider values from saved settings (convert dB to percent)
            if ( m_masterVolumeSlider != null ) {
                m_masterVolumeSlider.value = DecibelsToPercent( AudioValues.MasterVolume );
            }

            if ( m_musicVolumeSlider != null ) {
                m_musicVolumeSlider.value = DecibelsToPercent( AudioValues.MusicVolume );
            }

            if ( m_sfxVolumeSlider != null ) {
                m_sfxVolumeSlider.value = DecibelsToPercent( AudioValues.SfxVolume );
            }

            // Create callbacks (one shared helper, different parameter names)
            m_masterCallback = CreateVolumeCallback( MASTER_PARAM );
            m_musicCallback = CreateVolumeCallback( MUSIC_PARAM );
            m_sfxCallback = CreateVolumeCallback( SFX_PARAM );

            // Register if sliders exist
            m_masterVolumeSlider?.RegisterValueChangedCallback( m_masterCallback );
            m_musicVolumeSlider?.RegisterValueChangedCallback( m_musicCallback );
            m_sfxVolumeSlider?.RegisterValueChangedCallback( m_sfxCallback );

            ApplyValues();

            m_initialized = true;
        }

        void ApplyValues() {
            // Push current slider values into the mixer so it's immediately in sync
            if ( m_masterVolumeSlider != null ) {
                ApplyVolume( m_masterVolumeSlider.value, MASTER_PARAM );
            }

            if ( m_musicVolumeSlider != null ) {
                ApplyVolume( m_musicVolumeSlider.value, MUSIC_PARAM );
            }

            if ( m_sfxVolumeSlider != null ) {
                ApplyVolume( m_sfxVolumeSlider.value, SFX_PARAM );
            }
        }

        public void Dispose() {
            if ( m_masterCallback != null && m_masterVolumeSlider != null ) {
                m_masterVolumeSlider.UnregisterValueChangedCallback( m_masterCallback );
            }

            if ( m_musicCallback != null && m_musicVolumeSlider != null ) {
                m_musicVolumeSlider.UnregisterValueChangedCallback( m_musicCallback );
            }

            if ( m_sfxCallback != null && m_sfxVolumeSlider != null ) {
                m_sfxVolumeSlider.UnregisterValueChangedCallback( m_sfxCallback );
            }

            m_masterCallback = null;
            m_musicCallback = null;
            m_sfxCallback = null;
            m_initialized = false;
        }

        EventCallback<ChangeEvent<float>> CreateVolumeCallback(string mixerParamName)
            => evt => ApplyVolume( evt.newValue, mixerParamName );

        public AudioValues GetCurrentDecibelLevels() {
            float masterDb = MIN_DB;
            float musicDb = MIN_DB;
            float sfxDb = MIN_DB;

            if ( m_mixer != null ) {
                m_mixer.GetFloat( MASTER_PARAM, out masterDb );
                m_mixer.GetFloat( MUSIC_PARAM, out musicDb );
                m_mixer.GetFloat( SFX_PARAM, out sfxDb );
            }

            return new AudioValues {
                MasterVolume = masterDb,
                MusicVolume = musicDb,
                SfxVolume = sfxDb,
            };
        }

        void ApplyVolume(float sliderPercent, string mixerParamName) {
            if ( m_mixer == null ) {
                return;
            }

            // Clamp first, then convert to decibels
            float clamped = Mathf.Clamp( sliderPercent, 0f, MAX_PERCENT );
            float dB = PercentToDecibels( clamped );
            m_mixer.SetFloat( mixerParamName, dB );
        }

        public static float PercentToDecibels(float percent) {
            if ( percent <= 0f ) {
                return MIN_DB; // hard mute
            }

            float normalized = Mathf.Clamp( percent, MIN_PERCENT_NON_ZERO, MAX_PERCENT ) / 100f;
            return Mathf.Log10( normalized ) * 20f;
        }

        public static float DecibelsToPercent(float dB) {
            if ( dB <= MIN_DB ) {
                return 0f;
            }

            float normalized = Mathf.Pow( 10f, dB / 20f );
            return Mathf.Clamp01( normalized ) * 100f;
        }
    }
    #endregion

    #region Video Settings
    public class VideoSettings : IDisposable {
        EventCallback<ChangeEvent<string>> m_resolutionCallback;
        EventCallback<ChangeEvent<bool>> m_fullscreenCallback;
        EventCallback<ChangeEvent<bool>> m_vSyncCallback;
        EventCallback<ChangeEvent<bool>> m_depthOfFieldCallback;

        readonly DropdownField m_resolutionDropdown;
        readonly Toggle m_fullscreenToggle;
        readonly Toggle m_vSyncToggle;
        readonly Toggle m_depthOfFieldToggle;
        readonly Button m_acceptChangesButton;
        
        public VideoValues VideoValues { get; }
        readonly VolumeProfile m_volumeProfile;

        public VideoSettings(OptionElementComponents elementComponents, VideoValues videoValues) {
            VideoValues = videoValues;

            m_resolutionCallback = evt => { SetResolution( evt.newValue ); };
            m_fullscreenCallback = CreateToggleCallback( value => VideoValues.Fullscreen = value );
            m_vSyncCallback = CreateToggleCallback( value => VideoValues.VSync = value );
            m_depthOfFieldCallback = CreateToggleCallback( value => VideoValues.DepthOfField = value );

            m_volumeProfile = elementComponents.VolumeProfile;
            m_resolutionDropdown = elementComponents.ResolutionDropdown;
            m_fullscreenToggle = elementComponents.FullscreenToggle;
            m_vSyncToggle = elementComponents.VSyncToggle;
            m_depthOfFieldToggle = elementComponents.DepthOfFieldToggle;
            m_acceptChangesButton = elementComponents.AcceptChangesButton;

            m_resolutionDropdown?.RegisterValueChangedCallback( m_resolutionCallback );
            m_fullscreenToggle?.RegisterValueChangedCallback( m_fullscreenCallback );
            m_vSyncToggle?.RegisterValueChangedCallback( m_vSyncCallback );
            m_depthOfFieldToggle?.RegisterValueChangedCallback( m_depthOfFieldCallback );
            
            m_acceptChangesButton.clicked += HandleAcceptChangesPressed;
        }
        
        void HandleAcceptChangesPressed() {
            if ( VideoValues.Resolution != null ) {
                Screen.SetResolution( VideoValues.Resolution.Width, VideoValues.Resolution.Height,
                    VideoValues.Fullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed );
            }

            QualitySettings.vSyncCount = VideoValues.VSync ? 1 : 0;

            if ( m_volumeProfile == null ) return;
            if ( m_volumeProfile.TryGet<DepthOfField>( out var dof ) ) {
                dof.active = VideoValues.DepthOfField;
            }
        }

        public void Dispose() {
            if ( m_resolutionCallback != null && m_resolutionDropdown != null ) {
                m_resolutionDropdown.UnregisterValueChangedCallback( m_resolutionCallback );
            }

            if ( m_fullscreenCallback != null && m_fullscreenToggle != null ) {
                m_fullscreenToggle.UnregisterValueChangedCallback( m_fullscreenCallback );
            }

            if ( m_vSyncCallback != null && m_vSyncToggle != null ) {
                m_vSyncToggle.UnregisterValueChangedCallback( m_vSyncCallback );
            }

            if ( m_depthOfFieldCallback != null && m_depthOfFieldToggle != null ) {
                m_depthOfFieldToggle.UnregisterValueChangedCallback( m_depthOfFieldCallback );
            }
            
            m_acceptChangesButton.clicked -= HandleAcceptChangesPressed;

            m_resolutionCallback = null;
            m_fullscreenCallback = null;
            m_vSyncCallback = null;
            m_depthOfFieldCallback = null;
        }

        void SetResolution(string resolutionString) {
            int index = resolutionString.IndexOf( 'x' );
            if ( index <= 0 ) return;

            string widthStr = resolutionString.Substring( 0, index ).Trim();
            string heightStr = resolutionString.Substring( index + 1 ).Trim();
            if ( int.TryParse( widthStr, out int width ) &&
                 int.TryParse( heightStr, out int height ) ) {
                VideoValues.Resolution = new ResolutionData {
                    Width = width,
                    Height = height,
                };
            }
        }

        public VideoValues GetCurrentVideoValues() {
            return new VideoValues {
                Resolution = VideoValues.Resolution,
                VSync = VideoValues.VSync,
                DepthOfField = VideoValues.DepthOfField,
            };
        }

        static EventCallback<ChangeEvent<bool>> CreateToggleCallback(Action<bool> applyAction)
            => evt => applyAction( evt.newValue );
    }
    #endregion

    #region Models (Object Data)
    [Serializable] public class ResolutionData {
        public int Width { get; set; }
        public int Height { get; set; }
    }

    [Serializable] public class VideoValues {
        public ResolutionData Resolution { get; set; } = new();
        public bool VSync { get; set; }
        public bool DepthOfField { get; set; }
        public bool Fullscreen { get; set; }
        
        public Vector2 GetResolutionVector2() => new(Resolution.Width, Resolution.Height);
    }

    [Serializable] public class AudioValues {
        public float MasterVolume { get; set; }
        public float MusicVolume { get; set; }
        public float SfxVolume { get; set; }
    }

    [Serializable] public class SettingValues {
        public VideoValues videoValues { get; set; } = new();
        public AudioValues audioValues { get; set; } = new();
    }

    public enum SaveType {
        PlayerPrefs,
        JsonFile,
        //TextFile,
    }
    #endregion
}