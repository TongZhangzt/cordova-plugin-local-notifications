/**
 *  LocalNotification.cs
 *  Cordova LocalNotification Plugin
 *
 *  Created by Sebastian Katzer (github.com/katzer) on 07/10/2013.
 *  Copyright 2013 Sebastian Katzer. All rights reserved.
 *  GPL v2 licensed
 */

using System;
using System.Linq;

using Microsoft.Phone.Shell;

using WPCordovaClassLib.Cordova;
using WPCordovaClassLib.Cordova.Commands;
using WPCordovaClassLib.Cordova.JSON;

using De.APPPlant.Cordova.Plugin.LocalNotification;

namespace Cordova.Extension.Commands
{
    /// <summary>
    /// Implementes access to application live tiles
    /// http://msdn.microsoft.com/en-us/library/hh202948(v=VS.92).aspx
    /// </summary>
    public class LocalNotification : BaseCommand
    {
        /// <summary>
        /// Sets application live tile
        /// </summary>
        public void add(string jsonArgs)
        {
            string[] args = JsonHelper.Deserialize<string[]>(jsonArgs);
            LocalNotificationOptions options = JsonHelper.Deserialize<LocalNotificationOptions>(args[0]);
            // Application Tile is always the first Tile, even if it is not pinned to Start.
            ShellTile AppTile = ShellTile.ActiveTiles.First();

            if (AppTile != null)
            {
                // Set the properties to update for the Application Tile
                // Empty strings for the text values and URIs will result in the property being cleared.
                FlipTileData TileData = CreateTileData(options);

                // Update the Application Tile
                AppTile.Update(TileData);

                if (!string.IsNullOrEmpty(options.Foreground))
                {
                    string arguments = String.Format("{0}({1})", options.Foreground, options.ID);

                    DispatchCommandResult(new PluginResult(PluginResult.Status.OK, arguments));
                }
            }

            DispatchCommandResult();
        }

        /// <summary>
        /// Clears the application live tile
        /// </summary>
        public void cancel(string jsonArgs)
        {
            cancelAll(jsonArgs);
        }

        /// <summary>
        /// Clears the application live tile
        /// </summary>
        public void cancelAll(string jsonArgs)
        {
            // Application Tile is always the first Tile, even if it is not pinned to Start.
            ShellTile AppTile = ShellTile.ActiveTiles.First();

            if (AppTile != null)
            {
                // Set the properties to update for the Application Tile
                // Empty strings for the text values and URIs will result in the property being cleared.
                FlipTileData TileData = new FlipTileData
                {
                    BackTitle            = "",
                    BackContent          = "",
                    WideBackContent      = "",
                    SmallBackgroundImage = new Uri("appdata:Background.png"),
                    BackgroundImage      = new Uri("appdata:Background.png"),
                    WideBackgroundImage  = new Uri("/Assets/Tiles/FlipCycleTileLarge.png", UriKind.Relative),
                };

                // Update the Application Tile
                AppTile.Update(TileData);
            }

            DispatchCommandResult();
        }

        /// <summary>
        /// Creates tile data
        /// </summary>
        private FlipTileData CreateTileData(LocalNotificationOptions options)
        {
            FlipTileData tile = new FlipTileData();

            // Badge sollte nur gelöscht werden, wenn expliziet eine `0` angegeben wurde
            if (options.Badge != 0)
            {
                tile.Count = options.Badge;
            }

            tile.BackTitle       = options.Title;
            tile.BackContent     = options.ShortMessage;
            tile.WideBackContent = options.Message;

            if (!String.IsNullOrEmpty(options.SmallImage))
            {
                tile.SmallBackgroundImage = new Uri(options.SmallImage, UriKind.RelativeOrAbsolute);
            }

            if (!String.IsNullOrEmpty(options.Image))
            {
                tile.BackgroundImage = new Uri(options.Image, UriKind.RelativeOrAbsolute);
            }

            if (!String.IsNullOrEmpty(options.WideImage))
            {
                tile.WideBackgroundImage = new Uri(options.WideImage, UriKind.RelativeOrAbsolute);
            }

            return tile;
        }
    }
}