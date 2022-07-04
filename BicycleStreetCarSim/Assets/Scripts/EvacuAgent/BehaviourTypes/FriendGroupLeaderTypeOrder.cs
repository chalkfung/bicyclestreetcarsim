using System;
using System.Collections.Generic;

public class FriendGroupLeaderTypeOrder : BehaviourTypeOrder
{
    private void Awake()
    {
        behaviourTypes = new List<BehaviourType>()
        {
            new GenericPathCreationBehaviourType(),
            new GenericEnterLeaveBuildingBehaviourType(),
            new WaitForFollowersBehaviourType(),
            new WaitAtDestinationBehaviourType(),
            new FriendGroupBoidBehaviourType(),
            new GenericNoNewBehaviourType()
        };
    }

    public override List<BehaviourType> GetBehaviourTypes() => behaviourTypes;

    private class GenericEnterLeaveBuildingBehaviourType : BehaviourType
    {
        private readonly float behaviourStrategyChanceToUse = 1f;

        public override Type GetBehaviourStrategyClass<BehaviourStrategy>() => typeof(GenericEnterLeaveBuildingBehaviour);
        public override float GetBehaviourStrategyChance() => behaviourStrategyChanceToUse;
    }

    private class GenericPathCreationBehaviourType : BehaviourType
    {
        private readonly float behaviourStrategyChanceToUse = 1f;

        public override Type GetBehaviourStrategyClass<BehaviourStrategy>() => typeof(GenericPathCreationBehaviour);
        public override float GetBehaviourStrategyChance() => behaviourStrategyChanceToUse;
    }

    private class GenericNoNewBehaviourType : BehaviourType
    {
        private readonly float behaviourStrategyChanceToUse = 1f;

        public override Type GetBehaviourStrategyClass<BehaviourStrategy>() => typeof(GenericNoNewBehaviour);
        public override float GetBehaviourStrategyChance() => behaviourStrategyChanceToUse;
    }

    private class WaitForFollowersBehaviourType : BehaviourType
    {
        private readonly float behaviourStrategyChanceToUse = 1f;

        public override Type GetBehaviourStrategyClass<BehaviourStrategy>() => typeof(WaitForFollowersBehaviour);
        public override float GetBehaviourStrategyChance() => behaviourStrategyChanceToUse;
    }

    private class FriendGroupBoidBehaviourType : BehaviourType
    {
        private readonly float behaviourStrategyChanceToUse = 1f;

        public override Type GetBehaviourStrategyClass<BehaviourStrategy>() => typeof(FriendGroupBoidBehaviour);
        public override float GetBehaviourStrategyChance() => behaviourStrategyChanceToUse;
    }

    private class WaitAtDestinationBehaviourType : BehaviourType
    {
        private readonly float behaviourStrategyChanceToUse = 1f;

        public override Type GetBehaviourStrategyClass<BehaviourStrategy>() => typeof(WaitAtDestinationBehaviour);
        public override float GetBehaviourStrategyChance() => behaviourStrategyChanceToUse;
    }
}