I don't really know what exactly to show here :p check the **ytube** video or the **WebGL** build i guess ^^

Gizmos
-
Throughout the project when dealing with implementing _math heavy_ logic like **WorldGen** and **LOS** i used the gizmos
like a 'visual' debugger, and then implemented the logic from the Gizmo classes in another script. 

An example is the **DrawLOS** script, it draws the line of sight of the Gameobject attached to it interacting with scenery like **walls** tilemap
![image](https://github.com/ymersod/GMD/assets/95355670/29d1f747-a845-4ed0-9612-005fa5dd8100)
Each dot represents a cell in the **tilemap**. Blue dots means no vision for the gameobject and red means it has vision - the idea of using this approach to **LOS** and pathing
is that i dont have to update and calculate if player is in enemys view every frame, but only when the player moves to a new tile.

It was also planned to help with pathing(A project im not 100% finished with the implementation for yet)
![image](https://github.com/ymersod/GMD/assets/95355670/7812d9a5-286a-4a4d-8508-8dacdf93df77)
I created a pathfinding algorithm which finds the fastest way to _(in the gizmos case)_ target put in the inspector, however practically it uses the position of the cell closest to the player that the enemy last time has LOS of the players LOS.

I Hope that makes sense ^^

Finishing Words?
-
I planned on the game not having an end in my desigm - however i had to implement it anyways since the objective of the game wouldn't be very clear besides just bonking enemies.

I would say my game more or less includes all the elements that it had to cover
- coroutines(ex. **SlimeAttackMode** uses coroutines as their 'actions' to on ex. **RegularAttack** to make sure it charges up before attacking)
- events(ex. **UIEvents** and **Events** use a pub-sub pattern as example **player_attack** subcribes to the Fire event which is triggered by a fire action
- Input systems(using new unity input system :) )
- Vectors(When attacking **Player_attack** uses the last moveinput as to gauge the direction of which way to attack)
- Graphics & Audio(yep)
- Shader(I did try to make my own shader for a _glow_ effect on my drops - however i have the wrong rendering pipepline and did not have success changing the pipeline)
- Animations(Player + Enemies)
- Gane Architecture(Used **Singleton** managers with different responisbilities: spawning, switching scenes etc)
- Game AI(Made my own scuffed version of state machine on my enemies attack modes :), made my own pathfinding algorithm)
- User Interface(yep)
