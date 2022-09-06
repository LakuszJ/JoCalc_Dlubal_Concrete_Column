using Dlubal.WS.Rfem6.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCDlubalCSVForcesGeneratorLibrary
{
    public class MyMember
    {
        /// <summary>
        /// No. of member.
        /// </summary>
        public int memberlNo { get; set; }

        /// <summary>
        /// Start node of member (not always bottom node), readed from MODEL.
        /// </summary>
        public int startNode { get; set; }

        /// <summary>
        /// End node of member (not always bottom node), readed from MODEL.
        /// </summary>
        public int endNode { get; set; }

        /// <summary>
        /// No. of top node for member. Top node mean that it is in model above te second node.
        /// </summary>
        public int topNode { get; set; }

        /// <summary>
        /// No. of bottom node for member. Bottom node mean that it is in model below te second node.
        /// </summary>
        public int bottomNode { get; set; }


        public double memberVectorX { get; set; }
        public double memberVectorY { get; set; }
        public double memberVectorZ { get; set; }

        /// <summary>
        /// Lenght of member. ( [m] )
        /// </summary>
        public decimal memberLength { get;  set; }

        /// <summary>
        /// Informaction about section type at ennum. ex. 
        /// </summary>
        public section_parametrization_type membersSectionType { get; set; }


        /// <summary>
        /// String for extract dimensions of cross section. Method of extract is dependign from section_parametrization_type (Method not implemeneted)
        /// </summary>
        public string memberSectionDim { get; set; }

        /// <summary>
        /// Rotation specifaied at section deifnition.
        /// </summary>
        public double sectionRotation { get; set; }

        /// <summary>
        /// Rotation specifaied at memebr deifnition.
        /// </summary>
        public double memberRotation { get; set; }

        private double _finalRotation;
        /// <summary>
        /// Sum of roations section and member
        /// </summary>
        public double finalRotation
        {
            get { return sectionRotation + memberRotation; }
        }

        /// <summary>
        /// No of Material
        /// </summary>
        public int memberMaterial { get; set; } // for change
    }
}
