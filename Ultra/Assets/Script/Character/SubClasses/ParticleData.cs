using UnityEngine;

public class ParticleData : MonoBehaviour
{
    //////////// Particles ////////////

    [Header("JumpParticle")]
    public GameObject ps_JumpOnGround;
    public GameObject ps_JumpInAir;
    public GameObject ps_Landing;

    [Header("Dash")]
    public GameObject ps_DashCloudLeft;
    public GameObject ps_DashCloudRight;

    [Header("Teleport")]
    public GameObject ps_Teleport;

    [Header("GetDamage")]
    public GameObject ps_GetDamaged;
    public ParticleSystem ps_Disabled;

    [Header("Atttack Dash")]
    public GameObject trail;
    public GameObject ps_Slash;

    [Header("Bounce")]
    public GameObject bounce;

    [Header("Attack")]
    public GameObject attackRight;
    public GameObject attackLeft;
}