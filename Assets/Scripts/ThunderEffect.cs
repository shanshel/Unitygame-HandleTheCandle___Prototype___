using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;
using UnityEngine.Rendering.PostProcessing;

public class ThunderEffect : MonoBehaviour
{
    public Light2D _light;
    public PostProcessVolume _postProccess;

    
    float flashTimer;
    ColorGrading colorGrad;
    Vector4 baseLift;

    private void Start()
    {
        _postProccess.profile.TryGetSettings<ColorGrading>(out colorGrad);
        InvokeRepeating("flashThunder", 2f, 4f);
    }

    void flashThunder()
    {
        baseLift = colorGrad.lift.value;

        float rand = Random.Range(0, 100);

        if (rand > 40f)
        {
            flashTimer = .2f;
            Invoke("playSound", 0.5f);
        }
    }
    void playSound()
    {
        AudioManager.singlton.playSFX(Enums.SFXEnum.thunderSFX, false, Random.Range(0.4f, 1.2f));
    }

    private void Update()
    {
        if (flashTimer > 0f)
        {
            flashTimer -= Time.deltaTime;
            _light.intensity = Mathf.MoveTowards(_light.intensity, 10f, Time.deltaTime * 5f);
            colorGrad.lift.value = new Vector4(-0.13f, 0.03f, 0.79f);


        }
        else
        {
            _light.intensity = Mathf.MoveTowards(_light.intensity, 0f, Time.deltaTime * 15f);
            colorGrad.lift.value = baseLift;
        }
    }
}
