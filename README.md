# LumberHack

## About

A Windows application for hacking the telegram game Lumberjack

Doing this as a toy project for C#, as well as to beat my friends!

It only works on IE so far, because Chrome doesnt work well with the application commands

Max thread sleep time seems to peak around 150. 

Max score with assistance: 367 (meaning in rows of no branches, I manually press left or right to speed things up)

Max score without assistance: 352

Not very impressive, but able to beat most human players!

## Underlying

When the start button is clicked, the program scans the entire screen for the following image

No Branches, and on the left

![alt text](https://github.com/Charmanderander/LumberHack/blob/master/LumberHack/images/nobranchleft.jpg "No Branches, and on the left")

No Branches, and on the right

![alt text](https://github.com/Charmanderander/LumberHack/blob/master/LumberHack/images/nobranchright.jpg "No Branches, and on the right")

It uses image recognition to calculate the similarity of the screen shot, and those images.

Next, we begin classifying the following images and sending appropriate commands. There are 6 scenarios in total:

---

Branch directly on the left: Right

![alt text](https://github.com/Charmanderander/LumberHack/blob/master/LumberHack/images/leftbranch.jpg "Branch directly on the left")

---

Branch directly on the right: Left

![alt text](https://github.com/Charmanderander/LumberHack/blob/master/LumberHack/images/rightbranch.jpg "Branch directly on the right")

---

Branch above on the left: Right

![alt text](https://github.com/Charmanderander/LumberHack/blob/master/LumberHack/images/leftbranchabove.jpg "Branch above on the left")

---

Branch above on the right: Left

![alt text](https://github.com/Charmanderander/LumberHack/blob/master/LumberHack/images/rightbranchabove.jpg "Branch above on the right")

---

Branch directly above on the left: Right

![alt text](https://github.com/Charmanderander/LumberHack/blob/master/LumberHack/images/leftbranchdirectabove.jpg "Branch directly above on the left")

---

Branch directly above on the right: Left

![alt text](https://github.com/Charmanderander/LumberHack/blob/master/LumberHack/images/rightbranchdirectabove.jpg "Branch directly above on the right")

Chop Chop!

## TODO

1. Implement look ahead. Currently, image recognition is only done around the LumberJack. If we can look at whats ahead of us, we can enter commands more quickly.

2. A better image recognition. It still does classify the images correctly in some situations (when there are no branches, and the LumberJack is on the right)

3. Multithreading for finding the starting branch. Speeds things up a little bit by spawning multiple threads to search different areas on the screen
