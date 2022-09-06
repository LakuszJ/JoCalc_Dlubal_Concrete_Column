using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCDlubalCSVForcesGeneratorLibrary
{
    public static class ForcesModyficator
    {
        public static void ModifyNodeOrder(Dictionary<int, Dictionary<int, List<InternalForcesMemberSingleItem>>> dicMCF, Dictionary<int, MyMember> members)
        {
            foreach (var dicMCF_member in dicMCF)
            {
                foreach (var dicMCF_co in dicMCF_member.Value)
                {
                    if (dicMCF_co.Value[0].startNode != members[dicMCF_member.Key].bottomNode)
                    {
                        dicMCF_co.Value.Reverse();
                        dicMCF_co.Value.First().startNode = members[dicMCF_member.Key].bottomNode;
                        dicMCF_co.Value.First().endNode = members[dicMCF_member.Key].topNode;
                        dicMCF_co.Value.Last().startNode = null;
                        dicMCF_co.Value.Last().endNode = null;
                    }
                }
            }
        }

    }
}
