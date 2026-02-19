# Object Handling System

This system allows players to pick up, carry, and manipulate objects in the game world.

## Components

### IHandleObject Interface
Defines the contract for objects that can be handled by the player.

- `bool CanBeHandled { get; }` - Whether the object can be picked up
- `GameObject GetGameObject()` - Returns the GameObject of the handleable object
- `void OnPickup()` - Called when the object is picked up
- `void OnDrop()` - Called when the object is dropped

### HandleObjectController
Main controller that manages the object handling logic.

**Key Features:**
- Pickup objects by pressing the pickup button (LMB by default)
- Carry objects in front of the camera at a fixed distance
- Zoom in/out using mouse wheel
- Rotate objects by holding the rotate button (RMB) and moving mouse
- Drop objects by releasing the pickup button
- Objects maintain their orientation relative to player
- Automatic rotation return to initial pickup orientation
- Manual rotation updates the target orientation
- Physics-based movement and rotation for realistic behavior

**Settings:**
- `pickupRange` - Maximum distance to pick up objects
- `minDistance` / `maxDistance` - Zoom limits
- `zoomSpeed` - Speed of zooming
- `rotationSensitivity` - Sensitivity of object rotation
- `attractionForce` - Force attracting object to target position
- `maxAttractionSpeed` - Maximum attraction speed
- `dampingFactor` - Velocity damping factor
- `angularDampingPickup` - Angular damping when object is picked up
- `rotationReturnSpeed` - Speed of automatic rotation return to target
- `manualRotationSpeed` - Speed of manual rotation with RMB

### HandleableObject
Example implementation of IHandleObject for physical objects.

- Disables physics (Rigidbody.isKinematic = true) and collider when picked up
- Re-enables them when dropped

## Integration

### PlayerInputManager
Added new input actions:
- `pickupAction` - For picking up/dropping objects
- `rotateAction` - For rotating objects
- `ZoomDelta` - Mouse wheel input

### CameraController
Added `SetCameraRotationEnabled(bool)` to disable camera rotation during object rotation.

### PlayerCharacterController
Modified to disable interaction system when holding an object.

## Usage

1. Attach `HandleObjectController` to your player GameObject
2. Configure input actions in `PlayerInputManager`
3. Implement `IHandleObject` on objects you want to be handleable
4. Or use the provided `HandleableObject` component

## Controls

- **Pickup/Drop**: Hold pickup button (LMB)
- **Zoom**: Mouse wheel
- **Rotate Object**: Hold rotate button (RMB) + move mouse
- **Interactions**: Disabled while holding objects

## Rotation Behavior

- When picked up, object remembers its orientation relative to the player
- Object automatically returns to this initial orientation using physics forces
- Manual rotation (RMB) updates the target orientation and rotates faster
- Angular damping is increased during pickup for stable behavior