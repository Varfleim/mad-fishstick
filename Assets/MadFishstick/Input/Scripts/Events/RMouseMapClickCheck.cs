
using Leopotam.EcsLite;

namespace MF.Input
{
    public readonly struct RMouseMapClickCheck
    {
        public RMouseMapClickCheck(
            EcsPackedEntity currentProvincePE, 
            bool leftMouseButtonClick, bool rightMouseButtonClick)
        {
            this.currentProvincePE = currentProvincePE;
            
            this.leftMouseButtonClick = leftMouseButtonClick;
            this.rightMouseButtonClick = rightMouseButtonClick;
        }

        public readonly EcsPackedEntity currentProvincePE;

        public readonly bool leftMouseButtonClick;
        public readonly bool rightMouseButtonClick;
    }
}
