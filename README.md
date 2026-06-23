# Unity Basic Game - Arcade Flight Combat Prototype

## Project Overview

This is a Unity 6 learning project built with C#. It is a simple arcade flight combat prototype where the player controls a rusty biplane flying through mountains while avoiding and destroying enemy tanks that shoot back.

The primary goal of this project is learning Unity and game development concepts, with a focus on simple, maintainable solutions over rapid feature delivery.

## Features

### Gameplay & Mechanics
- **Player-controlled biplane flight:** Experience flying a rusty biplane with a realistic feel.
- **Enemy Tanks:** Ground targets that move, aim, and shoot back using deflection math to predict the player's position.
- **Shooting Mechanics:** Drop bombs and shoot at enemies with gravity compensation.
- **Health & Damage System:** Both the player and tanks have health bars. Entities take damage from projectiles/bomb collisions and explode gracefully upon destruction.
- **Mission Logic:** Includes checkpoint rings, finish line base, victory UI, and a mission manager handling game state.

### Environment & Graphics
- **Mountain Terrain:** Sculpted mountain ranges.
- **Terrain Visuals:** Procedurally textured terrain with grass at lower elevations and rocky textures at higher elevations. Natural blending between terrain layers via an auto-paint script.
- **Visual Effects (VFX):** Crash smoke and explosion particle systems for plane crashes and bomb impacts.

### User Interface
- **Heads-Up Display (HUD):** Billboard health bars for enemies, main screen UI for the player, and general UI canvas setups.
- **Menus:** Main menu, Pause menu, and a Game Over screen that features a massive explosion and gracefully detaches the camera while showing a Restart button.

## Technical Stack
- **Engine:** Unity 6
- **Language:** C#
- **Rendering:** Universal Render Pipeline (URP)

## Architecture Principles
- **Built-in Systems First:** Prefer built-in Unity systems and avoid unnecessary third-party packages.
- **Composition & Modularity:** Prefer composition over inheritance. Keep systems modular and loosely coupled.
- **Lightweight Logic:** Keep MonoBehaviours focused and separate gameplay logic from presentation/UI where practical.
- **Configuration:** Use ScriptableObjects for configuration and reusable data, and favor inspector-configurable values over hardcoded constants.

## Version History (Changelog Highlights)
- **v0.8.0:** Introduced Mission logic (checkpoints, finish line, Victory UI, MissionManager). Added crash smoke & explosion VFX.
- **v0.7.0:** Added terrain textures, texture layers, and Auto Paint script.
- **v0.6.0:** Implemented Death State, Game Over gracefully handling, and a Game Over Panel.
- **v0.5.x:** Added separation of concerns for Health Data & UI. Billboard UI, taking damage, and dying logic added.
- **v0.4.x:** Gravity Compensation added to bombs and rockets, Bomb collisions and explosions.
- **v0.3.x:** Deflection math introduced for tanks to aim dynamically, basic shooting mechanics implemented.
- **v0.2.x:** Shaped terrain, base plane & tank prefabs, and basic AI.

## Current Priorities & Goals
- **Terrain Improvements:** Add environmental details (trees, vegetation, props) prioritizing performant approaches.
- **Plane Startup Sequence:** Implement an engine start sequence and takeoff preparation with UI prompts before full flight control is given.
- **Shooting Improvements:** Explore better projectile behavior, fire rate balancing, hit feedback, targeting logic, and audio/visual effects.
- **Collision Rules:** Refine plane collision damage specifically tailored for terrain vs. enemy fire.
