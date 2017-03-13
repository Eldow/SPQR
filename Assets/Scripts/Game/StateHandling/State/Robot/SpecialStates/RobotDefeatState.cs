using System;
using UnityEngine;

public class RobotDefeatState : RobotState {
    public override State HandleInput(StateMachine stateMachine) {
        return null;
    }

    public RobotDefeatState() {
        this.Initialize();
    }

    public override void Update(StateMachine stateMachine) {

    }

    public override void Enter(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        RobotStateMachine robotStateMachine =
            ((RobotStateMachine) stateMachine);

        /* to improve by putting all children in one GameObject, to be 
         * destroyed after.
         */
        Transform robot = robotStateMachine.PlayerController.transform;

        try {
            GameObject robotRemains =
                PhotonNetwork.Instantiate(
                    "RobotRemains", 
                    robot.transform.position, 
                    robot.transform.rotation, 
                    0
                );

            PhotonNetwork.Destroy(robot.gameObject);

            robotRemains.transform.parent = robotStateMachine.transform.parent;
            PlayAudioEffect(robotRemains.GetComponent<PlayerAudio>());
        } catch (ArgumentException argumentException) {
            Debug.LogError(argumentException.Message);
            Debug.LogError("Failed to load RobotRemains!");
        } catch (Exception exception) {
            Debug.LogError(exception.Message);
        }
    }

    public override void Exit(StateMachine stateMachine) {
    }

    public override void PlayAudioEffect(PlayerAudio audio)
    {
        audio.Destruction();
    }
}
