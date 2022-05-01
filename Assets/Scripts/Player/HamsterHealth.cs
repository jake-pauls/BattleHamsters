using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

[RequireComponent(typeof(BallRotation), typeof(CapsuleCollider))]
public class HamsterHealth : MonoBehaviour
{
    public UnityEvent<HamsterHealth, string> OnDead;

    [NonSerialized]public bool IsInvulnerable;

    public bool IsDead => isDirty_Dead;

    [SerializeField] private float targetSquishScale_Y = 0.05f;
    [SerializeField] private float ballKillVelThreshold = 2f;
    
    [SerializeField]
    private float reviveCooldown = 3f;
    // to prevent from re-triggering revive
    private bool isDirty_Dead;
    private Vector3 _capturedScale;

    private void Start()
    {
        _capturedScale = transform.localScale;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(IsDead || IsInvulnerable) return;
        
        //todo ball check
        if (!collision.collider.CompareTag("PlayerHamsterBall")) return;

        var ballRigid = collision.transform.GetComponent<Rigidbody>();
        Assert.IsNotNull(ballRigid, $"{collision.gameObject.name} doesnt have rigid body");

        // check ball riding
        if(GetComponent<BallRotation>()._ballMode)
        {
            Debug.Log("Ham is unkillable, since ball mode");
            return;
        }

        // calculate velocity
        var capturedVel = ballRigid.velocity.magnitude;
        Debug.Log($"Ball velocity = {capturedVel}");
        if (capturedVel < ballKillVelThreshold) return;
        
        // disable input
        GetComponent<BallRotation>().inputDisabled = true;

        // trigger revive cooldown
        StartCoroutine(TryRevive());
        
        // squish effects
        StartCoroutine(TriggerSquishEffect());
        
        // mark invulnerable
        UpdateCollision(true);
        
        //IMPORTANT mark dirty
        isDirty_Dead = true;
    }

    private void UpdateCollision(bool inDead)
    {
        // GetComponent<Rigidbody>().detectCollisions = !inDead;
        // GetComponent<Rigidbody>().useGravity = !inDead;
        // GetComponent<CapsuleCollider>().enabled = !inDead;
        gameObject.layer = inDead ? 6 : LayerMask.GetMask("Default");
    }

    private IEnumerator TryRevive()
    {
        if(isDirty_Dead) yield break;

        yield return new WaitForSeconds(reviveCooldown);
        
        OnRevive();
    }

    private IEnumerator TriggerSquishEffect()
    {
        if(isDirty_Dead) yield break;
        float capturedY = transform.localPosition.y;
        float y_Offset = GetComponent<CapsuleCollider>().height / 2 * transform.localScale.y;
        var interval = 0.033f;
        float squishTimer = 0f, squishDur = .2f;

        while (squishTimer < squishDur)
        {
            squishTimer += interval;
            transform.localScale = Vector3.Slerp(_capturedScale,
                new Vector3(_capturedScale.x, targetSquishScale_Y, _capturedScale.z), squishTimer / squishDur);
            // transform.localPosition = new Vector3(transform.localPosition.x, capturedY + Mathf.Lerp(0, -y_Offset, squishTimer / squishDur), transform.localPosition.z);

            yield return new WaitForSeconds(interval);
        }
    }

    private void OnRevive()
    {
        transform.localScale = _capturedScale;
        
        UpdateCollision(false);
        
        // reset location to the spawning point
        Transform spawnTrans = PlayerManager.Instance.SpawnPoint(GetComponent<BallRotation>().pid);
        transform.position = spawnTrans.position;
        transform.rotation = spawnTrans.rotation;
        
        // reset input
        GetComponent<BallRotation>().inputDisabled = false;
        
        isDirty_Dead = false;
    }
}
