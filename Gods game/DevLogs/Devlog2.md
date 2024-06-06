So for the past time i finished first up all the **core elements**, however most is still 'individual' pieces. I made my own scuffed world **generation** and **pathing**(Doesn't work 100%) to make the **combat** as finished as i could.
I ended up scrapping the **Map** for now, as it doesn't make sense to draw your own map on an arcade machine.

Now that i have a lot of pieces to work with i thought it would be a good time to get started on rewamping the architecture... however i decided on not to do it - mainly i think the architecture isn't terrible, buuuut that being said
there's a lot of room for improvement. 

Mostly it feels articial as it shouldn't change the gameplay but would be helpfull for me going forward, but since the project is nearing it's end it wouldn't make sense to go to crazy on rewamping my architecture as im not sure i want
to continue with this exact project after the project is done. But i did make a list of what i wanted to work on if i got to that point.
- Make testing easier
  - Have to start from **base** or **menu** scenes right now.
  - Maybe make a test scene?
  - If i want to test an enemys interactions i have to start in **base** go to **world** then find an enemy
  - **Environment** is prefabs this felt right to do at the time, however now i have to play the game, as they are being loaded by my **init-** managers
- Better overwiew
  - Diagram: How GameObjects interact
  - Diagram: High level overview of how the classes interact
- Code Optimization/Fix
  - My code is VERY homebrewed (LOS, Pathing, WorldGeneration) - these can surely be optimized greatly
  - Consistent naming schemes - _(Sorry about this for whoever is gonna read my code ;) )_
  - _maybe_ rework how classes interact - THINK more about how managers can solve some issues
  - I have a few instances of _sort of duplicated_ code - i think i might be able to make some things more generic to fix this.


New focus:
- Tie up the game with an actual gameplay loop 

The game might have combat, however there isnt really a **start**, **end** & **objective** atm, i want to make that :)
