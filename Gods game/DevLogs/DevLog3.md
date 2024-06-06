The finishing touches... first let me talk a bit about what i did to finish the **gameplay loop**
- Toturial added in base
- Objective added
- Added proper interacts when winning / losing
- 1 more weapon for fun
- QOL
  - **Main camera** zoomed more out
  - **Enemies** have more health
  - **Enemies** spawn rate increased

**Arcade machine inputs** gave a lot of trouble, turns out i was stupid and thought having multiple gameobjects with **PlayerInput** wouldn't hurt :)
What a grave mistake... I overhauled both my thinking and the input system. As i was in a timecrunch i ended up having the **Player** gameobject have the
responisbility for all inputs made, it does work for now - but i did implement a manager for this i had to scrap as i scrambled to make the inputs work.
Im not happy with this solution but if it works it works 🥉.

I also didnt think about how i wanted to implement my inventory system for the Arcade machine. In my testing i made it work with a click of mouse using raycasting on the UI
buuut you dont have a mouse on the arcade machine... I decided on making a system that uses the first item in the inventory with a button press, as i would have to rework the
inventory logic to make it able to handle navigating using the stick. And after testing this doesn't actually feel as bad as i thought it would.

So thats it, the game provides a ton of half finished features, but i got to work with a lot of different stuff and that was my plan going in as i wanted to _experiment_ and not _perfect_
