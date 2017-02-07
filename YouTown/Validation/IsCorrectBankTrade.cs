using System;
using System.Collections.Generic;
using System.Linq;

namespace YouTown.Validation
{
    public class IsCorrectBankTrade : ValidatorBase<Tuple<IPortList, IResourceList, IResourceList>>
    {
        public override IValidationResult Validate(Tuple<IPortList, IResourceList, IResourceList> tuple, string playerName = null)
        {
            var ports = tuple.Item1;
            var offeredTobank = tuple.Item2;
            var requestedFromBank = tuple.Item3;
            int gold = ports.AmountGold(offeredTobank);
            int goldNeeded = requestedFromBank.Count;
            if (gold != goldNeeded)
            {
                return new Invalid($"player {playerName} offered {offeredTobank} which yields {gold} gold. Requested {requestedFromBank} needs {goldNeeded}.");
            }
            // Check if player offers too many resources, e.g. 5 resources where a FourToOnePort is applicable
            var tooMuch = new List<ResourceType>();
            foreach (var resourceType in offeredTobank.ResourceTypes)
            {
                var resourcesOfType = offeredTobank.OfType(resourceType);
                if (!resourcesOfType.Any())
                {
                    continue;
                }
                var port = ports.BestPortForResourceType(resourceType);
                var remainder = resourcesOfType.Count % port.InAmount;
                bool offersTooMuch = remainder > 0;
                if (offersTooMuch)
                {
                    tooMuch.Add(resourceType);
                }
            }
            if (tooMuch.Any())
            {
                var resourceTypesString = string.Join(",", tooMuch);
                return new Invalid($"Resource types {resourceTypesString} do not have multiple of port requirement");
            }
            return Validator.Valid;
        }
    }
}
