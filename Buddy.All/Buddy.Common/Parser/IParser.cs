﻿using Buddy.Common.Structures;

namespace Buddy.Common.Parser
{
    internal interface IParser
    {
        IGraph ParseCrsGraph(string filename);

        ISymmetricGraph ParseSymmetricCrsGraph(string filename);

        ISocialGraph Parse(string filename);
    }
}