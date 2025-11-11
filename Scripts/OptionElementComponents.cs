using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
namespace TCS.Options {
    public class OptionElementComponents {
        // Audio components
        public AudioMixer AudioMixer { get; set; }
        public Slider MasterVolumeSlider { get; set; }
        public Slider MusicVolumeSlider { get; set; }
        public Slider SfxVolumeSlider { get; set; }

        // Video components
        public static readonly Vector2[] SupportedResolutions = {
            new(1280, 720),
            new(1920, 1080),
            new(2560, 1440),
            new(3840, 2160),
        };
        public VolumeProfile VolumeProfile { get; set; }
        public DropdownField ResolutionDropdown { get; set; }
        public Toggle FullscreenToggle { get; set; }
        public Toggle VSyncToggle { get; set; }
        public Toggle DepthOfFieldToggle { get; set; }
        public Button AcceptChangesButton { get; set; }
    }
}