public enum GroundState
{
    Idle,
    Walking,
    Landing
};

public enum FallState
{
    Fall,
    Attack,
    Jump,
    Dodge,
    OnWall
};

public enum AttackState
{
    AttackUp,
    AttackAngledUp,
    AttackSide,
    AttackAngledDown,
    AttackDown
};

public enum JumpState
{
    JumpOnGround,
    JumpInAir,
    JumpInAirTwo
};