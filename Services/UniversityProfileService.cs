using Regis.Helpers;
using Regis.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Regis.Services
{
    public class UniversityProfileService
    {
        private readonly DBHelper db = new DBHelper();

        //=========================================================
        // Get University Profile (singleton — always 1 row)
        //=========================================================

        public UniversityProfileModel GetUniversityProfile()
        {
            UniversityProfileModel model = null;

            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_UniversityProfiles", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GET");

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    model = new UniversityProfileModel
                    {
                        UniversityId = Convert.ToInt32(dr["UniversityId"]),

                        UniversityName = dr["UniversityName"].ToString(),
                        ShortName = dr["ShortName"].ToString(),
                        UniversityCode = dr["UniversityCode"].ToString(),
                        UniversityTypeId = dr["UniversityTypeId"] != DBNull.Value ? Convert.ToInt32(dr["UniversityTypeId"]) : (int?)null,
                        EstablishedYear = dr["EstablishedYear"] != DBNull.Value ? Convert.ToInt32(dr["EstablishedYear"]) : (int?)null,
                        Accreditation = dr["Accreditation"].ToString(),
                        Website = dr["Website"].ToString(),
                        Email = dr["Email"].ToString(),
                        Phone = dr["Phone"].ToString(),
                        TaxNumber = dr["TaxNumber"].ToString(),
                        AffiliatedUniversity = dr["AffiliatedUniversity"].ToString(),
                        IsActive = dr["IsActive"] != DBNull.Value && Convert.ToBoolean(dr["IsActive"]),

                        Motto = dr["Motto"].ToString(),
                        Vision = dr["Vision"].ToString(),
                        Mission = dr["Mission"].ToString(),
                        Description = dr["Description"].ToString(),
                        AboutUniversity = dr["AboutUniversity"].ToString(),
                        RegistrationNumber = dr["RegistrationNumber"].ToString(),
                        UGCApprovalNumber = dr["UGCApprovalNumber"].ToString(),
                        AICTEApproval = dr["AICTEApproval"].ToString(),
                        NAACGrade = dr["NAACGrade"].ToString(),
                        NIRFRank = dr["NIRFRank"].ToString(),
                        ISOCertification = dr["ISOCertification"].ToString(),
                        OfficeNumber = dr["OfficeNumber"].ToString(),
                        FaxNumber = dr["FaxNumber"].ToString(),
                        DetailsEmail = dr["DetailsEmail"].ToString(),
                        DetailsWebsite = dr["DetailsWebsite"].ToString(),
                        Facebook = dr["Facebook"].ToString(),
                        LinkedIn = dr["LinkedIn"].ToString(),
                        Instagram = dr["Instagram"].ToString(),
                        Twitter = dr["Twitter"].ToString(),

                        LogoMainPath = dr["LogoMainPath"].ToString(),
                        LogoDarkPath = dr["LogoDarkPath"].ToString(),
                        LogoLightPath = dr["LogoLightPath"].ToString(),
                        LogoSidebarPath = dr["LogoSidebarPath"].ToString(),
                        FaviconPath = dr["FaviconPath"].ToString(),

                        Country = dr["Country"].ToString(),
                        State = dr["State"].ToString(),
                        District = dr["District"].ToString(),
                        City = dr["City"].ToString(),
                        Area = dr["Area"].ToString(),
                        Street = dr["Street"].ToString(),
                        BuildingNumber = dr["BuildingNumber"].ToString(),
                        PostalCode = dr["PostalCode"].ToString(),
                        Latitude = dr["Latitude"].ToString(),
                        Longitude = dr["Longitude"].ToString(),

                        ContOffice = dr["ContOffice"].ToString(),
                        ContMobile = dr["ContMobile"].ToString(),
                        ContAlternate = dr["ContAlternate"].ToString(),
                        ContAdmissionHelpline = dr["ContAdmissionHelpline"].ToString(),
                        ContRegistrarOffice = dr["ContRegistrarOffice"].ToString(),
                        ContEmail = dr["ContEmail"].ToString(),
                        ContSupportEmail = dr["ContSupportEmail"].ToString(),
                        ContWebsite = dr["ContWebsite"].ToString()
                    };
                }
            }

            return model; // null hoga agar abhi tak kabhi save nahi hua
        }

        //=========================================================
        // Save University Profile (insert if none exists, else update)
        //=========================================================

        public bool SaveUniversityProfile(UniversityProfileModel m)
        {
            try
            {
                using (SqlConnection con = db.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_UniversityProfiles", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Flag", "SAVE");

                    cmd.Parameters.AddWithValue("@UniversityName", (object)m.UniversityName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ShortName", (object)m.ShortName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@UniversityCode", (object)m.UniversityCode ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@UniversityTypeId", (object)m.UniversityTypeId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@EstablishedYear", (object)m.EstablishedYear ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Accreditation", (object)m.Accreditation ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Website", (object)m.Website ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", (object)m.Email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Phone", (object)m.Phone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TaxNumber", (object)m.TaxNumber ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@AffiliatedUniversity", (object)m.AffiliatedUniversity ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsActive", m.IsActive);

                    cmd.Parameters.AddWithValue("@Motto", (object)m.Motto ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Vision", (object)m.Vision ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Mission", (object)m.Mission ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Description", (object)m.Description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@AboutUniversity", (object)m.AboutUniversity ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@RegistrationNumber", (object)m.RegistrationNumber ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@UGCApprovalNumber", (object)m.UGCApprovalNumber ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@AICTEApproval", (object)m.AICTEApproval ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@NAACGrade", (object)m.NAACGrade ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@NIRFRank", (object)m.NIRFRank ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ISOCertification", (object)m.ISOCertification ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@OfficeNumber", (object)m.OfficeNumber ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FaxNumber", (object)m.FaxNumber ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DetailsEmail", (object)m.DetailsEmail ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DetailsWebsite", (object)m.DetailsWebsite ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Facebook", (object)m.Facebook ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@LinkedIn", (object)m.LinkedIn ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Instagram", (object)m.Instagram ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Twitter", (object)m.Twitter ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@LogoMainPath", (object)m.LogoMainPath ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@LogoDarkPath", (object)m.LogoDarkPath ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@LogoLightPath", (object)m.LogoLightPath ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@LogoSidebarPath", (object)m.LogoSidebarPath ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FaviconPath", (object)m.FaviconPath ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@Country", (object)m.Country ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@State", (object)m.State ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@District", (object)m.District ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@City", (object)m.City ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Area", (object)m.Area ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Street", (object)m.Street ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@BuildingNumber", (object)m.BuildingNumber ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PostalCode", (object)m.PostalCode ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Latitude", (object)m.Latitude ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Longitude", (object)m.Longitude ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@ContOffice", (object)m.ContOffice ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ContMobile", (object)m.ContMobile ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ContAlternate", (object)m.ContAlternate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ContAdmissionHelpline", (object)m.ContAdmissionHelpline ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ContRegistrarOffice", (object)m.ContRegistrarOffice ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ContEmail", (object)m.ContEmail ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ContSupportEmail", (object)m.ContSupportEmail ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ContWebsite", (object)m.ContWebsite ?? DBNull.Value);

                    con.Open();
                    cmd.ExecuteNonQuery();   // return value ignore — SET NOCOUNT ON ki wajah se hamesha -1 aata hai
                    return true;             // yahan tak koi exception nahi aayi, matlab save ho gaya
                }
            }
            catch (Exception)
            {
                return false;   // sirf real DB error pe hi false
            }
        }
    }
    
}