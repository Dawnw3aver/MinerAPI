﻿namespace MinerAPI.APIObjects
{
    public class GameTurnRequest
    {
        public Guid Game_id { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
    }
}
