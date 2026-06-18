
### 0.6.0

The Death State & Game Over: Handle the plane's destruction gracefully (a massive explosion, detaching the camera so it doesn't get deleted, and showing a "Restart" button)

Added Game Over Panel

### 0.5.3

Updated HealthBarUI.cs. It now first checks GetComponentInParent<Health>(). If it's attached to a Tank, it will find the Tank's health. If it's attached to the Main Screen UI, it will fall back to finding the Biplane. 

### 0.5.2

Added BillboardUI

### 0.5.1

Added UI Canvas

Added HealthBarUI.cs component

find the player and use .AddListener()

Changes the color from Green to Red as health dropsW

### 0.5.0

Adding Health Bars: using Unity Separation of Concerns - Data/logic separate from Visuals.

In Health:cs:

- Added TakeDamage function
- Die function

Also:

Updated BombCollision.cs so explosions actually reduce health.

### 0.4.1

Added Gravity Compensation to bombs and rockets

Reasoning:
Because Physics.gravity in Unity is a downward force (usually (0, -9.81, 0)), we calculate that downward distance, and then invert it (using a minus sign) to aim the tank's tower perfectly upward by that exact amount
A
### 0.4.0

Added BombCollision.cs for bomb collision. 
Added bomb explosions.

Used syntax for visibility in Unity Engine:  [Header("Explosion Settings")]


### 0.3.2

Adjusted the bomb/missile spawn point so it's below the plane by using empty GameObject

### 0.3.1
Improved shooting mechanic:
- Introduced Deflection Math

Tanks aim at the player and calculate where the player will be next, so the balls can be shooted slower around 40-50.0f

### 0.3.0
Added shooting: Tanks shoot balls on the same direction they move (forward/up)

### 0.2.0
Shaping the terrain, added base plane, tank prefabs, basic Moving AI agent

### 0.1.1
Project initialization, added base terrain, make it green