# Farkle
Simple 2-player farkle game for AI testing, now communicating through standard output/input.

## Rules of the game
Farkle is a simple dice game for any number of players (our version supporting 2 players), played with 6 dices.
The goal of the game is to score 10 000 points. At each turn, a player rolls all six dices, and the goal is to achieve at least 350 points, which is the least amount a player can score in turn. After each roll, the player must choose whether to score current points he has, or to keep some of the dices and reroll the rest. However, the turn also ends immediately if the player cant keep any of the dices he just rolled. Also, at each turn, a player has only 3 attempts in rolling dices to pass the 350 points bound. If all 6 dices were rolled and kept, the player rolls all six dices once more.
The scoring of the dices is as follows:
* single 1 - 100 points  
* single 5 - 50 points
* three or more dices with the same value - (number of dices - 2) * (1000/200/300/400/500/600 for dice values 1/2/3/4/5/6 respectively)

## Usage
The game is implemented in Farkle/Game folder, and be either played with external players (implemented as dlls), or locally through console. To use the external version, the application expects two arguments, filenames of both players' dlls.

### Communication protocol
 The communication between the players/bots and the game happens through one-line text messages written according to the following protocol. Every message ends with a newline.
#### Messages from the game
* *START* - start of the game
* *PLAY* - start of the player's turn
* *SUCCESS* - last player's action was successful
* *CURRENT-SCORE {X}* - player's score in the current turn is X
* *DICES {A} {B} {C} {D} {E} {F}* - player currently has dices with values A-F on board
* *FAILURE* - last player's action failed
* *TURN-OVER* - end of the player's turn
* *OPPONENT-SCORE {X}* - score of the opponent is X (information sent after opponent's turn)
* *GAME-OVER* - end of the game
* *-- XYZ...* - comments from the game, not to be cared about by the bot
#### Messages from the bots
* *SCORE* - score currently kept and all keepable lastly rolled dices
* *KEEP {A} {B} {C}...* - keep dices with ONE-BASED indices A, B, C.   

### Example communication
> **GAME to all**         *START*
>
> **GAME to all**         *-- Game starts!*
>
> **GAME to all**         *-- Turn starts.*
>
> **GAME to all**         *-- Current player: Player number 0, score 0.*
>
> **GAME to player 1**    *PLAY*
>
> **GAME to player 1**    *-- Roll successful.*
>
> **GAME to player 1**    *CURRENT-SCORE 0*
>
> **GAME to player 1**    *DICES 4 1 4 3 4 6*
>
> **Player 1 to GAME**    *KEEP 1 3 5 2*
>
> **GAME to player 1**    *SUCCESS*
>
> **GAME to player 1**    *-- Roll successful.*
>
> **GAME to player 1**    *CURRENT-SCORE 500*
>
> **GAME to player 1**    *DICES 4 1 4 2 4 5*
>
> **Player 1 to GAME**    *SCORE*
>
> **GAME to player 1**    *SUCCESS*
>
> **GAME to player 1**    *-- Turn ends.*
>
> **GAME to player 1**    *TURN-OVER*
>
> **GAME to player 2**    *OPPONENT-SCORE 550*
>
> **GAME to all**         *-- Turn starts.*
>
> **GAME to all**         *-- Current player: Player number 1, score 0.*
>
> *...*

