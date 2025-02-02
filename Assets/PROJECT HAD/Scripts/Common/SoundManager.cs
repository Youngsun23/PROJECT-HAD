using UnityEngine;

namespace HAD
{
    public class SoundManager : SingletonBase<SoundManager>
    {
        public AudioSource audioPrefab;
        public AudioSource sfxSourcePrefab;

        public AudioSource musicSourceA;
        public AudioSource musicSourceB;

        private bool isUsingMusicA = true;

        public void Initialize()
        {
            GameObject newAudioSource = new GameObject("Audio Source");
            audioPrefab = newAudioSource.AddComponent<AudioSource>();
            audioPrefab.transform.SetParent(transform);

            musicSourceA = Instantiate(audioPrefab, transform);
            musicSourceB = Instantiate(audioPrefab, transform);

            musicSourceA.spatialBlend = 0;
            musicSourceA.loop = true;
            musicSourceA.volume = 1f;
            musicSourceB.spatialBlend = 0;
            musicSourceB.loop = true;
            musicSourceB.volume = 0f;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                PlayBGM("Peaceful_BGM");
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                PlayBGM("Endless Love");
            }

            if (isUsingMusicA)
            {
                musicSourceA.volume = Mathf.Lerp(musicSourceA.volume, 1f, Time.deltaTime);
                musicSourceB.volume = Mathf.Lerp(musicSourceB.volume, 0f, Time.deltaTime);
            }
            else
            {
                musicSourceA.volume = Mathf.Lerp(musicSourceA.volume, 0f, Time.deltaTime);
                musicSourceB.volume = Mathf.Lerp(musicSourceB.volume, 1f, Time.deltaTime);
            }
        }

        public AudioClip GetAudioClip(string audioName)
        {
            return Resources.Load<AudioClip>($"Sound/{audioName}");
        }

        // BGM은 Pool 필요 x
        public void PlayBGM(string audioName)
        {
            isUsingMusicA = !isUsingMusicA;

            //TODO : Fade In/Out 구현
            // MusicSourceA 로 재생하고 있었다면?
            // MusicSourceB의 Audio Clip에 GetAudioClip("audioName") 을 넣어준다.
            // MusicSourceA 의 볼륨을 서서히 죽인다.
            // MusicSourceB 의 볼륨을 서서히 올린다.
            // 반대 경우도
        }

        public void PlaySFX(string audioName, Vector3 position)
        {
            // 풀링 사용 - 미리 생성된 sfx pool에서 하나를 가져와서 사용
        }

        public void PlayUI(string audioName)
        {

        }
    }
}