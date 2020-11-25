using System;

namespace FootBallTournament.Tests
{
    public class TestPriorityAttribute : Attribute
{
    public TestPriorityAttribute(int priority)
    {
        Priority = priority;
    }

    public int Priority { get; private set; }
    
        
    }
}