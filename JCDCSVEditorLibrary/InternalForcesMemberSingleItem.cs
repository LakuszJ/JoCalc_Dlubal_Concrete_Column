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
        public int? speccyficationJC { get; set; }

        /// <summary>
        /// Start node of member (not always bottom node), readed from CSV. Values are present only at the first position at List.
        /// </summary>
        public int? startNode { get; set; }

        /// <summary>
        /// End node of member (not always top node), readed from CSV. Values are present only at the first position at List
        /// </summary>
        public int? endNode { get; set; }

        /// <summary>
        /// No. of current member (only for check).
        /// </summary>
        public int memberControlNo { get; set; }

        /// <summary>
        /// Location along of member. Mesured from startNode. 
        /// </summary>
        public decimal location { get; set; }

        // Traditional Forces at [N] or [Nm]

        public int n { get; set; }
        public int vy { get; set; }
        public int vz { get; set; }
        public int mt { get; set; }
        public int my { get; set; }
        public int mz { get; set; }
    }
}
