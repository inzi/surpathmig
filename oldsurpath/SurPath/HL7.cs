using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurPath
{
    public class DonorInfoDetails
    {
        public string SpecimenId { get; set; }
        public string LabSampleId { get; set; }
        public string SsnId { get; set; }
        public string DonorLastName { get; set; }
        public string DonorFirstName { get; set; }
        public string DonorMI { get; set; }
        public string DonorDOB { get; set; }
        public string DonorGender { get; set; }
    }

    public class OBX_Info
    {
        public int Sequence { get; set; }
        public string TestCode { get; set; }
        public string TestName { get; set; }
        public string Result { get; set; }
        public string Status { get; set; }
        public string UnitOfMeasure { get; set; }
        public string ReferenceRange { get; set; }
        public string OrderStatus { get; set; }
    }

    public class OBR_Info
    {
        public int TransmitedOrder { get; set; }
        public string CollectionSiteInfo { get; set; }
        public string SpecimenCollectionDate { get; set; }
        public string SpecimenReceivedDate { get; set; }
        public string CrlClientCode { get; set; }
        public string SpecimenType { get; set; }
        public string SectionHeader { get; set; }
        public string CrlTransmitDate { get; set; }
        public string ServiceSectionId { get; set; }
        public string OrderStatus { get; set; }
        public string ReasonType { get; set; }

        public List<OBX_Info> observatinos = new List<OBX_Info>();
    }
}
