### Completed
So far i've been working a bit on everything, i wouldn't say i have any aspect totally finished but according to my milestone i do have an MVP for the _map drawing_ as well as _enemies_ and one could argue that making
logic for combat and items as well as drops and a backpack is nessescary for the _core loop_.

My strategy for this dev cycle has been working on a problem until it works without obsessing to much about quality - i want to be able to keep implementing features that are as decoupled as possible, so that when
the time comes for me to actually make the code pretty it won't be a massive hassle. 

A little list in somewhat order of what i made:
- Player movement
- Added tilemap
- Drew static testing map (primarily for colliders)
- Player animation
- Camera(cinemachine)
- Slime enemy added
- Slime idle/walking animations
- Gave playerprefab a weapon
- Made weapon logic (simple slice)
- Made a healthbar
- Made logic for healthbar
- Made Piggy enemy
- Made Piggy idle/walk animations
- Made simple LOS(Line of sight) system based on tiles in tilemap
- Made logic for enemy to enter _Attack mode_ when in **Players** LOS
- Made Piggys moveset more interesting
- Made Quick cast indicators for Enemies
- Made a simple start screen
- Made slime attack logic
- Made Backpack UI
- Added GridLayout to Backpack
- Made logic for Enemies dropping items upon death
- Made logic for items to be picked up into bag
- Made logic for player to equip item from bag
- Fixed Player so the _Starter sword_ isn't attached to the player prefab
- Made some managers for **Events**, **Inventory**
- 'Tried' to make a shader for _Drops_ to make them glow - need URP later for that to work
- Added _Base scene_
- Made base pretty :3
- Made logic for moving between scenes
- MASSIVE Overhaul of logic - to be able to move between scenes seamlessly
- Made **SceneManager** + **InitSetupManagers**
- Made map on canvas
- Implemented logic to draw on canvas (_Not working 100% at moment of writing this_)

Whew - that's a lot of stuff :D

Right not my focus is on making an inventory system:
1. Make some barebone world gen
2. Expand on LOS (enemies should move smart not just at player)
3. Make player 'feel' better - (_Dash, Attack, Spells_)
4. Expand on enemies and player
5. Fix canvas scaling
6. Options
7. _Saving / loading_
