using UnityEngine;

public class Audio : MonoBehaviour
{

    [SerializeField] AudioSource SFXSource;
    public AudioClip ataque1;
    public AudioClip ataque2;
    public AudioClip ataque3;
    public AudioClip matar;

    public void Reproducir(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

}
