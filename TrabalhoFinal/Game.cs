using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System;

namespace TrabalhoFinal;

public class Engine : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private float width { get; }
    private float height { get; }

    //
    private Texture2D[,] textureCubes = new Texture2D[1, 5];
    private Texture2D textureSeta;
    private int cube = 0, face = 0;
    // interactions
    MouseState mouseAtual, mouseAnterior;
    // areas 
    Rectangle areaSetaDireita, areaSetaEsquerda, areaSetaCima, areaSetaBaixo;

    public Engine()
    {
        _graphics = new GraphicsDeviceManager(this);
        width = 1280f;
        height = 720f;
        _graphics.PreferredBackBufferWidth = (int)width;   // tamXura
        _graphics.PreferredBackBufferHeight = (int)height;   // altura

        _graphics.ApplyChanges();

        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        //sprites
        string[] locals = {"data/a.png",
                         "data/b.png",
                         "data/c.png",
                         "data/d.png",
                         "data/e.png"
                        };
        for (int c = 0; c < 5; c++)
        {
            using var str = new FileStream(locals[c], FileMode.Open);
            // textureCubes[0] = new Texture2D[6];
            textureCubes[0, c] = Texture2D.FromStream(GraphicsDevice, str);
        }
        textureSeta = Texture2D.FromStream(GraphicsDevice, new FileStream("data/seta.png", FileMode.Open));
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        mouseAnterior = mouseAtual;
        mouseAtual = Mouse.GetState();
        Point mousePos = new Point(mouseAtual.X, mouseAtual.Y);

        if (mouseAnterior.LeftButton == ButtonState.Pressed &&
        mouseAtual.LeftButton == ButtonState.Released && areaSetaDireita.Contains(mousePos))
        {
            if (face < 3) face++; else face = 0;
            Console.WriteLine("Clicou em cima da seta direita!");
        }else if (mouseAnterior.LeftButton == ButtonState.Pressed &&
        mouseAtual.LeftButton == ButtonState.Released && areaSetaEsquerda.Contains(mousePos))
        {
            if (face > 0) face--; else face = 3;
            Console.WriteLine("Clicou em cima da seta esquerda!");
        }else if (mouseAnterior.LeftButton == ButtonState.Pressed &&
        mouseAtual.LeftButton == ButtonState.Released && areaSetaCima.Contains(mousePos))
        {
            Console.WriteLine("Clicou em cima da seta de cima!");
        }else if (mouseAnterior.LeftButton == ButtonState.Pressed &&
        mouseAtual.LeftButton == ButtonState.Released && areaSetaBaixo.Contains(mousePos))
        {
            Console.WriteLine("Clicou em cima da seta de baixo!");
        }


        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
        //
        // 400,295
        int tamX = 400;
        int tamY = 295;
        int padX = 5, padY = 5;
        float zoom = 1f;

        // TODO: Add your drawing code here
        var flip = SpriteEffects.None;  // flipDrawSprite ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        _spriteBatch.Draw(textureCubes[cube, face], new Vector2(width / 2 - tamX / 2, height / 2 - tamY / 2), new Rectangle(padX, padY, tamX, tamY), Color.White, 0f, Vector2.Zero, zoom, flip, 0); // 192 x 96

        setas(tamX, tamY, padX, padY, zoom);
        // flip = SpriteEffects.FlipHorizontally;
        // rotation = MathHelper.PiOver2;
        //
        _spriteBatch.End();
        base.Draw(gameTime);
    }

    private void setas(int tamX, int tamY, int padX, int padY, float zoom)
    {
        // direita
        int tamY2 = 20;
        int tamX2 = 17;
        int marginX = 5;
        var rotation = 0f;
        var flip = SpriteEffects.None;

        var position = new Vector2(width / 2 + tamX / 2 - tamX2 - marginX, height / 2 - tamY2 / 2);
        var cut = new Rectangle(padX, padY, tamX2, tamY2);
        areaSetaDireita = new Rectangle(
            (int)position.X,
            (int)position.Y,
            cut.Width,
            cut.Height
        );
        _spriteBatch.Draw(textureSeta, position, cut, Color.White, rotation, Vector2.Zero, zoom, flip, 0);

        // esquerda
        flip = SpriteEffects.FlipHorizontally;
        position = new Vector2(width / 2 - tamX / 2 + marginX, height / 2 - tamY2 / 2);
        cut = new Rectangle(padX, padY, tamX2, tamY2);
        areaSetaEsquerda = new Rectangle(
            (int)position.X,
            (int)position.Y,
            cut.Width,
            cut.Height
        );
        _spriteBatch.Draw(textureSeta, position, cut, Color.White, rotation, Vector2.Zero, zoom, flip, 0);
        
    }
}
