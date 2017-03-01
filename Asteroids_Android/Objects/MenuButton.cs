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

namespace Mono_test_android2
{
    class MenuButton
    {
        private Rectangle rect;
        private String label;
        private Vector2 position;
        private Texture2D texture;
        private int width;
        private int height;
        private bool isClicked = false;

        public MenuButton(Vector2 position, GraphicsDeviceManager graphics, int width, int height, String label)
        {
            this.label = label;
            this.width = width;
            this.height = height;
            this.position.X = position.X-(width/2);//incoming position should be the middle of the button so we need to -width/2 to get the left edge
            this.position.Y = position.Y-((height/2)+5);//incoming position should be the middle of the button so we need to -heigh/2 to get the top edge - we also plus 5 to make up for text being not center aligned
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

        public void draw(SpriteBatch spritebatch)
        {
            //spritebatch.Begin();
            spritebatch.Draw(texture, rect, Color.White*0.5f);
            //spritebatch.DrawString
            //spritebatch.End();
        }

        public bool getisClicked()
        {
            return isClicked;
        }

    }
}