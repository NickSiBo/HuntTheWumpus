﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

#if DESKTOP
using Tharga.Toolkit.Console;
using Tharga.Toolkit.Console.Command;
using Tharga.Toolkit.Console.Command.Base;
using HuntTheWumpus.SharedCode.GameMap;
using HuntTheWumpus.SharedCode.Helpers;
using HuntTheWumpus.SharedCode.GUI;
using Microsoft.Xna.Framework;

namespace HuntTheWumpus.SharedCode.GameControl
{
    public class GodManager
    {
        public Map Map;
        public MapRenderer MapRenderer;

        public GodManager(Map Map, MapRenderer MapRenderer)
        {
            this.Map = Map;
            this.MapRenderer = MapRenderer;
        }

        public void Initialize()
        {
            RootCommand Root = new RootCommand(Log.Console);

            Root.RegisterCommand(new GoToRoomCommand(Map));
            Root.RegisterCommand(new RoomInfoCommand(Map));
            Root.RegisterCommand(new ListRoomsCommand(Map));
            Root.RegisterCommand(new SetViewCommand(MapRenderer));

            CommandEngine Engine = new CommandEngine(Root);
            Engine.Run(new string[0]);

        }
    }

    class GoToRoomCommand : ActionCommandBase
    {
        private const string Help = "Moves the player to the specified room.";

        Map Map;
        public GoToRoomCommand(Map Map)
            : base("mv", Help)
        {
            this.Map = Map;
        }

        public override async System.Threading.Tasks.Task<bool> InvokeAsync(string paramList)
        {
            int ID = int.Parse(GetParam(paramList, 0));
            return Map.MovePlayerTo(ID);
        }
    }

    class ListRoomsCommand : ActionCommandBase
    {
        private const string Help = "Lists all rooms.";

        Map Map;
        public ListRoomsCommand(Map Map)
            : base("li", Help)
        {
            this.Map = Map;
        }

        public override async System.Threading.Tasks.Task<bool> InvokeAsync(string paramList)
        {
            foreach (Room Room in Map.Cave.getRoomList())
                OutputInformation(Room.ToString().Replace("{", "{{").Replace("}", "}}"));
            return true;
        }
    }

    class SetViewCommand : ActionCommandBase
    {
        private const string Help = "Sets the coordinates or zoom of the map camera to the specified value(s), or resets the camera to auto-follow if no arguments are given.";

        MapRenderer MapRenderer;
        public SetViewCommand(MapRenderer MapRenderer)
            : base("cam", Help)
        {
            this.MapRenderer = MapRenderer;
        }

        public override async System.Threading.Tasks.Task<bool> InvokeAsync(string paramList)
        {
            if(paramList == null || paramList.Length <= 0)
            {
                MapRenderer.OverriddenCameraPosition = null;
                return true;
            }

            if (paramList.Contains(",") || paramList.Contains(" "))
            {
                int[] Coords = paramList.Split(new string[] { " ", "," }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s.Trim())).ToArray();
                MapRenderer.OverriddenCameraPosition = new Vector2(Coords[0], Coords[1]);
            }
            else
                MapRenderer.CameraZoom = float.Parse(paramList.Trim());

            return true;
        }
    }

    class RoomInfoCommand : ActionCommandBase
    {
        private const string Help = "Gets or sets information about the specified room, or the current one if no room is specified.";

        Map Map;
        public RoomInfoCommand(Map Map)
            : base("rinfo", Help)
        {
            this.Map = Map;

        }

        public override async System.Threading.Tasks.Task<bool> InvokeAsync(string paramList)
        {
            string PropertyName = null, NewValue = null;

            int ID;
            if (!int.TryParse(GetParam(paramList, 0), out ID))
                ID = Map.PlayerRoom;

            if (paramList != null)
            {
                var Arguments = ArgumentParser.Parse(paramList);

                PropertyName = ArgumentParser.GetParam(Arguments, "property", "prop");
                NewValue = ArgumentParser.GetParam(Arguments, "value", "val"); ;

            }

            if(PropertyName != null && NewValue == null)
            {
                // Get a value
                OutputInformation(
                    "Room {0} property {1}: {2}",
                    ID,
                    PropertyName,
                    ReflectionUtils.GetPropertyOrField<Room>(Map.Cave.GetRoom(ID), PropertyName));
            }
            else if (PropertyName != null && NewValue != null)
            {
                // Set a value
                ReflectionUtils.SetPropertyOrField<Room>(Map.Cave.GetRoom(ID), PropertyName, NewValue);
            }
            else
            {
                // Give some basic info
                OutputInformation("Room info: {0}", Map.Cave.GetRoom(Map.PlayerRoom));
            }

            return true;
        }
    }


}

#endif