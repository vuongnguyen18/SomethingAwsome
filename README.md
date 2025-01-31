# SomethingAwsome
This is the source code to my mini-game program, which is about the player trying to dodge where the user will try to stay alive as much as possible and "eat" a coin; for every coin that the player eats, he or she will earn 10 points. The game will record the highest score.
The program has two major parts: the animation script and the coding. 
For the animation script, it is really easy to understand. First, it is saved in TXT format. Here is the example code:
SplashKit Animation
//f:[start-end], [start-end], fram_duration
f:[0-3],[0-3],12,
f:[4-7],[4-7],12,
f:[8-11],[8-11],12,
f:[12-15],[12-15],12,
For example, f:[0-3],[0-3],12 indicating that for the frame range (X), frame from 0 to 3 and the same for frame range (Y) and the duration for that animation frame is 12 unit per frame.

//s: frame_number, sound_name, sound_file
s:0,boing,comedy_boing.ogg
s:4,boing,comedy_boing.ogg
s:8,boing,comedy_boing.ogg
s:12,boing,comedy_boing.ogg
The s: keyword adds sound effects to animations. For example, s:0,boing,comedy_boing.ogg → Plays "comedy_boing.ogg" when frame 0 is displayed.

//identifiers
//i: AnimationName, frame_index
i:WalkFront,0
i:WalkLeft,4
i:WalkRight,8
i:WalkBack,12
The i: keyword names an animation. For example, i:WalkFront,0 → "WalkFront" animation starts from frame 0.

For the program.cs, there are few things you need to know. First the program need to load the image. To load the image manually, you need to:
- Call Load Bitmap to load the sprite sheet.
- Call Bitmap Set Cell Details to set the cell details (which includes width, height, columns, rows, count).

 Then load the animation script using Load Animation Script. Followed by create an animation from the animation script using the identifier from the script. You need to create a drawing option using Option With Animation, which can be used to pass options to a draw bitmap call. Next, draw the bitmap, passing in the image, the location to draw it, and the drawing options. Last but no least, Update the animation using Update Animation.



