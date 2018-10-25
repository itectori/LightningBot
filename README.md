# LightningBot

This project contains the source code of the LightningBot viewer: https://lightningbot.tk/viewer

For more information, check out our website: https://lightningbot.tk

## Project overview

This overview is designed to help you get a general idea of how this coding game works.

First of all, keep in mind that there is a [detailed documentation](https://lightningbot.tk/doc) of every point you may have difficulties on.
The goal of the game is to blend the fun of the gaming side with the cool aspect of AI programming.
To do so you are completely free, you can choose the language you prefer!
If you have trouble getting started, there are some code samples in the [documentation](https://lightningbot.tk/doc).
If you need help feel free to ask questions to our community on [our Discord server](https://discordapp.com/invite/wxzd7Us).

To help you visualize how the game works, here is an example of the stages a player may go through.

### Stage 0

The player uses the code samples provided in the [docs](https://lightningbot.tk/doc).
He only needs to run it, his bot already goes in a straight line!
Don't forget that for a game to start there must be at least 2 bots playing. If nobody else is playing try to launch 2 of your bots.

### Stage 1

The player choses a random direction each turn.
His bot will not survive for long and will probably kill himself (invalid move) because it went to the opposite direction it was previously facing.

### Stage 2

At this point, he tries not to kill himself by storing the value of the direction of the previous turn in a variable and excluding its opposite from the possible directions of the current turn.
His bot can still die on his tail but at least there will be no invalid moves.

### Stage 3

Now he considers using Object Oriented Programming. He plans to create very simples classes that he will use to update a map of the game in order to keep track of all the information. During the info phase he aquires the dimension of the map and the starting positions of all the bots.

Finally, he his now at the point where he can think of his own way to guide his bot instead of using randomness.
Some methods he might consider are:
- Dijkstra/A*
- Voronoï
- Machine learning
