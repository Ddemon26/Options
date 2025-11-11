using System;
using UnityEngine;
namespace TCS.Options {
    [Serializable] public class SaveSettings {
        public SaveType m_saveType = SaveType.PlayerPrefs;
        const string FILE_NAME = "settings";
        public string SavePath => Application.persistentDataPath + "/" + FILE_NAME;
        public void Save(SettingValues values) {
            switch (m_saveType) {
                case SaveType.PlayerPrefs:
                    SavePlayerPrefs( values );
                    break;
                case SaveType.JsonFile:
                    SaveJsonFile( values );
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void SaveJsonFile(SettingValues values) {
            //save the values as json to a file
            string path = SavePath + ".json";
            string json = JsonUtility.ToJson( values, true );
            System.IO.File.WriteAllText( path, json );
        }

        static void SavePlayerPrefs(SettingValues values) {
            // Audio settings
            PlayerPrefs.SetFloat( "MasterVolume", values.audioValues.MasterVolume );
            PlayerPrefs.SetFloat( "MusicVolume", values.audioValues.MusicVolume );
            PlayerPrefs.SetFloat( "SFXVolume", values.audioValues.SfxVolume );
            // Video settings
            PlayerPrefs.SetInt( "ResolutionWidth", values.videoValues.Resolution.Width );
            PlayerPrefs.SetInt( "ResolutionHeight", values.videoValues.Resolution.Height );
            PlayerPrefs.SetInt( "VSync", values.videoValues.VSync ? 1 : 0 );
            PlayerPrefs.SetInt( "DepthOfField", values.videoValues.DepthOfField ? 1 : 0 );
            PlayerPrefs.Save();
        }

        public SettingValues Load() {
            return m_saveType switch {
                SaveType.PlayerPrefs => LoadPlayerPrefs(),
                SaveType.JsonFile => LoadJsonFile(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        SettingValues LoadJsonFile() {
            string path = SavePath + ".json";
            if ( !System.IO.File.Exists( path ) ) {
                return new SettingValues();
            }

            string json = System.IO.File.ReadAllText( path );
            var values = JsonUtility.FromJson<SettingValues>( json );
            return values;
        }

        static SettingValues LoadPlayerPrefs() {
            SettingValues values = new() {
                audioValues = new AudioValues {
                    MasterVolume = PlayerPrefs.GetFloat( "MasterVolume", 75f ),
                    MusicVolume = PlayerPrefs.GetFloat( "MusicVolume", 60f ),
                    SfxVolume = PlayerPrefs.GetFloat( "SFXVolume", 80f ),
                },

                videoValues = new VideoValues {
                    Resolution = new ResolutionData {
                        Width = PlayerPrefs.GetInt( "ResolutionWidth", 1920 ),
                        Height = PlayerPrefs.GetInt( "ResolutionHeight", 1080 ),
                    },
                    VSync = PlayerPrefs.GetInt( "VSync", 1 ) == 1,
                    DepthOfField = PlayerPrefs.GetInt( "DepthOfField", 1 ) == 1,
                }
            };
            return values;
        }
    }
}