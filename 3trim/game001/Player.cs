
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System;

namespace game001;
public class Player
{
    public Texture2D Texture { get; set; }
    public Vector2 _playerPosition = new Vector2(350,200);
    public float Speed { get; set; }
    private Texture2D _playerTexture;
    private int lineDrawSprite = 1, columnDrawSprite = 1;
    private bool flipDrawSprite = false;
    // ...existing code...
    // private GraphicsDevice _graphicsDevice;
    private SpriteBatch _spriteBatch;
// ...existing code...
    KeyboardState keyboardState;
    

    public Player(GraphicsDevice graphicsDevice)
    {
        // Texture = texture;
        Speed = 200; // Velocidade em pixels por segundo
        _spriteBatch = new SpriteBatch(graphicsDevice);

        // Declaração da variável na classe Game1

        // Dentro do método LoadContent()
        using var str1 = new FileStream("data/Walk.png", FileMode.Open);
        _playerTexture = Texture2D.FromStream(graphicsDevice, str1);
    }
    private void println(string texto)
    {
        Console.WriteLine(texto);
    }
    private void print(string texto)
    {
        Console.Write(texto);
    }

    public void Update(GameTime gameTime)
    {
        keyboardState = Keyboard.GetState();
        Vector2 direction = Vector2.Zero;
        var flip = flipDrawSprite;
        if (keyboardState.IsKeyDown(Keys.W))
        {
            direction.Y -= 1;
            lineDrawSprite = 1;
            columnDrawSprite += -1;
        }
        else if (keyboardState.IsKeyDown(Keys.S))
        {
            direction.Y += 1;
            lineDrawSprite = 0;
            columnDrawSprite += 1;
        }
        else if (keyboardState.IsKeyDown(Keys.A))
        {
            direction.X -= 1;
            lineDrawSprite = 2;
            columnDrawSprite += 1;
            flip = true;
        }
        else if (keyboardState.IsKeyDown(Keys.D))
        {
            direction.X += 1;
            lineDrawSprite = 2;
            columnDrawSprite += 1;
            flip = false;
            println("D");
        }

        // Normalizar o vetor de direção para evitar movimento diagonal mais rápido
        if (direction != Vector2.Zero)
        {
            direction.Normalize();
        }
        var _playerSpeed = 1;
        if (direction != Vector2.Zero)
        {
            direction.Normalize();
        }
        // Problema: movimento dependente de framerate
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _playerPosition += direction * _playerSpeed;
        println("ola: "+gameTime.ElapsedGameTime.TotalSeconds);
        flipDrawSprite = flip;

    }

    public void Draw()
    {
        _spriteBatch.Begin();
        int larg = 192, alt = 96;
        int step = 6 * 6;
        if (columnDrawSprite > step-1)
        {
            columnDrawSprite = 0;
        }
        else if (columnDrawSprite < 0)
        {
            columnDrawSprite = step-1;
        }
        _spriteBatch.Draw(_playerTexture, _playerPosition, new Rectangle(larg / 6 * (columnDrawSprite/(step/6)), (int)(alt/3*lineDrawSprite), larg / 6, alt/3), Color.White,0f,Vector2.Zero,4f,flipDrawSprite ? SpriteEffects.FlipHorizontally : SpriteEffects.None,0); // 192 x 96

        Console.WriteLine(""+_playerPosition);

        _spriteBatch.End();
    }
}
