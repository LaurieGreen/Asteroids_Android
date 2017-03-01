using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Mono_test_android2
{
    public class BasicMenu
    {
        int finalSelection = -10;
        //public bool isNew = false;
        int currentSelection;
        List<String> choiceList;
        List<MenuButton> buttonList;
        String title;
        SpriteFont small_font, medium_font, large_font;
        int screenwidth;
        int screenheight;
        
        public BasicMenu(SpriteFont small_font, SpriteFont medium_font, SpriteFont large_font, String title,  List<String> optionList, GraphicsDeviceManager graphics)
        {
            screenwidth = graphics.GraphicsDevice.Viewport.Width;
            screenheight = graphics.GraphicsDevice.Viewport.Height;
            this.small_font = small_font;
            this.medium_font = medium_font;
            this.large_font = large_font;
            this.title = title;
            this.choiceList = optionList;
            buttonList = new List<MenuButton>(optionList.Count);

            for(int i = 0; i < optionList.Count;i++)
            {
                buttonList.Add(new MenuButton(new Vector2((screenwidth/2), ((screenheight/8)*6+((small_font.MeasureString(optionList[i]).Y)*i))),graphics,500, 70, choiceList[i]));
            }
            currentSelection = 0;
        }

        public void setCurrentSelection(int i)
        {
            currentSelection = i;
        }

        public void setFinalSelection(int i)
        {
            finalSelection = i;
        }
        public int getFinalSelection()
        {
            return finalSelection;
        }

        public int getCurrentSelection()
        {
            return currentSelection;
        }
        public void MoveSelectionUp()
        {
            if (currentSelection > 0)
            {
                currentSelection--;
                if (GameConstants.Debug)
#pragma warning disable CS0162 // Unreachable code detected
                    System.Console.WriteLine("up: " + currentSelection);
#pragma warning restore CS0162 // Unreachable code detected
            }
        }

        public void MoveSelectionDown()
        {
            if (currentSelection < choiceList.Count-1)
            {
                currentSelection++;
                if (GameConstants.Debug)
#pragma warning disable CS0162 // Unreachable code detected
                    System.Console.WriteLine("down: " + currentSelection);
#pragma warning restore CS0162 // Unreachable code detected
            }
        }

        public void Update(KeyboardState state, KeyboardState lastState)
        {
            int x = 0;
            int y = 0;
            bool isInputPressed = false;
            TouchCollection touchPanelState = TouchPanel.GetState();

            if (touchPanelState.Count >= 1)
            {
                var touch = touchPanelState[0];
                x = (int)touch.Position.X;
                y = (int)touch.Position.Y;

                isInputPressed = touch.State == TouchLocationState.Pressed || touch.State == TouchLocationState.Moved;
            }

            for (int i = 0; i < buttonList.Count; i++)
            {
                buttonList[i].update(new Vector2(x, y));
                if (buttonList[i].getisClicked())
                {
                    finalSelection = i;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Color color, GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(large_font, "ASTEROIDS", new Vector2(screenwidth / 2 - (large_font.MeasureString("ASTEROIDS").Length() / 2), screenheight / 16), Color.White);
            spriteBatch.DrawString(medium_font, title, new Vector2(screenwidth / 2 - (medium_font.MeasureString(title).Length() / 2), screenheight / 5 + (medium_font.MeasureString(title).Y)), Color.White);
            // Pulsate the size of the selected menu entry.
            double time = gameTime.TotalGameTime.TotalSeconds;
            float pulsate = (float)Math.Sin(time * 6) + 1;
            float scale;
            Vector2 origin = new Vector2(0, small_font.LineSpacing / 2);
            Color choiceColor = new Color();
            for (int i = 0; i < choiceList.Count(); i++)
            {
               
                if (currentSelection == i)
                {
                    choiceColor = new Color(0, 204, 0);//green
                    scale = 1 + pulsate * 0.05f;
                }
                else
                {
                    choiceColor = new Color(255, 255, 255);//white
                    scale = 1;
                }
                buttonList[i].draw(spriteBatch);
                spriteBatch.DrawString(small_font, choiceList[i], new Vector2(screenwidth / 2, (screenheight / 8)* 6 + ((small_font.MeasureString(choiceList[i]).Y)*i)), choiceColor, 0, new Vector2(small_font.MeasureString(choiceList[i]).Length() / 2, small_font.MeasureString(choiceList[i]).Y / 2), scale, SpriteEffects.None, 0);
            }
            spriteBatch.End();

        }
    }
}
