using UnityEngine;

public class RobotAttackState : RobotFramedState {
    public int Damage { get; protected set; }
    public int Hitstun { get; protected set; }

    protected override void Initialize() {
        base.Initialize();

        this.Damage = 0;
        this.Hitstun = 0;
    }

    public virtual void HandleAttack(HandleHit handleHit, Collision other) {
        if (!this.IsAttackActive()) return;

        handleHit.SendHit(other, this.Damage, this.Hitstun);
        PlayerController opponent = (PlayerController)other.gameObject.GetComponent<PlayerController>();
        if(opponent != null) SendAudioHit(opponent.PlayerAudio);
    }

    public virtual bool IsAttackActive() {
        return this.CurrentFrame >=
            this.MinActiveState &&
            this.CurrentFrame <=
            this.MaxActiveState;
    }

    public override void PlayAudioEffect(PlayerAudio audio)
    {
        audio.Attack();
    }

    protected virtual void SendAudioHit(PlayerAudio audio)
    {
        if (audio == null) return;
        audio.Hit();
    }
}
