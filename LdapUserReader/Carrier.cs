using System;
using System.Collections.Generic;
using System.Text;

namespace LdapUserReader
{
    public class Carrier
    {
        public long ObjectId { get; set; }

        public int NetworkId { get; set; }

        public string Ctype { get; set; }

        public int CarrierType { get; set; }

        public long ProfileObjectId { get; set; }

        public DateTime? ArrivalDateTime { get; set; }

        public DateTime? LeaveDateTime { get; set; }

        public DateTime? ExtraFromDateTime { get; set; }

        public DateTime? ExtraToDateTime { get; set; }

        public int? PresenceTime { get; set; }

        public int? NrMovements { get; set; }

        public long? UnitObjectId { get; set; }

        public string UseInBioFilter { get; set; }

        public string Transient { get; set; }

        public string IsReadOnly { get; set; }

        public string ExternalId { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? ModificationDate { get; set; }

        public DateTime? RemovalDate { get; set; }

        public int RemovalStatus { get; set; }

        public string Title { get; set; }

        public string Initials { get; set; }

        public string MiddleName { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }

        public int? Gender { get; set; }

        public string PersonnelNr { get; set; }

        public string PhoneNumber { get; set; }

        public string MobilePhoneNumber { get; set; }

        public string CanBeUser { get; set; }

        public string CanBeGuard { get; set; }

        public string Language { get; set; }

        public string Unit { get; set; }

        public string Company { get; set; }

        public DateTime? PermitFromDatetime { get; set; }

        public DateTime? PermitUntilDateTime { get; set; }

        public int? VendorObjectId { get; set; }

        public string Identification { get; set; }

        public int? Weight { get; set; }

        public long? OwnerObjectId { get; set; }

        public string LicenceNumber { get; set; }

        public string CarNumber { get; set; }

        public long? DepartmentObjectId { get; set; }

        public string UniqueId { get; set; }
        public string Value { get; set; }

    }
}
