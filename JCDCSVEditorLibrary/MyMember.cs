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
        public int memberlNo { get; internal set; }

        /// <summary>
        /// Start node of member (not always bottom node), readed from MODEL.
        /// </summary>
        public int startNode { get; internal set; }

        /// <summary>
        /// End node of member (not always bottom node), readed from MODEL.
        /// </summary>
        public int endNode { get; internal set; }

        /// <summary>
        /// No. of top node for member. Top node mean that it is in model above te second node.
        /// </summary>
        public int topNode { get; internal set; }

        /// <summary>
        /// No. of bottom node for member. Bottom node mean that it is in model below te second node.
        /// </summary>
        public int bottomNode { get; internal set; }

        /// <summary>
        /// Vector of silngle member element. For future func.
        /// </summary>
        public vector_3d memberVector { get; internal set; }

        /// <summary>
        /// Lenght of member. ( [m] )
        /// </summary>
        public decimal memberLength { get; internal set; }

        /// <summary>
        /// Informaction about section type at ennum. ex. 
        /// </summary>
        public section_parametrization_type membersSectionType { get; internal set; }


        /// <summary>
        /// String for extract dimensions of cross section. Method of extract is dependign from section_parametrization_type (Method not implemeneted)
        /// </summary>
        public string memberSectionDim { get; internal set; }

        /// <summary>
        /// Rotation specifaied at section deifnition.
        /// </summary>
        public double sectionRotation { get; internal set; }

        /// <summary>
        /// Rotation specifaied at memebr deifnition.
        /// </summary>
        public double memberRotation { get; internal set; }

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
        public int memberMaterial { get; internal set; } // for change

        public MyMember()
        {
            memberVector = new vector_3d();
        }

    }
}
