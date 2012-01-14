/* 
 * Sharpcraft
 * Copyright (c) 2012 by Sijmen Schoon and Adam Hellberg.
 * All Rights Reserved.
 */

using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;

using Keys = Microsoft.Xna.Framework.Input.Keys;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;

using Sharpcraft.Steam;
using Sharpcraft.Forms;
using Sharpcraft.Logging;
using Sharpcraft.Networking;
using Sharpcraft.Components.Debug;
using Sharpcraft.Library.Configuration;

namespace Sharpcraft
{
	/// <summary>
	/// Main class of Sharpcraft, this is the game itself.
	/// </summary>
	/// <remarks>Most documentation in this class comes from XNA.</remarks>
	public class Sharpcraft : Game
	{
		/// <summary>
		/// Log object for this class.
		/// </summary>
		private readonly log4net.ILog _log;

		private readonly GameSettings _settings;

		/// <summary>
		/// The graphics device manager.
		/// </summary>
		private GraphicsDeviceManager _graphics;
		/// <summary>
		/// Sprite batch.
		/// </summary>
		private SpriteBatch _spriteBatch;

		private bool _gameMenuOpen;
		private bool _menuToggling;
		private bool _inServer = true;

		private Texture2D _crosshair;
		private SpriteFont _menuFont;

		private User _user;

		/// <summary>
		/// Initializes a new instance of Sharpcraft.
		/// </summary>
		public Sharpcraft(User user)
		{
			_log = LogManager.GetLogger(this);
			_settings = new GameSettings(SharpcraftConstants.GameSettings);
			_user = user;
			_log.Debug("Initializing graphics device.");
			_graphics = new GraphicsDeviceManager(this)
			{
				PreferredBackBufferWidth = 1280,
				PreferredBackBufferHeight = 720
			};
			_log.Debug("Setting content directory.");
			Content.RootDirectory = SharpcraftConstants.ContentDirectory;
			_log.Debug("Creating DebugDisplay...");
			Components.Add(new DebugDisplay(this));
#if DEBUG
			_gameMenuOpen = true;
#endif
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			_log.Debug("Initialize();");
			_log.Info("Sharpcraft is initializing!");
			// TODO: Add your initialization logic here

			base.Initialize();

			/* /!\ Steam hardcore loading action /!\ */
			_log.Info("Loading Steam components...");
			if (SteamManager.Init())
			{
				//SteamManager.FriendList.LoadFriends(); // Should load automatically now
				//Application.EnableVisualStyles();
				_log.Info("Creating Steam GUI.");
				// TODO: Find a way to set the start location of SteamGUI to be next to Game Window.
				var steamGUI = new SteamGUI();
				if (!steamGUI.Visible)
					steamGUI.Show();
				_log.Info("Steam components loaded!");
			}
			else
			{
				_log.Info("Steam not installed or not running, Steam functionality will NOT be available.");
			}

			/* Commented out by Vijfhoek:
			 * Removed code seeing that we will add it somewhere else later.
			_log.Debug("Creating protocol...");
			var protocol = new Protocol("localhost", 25565);

			_log.Debug("Sending handshake packet.");
			protocol.PacketHandshake("Sharpcraft");
			protocol.GetPacket();
			_log.Debug("Sending login request.");
			protocol.PacketLoginRequest(22, "Sharpcraft");
			protocol.GetPacket();
			*/
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			_log.Debug("LoadContent();");
			_log.Info("!!! GAME LOAD !!!");
			// Create a new SpriteBatch, which can be used to draw textures.
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			_crosshair = Content.Load<Texture2D>("crosshair");
			_menuFont = Content.Load<SpriteFont>(SharpcraftConstants.MenuFont);
			_log.Debug("LoadContent(); ## END ##");
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			_log.Info("!!! GAME UNLOAD !!!");
			_log.Debug("UnloadContent();");
			// TODO: Unload any non ContentManager content here
			SteamManager.Close();
			_log.Debug("UnloadContent(); ## END ##");
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
				ToggleGameMenu();

			if (Keyboard.GetState().IsKeyUp(Keys.Escape))
				_menuToggling = false;

			if (!_gameMenuOpen && IsActive)
				Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);
			_spriteBatch.Begin();
			_spriteBatch.Draw(_crosshair, new Vector2(Mouse.GetState().X - 24, Mouse.GetState().Y - 24), Color.White);
			if (_gameMenuOpen)
				_spriteBatch.DrawString(_menuFont, "!!! GAME MENU OPEN !!!", new Vector2((float) GraphicsDevice.Viewport.Width / 2 - 120, (float) GraphicsDevice.Viewport.Height / 2 + 20), Color.Yellow);
			_spriteBatch.End();
			
			base.Draw(gameTime);
		}

		/// <summary>
		/// Event handler for when the game loses focus.
		/// </summary>
		/// <param name="sender">N/A (Not Used) (See XNA Documentation)</param>
		/// <param name="args">N/A (Not Used) (See XNA Documentation)</param>
		/// <remarks>Displays game menu to allow mouse movement when game is minimized/in the background.</remarks>
		protected override void OnDeactivated(object sender, EventArgs args)
		{
			_gameMenuOpen = true;
			base.OnDeactivated(sender, args);
		}

		private void ToggleGameMenu()
		{
			if (!_inServer || _menuToggling)
				return;
			_menuToggling = true;
			_gameMenuOpen = !_gameMenuOpen;
			_log.Debug("Game menu is now " + (_gameMenuOpen ? "open" : "closed"));
		}
	}
}
