using System.Collections.Generic;

namespace InteractionSystem
{
    public static class InteractionUtility
    {
        public static Dictionary<int, List<InteractionSet>> CreateInteractionMap()
        {
            Dictionary<int, List<InteractionSet>> interactionMap = new Dictionary<int, List<InteractionSet>>();
            
            List<InteractionSet> listFifteen = new List<InteractionSet>
            {
                new InteractionSet(InteractionType.Attach, InteractionType.UseOn),
                new InteractionSet(InteractionType.Attach, InteractionType.Activate),
                new InteractionSet(InteractionType.Attach, InteractionType.Use)
            };
            interactionMap[15] = listFifteen;
            
            List<InteractionSet> listFourteen = new List<InteractionSet>
            {
                new InteractionSet(InteractionType.Attach, InteractionType.Use)
            };
            interactionMap[14] = listFourteen;

            List<InteractionSet> listThirteen = new List<InteractionSet>()
            {
                new InteractionSet(InteractionType.Attach, InteractionType.Activate)
            };
            interactionMap[13] = listThirteen;

            List<InteractionSet> listTwelve = new List<InteractionSet>()
            {
                new InteractionSet(InteractionType.Attach, InteractionType.None)
            };
            interactionMap[12] = listTwelve;

            List<InteractionSet> listEleven = new List<InteractionSet>()
            {
                new InteractionSet(InteractionType.None, InteractionType.UseOn),
                new InteractionSet(InteractionType.None, InteractionType.Activate),
                new InteractionSet(InteractionType.None, InteractionType.Use)
            };
            interactionMap[11] = listEleven;
            interactionMap[7] = listEleven;
            interactionMap[3] = listEleven;

            List<InteractionSet> listTen = new List<InteractionSet>()
            {
                new InteractionSet(InteractionType.Drop, InteractionType.Use)
            };
            interactionMap[10] = listTen;

            List<InteractionSet> listNine = new List<InteractionSet>()
            {
                new InteractionSet(InteractionType.None, InteractionType.Activate)
            };
            interactionMap[9] = listNine;
            interactionMap[1] = listNine;

            List<InteractionSet> listEight = new List<InteractionSet>()
            {
                new InteractionSet(InteractionType.Drop, InteractionType.None)
            };
            interactionMap[8] = listEight;

            List<InteractionSet> listSix = new List<InteractionSet>()
            {
                new InteractionSet(InteractionType.None, InteractionType.Use)
            };
            interactionMap[6] = listSix;
            interactionMap[2] = listSix;

            List<InteractionSet> listFive = new List<InteractionSet>()
            {
                new InteractionSet(InteractionType.Pickup, InteractionType.Activate)
            };
            interactionMap[5] = listFive;

            List<InteractionSet> listFour = new List<InteractionSet>()
            {
                new InteractionSet(InteractionType.Pickup, InteractionType.None)
            };
            interactionMap[4] = listFour;

            List<InteractionSet> listZero = new List<InteractionSet>()
            {
                new InteractionSet(InteractionType.None, InteractionType.None)
            };
            interactionMap[0] = listZero;
            
            return interactionMap;
        }

        
        
        public static int InteractionInteger(bool p, bool q, bool r, bool s)
        {
            int pval = p ? 1 : 0;
            int qval = q ? 2 : 0;
            int rval = r ? 4 : 0;
            int sval = s ? 8 : 0;
            return  pval + qval + rval + sval;
        }
    }
    
    public enum InteractionType
    {
        None,
        Pickup,
        Drop,
        Attach,
        Activate,
        Use,
        UseOn
    }

    public struct InteractableInfo
    {
        public string Name;
        public string Prompt;
        public InteractionType Type;
    }

    public struct InteractionSet
    {
        public InteractionSet(InteractionType first, InteractionType second)
        {
            Primary = first;
            Secondary = second;
        }
        
        public InteractionType Primary;
        public InteractionType Secondary;
    }
}
