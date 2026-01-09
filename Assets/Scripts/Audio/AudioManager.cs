using UnityEngine;
using System.Collections;
using Barcode;

/// <summary>
/// 전체 오디오를 관리하는 매니저
/// 화면 전환 시 현재 오디오 중지 후 새 오디오 재생
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("오디오 소스")]
    [SerializeField] private AudioSource audioSource;

    [Header("메인화면 대기 음악")]
    [Tooltip("메인화면에서 반복 재생될 음악")]
    [SerializeField] private AudioClip mainIdleClip;
    [Tooltip("대기 음악 재생 간격 (초)")]
    [SerializeField] private float idleInterval = 10f;

    [Header("영수증 화면 음악")]
    [SerializeField] private AudioClip receiptClip;

    [Header("품목별 오디오")]
    [SerializeField] private ProductAudioMapping[] productAudios;

    private Coroutine _idleCoroutine;
    private bool _isMainScreen = true;

    private void Awake()
    {
        // 싱글톤 설정
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // AudioSource 자동 생성
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Start()
    {
        // 메인화면 대기 음악 시작
        StartIdleMusic();
    }

    /// <summary>
    /// 메인화면 대기 음악 시작 (10초마다 반복)
    /// </summary>
    public void StartIdleMusic()
    {
        _isMainScreen = true;
        StopAllAudio();

        if (mainIdleClip != null)
        {
            _idleCoroutine = StartCoroutine(IdleMusicLoop());
        }
    }

    private IEnumerator IdleMusicLoop()
    {
        while (_isMainScreen)
        {
            PlayClip(mainIdleClip);
            yield return new WaitForSeconds(idleInterval);
        }
    }

    /// <summary>
    /// 품목 스캔 시 오디오 재생
    /// </summary>
    public void PlayProductAudio(ProductType productType)
    {
        _isMainScreen = false;
        StopIdleCoroutine();
        StopAllAudio();

        AudioClip clip = GetProductClip(productType);
        if (clip != null)
        {
            PlayClip(clip);
        }
    }

    /// <summary>
    /// 영수증 화면 오디오 재생
    /// </summary>
    public void PlayReceiptAudio()
    {
        _isMainScreen = false;
        StopIdleCoroutine();
        StopAllAudio();

        if (receiptClip != null)
        {
            PlayClip(receiptClip);
        }
    }

    /// <summary>
    /// 메인화면으로 돌아갈 때 (Exit)
    /// </summary>
    public void ReturnToMain()
    {
        StartIdleMusic();
    }

    /// <summary>
    /// 현재 재생 중인 오디오 중지
    /// </summary>
    public void StopAllAudio()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void StopIdleCoroutine()
    {
        if (_idleCoroutine != null)
        {
            StopCoroutine(_idleCoroutine);
            _idleCoroutine = null;
        }
    }

    private void PlayClip(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    private AudioClip GetProductClip(ProductType productType)
    {
        if (productAudios == null) return null;

        foreach (var mapping in productAudios)
        {
            if (mapping.productType == productType)
            {
                return mapping.audioClip;
            }
        }
        return null;
    }
}

/// <summary>
/// 품목별 오디오 매핑
/// </summary>
[System.Serializable]
public class ProductAudioMapping
{
    public ProductType productType;
    public AudioClip audioClip;
}
