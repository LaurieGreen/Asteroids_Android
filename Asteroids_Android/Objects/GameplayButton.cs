using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids_Android
{
    class GameplayButton
    {
        Rectangle rect;
        String label;
        Vector2 position;
        Texture2D texture;
        int width;
        int height;
        int screenWidth;
        int screenHeight;
        bool isClicked = false;
        SpriteFont font;

        public GameplayButton(Vector2 position, GraphicsDeviceManager graphics, int width, int height)
        {
            screenWidth = graphics.GraphicsDevice.Viewport.Width;
            screenHeight = graphics.GraphicsDevice.Viewport.Height;
            this.width = width;
            this.height = height;
            this.position.X = position.X;
            this.position.Y = position.Y;
            rect = new Rectangle((int)this.position.X, (int)this.position.Y, width, height);
            texture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            texture.SetData(new[] { Color.White });
        }

        public void update(Vector2 click)
        {
            if (rect.Contains(click))
            {
                isClicked = true;
                System.Console.WriteLine("clicked");
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            //spritebatch.Begin();
            spriteBatch.Draw(texture, rect, Color.White * 0.5f);
            //spriteBatch.DrawString(font, label, new Vector2((position.X + ((width / 2) - (font.MeasureString(label).X / 2))), (position.Y + ((height / 2) - (font.MeasureString(label).Y / 2)))), Color.White);
            //spritebatch.DrawString
            //spritebatch.End();
        }

        public bool getisClicked()
        {
            return isClicked;
        }

        public void setIsClicked(bool b)
        {
            isClicked = b;
        }

        public Rectangle getRect()
        {
            return rect;
        }

    }
}