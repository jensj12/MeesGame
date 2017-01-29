using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace MeesGame
{
    internal class SettingsMenuState : IGameLoopObject
    {
        private UIComponent overlay;
        private Slider slider;
        private Textbox textbox;

        public SettingsMenuState()
        {
            overlay = new UIComponent(SimpleLocation.Zero, InheritDimensions.All);

            slider = new Slider(new SimpleLocation(0, 75), new InheritDimensions(false, false, 500, 50));
            textbox = new Textbox(new SimpleLocation(25, 0), InheritDimensions.All, Strings.volume);

            overlay.AddChild(slider);
            overlay.AddChild(textbox);

            //Button for returning to the titlemenu
            overlay.AddChild(new SpriteSheetButton(new SimpleLocation(25, 120), null, Strings.menu, (UIComponent o) =>
            {
                GameEnvironment.GameStateManager.SwitchTo("TitleMenuState");
            }));

            //Button for quitting the game
            overlay.AddChild(new SpriteSheetButton(new SimpleLocation(25, 230), null, Strings.exit, (UIComponent o) =>
            {
                GameEnvironment.Instance.Exit();
            }));
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            overlay.Draw(gameTime, spriteBatch);
        }

        public void HandleInput(InputHelper inputHelper)
        {
            overlay.HandleInput(inputHelper);
        }

        public void Reset()
        {
            overlay.Reset();
        }

        public void Update(GameTime gameTime)
        {
            overlay.Update(gameTime);
            SoundEffect.MasterVolume = (float)slider.Value / 100;   //Changes the sound level (everywhere, not only in the settingsmenu) when the slider is moved
        }
    }
}
