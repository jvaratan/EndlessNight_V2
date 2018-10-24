using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpToState : InterfaceAIState
{
    private readonly StatePatternAI m_statePatternAI;

    public JumpToState(StatePatternAI statePatternAI)
    {
        m_statePatternAI = statePatternAI;
    }

    public void UpdateState()
    {

    }

    public void ToEmergeState()
    {
        m_statePatternAI.CurrentState = m_statePatternAI.EmergeState;
    }

    public void ToChasingState()
    {
        m_statePatternAI.CurrentState = m_statePatternAI.ChasingState;
    }

    public void ToAttackState()
    {
        m_statePatternAI.CurrentState = m_statePatternAI.AttackState;
    }

    public void ToJumpToState()
    {
        Debug.Log("Can't transition to same state");
    }

    public void ToExhaustState()
    {
        m_statePatternAI.CurrentState = m_statePatternAI.CurrentState;
    }
}
