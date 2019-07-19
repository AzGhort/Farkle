# Farkle
Simple 2-player farkle game for AI testing, now communicating through standard output/input.

## Rules of the game
Farkle is a simple dice game for any number of players (our version supporting 2 players), played with 6 dices.
The goal of the game is to score 10 000 points. At each turn, a player rolls all six dices, and the goal is to achieve at least 350 points, which is the least amount a player can score in turn. After each roll, the player must choose whether to score current points he has, or to keep some of the dices and reroll the rest. However, the turn also ends immediately if the player cant keep any of the dices he just rolled. Also, at each turn, a player has only 3 attempts in rolling dices to pass the 350 points bound. If all 6 dices were rolled and kept, the player rolls all six dices once more.
The scoring of the dices is as follows:
* single 1 - 100 points  
* single 5 - 50 points
* 3 or more dices with the same value - (number of dices - 2) * (1000/200/300/400/500/600 for dice values 1/2/3/4/5/6 respectively)
