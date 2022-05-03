using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RigidbodySync : MonoBehaviourPun, IPunObservable
{
    private Rigidbody _rigidbody;
    private Vector3 _networkPosition;
    private Quaternion _networkRotation;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // called by PhotonView component to stream network data
    // called on both owner and other players, 10/s by default
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // if true, this is my object
        if(stream.IsWriting)
        {
            // send data to other players (order matters!)
            stream.SendNext(_rigidbody.position);
            stream.SendNext(_rigidbody.rotation);
            stream.SendNext(_rigidbody.velocity);
            stream.SendNext(_rigidbody.angularVelocity);
        }
        // else, this is NOT my object
        else
        {
            // receive data from owner, cast to usable types (order matters!)
            _networkPosition = (Vector3)stream.ReceiveNext();
            _networkRotation = (Quaternion)stream.ReceiveNext();

            // apply velocity/angular velocity directly
            _rigidbody.velocity = (Vector3)stream.ReceiveNext();
            _rigidbody.angularVelocity = (Vector3)stream.ReceiveNext();

            // calculate current lag time
            double timeDiff = PhotonNetwork.Time - info.SentServerTime; // time difference between sent and received timestamps
            float lag = (float)timeDiff;

            // extrapolate current position and rotation based on lag time
            _networkPosition += _rigidbody.velocity * lag;
            _networkRotation *= Quaternion.Euler(_rigidbody.angularVelocity * lag);
        }
    }

    private void FixedUpdate()
    {
        // stop if we own this object, only other players need to sync
        if (photonView.IsMine) return;

        // find network update period (time between network messages)
        float networkDeltatime = 1f / PhotonNetwork.SerializationRate;

        // find error amounts for position and rotation
        float distanceError = Vector3.Distance(_rigidbody.position, _networkPosition);
        float angleError = Quaternion.Angle(_rigidbody.rotation, _networkRotation);

        // interpolate towards correct position/rotation
        Vector3 position = Vector3.MoveTowards(_rigidbody.position, _networkPosition, networkDeltatime * distanceError);
        Quaternion rotation = Quaternion.RotateTowards(_rigidbody.rotation, _networkRotation, networkDeltatime * angleError);

        // update rigidbody, MovePosition/Rotation adjust rigidbody but adheres to physics
        _rigidbody.MovePosition(position);
        _rigidbody.MoveRotation(rotation);
    }
}