# LumberHack

A Windows application for hacking the telegram game Lumberjack

Doing this as a toy project for C#, as well as to beat my friends!

It only works on IE so far, because Chrome doesnt work well with the application commands

Max thread sleep time seems to peak around 150. 

Max score with assistance: 367 (meaning in rows of no branches, I manually press left or right to speed things up)

Max score without assistance: 352

Not very impressive, but able to beat most human players!

## TODO

1. Implement look ahead. Currently, image recognition is only done around the LumberJack. If we can look at whats ahead of us, we can enter commands more quickly.

2. A better image recognition. It still does classify the images correctly in some situations (when there are no branches, and the LumberJack is on the right)

3. Multithreading for finding the starting branch. Speeds things up a little bit by spawning multiple threads to search different areas on the screen
