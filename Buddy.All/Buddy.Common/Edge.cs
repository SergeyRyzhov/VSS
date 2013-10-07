﻿namespace Buddy.Common
{
    public struct Edge
    {
        public uint Id { get; set; }

        public Vertex U { get; set; }

        public Vertex V { get; set; }

        public uint Weight { get; set; }
    }
}