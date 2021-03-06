﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

public class GameStateManager : IGameLoopObject
{
    Dictionary<string, IGameLoopObject> gameStates;
    IGameLoopObject currentGameState;
    string previousGameState;

    public GameStateManager()
    {
        gameStates = new Dictionary<string, IGameLoopObject>();
        currentGameState = null;
    }

    public void AddGameState(string name, IGameLoopObject state)
    {
        gameStates[name] = state;
    }

    public IGameLoopObject GetGameState(string name)
    {
        return gameStates[name];
    }

    public void SwitchTo(string name)
    {
        name = name.Replace("MeesGame.", "");

        if (gameStates.ContainsKey(name))
        {
            currentGameState = gameStates[name];
        }
        else
        {
            throw new KeyNotFoundException("Could not find game state: " + name);
        }
    }

    public IGameLoopObject CurrentGameState
    {
        get
        {
            return currentGameState;
        }
    }

    public string PreviousGameState
    {
        get { return previousGameState; }
        set { previousGameState = value; }
    }

    public void HandleInput(InputHelper inputHelper)
    {
        //If P is pressed, and you're not currently in the settings menu, the settings menu will be opened, 
        //and the gamestate where you came from will be stored for potential use
        if (inputHelper.KeyPressed(Keys.P))
        {
            if (currentGameState.ToString() == "MeesGame.SettingsMenuState")
            {
                SwitchTo(previousGameState);
            }
            else
            {
                previousGameState = currentGameState.ToString();
                SwitchTo("SettingsMenuState");
            }
        }
        if (inputHelper.KeyPressed(Keys.Escape))
        {
            SwitchTo("TitleMenuState");
        }
        if (currentGameState != null)
        {
            currentGameState.HandleInput(inputHelper);
        }
    }

    public void Update(GameTime gameTime)
    {
        if (currentGameState != null)
        {
            currentGameState.Update(gameTime);
        }
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (currentGameState != null)
        {
            currentGameState.Draw(gameTime, spriteBatch);
        }
    }

    public void Reset()
    {
        if (currentGameState != null)
        {
            currentGameState.Reset();
        }
    }
}
