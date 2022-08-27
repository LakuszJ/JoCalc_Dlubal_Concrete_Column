using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JCDlubalCSVForcesGeneratorLibrary
{
    public class InternalForcesMemberSingleItem
    {
        /// <summary>
        ///  Sppecial JC_ number, used for futere functionality. This number is read from "Comments" field at RFEM. It is for replace lack of "select" functionality.
        /// </summary>
        public int? speccyficationJC { get; internal set; }

        /// <summary>
        /// Start node of member (not always bottom node), readed from CSV. Values are present only at the first position at List.
        /// </summary>
        public int? startNode { get; internal set; }

        /// <summary>
        /// End node of member (not always top node), readed from CSV. Values are present only at the first position at List
        /// </summary>
        public int? endNode { get; internal set; }

        /// <summary>
        /// No. of current member (only for check).
        /// </summary>
        public int memberControlNo { get; internal set; }

        /// <summary>
        /// Location along of member. Mesured from startNode. 
        /// </summary>
        public decimal location { get; internal set; }

        // Traditional Forces at [N] or [Nm]

        public int n { get; internal set; }
        public int vy { get; internal set; }
        public int vz { get; internal set; }
        public int mt { get; internal set; }
        public int my { get; internal set; }
        public int mz { get; internal set; }
    }
}
