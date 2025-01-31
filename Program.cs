using System;
using SplashKitSDK;

public class AnimationsDemo
{
    public static void Main()
    {
        // Initialize the window
        const int WindowWidth = 800;
        const int WindowHeight = 550;
        Window w = new Window("SplashKit Animations Demo", WindowWidth, WindowHeight);

        // Load resources
        Bitmap characterBitmap = SplashKit.LoadBitmap("character", "Player.png");
        Bitmap backgroundBitmap = SplashKit.LoadBitmap("backGround", "BackGround.png");
        Bitmap bulletBitmap = SplashKit.LoadBitmap("bullet", "bullet.png");
        Bitmap coinBitmap = SplashKit.LoadBitmap("coin", "coin.png");

        if (characterBitmap == null || backgroundBitmap == null || bulletBitmap == null || coinBitmap == null)
        {
            Console.WriteLine("Failed to load resources. Check file paths.");
            return;
        }

        // Set up sprite sheet details
        characterBitmap.SetCellDetails(46, 66, 4, 4, 16); // (width, height, cols, rows, total count)
        string animScriptPath = "animations.txt";
        AnimationScript animScript = SplashKit.LoadAnimationScript("sprite_animations", animScriptPath);

        // Create and set up frog sprite
        Sprite frogSprite = new Sprite(characterBitmap, animScript);
        frogSprite.StartAnimation("WalkFront"); // Default animation

        // Set initial position of the frog to the center of the window and move it down by 10 pixels
        frogSprite.X = (WindowWidth - frogSprite.Width) / 2;
        frogSprite.Y = (WindowHeight - frogSprite.Height) / 2 + 10;

        // Create and set up red dot sprite
        Sprite bulletSprite = new Sprite(bulletBitmap);
        bulletSprite.X = SplashKit.Rnd(WindowWidth);
        bulletSprite.Y = -bulletBitmap.Height; // Start above the window

        // Create and set up star sprite
        Sprite coinSprite = new Sprite(coinBitmap);
        coinSprite.X = SplashKit.Rnd(WindowWidth - coinBitmap.Width);
        coinSprite.Y = SplashKit.Rnd(WindowHeight - coinBitmap.Height);

        // Movement and animation control variables
        double frogSpeed = 4;
        double backgroundX1 = 0;
        double backgroundX2 = WindowWidth;
        double frogScale = 1.0;
        double frogRotation = 0;
        double bulletSpeed = 2;
        int score = 0;
        int highScore = 0;

        // Background scaling
        const double BackgroundScaleX = (double)WindowWidth / 185.0;
        const double BackgroundScaleY = (double)WindowHeight / 174.0;

        // Main game loop
        while (!w.CloseRequested)
        {
            SplashKit.ProcessEvents();

            bool moving = false;

            // Character movement and animation
            if (SplashKit.KeyDown(KeyCode.RightKey))
            {
                frogSprite.X += (float)frogSpeed;
                frogSprite.StartAnimation("WalkRight");
                frogScale = 1.0;  // Normal size
                frogRotation = 0; // Face right
                moving = true;
            }
            if (SplashKit.KeyDown(KeyCode.LeftKey))
            {
                frogSprite.X -= (float)frogSpeed;
                frogSprite.StartAnimation("WalkLeft");
                frogScale = 1.0;  // Normal size
                frogRotation = 180; // Flip to face left
                moving = true;
            }
            if (SplashKit.KeyDown(KeyCode.UpKey))
            {
                frogSprite.Y -= (float)frogSpeed;
                frogSprite.StartAnimation("WalkBack");
                moving = true;
            }
            if (SplashKit.KeyDown(KeyCode.DownKey))
            {
                frogSprite.Y += (float)frogSpeed;
                frogSprite.StartAnimation("WalkFront");
                moving = true;
            }

            // Update red dot position
            bulletSprite.Y += (float)bulletSpeed;
            if (bulletSprite.Y > WindowHeight)
            {
                bulletSprite.X = SplashKit.Rnd(WindowWidth);
                bulletSprite.Y = -bulletBitmap.Height; // Reset to top
            }

            // Check for collision between frog and red dot
            if (SplashKit.SpriteCollision(frogSprite, bulletSprite))
            {
                Console.WriteLine("Game Over!");
                if (score > highScore)
                {
                    highScore = score;
                }
                ShowGameOverDialog(score, highScore);
                score = 0; // Reset score for new game
                frogSprite.X = (WindowWidth - frogSprite.Width) / 2;
                frogSprite.Y = (WindowHeight - frogSprite.Height) / 2;
                bulletSprite.X = SplashKit.Rnd(WindowWidth);
                bulletSprite.Y = -bulletBitmap.Height; // Reset bullet position
                coinSprite.X = SplashKit.Rnd(WindowWidth - coinBitmap.Width);
                coinSprite.Y = SplashKit.Rnd(WindowHeight - coinBitmap.Height);
                // Make sure the screen is cleared before continuing the game loop
            }

            // Check for collision between frog and star
            if (SplashKit.SpriteCollision(frogSprite, coinSprite))
            {
                score += 10;
                Console.WriteLine($"Score: {score}");
                coinSprite.X = SplashKit.Rnd(WindowWidth - coinBitmap.Width);
                coinSprite.Y = SplashKit.Rnd(WindowHeight - coinBitmap.Height);
            }

            // Background scrolling (seamless effect)
            backgroundX1 -= 2;
            backgroundX2 -= 2;
            if (backgroundX1 <= -WindowWidth)
            {
                backgroundX1 = backgroundX2 + WindowWidth;
            }
            if (backgroundX2 <= -WindowWidth)
            {
                backgroundX2 = backgroundX1 + WindowWidth;
            }

            // Update frog sprite animation
            frogSprite.Update();

            // Clear and draw everything
            SplashKit.ClearScreen(Color.White);

            // Draw the scrolling background
            SplashKit.DrawBitmap(backgroundBitmap, backgroundX1, 0, SplashKit.OptionScaleBmp(BackgroundScaleX, BackgroundScaleY));
            SplashKit.DrawBitmap(backgroundBitmap, backgroundX2, 0, SplashKit.OptionScaleBmp(BackgroundScaleX, BackgroundScaleY));

            // Apply scaling & rotation while drawing the frog
            DrawingOptions options = SplashKit.OptionRotateBmp((float)frogRotation);
            options = SplashKit.OptionScaleBmp(frogScale, frogScale, options);
            SplashKit.DrawSprite(frogSprite);

            // Draw the red dot sprite
            SplashKit.DrawSprite(bulletSprite);

            // Draw the star sprite
            SplashKit.DrawSprite(coinSprite);

            // Draw the current score
            SplashKit.DrawText($"Score: {score}", Color.Black, 10, 10);

            // Refresh the screen
            SplashKit.RefreshScreen(60);
        }
    }
   private static void ShowGameOverDialog(int score, int highScore)
{
    Window dialog = new Window("Game Over", 400, 300);
    //ShowGameOverDialog function is using "Arial" as the font, 
    //but SplashKit requires you to load a font before using it.
    Font arialFont = SplashKit.LoadFont("Arial", "arial.ttf");

    while (!dialog.CloseRequested)
    {
        SplashKit.ProcessEvents();
        dialog.Clear(Color.White);
        dialog.DrawText("Game Over!", Color.Black, arialFont, 24, 100, 40);
        dialog.DrawText($"Score: {score}", Color.Black, arialFont, 20, 100, 90);
        dialog.DrawText($"High Score: {highScore}", Color.Black, arialFont, 20, 100, 140);
        dialog.DrawText("Press C to Continue or E to Exit", Color.Black, arialFont, 14, 50, 200);
        dialog.Refresh();

        if (SplashKit.KeyTyped(KeyCode.CKey))
        {
            dialog.Close();
            break;
        }
        if (SplashKit.KeyTyped(KeyCode.EKey))
        {
            Environment.Exit(0);
        }
    }
}

    
}