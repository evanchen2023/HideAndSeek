using System;
using Fusion;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[OrderBefore(typeof(NetworkTransform))]
[DisallowMultipleComponent]
// ReSharper disable once CheckNamespace
public class NetworkCharacterControllerPrototype : NetworkTransform {
  //Private Components
  private Camera localCamera;
  private float camRotateX;
  private float camRotateY;
  private Vector3 lastDirection;
  private Transform playerModel;
  
  [Header("Character Controller Settings")]
  public float gravity       = -9.81f;
  public float jumpImpulse   = 5.0f;
  public float acceleration  = 5.0f;
  public float braking       = 50.0f;
  public float maxSpeed      = 2.0f;
  public float rotationSpeed = 15.0f;
  public float turnSmoothing = 0.06f;

  [Networked]
  [HideInInspector]
  public bool IsGrounded { get; set; }

  [Networked]
  [HideInInspector]
  public Vector3 Velocity { get; set; }

  /// <summary>
  /// Sets the default teleport interpolation velocity to be the CC's current velocity.
  /// For more details on how this field is used, see <see cref="NetworkTransform.TeleportToPosition"/>.
  /// </summary>
  protected override Vector3 DefaultTeleportInterpolationVelocity => Velocity;

  /// <summary>
  /// Sets the default teleport interpolation angular velocity to be the CC's rotation speed on the Z axis.
  /// For more details on how this field is used, see <see cref="NetworkTransform.TeleportToRotation"/>.
  /// </summary>
  protected override Vector3 DefaultTeleportInterpolationAngularVelocity => new Vector3(0f, 0f, rotationSpeed);

  public CharacterController Controller { get; private set; }

  void Start()
  {
    playerModel = gameObject.transform.Find("Model");
  }
  protected override void Awake() {
    base.Awake();
    CacheController();
  }

  public override void Spawned() {
    base.Spawned();
    CacheController();
  }

  private void CacheController() {
    if (Controller == null) {
      Controller = GetComponent<CharacterController>();

      Assert.Check(Controller != null, $"An object with {nameof(NetworkCharacterControllerPrototype)} must also have a {nameof(CharacterController)} component.");
    }
  }

  protected override void CopyFromBufferToEngine() {
    // Trick: CC must be disabled before resetting the transform state
    Controller.enabled = false;

    // Pull base (NetworkTransform) state from networked data buffer
    base.CopyFromBufferToEngine();

    // Re-enable CC
    Controller.enabled = true;
  }

  /// <summary>
  /// Basic implementation of a jump impulse (immediately integrates a vertical component to Velocity).
  /// <param name="ignoreGrounded">Jump even if not in a grounded state.</param>
  /// <param name="overrideImpulse">Optional field to override the jump impulse. If null, <see cref="jumpImpulse"/> is used.</param>
  /// </summary>
  public virtual void Jump(bool ignoreGrounded = false, float? overrideImpulse = null) {
    if (IsGrounded || ignoreGrounded) {
      var newVel = Velocity;
      newVel.y += overrideImpulse ?? jumpImpulse;
      Velocity =  newVel;
    }
  }

  /// <summary>
  /// Basic implementation of a character controller's movement function based on an intended direction.
  /// <param name="direction">Intended movement direction, subject to movement query, acceleration and max speed values.</param>
  /// </summary>
  public virtual void Move(Vector3 direction) {
    var deltaTime    = Runner.DeltaTime;
    var previousPos  = transform.position;
    var moveVelocity = Velocity;

    direction = direction.normalized;

    if (IsGrounded && moveVelocity.y < 0) {
      moveVelocity.y = 0f;
    }

    moveVelocity.y += gravity * Runner.DeltaTime;

    var horizontalVel = default(Vector3);
    horizontalVel.x = moveVelocity.x;
    horizontalVel.z = moveVelocity.z;

    if (direction == default) {
      horizontalVel = Vector3.Lerp(horizontalVel, default, braking * deltaTime);
    } else {
      horizontalVel      = Vector3.ClampMagnitude(horizontalVel + direction * acceleration * deltaTime, maxSpeed);
      //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Runner.DeltaTime);
    }

    moveVelocity.x = horizontalVel.x;
    moveVelocity.z = horizontalVel.z;

    Controller.Move(moveVelocity * deltaTime);

    Velocity   = (transform.position - previousPos) * Runner.Simulation.Config.TickRate;
    IsGrounded = Controller.isGrounded;
  }

  public void Rotate(Vector3 viewInput, Vector3 input, Transform cameraTransform)
  { 
    //Horizontal, Vertical Input
    float vertical = input.z;
    float horizontal = input.x;
    
    // Get camera forward direction, without vertical component.
    Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);

    // Player is moving on ground, Y component of camera facing is not relevant.
    forward.y = 0.0f;
    forward = forward.normalized;

    // Calculate target direction based on camera forward and direction key.
    Vector3 right = new Vector3(forward.z, 0, -forward.x);
    Vector3 targetDirection = forward * vertical + right * horizontal *-1;

    // Lerp current direction to calculated target direction.
    if ((IsMoving(horizontal, vertical) && targetDirection != Vector3.zero))
    {
      Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

      Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSmoothing);
      playerModel.rotation = Quaternion.Lerp(targetRotation, newRotation, turnSmoothing * Runner.DeltaTime);
      SetLastDirection(targetDirection);
    }
    // If idle, Ignore current camera facing and consider last moving direction.
    if (!(Mathf.Abs(horizontal) > 0.9 || Mathf.Abs(vertical) > 0.9))
    {
      Repositioning();
    }
  }
  
  private void Repositioning()
  {
    if(lastDirection != Vector3.zero)
    {
      lastDirection.y = 0;
      Quaternion targetRotation = Quaternion.LookRotation (lastDirection);
      Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSmoothing);
      playerModel.rotation = Quaternion.Lerp(targetRotation, newRotation, turnSmoothing * Runner.DeltaTime);
    }
  }
  
  public void SetLastDirection(Vector3 direction)
  {
    lastDirection = direction;
  }

  private bool IsMoving(float h, float v)
  {
    return (h != 0)|| (v != 0);
  }
}