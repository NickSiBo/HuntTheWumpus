﻿using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace HuntTheWumpus.SharedCode
{
    /// <summary>
    /// The cave object generates a cave system and places objects inside it (player, arrows, gold, etc)
    /// Includes methods to get locations of game objects in cave
    /// </summary>
    public class Cave
    {
        /// <summary>
        /// contains generated cave (list of rooms)
        /// </summary>
        private Dictionary<int, Room> cave = new Dictionary<int, Room>();
        /// <summary>
        /// returns current cave on request
        /// </summary>
        /// <returns>current cave</returns>
        public List<Room> getCave()
        {
            return cave.Values.ToList<Room>();
        }

        public Room getRoom(int id)
        {
           return cave[id];
        }
        public void addRoom(int id, int[] connections)
        {
            this.cave[id] = new Room(){
                roomId = id,
                adjacentRooms = connections,
            };
        }
    }
    /// <summary>
    /// Class which represents one room which is part of the cave system
    /// </summary>
    public class Room
    {
        /// <summary>
        /// room's location in cave
        /// </summary>
        public int roomId;
        /// <summary>
        /// how many doors the room has (1-3)
        /// </summary>
        public int doors;
        /// <summary>
        /// how much gold the room contains (gold >= 0)
        /// </summary>
        public int gold;
        /// <summary>
        /// how many arrows the room contains (arrows >= 0)
        /// </summary>
        public int arrows;
        /// <summary>
        /// true if room contains bats, false if not
        /// </summary>
        public bool bats;
        /// <summary>
        /// true if room contains a pit, false if not
        /// </summary>
        public bool pit;
        /// <summary>
        /// what other rooms this room is connected to
        /// </summary>
        public int[] adjacentRooms;
    }
}
