using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace MeesGame
{
    internal class SettingsMenuState : IGameLoopObject
    {
        private UIComponent overlay;
        private UIComponent controls;
        private Slider slider;
        private Textbox textbox;

        public SettingsMenuState()
        {
            overlay = new UIComponent(SimpleLocation.Zero, InheritDimensions.All);

            slider = new Slider(new SimpleLocation(0, 75), new InheritDimensions(false, false, 500, 50));
            textbox = new Textbox(new SimpleLocation(25, 0), InheritDimensions.All, Strings.volume);
            controls = new UIComponent(SimpleLocation.Zero, InheritDimensions.All);

            overlay.AddChild(slider);
            overlay.AddChild(textbox);
            overlay.AddChild(controls);

            //Button for returning to the titlemenu
            overlay.AddChild(new SpriteSheetButton(new SimpleLocation(25, 120), null, Strings.menu, (UIComponent o) =>
            {
                GameEnvironment.GameStateManager.SwitchTo("TitleMenuState");
            }));

            //Button for quitting the game
            overlay.AddChild(new SpriteSheetButton(new SimpleLocation(25, 230), null, Strings.quit, (UIComponent o) =>
            {
                GameEnvironment.Instance.Exit();
            }));

            //Button for returning to the previous gamestate
            overlay.AddChild(new SpriteSheetButton(new SimpleLocation(25, 340), null, Strings.back, (UIComponent o) =>
            {
                GameEnvironment.GameStateManager.SwitchTo(GameEnvironment.GameStateManager.PreviousGameState);
            }));

            //Adds the lines of text for the controls guide
            controls.AddChild(new Textbox(new SimpleLocation(576, 0), InheritDimensions.All, Strings.controls));
            controls.AddChild(new Textbox(new SimpleLocation(580, 75), InheritDimensions.All, Strings.arrows));
            controls.AddChild(new Textbox(new SimpleLocation(626, 150), InheritDimensions.All, Strings.move));
            controls.AddChild(new Textbox(new SimpleLocation(580, 225), InheritDimensions.All, Strings.space));
            controls.AddChild(new Textbox(new SimpleLocation(626, 300), InheritDimensions.All, Strings.specialaction));
            controls.AddChild(new Textbox(new SimpleLocation(580, 375), InheritDimensions.All, Strings.p));
            controls.AddChild(new Textbox(new SimpleLocation(626, 450), InheritDimensions.All, Strings.settingsmenu));
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
            //Changes the sound level (everywhere, not only in the settingsmenu) when the slider is moved
            SoundEffect.MasterVolume = (float)slider.Value / 100;
        }
    }
}
