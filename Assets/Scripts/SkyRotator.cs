using UnityEngine;

public class SkyRotator : MonoBehaviour
{
    public void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 0.4f);
    }
}
