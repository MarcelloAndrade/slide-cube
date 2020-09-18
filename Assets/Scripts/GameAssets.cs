using UnityEngine;

public class GameAssets : MonoBehaviour {

    public SoundAudioClip[] soundsArray;

    private static GameAssets _instance;

    public static GameAssets instance {
        get {
            if (_instance == null) _instance = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            return _instance;
        }
    }

    [System.Serializable]
    public class SoundAudioClip {
        public GameSounds.Sound sound;
        public AudioClip audioClip;
    }
}
