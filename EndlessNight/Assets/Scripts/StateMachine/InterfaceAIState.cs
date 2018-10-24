using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InterfaceAIState{

    void UpdateState();

    void ToEmergeState();

    void ToChasingState();

    void ToAttackState();

    void ToJumpToState();

    void ToExhaustState();
}
