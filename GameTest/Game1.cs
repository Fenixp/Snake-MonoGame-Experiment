﻿#region Using Statements
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.Diagnostics;
#endregion

namespace GameTest
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        List<Square> gameObjects;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D texture1px;
        Texture2D blackSquare;
        Square squareObject;
        Snake snake;
        Random rnd = new Random();
        int gameSpeed = 100;
        double lastTick;
        int windowSizeY = 600;
        int windowSizeX = 600;
        int cols = 15;
        int rows = 15;
        int centerX;
        int centerY;
        int gridSize = 20;
        int height;
        int width;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            centerX = windowSizeX / 2;
            centerY = windowSizeY / 2;
            height = windowSizeY;
            width = windowSizeX;
            gameObjects = new List<Square>();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = windowSizeX;
            graphics.PreferredBackBufferHeight = windowSizeY;
            graphics.ApplyChanges();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            blackSquare = Content.Load<Texture2D>("Square.png");
            squareObject = new Square(29, 0);
            snake = new Snake(windowSizeX / gridSize, windowSizeY / gridSize);
            texture1px = new Texture2D(graphics.GraphicsDevice, 1, 1);
            texture1px.SetData(new Color[] { Color.Black });
            lastTick = 0;
            gameObjects.AddRange(snake.Squares);
            gameObjects.Add(squareObject);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                snake.Direction = DirectionEnum.down;
            }
            if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                snake.Direction = DirectionEnum.up;
            }
            if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                snake.Direction = DirectionEnum.left;
            }
            if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                snake.Direction = DirectionEnum.right;
            }

            double hello = gameTime.TotalGameTime.TotalMilliseconds - lastTick;

            snake.Neighbours = CheckNeighbours();

            if (gameTime.TotalGameTime.TotalMilliseconds - lastTick > gameSpeed)
            {
                lastTick = gameTime.TotalGameTime.TotalMilliseconds;
                snake.Move();
            }

            if (snake.Squares.Contains(squareObject))
            {
                squareObject = new Square(rnd.Next(windowSizeX / gridSize), rnd.Next(windowSizeY / gridSize));
                gameObjects.Add(squareObject);
                gameSpeed -= 1;
            }

            Debug.WriteLine(gameTime.TotalGameTime.ToString() + " : " + hello.ToString());

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            for (float x = -cols; x < cols; x++)
            {
                Rectangle rectangle = new Rectangle((int)(centerX + x * gridSize), 0, 1, height);
                spriteBatch.Draw(texture1px, rectangle, Color.Red);
            }
            for (float y = -rows; y < rows; y++)
            {
                Rectangle rectangle = new Rectangle(0, (int)(centerY + y * gridSize), width, 1);
                spriteBatch.Draw(texture1px, rectangle, Color.Red);
            }

            spriteBatch.Draw(blackSquare, new Vector2(squareObject.XCoord * gridSize, squareObject.YCoord * gridSize), Color.White);

            foreach(Square square in snake.Squares)
            {
                spriteBatch.Draw(blackSquare, new Vector2(square.XCoord * gridSize, square.YCoord * gridSize), Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private Neighbours CheckNeighbours()
        {
            Neighbours neighbours = new Neighbours();

            foreach (Square gameObject in gameObjects)
            {
                if(CheckNeighbour(-1, 0, gameObject, snake.Head))
                {
                    neighbours.SetDirection(DirectionEnum.left, gameObject);
                }
                if (CheckNeighbour(1, 0, gameObject, snake.Head))
                {
                    neighbours.SetDirection(DirectionEnum.right, gameObject);
                }
                if (CheckNeighbour(0, -1, gameObject, snake.Head))
                {
                    neighbours.SetDirection(DirectionEnum.up, gameObject);
                }
                if(CheckNeighbour(0, 1, gameObject, snake.Head))
                {
                    neighbours.SetDirection(DirectionEnum.down, gameObject);
                }
            }

            return neighbours;
        }

        private bool CheckNeighbour(int x, int y, Square object1, Square object2)
        {
            if (object1.XCoord == CalculateCoords(object2.XCoord + x)
                && object1.YCoord == CalculateCoords(object2.YCoord + y))
            {
                return true;
            }

            return false;
        }

        private int CalculateCoords(int coord)
        {
            if (coord < 0)
            {
                coord = coord + windowSizeX / gridSize;
            }
            return coord % (windowSizeX / gridSize);
        }
    }
}