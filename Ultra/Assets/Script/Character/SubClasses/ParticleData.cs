using UnityEngine;

public class ParticleData : MonoBehaviour
{
    //////////// Particles ////////////

    [Header("JumpParticle")]
    public GameObject ps_JumpOnGround;
    public GameObject ps_JumpInAir;
    public GameObject ps_JumpInAir2;
    public GameObject ps_Landing;
    public GameObject ps_OnWall_Right;
    public GameObject ps_OnWall_Left;

    [Header("Dash")]
    public GameObject ps_DashCloudLeft;
    public GameObject ps_DashCloudRight;

    [Header("Teleport")]
    public GameObject ps_Teleport;

    [Header("GetDamage")]
    public GameObject ps_GetDamaged;
    public ParticleSystem ps_Disabled;
    public GameObject ps_GetHit;

    [Header("Atttack Dash")]
    public GameObject ps_Slash;

    [Header("Bounce")]
    public GameObject bounce;

    [Header("Attack")]
    public GameObject attackRight;
    public GameObject attackLeft;
    public GameObject attackUp;
    public GameObject attackDown;
    public GameObject attackRightUp;
    public GameObject attackLeftUp;
    public GameObject attackRightDown;
    public GameObject attackLeftDown;

    [Header("ChangeDirection")]
    public GameObject turnAroundLeft;
    public GameObject turnAroundRight;
}