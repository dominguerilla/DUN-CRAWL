# DUN-CRAWL
Generic dungeon generation development. For use in other projects.

## The Blind Digger (v1.0: hallways)
The Blind Digger uses the first agent based dungeon-growing algorithm described in Chapter 3 of Shaker's "Procedural Content Generation in Games". The BD, given an initial starting position, will pick a direction relative to his location and places some floor. The longer he continues in a specific direction, the more likely he will turn and switch directions. Whenever he turns, his chance of switching directions is reset back to 0.

The only adjustment available to his generation algorithm is his 'changeDirDelta': how much the chance that he turns increases as he continues in one direction. The higher this delta is, the more chaotic his hallways, as well as his chances of trapping himself.

![blind digger 6](https://raw.githubusercontent.com/dominguerilla/DUN-CRAWL/develop/Images/Delta2.5/6.PNG)
![blind digger 2](https://raw.githubusercontent.com/dominguerilla/DUN-CRAWL/develop/Images/Delta2.5/2.PNG)
![blind digger 1](https://raw.githubusercontent.com/dominguerilla/DUN-CRAWL/develop/Images/Delta2.5/1.PNG)

## Growing Tree (v0.9.2: hallways and rooms)
This generator is an implementation of the algorithm described by Bob Nystrom. It will generate a random selection of rooms and then fill the empty gaps with corridors.

Currently, corridors and rooms are generated without being connected to each other.

![growing tree 1](https://raw.githubusercontent.com/dominguerilla/DUN-CRAWL/develop/Images/Growing Tree 0.9.2.png)

## Sources
Shaker, Noor, et al. ‚ÄúProcedural Content Generation in Games.‚Äù Procedural Content Generation in Games, pcgbook.com/.
Nystrom, Bob. ìRooms and Mazes: A Procedural Dungeon Generator.î Rooms and Mazes: A Procedural Dungeon Generator ñ Journal.stuffwithstuff.com, 21 Dec. 2014, journal.stuffwithstuff.com/2014/12/21/rooms-and-mazes/.
