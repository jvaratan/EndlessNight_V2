using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : InterfaceAIState
{

    private readonly StatePatternAI m_statePatternAI;

    public AttackState(StatePatternAI statePatternAI)
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
        Debug.Log("Can't transition to same state");
    }

    public void ToJumpToState()
    {
        m_statePatternAI.CurrentState = m_statePatternAI.JumpToState;
    }

    public void ToExhaustState()
    {
        m_statePatternAI.CurrentState = m_statePatternAI.CurrentState;
    }
}
