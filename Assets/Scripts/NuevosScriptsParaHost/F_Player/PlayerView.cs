using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : NetworkBehaviour
{
    [SerializeField] GameObject _visualMesh;
    [SerializeField] ParticleSystem _shotParticles;

    NetworkMecanimAnimator _mecanimAnimator;

    [Networked, OnChangedRender(nameof(ShowShotParticles))]
    NetworkBool Firing { get; set; }

    public override void Spawned()
    {
        var lifeHandler = GetComponentInParent<LifeHandler>();
        if (lifeHandler)
        {
            lifeHandler.OnDeadStateChanged += ShowMesh;
        }

        var shotComponent = GetComponentInParent<ShotHandler>();

        if (shotComponent)
        {
            shotComponent.OnShot += () => { Firing = !Firing; };
        }

        _mecanimAnimator = GetComponent<NetworkMecanimAnimator>();

        if (_mecanimAnimator)
        {
            var movementComponent = GetComponentInParent<NetworkCharacterControllerCustom>();
        
            if (movementComponent)
            {
                movementComponent.OnMovement += MoveAnimation;
            }
        }
    }

    void MoveAnimation(float xAxi)
    {
        _mecanimAnimator.Animator.SetFloat("xAxi", Mathf.Abs(xAxi));
    }

    void ShowShotParticles()
    {
        _shotParticles.Play();
    }

    void ShowMesh(bool isDead)
    {
        _visualMesh.SetActive(!isDead);
    }
}
