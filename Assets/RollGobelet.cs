using UnityEngine;
using UnityEngine.InputSystem;
using PurrNet.StateMachine;
using PurrNet;
public class RollGobelet : MonoBehaviour
{
    [SerializeField] private GameObject gobelet;
    [SerializeField] private NetworkAnimator animator;
    [SerializeField] private Animator Uanimator;
    [SerializeField] private InputActionReference lookInput;
    [SerializeField] private CheckMouse checkMouse;
    [SerializeField] private MeshCollider wall;
    [SerializeField] private BoxCollider close;
    [SerializeField] private NetworkAudioSource audioSource;
    [SerializeField] private AudioClip audioRoll;
    [SerializeField] private AudioClip downRoll;
    [SerializeField] private AudioClip UpRoll;
    [SerializeField] private float fadeInDuration = 0.5f;
    [SerializeField] private float fadeOutDuration = 0.5f;
    private float initialVolume;
    public float movementSpeed;
    private bool onRoll = false;
    private void Awake()
    {
        initialVolume = audioSource.volume;
    }

    private Vector3 OGpos;
    public void StartRoll()
    {
        animator.SetTrigger("StartRoll");
        wall.enabled = false;
        close.enabled = true;
    }
    public void PauseRoll()
    {
        audioSource.clip = audioRoll;
        PlayWithFadeIn();
        animator.speed = 0;
        OGpos = checkMouse.transform.position;
        Uanimator.enabled = false;
        checkMouse.On = true;
    }
    public void StopRoll()
    {
        onRoll = false;
        StopWithFadeOut();
        checkMouse.On = false;
        Uanimator.enabled = true;
        animator.speed = 1;
        checkMouse.transform.position = OGpos;
    }
    void Update()
    {
        if(onRoll)
            audioSource.volume = movementSpeed / 2;
    }
    public void PlayRollUp()
    {
        audioSource.volume = initialVolume;
        audioSource.clip = UpRoll;
        audioSource.Play();
    }
    public void PlayRollDown()
    {
        audioSource.volume = initialVolume;
        audioSource.clip = downRoll;
        audioSource.Play();
    }
    public void PlayWithFadeIn()
    {
        audioSource.volume = 0f;
        audioSource.Play();
        StartCoroutine(FadeIn());
    }

    // Appelle cette méthode pour arrêter le son avec un fade-out
    public void StopWithFadeOut()
    {
        StartCoroutine(FadeOut());
    }
        private System.Collections.IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeInDuration)
        {
            audioSource.volume = Mathf.Lerp(0f, initialVolume, elapsedTime / fadeInDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        onRoll = true;
        audioSource.volume = initialVolume;
    }

    private System.Collections.IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            audioSource.volume = Mathf.Lerp(initialVolume, 0f, elapsedTime / fadeOutDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
